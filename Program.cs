using DSW_3P_PR02.Repositories;
using DSW_3P_PR02.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IEventoRepository, JsonEventoRepository>();
builder.Services.AddScoped<IEventoService, EventoService>();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();

