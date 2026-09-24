using Microsoft.EntityFrameworkCore;
using PetShopApi.Data;
using PetShopApi.Dtos;
using PetShopApi.Entities;
using PetShopApi.Exceptions;

namespace PetShopApi.Services.Impl;

public class AgendamentoService : IAgendamentoService
{
    private readonly AppDbContext _context;

    public AgendamentoService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<AgendamentoResponse>> GetAllAsync(int? petId)
    {
        var query = _context.Agendamentos.Include(a => a.Pet).AsQueryable();

        if (petId.HasValue)
        {
            query = query.Where(a => a.PetId == petId.Value);
        }

        var agendamentos = await query.OrderBy(a => a.DataHora).ToListAsync();
        return agendamentos.Select(ToResponse);
    }

    public async Task<AgendamentoResponse> GetByIdAsync(int id)
    {
        var agendamento = await FindAsync(id);
        return ToResponse(agendamento);
    }

    public async Task<AgendamentoResponse> CreateAsync(AgendamentoRequest request)
    {
        await EnsurePetExistsAsync(request.PetId);

        var agendamento = new Agendamento
        {
            PetId = request.PetId,
            TipoServico = request.TipoServico,
            DataHora = request.DataHora,
            Observacoes = request.Observacoes
        };

        _context.Agendamentos.Add(agendamento);
        await _context.SaveChangesAsync();

        await _context.Entry(agendamento).Reference(a => a.Pet).LoadAsync();
        return ToResponse(agendamento);
    }

    public async Task UpdateAsync(int id, AgendamentoRequest request)
    {
        var agendamento = await FindAsync(id);
        await EnsurePetExistsAsync(request.PetId);

        agendamento.PetId = request.PetId;
        agendamento.TipoServico = request.TipoServico;
        agendamento.DataHora = request.DataHora;
        agendamento.Observacoes = request.Observacoes;

        await _context.SaveChangesAsync();
    }

    public async Task UpdateStatusAsync(int id, AgendamentoStatusUpdateRequest request)
    {
        var agendamento = await FindAsync(id);
        agendamento.Status = request.Status;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var agendamento = await FindAsync(id);

        _context.Agendamentos.Remove(agendamento);
        await _context.SaveChangesAsync();
    }

    private async Task<Agendamento> FindAsync(int id)
    {
        return await _context.Agendamentos.Include(a => a.Pet).FirstOrDefaultAsync(a => a.Id == id)
            ?? throw new NotFoundException($"Agendamento com id {id} nao encontrado.");
    }

    private async Task EnsurePetExistsAsync(int petId)
    {
        var exists = await _context.Pets.AnyAsync(p => p.Id == petId);
        if (!exists)
        {
            throw new NotFoundException($"Pet com id {petId} nao encontrado.");
        }
    }

    private static AgendamentoResponse ToResponse(Agendamento agendamento) => new(
        agendamento.Id,
        agendamento.PetId,
        agendamento.Pet?.Nome,
        agendamento.TipoServico,
        agendamento.DataHora,
        agendamento.Status,
        agendamento.Observacoes
    );
}
