using EstoqueVeiculos.Api.DTOs;
using EstoqueVeiculos.Api.Models;
using EstoqueVeiculos.Api.Repositories;

namespace EstoqueVeiculos.Api.Services;

public class VeiculoService
{
    private readonly VeiculoRepository _repository;

    public VeiculoService(VeiculoRepository repository)
    {
        _repository = repository;
    }

    public async Task CriarAsync(VeiculoCreateDto dto)
    {
        if (string.IsNullOrWhiteSpace(dto.Marca))
            throw new ArgumentException("A marca é obrigatória.");

        if (string.IsNullOrWhiteSpace(dto.Modelo))
            throw new ArgumentException("O modelo é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.Cor))
            throw new ArgumentException("A cor é obrigatória.");

        if (string.IsNullOrWhiteSpace(dto.Tipo))
            throw new ArgumentException("O tipo é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.Placa))
            throw new ArgumentException("A placa é obrigatória.");

        if (dto.Ano < 1900 || dto.Ano > DateTime.Now.Year)
            throw new ArgumentException("O ano do veículo é inválido.");

        if (dto.Preco < 0)
            throw new ArgumentException("O preço não pode ser negativo.");

        if (dto.Quilometragem < 0)
            throw new ArgumentException("A quilometragem não pode ser negativa.");

        if (await _repository.PlacaExisteAsync(dto.Placa))
            throw new ArgumentException("Já existe um veículo cadastrado com essa placa.");

        var veiculo = new Veiculo
        {
            Marca = dto.Marca,
            Modelo = dto.Modelo,
            Ano = dto.Ano,
            Cor = dto.Cor,
            Preco = dto.Preco,
            Tipo = dto.Tipo,
            Situacao = "Disponível",
            Placa = dto.Placa,
            Quilometragem = dto.Quilometragem
        };

        await _repository.CriarAsync(veiculo);
    }
}