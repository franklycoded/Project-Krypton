using System.Threading.Tasks;
using System.Security.Claims;
using Microsoft.AspNet.Builder;
using Microsoft.AspNet.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using KryptonAPI.Configuration;
using Microsoft.Extensions.OptionsModel;
using Microsoft.AspNet.Authentication.JwtBearer;

namespace KryptonAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            // Set up configuration sources.
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Setup options with DI
            services.AddOptions();
            
            // Adding Auth0 settings to services
            services.Configure<Auth0Config>(Configuration.GetSection("Auth0Config"));
            
            // Configuring cors
            services.AddCors(options => {
               options.AddPolicy(
                   "mypolicy",
                   builder => {
                       var allowedDomains = new []{"http://localhost:5000","http://localhost:8100"};
                       
                       builder
                            .WithOrigins(allowedDomains)
                            .AllowAnyHeader()
                            .AllowAnyMethod()
                            .AllowCredentials();
                   }
               ); 
            });
            
            // Add framework services.
            services.AddMvc();
        }
        
        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IOptions<Auth0Config> auth0Config)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            app.UseIISPlatformHandler();
            app.UseCors("mypolicy");
            app.UseStaticFiles();
            
            ConfigureAuth0(app, loggerFactory, auth0Config.Value.Auth0Domain, auth0Config.Value.Auth0ClientID);
            
            app.UseMvc();
        }

        // Entry point for the application.
        public static void Main(string[] args) => Microsoft.AspNet.Hosting.WebApplication.Run<Startup>(args);
        
        private void ConfigureAuth0(IApplicationBuilder app, ILoggerFactory loggerFactory, string domain, string clientId){
            
            
            var logger = loggerFactory.CreateLogger("Auth0");
                       
            app.UseJwtBearerAuthentication(options =>
            {
                options.Audience = clientId;
                options.Authority = domain;
                options.AutomaticAuthenticate = true;
                options.AutomaticChallenge = true;

                options.Events = new JwtBearerEvents
                {
                    OnAuthenticationFailed = context =>
                    {
                        logger.LogError("Authentication failed.", context.Exception);
                        return Task.FromResult(0);
                    },
                    OnValidatedToken = context =>
                    {
                        var claimsIdentity = context.AuthenticationTicket.Principal.Identity as ClaimsIdentity;
                        claimsIdentity?.AddClaim(new Claim("id_token",
                            context.Request.Headers["Authorization"][0].Substring(context.AuthenticationTicket.AuthenticationScheme.Length + 1)));
                        
                        return Task.FromResult(0);
                    }
                };
            });
        }
    }
}
