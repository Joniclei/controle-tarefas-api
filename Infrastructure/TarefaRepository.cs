using ControleTarefas.Domain;
using ControleTarefas.Services;
using Microsoft.EntityFrameworkCore;

namespace ControleTarefas.Infrastructure;

public class TarefaRepository : ITarefaRepository
{
    private readonly AppDbContext _context;

    public TarefaRepository(AppDbContext context)
    {
        _context = context;
    }

    public Tarefa? GetById(Guid id)
    {
        return _context.Tarefas.FirstOrDefault(t => t.Id == id);
    }

    public List<Tarefa> GetAll()
    {
        return _context.Tarefas.ToList();
    }

    public void Add(Tarefa tarefa)
    {
        _context.Tarefas.Add(tarefa);
        _context.SaveChanges();
    }

    public void Update(Tarefa tarefa)
    {
        _context.SaveChanges();
    }

    public void Delete(Guid id)
    {
        var tarefa = GetById(id);
        if (tarefa != null)
        {
            _context.Tarefas.Remove(tarefa);
            _context.SaveChanges();
        }
    }
}
