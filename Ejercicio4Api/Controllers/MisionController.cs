using Microsoft.AspNetCore.Mvc;

namespace Ejercicio4.Controllers;

[ApiController]
[Route("[controller]")]
public class MisionController : ControllerBase
{
    public static readonly List<Mision> misiones = new()
    {
        new Mision
        {
            Id = 1,
            DroneId = 1,
            Descripcion = "Control de zona",
            DistanciaKm = 10,
            Fecha = new DateTime(2026, 9, 20),
            Completada = true
        },

        new Mision
        {
            Id = 2,
            DroneId = 1,
            Descripcion = "Inspección",
            DistanciaKm = 15,
            Fecha = new DateTime(2026, 9, 25),
            Completada = false
        }
    };

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(misiones);
    }

    [HttpPost]
    public IActionResult AsignarMision(Mision nuevaMision)
    {
        var drone = DroneController.drones
            .FirstOrDefault(x => x.Id == nuevaMision.DroneId);

        if (drone == null)
            return NotFound("El drone no existe.");

        if (drone.Estado != "Disponible")
            return BadRequest("El drone no está disponible.");

        if (drone.Bateria < 30)
            return BadRequest("El drone debe tener como mínimo 30% de batería.");

        bool tieneMisionActiva = misiones.Any(x =>
            x.DroneId == drone.Id &&
            x.Completada == false);

        if (tieneMisionActiva)
            return BadRequest("El drone ya tiene otra misión activa.");

        nuevaMision.Id = misiones.Count + 1;
        nuevaMision.Completada = false;

        misiones.Add(nuevaMision);

        drone.Estado = "EnMision";

        return Ok(nuevaMision);
    }

    [HttpPut("finalizar/{id}")]
    public IActionResult FinalizarMision(int id)
    {
        var mision = misiones
            .FirstOrDefault(x => x.Id == id);

        if (mision == null)
            return NotFound("La misión no existe.");

        if (mision.Completada)
            return BadRequest("La misión ya está finalizada.");

        mision.Completada = true;

        var drone = DroneController.drones
            .FirstOrDefault(x => x.Id == mision.DroneId);

        if (drone != null)
        {
            drone.Estado = "Disponible";
        }

        return Ok(mision);
    }

    [HttpGet("drone/{droneId}")]
    public IActionResult MisionesPorDrone(int droneId)
    {
        var drone = DroneController.drones
            .FirstOrDefault(x => x.Id == droneId);

        if (drone == null)
            return NotFound("El drone no existe.");

        var resultado = misiones
            .Where(x => x.DroneId == droneId)
            .ToList();

        return Ok(resultado);
    }

    [HttpGet("distancia-total/{droneId}")]
    public IActionResult DistanciaTotal(int droneId)
    {
        var drone = DroneController.drones
            .FirstOrDefault(x => x.Id == droneId);

        if (drone == null)
            return NotFound("El drone no existe.");

        double distancia = misiones
            .Where(x => x.DroneId == droneId && x.Completada)
            .Sum(x => x.DistanciaKm);

        return Ok(distancia);
    }

    [HttpGet("mayor-distancia")]
    public IActionResult MayorDistancia()
    {
        var resultado = DroneController.drones
            .Select(d => new
            {
                Drone = d,

                DistanciaTotal = misiones
                    .Where(m => m.DroneId == d.Id && m.Completada)
                    .Sum(m => m.DistanciaKm)
            })
            .OrderByDescending(x => x.DistanciaTotal)
            .FirstOrDefault();

        if (resultado == null)
            return NotFound();

        return Ok(resultado);
    }

    [HttpGet("promedio-bateria")]
    public IActionResult PromedioBateria()
    {
        var dronesDisponibles = DroneController.drones
            .Where(x => x.Estado == "Disponible")
            .ToList();

        if (dronesDisponibles.Count == 0)
            return NotFound("No hay drones disponibles.");

        double promedio = dronesDisponibles
            .Average(x => x.Bateria);

        return Ok(promedio);
    }

    [HttpGet("pendientes")]
    public IActionResult MisionesPendientes()
    {
        var resultado = misiones
            .Where(x => x.Completada == false)
            .OrderBy(x => x.Fecha)
            .ToList();

        return Ok(resultado);
    }
}