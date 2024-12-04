using TweyesBackend.Core.Contract;
using TweyesBackend.Core.DTO.Request;
using TweyesBackend.Core.DTO.Response;
using TweyesBackend.Domain.Common;
using TweyesBackend.Domain.QueryParameters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TweyesBackend.Controllers
{
    [Route("api/[controller]")]
   // [Authorize(Roles = "Administrator")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService accountService)
        {
            _userService = accountService;
        }
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<Response<AuthenticationResponse>>> AuthenticateAsync(AuthenticationRequest request)
        {
            return Ok(await _userService.AuthenticateAsync(request, HttpContext.RequestAborted));
        }
        [AllowAnonymous]
        [HttpPost("logout")]
        public async Task<ActionResult<Response<string>>> Logout()
        {
            return Ok(await _userService.LogoutAsync());
        }
        [HttpGet("getUsers")]
        public async Task<ActionResult<PagedResponse<List<UserResponse>>>> GetUsers([FromQuery] UserQueryParameters queryParameters)
        {
            return Ok(await _userService.GetUsersAsync(queryParameters, HttpContext.RequestAborted));
        }
        [HttpGet("getUser/{id}")]
        public async Task<ActionResult<Response<UserResponse>>> GetUserById(string id)
        {
            return Ok(await _userService.GetUserById(id, HttpContext.RequestAborted));
        }
        //[HttpPost("addUser")]
        //public async Task<ActionResult<Response<string>>> AddUser(AddUserRequest request)
        //{
        //    return Ok(await _userService.AddUserAsync(request, HttpContext.RequestAborted));
        //}
        //[HttpPost("editUser")]
        //public async Task<ActionResult<Response<string>>> EditUser([FromBody] EditUserRequest request)
        //{
        //    return Ok(await _userService.EditUserAsync(request, HttpContext.RequestAborted));
        //}
        //[HttpPost("deleteUser")]
        //public async Task<ActionResult<Response<string>>> DeleteUser([FromBody] DeleteUserRequest request)
        //{
        //    return Ok(await _userService.DeleteUserAsync(request, HttpContext.RequestAborted));
        //}
        [AllowAnonymous]
        [HttpPost("resetUser")]
        public async Task<ActionResult<Response<string>>> ResetUser([FromBody] ResetUserRequest request)
        {
            return Ok(await _userService.ResetUserAsync(request));
        }
        //[HttpPost("resetUserLockout")]
        //public async Task<ActionResult<Response<string>>> ResetUserLockout([FromBody] ResetUserRequest request)
        //{
        //    return Ok(await _userService.ResetUserLockoutAsync(request));
        //}
        [AllowAnonymous]
        [HttpPost("changePasswordWithToken")]
        public async Task<ActionResult<Response<string>>> PasswordReset([FromBody] ChangePasswordRequest request)
        {
            return Ok(await _userService.ChangePasswordWithToken(request));
        }
        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<ActionResult<Response<string>>> Register([FromBody]RegisterRequest request)
        {
            return Ok(await _userService.RegisterAsync(request));
        }
        [AllowAnonymous]
        [HttpPost("sendOtp")]
        public async Task<ActionResult<Response<string>>> SendOtp([FromBody] OtpRequest request)
        {
            return Ok(await _userService.SendOtp(request));
        }
        [AllowAnonymous]
        [HttpPost("verifyOtp")]
        public async Task<ActionResult<Response<string>>> VerifyOtp([FromBody] VerifyOtpRequest request)
        {
            return Ok(await _userService.VerifyOtp(request));
        }
        [AllowAnonymous]
        [HttpPost("createPassword")]
        public async Task<ActionResult<Response<string>>> CreatePassword([FromBody] CreatePasswordRequest request)
        {
            return Ok(await _userService.CreatePassword(request));
        }
    }
}