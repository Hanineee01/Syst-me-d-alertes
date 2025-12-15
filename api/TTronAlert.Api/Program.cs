using TTronAlert.Api.Data;
using Microsoft.EntityFrameworkCore;
using TTronAlert.Api.Hubs;
using TTronAlert.Api.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo { Title = "TTronAlert API", Version = "v1" });
});

var conn = builder.Configuration.GetConnectionString("DefaultConnection") ?? builder.Configuration.GetConnectionString("MariaDB");
if (string.IsNullOrEmpty(conn))
    throw new InvalidOperationException("No connection string configured");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseMySql(conn, new MariaDbServerVersion(new Version(10, 4, 32))).EnableSensitiveDataLogging());

builder.Services.AddSignalR();
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<IDatabaseHealthService, DatabaseHealthService>();

builder.Services.AddCors(options => options.AddPolicy("DevCorsPolicy", policy => policy.SetIsOriginAllowed(_ => true).AllowAnyMethod().AllowAnyHeader().AllowCredentials()));

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI(options => options.SwaggerEndpoint("/swagger/v1/swagger.json", "TTronAlert API V1"));

app.UseCors("DevCorsPolicy");
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.MapHub<AlertHub>("/alerthub");
app.Run();