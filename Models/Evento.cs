using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR02.Models;

public class Evento
{
    public int Id { get; set; }

    
    public string Nombre { get; set; } = string.Empty;

    public string? Categoria { get; set; }

   
    public DateOnly Fecha { get; set; }

    public string? Lugar { get; set; }

    
    public int CupoMaximo { get; set; }

    public bool Activo { get; set; }
}


