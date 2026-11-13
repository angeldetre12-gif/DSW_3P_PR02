using DSW_3P_PR02.Models;
using DSW_3P_PR02.Repositories;

namespace DSW_3P_PR02.Services;

public sealed class EventoService(IEventoRepository repository) : IEventoService
{
    public Task<IReadOnlyList<Evento>> ObtenerTodosAsync() =>
        repository.ObtenerTodosAsync();

    public Task<Evento?> ObtenerPorIdAsync(int id) =>
        repository.ObtenerPorIdAsync(id);

    public Task<Evento> CrearAsync(Evento evento) =>
        repository.CrearAsync(evento);

    public Task<bool> ActualizarAsync(Evento evento) =>
        repository.ActualizarAsync(evento);

    public Task<bool> EliminarAsync(int id) =>
        repository.EliminarAsync(id);
}
