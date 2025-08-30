
using Gateway.Api;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using System.Text;

using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;

var builder = WebApplication.CreateBuilder(args);

// Configurar Ocelot para cargar rutas desde un archivo JSON
builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddJsonFile("ocelot.SwaggerEndPoints.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

builder.Services
    .AddOcelot()
    .AddConsul();
builder.Services.AddSwaggerForOcelot(builder.Configuration);
var app = builder.Build();
app.UseSwaggerForOcelotUI(opt => { opt.PathToSwaggerGenerator = "/swagger/docs"; }); // opcional
await app.UseOcelot();

app.Run();