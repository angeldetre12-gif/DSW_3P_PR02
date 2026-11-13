using DSW_3P_PR02.Models;

namespace DSW_3P_PR02.Services;

public interface IEventoService
{
    Task<IReadOnlyList<Evento>> ObtenerTodosAsync();
    Task<Evento?> ObtenerPorIdAsync(int id);
    Task<Evento> CrearAsync(Evento evento);
    Task<bool> ActualizarAsync(Evento evento);
    Task<bool> EliminarAsync(int id);
}
