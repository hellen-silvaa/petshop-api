using Microsoft.EntityFrameworkCore;
using PetShopApi.Data;
using PetShopApi.Dtos;
using PetShopApi.Entities;
using PetShopApi.Exceptions;

namespace PetShopApi.Services.Impl;

public class PetService : IPetService
{
    private readonly AppDbContext _context;

    public PetService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PetResponse>> GetAllAsync(int? tutorId)
    {
        var query = _context.Pets.Include(p => p.Tutor).AsQueryable();

        if (tutorId.HasValue)
        {
            query = query.Where(p => p.TutorId == tutorId.Value);
        }

        var pets = await query.OrderBy(p => p.Nome).ToListAsync();
        return pets.Select(ToResponse);
    }

    public async Task<PetResponse> GetByIdAsync(int id)
    {
        var pet = await FindAsync(id);
        return ToResponse(pet);
    }

    public async Task<PetResponse> CreateAsync(PetRequest request)
    {
        await EnsureTutorExistsAsync(request.TutorId);

        var pet = new Pet
        {
            Nome = request.Nome,
            Especie = request.Especie,
            Raca = request.Raca,
            DataNascimento = request.DataNascimento,
            TutorId = request.TutorId
        };

        _context.Pets.Add(pet);
        await _context.SaveChangesAsync();

        await _context.Entry(pet).Reference(p => p.Tutor).LoadAsync();
        return ToResponse(pet);
    }

    public async Task UpdateAsync(int id, PetRequest request)
    {
        var pet = await FindAsync(id);
        await EnsureTutorExistsAsync(request.TutorId);

        pet.Nome = request.Nome;
        pet.Especie = request.Especie;
        pet.Raca = request.Raca;
        pet.DataNascimento = request.DataNascimento;
        pet.TutorId = request.TutorId;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var pet = await FindAsync(id);

        _context.Pets.Remove(pet);
        await _context.SaveChangesAsync();
    }

    private async Task<Pet> FindAsync(int id)
    {
        return await _context.Pets.Include(p => p.Tutor).FirstOrDefaultAsync(p => p.Id == id)
            ?? throw new NotFoundException($"Pet com id {id} nao encontrado.");
    }

    private async Task EnsureTutorExistsAsync(int tutorId)
    {
        var exists = await _context.Tutores.AnyAsync(t => t.Id == tutorId);
        if (!exists)
        {
            throw new NotFoundException($"Tutor com id {tutorId} nao encontrado.");
        }
    }

    private static PetResponse ToResponse(Pet pet) =>
        new(pet.Id, pet.Nome, pet.Especie, pet.Raca, pet.DataNascimento, pet.TutorId, pet.Tutor?.Nome);
}
