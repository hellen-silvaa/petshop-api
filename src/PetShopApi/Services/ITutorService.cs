using PetShopApi.Dtos;

namespace PetShopApi.Services;

public interface ITutorService
{
    Task<IEnumerable<TutorResponse>> GetAllAsync();
    Task<TutorResponse> GetByIdAsync(int id);
    Task<TutorResponse> CreateAsync(TutorRequest request);
    Task UpdateAsync(int id, TutorRequest request);
    Task DeleteAsync(int id);
}
