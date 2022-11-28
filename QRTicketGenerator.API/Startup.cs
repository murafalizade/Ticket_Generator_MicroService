using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using QRTicketGenerator.API.Data;
using QRTicketGenerator.API.Services;
using Swashbuckle.AspNetCore.Filters;

namespace QRTicketGenerator.API
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
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(
                option =>
                {
                    option.Authority = "https://localhost:5001";
                    //option.Audience = "Ticket_aud";
                                    option.Audience = "https://localhost:5001/resources";

                    option.RequireHttpsMetadata = false;

                });
            services.Configure<TicketDatabaseSettings>(
      Configuration.GetSection(nameof(TicketDatabaseSettings)));

            services.AddSingleton<ITicketDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<TicketDatabaseSettings>>().Value);
            services.AddScoped<ITicketService, TicketService>();
            services.AddScoped<IEventService, EventService>();
            services.AddScoped<ITicketDesignService, TicketDesignService>();
            services.AddAutoMapper(typeof(Program));
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "QRTicketGenerator.API", Version = "v1" });
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
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "QRTicketGenerator.API v1"));
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
