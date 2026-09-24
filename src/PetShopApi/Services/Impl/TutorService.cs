using Microsoft.EntityFrameworkCore;
using PetShopApi.Data;
using PetShopApi.Dtos;
using PetShopApi.Entities;
using PetShopApi.Exceptions;

namespace PetShopApi.Services.Impl;

public class TutorService : ITutorService
{
    private readonly AppDbContext _context;

    public TutorService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TutorResponse>> GetAllAsync()
    {
        var tutores = await _context.Tutores.OrderBy(t => t.Nome).ToListAsync();
        return tutores.Select(ToResponse);
    }

    public async Task<TutorResponse> GetByIdAsync(int id)
    {
        var tutor = await FindAsync(id);
        return ToResponse(tutor);
    }

    public async Task<TutorResponse> CreateAsync(TutorRequest request)
    {
        var tutor = new Tutor
        {
            Nome = request.Nome,
            Email = request.Email,
            Telefone = request.Telefone
        };

        _context.Tutores.Add(tutor);
        await _context.SaveChangesAsync();

        return ToResponse(tutor);
    }

    public async Task UpdateAsync(int id, TutorRequest request)
    {
        var tutor = await FindAsync(id);

        tutor.Nome = request.Nome;
        tutor.Email = request.Email;
        tutor.Telefone = request.Telefone;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var tutor = await FindAsync(id);

        _context.Tutores.Remove(tutor);
        await _context.SaveChangesAsync();
    }

    private async Task<Tutor> FindAsync(int id)
    {
        return await _context.Tutores.FindAsync(id)
            ?? throw new NotFoundException($"Tutor com id {id} nao encontrado.");
    }

    private static TutorResponse ToResponse(Tutor tutor) =>
        new(tutor.Id, tutor.Nome, tutor.Email, tutor.Telefone);
}
