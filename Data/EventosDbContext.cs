using DSW_3P_PR02.Models;
using Microsoft.EntityFrameworkCore;

namespace DSW_3P_PR02.Data;

public sealed class EventosDbContext(DbContextOptions<EventosDbContext> options) : DbContext(options)
{
    public DbSet<Evento> Eventos => Set<Evento>();
    public DbSet<Personaje> Personajes => Set<Personaje>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Evento>(entity =>
        {
            entity.HasKey(evento => evento.Id);
            entity.Property(evento => evento.Nombre).IsRequired().HasMaxLength(200);
            entity.Property(evento => evento.Categoria).HasMaxLength(100);
            entity.Property(evento => evento.Fecha).HasColumnType("date");
            entity.Property(evento => evento.Lugar).HasMaxLength(200);
        });
    }
}
