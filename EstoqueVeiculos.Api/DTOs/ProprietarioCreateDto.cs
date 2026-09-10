namespace EstoqueVeiculos.Api.DTOs;

public class ProprietarioCreateDto
{
    public int VeiculoId { get; set; }
    public string NomeCompleto { get; set; } = string.Empty;
    public string CPF { get; set; } = string.Empty;
    public DateTime DataAquisicao { get; set; }
    public string Observacao { get; set; } = string.Empty;
}