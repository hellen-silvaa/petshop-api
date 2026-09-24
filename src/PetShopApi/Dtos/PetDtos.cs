using System.ComponentModel.DataAnnotations;

namespace PetShopApi.Dtos;

public record PetResponse(
    int Id,
    string Nome,
    string Especie,
    string? Raca,
    DateOnly DataNascimento,
    int TutorId,
    string? TutorNome
);

public record PetRequest(
    [Required, MaxLength(80)] string Nome,
    [Required, MaxLength(40)] string Especie,
    [MaxLength(40)] string? Raca,
    [Required] DateOnly DataNascimento,
    [Required] int TutorId
);
