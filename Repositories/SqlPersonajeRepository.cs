using DSW_3P_PR02.Data;
using DSW_3P_PR02.Models;
using Microsoft.EntityFrameworkCore;

namespace DSW_3P_PR02.Repositories;

public sealed class SqlPersonajeRepository(PersonajesDbContext dbContext) : IPersonajeRepository
{
    public async Task<IReadOnlyList<Personaje>> ObtenerTodosAsync() =>
        await dbContext.Personajes.AsNoTracking().OrderBy(personaje => personaje.Id).ToListAsync();

    public Task<Personaje?> ObtenerPorIdAsync(int id) =>
        dbContext.Personajes.AsNoTracking().FirstOrDefaultAsync(personaje => personaje.Id == id);

    public async Task<Personaje> CrearAsync(Personaje personaje)
    {
        dbContext.Personajes.Add(personaje);
        await dbContext.SaveChangesAsync();
        return personaje;
    }

    public async Task<bool> ActualizarAsync(Personaje personaje)
    {
        var existente = await dbContext.Personajes.FindAsync(personaje.Id);
        if (existente is null)
        {
            return false;
        }

        dbContext.Entry(existente).CurrentValues.SetValues(personaje);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var personaje = await dbContext.Personajes.FindAsync(id);
        if (personaje is null)
        {
            return false;
        }

        dbContext.Personajes.Remove(personaje);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
