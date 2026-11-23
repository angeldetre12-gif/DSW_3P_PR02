using System.ComponentModel.DataAnnotations;
using DSW_3P_PR02.Models;
using DSW_3P_PR02.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DSW_3P_PR02.Controllers;

[ApiController]
[Authorize]
[Route("api/personajes")]
public sealed class PersonajesController(IPersonajeService service) : ControllerBase
{
    /// <summary>Obtiene todos los personajes registrados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IReadOnlyList<PersonajeResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PersonajeResponse>>> ObtenerTodos(
        CancellationToken cancellationToken) =>
        Ok(await service.ObtenerTodosAsync(cancellationToken));

    /// <summary>Obtiene un personaje por su identificador.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PersonajeResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PersonajeResponse>> ObtenerPorId(
        int id,
        CancellationToken cancellationToken)
    {
        var personaje = await service.ObtenerPorIdAsync(id, cancellationToken);
        return personaje is null ? NotFound() : Ok(personaje);
    }

    /// <summary>Filtra personajes por el nombre de su videojuego.</summary>
    [HttpGet("videojuego/{nombre}")]
    [ProducesResponseType(typeof(IReadOnlyList<PersonajeResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IReadOnlyList<PersonajeResponse>>> ObtenerPorVideojuego(
        string nombre,
        CancellationToken cancellationToken) =>
        Ok(await service.ObtenerPorVideojuegoAsync(nombre, cancellationToken));

    /// <summary>Obtiene personajes cuyo nivel es igual o superior al indicado.</summary>
    [HttpGet("nivel/{nivel:int}")]
    [ProducesResponseType(typeof(IReadOnlyList<PersonajeResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IReadOnlyList<PersonajeResponse>>> ObtenerPorNivel(
        [Range(1, 100)] int nivel,
        CancellationToken cancellationToken) =>
        Ok(await service.ObtenerPorNivelAsync(nivel, cancellationToken));

    /// <summary>Registra un personaje nuevo, disponible de forma predeterminada.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PersonajeResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PersonajeResponse>> Crear(
        CrearPersonajeRequest request,
        CancellationToken cancellationToken)
    {
        var creado = await service.CrearAsync(request, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }

    /// <summary>Reemplaza los datos editables de un personaje.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Actualizar(
        int id,
        ActualizarPersonajeRequest request,
        CancellationToken cancellationToken)
    {
        var actualizado = await service.ActualizarAsync(id, request, cancellationToken);
        return actualizado ? NoContent() : NotFound();
    }

    /// <summary>Elimina un personaje por su identificador.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancellationToken) =>
        await service.EliminarAsync(id, cancellationToken) ? NoContent() : NotFound();
}
