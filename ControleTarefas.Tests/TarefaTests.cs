using ControleTarefas.Domain;
using ControleTarefas.Domain.Exceptions;

namespace ControleTarefas.Tests;

public class TarefaTests
{
    [Fact]
    public void Criar_ComTituloValido_DeveCriarComStatusPendente()
    {
        var tarefa = Tarefa.Criar("Estudar C#", "descricao", DateTime.Now.AddDays(5));

        Assert.Equal("Estudar C#", tarefa.Titulo);
        Assert.Equal(StatusTarefa.Pendente, tarefa.Status);
        Assert.Null(tarefa.DataConclusao);
    }

    [Fact]
    public void Criar_ComTituloVazio_DeveLancarDomainException()
    {
        Assert.Throws<DomainException>(() =>
            Tarefa.Criar("", "descricao", DateTime.Now.AddDays(5)));
    }

    [Fact]
    public void Criar_ComDataPrevistaNoPassado_DeveLancarDomainException()
    {
        Assert.Throws<DomainException>(() =>
            Tarefa.Criar("Titulo valido", "descricao", DateTime.Now.AddDays(-1)));
    }

    [Fact]
    public void Criar_SemDataPrevista_NaoDeveLancarExcecao()
    {
        var tarefa = Tarefa.Criar("Titulo valido", "descricao", null);

        Assert.Null(tarefa.DataPrevistaConclusao);
    }

    [Fact]
    public void Concluir_DeveDefinirStatusConcluidaEDataConclusao()
    {
        var tarefa = Tarefa.Criar("Titulo", "desc", DateTime.Now.AddDays(1));

        tarefa.Concluir();

        Assert.Equal(StatusTarefa.Concluida, tarefa.Status);
        Assert.NotNull(tarefa.DataConclusao);
    }

    [Fact]
    public void Concluir_QuandoJaConcluida_DeveLancarDomainException()
    {
        var tarefa = Tarefa.Criar("Titulo", "desc", DateTime.Now.AddDays(1));
        tarefa.Concluir();

        Assert.Throws<DomainException>(() => tarefa.Concluir());
    }

    [Fact]
    public void AtualizarStatus_DeConcluidaParaPendente_DeveLancarDomainException()
    {
        var tarefa = Tarefa.Criar("Titulo", "desc", DateTime.Now.AddDays(1));
        tarefa.Concluir();

        Assert.Throws<DomainException>(() => tarefa.AtualizarStatus(StatusTarefa.Pendente));
    }

    [Fact]
    public void AtualizarStatus_ComValorInvalido_DeveLancarDomainException()
    {
        var tarefa = Tarefa.Criar("Titulo", "desc", DateTime.Now.AddDays(1));

        Assert.Throws<DomainException>(() => tarefa.AtualizarStatus((StatusTarefa)99));
    }
}
