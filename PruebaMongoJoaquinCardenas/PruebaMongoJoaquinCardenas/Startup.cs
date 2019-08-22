using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PruebaMongoJoaquinCardenas.Modelos;
using PruebaMongoJoaquinCardenas.Servicios;

namespace PruebaMongoJoaquinCardenas
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
            services.Configure<PruebaMonostoreDatabaseSettings>(
            Configuration.GetSection(nameof(PruebaMonostoreDatabaseSettings)));

            services.AddSingleton<IPruebaMonostoreDatabaseSettings>(sp =>
                sp.GetRequiredService<IOptions<PruebaMonostoreDatabaseSettings>>().Value);
            services.AddSingleton<FacturaService>();
            services.AddSingleton<ClienteService>();
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
            app.UseCors(builder => builder.AllowAnyHeader().AllowAnyMethod().AllowAnyOrigin().AllowCredentials());
            app.UseHttpsRedirection();
            app.UseMvc();

        }
    }
}
