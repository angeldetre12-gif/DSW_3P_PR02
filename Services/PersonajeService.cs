using DSW_3P_PR02.Models;
using DSW_3P_PR02.Repositories;

namespace DSW_3P_PR02.Services;

public sealed class PersonajeService(IPersonajeRepository repository) : IPersonajeService
{
    public async Task<IReadOnlyList<PersonajeResponse>> ObtenerTodosAsync(
        CancellationToken cancellationToken) =>
        (await repository.ObtenerTodosAsync(cancellationToken)).Select(Map).ToArray();

    public async Task<PersonajeResponse?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken)
    {
        var personaje = await repository.ObtenerPorIdAsync(id, cancellationToken);
        return personaje is null ? null : Map(personaje);
    }

    public async Task<IReadOnlyList<PersonajeResponse>> ObtenerPorVideojuegoAsync(
        string nombre,
        CancellationToken cancellationToken) =>
        (await repository.ObtenerPorVideojuegoAsync(nombre, cancellationToken)).Select(Map).ToArray();

    public async Task<IReadOnlyList<PersonajeResponse>> ObtenerPorNivelAsync(
        int nivel,
        CancellationToken cancellationToken) =>
        (await repository.ObtenerPorNivelAsync(nivel, cancellationToken)).Select(Map).ToArray();

    public async Task<PersonajeResponse> CrearAsync(
        CrearPersonajeRequest request,
        CancellationToken cancellationToken)
    {
        var personaje = new Personaje
        {
            Nombre = request.Nombre,
            Videojuego = request.Videojuego,
            Clase = request.Clase,
            Nivel = request.Nivel,
            Poder = request.Poder,
            Disponible = true,
            ImagenUrl = request.ImagenUrl
        };

        return Map(await repository.CrearAsync(personaje, cancellationToken));
    }

    public Task<bool> ActualizarAsync(
        int id,
        ActualizarPersonajeRequest request,
        CancellationToken cancellationToken) =>
        repository.ActualizarAsync(new Personaje
        {
            Id = id,
            Nombre = request.Nombre,
            Videojuego = request.Videojuego,
            Clase = request.Clase,
            Nivel = request.Nivel,
            Poder = request.Poder,
            Disponible = request.Disponible,
            ImagenUrl = request.ImagenUrl
        }, cancellationToken);

    public Task<bool> EliminarAsync(int id, CancellationToken cancellationToken) =>
        repository.EliminarAsync(id, cancellationToken);

    private static PersonajeResponse Map(Personaje personaje) =>
        new(
            personaje.Id,
            personaje.Nombre,
            personaje.Videojuego,
            personaje.Clase,
            personaje.Nivel,
            personaje.Poder,
            personaje.Disponible,
            personaje.ImagenUrl);
}
