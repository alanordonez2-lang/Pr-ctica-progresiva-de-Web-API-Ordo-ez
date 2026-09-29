using Microsoft.AspNetCore.Mvc;

namespace Ejercicio5.Controllers;

[ApiController]
[Route("[controller]")]
public class ActividadController : ControllerBase
{
    public static readonly List<Actividad> actividades = new()
    {
        new Actividad
        {
            Id = 1,
            Nombre = "Introducción a la IA",
            Tipo = "Charla",
            Horario = new DateTime(2026, 10, 10, 10, 0, 0),
            DuracionMinutos = 60,
            Capacidad = 50,
            Activa = true
        }
    };
    private readonly ILogger<ActividadController> _logger;

    public ActividadController(ILogger<ActividadController> logger)
    {
        _logger = logger;
    }
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(actividades);
    }

    // Crear actividad
    [HttpPost]
    public IActionResult CrearActividad(Actividad nuevaActividad)
    {
        nuevaActividad.Id = actividades.Count + 1;

        actividades.Add(nuevaActividad);

        return Ok(nuevaActividad);
    }

    [HttpGet("disponibles")]
    public IActionResult ActividadesDisponibles()
    {
        var resultado = actividades
            .Where(x => x.Activa &&
                ReservaActividadController.reservas
                    .Count(r => r.ActividadId == x.Id) < x.Capacidad)
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("nombre/{nombre}")]
    public IActionResult BuscarPorNombre(string nombre)
    {
        var resultado = actividades
            .Where(x => x.Nombre.ToLower().Contains(nombre.ToLower()))
            .ToList();

        return Ok(resultado);
    }

    // Filtrar actividades por tipo
    [HttpGet("tipo/{tipo}")]
    public IActionResult BuscarPorTipo(string tipo)
    {
        var resultado = actividades
            .Where(x => x.Tipo.ToLower() == tipo.ToLower())
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("lugares-disponibles")]
    public IActionResult LugaresDisponibles()
    {
        var resultado = actividades
            .Where(x =>
                ReservaActividadController.reservas
                    .Count(r => r.ActividadId == x.Id) < x.Capacidad)
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("completas")]
    public IActionResult ActividadesCompletas()
    {
        var resultado = actividades
            .Where(x =>
                ReservaActividadController.reservas
                    .Count(r => r.ActividadId == x.Id) >= x.Capacidad)
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("ordenadas-inscriptos")]
    public IActionResult OrdenarPorInscriptos()
    {
        var resultado = actividades
            .Select(x => new
            {
                Actividad = x,
                Inscriptos = ReservaActividadController.reservas
                    .Count(r => r.ActividadId == x.Id)
            })
            .OrderByDescending(x => x.Inscriptos)
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("mayor-cantidad-participantes")]
    public IActionResult MayorCantidadParticipantes()
    {
        var resultado = actividades
            .Select(x => new
            {
                Actividad = x,
                Inscriptos = ReservaActividadController.reservas
                    .Count(r => r.ActividadId == x.Id)
            })
            .OrderByDescending(x => x.Inscriptos)
            .FirstOrDefault();

        if (resultado == null)
            return NotFound();

        return Ok(resultado);
    }

    [HttpGet("porcentaje-ocupacion")]
    public IActionResult PorcentajeOcupacion()
    {
        var resultado = actividades
            .Select(x => new
            {
                Actividad = x,
                Inscriptos = ReservaActividadController.reservas
                    .Count(r => r.ActividadId == x.Id),
                PorcentajeOcupacion =
                    (double)ReservaActividadController.reservas
                    .Count(r => r.ActividadId == x.Id)
                    / x.Capacidad * 100
            })
            .ToList();

        return Ok(resultado);
    }
}