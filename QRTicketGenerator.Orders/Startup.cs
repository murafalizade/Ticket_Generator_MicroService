using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace QRTicketGenerator.Orders
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
            services.AddMassTransit(x =>

              x.UsingRabbitMq((context, cfg) =>
              {
                  cfg.Host("localhost", "/", host =>
                  {
                      host.Username("guest");
                      host.Password("guest");
                  });
              }));

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                option =>
                {
                    option.Authority = "https://localhost:5001";
                    option.Audience = "Order_aud";
                    //option.Audience = "https://localhost:5001/resources";
                    option.RequireHttpsMetadata = false;

                });

            string mySqlConnectionStr = Configuration.GetConnectionString("DefaultConnection");
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseMySql(mySqlConnectionStr, ServerVersion.AutoDetect(mySqlConnectionStr)));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "QRTicketGenerator.Orders", Version = "v1" });
                c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
                {
                    In = ParameterLocation.Header,
                    Description = "Bearer",
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey
                }
                    );
                c.OperationFilter<SecurityRequirementsOperationFilter>();
            });
            services.AddAutoMapper(typeof(Program));
            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IOrderRepository,OrderRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "QRTicketGenerator.Orders v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
