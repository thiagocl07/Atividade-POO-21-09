#nullable enable
using System;

// =========================================================
// Exercício 2 - Atendimento em clínica veterinária
// Vários atendimentos podem compartilhar a MESMA referência
// de Animal. Alterações feitas por meio de um comportamento
// válido de Animal são visíveis por qualquer caminho de acesso.
// =========================================================

public class Animal
{
    public string Nome { get; private set; }
    public string Especie { get; }
    public int AnoNascimento { get; }

    public Animal(string nome, string especie, int anoNascimento)
    {
        if (string.IsNullOrWhiteSpace(nome))
            throw new ArgumentException("O nome do animal é obrigatório.", nameof(nome));

        if (string.IsNullOrWhiteSpace(especie))
            throw new ArgumentException("A espécie é obrigatória.", nameof(especie));

        // Regra documentada: ano de nascimento deve estar entre 1900 e o ano atual.
        if (anoNascimento < 1900 || anoNascimento > DateTime.Today.Year)
            throw new ArgumentException(
                $"Ano de nascimento inválido. Deve estar entre 1900 e {DateTime.Today.Year}.",
                nameof(anoNascimento));

        Nome = nome;
        Especie = especie;
        AnoNascimento = anoNascimento;
    }

    // Alteração controlada de estado (não é um "public set").
    public void Renomear(string novoNome)
    {
        if (string.IsNullOrWhiteSpace(novoNome))
            throw new ArgumentException("O novo nome não pode ser vazio.", nameof(novoNome));

        Nome = novoNome;
    }

    public override string ToString() => $"{Nome} ({Especie}, nascido em {AnoNascimento})";
}

public class AtendimentoVeterinario
{
    public string Motivo { get; }
    public DateTime Data { get; }

    // Apenas referência; nome/espécie/ano NÃO são copiados aqui.
    public Animal Animal { get; }

    public AtendimentoVeterinario(string motivo, DateTime data, Animal animal)
    {
        if (string.IsNullOrWhiteSpace(motivo))
            throw new ArgumentException("O motivo do atendimento é obrigatório.", nameof(motivo));

        Animal = animal ?? throw new ArgumentNullException(nameof(animal),
            "Nenhum atendimento pode ser criado sem identificar o animal atendido.");

        Data = data;
        Motivo = motivo;
    }

    public override string ToString() => $"Atendimento em {Data:dd/MM/yyyy} - {Motivo} | Animal: {Animal}";
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Exercício 2 - Atendimento em clínica veterinária ===\n");

        var animal = new Animal("Rex", "Cachorro", 2020);

        var atendimento1 = new AtendimentoVeterinario("Vacinação anual", new DateTime(2026, 1, 10), animal);
        var atendimento2 = new AtendimentoVeterinario("Consulta de rotina", new DateTime(2026, 4, 2), animal);

        Console.WriteLine(atendimento1);
        Console.WriteLine(atendimento2);

        Console.WriteLine($"\nOs dois atendimentos apontam para o MESMO objeto Animal? " +
            $"{ReferenceEquals(atendimento1.Animal, atendimento2.Animal)}");

        Console.WriteLine("\n--- Alterando o nome do animal por meio de um comportamento válido ---");
        animal.Renomear("Rex II");

        Console.WriteLine($"Pela variável original: {animal.Nome}");
        Console.WriteLine($"Pelo atendimento 1:     {atendimento1.Animal.Nome}");
        Console.WriteLine($"Pelo atendimento 2:     {atendimento2.Animal.Nome}");

        Console.WriteLine("\n--- Tentativas inválidas ---");

        try
        {
            var animalInvalido = new Animal("Totó", "Gato", 1850);
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Falhou ao criar animal com ano inválido: {e.Message}");
        }

        try
        {
            var atendimentoSemAnimal = new AtendimentoVeterinario("Check-up", DateTime.Today, null!);
        }
        catch (ArgumentNullException e)
        {
            Console.WriteLine($"Falhou ao criar atendimento sem animal: {e.Message}");
        }
    }
}
