using Microsoft.AspNetCore.Mvc;

namespace Ejercicio7.Controllers;

[ApiController]
[Route("[controller]")]
public class ExpedicionController : ControllerBase
{
    public static readonly List<Expedicion> expediciones = new()
    {
        new Expedicion
        {
            Id = 1,
            Nombre = "Expedición Andes",
            Destino = "Mendoza",
            FechaInicio = new DateTime(2026, 10, 10),
            FechaFin = new DateTime(2026, 10, 20),
            Capacidad = 3,
            Estado = "Activa"
        },
        new Expedicion
        {
            Id = 2,
            Nombre = "Expedición Patagonia",
            Destino = "Bariloche",
            FechaInicio = new DateTime(2026, 12, 1),
            FechaFin = new DateTime(2026, 12, 15),
            Capacidad = 2,
            Estado = "Futura"
        }
    };

    private readonly ILogger<ExpedicionController> _logger;

    public ExpedicionController(ILogger<ExpedicionController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(expediciones);
    }

    [HttpPost]
    public IActionResult CrearExpedicion(Expedicion nuevaExpedicion)
    {
        if (nuevaExpedicion.FechaFin < nuevaExpedicion.FechaInicio)
            return BadRequest("La FechaFin no puede ser anterior a la FechaInicio.");

        nuevaExpedicion.Id = expediciones.Count + 1;

        expediciones.Add(nuevaExpedicion);

        return Ok(nuevaExpedicion);
    }

    [HttpGet("{id}")]
    public IActionResult GetById(int id)
    {
        var expedicion = expediciones
            .FirstOrDefault(x => x.Id == id);

        if (expedicion == null)
            return NotFound("La expedición no existe.");

        return Ok(expedicion);
    }

    [HttpGet("activas")]
    public IActionResult ExpedicionesActivas()
    {
        var resultado = expediciones
            .Where(x => x.Estado == "Activa")
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("futuras")]
    public IActionResult ExpedicionesFuturas()
    {
        var resultado = expediciones
            .Where(x => x.Estado == "Futura")
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("completas")]
    public IActionResult ExpedicionesCompletas()
    {
        var resultado = expediciones
            .Where(x => ParticipacionController.participaciones
                .Count(p => p.ExpedicionId == x.Id) >= x.Capacidad)
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("ordenadas")]
    public IActionResult OrdenarPorFecha()
    {
        var resultado = expediciones
            .OrderBy(x => x.FechaInicio)
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("{expedicionId}/participantes/cantidad")]
    public IActionResult CantidadParticipantes(int expedicionId)
    {
        var expedicion = expediciones
            .FirstOrDefault(x => x.Id == expedicionId);

        if (expedicion == null)
            return NotFound("La expedición no existe.");

        int cantidad = ParticipacionController.participaciones
            .Count(x => x.ExpedicionId == expedicionId);

        return Ok(cantidad);
    }

    [HttpGet("{expedicionId}/ocupacion")]
    public IActionResult PorcentajeOcupacion(int expedicionId)
    {
        var expedicion = expediciones
            .FirstOrDefault(x => x.Id == expedicionId);

        if (expedicion == null)
            return NotFound("La expedición no existe.");

        int cantidad = ParticipacionController.participaciones
            .Count(x => x.ExpedicionId == expedicionId);

        double porcentaje = (double)cantidad / expedicion.Capacidad * 100;

        return Ok(porcentaje);
    }
}