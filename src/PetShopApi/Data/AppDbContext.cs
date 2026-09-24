using Microsoft.EntityFrameworkCore;
using PetShopApi.Entities;

namespace PetShopApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Tutor> Tutores => Set<Tutor>();
    public DbSet<Pet> Pets => Set<Pet>();
    public DbSet<Agendamento> Agendamentos => Set<Agendamento>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<PedidoItem> PedidoItens => Set<PedidoItem>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Tutor>(entity =>
        {
            entity.HasIndex(t => t.Email).IsUnique();
            entity.Property(t => t.Nome).HasMaxLength(120).IsRequired();
            entity.Property(t => t.Email).HasMaxLength(160).IsRequired();
            entity.Property(t => t.Telefone).HasMaxLength(20).IsRequired();
        });

        modelBuilder.Entity<Pet>(entity =>
        {
            entity.Property(p => p.Nome).HasMaxLength(80).IsRequired();
            entity.Property(p => p.Especie).HasMaxLength(40).IsRequired();
            entity.Property(p => p.Raca).HasMaxLength(40);

            entity.HasOne(p => p.Tutor)
                .WithMany(t => t.Pets)
                .HasForeignKey(p => p.TutorId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Agendamento>(entity =>
        {
            entity.Property(a => a.Observacoes).HasMaxLength(300);

            entity.HasOne(a => a.Pet)
                .WithMany(p => p.Agendamentos)
                .HasForeignKey(a => a.PetId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.Property(p => p.Nome).HasMaxLength(120).IsRequired();
            entity.Property(p => p.Descricao).HasMaxLength(400);
            entity.Property(p => p.Preco).HasColumnType("decimal(10,2)");
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasOne(p => p.Tutor)
                .WithMany(t => t.Pedidos)
                .HasForeignKey(p => p.TutorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<PedidoItem>(entity =>
        {
            entity.Property(i => i.PrecoUnitario).HasColumnType("decimal(10,2)");

            entity.HasOne(i => i.Pedido)
                .WithMany(p => p.Itens)
                .HasForeignKey(i => i.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(i => i.Produto)
                .WithMany(p => p.PedidoItens)
                .HasForeignKey(i => i.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
