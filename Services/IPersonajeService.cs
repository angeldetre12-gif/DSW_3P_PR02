using DSW_3P_PR02.Models;

namespace DSW_3P_PR02.Services;

public interface IPersonajeService
{
    Task<IReadOnlyList<Personaje>> ObtenerTodosAsync();
    Task<Personaje?> ObtenerPorIdAsync(int id);
    Task<Personaje> CrearAsync(Personaje personaje);
    Task<bool> ActualizarAsync(Personaje personaje);
    Task<bool> EliminarAsync(int id);
}
