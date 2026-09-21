#nullable enable
using System;

// =========================================================
// Exercício 4 - Matrícula em curso livre
// Matricula mantém DUAS referências obrigatórias (Pessoa e
// Curso), ambas exigidas desde a construção. Cada classe
// valida apenas o que é de sua responsabilidade.
// =========================================================

public class Pessoa
{
    public string Nome { get; }
    public string Email { get; }

    public Pessoa(string nome, string email)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome da pessoa é obrigatório.", nameof(nome));

        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new ArgumentException("O e-mail informado é inválido.", nameof(email));

        Nome = nome;
        Email = email;
    }

    public override string ToString() => $"{Nome} <{Email}>";
}

public class Curso
{
    public string Titulo { get; }
    public int CargaHoraria { get; }

    public Curso(string titulo, int cargaHoraria)
    {
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("O título do curso é obrigatório.", nameof(titulo));

        if (cargaHoraria <= 0)
            throw new ArgumentException("A carga horária deve ser maior que zero.", nameof(cargaHoraria));

        Titulo = titulo;
        CargaHoraria = cargaHoraria;
    }

    public override string ToString() => $"{Titulo} ({CargaHoraria}h)";
}

public class Matricula
{
    public DateTime Data { get; }
    public Pessoa Pessoa { get; }
    public Curso Curso { get; }

    public Matricula(DateTime data, Pessoa pessoa, Curso curso)
    {
        // Matricula NÃO repete a validação de e-mail nem de carga horária;
        // apenas garante que recebeu referências válidas (não nulas).
        Pessoa = pessoa ?? throw new ArgumentNullException(nameof(pessoa),
            "Uma matrícula não faz sentido sem uma pessoa.");

        Curso = curso ?? throw new ArgumentNullException(nameof(curso),
            "Uma matrícula não faz sentido sem um curso.");

        Data = data;
    }

    public override string ToString() =>
        $"Matrícula em {Data:dd/MM/yyyy} - {Pessoa.Nome} em \"{Curso.Titulo}\"";
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Exercício 4 - Matrícula em curso livre ===\n");

        var pessoa1 = new Pessoa("João Pedro", "joao@email.com");
        var pessoa2 = new Pessoa("Ana Paula", "ana@email.com");

        var curso1 = new Curso("Lógica de Programação", 40);
        var curso2 = new Curso("Introdução ao C#", 60);

        var matricula1 = new Matricula(DateTime.Today, pessoa1, curso1);
        var matricula2 = new Matricula(DateTime.Today, pessoa1, curso2);
        var matricula3 = new Matricula(DateTime.Today, pessoa2, curso1);

        foreach (var m in new[] { matricula1, matricula2, matricula3 })
            Console.WriteLine(m);

        Console.WriteLine("\n--- Navegando pelas referências ---");
        Console.WriteLine($"Matrícula 1 -> Pessoa: {matricula1.Pessoa.Email} | Curso: {matricula1.Curso.CargaHoraria}h");

        Console.WriteLine("\n--- Tentativas inválidas ---");

        try
        {
            var pessoaInvalida = new Pessoa("Beltrano", "email-sem-arroba");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Falhou ao criar pessoa com e-mail inválido: {e.Message}");
        }

        try
        {
            var cursoInvalido = new Curso("Curso Fantasma", 0);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Falhou ao criar curso com carga horária inválida: {e.Message}");
        }

        try
        {
            var matriculaSemCurso = new Matricula(DateTime.Today, pessoa1, null!);
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine($"Falhou ao criar matrícula sem curso: {e.Message}");
        }
    }
}
