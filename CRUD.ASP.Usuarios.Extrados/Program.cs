using AccesData.Interfaces;
using CRUD.ASP.Usuarios.Extrados.GateWays;
using Services.Interfaces;
using Services.Security;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.--------------

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
// dependencies--------------
builder.Services.AddScoped<IConfigSqlConnect, ConfigSqlConnect>();
builder.Services.AddScoped<IHashService, HashService>();
var app = builder.Build();

// Configure the HTTP request pipeline.----------
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
