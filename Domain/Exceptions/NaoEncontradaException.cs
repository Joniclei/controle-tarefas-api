namespace ControleTarefas.Domain.Exceptions;

public class NaoEncontradaException : Exception
{
  public NaoEncontradaException(string message) : base(message)
  {

  }
}
