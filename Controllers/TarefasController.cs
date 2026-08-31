using ControleTarefas.Domain.Exceptions;
using ControleTarefas.DTOs;
using ControleTarefas.Services;
using Microsoft.AspNetCore.Mvc;

namespace ControleTarefas.Controllers;

[ApiController]
[Route("tarefas")]
public class TarefasController : ControllerBase
{
    private readonly TarefaService _service;

    public TarefasController(TarefaService service)
    {
        _service = service;
    }

    [HttpPost]
    public IActionResult Criar(CreateTarefaDto dto)
    {
        try
        {
            var tarefa = _service.Criar(dto);
            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public IActionResult ListarTodas()
    {
        return Ok(_service.ListarTodas());
    }

    [HttpGet("{id}")]
    public IActionResult ObterPorId(Guid id)
    {
        try
        {
            return Ok(_service.ObterPorId(id));
        }
        catch (NaoEncontradaException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public IActionResult Atualizar(Guid id, UpdateTarefaDto dto)
    {
        try
        {
            return Ok(_service.Atualizar(id, dto));
        }
        catch (NaoEncontradaException ex)
        {
            return NotFound(ex.Message);
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPatch("{id}/concluir")]
    public IActionResult Concluir(Guid id)
    {
        try
        {
            return Ok(_service.Concluir(id));
        }
        catch (NaoEncontradaException ex)
        {
            return NotFound(ex.Message);
        }
        catch (DomainException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public IActionResult Excluir(Guid id)
    {
        try
        {
            _service.Excluir(id);
            return NoContent();
        }
        catch (NaoEncontradaException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
