#nullable enable
using System;

// =========================================================
// Exercício 3 - Encomenda e destinatário
// Nome e Documento são obrigatórios -> string (não anulável).
// Telefone pode legitimamente não existir no momento do
// cadastro -> string? (nulabilidade com justificativa de domínio).
// Destinatario é o único responsável por validar seus dados;
// Encomenda apenas garante que RECEBEU um destinatário válido.
// =========================================================

public class Destinatario
{
    public string Nome { get; }
    public string Documento { get; }
    public string? Telefone { get; private set; }

    public Destinatario(string nome, string documento, string? telefone = null)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do destinatário é obrigatório.", nameof(nome));

        if (string.IsNullOrWhiteSpace(documento))
            throw new ArgumentException("O documento do destinatário é obrigatório.", nameof(documento));

        Nome = nome;
        Documento = documento;
        Telefone = string.IsNullOrWhiteSpace(telefone) ? null : telefone;
    }

    // Comportamento explícito para atualização posterior (não é um "public set").
    public void AtualizarTelefone(string novoTelefone)
    {
        if (string.IsNullOrWhiteSpace(novoTelefone))
            throw new ArgumentException("O telefone informado não pode ser vazio.", nameof(novoTelefone));

        Telefone = novoTelefone;
    }

    public override string ToString() =>
        $"{Nome} ({Documento}) - Telefone: {Telefone ?? "não informado"}";
}

public class Encomenda
{
    public string CodigoRastreamento { get; }
    public string Descricao { get; }
    public double Peso { get; }
    public Destinatario Destinatario { get; }

    public Encomenda(string codigoRastreamento, string descricao, double peso, Destinatario destinatario)
    {
        if (string.IsNullOrWhiteSpace(codigoRastreamento))
            throw new ArgumentException("O código de rastreamento é obrigatório.", nameof(codigoRastreamento));

        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("A descrição não pode estar vazia.", nameof(descricao));

        if (peso <= 0)
            throw new ArgumentException("O peso precisa ser maior que zero.", nameof(peso));

        // Encomenda só garante que o destinatário existe; NÃO valida Nome/Documento
        // do destinatário, pois essa é responsabilidade exclusiva de Destinatario.
        Destinatario = destinatario ?? throw new ArgumentNullException(nameof(destinatario),
            "Uma encomenda não pode existir sem destinatário.");

        CodigoRastreamento = codigoRastreamento;
        Descricao = descricao;
        Peso = peso;
    }

    public override string ToString() =>
        $"Encomenda {CodigoRastreamento} - {Descricao} ({Peso}kg) | Destinatário: {Destinatario}";
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Exercício 3 - Encomenda e destinatário ===\n");

        var destinatario = new Destinatario("Maria Oliveira", "123.456.789-00");
        Console.WriteLine($"Destinatário criado sem telefone: {destinatario}");

        var encomenda = new Encomenda("BR123456789", "Livros", 1.5, destinatario);
        Console.WriteLine(encomenda);

        Console.WriteLine("\n--- Atualizando telefone posteriormente ---");
        destinatario.AtualizarTelefone("(45) 99999-0000");

        Console.WriteLine($"Telefone consultado através da referência em Encomenda: {encomenda.Destinatario.Telefone}");

        Console.WriteLine("\n--- Tentativas inválidas ---");

        try
        {
            var destinatarioInvalido = new Destinatario("", "111.111.111-11");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Falhou ao criar destinatário sem nome: {e.Message}");
        }

        try
        {
            var encomendaSemDestinatario = new Encomenda("BR000000000", "Item qualquer", 1.0, null!);
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine($"Falhou ao criar encomenda sem destinatário: {e.Message}");
        }

        try
        {
            var encomendaPesoInvalido = new Encomenda("BR999999999", "Item qualquer", -2, destinatario);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Falhou ao criar encomenda com peso inválido: {e.Message}");
        }
    }
}
