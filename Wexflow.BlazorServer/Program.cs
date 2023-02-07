using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.OpenApi.Models;
using System.Reflection;
using System.Xml;
using Wexflow.BlazorServer.Authentication;
using Wexflow.BlazorServer.Controllers;
using Wexflow.BlazorServer.Data;
using Wexflow.BlazorServer.Middleware;

namespace Wexflow.BlazorServer
{
    public class Program
    {


        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Configuration.AddConfiguration(BuildConfig());
            builder.Logging.AddLog4Net();

            
            // Add services to the container.
            builder.Services.Configure<KestrelServerOptions>(options =>
            {
                options.AllowSynchronousIO = true;
            });
            builder.Services.AddCors();

            builder.Services.AddControllers();

            builder.Services.AddAntDesign();

            builder.Services.AddRazorPages();

            builder.Services.AddBlazoredLocalStorage();

            builder.Services.AddAutoMapper(cfg => { cfg.AddProfile<MapProfile>(); });

            builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();

            builder.Services.AddServerSideBlazor();
            builder.Services.AddSingleton<WeatherForecastService>();
            builder.Services.AddSingleton<WexflowService>();
            builder.Services.AddScoped<DashboardController>();
            builder.Services.AddScoped<HistoryController>();
            builder.Services.AddScoped<ManagerController>();

            builder.Services.AddScoped<UserController>();
            

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
            }
            else
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }


            app.UseStaticFiles();


            //app.UseMiddleware<BasicAuthMiddleware>(!app.Environment.IsDevelopment());

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials()
            );



            app.MapBlazorHub();

            app.MapFallbackToPage("/_Host");


            app.MapControllers();

            app.Run();
        }

        private static IConfiguration BuildConfig()
        {
            var Config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                
                .Build();

            return Config;

        }

    }
}