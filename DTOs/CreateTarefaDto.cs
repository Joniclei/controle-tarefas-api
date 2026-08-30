using System.ComponentModel.DataAnnotations;

namespace ControleTarefas.DTOs;

public class CreateTarefaDto
{
    [Required(ErrorMessage = "O título é obrigatório.")]
    public string Titulo { get; set; } = string.Empty;

    public string? Descricao { get; set; }

    public DateTime? DataPrevistaConclusao { get; set; }


}
