using Microsoft.AspNetCore.Mvc;

namespace Ejercicio6.Controllers;

[ApiController]
[Route("[controller]")]
public class ObservacionController : ControllerBase
{
    public static readonly List<Observacion> observaciones = new()
    {
        new Observacion
        {
            Id = 1,
            ObjetoEspacialId = 1,
            Fecha = new DateTime(2026, 9, 20),
            DistanciaMedida = 145000,
            Velocidad = 25.5,
            Comentario = "Objeto acercándose."
        }
    };
    private readonly ILogger<ObservacionController> _logger;

    public ObservacionController(ILogger<ObservacionController> logger)
    {
        _logger = logger;
    }
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(observaciones);
    }

    [HttpPost]
    public IActionResult CrearObservacion(Observacion nuevaObservacion)
    {
        var objeto = ObjetoEspacialController.objetos
            .FirstOrDefault(x => x.Id == nuevaObservacion.ObjetoEspacialId);

        if (objeto == null)
            return NotFound("El objeto espacial no existe.");

        nuevaObservacion.Id = observaciones.Count + 1;

        observaciones.Add(nuevaObservacion);

        return Ok(nuevaObservacion);
    }

    [HttpGet("objeto/{objetoId}")]
    public IActionResult ObservacionesPorObjeto(int objetoId)
    {
        var objeto = ObjetoEspacialController.objetos
            .FirstOrDefault(x => x.Id == objetoId);

        if (objeto == null)
            return NotFound("El objeto espacial no existe.");

        var resultado = observaciones
            .Where(x => x.ObjetoEspacialId == objetoId)
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("promedio-velocidad")]
    public IActionResult PromedioVelocidad()
    {
        if (observaciones.Count == 0)
            return NotFound("No existen observaciones.");

        double promedio = observaciones
            .Average(x => x.Velocidad);

        return Ok(promedio);
    }

    [HttpGet("objeto-mas-observado")]
    public IActionResult ObjetoMasObservado()
    {
        var resultado = ObjetoEspacialController.objetos
            .Select(x => new
            {
                Objeto = x,
                CantidadObservaciones = observaciones
                    .Count(o => o.ObjetoEspacialId == x.Id)
            })
            .OrderByDescending(x => x.CantidadObservaciones)
            .FirstOrDefault();

        if (resultado == null)
            return NotFound();

        return Ok(resultado);
    }
}