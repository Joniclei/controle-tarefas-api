using ControleTarefas.Domain;
using ControleTarefas.Domain.Exceptions;
using ControleTarefas.DTOs;

namespace ControleTarefas.Services;

public class TarefaService
{
    private readonly ITarefaRepository _repository;

    public TarefaService(ITarefaRepository repository)
    {
        _repository = repository;
    }

    public TarefaResponseDto Criar(CreateTarefaDto dto)
    {
        var tarefa = Tarefa.Criar(dto.Titulo, dto.Descricao, dto.DataPrevistaConclusao);
        _repository.Add(tarefa);
        return MapearParaDto(tarefa);
    }

    public List<TarefaResponseDto> ListarTodas()
    {
        return _repository.GetAll().Select(MapearParaDto).ToList();
    }

    public TarefaResponseDto ObterPorId(Guid id)
    {
        var tarefa = _repository.GetById(id);
        if (tarefa == null)
        {
            throw new NaoEncontradaException("Tarefa não encontrada.");
        }
        return MapearParaDto(tarefa);
    }

    public TarefaResponseDto Atualizar(Guid id, UpdateTarefaDto dto)
    {
        var tarefa = _repository.GetById(id);
        if (tarefa == null)
        {
            throw new NaoEncontradaException("Tarefa não encontrada.");
        }

        tarefa.AtualizarDados(dto.Titulo, dto.Descricao, dto.DataPrevistaConclusao);
        tarefa.AtualizarStatus(dto.Status);
        _repository.Update(tarefa);

        return MapearParaDto(tarefa);
    }

    public TarefaResponseDto Concluir(Guid id)
    {
        var tarefa = _repository.GetById(id);
        if (tarefa == null)
        {
            throw new NaoEncontradaException("Tarefa não encontrada.");
        }

        tarefa.Concluir();
        _repository.Update(tarefa);

        return MapearParaDto(tarefa);
    }

    public void Excluir(Guid id)
    {
        var tarefa = _repository.GetById(id);
        if (tarefa == null)
        {
            throw new NaoEncontradaException("Tarefa não encontrada.");
        }

        _repository.Delete(id);
    }

    private static TarefaResponseDto MapearParaDto(Tarefa tarefa)
    {
        return new TarefaResponseDto
        {
            Id = tarefa.Id,
            Titulo = tarefa.Titulo,
            Descricao = tarefa.Descricao,
            DataCriacao = tarefa.DataCriacao,
            DataPrevistaConclusao = tarefa.DataPrevistaConclusao,
            DataConclusao = tarefa.DataConclusao,
            Status = tarefa.Status
        };
    }
}

