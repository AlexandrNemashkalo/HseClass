using HseClass.Api.Configurations;
using HseClass.Api.Helpers;
using HseClass.Core.EF;
using HseClass.Core.Jwt;
using HseClass.Core.Repositories;
using HseClass.Core.Services;
using HseClass.Data.IRepositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace HseClass.Api
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
            services
                .AddControllers()
                .AddNewtonsoftJson(
                x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policyBuilder => policyBuilder
                    .AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                );
            });
            
            services.AddSwagger();

            services.AddDbContext<HseClassContext>(options => 
                    options.UseSqlite(Configuration.GetConnectionString("SqliteDb"),
                        b => b.MigrationsAssembly("HseClass.Api")),
                ServiceLifetime.Transient
            );

            services.ConfigureIdentity();
            services.ConfigureAuthentication(Configuration);

            services.AddTransient<IJwtGenerator, JwtGenerator>();
            services.AddTransient<IAuthService, AuthService>();

            services.AddTransient<IClassRoomRepository, ClassRoomRepository>();
            services.AddTransient<ILabRepository, LabRepository>();
            services.AddTransient<ITaskLabRepository,TaskLabRepository>();
            services.AddTransient<ISolutionLabRepository,SolutionLabRepository>();
            services.AddTransient<IUserClassRepository, UserClassRepository>();
            services.AddTransient<IUserRepository,UserRepository>();
          
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HseClass.Api v1"));
            }
            
            app.UseExceptionHandler(err => err.UseCustomErrors(env));

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors("CorsPolicy");
            
            app.UseAuthentication(); 
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}