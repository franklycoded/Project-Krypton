using KryptonAPI.Configuration;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using System.Threading.Tasks;
using System.Security.Claims;
using KryptonAPI.UnitOfWork;
using KryptonAPI.Repository;
using KryptonAPI.DataContractMappers;
using KryptonAPI.Data;
using KryptonAPI.Service;
using KryptonAPI.DataContracts.JobScheduler;
using KryptonAPI.Data.Models.JobScheduler;
using KryptonAPI.DataContractMappers.JobScheduler;

namespace KryptonAPI
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Setup options with DI
            services.AddOptions();

            // Adding Auth0 settings to services
            services.Configure<Auth0Config>(Configuration.GetSection("Auth0Config"));
            
            // Registering uow and repository layer
            services.AddSingleton<IUnitOfWorkContextFactory, UnitOfWorkContextFactory>();
            services.AddScoped<IUnitOfWork, UnitOfWorkScope>();
            services.AddScoped<IUnitOfWorkScope, UnitOfWorkScope>((serviceProvider) => {
                return serviceProvider.GetService<IUnitOfWork>() as UnitOfWorkScope;
            });
            services.AddScoped<IRepositoryFactory<KryptonAPIContext>, RepositoryFactory<KryptonAPIContext>>();

            // Registering service layer
            //services.AddSingleton<IDataContractMapperFactory, DataContractMapperFactory>();
            services.AddSingleton<IDataContractMapper<JobItem, JobItemDto>, JobItemDtoMapper>();
            services.AddScoped<ICRUDManager<JobItem, JobItemDto>, CRUDManager<KryptonAPIContext, JobItem, JobItemDto>>();
            
            // Configuring cors
            services.AddCors(options => {
               options.AddPolicy(
                   "mypolicy",
                   builder => {
                       var allowedDomains = new []{"http://localhost:5000","http://localhost:8100", "http://192.168.1.66:8100"};
                       
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

            app.UseCors("mypolicy");
            
            ConfigureAuth0(app, loggerFactory, auth0Config.Value.Auth0Domain, auth0Config.Value.Auth0ClientID);

            app.UseMvc();
        }
        
        private void ConfigureAuth0(IApplicationBuilder app, ILoggerFactory loggerFactory, string domain, string clientId){
            
            
            var logger = loggerFactory.CreateLogger("Auth0");
            
            var jwtBearerOptions = new JwtBearerOptions();
            jwtBearerOptions.Audience = clientId;
            jwtBearerOptions.Authority = domain;
            jwtBearerOptions.AutomaticAuthenticate = true;
            jwtBearerOptions.AutomaticChallenge = true;

            var events = new JwtBearerEvents();
            events.OnAuthenticationFailed = context =>
                {
                    logger.LogError("Authentication failed.", context.Exception);
                    return Task.FromResult(0);
                };
            events.OnTokenValidated = context =>
                {
                    var claimsIdentity = context.Ticket.Principal.Identity as ClaimsIdentity;
                    claimsIdentity?.AddClaim(new Claim("id_token",
                        context.Request.Headers["Authorization"][0].Substring(context.Ticket.AuthenticationScheme.Length + 1)));
                    
                    return Task.FromResult(0);
                };

            jwtBearerOptions.Events = events;
            app.UseJwtBearerAuthentication(jwtBearerOptions);
        }
    }
}
