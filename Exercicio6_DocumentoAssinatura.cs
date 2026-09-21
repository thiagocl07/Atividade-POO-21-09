#nullable enable
using System;

// =========================================================
// Exercício 6 - Documento e serviço de assinatura
// ServicoAssinatura é usado apenas DURANTE a operação Assinar(...).
// Ele é recebido como PARÂMETRO, não é armazenado como campo de
// Documento: é colaboração temporária, não associação persistente.
// =========================================================

public class ServicoAssinatura
{
    // Responsável tecnicamente por realizar a operação de assinatura.
    public string Assinar(string conteudoDocumento)
    {
        // Simulação de uma assinatura digital (ex.: geração de carimbo/hash).
        return $"ASSINADO-{DateTime.Now:yyyyMMddHHmmss}";
    }
}

public class Documento
{
    public string Titulo { get; }
    public string Conteudo { get; }
    public bool Assinado { get; private set; }
    public string? CarimboAssinatura { get; private set; }

    public Documento(string titulo, string conteudo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("O título do documento é obrigatório.", nameof(titulo));

        if (string.IsNullOrWhiteSpace(conteudo))
            throw new ArgumentException("O conteúdo não pode ser vazio.", nameof(conteudo));

        Titulo = titulo;
        Conteudo = conteudo;
        Assinado = false;
        CarimboAssinatura = null;
    }

    // O serviço colabora apenas durante a chamada: não vira propriedade do
    // documento. Apenas o RESULTADO da operação (situação de assinatura)
    // passa a fazer parte do estado de Documento.
    public void Assinar(ServicoAssinatura servico)
    {
        if (servico is null)
            throw new ArgumentNullException(nameof(servico), "É necessário um serviço de assinatura para assinar o documento.");

        if (Assinado)
            throw new InvalidOperationException("O documento já está assinado.");

        CarimboAssinatura = servico.Assinar(Conteudo);
        Assinado = true;
    }

    public override string ToString() =>
        $"\"{Titulo}\" - {(Assinado ? $"Assinado ({CarimboAssinatura})" : "Não assinado")}";
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Exercício 6 - Documento e serviço de assinatura ===\n");

        var documento = new Documento("Contrato de Prestação de Serviço", "Cláusulas do contrato...");
        Console.WriteLine(documento);

        var servico = new ServicoAssinatura();

        Console.WriteLine("\n--- Utilizando o serviço para assinar o documento ---");
        documento.Assinar(servico);
        Console.WriteLine(documento);

        Console.WriteLine("\n--- Tentativa inválida: assinar novamente ---");
        try
        {
            documento.Assinar(servico);
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine($"Falhou: {e.Message}");
        }

        Console.WriteLine("\nJustificativa de projeto:");
        Console.WriteLine("ServicoAssinatura NÃO é armazenado como propriedade de Documento — ele é");
        Console.WriteLine("apenas um colaborador temporário, recebido como parâmetro de Assinar(...).");
        Console.WriteLine("O estado do documento após a operação (Assinado / CarimboAssinatura) pertence");
        Console.WriteLine("exclusivamente a Documento; não há razão de domínio para manter a referência");
        Console.WriteLine("ao serviço além da execução do método.");
    }
}
