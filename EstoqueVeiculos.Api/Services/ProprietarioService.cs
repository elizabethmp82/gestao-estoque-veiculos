using EstoqueVeiculos.Api.DTOs;
using EstoqueVeiculos.Api.Models;
using EstoqueVeiculos.Api.Repositories;
using EstoqueVeiculos.Api.Validators;

namespace EstoqueVeiculos.Api.Services;

public class ProprietarioService
{
    private readonly ProprietarioRepository _proprietarioRepository;
    private readonly VeiculoRepository _veiculoRepository;

    public ProprietarioService(
        ProprietarioRepository proprietarioRepository,
        VeiculoRepository veiculoRepository)
    {
        _proprietarioRepository = proprietarioRepository;
        _veiculoRepository = veiculoRepository;
    }

    public async Task CriarAsync(ProprietarioCreateDto dto)
    {
        if (dto.VeiculoId <= 0)
            throw new ArgumentException("O veículo é obrigatório.");

        if (string.IsNullOrWhiteSpace(dto.NomeCompleto))
            throw new ArgumentException("O nome completo é obrigatório.");

        if (!CpfValidator.FormatoValido(dto.CPF))
            throw new ArgumentException("O CPF informado é inválido.");

        if (dto.DataAquisicao == default)
            throw new ArgumentException("A data de aquisição é obrigatória.");

        var veiculo = await _veiculoRepository.BuscarPorIdAsync(dto.VeiculoId);

        if (veiculo is null)
            throw new ArgumentException("Veículo não encontrado.");

        var proprietario = new Proprietario
        {
            VeiculoId = dto.VeiculoId,
            NomeCompleto = dto.NomeCompleto,
            CPF = dto.CPF,
            DataAquisicao = dto.DataAquisicao,
            DataVenda = null,
            Observacao = dto.Observacao
        };

        await _proprietarioRepository.CriarAsync(proprietario);
    }
    public async Task<List<Proprietario>> ListarPorVeiculoAsync(int veiculoId)
{
    return await _proprietarioRepository.ListarPorVeiculoAsync(veiculoId);
}
    

   public async Task AtualizarAsync(int id, ProprietarioUpdateDto dto)
{
    if (id <= 0)
        throw new ArgumentException("O proprietário informado é inválido.");

    if (string.IsNullOrWhiteSpace(dto.NomeCompleto))
        throw new ArgumentException("O nome completo é obrigatório.");

    if (!CpfValidator.FormatoValido(dto.CPF))
        throw new ArgumentException("O CPF informado é inválido.");

    if (dto.DataAquisicao == default)
        throw new ArgumentException("A data de aquisição é obrigatória.");

    if (dto.DataVenda.HasValue &&
        dto.DataVenda.Value < dto.DataAquisicao)
    {
        throw new ArgumentException(
            "A data de venda não pode ser anterior à data de aquisição."
        );
    }

    var proprietario =
        await _proprietarioRepository.BuscarPorIdAsync(id);

    if (proprietario is null)
        throw new ArgumentException("Proprietário não encontrado.");

    proprietario.NomeCompleto = dto.NomeCompleto;
    proprietario.CPF = dto.CPF;
    proprietario.DataAquisicao = dto.DataAquisicao;
    proprietario.DataVenda = dto.DataVenda;
    proprietario.Observacao = dto.Observacao;

    await _proprietarioRepository.AtualizarAsync(proprietario);
} 

public async Task ExcluirAsync(int id)
{
    var proprietario =
        await _proprietarioRepository.BuscarPorIdAsync(id);

    if (proprietario is null)
        throw new ArgumentException("Proprietário não encontrado.");

    if (proprietario.DataVenda is null)
    {
        throw new ArgumentException(
            "Não é possível excluir o proprietário atual do veículo."
        );
    }

    await _proprietarioRepository.ExcluirAsync(id);
}
}