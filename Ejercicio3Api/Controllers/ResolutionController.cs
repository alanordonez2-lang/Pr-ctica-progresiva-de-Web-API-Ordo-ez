using Microsoft.AspNetCore.Mvc;

namespace Ejercicio3.Controllers;

[ApiController]
[Route("[controller]")]
public class ResolutionController : ControllerBase
{
    public static readonly List<Resolution> resolutions = new()
    {
        new Resolution
        {
            Id = 1,
            ParticipanteId = 1,
            DesafioId = 1,
            PuntajeObtenido = 90,
            FechaEntrega = new DateTime(2026, 9, 10)
        }
    };
    private static readonly List<Participant>participants = new()
    {
        new Participant
        {
            Id=1,
            Nombre = "Alan",
            Email = "alano@gmial.com",
            Nivel = "pro"
        }
    };
    private static readonly List<Challenge>challenges = new()
    {
        new Challenge
        {
            Id=1,
            Titulo = "Dominadas",
            Dificultad = "Pro",
            PuntajeMaximo = 295.50,
            Activo = true
        }
    };
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(resolutions);
    }

    [HttpPost]
    public IActionResult CrearResolucion(Resolution newResolution)
    {
        var participante = participants.FirstOrDefault(x => x.Id == newResolution.ParticipanteId);

        if (participante == null)
            return NotFound("El participante no existe.");

        var desafio = challenges.FirstOrDefault(x => x.Id == newResolution.DesafioId);

        if (desafio == null)
            return NotFound("El desafío no existe.");

        if (desafio.Activo == false)
            return BadRequest("El desafío no está activo.");

        bool yaResolvio = resolutions.Any(x =>x.ParticipanteId == newResolution.ParticipanteId && x.DesafioId == newResolution.DesafioId);

        if (yaResolvio)
            return BadRequest("El participante ya resolvió este desafío.");

        if (newResolution.PuntajeObtenido > desafio.PuntajeMaximo)
            return BadRequest("El puntaje supera el máximo permitido.");

        newResolution.Id = resolutions.Count + 1;

        resolutions.Add(newResolution);

        return Ok(newResolution);
    }

    [HttpGet("participante/{participanteId}")]
    public IActionResult ResolucionesParticipante(int participanteId)
    {
        var participante = participants.FirstOrDefault(x => x.Id == participanteId);

        if (participante == null)
            return NotFound("El participante no existe.");

        var resultado = resolutions.Where(x => x.ParticipanteId == participanteId).ToList();

        return Ok(resultado);
    }

    [HttpGet("promedio/{desafioId}")]
    public IActionResult PromedioDesafio(int desafioId)
    {
        var desafio = challenges.FirstOrDefault(x => x.Id == desafioId);

        if (desafio == null)
            return NotFound("El desafío no existe.");

        var resultados = resolutions.Where(x => x.DesafioId == desafioId).ToList();

        if (resultados.Count == 0)
            return NotFound("El desafío todavía no tiene resoluciones.");

        double promedio = resultados.Average(x => x.PuntajeObtenido);

        return Ok(promedio);
    }
}