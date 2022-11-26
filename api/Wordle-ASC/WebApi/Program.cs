using Database;
using Microsoft.EntityFrameworkCore;
using WebApi.Services;
using WebApi.Services.Interfaces;

var builder = WebApplication.CreateBuilder(args);

/////

var server = Environment.GetEnvironmentVariable("DatabaseServer");
var port = Environment.GetEnvironmentVariable("DatabasePort");
var user = Environment.GetEnvironmentVariable("DatabaserUser");
var password = Environment.GetEnvironmentVariable("DatabasePassword");
var database = Environment.GetEnvironmentVariable("DatabaseName");

var connectionString = $"Server={server}, {port}; Initial Catalog={database}; User ID={user}; Password={password}";

builder.Services.AddDbContext<UoW>(options => options.UseSqlServer(connectionString));

//////

builder.Services.AddScoped<IOpenerService, OpenerService>();
builder.Services.AddScoped<IGuessesService, GuessesService>();

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//if (app.Environment.IsDevelopment())
//{
app.UseSwagger();
app.UseSwaggerUI();
//}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
