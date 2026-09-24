using System.ComponentModel.DataAnnotations;

namespace PetShopApi.Dtos;

public record TutorResponse(int Id, string Nome, string Email, string Telefone);

public record TutorRequest(
    [Required, MaxLength(120)] string Nome,
    [Required, EmailAddress, MaxLength(160)] string Email,
    [Required, MaxLength(20)] string Telefone
);
