#nullable enable
using System;

// =========================================================
// Exercício 5 - Veículo e vaga de estacionamento
// Diferente dos exercícios anteriores: a associação Vaga->Veiculo
// é OPCIONAL (null é estado legítimo, não erro) e MUTÁVEL ao
// longo do tempo, mas só pode mudar por meio de comportamentos
// controlados (Ocupar/Liberar) — nunca por um "public set".
// =========================================================

public class Veiculo
{
    public string Placa { get; }
    public string Modelo { get; }

    public Veiculo(string placa, string modelo)
    {
        if (string.IsNullOrWhiteSpace(placa))
            throw new ArgumentException("A placa do veículo é obrigatória.", nameof(placa));

        if (string.IsNullOrWhiteSpace(modelo))
            throw new ArgumentException("O modelo do veículo é obrigatório.", nameof(modelo));

        Placa = placa;
        Modelo = modelo;
    }

    public override string ToString() => $"{Modelo} (Placa: {Placa})";
}

public class Vaga
{
    public string Codigo { get; }
    public string Setor { get; }

    // Nulável por natureza do domínio: uma vaga pode legitimamente estar livre.
    public Veiculo? Veiculo { get; private set; }

    public Vaga(string codigo, string setor)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            throw new ArgumentException("O código da vaga é obrigatório.", nameof(codigo));

        if (string.IsNullOrWhiteSpace(setor))
            throw new ArgumentException("O setor da vaga é obrigatório.", nameof(setor));

        Codigo = codigo;
        Setor = setor;
        Veiculo = null; // toda vaga nasce livre
    }

    public bool EstaLivre => Veiculo is null;

    public void Ocupar(Veiculo veiculo)
    {
        if (veiculo is null)
            throw new ArgumentNullException(nameof(veiculo), "É necessário informar um veículo para ocupar a vaga.");

        if (!EstaLivre)
            throw new InvalidOperationException($"A vaga {Codigo} já está ocupada por outro veículo.");

        Veiculo = veiculo;
    }

    public void Liberar()
    {
        if (EstaLivre)
            throw new InvalidOperationException($"A vaga {Codigo} já está livre.");

        Veiculo = null;
    }

    public override string ToString() =>
        $"Vaga {Codigo} ({Setor}) - {(EstaLivre ? "livre" : $"ocupada por {Veiculo}")}";
}

class Program
{
    static void Main()
    {
        Console.WriteLine("=== Exercício 5 - Veículo e vaga de estacionamento ===\n");

        var vaga = new Vaga("A-12", "Setor A");
        Console.WriteLine(vaga);

        var veiculo = new Veiculo("ABC1D23", "Fiat Argo");

        Console.WriteLine("\n--- Ocupando a vaga ---");
        vaga.Ocupar(veiculo);
        Console.WriteLine(vaga);
        Console.WriteLine($"Veículo relacionado: {vaga.Veiculo}");

        Console.WriteLine("\n--- Liberando a vaga ---");
        vaga.Liberar();
        Console.WriteLine(vaga);

        Console.WriteLine("\n--- Tentativas inválidas ---");

        try
        {
            vaga.Liberar(); // já está livre
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine($"Falhou ao liberar vaga já livre: {e.Message}");
        }

        vaga.Ocupar(veiculo);
        try
        {
            var outroVeiculo = new Veiculo("XYZ9K88", "Chevrolet Onix");
            vaga.Ocupar(outroVeiculo); // já ocupada
        }
        catch (InvalidOperationException e)
        {
            Console.WriteLine($"Falhou ao ocupar vaga já ocupada: {e.Message}");
        }

        try
        {
            var vagaInvalida = new Vaga("", "Setor B");
        }
        catch (ArgumentException e)
        {
            Console.WriteLine($"Falhou ao criar vaga sem código: {e.Message}");
        }

        // Observação: não existe "public set" para Veiculo. A única forma de
        // alterar a associação é por meio de Ocupar(...) e Liberar(...), que
        // aplicam as regras do domínio (ex.: não sobrescrever vaga ocupada).
    }
}
