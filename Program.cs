using DSW_3P_PR02.Data;
using DSW_3P_PR02.Repositories;
using DSW_3P_PR02.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<EventosDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SQLSERVERCONNECTION")));
builder.Services.AddScoped<IEventoRepository, SqlEventoRepository>();
builder.Services.AddScoped<IEventoService, EventoService>();
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
    var dbContext = scope.ServiceProvider.GetRequiredService<EventosDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("AnimissionFrontend");

app.MapControllers();

app.Run();

