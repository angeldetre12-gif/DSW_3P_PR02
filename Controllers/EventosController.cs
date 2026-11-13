using DSW_3P_PR02.Models;
using DSW_3P_PR02.Services;
using Microsoft.AspNetCore.Mvc;

namespace DSW_3P_PR02.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EventosController(IEventoService service) : ControllerBase
{

    /// TRAE LOS EVENTOS QUE SE CREARON POR ID 
    
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<Evento>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<Evento>>> ObtenerTodos()
    {
        return Ok(await service.ObtenerTodosAsync());
    }

   
    /// Obtiene un evento por su ID. Devuelve 404 si no se encuentra el evento.
   
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(Evento), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<Evento>> ObtenerPorId(int id)
    {
        var evento = await service.ObtenerPorIdAsync(id);
        return evento is null ? NotFound() : Ok(evento);
    }

    /// CREA UN NUEVO EVENTO Y DEVUELVE EL EVENTO CREADO CON SU ID
    [HttpPost]
    [ProducesResponseType(typeof(Evento), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Evento>> Crear(Evento evento)
    {
        var creado = await service.CrearAsync(evento);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }

    
    /// CREA UN METODO PARA ACTUALIZAR UN EVENTO EXISTENTE Y SINO EXITE EL EVTO DA UN ERRO DE 404
  
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(int id, Evento evento)
    {
        if (id != evento.Id)
        {
            return BadRequest("El ID DEL EVENTO TIENE QUE COICIDRI CON LA RUTA O EL NOMBRE .");
        }

        return await service.ActualizarAsync(evento) ? NoContent() : NotFound();
    }

    ///ELIMINA UN EVENTO POR SU ID
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id)
    {
        return await service.EliminarAsync(id) ? NoContent() : NotFound();
    }
}
