using Microsoft.AspNetCore.Mvc;

using Microsoft.AspNetCore.Mvc;

namespace Ejercicio6.Controllers;

[ApiController]
[Route("[controller]")]
public class ObjetoEspacialController : ControllerBase
{
    public static readonly List<ObjetoEspacial> objetos = new()
    {
        new ObjetoEspacial
        {
            Id = 1,
            Nombre = "Apophis",
            Tipo = "Asteroide",
            Distancia = 150000,
            NivelRiesgo = 9,
            FechaDescubrimiento = new DateTime(2026, 1, 10),
            Activo = true
        }
    };


    private readonly ILogger<ObjetoEspacialController> _logger;

    public ObjetoEspacialController(ILogger<ObjetoEspacialController> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(objetos);
    }

    [HttpPost]
    public IActionResult CrearObjeto(ObjetoEspacial nuevoObjeto)
    {
        nuevoObjeto.Id = objetos.Count + 1;

        objetos.Add(nuevoObjeto);

        return Ok(nuevoObjeto);
    }

    [HttpGet("nombre/{nombre}")]
    public IActionResult BuscarPorNombre(string nombre)
    {
        var resultado = objetos
            .Where(x => x.Nombre.ToLower().Contains(nombre.ToLower()))
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("tipo/{tipo}")]
    public IActionResult BuscarPorTipo(string tipo)
    {
        var resultado = objetos
            .Where(x => x.Tipo.ToLower() == tipo.ToLower())
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("riesgo/{nivel}")]
    public IActionResult BuscarPorRiesgo(int nivel)
    {
        var resultado = objetos
            .Where(x => x.NivelRiesgo >= nivel)
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("distancia-menor/{distancia}")]
    public IActionResult DistanciaMenor(double distancia)
    {
        var resultado = objetos
            .Where(x => x.Distancia < distancia)
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("sin-observaciones")]
    public IActionResult SinObservaciones()
    {
        var resultado = objetos
            .Where(x => !ObservacionController.observaciones
                .Any(o => o.ObjetoEspacialId == x.Id))
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("ordenados-riesgo")]
    public IActionResult OrdenadosPorRiesgo()
    {
        var resultado = objetos
            .OrderByDescending(x => x.NivelRiesgo)
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("alertas")]
    public IActionResult Alertas()
    {
        var resultado = objetos
            .Where(x =>
                x.NivelRiesgo >= 7 &&
                x.Distancia < 200000 &&
                x.Activo)
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("estadisticas")]
    public IActionResult Estadisticas()
    {
        int totalObjetos = objetos.Count;

        int objetosActivos = objetos
            .Count(x => x.Activo);

        int totalObservaciones = ObservacionController.observaciones.Count;

        int objetosAltoRiesgo = objetos
            .Count(x => x.NivelRiesgo >= 7);

        return Ok(new
        {
            TotalObjetos = totalObjetos,
            ObjetosActivos = objetosActivos,
            TotalObservaciones = totalObservaciones,
            ObjetosAltoRiesgo = objetosAltoRiesgo
        });
    }
}
