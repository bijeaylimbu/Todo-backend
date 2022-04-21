
using MediatR;
using Microsoft.EntityFrameworkCore;
using TodoApi.Application.CommandHandlers;
using TodoApi.Application.Commands;
using TodoApi.Controllers;
using TodoApi.Domain.Interfaces;
using TodoApi.Infrastructure.Persistence;
using TodoApi.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<TodoDbContext>(options =>
    options.UseNpgsql(builder
            .Configuration
            .GetConnectionString("DefaultConnection")
            // x=> x.MigrationsAssembly(typeof(TodoDbContext).GetTypeInfo().Assembly.GetName().Name)
            )
        .UseSnakeCaseNamingConvention());
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMediatR(typeof(CreateTodoCommandHandler));
builder.Services.AddScoped<ITodoRepository, TodoRepository>();
builder.Services.AddCors();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors(x => x

    .AllowAnyOrigin()
    .AllowAnyMethod()
    .AllowAnyHeader());
app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
