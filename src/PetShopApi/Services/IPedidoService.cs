using PetShopApi.Dtos;

namespace PetShopApi.Services;

public interface IPedidoService
{
    Task<IEnumerable<PedidoResponse>> GetAllAsync(int? tutorId);
    Task<PedidoResponse> GetByIdAsync(int id);
    Task<PedidoResponse> CreateAsync(PedidoRequest request);
    Task UpdateStatusAsync(int id, PedidoStatusUpdateRequest request);
    Task DeleteAsync(int id);
}
