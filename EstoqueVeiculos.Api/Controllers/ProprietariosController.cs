using EstoqueVeiculos.Api.DTOs;
using EstoqueVeiculos.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace EstoqueVeiculos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProprietariosController : ControllerBase
{
    private readonly ProprietarioService _service;

    public ProprietariosController(ProprietarioService service)
    {
        _service = service;
    }

    [HttpGet("veiculo/{veiculoId:int}")]
    public async Task<IActionResult> ListarPorVeiculo(int veiculoId)
    {
        var proprietarios =
            await _service.ListarPorVeiculoAsync(veiculoId);

        return Ok(proprietarios);
    }

    [HttpPost]
    public async Task<IActionResult> Criar(ProprietarioCreateDto dto)
    {
        try
        {
            await _service.CriarAsync(dto);

            return Created(
                "/api/proprietarios",
                new { mensagem = "Proprietário cadastrado com sucesso." }
            );
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { mensagem = ex.Message });
        }
    }

    [HttpPut("{id:int}")]
public async Task<IActionResult> Atualizar(
    int id,
    ProprietarioUpdateDto dto)
{
    try
    {
        await _service.AtualizarAsync(id, dto);

        return Ok(new
        {
            mensagem = "Proprietário atualizado com sucesso."
        });
    }
    catch (ArgumentException ex)
    {
        return BadRequest(new
        {
            mensagem = ex.Message
        });
    }
}

[HttpDelete("{id:int}")]
public async Task<IActionResult> Excluir(int id)
{
    try
    {
        await _service.ExcluirAsync(id);

        return Ok(new
        {
            mensagem = "Proprietário excluído com sucesso."
        });
    }
    catch (ArgumentException ex)
    {
        return BadRequest(new
        {
            mensagem = ex.Message
        });
    }
}
}