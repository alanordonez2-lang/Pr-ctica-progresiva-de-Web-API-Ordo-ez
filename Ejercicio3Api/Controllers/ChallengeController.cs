using Microsoft.AspNetCore.Mvc;

namespace Ejercicio3.Controllers;

[ApiController]
[Route("[controller]")]
public class ChallengeController : ControllerBase
{
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
    private readonly ILogger<ChallengeController> _logger;

    public ChallengeController(ILogger<ChallengeController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(challenges);
    }
    [HttpPost]
    public IActionResult CrearDesafio(Challenge newChallenge)
    {
        newChallenge.Id = challenges.Count + 1;

        challenges.Add(newChallenge);

        return Ok(newChallenge);
    }
    [HttpGet("activos")]
    public IActionResult DesafiosActivos()
    {
        var resultado = challenges.Where(x => x.Activo == true).ToList();

        return Ok(resultado);
    }
    [HttpGet("dificultad/{dificultad}")]
    public IActionResult BuscarPorDificultad(string dificultad)
    {
        var resultado = challenges.Where(x => x.Dificultad.ToLower() == dificultad.ToLower()).ToList();

        return Ok(resultado);
    }
     [HttpGet("no-resueltos/{participanteId}")]
    public IActionResult DesafiosNoResueltos(int participanteId)
    {
        var participante = ParticipantController.participants.FirstOrDefault(x => x.Id == participanteId);

        if (participante == null)
            return NotFound("El participante no existe.");

        var resultado = challenges.Where(d => !ResolutionController.resolutions.Any(r => r.ParticipanteId == participanteId && r.DesafioId == d.Id)).ToList();

        return Ok(resultado);
    }


}