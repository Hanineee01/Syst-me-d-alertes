using Microsoft.EntityFrameworkCore;
using AlertesApi.Data;
using AlertesApi.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MariaDB
builder.Services.AddDbContext<AlertesContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MariaDB"),
        new MySqlServerVersion(new Version(10, 4, 32))
    ));

// SignalR
builder.Services.AddSignalR();

// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();
app.MapHub<AlertesHub>("/alerthub"); // Matcher le client

app.Run();