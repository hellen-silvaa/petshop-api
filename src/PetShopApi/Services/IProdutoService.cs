using PetShopApi.Dtos;

namespace PetShopApi.Services;

public interface IProdutoService
{
    Task<IEnumerable<ProdutoResponse>> GetAllAsync();
    Task<ProdutoResponse> GetByIdAsync(int id);
    Task<ProdutoResponse> CreateAsync(ProdutoRequest request);
    Task UpdateAsync(int id, ProdutoRequest request);
    Task DeleteAsync(int id);
}
