using DSW_3P_PR02.Data;
using DSW_3P_PR02.Models;
using Microsoft.EntityFrameworkCore;

namespace DSW_3P_PR02.Repositories;

public sealed class SqlPersonajeRepository(PersonajesDbContext dbContext) : IPersonajeRepository
{
    public async Task<IReadOnlyList<Personaje>> ObtenerTodosAsync(CancellationToken cancellationToken) =>
        await dbContext.Personajes.AsNoTracking().OrderBy(personaje => personaje.Id)
            .ToListAsync(cancellationToken);

    public Task<Personaje?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken) =>
        dbContext.Personajes.AsNoTracking()
            .FirstOrDefaultAsync(personaje => personaje.Id == id, cancellationToken);

    public async Task<IReadOnlyList<Personaje>> ObtenerPorVideojuegoAsync(
        string nombre,
        CancellationToken cancellationToken) =>
        await dbContext.Personajes.AsNoTracking()
            .Where(personaje => personaje.Videojuego == nombre)
            .OrderBy(personaje => personaje.Id)
            .ToListAsync(cancellationToken);

    public async Task<IReadOnlyList<Personaje>> ObtenerPorNivelAsync(
        int nivel,
        CancellationToken cancellationToken) =>
        await dbContext.Personajes.AsNoTracking()
            .Where(personaje => personaje.Nivel >= nivel)
            .OrderBy(personaje => personaje.Id)
            .ToListAsync(cancellationToken);

    public async Task<Personaje> CrearAsync(Personaje personaje, CancellationToken cancellationToken)
    {
        dbContext.Personajes.Add(personaje);
        await dbContext.SaveChangesAsync(cancellationToken);
        return personaje;
    }

    public async Task<bool> ActualizarAsync(Personaje personaje, CancellationToken cancellationToken)
    {
        var existente = await dbContext.Personajes
            .FirstOrDefaultAsync(item => item.Id == personaje.Id, cancellationToken);
        if (existente is null)
        {
            return false;
        }

        dbContext.Entry(existente).CurrentValues.SetValues(personaje);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> EliminarAsync(int id, CancellationToken cancellationToken)
    {
        var personaje = await dbContext.Personajes
            .FirstOrDefaultAsync(item => item.Id == id, cancellationToken);
        if (personaje is null)
        {
            return false;
        }

        dbContext.Personajes.Remove(personaje);
        await dbContext.SaveChangesAsync(cancellationToken);
        return true;
    }
}
