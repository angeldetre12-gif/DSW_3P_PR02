using DSW_3P_PR02.Models;

namespace DSW_3P_PR02.Repositories;

public interface IPersonajeRepository
{
    Task<IReadOnlyList<Personaje>> ObtenerTodosAsync(CancellationToken cancellationToken);
    Task<Personaje?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<Personaje>> ObtenerPorVideojuegoAsync(string nombre, CancellationToken cancellationToken);
    Task<IReadOnlyList<Personaje>> ObtenerPorNivelAsync(int nivel, CancellationToken cancellationToken);
    Task<Personaje> CrearAsync(Personaje personaje, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(Personaje personaje, CancellationToken cancellationToken);
    Task<bool> EliminarAsync(int id, CancellationToken cancellationToken);
}
