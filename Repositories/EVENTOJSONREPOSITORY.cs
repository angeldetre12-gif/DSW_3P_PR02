using System.Text.Json;
using DSW_3P_PR02.Models;

namespace DSW_3P_PR02.Repositories;

public sealed class JsonEventoRepository : IEventoRepository
{
    private static readonly JsonSerializerOptions JsonOptions = new()
    {
        WriteIndented = true
    };

    private readonly string _filePath;
    private readonly SemaphoreSlim _mutex = new(1, 1);

    public JsonEventoRepository(IWebHostEnvironment environment)
    {
        var dataDirectory = Path.Combine(environment.ContentRootPath, "Data");
        Directory.CreateDirectory(dataDirectory);
        _filePath = Path.Combine(dataDirectory, "eventos.json");
    }

    public async Task<IReadOnlyList<Evento>> ObtenerTodosAsync()
    {
        await _mutex.WaitAsync();
        try
        {
            return await LeerAsync();
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task<Evento?> ObtenerPorIdAsync(int id)
    {
        var eventos = await ObtenerTodosAsync();
        return eventos.FirstOrDefault(evento => evento.Id == id);
    }

    public async Task<Evento> CrearAsync(Evento evento)
    {
        await _mutex.WaitAsync();
        try
        {
            var eventos = await LeerAsync();
            evento.Id = eventos.Count == 0 ? 1 : eventos.Max(item => item.Id) + 1;
            eventos.Add(evento);
            await GuardarAsync(eventos);
            return evento;
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task<bool> ActualizarAsync(Evento evento)
    {
        await _mutex.WaitAsync();
        try
        {
            var eventos = await LeerAsync();
            var index = eventos.FindIndex(item => item.Id == evento.Id);
            if (index < 0)
            {
                return false;
            }

            eventos[index] = evento;
            await GuardarAsync(eventos);
            return true;
        }
        finally
        {
            _mutex.Release();
        }
    }

    public async Task<bool> EliminarAsync(int id)
    {
        await _mutex.WaitAsync();
        try
        {
            var eventos = await LeerAsync();
            var evento = eventos.FirstOrDefault(item => item.Id == id);
            if (evento is null)
            {
                return false;
            }

            eventos.Remove(evento);
            await GuardarAsync(eventos);
            return true;
        }
        finally
        {
            _mutex.Release();
        }
    }

    private async Task<List<Evento>> LeerAsync()
    {
        if (!File.Exists(_filePath))
        {
            return [];
        }

        await using var stream = File.OpenRead(_filePath);
        return await JsonSerializer.DeserializeAsync<List<Evento>>(stream, JsonOptions) ?? [];
    }

    private async Task GuardarAsync(List<Evento> eventos)
    {
        var temporaryPath = $"{_filePath}.tmp";
        await using (var stream = File.Create(temporaryPath))
        {
            await JsonSerializer.SerializeAsync(stream, eventos, JsonOptions);
        }

        File.Move(temporaryPath, _filePath, overwrite: true);
    }

}
