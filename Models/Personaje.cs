using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR02.Models;

public class Personaje
{
    public int Id { get; set; }

    [Required]
    [MaxLength(120)]
    public string Nombre { get; set; } = string.Empty;

    [MaxLength(120)]
    public string? Anime { get; set; }

    [MaxLength(50)]
    public string Tipo { get; set; } = "Secundario";

    [Range(0, 100)]
    public int Poder { get; set; }

    [MaxLength(120)]
    public string? Habilidad { get; set; }

    public bool Favorito { get; set; }
}
