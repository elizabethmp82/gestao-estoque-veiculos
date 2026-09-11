using EstoqueVeiculos.Api.DTOs;
using EstoqueVeiculos.Api.Services;
using Microsoft.AspNetCore.Mvc;
using EstoqueVeiculos.Api.Exceptions;

namespace EstoqueVeiculos.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VeiculosController : ControllerBase
{
    //private readonly VeiculoRepository _repository;
    private readonly VeiculoService _service;

  public VeiculosController(VeiculoService service)
{
    _service = service;
}

      [HttpGet]
public async Task<IActionResult> Listar(
    [FromQuery] string? marca,
    [FromQuery] string? situacao)
{
    var veiculos = await _service.ListarAsync(marca, situacao);

    return Ok(veiculos);
}

    [HttpGet("{id:int}")]
public async Task<IActionResult> BuscarPorId(int id)
{
    var veiculo = await _service.BuscarPorIdAsync(id);

    if (veiculo is null)
    {
        return NotFound(new
        {
            mensagem = "Veículo não encontrado."
        });
    }

    return Ok(veiculo);
}

   [HttpPost]
   public async Task<IActionResult> Criar(VeiculoCreateDto dto)
{
    await _service.CriarAsync(dto);

    return Created("/api/veiculos",
        new { mensagem = "Veículo cadastrado com sucesso." });
}



[HttpPut("{id:int}")]
public async Task<IActionResult> Atualizar(int id, VeiculoUpdateDto dto)
{
    await _service.AtualizarAsync(id, dto);

    return Ok(new
    {
        mensagem = "Veículo atualizado com sucesso."
    });
}


[HttpDelete("{id:int}")]
public async Task<IActionResult> Excluir(int id)
{
        await _service.ExcluirAsync(id);

        return Ok(new
        {
            mensagem = "Veículo excluído com sucesso."
        });
}




}