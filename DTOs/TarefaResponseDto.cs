using ControleTarefas.Domain;

namespace ControleTarefas.DTOs;

public class TarefaResponseDto
{
    public Guid Id { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime DataCriacao { get; set; }
    public DateTime? DataPrevistaConclusao { get; set; }
    public DateTime? DataConclusao { get; set; }
    public StatusTarefa Status { get; set; }
}
