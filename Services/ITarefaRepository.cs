using ControleTarefas.Domain;

namespace ControleTarefas.Services;

public interface ITarefaRepository
{
    Tarefa? GetById(Guid id);
    List<Tarefa> GetAll();
    void Add(Tarefa tarefa);
    void Update(Tarefa tarefa);
    void Delete(Guid id);
}
