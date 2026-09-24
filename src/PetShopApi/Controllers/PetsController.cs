using Microsoft.AspNetCore.Mvc;
using PetShopApi.Dtos;
using PetShopApi.Services;

namespace PetShopApi.Controllers;

/// <summary>Gerencia o cadastro de pets vinculados a um tutor.</summary>
[ApiController]
[Route("api/v1/pets")]
[Produces("application/json")]
public class PetsController : ControllerBase
{
    private readonly IPetService _service;

    public PetsController(IPetService service)
    {
        _service = service;
    }

    /// <summary>Lista pets, opcionalmente filtrando por tutor.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PetResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PetResponse>>> GetAll([FromQuery] int? tutorId)
    {
        return Ok(await _service.GetAllAsync(tutorId));
    }

    /// <summary>Busca um pet pelo id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PetResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PetResponse>> GetById(int id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>Cadastra um novo pet.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PetResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PetResponse>> Create(PetRequest request)
    {
        var created = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Atualiza os dados de um pet.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, PetRequest request)
    {
        await _service.UpdateAsync(id, request);
        return NoContent();
    }

    /// <summary>Remove um pet.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
