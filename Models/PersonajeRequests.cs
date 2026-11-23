using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR02.Models;

/// <summary>Datos necesarios para registrar un personaje.</summary>
public sealed record CrearPersonajeRequest
{
    [Required, MaxLength(120)]
    public required string Nombre { get; init; }

    [Required, MaxLength(120)]
    public required string Videojuego { get; init; }

    [Required, MaxLength(60)]
    public required string Clase { get; init; }

    [Range(1, 100)]
    public int Nivel { get; init; }

    [Range(1, int.MaxValue)]
    public int Poder { get; init; }

    [Required, MaxLength(2048), AbsoluteImageUrl]
    public required string ImagenUrl { get; init; }
}

/// <summary>Datos para reemplazar los campos editables de un personaje.</summary>
public sealed record ActualizarPersonajeRequest
{
    [Required, MaxLength(120)]
    public required string Nombre { get; init; }

    [Required, MaxLength(120)]
    public required string Videojuego { get; init; }

    [Required, MaxLength(60)]
    public required string Clase { get; init; }

    [Range(1, 100)]
    public int Nivel { get; init; }

    [Range(1, int.MaxValue)]
    public int Poder { get; init; }

    public bool Disponible { get; init; }

    [Required, MaxLength(2048), AbsoluteImageUrl]
    public required string ImagenUrl { get; init; }
}

/// <summary>Representa un personaje devuelto por la API.</summary>
public sealed record PersonajeResponse(
    int Id,
    string Nombre,
    string Videojuego,
    string Clase,
    int Nivel,
    int Poder,
    bool Disponible,
    string ImagenUrl);
