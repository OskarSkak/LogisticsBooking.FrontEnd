using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.DataServices;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Logging;
using Microsoft.IdentityModel.Tokens;

namespace LogisticsBooking.FrontEnd
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
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                // #TODO 
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            services.AddHsts(options =>
            {
                options.Preload = true;
                options.IncludeSubDomains = true;
                options.MaxAge = TimeSpan.FromDays(60);
                options.ExcludedHosts.Add("identity.logistictechnologies.eu/");
                options.ExcludedHosts.Add("identity.logistictechnologies.eu/");
            });

            services.AddHttpsRedirection(options =>
            {
                options.RedirectStatusCode = StatusCodes.Status307TemporaryRedirect;
                options.HttpsPort = 5001;
            });
            services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.KnownProxies.Add(IPAddress.Parse("127.0.0.1"));
                options.KnownProxies.Add(IPAddress.Parse("206.189.120.107"));
                options.KnownProxies.Add(IPAddress.Parse("138.68.134.10"));
                options.KnownProxies.Add(IPAddress.Parse("5.186.119.6"));
            });
            
            // Show logs error Identity
            IdentityModelEventSource.ShowPII = true;
            
            // clear the mapping of claims
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";

                })
                .AddCookie("Cookies")

                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = "https://identity.logistictechnologies.eu";
                    options.RequireHttpsMetadata = false;
                    options.ClientSecret = "secret";
                    options.ClientId = "LogisticBooking";
                    options.SaveTokens = true;

                    options.ResponseType = "code id_token";
                    options.Scope.Add("address");
                    options.Scope.Add("openid");
                    options.Scope.Add("profile");
                    options.Scope.Add("roles");
                    options.Scope.Add("logisticbookingapi");
                    options.SignInScheme = "Cookies";
                    options.GetClaimsFromUserInfoEndpoint = true;
                    options.ClaimActions.MapCustomJson("role", jobj => jobj["role"].ToString());
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RoleClaimType = "role"
                    };


                });

            
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession();
            services.AddMemoryCache();
            

            //Add DI�s below
            services.AddTransient<IBookingDataService, BookingDataService>();
            services.AddTransient<ITransporterDataService, TransporterDataService>();
            services.AddTransient<ISupplierDataService, SupplierDataService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            
            
            var fordwardedHeaderOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            fordwardedHeaderOptions.KnownNetworks.Clear();
            fordwardedHeaderOptions.KnownProxies.Clear();

            app.UseForwardedHeaders(fordwardedHeaderOptions);

            app.UseSession();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            
            app.UseMvc();
        }
    }
}
