namespace EstoqueVeiculos.Api.Models;

public class Proprietario
{
    public int Id { get; set; }

    public int VeiculoId { get; set; }

    public string NomeCompleto { get; set; } = string.Empty;

    public string CPF { get; set; } = string.Empty;

    public DateTime DataAquisicao { get; set; }

    public DateTime? DataVenda { get; set; }

    public string Observacao { get; set; } = string.Empty;
}