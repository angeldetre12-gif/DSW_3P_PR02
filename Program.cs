using DSW_3P_PR02.Data;
using DSW_3P_PR02.Models;
using DSW_3P_PR02.Repositories;
using DSW_3P_PR02.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<PersonajesDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLSERVERCONNECTION")));
builder.Services.AddScoped<IPersonajeRepository, SqlPersonajeRepository>();
builder.Services.AddScoped<IPersonajeService, PersonajeService>();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AnimissionFrontend", policy =>
        policy.WithOrigins(
                "https://angeldetre12-gif.github.io",
                "http://localhost:5500",
                "http://127.0.0.1:5500")
            .AllowAnyHeader()
            .AllowAnyMethod());
});

var app = builder.Build();

await using (var scope = app.Services.CreateAsyncScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<PersonajesDbContext>();
    await dbContext.Database.MigrateAsync();

    if (!await dbContext.Personajes.AnyAsync())
    {
        dbContext.Personajes.AddRange(
            new Personaje
            {
                Nombre = "Ariadna Solís",
                Anime = "El archivo de las auroras",
                Tipo = "Protagonista",
                Poder = 93,
                Habilidad = "Luz prismática",
                Favorito = true
            },
            new Personaje
            {
                Nombre = "Kael Virel",
                Anime = "Los mapas de ceniza",
                Tipo = "Antagonista",
                Poder = 86,
                Habilidad = "Gravedad invertida"
            },
            new Personaje
            {
                Nombre = "Nima",
                Anime = "El jardín de las lunas",
                Tipo = "Secundario",
                Poder = 72,
                Habilidad = "Eco curativo"
            });

        await dbContext.SaveChangesAsync();
    }
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AnimissionFrontend");

app.MapControllers();

app.Run();

