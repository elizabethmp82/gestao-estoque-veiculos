using EstoqueVeiculos.Api.Database;
using EstoqueVeiculos.Api.Models;
using Oracle.ManagedDataAccess.Client;

namespace EstoqueVeiculos.Api.Repositories;

public class ProprietarioRepository
{
    private readonly OracleConnectionFactory _connectionFactory;

    public ProprietarioRepository(OracleConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<List<Proprietario>> ListarPorVeiculoAsync(int veiculoId)
    {
        var proprietarios = new List<Proprietario>();

        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();

        command.CommandText = @"
            SELECT
                ID,
                VEICULOID,
                NOMECOMPLETO,
                CPF,
                DATAAQUISICAO,
                DATAVENDA,
                OBSERVACAO
            FROM PROPRIETARIO
            WHERE VEICULOID = :veiculoId
            ORDER BY DATAAQUISICAO
        ";

        command.Parameters.Add(
            new OracleParameter("veiculoId", veiculoId)
        );

        using var reader = await command.ExecuteReaderAsync();

        while (await reader.ReadAsync())
        {
            var proprietario = new Proprietario
            {
                Id = reader.GetInt32(reader.GetOrdinal("ID")),
                VeiculoId = reader.GetInt32(reader.GetOrdinal("VEICULOID")),
                NomeCompleto = reader.GetString(reader.GetOrdinal("NOMECOMPLETO")),
                CPF = reader.GetString(reader.GetOrdinal("CPF")),
                DataAquisicao = reader.GetDateTime(reader.GetOrdinal("DATAAQUISICAO")),

                DataVenda = reader.IsDBNull(reader.GetOrdinal("DATAVENDA"))
                    ? null
                    : reader.GetDateTime(reader.GetOrdinal("DATAVENDA")),

                Observacao = reader.IsDBNull(reader.GetOrdinal("OBSERVACAO"))
                    ? string.Empty
                    : reader.GetString(reader.GetOrdinal("OBSERVACAO"))
            };

            proprietarios.Add(proprietario);
        }

        return proprietarios;
    }
    public async Task CriarAsync(Proprietario proprietario)
{
    using var connection = _connectionFactory.CreateConnection();
    await connection.OpenAsync();

    using var command = connection.CreateCommand();

    command.CommandText = @"
        INSERT INTO PROPRIETARIO
        (
            VEICULOID,
            NOMECOMPLETO,
            CPF,
            DATAAQUISICAO,
            DATAVENDA,
            OBSERVACAO
        )
        VALUES
        (
            :veiculoId,
            :nomeCompleto,
            :cpf,
            :dataAquisicao,
            NULL,
            :observacao
        )
    ";

    command.Parameters.Add(
        new OracleParameter("veiculoId", proprietario.VeiculoId)
    );

    command.Parameters.Add(
        new OracleParameter("nomeCompleto", proprietario.NomeCompleto)
    );

    command.Parameters.Add(
        new OracleParameter("cpf", proprietario.CPF)
    );

    command.Parameters.Add(
        new OracleParameter("dataAquisicao", proprietario.DataAquisicao)
    );

    command.Parameters.Add(
        new OracleParameter("observacao", proprietario.Observacao)
    );

    await command.ExecuteNonQueryAsync();
}

 public async Task<Proprietario?> BuscarPorIdAsync(int id)
{
    using var connection = _connectionFactory.CreateConnection();
    await connection.OpenAsync();

    using var command = connection.CreateCommand();

    command.CommandText = @"
        SELECT
            ID,
            VEICULOID,
            NOMECOMPLETO,
            CPF,
            DATAAQUISICAO,
            DATAVENDA,
            OBSERVACAO
        FROM PROPRIETARIO
        WHERE ID = :id
    ";

    command.Parameters.Add(
        new OracleParameter("id", id)
    );

    using var reader = await command.ExecuteReaderAsync();

    if (!await reader.ReadAsync())
        return null;

    return new Proprietario
    {
        Id = reader.GetInt32(reader.GetOrdinal("ID")),
        VeiculoId = reader.GetInt32(reader.GetOrdinal("VEICULOID")),
        NomeCompleto = reader.GetString(reader.GetOrdinal("NOMECOMPLETO")),
        CPF = reader.GetString(reader.GetOrdinal("CPF")),
        DataAquisicao = reader.GetDateTime(reader.GetOrdinal("DATAAQUISICAO")),
        DataVenda = reader.IsDBNull(reader.GetOrdinal("DATAVENDA"))
            ? null
            : reader.GetDateTime(reader.GetOrdinal("DATAVENDA")),
        Observacao = reader.IsDBNull(reader.GetOrdinal("OBSERVACAO"))
            ? string.Empty
            : reader.GetString(reader.GetOrdinal("OBSERVACAO"))
    };
}

 public async Task AtualizarAsync(Proprietario proprietario)
{
    using var connection = _connectionFactory.CreateConnection();
    await connection.OpenAsync();

    using var command = connection.CreateCommand();

    command.CommandText = @"
        UPDATE PROPRIETARIO
        SET
            NOMECOMPLETO = :nomeCompleto,
            CPF = :cpf,
            DATAAQUISICAO = :dataAquisicao,
            DATAVENDA = :dataVenda,
            OBSERVACAO = :observacao
        WHERE ID = :id
    ";

    command.Parameters.Add(
        new OracleParameter("nomeCompleto", proprietario.NomeCompleto)
    );

    command.Parameters.Add(
        new OracleParameter("cpf", proprietario.CPF)
    );

    command.Parameters.Add(
        new OracleParameter("dataAquisicao", proprietario.DataAquisicao)
    );

    command.Parameters.Add(
        new OracleParameter(
            "dataVenda",
            proprietario.DataVenda.HasValue
                ? proprietario.DataVenda.Value
                : DBNull.Value
        )
    );

    command.Parameters.Add(
        new OracleParameter("observacao", proprietario.Observacao)
    );

    command.Parameters.Add(
        new OracleParameter("id", proprietario.Id)
    );

    await command.ExecuteNonQueryAsync();
}

   public async Task ExcluirAsync(int id)
{
    using var connection = _connectionFactory.CreateConnection();
    await connection.OpenAsync();

    using var command = connection.CreateCommand();

    command.CommandText = @"
        DELETE FROM PROPRIETARIO
        WHERE ID = :id
    ";

    command.Parameters.Add(
        new OracleParameter("id", id)
    );

    await command.ExecuteNonQueryAsync();
}
}