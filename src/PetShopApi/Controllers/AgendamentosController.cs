using Microsoft.AspNetCore.Mvc;
using PetShopApi.Dtos;
using PetShopApi.Services;

namespace PetShopApi.Controllers;

/// <summary>Gerencia agendamentos de serviços (banho, tosa, consulta etc.) para os pets.</summary>
[ApiController]
[Route("api/v1/agendamentos")]
[Produces("application/json")]
public class AgendamentosController : ControllerBase
{
    private readonly IAgendamentoService _service;

    public AgendamentosController(IAgendamentoService service)
    {
        _service = service;
    }

    /// <summary>Lista agendamentos, opcionalmente filtrando por pet.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AgendamentoResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<AgendamentoResponse>>> GetAll([FromQuery] int? petId)
    {
        return Ok(await _service.GetAllAsync(petId));
    }

    /// <summary>Busca um agendamento pelo id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(AgendamentoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AgendamentoResponse>> GetById(int id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>Cria um novo agendamento.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(AgendamentoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<AgendamentoResponse>> Create(AgendamentoRequest request)
    {
        var created = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Atualiza os dados de um agendamento.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, AgendamentoRequest request)
    {
        await _service.UpdateAsync(id, request);
        return NoContent();
    }

    /// <summary>Atualiza apenas o status de um agendamento.</summary>
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(int id, AgendamentoStatusUpdateRequest request)
    {
        await _service.UpdateStatusAsync(id, request);
        return NoContent();
    }

    /// <summary>Remove um agendamento.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
