using AutoMapper;
using DataLibrary.Interfaces;
using DataLibrary.Repositories;
using DomainServices.Classes;
using DomainServices.Interfaces;
using DomainServices.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using WebApi.Classes;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers(opt =>
            {
                opt.Filters.Add<ExceptionFilter>();
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            
            builder.Services.AddApiVersioning(opt => 
            { 
                opt.AssumeDefaultVersionWhenUnspecified = true; 
            });

            builder.Services.AddSingleton(st => new MapperConfiguration(cfg =>
            {
                cfg.AddProfile(new DomainModelMapper());
            }).CreateMapper());

            builder.Services.AddScoped<ITransactionService, TransactionService>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();

            WebApplication app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthorization();
            app.MapControllers();
            app.Run();
        }
    }
}