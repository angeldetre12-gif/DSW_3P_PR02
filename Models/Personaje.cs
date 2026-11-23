using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR02.Models;

public class Personaje
{
    public int Id { get; set; }

    [Required, MaxLength(120)]
    public string Nombre { get; set; } = string.Empty;

    [Required, MaxLength(120)]
    public string Videojuego { get; set; } = string.Empty;

    [Required, MaxLength(60)]
    public string Clase { get; set; } = string.Empty;

    [Range(1, 100)]
    public int Nivel { get; set; }

    [Range(1, int.MaxValue)]
    public int Poder { get; set; }

    public bool Disponible { get; set; } = true;

    [Required, MaxLength(2048), AbsoluteImageUrl]
    public string ImagenUrl { get; set; } = string.Empty;
}

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter)]
public sealed class AbsoluteImageUrlAttribute : ValidationAttribute
{
    public AbsoluteImageUrlAttribute()
        : base("La URL de imagen debe ser absoluta y utilizar HTTP o HTTPS.")
    {
    }

    public override bool IsValid(object? value)
    {
        if (value is not string url || string.IsNullOrWhiteSpace(url))
        {
            return true;
        }

        return Uri.TryCreate(url, UriKind.Absolute, out var uri)
            && (uri.Scheme == Uri.UriSchemeHttp || uri.Scheme == Uri.UriSchemeHttps);
    }
}
