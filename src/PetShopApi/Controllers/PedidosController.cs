using Microsoft.AspNetCore.Mvc;
using PetShopApi.Dtos;
using PetShopApi.Services;

namespace PetShopApi.Controllers;

/// <summary>Gerencia pedidos de produtos feitos pelos tutores, com baixa automática de estoque.</summary>
[ApiController]
[Route("api/v1/pedidos")]
[Produces("application/json")]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _service;

    public PedidosController(IPedidoService service)
    {
        _service = service;
    }

    /// <summary>Lista pedidos, opcionalmente filtrando por tutor.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PedidoResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<PedidoResponse>>> GetAll([FromQuery] int? tutorId)
    {
        return Ok(await _service.GetAllAsync(tutorId));
    }

    /// <summary>Busca um pedido pelo id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PedidoResponse>> GetById(int id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>Cria um novo pedido, dando baixa no estoque dos produtos.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(PedidoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<PedidoResponse>> Create(PedidoRequest request)
    {
        var created = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Atualiza o status de um pedido (cancelar restaura o estoque).</summary>
    [HttpPatch("{id:int}/status")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> UpdateStatus(int id, PedidoStatusUpdateRequest request)
    {
        await _service.UpdateStatusAsync(id, request);
        return NoContent();
    }

    /// <summary>Remove um pedido.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
