using System.ComponentModel.DataAnnotations;

namespace DSW_3P_PR02.Models;

/// <summary>Credenciales del usuario de demostración.</summary>
public sealed record LoginRequest
{
    [Required]
    public required string Usuario { get; init; }

    [Required]
    public required string Contrasena { get; init; }
}

/// <summary>Token de acceso y su fecha de expiración.</summary>
public sealed record LoginResponse(string AccessToken, string TokenType, DateTimeOffset ExpiresAt);
