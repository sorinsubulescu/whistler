using System;
using System.IO;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;

namespace apigateway
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers().SetCompatibilityVersion(CompatibilityVersion.Version_3_0);
            services.AddControllers().AddNewtonsoftJson(opt =>
            {
                opt.SerializerSettings.NullValueHandling = NullValueHandling.Ignore;
            });

            services.AddSingleton<IDatabase, MongoDatabase>();
            services.AddSingleton<IPostProvider, PostProvider>();
            services.AddSingleton<IUserProvider, UserProvider>();
            services.AddSingleton<IRefreshTokenProvider, RefreshTokenProvider>();
            services.AddSingleton<IAuthTokenHelper, AuthTokenHelper>();
            services.AddSingleton<IImageWriter, ImageWriter>();
            services.AddSingleton<IQueryFactory, QueryFactory>();
            services.AddSingleton<ICommandFactory, CommandFactory>();

            ConfigureAuthentication(services);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }


            app.UseCors(builder => builder.AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin());

            //app.UseHttpsRedirection();
            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), "wwwroot"));
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(Directory.GetCurrentDirectory(), "wwwroot")),
                RequestPath = "/whstore"
            });

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private void ConfigureAuthentication(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();
            var key = Encoding.ASCII.GetBytes(appSettings.Secret);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x =>
            {
                x.Events = new JwtBearerEvents
                {
                    OnTokenValidated = async context =>
                    {
                        var userProvider = context.HttpContext.RequestServices.GetRequiredService<IUserProvider>();

                        var userId = context.Principal.FindFirstValue(ClaimTypes.NameIdentifier);
                        var user = await userProvider.GetById(userId).ConfigureAwait(false);

                        if (!IsUserValid(context.Principal, user))
                            context.Fail("Unauthorized");
                    },
                    OnAuthenticationFailed = context =>
                    {
                        if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
                            context.Response.Headers.Add("Token-Expired", "true");
                        return Task.CompletedTask;
                    }
                };
                x.RequireHttpsMetadata = false;
                x.SaveToken = true;
                x.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });
        }

        private static bool IsUserValid(ClaimsPrincipal claimsPrincipal, User user)
        {
            var userName = claimsPrincipal.FindFirstValue(ClaimTypes.Name);
            var email = claimsPrincipal.FindFirstValue(ClaimTypes.Email);

            return user != null && Equals(user.UserName, userName) && Equals(user.Email, email);
        }
    }
}