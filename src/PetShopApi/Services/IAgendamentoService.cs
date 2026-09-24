using PetShopApi.Dtos;

namespace PetShopApi.Services;

public interface IAgendamentoService
{
    Task<IEnumerable<AgendamentoResponse>> GetAllAsync(int? petId);
    Task<AgendamentoResponse> GetByIdAsync(int id);
    Task<AgendamentoResponse> CreateAsync(AgendamentoRequest request);
    Task UpdateAsync(int id, AgendamentoRequest request);
    Task UpdateStatusAsync(int id, AgendamentoStatusUpdateRequest request);
    Task DeleteAsync(int id);
}
