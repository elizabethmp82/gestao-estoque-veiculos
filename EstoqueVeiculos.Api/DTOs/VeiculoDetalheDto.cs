using EstoqueVeiculos.Api.Models;

namespace EstoqueVeiculos.Api.DTOs;

public class VeiculoDetalheDto
{
    public int Id { get; set; }
    public string Marca { get; set; } = string.Empty;
    public string Modelo { get; set; } = string.Empty;
    public int Ano { get; set; }
    public string Cor { get; set; } = string.Empty;
    public decimal Preco { get; set; }
    public string Tipo { get; set; } = string.Empty;
    public string Situacao { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public int Quilometragem { get; set; }

    public List<Proprietario> Proprietarios { get; set; } = new();
}