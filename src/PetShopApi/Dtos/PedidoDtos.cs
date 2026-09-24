using System.ComponentModel.DataAnnotations;
using PetShopApi.Entities.Enums;

namespace PetShopApi.Dtos;

public record PedidoItemRequest(
    [Required] int ProdutoId,
    [Range(1, int.MaxValue)] int Quantidade
);

public record PedidoItemResponse(
    int ProdutoId,
    string ProdutoNome,
    int Quantidade,
    decimal PrecoUnitario,
    decimal Subtotal
);

public record PedidoRequest(
    [Required] int TutorId,
    [Required, MinLength(1)] List<PedidoItemRequest> Itens
);

public record PedidoResponse(
    int Id,
    int TutorId,
    string? TutorNome,
    DateTime DataPedido,
    StatusPedido Status,
    List<PedidoItemResponse> Itens,
    decimal Total
);

public record PedidoStatusUpdateRequest([Required] StatusPedido Status);
