using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Luxubu.Filters;
using Luxubu.Models;
using Microsoft.EntityFrameworkCore;

namespace Luxubu
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
            services.AddDbContext<LuxubuContext>(options => 
            {
                options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=Luxubu;Trusted_Connection=True;");
               
            }, ServiceLifetime.Scoped);

            services.AddMvc(option => 
            {
                option.Filters.Add(new AuthenticationFilter());
            });
            services.AddDistributedMemoryCache();
            services.AddSession(option => 
            {
                option.IdleTimeout = TimeSpan.FromMinutes(10);
                option.Cookie.HttpOnly = true;
            });

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();
            app.UseSession();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
