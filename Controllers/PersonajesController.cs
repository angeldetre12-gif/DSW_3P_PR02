using DSW_3P_PR02.Models;
using DSW_3P_PR02.Services;
using Microsoft.AspNetCore.Mvc;

namespace DSW_3P_PR02.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PersonajesController(IPersonajeService service) : ControllerBase
{
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<Personaje>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<Personaje>>> ObtenerTodos()
    {
        return Ok(await service.ObtenerTodosAsync());
    }

    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Personaje), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Personaje>> ObtenerPorId(int id)
    {
        var personaje = await service.ObtenerPorIdAsync(id);
        return personaje is null ? NotFound() : Ok(personaje);
    }

    [HttpPost]
    [ProducesResponseType(typeof(Personaje), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Personaje>> Crear(Personaje personaje)
    {
        var creado = await service.CrearAsync(personaje);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, Personaje personaje)
    {
        if (id != personaje.Id)
        {
            return BadRequest("El ID del personaje debe coincidir con el ID de la ruta.");
        }

        return await service.ActualizarAsync(personaje) ? NoContent() : NotFound();
    }

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
    {
        return await service.EliminarAsync(id) ? NoContent() : NotFound();
    }
}
