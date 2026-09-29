using Microsoft.AspNetCore.Mvc;

namespace Ejercicio7.Controllers;

[ApiController]
[Route("[controller]")]
public class ExploradorController : ControllerBase
{
    public static readonly List<Explorador> exploradores = new()
    {
        new Explorador
        {
            Id = 1,
            Nombre = "Carlos Perez",
            Especialidad = "Montañismo",
            ExperienciaAnios = 5,
            Disponible = true
        },
        new Explorador
        {
            Id = 2,
            Nombre = "Juan Gomez",
            Especialidad = "Geología",
            ExperienciaAnios = 3,
            Disponible = true
        },
        new Explorador
        {
            Id = 3,
            Nombre = "Pedro Lopez",
            Especialidad = "Montañismo",
            ExperienciaAnios = 8,
            Disponible = false
        }
    };

    private readonly ILogger<ExploradorController> _logger;

    public ExploradorController(ILogger<ExploradorController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(exploradores);
    }

    [HttpPost]
    public IActionResult CrearExplorador(Explorador nuevoExplorador)
    {
        nuevoExplorador.Id = exploradores.Count + 1;

        exploradores.Add(nuevoExplorador);

        return Ok(nuevoExplorador);
    }

    [HttpGet("especialidad/{especialidad}")]
    public IActionResult BuscarPorEspecialidad(string especialidad)
    {
        var resultado = exploradores
            .Where(x => x.Especialidad == especialidad)
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("disponibles")]
    public IActionResult ExploradoresDisponibles()
    {
        var resultado = exploradores
            .Where(x => x.Disponible == true)
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("{exploradorId}/expediciones")]
    public IActionResult ExpedicionesPorExplorador(int exploradorId)
    {
        var explorador = exploradores
            .FirstOrDefault(x => x.Id == exploradorId);

        if (explorador == null)
            return NotFound("El explorador no existe.");

        var resultado = ParticipacionController.participaciones
            .Where(x => x.ExploradorId == exploradorId)
            .Select(x => ExpedicionController.expediciones
                .FirstOrDefault(e => e.Id == x.ExpedicionId))
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("mas-expediciones")]
    public IActionResult MasExpediciones()
    {
        var resultado = exploradores
            .Select(x => new
            {
                Explorador = x,
                CantidadExpediciones = ParticipacionController.participaciones
                    .Count(p => p.ExploradorId == x.Id)
            })
            .OrderByDescending(x => x.CantidadExpediciones)
            .FirstOrDefault();

        if (resultado == null)
            return NotFound();

        return Ok(resultado);
    }

    [HttpGet("especialidades-mas-utilizadas")]
    public IActionResult EspecialidadesMasUtilizadas()
    {
        var resultado = exploradores
            .GroupBy(x => x.Especialidad)
            .Select(x => new
            {
                Especialidad = x.Key,
                Cantidad = x.Count()
            })
            .OrderByDescending(x => x.Cantidad)
            .ToList();

        return Ok(resultado);
    }
}