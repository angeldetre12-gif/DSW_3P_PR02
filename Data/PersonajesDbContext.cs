using DSW_3P_PR02.Models;
using Microsoft.EntityFrameworkCore;

namespace DSW_3P_PR02.Data;

public sealed class PersonajesDbContext(DbContextOptions<PersonajesDbContext> options) : DbContext(options)
{
    public DbSet<Personaje> Personajes => Set<Personaje>();
}
