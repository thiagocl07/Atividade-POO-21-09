# Respostas — Questões de análise e reflexão

## Exercício 1 — Manutenção de equipamento

**1. Técnico faz parte do estado de Manutenção?**
Sim, mas apenas como **referência**. Manutenção guarda o objeto `Tecnico`, nunca uma cópia de seus dados.

**2. Manutenção poderia existir validamente sem Técnico?**
Não. A regra de domínio exige técnico responsável desde a criação, por isso o construtor de `Manutencao` recusa `tecnico == null`.

**3. Quem deve validar o registro profissional?**
A própria classe `Tecnico`, pois é dado que pertence exclusivamente a ela.

**4. Seria correto Manutenção validar `Tecnico.Nome`?**
Não. Isso duplicaria responsabilidade e acoplaria `Manutencao` às regras internas de `Tecnico`, violando encapsulamento.

**5. O que aconteceria se os dados do técnico fossem copiados para Manutenção?**
Haveria duplicação de estado: uma alteração no técnico (ex.: correção de nome) não se refletiria nas manutenções já registradas, gerando inconsistência entre os dados "reais" e os copiados.

---

## Exercício 2 — Atendimento em clínica veterinária

**6. Foram criados três animais ou apenas um?**
Apenas um. Os dois atendimentos referenciam o **mesmo** objeto `Animal`.

**7. Quantas referências apontam para esse objeto?**
Três: a variável `animal` e as propriedades `Animal` de `atendimento1` e `atendimento2`.

**8. Por que a mudança pode ser observada pelos diferentes caminhos?**
Porque todas as referências apontam para o mesmo endereço/objeto na memória; alterar o objeto por qualquer caminho altera o único objeto existente.

**9. Qual seria a diferença se os dados do animal tivessem sido copiados para cada atendimento?**
Cada atendimento teria sua própria cópia desatualizada; renomear o animal não se refletiria nos atendimentos, e o sistema perderia a noção de que se trata do mesmo animal.

---

## Exercício 3 — Encomenda e destinatário

**10. A ausência de telefone torna um destinatário inválido?**
Não. O telefone é opcional por regra de domínio (pode não estar disponível no cadastro), por isso é `string?`.

**11. A ausência de documento também deveria ser permitida?**
Não. O documento foi definido como obrigatório pela regra de domínio (identifica de forma única o destinatário), por isso é `string` não anulável.

**12. Encomenda deve verificar se `Destinatario.Nome` está preenchido?**
Não. Isso já é garantido pelo próprio construtor de `Destinatario`; repetir a validação em `Encomenda` duplicaria responsabilidade.

**13. Por que Destinatario deve proteger essa regra?**
Porque o dado (nome) pertence ao estado de `Destinatario`; só ele pode garantir que nunca existirá em estado inválido, independentemente de quem o utilize.

**14. Ser não anulável em C# elimina a necessidade de validação em tempo de execução?**
Não totalmente. `string` (não anulável) ajuda o compilador a alertar sobre uso indevido, mas não impede, em tempo de execução, que alguém passe `""` ou uma string vazia — por isso a validação explícita no construtor continua necessária.

---

## Exercício 4 — Matrícula em curso livre

**15. As duas referências fazem parte do estado de Matricula?**
Sim, `Pessoa` e `Curso` são parte do estado de `Matricula`, guardadas por referência.

**16. Ambas são obrigatórias?**
Sim, uma matrícula não faz sentido sem pessoa e sem curso; ambas são exigidas no construtor.

**17. Qual classe deve validar o e-mail?**
`Pessoa`, pois o e-mail é dado que pertence a ela.

**18. Qual classe deve validar a carga horária?**
`Curso`, pelo mesmo motivo.

**19. Seria correto Matricula repetir essas validações?**
Não. Repetir violaria a regra "não replique validações que são responsabilidade de outra classe" e criaria múltiplas fontes de verdade.

**20. Pessoa precisa obrigatoriamente conhecer Matricula para que exista uma associação?**
Não. A associação pode ser unidirecional: `Matricula` conhece `Pessoa` e `Curso`, mas o inverso não é necessário para este domínio.

---

## Exercício 5 — Veículo e vaga de estacionamento

**21. A referência para Veiculo deve possuir o mesmo tratamento de nulabilidade utilizado em associações obrigatórias?**
Não. Aqui `null` é um estado válido do domínio (vaga livre), então `Veiculo?` é o tipo correto — diferente das associações obrigatórias (não anuláveis) dos exercícios anteriores.

**22. `null`, neste caso, representa um erro ou um estado legítimo do domínio?**
Um estado legítimo: toda vaga nasce livre, e liberar uma vaga volta a esse mesmo estado.

**23. Por que colocar Veiculo obrigatoriamente no construtor de Vaga produziria uma modelagem inadequada?**
Porque contradiria a regra de domínio de que vagas existem e são cadastradas **antes** de qualquer veículo chegar; forçaria a criação de um veículo fictício só para instanciar a vaga.

**24. Quem deve decidir quando a associação pode mudar?**
A própria `Vaga`, por meio dos métodos `Ocupar(...)` e `Liberar()`, que aplicam as regras (não ocupar vaga já ocupada, não liberar vaga já livre).

**25. Um `public set` seria apropriado?**
Não. Um `public set` permitiria trocar o veículo de forma direta e descontrolada, sem respeitar as regras de negócio (ex.: substituir veículo sem liberar a vaga antes).

---

## Exercício 6 — Documento e serviço de assinatura

**26. ServicoAssinatura é uma entidade relevante?**
É relevante como **colaborador técnico** da operação, mas não como parte do estado do documento.

**27. O documento utiliza esse objeto?**
Sim, mas apenas durante a execução do método `Assinar(...)`.

**28. Apenas utilizar outro objeto caracteriza associação?**
Não. Uso temporário dentro de uma operação é **colaboração**, não associação estrutural/persistente.

**29. A referência precisa permanecer depois que o método terminar?**
Não. Após a assinatura, o documento só precisa manter o **resultado** (situação de assinatura/carimbo), não o serviço em si.

**30. Ela faz parte do estado do documento?**
Não. `ServicoAssinatura` não é campo de `Documento`; apenas passa como parâmetro.

**31. Qual diferença existe entre manter ServicoAssinatura como propriedade e recebê-lo apenas como parâmetro de Assinar(...)?**
Como propriedade, criaria uma dependência permanente e desnecessária, sugerindo (erroneamente) que o documento "pertence" a um serviço específico. Como parâmetro, o documento permanece desacoplado, podendo ser assinado por diferentes serviços ao longo do tempo, sem carregar esse vínculo em seu estado.

**32. Qual das alternativas representa melhor o domínio apresentado?**
Recebê-lo como parâmetro do método `Assinar(...)` — é a alternativa implementada, por representar fielmente uma colaboração temporária, e não uma associação persistente.
