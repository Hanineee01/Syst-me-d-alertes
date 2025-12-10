using TTronAlert.Api.Data;
using TTronAlert.Api.Hubs;
using Microsoft.EntityFrameworkCore;
using TTronAlert.Api.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// MariaDB with correct connection string
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MariaDB"),
        new MySqlServerVersion(new Version(10, 4, 32))
    ));

// SignalR
builder.Services.AddSignalR();

// CORS for WPF client
builder.Services.AddCors(options =>
    options.AddPolicy("DevCorsPolicy", policy =>
    {
        policy.SetIsOriginAllowed(origin => true) // allow all origins including file://
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // required for SignalR
    }));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
// Use CORS before authorization
app.UseCors("DevCorsPolicy");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHub<AlertHub>("/hubs/alertes");

app.Run();