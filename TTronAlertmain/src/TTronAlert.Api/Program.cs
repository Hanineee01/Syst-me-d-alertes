using TTronAlert.Api.Data;
using Microsoft.EntityFrameworkCore;
using TTronAlert.Api.Hubs;
using TTronAlert.Api.Services;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(
        builder.Configuration.GetConnectionString("MariaDB"),
        new MariaDbServerVersion(new Version(10, 4, 32))
    ).LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging());

builder.Services.AddSignalR();

// Enregistrement des services (c'est ici que ça fixe le 500)
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<IDatabaseHealthService, DatabaseHealthService>();

builder.Services.AddCors(options =>
    options.AddPolicy("DevCorsPolicy", policy =>
    {
        policy.SetIsOriginAllowed(origin => true)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    }));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("DevCorsPolicy");

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHub<AlertHub>("/alerthub");

app.Run();