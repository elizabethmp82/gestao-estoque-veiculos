# Respostas às Questões Técnicas

## Questão 1 — Conexão e Gerenciamento de Recursos
No seu projeto, como você gerenciou a abertura e o fechamento das conexões com o 
banco de dados? Explique por que isso é importante e mostre um trecho do seu código 
onde isso acontece, descrevendo o que aconteceria se você não fizesse esse gerenciamento 
corretamente. 

As conexões com o Oracle são criadas através da `OracleConnectionFactory` e abertas somente quando uma operação no banco precisa ser executada.

Nos repositórios utilizei `using var` para garantir que conexões, comandos e leitores sejam liberados após o uso.

```csharp
using var connection = _connectionFactory.CreateConnection();
await connection.OpenAsync();

using var command = connection.CreateCommand();
command.CommandText = "SELECT * FROM VEICULO WHERE ID = :id";
command.Parameters.Add(new OracleParameter("id", id));

using var reader = await command.ExecuteReaderAsync();
```

Esse gerenciamento é importante para evitar conexões abertas desnecessariamente. Caso os recursos não sejam liberados corretamente, o pool de conexões pode se esgotar, causando lentidão e erros de timeout na aplicação.

---

## Questão 2 — Segurança nas Queries

Explique o que é SQL Injection e como você o preveniu no seu projeto. Mostre um 
exemplo real de uma query do seu código que poderia ser vulnerável caso fosse escrita 
de forma diferente, e explique o que você fez para protegê-la.

SQL Injection é uma vulnerabilidade que ocorre quando valores recebidos pelo usuário são concatenados diretamente em uma instrução SQL, permitindo que o conteúdo enviado altere a consulta original.

No projeto, utilizei queries parametrizadas. Por exemplo, na verificação da existência de uma placa:

```csharp
command.CommandText = @"
    SELECT COUNT(*)
    FROM VEICULO
    WHERE PLACA = :placa";

command.Parameters.Add(
    new OracleParameter("placa", placa)
);
```

Uma implementação vulnerável seria:

```csharp
command.CommandText =
    $"SELECT COUNT(*) FROM VEICULO WHERE PLACA = '{placa}'";
```

No projeto, o valor da placa é enviado separadamente através do parâmetro `:placa`, evitando que o valor informado seja interpretado como parte do comando SQL.

---

## Questão 3 — Relacionamento entre Tabelas
Descreva como o relacionamento entre as tabelas Veiculo e Proprietario está 
implementado no seu banco de dados e como ele se reflete no código C#. Explique o que 
acontece na sua aplicação quando o usuário tenta excluir um veículo que possui 
proprietários cadastrados — onde essa regra é aplicada (banco, repositório ou outra 
camada?) e por quê você optou por essa abordagem.

O relacionamento entre `VEICULO` e `PROPRIETARIO` é de **1:N**, ou seja, um veículo pode possuir vários proprietários.

No banco, esse relacionamento é implementado pela chave estrangeira `VEICULOID`:

```sql
CONSTRAINT FK_PROPRIETARIO_VEICULO
    FOREIGN KEY (VEICULOID)
    REFERENCES VEICULO(ID)
```

No C#, o proprietário possui o identificador do veículo:

```csharp
public int VeiculoId { get; set; }
```

Antes de excluir um veículo, a aplicação verifica se existem proprietários vinculados. Essa regra é aplicada na camada de `Service`, pois é uma regra de negócio.

```csharp
if (await _proprietarioRepository.ExistePorVeiculoAsync(id))
{
    throw new ArgumentException(
        "Não é possível excluir um veículo que possui proprietários."
    );
}
```

A chave estrangeira no banco também protege a integridade dos dados. Optei por manter a regra de negócio no `Service` para conseguir validar a operação e retornar uma mensagem clara ao usuário antes de tentar realizar a exclusão.

---

## Questão 4 — Transações
Na funcionalidade de marcar um veículo como "Vendido", duas operações precisam 
acontecer juntas: a atualização da situação do veículo e o cadastro do novo proprietário. 
O que aconteceria se a primeira operação fosse concluída mas a segunda falhasse? Como 
você tratou (ou trataria) esse cenário no seu código? Se utilizou transações, explique como 
elas funcionam no ADO.NET e mostre o trecho relevante. 

Na venda de um veículo, várias alterações precisam ser concluídas juntas. Se o veículo fosse atualizado para `Vendido` e o cadastro do novo proprietário falhasse, os dados ficariam inconsistentes.

Para evitar isso, utilizei uma transação no ADO.NET.

```csharp
using var connection = _connectionFactory.CreateConnection();
await connection.OpenAsync();

using var transaction = connection.BeginTransaction();

try
{
    // Atualiza o proprietário atual
    // Atualiza o veículo
    // Cadastra o novo proprietário

    transaction.Commit();
}
catch
{
    transaction.Rollback();
    throw;
}
```

Todos os comandos envolvidos na venda utilizam a mesma conexão e a mesma transação.

O `Commit` confirma todas as alterações quando a operação é concluída com sucesso. Caso alguma etapa apresente erro, o `Rollback` desfaz as alterações realizadas naquela transação.

Assim, a venda é tratada como uma única operação e evita que apenas parte dos dados seja gravada.

---

## Questão 5 — Organização e Decisões de Projeto
Descreva como você organizou as camadas do seu projeto. Por que você separou a lógica 
de acesso ao banco em uma camada de repositório separada do Controller? Se você fosse 
refatorar ou melhorar alguma parte do código que entregou, o que você mudaria e por 
quê?


O backend foi organizado separando as responsabilidades em:

```text
Controller
    ↓
Service
    ↓
Repository
    ↓
ADO.NET
    ↓
Oracle
```

O `Controller` recebe as requisições HTTP.

O `Service` concentra as regras de negócio e validações.

O `Repository` concentra o acesso ao banco de dados e as queries SQL utilizando ADO.NET.

Separei o acesso ao banco do Controller para evitar misturar regras HTTP, regras de negócio e SQL no mesmo lugar. Dessa forma, o código fica mais organizado, fácil de manter e com responsabilidades mais claras.

Também utilizei DTOs para controlar os dados recebidos pelas operações. Por exemplo, o DTO de atualização do veículo não possui a propriedade `Placa`, impedindo sua alteração pela operação de edição.

Como melhoria futura, eu melhoraria as mensagens de validação e o retorno de erros para o usuário, deixando mais claro qual campo precisa ser corrigido quando uma operação não puder ser realizada.