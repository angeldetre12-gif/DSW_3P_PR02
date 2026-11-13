using DSW_3P_PR02.Data;
using DSW_3P_PR02.Models;
using Microsoft.EntityFrameworkCore;

namespace DSW_3P_PR02.Repositories;

public sealed class SqlEventoRepository(EventosDbContext dbContext) : IEventoRepository
{
    public async Task<IReadOnlyList<Evento>> ObtenerTodosAsync() =>
        await dbContext.Eventos.AsNoTracking().OrderBy(evento => evento.Id).ToListAsync();

    public Task<Evento?> ObtenerPorIdAsync(int id) =>
        dbContext.Eventos.AsNoTracking().FirstOrDefaultAsync(evento => evento.Id == id);

    public async Task<Evento> CrearAsync(Evento evento)
    {
        dbContext.Eventos.Add(evento);
        await dbContext.SaveChangesAsync();
        return evento;
    }

    public async Task<bool> ActualizarAsync(Evento evento)
    {
        var existente = await dbContext.Eventos.FindAsync(evento.Id);
        if (existente is null)
        {
            return false;
        }

        dbContext.Entry(existente).CurrentValues.SetValues(evento);
        await dbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var evento = await dbContext.Eventos.FindAsync(id);
        if (evento is null)
        {
            return false;
        }

        dbContext.Eventos.Remove(evento);
        await dbContext.SaveChangesAsync();
        return true;
    }
}
