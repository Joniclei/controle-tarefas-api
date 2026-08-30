namespace ControleTarefas.Domain;

using ControleTarefas.Domain.Exceptions;

public class Tarefa
{
  public Guid Id { get; private set; }
  public string Titulo { get; private set; } = string.Empty;
  public string? Descricao { get; private set; }
  public DateTime DataCriacao { get; private set; }
  public DateTime? DataPrevistaConclusao { get; private set; }
  public DateTime? DataConclusao { get; private set; }
  public StatusTarefa Status { get; private set; }

  private Tarefa()
  {
  }

 public static Tarefa Criar(string titulo, string? descricao, DateTime? dataPrevistaConclusao)
{
    ValidarTitulo(titulo);
    ValidarDataPrevistaConclusao(dataPrevistaConclusao);

    return new Tarefa
    {
        Id = Guid.NewGuid(),
        Titulo = titulo,
        Descricao = descricao,
        DataCriacao = DateTime.Now,
        DataPrevistaConclusao = dataPrevistaConclusao,
        Status = StatusTarefa.Pendente
    };
}

  private static void ValidarTitulo(string titulo)
  {
    if (string.IsNullOrWhiteSpace(titulo))
    {
      throw new DomainException("O título da tarefa é obrigatório.");
    }
  }

 private static void ValidarDataPrevistaConclusao(DateTime? dataPrevistaConclusao)
  {
      if (dataPrevistaConclusao.HasValue && dataPrevistaConclusao.Value.Date < DateTime.Now.Date)
      {
          throw new DomainException("A data prevista de conclusão não pode ser menor que a data atual.");
      }
  }


  public void AtualizarDados(string titulo, string? descricao, DateTime? dataPrevistaConclusao)
  {
    ValidarTitulo(titulo);
    ValidarDataPrevistaConclusao(dataPrevistaConclusao);

    Titulo = titulo;
    Descricao = descricao;
    DataPrevistaConclusao = dataPrevistaConclusao;
  }

  public void Concluir()
  {
    if (Status == StatusTarefa.Concluida)
    {
      throw new DomainException("A tarefa já está concluída.");
    }

    Status = StatusTarefa.Concluida;
    DataConclusao = DateTime.Now;
  }

  public void AtualizarStatus(StatusTarefa novoStatus)
{
    if (!Enum.IsDefined(typeof(StatusTarefa), novoStatus))
    {
        throw new DomainException("Status inválido.");
    }

    if (Status == StatusTarefa.Concluida && novoStatus == StatusTarefa.Pendente)
    {
        throw new DomainException("Uma tarefa concluída não pode voltar para o status Pendente.");
    }

    if (novoStatus == StatusTarefa.Concluida)
    {
        Concluir();
        return;
    }

    Status = novoStatus;
}


}
