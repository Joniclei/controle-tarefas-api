using ControleTarefas.Domain;
using Microsoft.EntityFrameworkCore;

namespace ControleTarefas.Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Tarefa> Tarefas { get; set; } = null!;
}
