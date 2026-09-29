using Microsoft.AspNetCore.Mvc;

namespace Ejercicio7.Controllers;

[ApiController]
[Route("[controller]")]
public class ParticipacionController : ControllerBase
{
    public static readonly List<Participacion> participaciones = new()
    {
        new Participacion
        {
            Id = 1,
            ExpedicionId = 1,
            ExploradorId = 1,
            Rol = "Lider"
        }
    };

    private readonly ILogger<ParticipacionController> _logger;

    public ParticipacionController(ILogger<ParticipacionController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(participaciones);
    }

    [HttpPost]
    public IActionResult AsignarExplorador(Participacion nuevaParticipacion)
    {
        var expedicion = ExpedicionController.expediciones
            .FirstOrDefault(x => x.Id == nuevaParticipacion.ExpedicionId);

        if (expedicion == null)
            return NotFound("La expedición no existe.");

        var explorador = ExploradorController.exploradores
            .FirstOrDefault(x => x.Id == nuevaParticipacion.ExploradorId);

        if (explorador == null)
            return NotFound("El explorador no existe.");

        if (explorador.Disponible == false)
            return BadRequest("El explorador no está disponible.");

        int cantidad = participaciones
            .Count(x => x.ExpedicionId == nuevaParticipacion.ExpedicionId);

        if (cantidad >= expedicion.Capacidad)
            return BadRequest("La expedición superó su capacidad.");

        var existe = participaciones
            .Any(x => x.ExpedicionId == nuevaParticipacion.ExpedicionId &&
                      x.ExploradorId == nuevaParticipacion.ExploradorId);

        if (existe)
            return BadRequest("El explorador ya está asignado a esta expedición.");

        var expedicionesExplorador = participaciones
            .Where(x => x.ExploradorId == nuevaParticipacion.ExploradorId)
            .ToList();

        foreach (var participacion in expedicionesExplorador)
        {
            var otraExpedicion = ExpedicionController.expediciones
                .FirstOrDefault(x => x.Id == participacion.ExpedicionId);

            if (otraExpedicion != null)
            {
                if (expedicion.FechaInicio <= otraExpedicion.FechaFin &&
                    expedicion.FechaFin >= otraExpedicion.FechaInicio)
                {
                    return BadRequest("El explorador ya participa en otra expedición con fechas superpuestas.");
                }
            }
        }

        nuevaParticipacion.Id = participaciones.Count + 1;

        participaciones.Add(nuevaParticipacion);

        return Ok(nuevaParticipacion);
    }

    [HttpGet("expedicion/{expedicionId}")]
    public IActionResult IntegrantesDeExpedicion(int expedicionId)
    {
        var expedicion = ExpedicionController.expediciones
            .FirstOrDefault(x => x.Id == expedicionId);

        if (expedicion == null)
            return NotFound("La expedición no existe.");

        var resultado = participaciones
            .Where(x => x.ExpedicionId == expedicionId)
            .Select(x => ExploradorController.exploradores
                .FirstOrDefault(e => e.Id == x.ExploradorId))
            .ToList();

        return Ok(resultado);
    }
}