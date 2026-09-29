using Microsoft.AspNetCore.Mvc;

namespace Ejercicio2.Controllers;

[ApiController]
[Route("[controller]")]
public class MeasurementController : ControllerBase
{
    private static readonly List<Measurement> measurements = new()
    {
        new Measurement
        {
            Id = 1,
            EstacionId = 1,
            Temperatura = 25.5,
            Humedad = 60,
            VelocidadViento = 10,
            FechaHora = new DateTime(2026, 9, 28, 10, 30, 0)
        }
    };
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


    private readonly ILogger<MeasurementController> _logger;

    public MeasurementController(ILogger<MeasurementController> logger)
    {
        _logger = logger;
    }
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(measurements);
    }
    [HttpGet("{idEstacion}")]
    public IActionResult PorEstacion(int idEstacion)
    {
        var estacion= stations.FirstOrDefault(x=> x.Id == idEstacion);
        if(estacion is null)
        {
            return NotFound("La estacion no existe");
        }

        var resultado = measurements.Where(x => x.EstacionId == idEstacion).ToList();

        return Ok(resultado);
    }
    [HttpGet("temperatura-mayor-a/")]
    public IActionResult TemperaturasMayores(double valor)
    {
        var resultado= measurements.Where(x=> x.Temperatura > valor).ToList();

        return Ok(resultado);
    }
    [HttpGet("ordenados")]
    public IActionResult OrdenarPorTemp()
    {
        var resultado = measurements.OrderByDescending(x=> x.Temperatura).ToList();

        return Ok(resultado);
    }
    [HttpGet("promedio")]
    public IActionResult TempPromedio()
    {
        if(measurements.Count == 0)
        {
            return NotFound("No hay mediciones registradas");
        }
        double promedio = measurements.Average(x => x.Temperatura);

        return Ok(promedio);
    }
    [HttpGet("maxima")]
    public IActionResult TempMax()
    {
        if(measurements.Count == 0)
        {
            return NotFound("No hay mediciones registradas");
        }
        double maxima = measurements.Max(x=> x.Temperatura);

        return Ok(maxima);
    }
    [HttpGet("minima")]
    public IActionResult TempMin()
    {
        if(measurements.Count == 0)
        {
            return NotFound("No hay mediciones registradas");
        }
        double minima = measurements.Min(x => x.Temperatura);

        return Ok(minima);
    }
    [HttpGet("cantEstacion")]
    public IActionResult CantPorEstacion(int idEstacion)
    {
        var estacion = stations.FirstOrDefault(x=> x.Id == idEstacion);

        if(estacion is null)
        {
            return NotFound("La estacion no existe.");
        }

        int cantidad = measurements.Count(x=>x.EstacionId == idEstacion);

        return Ok(cantidad);
    }
    [HttpGet("existe")]
    public IActionResult ExisteMedicion()
    {
        bool existe = measurements.Any();

        return Ok(existe);
    }
    [HttpPost]
    public IActionResult CreatedMeasurament([FromBody] Measurement NewMeasurement)
    {
        var estacion = stations.FirstOrDefault(x => x.Id == NewMeasurement.EstacionId);

        if (estacion is null)
        {
            return NotFound("La estación no existe.");
        }
        if (!estacion.Activa)
        {
            return BadRequest("La estación está inactiva y no puede recibir mediciones.");
        }
        if (NewMeasurement.Humedad < 0 || NewMeasurement.Humedad > 100)
        {
            return BadRequest("La humedad debe estar entre 0 y 100.");
        }

        if (NewMeasurement.VelocidadViento < 0)
        {
            return BadRequest("La velocidad del viento no puede ser negativa.");
        }

        if (NewMeasurement.FechaHora > DateTime.Now)
        {
            return BadRequest("La fecha de la medición no puede ser futura.");
        }

        NewMeasurement.Id = measurements.Count + 1;

        measurements.Add(NewMeasurement);

        return Ok("Medición registrada correctamente.");
    }
}