using AspNetCoreRateLimit;
using TweyesBackend.Core.BackgroundServices;
using TweyesBackend.Core.Contract;
using TweyesBackend.Core.Contract.Repository;
using TweyesBackend.Core.Implementation;
using TweyesBackend.Core.Repository;
using TweyesBackend.Domain.Entities;
using TweyesBackend.Domain.Settings;
using TweyesBackend.Infrastructure.Configs;
using TweyesBackend.Persistence;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Polly;
using Polly.Extensions.Http;
using System.Net;
using System.Text;
using IClientFactory = TweyesBackend.Core.Contract.IClientFactory;
using TweyesBackend.Infrastructure.Interceptor;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure;

namespace TweyesBackend.Infrastructure.Extension
{
    public static class ConfigureServiceContainer
    {
        public static void AddSqlServerDbContext(this IServiceCollection serviceCollection,
       IConfiguration configuration, IConfigurationRoot configRoot)
        {
            var connectionString = $"{configuration
                    .GetConnectionString("ConnectionString") ?? configRoot["ConnectionString:ConnectionString"]}";

            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlServer(connectionString, sqlOptions =>
                {
                    sqlOptions.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                    // SQL Server-specific options can be configured here
                    // sqlOptions.EnableRetryOnFailure(); // Example of enabling retry on failure
                });
                // options.AddInterceptors(serviceCollection.BuildServiceProvider().GetRequiredService<DBLongQueryLogger>());
            });
        }


        public static void AddTransientServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IAPIImplementation, APIImplementation>();
            serviceCollection.AddTransient<IClientFactory, ClientFactory>();
            serviceCollection.AddTransient<IAppSessionService, AppSessionService>();
            serviceCollection.AddTransient<INotificationService, NotificationService>();
            serviceCollection.AddTransient<IOtpRepository, OtpRepository>();
            serviceCollection.AddTransient<ITutorRepository, TutorRepository>();
            serviceCollection.AddTransient<IScheduleRepository, ScheduleRepository>();

           
        }

        public static void AddSingletonServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IQueueManager, QueueManager>();
        }

        public static void AddScopedServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            serviceCollection.AddScoped<IUserService, UserService>();
            serviceCollection.AddScoped<ITemplateService, TemplateService>();
            serviceCollection.AddScoped<IImageService, ImageService>();
            serviceCollection.AddScoped<ITutorService, TutorService>();
            serviceCollection.AddScoped<IScheduleService, ScheduleService>();

        }

        public static void AddRepositoryServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
          
        }

        public static void AddJwtIdentityService(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddIdentity<T_User, T_Role>(opt =>
            {
                opt.Lockout.AllowedForNewUsers = true;
                opt.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5000);
                opt.Lockout.MaxFailedAccessAttempts = 5;
            })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(o =>
                {
                    o.RequireHttpsMetadata = false;
                    o.SaveToken = false;
                    o.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero,
                        ValidIssuer = configuration["JWTSettings:Issuer"],
                        ValidAudience = configuration["JWTSettings:Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWTSettings:Key"]))
                    };
                    o.Events = new JwtBearerEvents()
                    {
                        OnChallenge = context =>
                        {
                            context.HandleResponse();
                            var code = HttpStatusCode.Unauthorized;
                            context.Response.StatusCode = (int)code;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new Domain.Common.Response<string>("You are not Authorized", (int)code));
                            return context.Response.WriteAsync(result);
                        },
                        OnForbidden = context =>
                        {
                            var code = HttpStatusCode.Forbidden;
                            context.Response.StatusCode = (int)code;
                            context.Response.ContentType = "application/json";
                            var result = JsonConvert.SerializeObject(new Domain.Common.Response<string>("You are not authorized to access this resource", (int)code));
                            return context.Response.WriteAsync(result);
                        },
                    };
                });
        }

        public static void AddCustomOptions(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddOptions<ExternalApiOptions>().BindConfiguration("ExternalApiOptions");
            serviceCollection.AddOptions<JWTSettings>().BindConfiguration("JWTSettings");
            serviceCollection.AddOptions<AdminOptions>().BindConfiguration("AdminOptions");
            serviceCollection.AddOptions<DatabaseOptions>().BindConfiguration("DatabaseOptions");
            serviceCollection.AddOptions<ExternalApiOptions>().BindConfiguration("ExternalApiOptions");
            serviceCollection.AddOptions<JWTSettings>().BindConfiguration("JWTSettings");
            serviceCollection.AddOptions<AdminOptions>().BindConfiguration("AdminOptions");
            serviceCollection.AddOptions<AzureStorageOptions>().BindConfiguration("AzureStorage");
        }
    

        public static void AddCustomHostedServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddHostedService<EmailBackgroundService>();
        }

        public static void AddCustomAutoMapper(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(typeof(MappingProfileConfiguration));
        }

        public static void AddValidation(this IServiceCollection serviceCollection)
        {
            serviceCollection.Configure<ApiBehaviorOptions>(opt => { opt.SuppressModelStateInvalidFilter = false; });
        }

        public static void AddSwaggerOpenAPI(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(setupAction =>
            {

                setupAction.SwaggerDoc(
                    "OpenAPISpecification",
                    new OpenApiInfo()
                    {
                        Title = "TweyesBackend WebAPI",
                        Version = "1",
                        Description = "API Details for TweyesBackend Admin",
                        Contact = new OpenApiContact()
                        {
                            Email = "info@theTweyesBackend.com",
                            Name = "The Coronation Portal",
                            Url = new Uri(" https://TweyesBackend.com")
                        },
                        License = new OpenApiLicense()
                        {
                            Name = "UNLICENSED"
                        }
                    });

                setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = $"Input your Bearer token in this format - Bearer token to access this API",
                });
                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        }, new List<string>()
                    },
                });
            });
        }

        public static void AddCustomControllers(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddEndpointsApiExplorer();

            serviceCollection.AddControllersWithViews()
                .AddNewtonsoftJson(ops =>
                {
                    ops.SerializerSettings.NullValueHandling = NullValueHandling.Include;
                    ops.SerializerSettings.DefaultValueHandling = DefaultValueHandling.Include;
                });
            serviceCollection.AddRazorPages();

            serviceCollection.Configure<ApiBehaviorOptions>(apiBehaviorOptions =>
                apiBehaviorOptions.InvalidModelStateResponseFactory = actionContext =>
                {
                    var logger = actionContext.HttpContext.RequestServices.GetRequiredService<ILogger<BadRequestObjectResult>>();
                    IEnumerable<string> errorList = actionContext.ModelState.Values.SelectMany(x => x.Errors).Select(x => x.ErrorMessage);
                    logger.LogError("Bad Request");
                    logger.LogError(string.Join(",", errorList));
                    return new BadRequestObjectResult(new Domain.Common.Response<IEnumerable<string>>("TweyesBackend Validation Error", 400, errorList));
                });
        }

        public static void AddHTTPPolicies(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            var policyConfigs = new HttpClientPolicyConfiguration();
            configuration.Bind("HttpClientPolicies", policyConfigs);

            var timeoutPolicy = Policy.TimeoutAsync<HttpResponseMessage>(TimeSpan.FromSeconds(policyConfigs.RetryTimeoutInSeconds));

            var retryPolicy = HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(r => r.StatusCode == HttpStatusCode.NotFound)
                .WaitAndRetryAsync(policyConfigs.RetryCount, _ => TimeSpan.FromMilliseconds(policyConfigs.RetryDelayInMs));

            var circuitBreakerPolicy = HttpPolicyExtensions
               .HandleTransientHttpError()
               .CircuitBreakerAsync(policyConfigs.MaxAttemptBeforeBreak, TimeSpan.FromSeconds(policyConfigs.BreakDurationInSeconds));

            var noOpPolicy = Policy.NoOpAsync().AsAsyncPolicy<HttpResponseMessage>();

            HttpClientHandler handler = new HttpClientHandler()
            {
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            };

            //Register a Typed Instance of HttpClientFactory for a Protected Resource
            //More info see: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/http-requests?view=aspnetcore-3.0

            serviceCollection.AddHttpClient<IClientFactory, ClientFactory>()
                .ConfigurePrimaryHttpMessageHandler(_ =>
                {
                    var handler = new HttpClientHandler
                    {
                        ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
                    };
                    return handler;
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(policyConfigs.HandlerTimeoutInMinutes))
                .AddPolicyHandler(request => request.Method == HttpMethod.Get ? retryPolicy : noOpPolicy)
                .AddPolicyHandler(timeoutPolicy)
                .AddPolicyHandler(circuitBreakerPolicy);
        }

        public static void AddRequestRateLimiter(this IServiceCollection serviceCollection, IConfiguration configuration)
        {
            // needed to load configuration from appsettings.json
            serviceCollection.AddOptions();
            // needed to store rate limit counters and ip rules
            serviceCollection.AddMemoryCache();

            //load general configuration from appsettings.json
            serviceCollection.Configure<IpRateLimitOptions>(configuration.GetSection("IpRateLimiting"));

            // inject counter and rules stores
            serviceCollection.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            serviceCollection.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();

            // configuration (resolvers, counter key builders)
            serviceCollection.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }
    }
}
