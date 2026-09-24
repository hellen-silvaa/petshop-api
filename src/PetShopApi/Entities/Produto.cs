namespace PetShopApi.Entities;

public class Produto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public int EstoqueQuantidade { get; set; }

    public ICollection<PedidoItem> PedidoItens { get; set; } = new List<PedidoItem>();
}
