using FluentValidation.AspNetCore;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using ZedCrestWalletApplication.DAL;
using ZedCrestWalletApplication.Models;
using ZedCrestWalletApplication.Services.Implementations;
using ZedCrestWalletApplication.Services.Interfaces;
using ZedCrestWalletApplication.Utils;

namespace ZedCrestWalletApplication
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
            services.AddDbContext<ZedCrestWalletContext>(option => option.UseSqlServer(Configuration.GetConnectionString("ZedCrestWalletConnection")));
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ITransactionService, TransactionService>();
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            services.AddSwaggerGen(k =>
            {
                k.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Zed crest wallet interview solution",
                    Version = "v2",
                    Description = "All you need to know about this test, it's very techie",
                    Contact = new Microsoft.OpenApi.Models.OpenApiContact
                    {
                        Name = "Adedayo Akerele",
                        Email = "akereledayo94@gmail.com",
                        Url = new Uri("https://github.com/GreenZoneResources")
                    }
                });
            });
            services.AddSingleton<IRabitMQProducer, RabbitMQProducer>();
            //services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddMediatR(typeof(Startup));
            services.AddControllers().AddFluentValidation(fv => {
                fv.DisableDataAnnotationsValidation = false;
                fv.RegisterValidatorsFromAssemblyContaining<Startup>();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseSwagger();
            app.UseSwaggerUI(x =>
            {
                var profile = string.IsNullOrEmpty(x.RoutePrefix) ? "." : "..";
                x.SwaggerEndpoint($"{profile}/swagger/v1/swagger.json", "Zed crest wallet interview solution");
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
