using AutoMapper;
using Zeppeling.Infrastructure.Domain.Context.Context;
using Zeppeling.Infrastructure.Presentation.API.Extensions;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Zeppeling.Infrastructure.Presentation.API
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
            services.AddMvc().AddFluentValidation();
            services.ConfigureContext(Configuration);
            services.AddHttpContextAccessor();
            services.ConfigureMediatr();
            services.ConfigureUnitOfWork();
            services.ConfigureRepository();
            services.ConfigureLogger(Configuration);
            services.ConfigureCors();
            services.ConfigureSwagger();
            services.AddAutoMapper(typeof(Startup));
            services.ConfigureFluentValidation();
            services.ConfigureRedisCache();
            services.ConfigureInterceptors();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, DomainContext domainContext)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseHsts();

            domainContext.Database.EnsureCreated();

            // ===== Use Authentication ======
            app.UseHttpsRedirection();
           
            app.UseCors(options => options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
            app.UseStaticFiles();

            app.UseRouting();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();
            // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c => { c.SwaggerEndpoint("/swagger/ZeppelingInfrastructure/swagger.json", "ZeppelingInfrastructure"); });

            app.UseMiddleware<ErrorHandlingMiddleware>();
            
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}