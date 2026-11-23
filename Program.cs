using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Text;
using DSW_3P_PR02.Data;
using DSW_3P_PR02.Models;
using DSW_3P_PR02.Repositories;
using DSW_3P_PR02.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi;

var builder = WebApplication.CreateBuilder(args);

var jwtOptions = builder.Configuration.GetSection("Jwt").Get<JwtOptions>() ?? new JwtOptions();
var demoUserOptions = builder.Configuration.GetSection("DemoUser").Get<DemoUserOptions>()
    ?? new DemoUserOptions();
if (Encoding.UTF8.GetByteCount(jwtOptions.SigningKey) < 32)
{
    throw new InvalidOperationException(
        "Configura Jwt:SigningKey con al menos 32 bytes mediante User Secrets o variables de entorno.");
}

if (string.IsNullOrWhiteSpace(jwtOptions.Issuer)
    || string.IsNullOrWhiteSpace(jwtOptions.Audience)
    || jwtOptions.ExpirationMinutes <= 0)
{
    throw new InvalidOperationException("La configuración Jwt:Issuer, Jwt:Audience y Jwt:ExpirationMinutes es inválida.");
}

if (string.IsNullOrWhiteSpace(demoUserOptions.Username)
    || string.IsNullOrWhiteSpace(demoUserOptions.Password))
{
    throw new InvalidOperationException(
        "Configura DemoUser:Username y DemoUser:Password mediante User Secrets o variables de entorno.");
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Escribe el token JWT obtenido en /api/auth/login."
    });
    options.AddSecurityRequirement(document => new OpenApiSecurityRequirement
    {
        [new OpenApiSecuritySchemeReference("Bearer", document, null)] =
            new List<string>()
    });

    var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFile));
});
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<PersonajesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLSERVERCONNECTION")));
builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("Jwt"));
builder.Services.Configure<DemoUserOptions>(builder.Configuration.GetSection("DemoUser"));
builder.Services.AddScoped<IPersonajeRepository, SqlPersonajeRepository>();
builder.Services.AddScoped<IPersonajeService, PersonajeService>();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.MapInboundClaims = false;
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = jwtOptions.Issuer,
            ValidateAudience = true,
            ValidAudience = jwtOptions.Audience,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey)),
            ValidateLifetime = true,
            RequireExpirationTime = true,
            NameClaimType = JwtRegisteredClaimNames.Sub
        };
    });
builder.Services.AddAuthorization();

var allowedOrigins = builder.Configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
    ?? Array.Empty<string>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("CharacterHubClient", policy =>
        policy.WithOrigins(allowedOrigins)
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

app.UseExceptionHandler();
app.UseStatusCodePages();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("CharacterHubClient");
app.UseAuthentication();
app.UseAuthorization();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PersonajesDbContext>();
    await dbContext.Database.MigrateAsync();

    if (!await dbContext.Personajes.AnyAsync())
    {
        dbContext.Personajes.AddRange(
            new Personaje
            {
                Nombre = "Barry Allen",
                Videojuego = "DC GAME",
                Clase = "Heroe",
                Nivel = 98,
                Poder = 930,
                Disponible = true,
                ImagenUrl = "https://placehold.co/600x800.png?text=Barry"
            },
            new Personaje
            {
                Nombre = "Robin",
                Videojuego = "DC FAME",
                Clase = "Heroe",
                Nivel = 86,
                Poder = 860,
                Disponible = true,
                ImagenUrl = "https://placehold.co/600x800.png?text=Robin"
            },
            new Personaje
            {
                Nombre = "Jonsey",
                Videojuego = "Fortnite",
                Clase = "Heroe",
                Nivel = 72,
                Poder = 720,
                Disponible = true,
                ImagenUrl = "https://placehold.co/600x800.png?text=Jonsey"
            });

        await dbContext.SaveChangesAsync();
    }
}

app.MapControllers();

app.Run();