namespace EstoqueVeiculos.Api.DTOs;

public class ProprietarioUpdateDto
{
    public string NomeCompleto { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public DateTime DataAquisicao { get; set; }
    public DateTime? DataVenda { get; set; }
    public string Observacao { get; set; } = string.Empty;
}