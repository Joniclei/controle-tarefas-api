# Controle de Tarefas

API feita em ASP.NET Core (.NET 10). É um CRUD de tarefas com algumas regras de negócio em cima (status, prazo, conclusão).

## Como rodar o projeto

Precisa ter Docker e .NET 10 SDK instalados.

1. Subir o banco (SQL Server rodando em container):
```bash
docker-compose up -d
```

2. Aplicar as migrations (cria o banco e a tabela):
```bash
dotnet ef database update
```

3. Rodar a API:
```bash
dotnet run
```

4. Abrir o Swagger no navegador (a porta aparece no terminal quando você roda o `dotnet run`):
```
https://localhost:PORTA/swagger
```

Ali dá pra testar todos os endpoints direto, sem precisar de Postman.

Para rodar os testes:
```bash
cd ControleTarefas.Tests
dotnet test
```

## Armazenamento

SQL Server, rodando local via Docker Compose (não precisa instalar nada na máquina, só o Docker). A senha que está no `appsettings.Development.json` é só pra esse container local, não é segredo de produção nem nada sensível.

Escolhi SQL Server em vez de banco em memória ou SQLite porque o enunciado aceitava e é provável que seja o banco que a empresa usa de verdade.

## Endpoints

| Método | Rota | O que faz |
|---|---|---|
| POST | /tarefas | Cria uma tarefa |
| GET | /tarefas | Lista todas as tarefas |
| GET | /tarefas/{id} | Busca uma tarefa pelo id |
| PUT | /tarefas/{id} | Atualiza título, descrição, data prevista e status |
| PATCH | /tarefas/{id}/concluir | Marca a tarefa como concluída |
| DELETE | /tarefas/{id} | Exclui a tarefa |

## Regras de negócio implementadas

- **Título é obrigatório.** Validado tanto no DTO (retorna erro mais cedo, mensagem amigável) quanto dentro da entidade `Tarefa` (regra de negócio de verdade, não dá pra burlar).
- **Data prevista de conclusão não pode ser no passado.** Só é validada se o campo foi informado — decidi deixar esse campo opcional, já que o enunciado só chama o título explicitamente de obrigatório.
- **Status só pode ser um dos 4 valores** (Pendente, Em Andamento, Concluída, Cancelada). Isso é garantido pelo próprio enum em C#, e tem uma checagem extra (`Enum.IsDefined`) pra rejeitar um número inválido que não corresponda a nenhum status real.
- **Tarefa concluída não pode voltar para Pendente.** Validado dentro do método que atualiza o status.
- **Ao concluir, a data de conclusão é registrada automaticamente**, o usuário não informa isso, o sistema preenche sozinho.

Toda essa lógica de negócio fica dentro da própria entidade `Tarefa` (pasta `Domain/`), não espalhada pelos controllers ou serviços. A entidade tem construtor privado e os setters são privados também, então a única forma de criar ou alterar uma tarefa é passando pelos métodos que já validam tudo (`Tarefa.Criar(...)`, `Concluir()`, `AtualizarStatus(...)`). Não dá pra criar uma tarefa inválida "por acidente" em nenhum lugar do código.

## Estrutura do projeto

Não usei múltiplos projetos/DLLs separados (Domain, Application, Infrastructure como projetos independentes) porque, pra esse tamanho de desafio, isso ia adicionar complexidade demais. Separei por pastas dentro de um projeto só:

- `Domain/` — entidade `Tarefa`, enum de status, exceptions customizadas
- `DTOs/` — o que a API recebe e devolve
- `Services/` — `TarefaService` (orquestra tudo) e a interface `ITarefaRepository`
- `Infrastructure/` — `AppDbContext` e a implementação real do repository, usando EF Core
- `Controllers/` — os endpoints

## Testes

Escrevi testes unitários (xUnit) cobrindo as regras de negócio da entidade `Tarefa` — criar com título vazio, criar com data no passado, concluir duas vezes, tentar voltar de concluída pra pendente, status inválido, etc. Ficam em `ControleTarefas.Tests/`.

## O que eu melhoraria se tivesse mais tempo

- Fazer o `Status` aparecer como texto (`"Pendente"`) no JSON de resposta, em vez do número do enum — mais legível pra quem consome a API.
- Diferenciar melhor "campo não enviado" de "campo com valor inválido" na validação da data prevista de conclusão (hoje os dois casos caem na mesma regra do Domain, com a mesma mensagem).
- Testes de integração para os endpoints da Controller, além dos testes unitários que já existem (testes que sobem a API de verdade e testam o fluxo completo, incluindo banco).
- Paginação no `GET /tarefas`, caso a lista de tarefas cresça muito.
- Configurar variável de ambiente para a connection string em vez de deixar fixa no `appsettings.Development.json` (aqui deixei assim de propósito, pra facilitar rodar o projeto sem configuração extra).
