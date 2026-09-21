#nullable enable
using System;

// =========================================================
// Exercício 1 - Manutenção de equipamento
// Associação obrigatória e simples: Manutenção SEMPRE precisa
// de um Técnico desde sua criação. A Manutenção guarda apenas
// a REFERÊNCIA ao Técnico, nunca copia nome/registro.
// =========================================================

public class Tecnico
{
    public string Nome { get; }
    public string Registro { get; }

    public Tecnico(string nome, string registro)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do técnico é obrigatório.", nameof(nome));

        if (string.IsNullOrWhiteSpace(registro))
            throw new ArgumentException("O número de registro profissional é obrigatório.", nameof(registro));

        Nome = nome;
        Registro = registro;
    }

    public override string ToString() => $"{Nome} (Registro: {Registro})";
}

public class Manutencao
{
    public string Descricao { get; }
    public DateTime Data { get; }

    // Referência ao técnico responsável. Só pode ser definida no construtor
    // (get-only), protegendo a associação da mesma forma que um valor simples.
    public Tecnico Tecnico { get; }

    public Manutencao(string descricao, DateTime data, Tecnico tecnico)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("A descrição do problema é obrigatória.", nameof(descricao));

        if (data == default)
            throw new ArgumentException("A data da manutenção deve ser informada.", nameof(data));

        // Manutenção só valida que RECEBEU um técnico; quem valida nome/registro
        // do técnico é a própria classe Tecnico (responsabilidade única).
        Tecnico = tecnico ?? throw new ArgumentNullException(nameof(tecnico),
            "Toda manutenção deve possuir um técnico responsável desde sua criação.");

        Descricao = descricao;
        Data = data;
    }

    public override string ToString() =>
        $"Manutenção em {Data:dd/MM/yyyy} - \"{Descricao}\" | Técnico: {Tecnico}";
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Exercício 1 - Manutenção de equipamento ===\n");

        var tecnico1 = new Tecnico("Carlos Silva", "CRT-1234");
        var tecnico2 = new Tecnico("Fernanda Souza", "CRT-5678");

        var manutencao1 = new Manutencao("Troca de rolamento do motor", new DateTime(2026, 3, 10), tecnico1);
        var manutencao2 = new Manutencao("Calibração de sensor de temperatura", new DateTime(2026, 3, 15), tecnico2);

        Console.WriteLine(manutencao1);
        Console.WriteLine(manutencao2);

        // Acessando os dados do técnico POR MEIO da manutenção (sem duplicação).
        Console.WriteLine($"\nTécnico responsável pela manutenção 1: {manutencao1.Tecnico.Nome} - {manutencao1.Tecnico.Registro}");

        // Provando que não há cópia: é a MESMA referência de objeto.
        Console.WriteLine($"Manutenção guarda a mesma referência do técnico original? " +
            $"{ReferenceEquals(tecnico1, manutencao1.Tecnico)}");

        Console.WriteLine("\n--- Tentativas inválidas ---");

        try
        {
            var tecnicoInvalido = new Tecnico("", "CRT-0001");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Falhou ao criar técnico sem nome: {e.Message}");
        }

        try
        {
            var manutencaoSemTecnico = new Manutencao("Revisão geral", DateTime.Today, null!);
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine($"Falhou ao criar manutenção sem técnico: {e.Message}");
        }

        try
        {
            var manutencaoSemDescricao = new Manutencao("   ", DateTime.Today, tecnico1);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Falhou ao criar manutenção sem descrição: {e.Message}");
        }
    }
}
