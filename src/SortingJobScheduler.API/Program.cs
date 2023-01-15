using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;
using SortingJobScheduler.API.Extensions;
using SortingJobScheduler.API.HostedServices;
using Swashbuckle.AspNetCore.Filters;
using System.Reflection;

namespace SortingJobScheduler.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Sorting Job API",
                    Description = "An ASP.NET Core Web API for managing Sorting Jobs",
                });

                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });
            builder.Services.AddServices();
            builder.Services.AddHostedService<SortingJobHostedService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseExceptionHandler(configure =>
            {
                configure.Run(async context =>
                {
                    var exceptionHandlerPathFeature = context.Features.Get<IExceptionHandlerPathFeature>();
                    var ex = exceptionHandlerPathFeature?.Error;
                    string title;

                    (context.Response.StatusCode, title) = ex switch
                    {
                        _ => (StatusCodes.Status500InternalServerError, "Internal server error")
                    };

                    await context.Response.WriteAsJsonAsync(
                        new ProblemDetails()
                        {
                            Title = title,
                            Status = context.Response.StatusCode,
                            Detail = ex?.Message
                        }).ConfigureAwait(false);
                });
            });

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}