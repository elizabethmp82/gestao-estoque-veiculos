// using EstoqueVeiculos.Api.Database;
// using EstoqueVeiculos.Api.Models;
// using Oracle.ManagedDataAccess.Client;



// namespace EstoqueVeiculos.Api.Repositories;

// public class VeiculoRepository
// {
//     private readonly OracleConnectionFactory _connectionFactory;

//     public VeiculoRepository(OracleConnectionFactory connectionFactory)
//     {
//         _connectionFactory = connectionFactory;
//     }

//     public async Task<List<Veiculo>> ListarAsync(string? marca, string? situacao)
//     {
//         var veiculos = new List<Veiculo>();

//         using var connection = _connectionFactory.CreateConnection();

//         await connection.OpenAsync();

//         using var command = connection.CreateCommand();

//         command.CommandText = @"
//                SELECT
//                  ID,
//                  MARCA,
//                  MODELO,
//                  ANO,
//                  COR,
//                  PRECO,
//                  TIPO,
//                  SITUACAO,
//                  PLACA,
//                  QUILOMETRAGEM
//               FROM VEICULO
//               WHERE 1 = 1
//                ";

//         if (!string.IsNullOrWhiteSpace(marca))
//          {
//           command.CommandText += " AND MARCA = :marca";

//     command.Parameters.Add(
//         new OracleParameter("marca", marca)
//     );
// }

// if (!string.IsNullOrWhiteSpace(situacao))
// {
//     command.CommandText += " AND SITUACAO = :situacao";

//     command.Parameters.Add(
//         new OracleParameter("situacao", situacao)
//     );
// }

// command.CommandText += " ORDER BY ID";

//     using var reader = await command.ExecuteReaderAsync();

//         while (await reader.ReadAsync())
//         {
//             var veiculo = new Veiculo
//             {
//                 Id = reader.GetInt32(reader.GetOrdinal("ID")),
//                 Marca = reader.GetString(reader.GetOrdinal("MARCA")),
//                 Modelo = reader.GetString(reader.GetOrdinal("MODELO")),
//                 Ano = reader.GetInt32(reader.GetOrdinal("ANO")),
//                 Cor = reader.GetString(reader.GetOrdinal("COR")),
//                 Preco = reader.GetDecimal(reader.GetOrdinal("PRECO")),
//                 Tipo = reader.GetString(reader.GetOrdinal("TIPO")),
//                 Situacao = reader.GetString(reader.GetOrdinal("SITUACAO")),
//                 Placa = reader.GetString(reader.GetOrdinal("PLACA")),
//                 Quilometragem = reader.GetInt32(reader.GetOrdinal("QUILOMETRAGEM"))
//             };

//             veiculos.Add(veiculo);
//         }

//         return veiculos;
//     }
//     public async Task<bool> PlacaExisteAsync(string placa)
//     {
//        using var connection = _connectionFactory.CreateConnection();

//        await connection.OpenAsync();

//        using var command = connection.CreateCommand();

//        command.CommandText = @"
//           SELECT COUNT(*)
//           FROM VEICULO
//           WHERE PLACA = :placa
//         ";

//        command.Parameters.Add(
//        new OracleParameter("placa", placa)
//        ) ;

//       var resultado = await command.ExecuteScalarAsync();

//       var quantidade = Convert.ToInt32(resultado);

//       return quantidade > 0;
//     }

//     public async Task CriarAsync(Veiculo veiculo)
//     {
//        using var connection = _connectionFactory.CreateConnection();

//        await connection.OpenAsync();

//        using var command = connection.CreateCommand();

//     command.CommandText = @"
//         INSERT INTO VEICULO
//         (
//             MARCA,
//             MODELO,
//             ANO,
//             COR,
//             PRECO,
//             TIPO,
//             SITUACAO,
//             PLACA,
//             QUILOMETRAGEM
//         )
//         VALUES
//         (
//             :marca,
//             :modelo,
//             :ano,
//             :cor,
//             :preco,
//             :tipo,
//             :situacao,
//             :placa,
//             :quilometragem
//         )
//     ";

//     command.Parameters.Add(new OracleParameter("marca", veiculo.Marca));
//     command.Parameters.Add(new OracleParameter("modelo", veiculo.Modelo));
//     command.Parameters.Add(new OracleParameter("ano", veiculo.Ano));
//     command.Parameters.Add(new OracleParameter("cor", veiculo.Cor));
//     command.Parameters.Add(new OracleParameter("preco", veiculo.Preco));
//     command.Parameters.Add(new OracleParameter("tipo", veiculo.Tipo));
//     command.Parameters.Add(new OracleParameter("situacao", veiculo.Situacao));
//     command.Parameters.Add(new OracleParameter("placa", veiculo.Placa));
//     command.Parameters.Add(new OracleParameter("quilometragem", veiculo.Quilometragem));

//     await command.ExecuteNonQueryAsync();
// }

// public async Task<Veiculo?> BuscarPorIdAsync(int id)
// {
//     using var connection = _connectionFactory.CreateConnection();

//     await connection.OpenAsync();

//     using var command = connection.CreateCommand();

//     command.CommandText = @"
//         SELECT
//             ID,
//             MARCA,
//             MODELO,
//             ANO,
//             COR,
//             PRECO,
//             TIPO,
//             SITUACAO,
//             PLACA,
//             QUILOMETRAGEM
//         FROM VEICULO
//         WHERE ID = :id
//     ";

//     command.Parameters.Add(
//         new OracleParameter("id", id)
//     );

//     using var reader = await command.ExecuteReaderAsync();

//     if (!await reader.ReadAsync())
//     {
//         return null;
//     }

//     return new Veiculo
//     {
//         Id = reader.GetInt32(reader.GetOrdinal("ID")),
//         Marca = reader.GetString(reader.GetOrdinal("MARCA")),
//         Modelo = reader.GetString(reader.GetOrdinal("MODELO")),
//         Ano = reader.GetInt32(reader.GetOrdinal("ANO")),
//         Cor = reader.GetString(reader.GetOrdinal("COR")),
//         Preco = reader.GetDecimal(reader.GetOrdinal("PRECO")),
//         Tipo = reader.GetString(reader.GetOrdinal("TIPO")),
//         Situacao = reader.GetString(reader.GetOrdinal("SITUACAO")),
//         Placa = reader.GetString(reader.GetOrdinal("PLACA")),
//         Quilometragem = reader.GetInt32(reader.GetOrdinal("QUILOMETRAGEM"))
//     };
// }

//     public async Task AtualizarAsync(Veiculo veiculo)
// {
//     using var connection = _connectionFactory.CreateConnection();
//     await connection.OpenAsync();

//     using var command = connection.CreateCommand();

//     command.CommandText = @"
//         UPDATE VEICULO
//         SET
//             MARCA = :marca,
//             MODELO = :modelo,
//             ANO = :ano,
//             COR = :cor,
//             PRECO = :preco,
//             TIPO = :tipo,
//             SITUACAO = :situacao,
//             QUILOMETRAGEM = :quilometragem
//         WHERE ID = :id
//     ";

//     command.Parameters.Add(new OracleParameter("marca", veiculo.Marca));
//     command.Parameters.Add(new OracleParameter("modelo", veiculo.Modelo));
//     command.Parameters.Add(new OracleParameter("ano", veiculo.Ano));
//     command.Parameters.Add(new OracleParameter("cor", veiculo.Cor));
//     command.Parameters.Add(new OracleParameter("preco", veiculo.Preco));
//     command.Parameters.Add(new OracleParameter("tipo", veiculo.Tipo));
//     command.Parameters.Add(new OracleParameter("situacao", veiculo.Situacao));
//     command.Parameters.Add(new OracleParameter("quilometragem", veiculo.Quilometragem));
//     command.Parameters.Add(new OracleParameter("id", veiculo.Id));

//     await command.ExecuteNonQueryAsync();
// }

//  public async Task VenderAsync(
//     Veiculo veiculo,
//     Proprietario novoProprietario)
// {
//     using var connection = _connectionFactory.CreateConnection();
//     await connection.OpenAsync();

//     using var transaction = connection.BeginTransaction();

//     try
//     {
//         using var commandVeiculo = connection.CreateCommand();
//         commandVeiculo.Transaction = transaction;

//         commandVeiculo.CommandText = @"
//             UPDATE VEICULO
//             SET
//                 MARCA = :marca,
//                 MODELO = :modelo,
//                 ANO = :ano,
//                 COR = :cor,
//                 PRECO = :preco,
//                 TIPO = :tipo,
//                 SITUACAO = :situacao,
//                 QUILOMETRAGEM = :quilometragem
//             WHERE ID = :id
//         ";

//         commandVeiculo.Parameters.Add(
//             new OracleParameter("marca", veiculo.Marca)
//         );

//         commandVeiculo.Parameters.Add(
//             new OracleParameter("modelo", veiculo.Modelo)
//         );

//         commandVeiculo.Parameters.Add(
//             new OracleParameter("ano", veiculo.Ano)
//         );

//         commandVeiculo.Parameters.Add(
//             new OracleParameter("cor", veiculo.Cor)
//         );

//         commandVeiculo.Parameters.Add(
//             new OracleParameter("preco", veiculo.Preco)
//         );

//         commandVeiculo.Parameters.Add(
//             new OracleParameter("tipo", veiculo.Tipo)
//         );

//         commandVeiculo.Parameters.Add(
//             new OracleParameter("situacao", veiculo.Situacao)
//         );

//         commandVeiculo.Parameters.Add(
//             new OracleParameter(
//                 "quilometragem",
//                 veiculo.Quilometragem
//             )
//         );

//         commandVeiculo.Parameters.Add(
//             new OracleParameter("id", veiculo.Id)
//         );

//         await commandVeiculo.ExecuteNonQueryAsync();

//         using var commandProprietario = connection.CreateCommand();
//         commandProprietario.Transaction = transaction;

//         commandProprietario.CommandText = @"
//             INSERT INTO PROPRIETARIO
//             (
//                 VEICULOID,
//                 NOMECOMPLETO,
//                 CPF,
//                 DATAAQUISICAO,
//                 DATAVENDA,
//                 OBSERVACAO
//             )
//             VALUES
//             (
//                 :veiculoId,
//                 :nomeCompleto,
//                 :cpf,
//                 :dataAquisicao,
//                 NULL,
//                 :observacao
//             )
//         ";

//         commandProprietario.Parameters.Add(
//             new OracleParameter(
//                 "veiculoId",
//                 novoProprietario.VeiculoId
//             )
//         );

//         commandProprietario.Parameters.Add(
//             new OracleParameter(
//                 "nomeCompleto",
//                 novoProprietario.NomeCompleto
//             )
//         );

//         commandProprietario.Parameters.Add(
//             new OracleParameter(
//                 "cpf",
//                 novoProprietario.CPF
//             )
//         );

//         commandProprietario.Parameters.Add(
//             new OracleParameter(
//                 "dataAquisicao",
//                 novoProprietario.DataAquisicao
//             )
//         );

//         commandProprietario.Parameters.Add(
//             new OracleParameter(
//                 "observacao",
//                 novoProprietario.Observacao
//             )
//         );

//         await commandProprietario.ExecuteNonQueryAsync();

//         transaction.Commit();
//     }
//     catch
//     {
//         transaction.Rollback();
//         throw;
//     }
// }


// }

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

    public async Task<List<Veiculo>> ListarAsync(
        string? marca,
        string? situacao)
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
            veiculos.Add(new Veiculo
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
                Quilometragem = reader.GetInt32(
                    reader.GetOrdinal("QUILOMETRAGEM")
                )
            });
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
        );

        var resultado = await command.ExecuteScalarAsync();

        return Convert.ToInt32(resultado) > 0;
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

        command.Parameters.Add(
            new OracleParameter("marca", veiculo.Marca)
        );

        command.Parameters.Add(
            new OracleParameter("modelo", veiculo.Modelo)
        );

        command.Parameters.Add(
            new OracleParameter("ano", veiculo.Ano)
        );

        command.Parameters.Add(
            new OracleParameter("cor", veiculo.Cor)
        );

        command.Parameters.Add(
            new OracleParameter("preco", veiculo.Preco)
        );

        command.Parameters.Add(
            new OracleParameter("tipo", veiculo.Tipo)
        );

        command.Parameters.Add(
            new OracleParameter("situacao", veiculo.Situacao)
        );

        command.Parameters.Add(
            new OracleParameter("placa", veiculo.Placa)
        );

        command.Parameters.Add(
            new OracleParameter(
                "quilometragem",
                veiculo.Quilometragem
            )
        );

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
            return null;

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
            Quilometragem = reader.GetInt32(
                reader.GetOrdinal("QUILOMETRAGEM")
            )
        };
    }

    public async Task AtualizarAsync(Veiculo veiculo)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var command = connection.CreateCommand();

        command.CommandText = @"
            UPDATE VEICULO
            SET
                MARCA = :marca,
                MODELO = :modelo,
                ANO = :ano,
                COR = :cor,
                PRECO = :preco,
                TIPO = :tipo,
                SITUACAO = :situacao,
                QUILOMETRAGEM = :quilometragem
            WHERE ID = :id
        ";

        command.Parameters.Add(
            new OracleParameter("marca", veiculo.Marca)
        );

        command.Parameters.Add(
            new OracleParameter("modelo", veiculo.Modelo)
        );

        command.Parameters.Add(
            new OracleParameter("ano", veiculo.Ano)
        );

        command.Parameters.Add(
            new OracleParameter("cor", veiculo.Cor)
        );

        command.Parameters.Add(
            new OracleParameter("preco", veiculo.Preco)
        );

        command.Parameters.Add(
            new OracleParameter("tipo", veiculo.Tipo)
        );

        command.Parameters.Add(
            new OracleParameter("situacao", veiculo.Situacao)
        );

        command.Parameters.Add(
            new OracleParameter(
                "quilometragem",
                veiculo.Quilometragem
            )
        );

        command.Parameters.Add(
            new OracleParameter("id", veiculo.Id)
        );

        await command.ExecuteNonQueryAsync();
    }

    public async Task VenderAsync(
        Veiculo veiculo,
        Proprietario novoProprietario)
    {
        using var connection = _connectionFactory.CreateConnection();
        await connection.OpenAsync();

        using var transaction = connection.BeginTransaction();

        try
        {
            // 1. Encerra o proprietário atual
            using var commandProprietarioAtual =
                connection.CreateCommand();

            commandProprietarioAtual.Transaction = transaction;

            commandProprietarioAtual.CommandText = @"
                UPDATE PROPRIETARIO
                SET DATAVENDA = :dataVenda
                WHERE VEICULOID = :veiculoId
                  AND DATAVENDA IS NULL
            ";

            commandProprietarioAtual.Parameters.Add(
                new OracleParameter(
                    "dataVenda",
                    novoProprietario.DataAquisicao
                )
            );

            commandProprietarioAtual.Parameters.Add(
                new OracleParameter(
                    "veiculoId",
                    veiculo.Id
                )
            );

            await commandProprietarioAtual.ExecuteNonQueryAsync();

            // 2. Atualiza os dados do veículo
            using var commandVeiculo = connection.CreateCommand();

            commandVeiculo.Transaction = transaction;

            commandVeiculo.CommandText = @"
                UPDATE VEICULO
                SET
                    MARCA = :marca,
                    MODELO = :modelo,
                    ANO = :ano,
                    COR = :cor,
                    PRECO = :preco,
                    TIPO = :tipo,
                    SITUACAO = :situacao,
                    QUILOMETRAGEM = :quilometragem
                WHERE ID = :id
            ";

            commandVeiculo.Parameters.Add(
                new OracleParameter("marca", veiculo.Marca)
            );

            commandVeiculo.Parameters.Add(
                new OracleParameter("modelo", veiculo.Modelo)
            );

            commandVeiculo.Parameters.Add(
                new OracleParameter("ano", veiculo.Ano)
            );

            commandVeiculo.Parameters.Add(
                new OracleParameter("cor", veiculo.Cor)
            );

            commandVeiculo.Parameters.Add(
                new OracleParameter("preco", veiculo.Preco)
            );

            commandVeiculo.Parameters.Add(
                new OracleParameter("tipo", veiculo.Tipo)
            );

            commandVeiculo.Parameters.Add(
                new OracleParameter(
                    "situacao",
                    veiculo.Situacao
                )
            );

            commandVeiculo.Parameters.Add(
                new OracleParameter(
                    "quilometragem",
                    veiculo.Quilometragem
                )
            );

            commandVeiculo.Parameters.Add(
                new OracleParameter("id", veiculo.Id)
            );

            await commandVeiculo.ExecuteNonQueryAsync();

            // 3. Insere o novo proprietário
            using var commandNovoProprietario =
                connection.CreateCommand();

            commandNovoProprietario.Transaction = transaction;

            commandNovoProprietario.CommandText = @"
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

            commandNovoProprietario.Parameters.Add(
                new OracleParameter(
                    "veiculoId",
                    novoProprietario.VeiculoId
                )
            );

            commandNovoProprietario.Parameters.Add(
                new OracleParameter(
                    "nomeCompleto",
                    novoProprietario.NomeCompleto
                )
            );

            commandNovoProprietario.Parameters.Add(
                new OracleParameter(
                    "cpf",
                    novoProprietario.CPF
                )
            );

            commandNovoProprietario.Parameters.Add(
                new OracleParameter(
                    "dataAquisicao",
                    novoProprietario.DataAquisicao
                )
            );

            commandNovoProprietario.Parameters.Add(
                new OracleParameter(
                    "observacao",
                    novoProprietario.Observacao
                )
            );

            await commandNovoProprietario.ExecuteNonQueryAsync();

            // 4. Confirma todas as operações
            transaction.Commit();
        }
        catch
        {
            // Se qualquer operação falhar,
            // desfaz tudo que ocorreu na transação.
            transaction.Rollback();

            throw;
        }
    }
}