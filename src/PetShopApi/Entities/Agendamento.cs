using PetShopApi.Entities.Enums;

namespace PetShopApi.Entities;

public class Agendamento
{
    public int Id { get; set; }

    public int PetId { get; set; }
    public Pet? Pet { get; set; }

    public TipoServico TipoServico { get; set; }
    public DateTime DataHora { get; set; }
    public StatusAgendamento Status { get; set; } = StatusAgendamento.Agendado;
    public string? Observacoes { get; set; }
}
