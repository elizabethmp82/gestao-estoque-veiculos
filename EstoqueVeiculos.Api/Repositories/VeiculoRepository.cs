using EstoqueVeiculos.Api.Database;
using EstoqueVeiculos.Api.Models;
using Oracle.ManagedDataAccess.Client;



namespace EstoqueVeiculos.Api.Repositories;

public class VeiculoRepository
{
    private readonly OracleConnectionFactory _connectionFactory;

    public VeiculoRepository(OracleConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<Veiculo>> ListarAsync(string? marca, string? situacao)
    {
        var veiculos = new List<Veiculo>();

        using var connection = _connectionFactory.CreateConnection();

        await connection.OpenAsync();

        using var command = connection.CreateCommand();

        command.CommandText = @"
               SELECT
                 ID,
                 MARCA,
                 MODELO,
                 ANO,
                 COR,
                 PRECO,
                 TIPO,
                 SITUACAO,
                 PLACA,
                 QUILOMETRAGEM
              FROM VEICULO
              WHERE 1 = 1
               ";

        if (!string.IsNullOrWhiteSpace(marca))
         {
          command.CommandText += " AND MARCA = :marca";

    command.Parameters.Add(
        new OracleParameter("marca", marca)
    );
}

if (!string.IsNullOrWhiteSpace(situacao))
{
    command.CommandText += " AND SITUACAO = :situacao";

    command.Parameters.Add(
        new OracleParameter("situacao", situacao)
    );
}

command.CommandText += " ORDER BY ID";

    using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var veiculo = new Veiculo
            {
                Id = reader.GetInt32(reader.GetOrdinal("ID")),
                Marca = reader.GetString(reader.GetOrdinal("MARCA")),
                Modelo = reader.GetString(reader.GetOrdinal("MODELO")),
                Ano = reader.GetInt32(reader.GetOrdinal("ANO")),
                Cor = reader.GetString(reader.GetOrdinal("COR")),
                Preco = reader.GetDecimal(reader.GetOrdinal("PRECO")),
                Tipo = reader.GetString(reader.GetOrdinal("TIPO")),
                Situacao = reader.GetString(reader.GetOrdinal("SITUACAO")),
                Placa = reader.GetString(reader.GetOrdinal("PLACA")),
                Quilometragem = reader.GetInt32(reader.GetOrdinal("QUILOMETRAGEM"))
            };

            veiculos.Add(veiculo);
        }

        return veiculos;
    }
    public async Task<bool> PlacaExisteAsync(string placa)
    {
       using var connection = _connectionFactory.CreateConnection();

       await connection.OpenAsync();

       using var command = connection.CreateCommand();

       command.CommandText = @"
          SELECT COUNT(*)
          FROM VEICULO
          WHERE PLACA = :placa
        ";

       command.Parameters.Add(
       new OracleParameter("placa", placa)
       ) ;

      var resultado = await command.ExecuteScalarAsync();

      var quantidade = Convert.ToInt32(resultado);

      return quantidade > 0;
    }

    public async Task CriarAsync(Veiculo veiculo)
    {
       using var connection = _connectionFactory.CreateConnection();

       await connection.OpenAsync();

       using var command = connection.CreateCommand();

    command.CommandText = @"
        INSERT INTO VEICULO
        (
            MARCA,
            MODELO,
            ANO,
            COR,
            PRECO,
            TIPO,
            SITUACAO,
            PLACA,
            QUILOMETRAGEM
        )
        VALUES
        (
            :marca,
            :modelo,
            :ano,
            :cor,
            :preco,
            :tipo,
            :situacao,
            :placa,
            :quilometragem
        )
    ";

    command.Parameters.Add(new OracleParameter("marca", veiculo.Marca));
    command.Parameters.Add(new OracleParameter("modelo", veiculo.Modelo));
    command.Parameters.Add(new OracleParameter("ano", veiculo.Ano));
    command.Parameters.Add(new OracleParameter("cor", veiculo.Cor));
    command.Parameters.Add(new OracleParameter("preco", veiculo.Preco));
    command.Parameters.Add(new OracleParameter("tipo", veiculo.Tipo));
    command.Parameters.Add(new OracleParameter("situacao", veiculo.Situacao));
    command.Parameters.Add(new OracleParameter("placa", veiculo.Placa));
    command.Parameters.Add(new OracleParameter("quilometragem", veiculo.Quilometragem));

    await command.ExecuteNonQueryAsync();
}

public async Task<Veiculo?> BuscarPorIdAsync(int id)
{
    using var connection = _connectionFactory.CreateConnection();

    await connection.OpenAsync();

    using var command = connection.CreateCommand();

    command.CommandText = @"
        SELECT
            ID,
            MARCA,
            MODELO,
            ANO,
            COR,
            PRECO,
            TIPO,
            SITUACAO,
            PLACA,
            QUILOMETRAGEM
        FROM VEICULO
        WHERE ID = :id
    ";

    command.Parameters.Add(
        new OracleParameter("id", id)
    );

    using var reader = await command.ExecuteReaderAsync();

    if (!await reader.ReadAsync())
    {
        return null;
    }

    return new Veiculo
    {
        Id = reader.GetInt32(reader.GetOrdinal("ID")),
        Marca = reader.GetString(reader.GetOrdinal("MARCA")),
        Modelo = reader.GetString(reader.GetOrdinal("MODELO")),
        Ano = reader.GetInt32(reader.GetOrdinal("ANO")),
        Cor = reader.GetString(reader.GetOrdinal("COR")),
        Preco = reader.GetDecimal(reader.GetOrdinal("PRECO")),
        Tipo = reader.GetString(reader.GetOrdinal("TIPO")),
        Situacao = reader.GetString(reader.GetOrdinal("SITUACAO")),
        Placa = reader.GetString(reader.GetOrdinal("PLACA")),
        Quilometragem = reader.GetInt32(reader.GetOrdinal("QUILOMETRAGEM"))
    };
}
}