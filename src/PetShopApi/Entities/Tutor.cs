namespace PetShopApi.Entities;

public class Tutor
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Telefone { get; set; } = string.Empty;

    public ICollection<Pet> Pets { get; set; } = new List<Pet>();
    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
