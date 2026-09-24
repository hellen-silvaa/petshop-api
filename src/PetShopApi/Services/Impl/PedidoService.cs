using Microsoft.EntityFrameworkCore;
using PetShopApi.Data;
using PetShopApi.Dtos;
using PetShopApi.Entities;
using PetShopApi.Entities.Enums;
using PetShopApi.Exceptions;

namespace PetShopApi.Services.Impl;

public class PedidoService : IPedidoService
{
    private readonly AppDbContext _context;

    public PedidoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PedidoResponse>> GetAllAsync(int? tutorId)
    {
        var query = IncludeGraph(_context.Pedidos.AsQueryable());

        if (tutorId.HasValue)
        {
            query = query.Where(p => p.TutorId == tutorId.Value);
        }

        var pedidos = await query.OrderByDescending(p => p.DataPedido).ToListAsync();
        return pedidos.Select(ToResponse);
    }

    public async Task<PedidoResponse> GetByIdAsync(int id)
    {
        var pedido = await FindAsync(id);
        return ToResponse(pedido);
    }

    public async Task<PedidoResponse> CreateAsync(PedidoRequest request)
    {
        var tutorExists = await _context.Tutores.AnyAsync(t => t.Id == request.TutorId);
        if (!tutorExists)
        {
            throw new NotFoundException($"Tutor com id {request.TutorId} nao encontrado.");
        }

        var pedido = new Pedido
        {
            TutorId = request.TutorId,
            DataPedido = DateTime.UtcNow,
            Status = StatusPedido.Pendente
        };

        foreach (var itemRequest in request.Itens)
        {
            var produto = await _context.Produtos.FindAsync(itemRequest.ProdutoId)
                ?? throw new NotFoundException($"Produto com id {itemRequest.ProdutoId} nao encontrado.");

            if (produto.EstoqueQuantidade < itemRequest.Quantidade)
            {
                throw new BusinessRuleException(
                    $"Estoque insuficiente para o produto '{produto.Nome}'. Disponivel: {produto.EstoqueQuantidade}.");
            }

            produto.EstoqueQuantidade -= itemRequest.Quantidade;

            pedido.Itens.Add(new PedidoItem
            {
                ProdutoId = produto.Id,
                Quantidade = itemRequest.Quantidade,
                PrecoUnitario = produto.Preco
            });
        }

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        var pedidoCriado = await FindAsync(pedido.Id);
        return ToResponse(pedidoCriado);
    }

    public async Task UpdateStatusAsync(int id, PedidoStatusUpdateRequest request)
    {
        var pedido = await FindAsync(id);

        if (request.Status == StatusPedido.Cancelado && pedido.Status != StatusPedido.Cancelado)
        {
            foreach (var item in pedido.Itens)
            {
                var produto = await _context.Produtos.FindAsync(item.ProdutoId);
                if (produto is not null)
                {
                    produto.EstoqueQuantidade += item.Quantidade;
                }
            }
        }

        pedido.Status = request.Status;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var pedido = await FindAsync(id);

        _context.Pedidos.Remove(pedido);
        await _context.SaveChangesAsync();
    }

    private async Task<Pedido> FindAsync(int id)
    {
        return await IncludeGraph(_context.Pedidos.AsQueryable()).FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new NotFoundException($"Pedido com id {id} nao encontrado.");
    }

    private static IQueryable<Pedido> IncludeGraph(IQueryable<Pedido> query) =>
        query.Include(p => p.Tutor)
            .Include(p => p.Itens)
            .ThenInclude(i => i.Produto);

    private static PedidoResponse ToResponse(Pedido pedido)
    {
        var itens = pedido.Itens.Select(i => new PedidoItemResponse(
            i.ProdutoId,
            i.Produto?.Nome ?? string.Empty,
            i.Quantidade,
            i.PrecoUnitario,
            i.PrecoUnitario * i.Quantidade
        )).ToList();

        var total = itens.Sum(i => i.Subtotal);

        return new PedidoResponse(
            pedido.Id,
            pedido.TutorId,
            pedido.Tutor?.Nome,
            pedido.DataPedido,
            pedido.Status,
            itens,
            total
        );
    }
}
