using Microsoft.AspNetCore.Authentication.JwtBearer;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using QueueControlServer.Data.Services;
using QueueControlServer.Interfaces;
using QueueControlServer.Models;
using QueueControlServer.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace QueueControlServer
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
            services.Configure<MyAppSettings>(Configuration.GetSection(MyAppSettings.SectionName));
            services.AddOptions(); // 

            services.AddControllersWithViews().AddRazorRuntimeCompilation();
            services.AddSignalR();
        /*    services.AddControllers().AddJsonOptions(x =>
              x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve);*/
            services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.PropertyNamingPolicy = null;
            });

            services.AddDbContext<ApplicationDBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            // services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true).AddEntityFrameworkStores<ApplicationDBContext>();

            services.AddIdentity<User, Role>()                               // HACK Здесь очень мутное подключение сервисов индефити , пересмотреть позже, возможны траблы 
                .AddEntityFrameworkStores<ApplicationDBContext>()
                .AddDefaultTokenProviders();


            services.Configure<CookiePolicyOptions>(options =>
            {
            options.CheckConsentNeeded = context => true;
            options.MinimumSameSitePolicy = Microsoft.AspNetCore.Http.SameSiteMode.None;

            });

            services.AddMvc().SetCompatibilityVersion(Microsoft.AspNetCore.Mvc.CompatibilityVersion.Version_2_1);

            services.AddAuthentication()
                .AddCookie(cfg => cfg.SlidingExpiration = true)
                .AddJwtBearer(cfg =>
                {
                    cfg.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidIssuer = MVCJwtTokens.Issuer,
                        ValidAudience = MVCJwtTokens.Audience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(MVCJwtTokens.Key)),
                        RoleClaimType = "role-abc"

                    };

                }
                );
            // services.AddScoped<IUserClaimsPrincipalFactory<User>, AppClaimsPrincipalFactory>();


            // adding Authentication
           /*services.AddAuthentication(options =>
            {
               
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
              .AddJwtBearer(options =>
              {
                  options.SaveToken = true;
                  options.RequireHttpsMetadata = false;
                  options.TokenValidationParameters = new TokenValidationParameters()
                  {
                      ValidateIssuer = true,
                      ValidateAudience = true,
                      ValidAudience = AuthOptions.AUDIENCE,
                      ValidIssuer = AuthOptions.ISSUER,
                      IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey()
                  }; 
              });*/


                  // adding JWT 




                 /* services.AddAuthorization(opts =>
            {
                opts.AddPolicy("OnlyForLondon", policy =>
                {
                    policy.RequireClaim(ClaimTypes.Locality, "Лондон", "London");
                });
                opts.AddPolicy("OnlyForMicrosoft", policy =>
                {
                    policy.RequireClaim("company", "Microsoft");
                });

                

            });*/




            services.AddControllersWithViews();

            services.AddTransient<IGetUserClaim, UserClaimService>();  //UNDONE регистрация сервиса идентификации клэймов


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
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

           /* app.UseFileServer(new FileServerOptions()     // поддержку каталога node_modules            
            {
                FileProvider = new PhysicalFileProvider(
                    Path.Combine(env.ContentRootPath, "node_modules")
                ),
                RequestPath = "/node_modules",
                EnableDirectoryBrowsing = false
            });*/

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
                endpoints.MapHub<SignalServer>("/signalServer");
            });
        }
    }
}
