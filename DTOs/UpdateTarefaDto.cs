using System.ComponentModel.DataAnnotations;
using ControleTarefas.Domain;

namespace ControleTarefas.DTOs;


public class UpdateTarefaDto
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    public string Titulo { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    public DateTime? DataPrevistaConclusao { get; set; }

    public StatusTarefa Status { get; set; }
}
