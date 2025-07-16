using Application.DTO;
using Application.Interfaces;
using Application.Services;
using Domain.Factory;
using Domain.Interfaces;
using Domain.IRepository;
using Domain.Models;
using Infrastructure;
using Infrastructure.Repositories;
using InterfaceAdapters;
using InterfaceAdapters.Consumers;
using MassTransit;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

builder.Services.AddDbContext<DeviceContext>(opt =>
    opt.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
    );

//Services
builder.Services.AddTransient<IDeviceService, DeviceService>();

//Repositories
builder.Services.AddTransient<IDeviceRepository, DeviceRepository>();

//Factories
builder.Services.AddScoped<IDeviceFactory, DeviceFactory>();

//Mappers
builder.Services.AddAutoMapper(cfg =>
{
    //DTO
    cfg.CreateMap<Device, DeviceDTO>();
    cfg.CreateMap<IDevice, DeviceDTO>();
    cfg.CreateMap<DeviceDTO, Device>();
});


// MassTransit
var instanceId = InstanceInfo.InstanceId;

builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<DeviceCreatedConsumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host("localhost", 5674, "/", h =>
        {
            h.Username("guest");
            h.Password("guest");
        });

        cfg.ReceiveEndpoint($"devices-query-{instanceId}", e =>
        {
            e.ConfigureConsumer<DeviceCreatedConsumer>(ctx);
        });
    });
});

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.

app.MapOpenApi();
app.UseSwagger();
app.UseSwaggerUI();



app.UseCors(builder => builder
                .AllowAnyHeader()
                .AllowAnyMethod()
                .SetIsOriginAllowed((host) => true)
                .AllowCredentials());

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }
