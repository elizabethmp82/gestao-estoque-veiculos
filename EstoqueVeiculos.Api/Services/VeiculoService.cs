using EstoqueVeiculos.Api.DTOs;
using EstoqueVeiculos.Api.Models;
using EstoqueVeiculos.Api.Repositories;
using EstoqueVeiculos.Api.Validators;
using EstoqueVeiculos.Api.Exceptions;


namespace EstoqueVeiculos.Api.Services;

public class VeiculoService
{
    private readonly VeiculoRepository _repository;
    private readonly ProprietarioRepository _proprietarioRepository;

    public VeiculoService(
        VeiculoRepository repository,
        ProprietarioRepository proprietarioRepository)
      {
          _repository = repository;
          _proprietarioRepository = proprietarioRepository;
      }

    public async Task CriarAsync(VeiculoCreateDto dto)
    {

         var tiposPermitidos = new[] { "Hatch", "Sedan", "SUV", "Picape" };

if (!tiposPermitidos.Contains(dto.Tipo))
{
    throw new ArgumentException(
        "O tipo do veículo deve ser Hatch, Sedan, SUV ou Picape."
    );
}
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


    public async Task<VeiculoDetalheDto?> BuscarPorIdAsync(int id)
{
    var veiculo = await _repository.BuscarPorIdAsync(id);

    if (veiculo is null)
        return null;

    var proprietarios =
        await _proprietarioRepository.ListarPorVeiculoAsync(id);

    return new VeiculoDetalheDto
    {
        Id = veiculo.Id,
        Marca = veiculo.Marca,
        Modelo = veiculo.Modelo,
        Ano = veiculo.Ano,
        Cor = veiculo.Cor,
        Preco = veiculo.Preco,
        Tipo = veiculo.Tipo,
        Situacao = veiculo.Situacao,
        Placa = veiculo.Placa,
        Quilometragem = veiculo.Quilometragem,
        Proprietarios = proprietarios
    };
}

public async Task<List<Veiculo>> ListarAsync(
    string? marca,
    string? situacao)
{
    return await _repository.ListarAsync(marca, situacao);
}

public async Task AtualizarAsync(int id, VeiculoUpdateDto dto)
{
  var situacoesPermitidas = new[]
{
    "Disponível",
    "Vendido",
    "Reservado"
};

if (!situacoesPermitidas.Contains(dto.Situacao))
{
    throw new ArgumentException(
        "A situação do veículo deve ser Disponível, Vendido ou Reservado."
    );
}
 
      
    if (id <= 0)
        throw new ArgumentException("O veículo informado é inválido.");

    if (string.IsNullOrWhiteSpace(dto.Marca))
        throw new ArgumentException("A marca é obrigatória.");

    if (string.IsNullOrWhiteSpace(dto.Modelo))
        throw new ArgumentException("O modelo é obrigatório.");

    if (string.IsNullOrWhiteSpace(dto.Cor))
        throw new ArgumentException("A cor é obrigatória.");

    if (string.IsNullOrWhiteSpace(dto.Tipo))
        throw new ArgumentException("O tipo é obrigatório.");

    if (string.IsNullOrWhiteSpace(dto.Situacao))
        throw new ArgumentException("A situação é obrigatória.");

    if (dto.Ano < 1900 || dto.Ano > DateTime.Now.Year)
        throw new ArgumentException("O ano do veículo é inválido.");

    if (dto.Preco < 0)
        throw new ArgumentException("O preço não pode ser negativo.");

    if (dto.Quilometragem < 0)
        throw new ArgumentException("A quilometragem não pode ser negativa.");

    var veiculo = await _repository.BuscarPorIdAsync(id);

    if (veiculo is null)
        throw new NotFoundException("Veículo não encontrado.");

    if (dto.Situacao == "Vendido" && dto.NovoProprietario is null)
    {
        throw new ArgumentException(
            "Para marcar o veículo como vendido, é obrigatório informar o novo proprietário."
        );
    }

    veiculo.Marca = dto.Marca;
    veiculo.Modelo = dto.Modelo;
    veiculo.Ano = dto.Ano;
    veiculo.Cor = dto.Cor;
    veiculo.Preco = dto.Preco;
    veiculo.Tipo = dto.Tipo;
    veiculo.Situacao = dto.Situacao;
    veiculo.Quilometragem = dto.Quilometragem;

    if (dto.Situacao != "Vendido")
    {
        await _repository.AtualizarAsync(veiculo);
        return;
    }

    var novoProprietario = dto.NovoProprietario!;

    if (string.IsNullOrWhiteSpace(novoProprietario.NomeCompleto))
    {
        throw new ArgumentException(
            "O nome completo do novo proprietário é obrigatório."
        );
    }

    if (!CpfValidator.FormatoValido(novoProprietario.CPF))
    {
        throw new ArgumentException(
            "O CPF do novo proprietário é inválido."
        );
    }

    if (novoProprietario.DataAquisicao == default)
    {
        throw new ArgumentException(
            "A data de aquisição do novo proprietário é obrigatória."
        );
    }

    var proprietario = new Proprietario
    {
        VeiculoId = veiculo.Id,
        NomeCompleto = novoProprietario.NomeCompleto,
        CPF = novoProprietario.CPF,
        DataAquisicao = novoProprietario.DataAquisicao,
        DataVenda = null,
        Observacao = novoProprietario.Observacao
    };

    await _repository.VenderAsync(veiculo, proprietario);
}

 public async Task ExcluirAsync(int id)
{
    if (id <= 0)
        throw new ArgumentException("O veículo informado é inválido.");

    var veiculo = await _repository.BuscarPorIdAsync(id);

    if (veiculo is null)
        throw new NotFoundException("Veículo não encontrado.");

    var possuiProprietarios =
        await _proprietarioRepository.ExistePorVeiculoAsync(id);

    if (possuiProprietarios)
    {
        throw new ArgumentException(
            "Não é possível excluir um veículo que possui proprietários cadastrados."
        );
    }

    await _repository.ExcluirAsync(id);
}
}