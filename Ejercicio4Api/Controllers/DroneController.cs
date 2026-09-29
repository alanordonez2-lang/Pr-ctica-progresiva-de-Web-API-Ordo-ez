using Microsoft.AspNetCore.Mvc;

namespace Ejercicio4.Controllers;

[ApiController]
[Route("[controller]")]
public class DroneController : ControllerBase
{
    public static readonly List<Drone> drones = new()
    {
        new Drone
        {
            Id = 1,
            Codigo = "DR-001",
            Modelo = "DJI Mini",
            Bateria = 90,
            Estado = "Disponible"
        }
        
    };

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(drones);
    }

    // Registrar drone
    [HttpPost]
    public IActionResult CrearDrone(Drone nuevoDrone)
    {
        nuevoDrone.Id = drones.Count + 1;

        drones.Add(nuevoDrone);

        return Ok(nuevoDrone);
    }

    [HttpGet("disponibles")]
    public IActionResult DronesDisponibles()
    {
        var resultado = drones
            .Where(x => x.Estado == "Disponible")
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("bateria-menor/{porcentaje}")]
    public IActionResult BateriaMenor(double porcentaje)
    {
        var resultado = drones
            .Where(x => x.Bateria < porcentaje)
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("modelo/{modelo}")]
    public IActionResult BuscarPorModelo(string modelo)
    {
        var resultado = drones
            .Where(x => x.Modelo.ToLower() == modelo.ToLower())
            .ToList();

        return Ok(resultado);
    }
}