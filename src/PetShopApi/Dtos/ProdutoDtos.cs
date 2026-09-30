using System.ComponentModel.DataAnnotations;

namespace PetShopApi.Dtos;

public record ProdutoResponse(int Id, string Nome, string? Descricao, decimal Preco, int EstoqueQuantidade);


public record ProdutoRequest(
    [Required, MaxLength(120)] string Nome,
    [MaxLength(400)] string? Descricao,
    [Range(0.01, double.MaxValue)] decimal Preco,
    [Range(0, int.MaxValue)] int EstoqueQuantidade
);
