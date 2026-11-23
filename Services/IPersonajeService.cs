using DSW_3P_PR02.Models;

namespace DSW_3P_PR02.Services;

public interface IPersonajeService
{
    Task<IReadOnlyList<PersonajeResponse>> ObtenerTodosAsync(CancellationToken cancellationToken);
    Task<PersonajeResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken);
    Task<IReadOnlyList<PersonajeResponse>> ObtenerPorVideojuegoAsync(
        string nombre,
        CancellationToken cancellationToken);
    Task<IReadOnlyList<PersonajeResponse>> ObtenerPorNivelAsync(int nivel, CancellationToken cancellationToken);
    Task<PersonajeResponse> CrearAsync(CrearPersonajeRequest request, CancellationToken cancellationToken);
    Task<bool> ActualizarAsync(
        int id,
        ActualizarPersonajeRequest request,
        CancellationToken cancellationToken);
    Task<bool> EliminarAsync(int id, CancellationToken cancellationToken);
}
