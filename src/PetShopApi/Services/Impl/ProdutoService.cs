using Microsoft.EntityFrameworkCore;
using PetShopApi.Data;
using PetShopApi.Dtos;
using PetShopApi.Entities;
using PetShopApi.Exceptions;

namespace PetShopApi.Services.Impl;

public class ProdutoService : IProdutoService
{
    private readonly AppDbContext _context;

    public ProdutoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProdutoResponse>> GetAllAsync()
    {
        var produtos = await _context.Produtos.OrderBy(p => p.Nome).ToListAsync();
        return produtos.Select(ToResponse);
    }

    public async Task<ProdutoResponse> GetByIdAsync(int id)
    {
        var produto = await FindAsync(id);
        return ToResponse(produto);
    }

    public async Task<ProdutoResponse> CreateAsync(ProdutoRequest request)
    {
        var produto = new Produto
        {
            Nome = request.Nome,
            Descricao = request.Descricao,
            Preco = request.Preco,
            EstoqueQuantidade = request.EstoqueQuantidade
        };

        _context.Produtos.Add(produto);
        await _context.SaveChangesAsync();

        return ToResponse(produto);
    }

    public async Task UpdateAsync(int id, ProdutoRequest request)
    {
        var produto = await FindAsync(id);

        produto.Nome = request.Nome;
        produto.Descricao = request.Descricao;
        produto.Preco = request.Preco;
        produto.EstoqueQuantidade = request.EstoqueQuantidade;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var produto = await FindAsync(id);

        _context.Produtos.Remove(produto);
        await _context.SaveChangesAsync();
    }

    private async Task<Produto> FindAsync(int id)
    {
        return await _context.Produtos.FindAsync(id)
            ?? throw new NotFoundException($"Produto com id {id} nao encontrado.");
    }

    private static ProdutoResponse ToResponse(Produto produto) =>
        new(produto.Id, produto.Nome, produto.Descricao, produto.Preco, produto.EstoqueQuantidade);
}
