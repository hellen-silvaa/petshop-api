using PetShopApi.Dtos;

namespace PetShopApi.Services;

public interface IPetService
{
    Task<IEnumerable<PetResponse>> GetAllAsync(int? tutorId);
    Task<PetResponse> GetByIdAsync(int id);
    Task<PetResponse> CreateAsync(PetRequest request);
    Task UpdateAsync(int id, PetRequest request);
    Task DeleteAsync(int id);
}
