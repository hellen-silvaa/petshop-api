using PetShopApi.Entities.Enums;

namespace PetShopApi.Entities;

public class Pedido
{
    public int Id { get; set; }

    public int TutorId { get; set; }
    public Tutor? Tutor { get; set; }

    public DateTime DataPedido { get; set; } = DateTime.UtcNow;
    public StatusPedido Status { get; set; } = StatusPedido.Pendente;

    public ICollection<PedidoItem> Itens { get; set; } = new List<PedidoItem>();
}
