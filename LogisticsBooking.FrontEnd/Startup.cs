using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using DocumentFormat.OpenXml.Presentation;
using LogisticsBooking.FrontEnd.Acquaintance;
using LogisticsBooking.FrontEnd.ConfigHelpers;
using LogisticsBooking.FrontEnd.DataServices;
using LogisticsBooking.FrontEnd.DataServices.Utilities;
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
using Exceptionless;
using LogisticsBooking.FrontEnd.AutoMapper;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Localization;
using Swashbuckle.AspNetCore.Swagger;

using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.Elasticsearch;
using ILogger = Serilog.ILogger;

namespace LogisticsBooking.FrontEnd
{

     
     
      public class Startup
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfiguration _config;


        public Startup(IHostingEnvironment env, IConfiguration config)
        {
            _env = env;
            _config = config;

        }

        

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            
            services.Configure<BackendServerUrlConfiguration>(
                _config.GetSection(nameof(BackendServerUrlConfiguration)));
            services.Configure<IdentityServerConfiguration>(_config.GetSection(nameof(IdentityServerConfiguration)));
            
            Log.Logger = new LoggerConfiguration()
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri("http://localhost:9200"))
                {
                    AutoRegisterTemplate = true
                })
                
                .CreateLogger();
            
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                // #TODO 
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });
            
            
           
            services.AddCors(o => o.AddPolicy("MyPolicy", builder =>
            {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            }));

            

            // Show logs error Identity
            IdentityModelEventSource.ShowPII = true;
            
            // clear the mapping of claims
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();

            var identityServerConfig = _config.GetSection(nameof(IdentityServerConfiguration))
                .Get<IdentityServerConfiguration>();
            
            services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Management.API Auth", new OAuth2Scheme
                {
                    Type = "oauth2",
                    Flow = "application",
                    Description = "This API uses the Management.API login Oauth2 Client Credentials flow",
                    TokenUrl = "https://qa-auth-management-identity.azurewebsite.net/connect/token",
                    Scopes = new Dictionary<string, string> { { "scope.fullacces", "Acces to all api-endpoints" } }
                });
                c.SwaggerDoc("v1", new Info { Title = "Management Backend", Version = "v1", Description = "Management API for use with prior agreement" });
            });
            
            services.AddAutoMapper(new Assembly[] { typeof(AutoMapperProfile).GetTypeInfo().Assembly });
            
            Console.WriteLine($"{identityServerConfig.IdentityServerUrl}");
            services.AddAuthentication(options =>
                {
                    options.DefaultScheme = "Cookies";
                    options.DefaultChallengeScheme = "oidc";
                    

                })
                .AddCookie("Cookies")

                .AddOpenIdConnect("oidc", options =>
                {
                    options.Authority = $"{identityServerConfig.IdentityServerUrl}";
                    
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
                    /*
                    options.ClaimActions.MapCustomJson("role", jobj => jobj["role"].ToString());
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        RoleClaimType = "role"
                    };
*/

                });

            
            services.Configure<CookiePolicyOptions>(options =>
            {
                // This lambda determines whether user consent for non-essential cookies is needed for a given request.
                options.CheckConsentNeeded = context => false;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            services.AddSession();
            services.AddMemoryCache();
            
            services.AddAntiforgery(options =>
            {
                options.Cookie.SameSite = SameSiteMode.None;
            });

            //Add DI�s below
            services.AddTransient<IBookingDataService, BookingDataService>();
            services.AddTransient<ITransporterDataService, TransporterDataService>();
            services.AddTransient<ISupplierDataService, SupplierDataService>();
            services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            services.AddTransient<IUtilBookingDataService, UtilBookingDataService>();
            services.AddTransient<IOrderDataService, OrderDataService>();
            services.AddTransient<IScheduleDataService, ScheduleDataService>();
            services.AddTransient<IIntervalDataService , IntervalDataService>();
            services.AddTransient<IUserUtility, UserUtility>();
            services.AddTransient<IMasterScheduleDataService, MasterShceduleDataService>();
            services.AddTransient<IDeletedBookingDataService, DeletedBookingDataService>();
          
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options =>
                {
                    options.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore;
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env , ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                // Usefull for enter a site that does not exists. 
                app.UseStatusCodePagesWithRedirects("/Error");
                
            }
            else
            {
                //ON exception trown (Whilst not in dev mode) redirect to this page:
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            
            /*var defaultDateCulture = "-FR";
            var ci = new CultureInfo(defaultDateCulture);
            ci.NumberFormat.NumberDecimalSeparator = ".";
            ci.NumberFormat.CurrencyDecimalSeparator = ".";

// Configure the Localization middleware
            app.UseRequestLocalization(new RequestLocalizationOptions
            {
                DefaultRequestCulture = new RequestCulture(ci),
                SupportedCultures = new List<CultureInfo>
                {
                    ci,
                },
                SupportedUICultures = new List<CultureInfo>
                {
                    ci,
                }
            });
            */

            loggerFactory.AddSerilog();
            var fordwardedHeaderOptions = new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            };
            fordwardedHeaderOptions.KnownNetworks.Clear();
            fordwardedHeaderOptions.KnownProxies.Clear();

             app.UseSwaggerUI(c =>
                        {
                            c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API v1");
                        });
            app.UseForwardedHeaders(fordwardedHeaderOptions);
            app.UseCors("MyPolicy");
            app.UseSession();
            app.UseAuthentication();
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();
            
            app.UseMvc();
            
            //Logging of exception
            //app.UseExceptionless("2jiqmnyqSQvOgjxTyi2bXN6G8xSPm24hwByMK3Vy");
            
            /*
             * Above only logs unhandled exceptions. Exceptions handled must be logged manually in the following way:
             * try {
                throw new ApplicationException(Guid.NewGuid().ToString());
               } catch (Exception ex) {
                ex.ToExceptionless().Submit();
               }
             */
        }
    }
}
