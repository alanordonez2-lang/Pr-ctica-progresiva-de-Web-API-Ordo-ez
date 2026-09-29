using Microsoft.AspNetCore.Mvc;

namespace Ejercicio2.Controllers;

[ApiController]
[Route("[controller]")]
public class StationController : ControllerBase
{
    private static readonly List<Station> stations = new()
    {
        new Station
        {
            Id=1,
            Nombre= "Estacion Central",
            Localidad= "Cba",
            Activa= true
        }
    };

    private readonly ILogger<StationController> _logger;

    public StationController(ILogger<StationController> logger)
    {
        _logger = logger;
    }
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(stations);
    }
    [HttpGet("{localidad}")]
    public IActionResult BuscarPorLocalidad(string localidad)
    {
        var resultado= stations.Where(x => x.Localidad.ToLower() == localidad.ToLower()).ToList();
        return Ok(resultado);
    }
    [HttpPost]
    public IActionResult CreatedStation([FromBody] Station newStation)
    {
        if (stations.Any(x => x.Id == newStation.Id))
        {
            return BadRequest("Ya existe una estación con ese ID.");
        }
        stations.Add(newStation);

        return Ok("Estación creada correctamente.");
    }
}
