using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR02.Models;

public class Evento
{
    public int Id { get; set; }

    [Required]
    public string Nombre { get; set; } = string.Empty;

    public string? Categoria { get; set; }

    [FechaValida]
    public DateOnly Fecha { get; set; }

    public string? Lugar { get; set; }

    [Range(1, int.MaxValue)]
    public int CupoMaximo { get; set; }

    public bool Activo { get; set; }
}

public sealed class FechaValidaAttribute : ValidationAttribute
{
    public override bool IsValid(object? value) =>
        value is DateOnly fecha && fecha != default;
}

