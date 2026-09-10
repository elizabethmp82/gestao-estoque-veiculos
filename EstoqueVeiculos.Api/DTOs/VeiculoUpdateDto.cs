namespace EstoqueVeiculos.Api.DTOs;

public class VeiculoUpdateDto
{
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Ano { get; set; }
    public string Cor { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Situacao { get; set; } = string.Empty;
    public int Quilometragem { get; set; }

    public ProprietarioVendaDto? NovoProprietario { get; set; }
}