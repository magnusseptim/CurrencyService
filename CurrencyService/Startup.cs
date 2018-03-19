using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using CurrencyService.Contexts;
using Microsoft.EntityFrameworkCore;
using CurrencyService.Model;
using CurrencyService.Repositories.Interfaces;
using CurrencyService.Repositories;
using CurrencyService.Data;
using CurrencyService.GlobalHelpers;
using CurrencyService.Providers.Interfaces;
using CurrencyService.Providers;
using CurrencyService.Services.Interfaces;
using CurrencyService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CurrencyService.Security.Interfaces;
using CurrencyService.GlobalHelpers.Interfaces;
using Microsoft.Extensions.Hosting;

namespace CurrencyService
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
            services.AddDbContext<CurrencyContext>(options =>
                        options.UseSqlServer(Configuration.GetConnectionString("CurrencyConnectionString")));
            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v1", new Swashbuckle.AspNetCore.Swagger.Info { Title = "CurrencyService", Version = "v1" });
            });

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                        {
                            ValidateIssuer = true,
                            ValidateAudience = true,
                            ValidateLifetime = true,
                            ValidateIssuerSigningKey = true,
                            ValidIssuer = Configuration["Jwt:Issuer"],
                            ValidAudience = Configuration["Jwt:Issuer"],
                            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                        };
                    });

            services.AddMvc();

            services.AddSingleton(Configuration.GetSection("RandomSeed").Get<RandomSeed>());
            services.AddSingleton<IXmlReaderRepository, XmlReaderRepository>();

            services.AddScoped<ICurrencyRepository, CurrencyRepository>();
            services.AddScoped<ITokenBuilder, TokenBuilder>();

            services.AddSingleton<ICurrencyProvider, CurrencyProvider>();
            services.AddSingleton<IHostedService, DataRefreshService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            if (env.IsDevelopment())
            {
                using (var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope())
                {
                    if (!serviceScope.ServiceProvider.GetService<CurrencyContext>().AllMigrationApplied())
                    {
                        serviceScope.ServiceProvider.GetService<CurrencyContext>().Database.Migrate();
                        serviceScope.ServiceProvider.GetService<CurrencyContext>().Seed(new XmlReaderRepository());
                    }
                    else
                    {
                        serviceScope.ServiceProvider.GetService<CurrencyContext>().ComplementarySeed(new XmlReaderRepository());
                    }
                }
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                 x.SwaggerEndpoint("/swagger/v1/swagger.json", "Currency Service");
            });
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                        name: "default",
                        template: "{controller}/{action}/{id?}",
                        defaults: new { controller = "Welcome", action = "Index" }
                    );
            });
        }
    }
}
