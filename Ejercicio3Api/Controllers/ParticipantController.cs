using Microsoft.AspNetCore.Mvc;

namespace Ejercicio3.Controllers;

[ApiController]
[Route("[controller]")]
public class ParticipantController : ControllerBase
{
    public static readonly List<Participant>participants = new()
    {
        new Participant
        {
            Id=1,
            Nombre = "Alan",
            Email = "alano@gmial.com",
            Nivel = "pro"
        }
    };
    private readonly ILogger<ParticipantController> _logger;

    public ParticipantController(ILogger<ParticipantController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(participants);
    }
    [HttpPost]
    public IActionResult CrearParticipante(Participant newParticipant)
    {
        bool emailExiste = participants.Any(x => x.Email == newParticipant.Email);

        if (emailExiste)
            return BadRequest("El email ya está registrado.");

        newParticipant.Id = participants.Count + 1;

        participants.Add(newParticipant);

        return Ok(newParticipant);
    }
    [HttpGet("puntaje/{puntaje}")]
    public IActionResult ParticipantesPorPuntaje(int puntaje)
    {
        var resultado = participants.Where(p => ResolutionController.resolutions.Where(r => r.ParticipanteId == p.Id).Sum(r => r.PuntajeObtenido) > puntaje).ToList();

        return Ok(resultado);
    }
    [HttpGet("mayor-puntaje")]
    public IActionResult MayorPuntaje()
    {
        var resultado = participants.Select(p => new
            {
                Participante = p,
                PuntajeTotal = ResolutionController.resolutions.Where(r => r.ParticipanteId == p.Id).Sum(r => r.PuntajeObtenido)
            }).OrderByDescending(x => x.PuntajeTotal).FirstOrDefault();

        if (resultado == null)
            return NotFound();

        return Ok(resultado);
    }
    [HttpGet("ordenados-por-puntaje")]
    public IActionResult OrdenarPorPuntaje()
    {
        var resultado = participants.Select(p => new
            {
                Participante = p,
                PuntajeTotal = ResolutionController.resolutions.Where(r => r.ParticipanteId == p.Id).Sum(r => r.PuntajeObtenido)
            }).OrderByDescending(x => x.PuntajeTotal).ToList();

        return Ok(resultado);
    }
}
