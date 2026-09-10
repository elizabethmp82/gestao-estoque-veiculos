namespace EstoqueVeiculos.Api.Validators;

public static class CpfValidator
{
    public static bool FormatoValido(string cpf)
    {
        if (string.IsNullOrWhiteSpace(cpf))
            return false;

        var numeros = new string(cpf.Where(char.IsDigit).ToArray());

        return numeros.Length == 11;
    }
}