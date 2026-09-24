using System.ComponentModel.DataAnnotations;
using PetShopApi.Entities.Enums;

namespace PetShopApi.Dtos;

public record AgendamentoResponse(
    int Id,
    int PetId,
    string? PetNome,
    TipoServico TipoServico,
    DateTime DataHora,
    StatusAgendamento Status,
    string? Observacoes
);

public record AgendamentoRequest(
    [Required] int PetId,
    [Required] TipoServico TipoServico,
    [Required] DateTime DataHora,
    [MaxLength(300)] string? Observacoes
);

public record AgendamentoStatusUpdateRequest([Required] StatusAgendamento Status);
