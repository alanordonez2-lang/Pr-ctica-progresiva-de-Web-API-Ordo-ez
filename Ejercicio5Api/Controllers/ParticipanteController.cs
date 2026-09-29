using Microsoft.AspNetCore.Mvc;

namespace Ejercicio5.Controllers;

[ApiController]
[Route("[controller]")]
public class ParticipanteController : ControllerBase
{
    public static readonly List<Participante> participantes = new()
    {
        new Participante
        {
            Id = 1,
            Nombre = "Alan",
            Email = "alan@gmail.com",
            Edad = 19
        },
        new Participante
        {
            Id = 2,
            Nombre = "Fabrizzio",
            Email = "fabrizzio@gmail.com",
            Edad = 19
        }
    };
    private readonly ILogger<ParticipanteController> _logger;

    public ParticipanteController(ILogger<ParticipanteController> logger)
    {
        _logger = logger;
    }
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(participantes);
    }

    [HttpPost]
    public IActionResult CrearParticipante(Participante nuevoParticipante)
    {
        bool emailExiste = participantes.Any(x =>
            x.Email.ToLower() == nuevoParticipante.Email.ToLower());

        if (emailExiste)
            return BadRequest("Ya existe un participante con ese email.");

        nuevoParticipante.Id = participantes.Count + 1;

        participantes.Add(nuevoParticipante);

        return Ok(nuevoParticipante);
    }

    [HttpGet("{id}/actividades")]
    public IActionResult ActividadesParticipante(int id)
    {
        var participante = participantes
            .FirstOrDefault(x => x.Id == id);

        if (participante == null)
            return NotFound("El participante no existe.");

        var resultado = ReservaActividadController.reservas
            .Where(x => x.ParticipanteId == id)
            .Join(
                ActividadController.actividades,
                reserva => reserva.ActividadId,
                actividad => actividad.Id,
                (reserva, actividad) => actividad
            )
            .ToList();

        return Ok(resultado);
    }
}