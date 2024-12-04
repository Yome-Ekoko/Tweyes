using AutoMapper;
using TweyesBackend.Core.Contract;
using TweyesBackend.Core.DTO.Request;
using TweyesBackend.Core.DTO.Response;
using TweyesBackend.Core.Exceptions;
using TweyesBackend.Core.Extension;
using TweyesBackend.Domain.Common;
using TweyesBackend.Domain.Entities;
using TweyesBackend.Domain.Enum;
using TweyesBackend.Domain.QueryParameters;
using TweyesBackend.Domain.Settings;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Web;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Crypto.Prng;
using System.Text.RegularExpressions;
using TweyesBackend.Core.Helpers;
using TweyesBackend.Core.Contract.Repository;
using Microsoft.Extensions.Configuration;
using ApiException = TweyesBackend.Core.Exceptions.ApiException;


namespace TweyesBackend.Core.Implementation
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ILogger<UserService> _logger;
        private readonly JWTSettings _jwtSettings;
        private readonly UserManager<T_User> _userManager;
        private readonly SignInManager<T_User> _signInManager;
        private readonly RoleManager<T_Role> _roleManager;
        private readonly IAppSessionService _appSession;
        private readonly INotificationService _notificationService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ExternalApiOptions _externalApiOptions;
        private readonly IImageService _imageService;
        private readonly IOtpRepository _otpRepository;
        private readonly string _defaultImageUrl;
        private readonly IConfiguration _configuration;



        public UserService(IMapper mapper,
            ILogger<UserService> logger,
            IOptions<JWTSettings> jwtSettings,
            UserManager<T_User> userManager,
            SignInManager<T_User> signInManager,
            RoleManager<T_Role> roleManager,
            IAppSessionService appSession,
            INotificationService notificationService,
            IHttpContextAccessor httpContextAccessor,
            IOptions<ExternalApiOptions> externalApiOptions,
            IImageService imageService,IOtpRepository otpRepository, IConfiguration configuration)
        {
            _mapper = mapper;
            _logger = logger;
            _jwtSettings = jwtSettings.Value;
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
            _appSession = appSession;
            _notificationService = notificationService;
            _httpContextAccessor = httpContextAccessor;
            _externalApiOptions = externalApiOptions.Value;
            _imageService = imageService;
            _otpRepository = otpRepository;
            _defaultImageUrl = configuration["CloudinarySetting:DefaultImageUrl"];
            _configuration= configuration;


        }

        public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request, CancellationToken cancellationToken)
        {
            var decodedUsername = string.Empty;
            var decodedPassword = string.Empty;
            try
            {
                decodedUsername = CoreHelpers.Base64DecodeString(request.Username);
                decodedPassword = CoreHelpers.Base64DecodeString(request.Password);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occured while decoding the username and password.");
                throw new ApiException($"Invalid Credentials.");
            }

            // Check for the username
            T_User user = await _userManager.Users
                .Where(x => x.NormalizedUserName == decodedUsername.ToUpper())
                .FirstOrDefaultAsync(cancellationToken) ?? throw new ApiException("No Accounts Registered.");

            if (user.Status != UserStatus.Active)
            {
                throw new ApiException($"Inactive account.");
            }

            // Verify the username and password
            SignInResult result = await _signInManager.CheckPasswordSignInAsync(user, decodedPassword, true);
            if (!result.Succeeded)
            {
                if (result.IsLockedOut)
                {
                    throw new ApiException($"This user has been locked. Kindly contact the administrator.");
                }
                throw new ApiException("Invalid Credentials.");
            }
            var roles = await _userManager.GetRolesAsync(user);

            JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user, roles, cancellationToken);
            AuthenticationResponse response = _mapper.Map<T_User, AuthenticationResponse>(user);

            response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
            response.ExpiresIn = _jwtSettings.DurationInMinutes * 60;
            response.ExpiryDate = DateTime.Now.AddSeconds(_jwtSettings.DurationInMinutes * 60);
            response.Roles = roles.ToList();

            user.IsLoggedIn = true;
            user.LastLoginTime = DateTime.Now;
            await _userManager.UpdateAsync(user);

            return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
        }

        private async Task<JwtSecurityToken> GenerateJWToken(T_User user, IList<string> roles, CancellationToken cancellationToken)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);

            var roleClaims = new List<Claim>();

            for (int i = 0; i < roles.Count; i++)
            {
                roleClaims.Add(new Claim("roles", roles[i]));
            }

            DateTime utcNow = DateTime.UtcNow;
            string ipAddress = IpHelper.GetIpAddress();
            string sessionKey = Guid.NewGuid().ToString();
            await _appSession.CreateSession(sessionKey, user.UserName);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim("userId", user.Id),
                new Claim("firstname", user.FirstName),
                new Claim("emailAddress", user.Email),
                new Claim("username", user.UserName),
                new Claim("ip", ipAddress),
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        public async Task<Response<string>> LogoutAsync()
        {
            var username = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "username")?.Value;

            if (string.IsNullOrEmpty(username))
            {
                return new Response<string>("Already logged out", (int)HttpStatusCode.OK, true);
            }

            _appSession.DeleteSession(username);

            // [TODO] Handle situations where the JWT token is expired
            T_User user = await _userManager.FindByNameAsync(username) ?? throw new ApiException("User not found.");
            user.IsLoggedIn = false;
            await _userManager.UpdateAsync(user);
            return new Response<string>("Successfully logged out", (int)HttpStatusCode.OK, true);
        }

        public async Task<PagedResponse<List<UserResponse>>> GetUsersAsync(UserQueryParameters queryParameters, CancellationToken cancellationToken)
        {
            IQueryable<T_User> pagedData = _userManager.Users;

            string? query = queryParameters.Query;
            string? role = queryParameters.Role;
            UserStatus? status = queryParameters.Status;

            // Check if there is a query and apply it
            if (!string.IsNullOrEmpty(query))
            {
                pagedData = pagedData.Where(x => x.Id.ToLower().Contains(query.ToLower())
                   || x.UserName.ToLower().Contains(query.ToLower())
                   || x.Email.ToLower().Contains(query.ToLower())
                   || x.FirstName.ToLower().Contains(query.ToLower()));
            }

            if (!string.IsNullOrEmpty(role))
            {
                pagedData = pagedData.Where(x => x.UserRoles.Any(x => x.Role!.Name == role));
            }

            // Check the status passed in the query parameters and if available use it to filter the result
            if (status.HasValue)
            {
                pagedData = pagedData.Where(x => x.Status == status.Value);
            }

            List<T_User> userList = await pagedData
                .OrderByDescending(x => x.CreatedAt)
                .Skip((queryParameters.PageNumber - 1) * queryParameters.PageSize)
                .Take(queryParameters.PageSize)
                .ToListAsync(cancellationToken);

            List<UserResponse> response = _mapper.Map<List<T_User>, List<UserResponse>>(userList);

            int totalRecords = await pagedData.CountAsync(cancellationToken);

            foreach (var userResponse in response)
            {
                var user = userList.First(x => x.Id == userResponse.Id);
                userResponse.Roles = await _userManager.GetRolesAsync(user);
            }

            return new PagedResponse<List<UserResponse>>(response, queryParameters.PageNumber, queryParameters.PageSize, totalRecords, $"Successfully retrieved users");
        }

        public async Task<Response<UserResponse>> GetUserById(string id, CancellationToken cancellationToken)
        {
            T_User userData = await _userManager.Users
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync(cancellationToken) ?? throw new ApiException($"No user found for User ID - {id}.");

            UserResponse response = _mapper.Map<T_User, UserResponse>(userData);

            return new Response<UserResponse>(response, $"Successfully retrieved user details for user with Id - {id}");
        }

        public async Task<Response<string>> AddUserAsync(AddUserRequest request, CancellationToken cancellationToken)
        {
            var userWithSameUserName = await _userManager.FindByNameAsync(request.UserName);
            if (userWithSameUserName != null)
            {
                throw new ApiException($"Username '{request.UserName}' is already registered.");
            }

            // Check if the role exists
            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                throw new ApiException($"Invalid role specified.");
            }

            string username = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "username")?.Value ?? throw new ApiException("Username not found.");
            string email = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "emailAddress")?.Value ?? throw new ApiException("Email not found.");

            T_User newUser = _mapper.Map<T_User>(request);
            newUser.CreatedAt = DateTime.Now;
            newUser.UpdatedAt = DateTime.Now;
            newUser.Status = UserStatus.Active;

            var result = await _userManager.CreateAsync(newUser);

            if (!result.Succeeded)
            {
                throw new ApiException($"{result.Errors.FirstOrDefault()?.Description}");
            }

            IdentityResult roleResult = await _userManager.AddToRoleAsync(newUser, request.Role);

            if (!roleResult.Succeeded)
            {
                // Roll back user creation and throw an error
                await _userManager.DeleteAsync(newUser);
                throw new ApiException($"An error occured while adding the user to the role");
            }

            string token = await _userManager.GeneratePasswordResetTokenAsync(newUser);

            var resetUserRequest = new
            {
                UserName = newUser.UserName!,
                Token = token
            };
            IDictionary<string, string?> param = resetUserRequest.ToDictionary();

            Uri url = new(QueryHelpers.AddQueryString(_externalApiOptions.PasswordResetUrl, param));

            string userName = newUser.UserName!;
            string firstName = newUser.FirstName;
            string userEmail = newUser.Email!;

            await _notificationService.SendPasswordResetToken(userName, url.ToString(), firstName, userEmail);

            return new Response<string>(newUser.Id, message: $"Successfully registered user with username - {request.UserName}");
        }

        public async Task<Response<string>> EditUserAsync(EditUserRequest request, CancellationToken cancellationToken)
        {
            T_User user = await _userManager.FindByNameAsync(request.UserName) ?? throw new ApiException("Username could not be found.");

            // Check if the new role exists
            if (request.Role is not null && !await _roleManager.RoleExistsAsync(request.Role))
            {
                throw new ApiException($"This role doesn't exists. Please check your roles and try again");
            }

            string username = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "username")?.Value ?? throw new ApiException("Username not found.");
            string email = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "emailAddress")?.Value ?? throw new ApiException("Email not found.");

            user.FirstName = string.IsNullOrEmpty(request.Name) ? user.FirstName : request.Name;
            user.Email = string.IsNullOrEmpty(request.Email) ? user.Email : request.Email;
            user.UpdatedAt = DateTime.Now;

            var updateResult = await _userManager.UpdateAsync(user);

            if (!updateResult.Succeeded)
            {
                throw new ApiException(updateResult.Errors.FirstOrDefault()?.Description ?? "An error occured while updating users.");
            }

            if (request.Role is not null)
            {
                IList<string> existingRoles = await _userManager.GetRolesAsync(user);

                if (!existingRoles.Contains(request.Role))
                {
                    // If there are existing roles then delete them
                    if (existingRoles.Count > 0)
                    {
                        IdentityResult roleResult = await _userManager.RemoveFromRolesAsync(user, existingRoles);

                        if (!roleResult.Succeeded)
                        {
                            throw new ApiException(roleResult.Errors.FirstOrDefault()?.Description ?? "An error occured while removing existing roles.");
                        }
                    }

                    IdentityResult addRoleResult = await _userManager.AddToRoleAsync(user, request.Role);

                    if (!addRoleResult.Succeeded)
                    {
                        throw new ApiException(addRoleResult.Errors.FirstOrDefault()?.Description ?? "An error occured while adding new role.");
                    }
                }
            }

            return new Response<string>(user.Id, message: $"Successfully edited user.");
        }

        public async Task<Response<string>> DeleteUserAsync(DeleteUserRequest request, CancellationToken cancellationToken)
        {
            T_User user = await _userManager.FindByNameAsync(request.UserName) ?? throw new ApiException($"The user does not exist.");

            // Work on the request logging
            string username = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "username")?.Value ?? throw new ApiException("Username not found.");
            string email = _httpContextAccessor.HttpContext?.User.Claims.FirstOrDefault(x => x.Type == "emailAddress")?.Value ?? throw new ApiException("Email not found.");

            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                throw new ApiException(result.Errors.FirstOrDefault()?.Description ?? "An error occured.");
            }

            return new Response<string>(user.Id, message: $"Successfully deleted the user.");
        }

        public async Task<Response<string>> ResetUserAsync(ResetUserRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                // This is a security measure to prevent a bad actor from knowing the list of users on the platform.
                return new Response<string>("", "Reset user successful.");
            }

            string userName = user.UserName!;
            string userFirstName = user.FirstName;
            string userEmail = user.Email!;
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);

            var resetUserRequest = new
            {
                UserName = userName,
                Token = token
            };
            IDictionary<string, string?> param = resetUserRequest.ToDictionary();

            Uri url = new(QueryHelpers.AddQueryString(_externalApiOptions.PasswordResetUrl, param));

            await _notificationService.SendPasswordResetToken(userName, url.ToString(), userFirstName, userEmail);

            return new Response<string>("", "Reset user successful.");
        }

        public async Task<Response<string>> ResetUserLockoutAsync(ResetUserRequest request)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null)
            {
                throw new ApiException($"Username '{request.UserName}' could not be found.");
            }

            var resetResult = await _userManager.SetLockoutEndDateAsync(user, DateTimeOffset.Now);

            if (!resetResult.Succeeded)
            {
                _logger.LogError("An error occured while resetting lockout");
                _logger.LogError(CoreHelpers.ClassToJsonData(resetResult.Errors));
                throw new ApiException($"An error occured while resetting lockout.");
            }

            return new Response<string>(user.Id, message: "Successfully reset user lockout.");
        }

        public async Task<Response<string>> ChangePasswordWithToken(ChangePasswordRequest request)
        {
            T_User user = await _userManager.FindByNameAsync(request.UserName) ?? throw new ApiException($"No user found with username '{request.UserName}'.");

            IdentityResult resetResponse = await _userManager.ResetPasswordAsync(user, request.Token, request.Password);

            if (!resetResponse.Succeeded)
            {
                throw new ApiException(resetResponse.Errors.FirstOrDefault()?.Description ?? "An error occured.");
            }

            return new Response<string>(user.Id, message: "Successfully changed password.");
        }
        public async Task<Response<string>> RegisterAsync(RegisterRequest request)
        {
            if (!await IsEmailAllowedAsync(request.Email))
            {
                throw new ApiException("Please enter a valid email address.");
            }

            if (!Regex.IsMatch(request.PhoneNumber, @"^\(?([0-9]{3})\)?[-. ]?([0-9]{4})[-. ]?([0-9]{4})$"))
            {
                throw new ApiException("Please enter a valid PhoneNumber.");
            }

            var existingUser = await _userManager.FindByEmailAsync(request.Email);

            if (existingUser != null)
            {
                throw new ApiException("User already exists.");
            }

            var newUser = new T_User
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                ContactAddress = request.ContactAddress,
                State = request.State,
                ImageUrl = request.Image,
                UserName = request.Email,
                CreatedAt = DateTime.Now
            };

            var response = await _userManager.CreateAsync(newUser);

            if (!response.Succeeded)
            {
                throw new ApiException("Unable to save user.");
            }

            if (!await _roleManager.RoleExistsAsync(request.Role))
            {
                await _userManager.DeleteAsync(newUser); // Rollback user creation
                throw new ApiException("This role doesn't exist. Please check your roles and try again.");
            }

            var roleResult = await _userManager.AddToRoleAsync(newUser, request.Role);

            if (!roleResult.Succeeded)
            {
                await _userManager.DeleteAsync(newUser); // Rollback user creation
                throw new ApiException("An error occurred while adding the user to the role.");
            }

            await SendOtp(new OtpRequest { email = newUser.Email });

            return new Response<string>(newUser.Id, "User Registered. Please confirm your account with the link sent to your email.");
        }


        private async Task<bool> IsEmailAllowedAsync(string email)
        {
            bool isEmail = Regex.IsMatch(email, @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z", RegexOptions.IgnoreCase);

            if (isEmail == false)
            {
                return false;
            }
            return true;
        }
        public async Task<Response<string>> SendOtp(OtpRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.email);
            if (user == null)
            {
                throw new ApiException("User not found.");
            }

            string otp = GenerateOtp();
            if (string.IsNullOrEmpty(otp))
            {
                throw new ApiException("Failed to generate OTP.");
            }

            await _otpRepository.StoreOtpAsync(user.Email, otp);
            ;

            var otpRequest = new SendOtpRequest
            {
                email = user.Email,
                Otp = otp,
                firstname = user.FirstName
                // url = verificationUri
            };

            await _notificationService.SendOtp(otpRequest);

            return new Response<string>($"Opt sent {otpRequest.email}", message: "Succeeded");
        }

        private string GenerateOtp()
        {
            Random random = new Random();
            int otp = random.Next(100000, 999999);
            return otp.ToString();
        }

        public async Task<Response<string>> VerifyOtp(VerifyOtpRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException("User not found.");
            }

            bool isValid = await _otpRepository.VerifyOtpAsync(request.Email, request.Otp);
            if (isValid)
            {
                return new Response<string>("OTP verified successfully!", message: "Succeeded");
            }
            else
            {
                throw new ApiException("Invalid or expired OTP.");
            }
        }
        public async Task<Response<UserResponse>> CreatePassword(CreatePasswordRequest request)
        {
            var validPassword = Regex.IsMatch(request.Password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{6,}$");
            if (!validPassword)
            {
                throw new ApiException("Password must have at least one uppercase letter, one lowercase letter, one number, one special character, and be at least 6 characters long.");
            }

            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                throw new ApiException("User not found.");
            }

            if (await _userManager.HasPasswordAsync(user))
            {
                throw new ApiException("User already has a password.");
            }

            var result = await _userManager.AddPasswordAsync(user, request.Password);
            if (!result.Succeeded)
            {
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                throw new ApiException($"Failed to add password: {errors}");
            }

            var welcomeRequest = new WelcomeRequest
            {
                Email = user.Email,
                FirstName = user.FirstName,
            };
            var userResponse = _mapper.Map<UserResponse>(user);

            // string jobId = BackgroundJob.Enqueue(() => _notificationService.SendWelcomeMail(welcomeRequest));
            await _notificationService.SendWelcomeMail(welcomeRequest);

            return new Response<UserResponse>(userResponse, message: "Success");
        }
    }
}
