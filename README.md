# Atividade de POO - 21/09

Este repositório reúne uma série de exercícios práticos de Programação Orientada a Objetos (POO), com foco em associação entre objetos, responsabilidade de classes, encapsulamento e validação de regras de domínio.

## Objetivo

Os exercícios demonstram como modelar corretamente:

- associações obrigatórias e opcionais;
- referências entre objetos;
- validação de dados em classes corretas;
- uso adequado de `null` e nulabilidade em C#;
- colaboração temporária entre objetos e serviços.

---

## Estrutura do projeto

- `Exercicio1_ManutencaoEquipamento.cs`  
  Modela a relação entre `Manutencao` e `Tecnico`, com associação obrigatória e referência compartilhada.

- `Exercicio2_ClinicaVeterinaria.cs`  
  Explora a reutilização de um mesmo objeto `Animal` por diversos atendimentos.

- `Exercicio3_EncomendaDestinatario.cs`  
  Demonstra a diferença entre dados obrigatórios e opcionais, como telefone e documento.

- `Exercicio4_MatriculaCursoLivre.cs`  
  Representa uma matrícula com duas referências obrigatórias: `Pessoa` e `Curso`.

- `Exercicio5_VeiculoVaga.cs`  
  Apresenta associação opcional e mutável, com regras de domínio para ocupar ou liberar uma vaga.

- `Exercicio6_DocumentoAssinatura.cs`  
  Mostra o uso de um serviço como colaboração temporária, e não como propriedade permanente do documento.

- `Respostas_Questoes_Reflexao.md`  
  Contém as respostas analisadas para as questões de reflexão e raciocínio sobre o projeto.

---

## Como executar os exercícios

Cada arquivo é um exemplo independente em C# e contém sua própria classe `Program`, então o ideal é executar um exercício por vez em um projeto console.

### Exemplo no Windows (PowerShell)

1. Crie um projeto console:

```powershell
dotnet new console -n Exercicio1
cd Exercicio1
```

2. Substitua o conteúdo de `Program.cs` pelo código do arquivo desejado, por exemplo:

```powershell
Copy-Item "C:\caminho\para\Atividadedia21POO\Exercicio1_ManutencaoEquipamento.cs" ".\Program.cs"
```

3. Execute:

```powershell
dotnet run
```

> Cada exercício pode ser executado dessa forma, um a um, em projetos separados.

---

## Conceitos abordados

### 1. Associação por referência
Os objetos guardam a referência para outros objetos, em vez de duplicar dados.

### 2. Validação de domínio
Cada classe valida apenas o que pertence ao seu próprio estado e responsabilidade.

### 3. Encapsulamento
A alteração de estado ocorre por meio de métodos controlados, não por `set` público desnecessário.

### 4. Nulabilidade
Algumas associações são obrigatórias (`string`, objetos obrigatórios), enquanto outras são legítimas em estado livre (`null` em vaga livre).

### 5. Colaboração temporária
Um serviço pode ser usado apenas durante a execução de uma operação, sem virar parte do estado do objeto principal.

---

## Observações

- O projeto foi desenvolvido em C# com suporte a `#nullable enable`.
- Os exemplos foram pensados para reforçar a prática de modelagem orientada a objetos e boas decisões de design.
- A lógica de cada exercício está explicada no arquivo de reflexão.

---

## Autor

Projeto desenvolvido como atividade de estudo de POO.
