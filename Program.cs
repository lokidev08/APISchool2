using Microsoft.EntityFrameworkCore;
using APISchool.Data;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;
using System.Collections.Generic;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

var jwt = builder.Configuration.GetSection("Jwt");

builder.Services
    .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = jwt["Issuer"],
            ValidAudience = jwt["Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt["Key"] ?? string.Empty)),
            ClockSkew = TimeSpan.Zero
        };

        options.Events = new JwtBearerEvents
        {
            OnChallenge = context =>
            {
                context.HandleResponse();

                var message = context.AuthenticateFailure is SecurityTokenExpiredException
                    ? "Token expirado. Inicia sesion otra vez."
                    : "Token invalido o no enviado. No estas autorizado.";

                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                context.Response.ContentType = "application/json";

                return context.Response.WriteAsJsonAsync(new
                {
                    authorized = false,
                    message
                });
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.FallbackPolicy = new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build();
});
builder.Services.AddHttpContextAccessor();

builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "APISchool",
        Version = "v1"
    });

    var bearerScheme = new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Pegue solo el token JWT. Swagger enviara el prefijo Bearer automaticamente."
    };

    options.AddSecurityDefinition("Bearer", bearerScheme);
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecuritySchemeReference("Bearer", document, null),
            new List<string>()
        }
    });

});

builder.Services.AddDbContext<ColegioDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

app.UseStaticFiles();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.InjectJavascript("/swagger-token-validation.js");
    });
    app.MapOpenApi().AllowAnonymous();
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProtectedController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok("Acceso autorizado a recurso protegido");
    }
}
