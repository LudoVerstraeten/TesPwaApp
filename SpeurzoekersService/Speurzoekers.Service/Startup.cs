using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Speurzoekers.Common.Config;
using Speurzoekers.Common.Repositories;
using Speurzoekers.Data;
using Speurzoekers.Data.Entities.Identity;
using Speurzoekers.Data.Repositories;
using Speurzoekers.Service.Infrastructure;
using Speurzoekers.Service.Mappers;
using System;

namespace Speurzoekers.Service
{
    public class Startup
    {
        private string CorsPolicyName = "_allowAll";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors( options => options.AddPolicy(CorsPolicyName, builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()));
            services.AddSwaggerDocumentation();

            services.Configure<AuthenticationOptions>(Configuration.GetSection("AuthenticationOptions"));
            services.AddScoped<IUserRepository, FakeUserRepository>();

            services.AddAutoMapper(typeof(UserProfile));

            services.AddDbContext<SpeurzoekersDbContext>(options =>
                options.UseSqlServer(GetConnectionString())
            );

            services.AddIdentity<ApplicationUser, ApplicationRole>()
                .AddEntityFrameworkStores<SpeurzoekersDbContext>()
                .AddDefaultTokenProviders();

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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            app.UseSwaggerDocumentation();
            app.UseHttpsRedirection();
            app.UseCors(CorsPolicyName);
            app.UseMvc();
        }

        private string GetConnectionString()
        {
            switch (Environment.OSVersion.Platform)
            {
                case PlatformID.Win32NT:
                    return Configuration.GetSection("DatabaseConnectionStringWin").Value;
                case PlatformID.Unix:
                    return Configuration.GetSection("DatabaseConnectionStringUnix").Value;
                default:
                    throw new Exception("Can't configurate the DB, OS not recognized.");
            }
        }
    }
}
