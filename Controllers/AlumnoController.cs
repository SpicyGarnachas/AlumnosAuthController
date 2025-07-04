﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SafeVaultExample.Features.Commands;
using SafeVaultExample.Features.Queries;
using SafeVaultExample.Models;

namespace SafeVaultExample.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class AlumnosController(IMediator mediator) : ControllerBase
{
    [HttpGet]
    [Authorize(Roles="Admin")]
    public async Task<ActionResult<List<Alumno>>> GetAll()
    {
        var result = await mediator.Send(new GetAllAlumnosQuery());
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Alumno>> GetById(int id)
    {
        var alumno = await mediator.Send(new GetAlumnoByIdQuery(id));
        return alumno is null ? NotFound() : Ok(alumno);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<bool>> Create(CreateAlumnoCommand command)
    {
        var success = await mediator.Send(command);

        if (!success)
            return BadRequest("No se pudo crear el alumno. Verifica los datos.");

        return Ok(true);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<bool>> Update(int id, UpdateAlumnoCommand command)
    {
        if (id != command.Id)
            return BadRequest("El ID en la ruta no coincide con el del cuerpo de la solicitud.");

        var isUpdated = await mediator.Send(command);

        if (!isUpdated)
            return NotFound("No se encontró el alumno o los datos no son válidos.");

        return Ok(true);
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        var success = await mediator.Send(new DeleteAlumnoCommand(id));
        return success ? NoContent() : NotFound();
    }
}
