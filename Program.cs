using Gateway.Api.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using MMLib.SwaggerForOcelot.DependencyInjection;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Consul;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

builder.Configuration
    .SetBasePath(Directory.GetCurrentDirectory())
    .AddJsonFile("ocelot.json", optional: false, reloadOnChange: true)
    .AddJsonFile("swagger.json", optional: true, reloadOnChange: true)
    .AddEnvironmentVariables();

var jwtSettings = builder.Configuration.GetSection("JwtSettings");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwtSettings["Issuer"],
            ValidAudience = jwtSettings["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]))
        };
    });

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowConfigured", policy =>
    {
        var origins = builder.Configuration.GetSection("cors:origins")
            .GetChildren()
            .Select(c => c.GetValue<string>("origin"))
            .Where(o => !string.IsNullOrEmpty(o))
            .ToArray();

        policy.WithOrigins(origins)
            .AllowAnyMethod()
            .AllowAnyHeader()
            .AllowCredentials();
    });
});

builder.Services.AddHttpClient()
    .ConfigureHttpClient((sp, client) =>
    {
        client.DefaultRequestHeaders.Add("User-Agent", "API-Gateway/1.0");
    })
    .AddHttpMessageHandler<BlacklistHandler>();

builder.Services.AddScoped<BlacklistHandler>();

builder.Services
    .AddOcelot()
    .AddConsul();

builder.Services.AddSwaggerForOcelot(builder.Configuration);

var app = builder.Build();

app.UseSwaggerForOcelotUI(opt =>
{
    opt.PathToSwaggerGenerator = "/swagger/docs";
});

app.UseHttpsRedirection();
app.UseCors("AllowConfigured");
app.UseAuthentication();
app.UseAuthorization();

await app.UseOcelot();

app.Run();