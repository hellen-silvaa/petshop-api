using Microsoft.AspNetCore.Mvc;
using PetShopApi.Dtos;
using PetShopApi.Services;

namespace PetShopApi.Controllers;

/// <summary>Gerencia o catálogo de produtos vendidos na loja.</summary>
[ApiController]
[Route("api/v1/produtos")]
[Produces("application/json")]
public class ProdutosController : ControllerBase
{
    private readonly IProdutoService _service;

    public ProdutosController(IProdutoService service)
    {
        _service = service;
    }

    /// <summary>Lista todos os produtos do catálogo.</summary>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<ProdutoResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<ProdutoResponse>>> GetAll()
    {
        return Ok(await _service.GetAllAsync());
    }

    /// <summary>Busca um produto pelo id.</summary>
    [HttpGet("{id:int}")]
    [ProducesResponseType(typeof(ProdutoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ProdutoResponse>> GetById(int id)
    {
        return Ok(await _service.GetByIdAsync(id));
    }

    /// <summary>Cadastra um novo produto.</summary>
    [HttpPost]
    [ProducesResponseType(typeof(ProdutoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ProdutoResponse>> Create(ProdutoRequest request)
    {
        var created = await _service.CreateAsync(request);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    /// <summary>Atualiza os dados de um produto.</summary>
    [HttpPut("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Update(int id, ProdutoRequest request)
    {
        await _service.UpdateAsync(id, request);
        return NoContent();
    }

    /// <summary>Remove um produto.</summary>
    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete(int id)
    {
        await _service.DeleteAsync(id);
        return NoContent();
    }
}
