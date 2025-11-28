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
        new MariaDbServerVersion(new Version(10, 4, 32))
    ));

// SignalR
builder.Services.AddSignalR();

// === CORS PARFAIT POUR DÉVELOPPEMENT LOCAL (fichier HTML + file://) ===
builder.Services.AddCors(options =>
{
    options.AddPolicy("DevCorsPolicy", policy =>
    {
        policy.SetIsOriginAllowed(origin => true) // autorise TOUT, y compris file:// et origin null
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials(); // obligatoire pour SignalR
    });
});
// ======================================================================

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// === TOUJOURS METTRE UseCors AVANT UseAuthorization ===
app.UseCors("DevCorsPolicy");
// =======================================================

app.UseHttpsRedirection();

app.UseAuthorization();
app.MapControllers();
app.MapHub<AlertesHub>("/hubs/alertes");

app.Run();