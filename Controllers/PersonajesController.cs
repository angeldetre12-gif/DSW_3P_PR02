using DSW_3P_PR02.Data;
using DSW_3P_PR02.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DSW_3P_PR02.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonajesController(EventosDbContext dbContext) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<Personaje>>> ObtenerTodos()
    {
        return Ok(await dbContext.Personajes.AsNoTracking().ToListAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Personaje>> ObtenerPorId(int id)
    {
        var personaje = await dbContext.Personajes.FindAsync(id);
        return personaje is null ? NotFound() : Ok(personaje);
    }

    [HttpPost]
    public async Task<ActionResult<Personaje>> Crear(Personaje personaje)
    {
        dbContext.Personajes.Add(personaje);
        await dbContext.SaveChangesAsync();
        return CreatedAtAction(nameof(ObtenerPorId), new { id = personaje.Id }, personaje);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(int id, Personaje personaje)
    {
        if (id != personaje.Id) return BadRequest();
        if (!await dbContext.Personajes.AnyAsync(p => p.Id == id)) return NotFound();
        dbContext.Entry(personaje).State = EntityState.Modified;
        await dbContext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var personaje = await dbContext.Personajes.FindAsync(id);
        if (personaje is null) return NotFound();
        dbContext.Personajes.Remove(personaje);
        await dbContext.SaveChangesAsync();
        return NoContent();
    }
}
