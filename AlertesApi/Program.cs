<<<<<<<< HEAD:TTronAlertmain/src/TTronAlert.Api/Program.cs
using TTronAlert.Api.Data;
using Microsoft.EntityFrameworkCore;
using TTronAlert.Api.Hubs;
using TTronAlert.Api.Services;
using Microsoft.Extensions.DependencyInjection;
========
﻿using Microsoft.EntityFrameworkCore;
using AlertesApi.Data;
using AlertesApi.Hubs;
>>>>>>>> parent of 2c229fb (Probleme de base de données):AlertesApi/Program.cs

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

<<<<<<<< HEAD:TTronAlertmain/src/TTronAlert.Api/Program.cs
builder.Services.AddDbContext<AppDbContext>(options =>
========
// MariaDB
builder.Services.AddDbContext<AlertesContext>(options =>
>>>>>>>> parent of 2c229fb (Probleme de base de données):AlertesApi/Program.cs
    options.UseMySql(
        builder.Configuration.GetConnectionString("MariaDB"),
        new MariaDbServerVersion(new Version(10, 4, 32))
    ).LogTo(Console.WriteLine, LogLevel.Information).EnableSensitiveDataLogging());

builder.Services.AddSignalR();

<<<<<<<< HEAD:TTronAlertmain/src/TTronAlert.Api/Program.cs
// Enregistrement des services (c'est ici que �a fixe le 500)
builder.Services.AddScoped<IAlertService, AlertService>();
builder.Services.AddScoped<IDatabaseHealthService, DatabaseHealthService>();

========
// CORS
>>>>>>>> parent of 2c229fb (Probleme de base de données):AlertesApi/Program.cs
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
<<<<<<<< HEAD:TTronAlertmain/src/TTronAlert.Api/Program.cs
        policy.SetIsOriginAllowed(origin => true)
              .AllowAnyMethod()
              .AllowAnyHeader()
              .AllowCredentials();
    }));
========
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
>>>>>>>> parent of 2c229fb (Probleme de base de données):AlertesApi/Program.cs

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
<<<<<<<< HEAD:TTronAlertmain/src/TTronAlert.Api/Program.cs

app.UseCors("DevCorsPolicy");
========
>>>>>>>> parent of 2c229fb (Probleme de base de données):AlertesApi/Program.cs

app.UseCors("AllowAll");

app.UseAuthorization();

app.MapControllers();
<<<<<<<< HEAD:TTronAlertmain/src/TTronAlert.Api/Program.cs
app.MapHub<AlertHub>("/alerthub");
========
app.MapHub<AlertesHub>("/alerthub"); // Matcher le client
>>>>>>>> parent of 2c229fb (Probleme de base de données):AlertesApi/Program.cs

app.Run();