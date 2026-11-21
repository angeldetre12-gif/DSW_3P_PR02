using DSW_3P_PR02.Models;
using DSW_3P_PR02.Repositories;

namespace DSW_3P_PR02.Services;

public sealed class PersonajeService(IPersonajeRepository repository) : IPersonajeService
{
    public Task<IReadOnlyList<Personaje>> ObtenerTodosAsync() =>
        repository.ObtenerTodosAsync();

    public Task<Personaje?> ObtenerPorIdAsync(int id) =>
        repository.ObtenerPorIdAsync(id);

    public Task<Personaje> CrearAsync(Personaje personaje) =>
        repository.CrearAsync(personaje);

    public Task<bool> ActualizarAsync(Personaje personaje) =>
        repository.ActualizarAsync(personaje);

    public Task<bool> EliminarAsync(int id) =>
        repository.EliminarAsync(id);
}
