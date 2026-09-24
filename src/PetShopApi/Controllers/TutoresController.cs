using Microsoft.AspNetCore.Mvc;
using PetShopApi.Dtos;
using PetShopApi.Services;

namespace PetShopApi.Controllers;

/// <summary>Gerencia o cadastro de tutores (clientes).</summary>
[ApiController]
[Route("api/v1/tutores")]
[Produces("application/json")]
public class TutoresController : ControllerBase
{
    private readonly ITutorService _service;

    public TutoresController(ITutorService service)
    {
        _service = service;
    }

    /// <summary>Lista todos os tutores cadastrados.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<TutorResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<TutorResponse>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    /// <summary>Busca um tutor pelo id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(TutorResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TutorResponse>> GetById(int id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>Cadastra um novo tutor.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(TutorResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TutorResponse>> Create(TutorRequest request)
    {
        var created = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Atualiza os dados de um tutor.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, TutorRequest request)
    {
        await _service.UpdateAsync(id, request);
        return NoContent();
    }

    /// <summary>Remove um tutor.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
