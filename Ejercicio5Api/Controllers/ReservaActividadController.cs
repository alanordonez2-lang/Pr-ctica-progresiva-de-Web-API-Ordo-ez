using Microsoft.AspNetCore.Mvc;

namespace Ejercicio5.Controllers;

[ApiController]
[Route("[controller]")]
public class ReservaActividadController : ControllerBase
{
    public static readonly List<ReservaActividad> reservas = new()
    {
        new ReservaActividad
        {
            Id = 1,
            ActividadId = 1,
            ParticipanteId = 1,
            FechaReserva = new DateTime(2026, 9, 28)
        }
    };
    private readonly ILogger<ReservaActividadController> _logger;

    public ReservaActividadController(ILogger<ReservaActividadController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    public IActionResult Inscribir(ReservaActividad nuevaReserva)
    {
        var participante = ParticipanteController.participantes
            .FirstOrDefault(x => x.Id == nuevaReserva.ParticipanteId);

        if (participante == null)
            return NotFound("El participante no existe.");

        var actividad = ActividadController.actividades
            .FirstOrDefault(x => x.Id == nuevaReserva.ActividadId);

        if (actividad == null)
            return NotFound("La actividad no existe.");

        if (!actividad.Activa)
            return BadRequest("La actividad está inactiva.");

        bool yaInscripto = reservas.Any(x =>
            x.ParticipanteId == nuevaReserva.ParticipanteId &&
            x.ActividadId == nuevaReserva.ActividadId);

        if (yaInscripto)
            return BadRequest("El participante ya está inscripto en esta actividad.");

        int cantidadInscriptos = reservas
            .Count(x => x.ActividadId == actividad.Id);

        if (cantidadInscriptos >= actividad.Capacidad)
            return BadRequest("La actividad está completa.");

        var actividadesParticipante = reservas
            .Where(x => x.ParticipanteId == participante.Id)
            .Select(x => ActividadController.actividades
                .FirstOrDefault(a => a.Id == x.ActividadId))
            .Where(x => x != null)
            .ToList();

        DateTime inicioNueva = actividad.Horario;

        DateTime finNueva = actividad.Horario
            .AddMinutes(actividad.DuracionMinutos);

        foreach (var actividadExistente in actividadesParticipante)
        {
            DateTime inicioExistente = actividadExistente.Horario;

            DateTime finExistente = actividadExistente.Horario
                .AddMinutes(actividadExistente.DuracionMinutos);

            bool seSuperpone =
                inicioNueva < finExistente &&
                finNueva > inicioExistente;

            if (seSuperpone)
                return BadRequest(
                    "El participante ya tiene otra actividad en ese horario.");
        }

        nuevaReserva.Id = reservas.Count + 1;
        nuevaReserva.FechaReserva = DateTime.Now;

        reservas.Add(nuevaReserva);

        return Ok(nuevaReserva);
    }

    [HttpDelete("{id}")]
    public IActionResult CancelarInscripcion(int id)
    {
        var reserva = reservas
            .FirstOrDefault(x => x.Id == id);

        if (reserva == null)
            return NotFound("La inscripción no existe.");

        reservas.Remove(reserva);

        return Ok("Inscripción cancelada.");
    }

    [HttpGet("actividad/{actividadId}/participantes")]
    public IActionResult ParticipantesActividad(int actividadId)
    {
        var actividad = ActividadController.actividades
            .FirstOrDefault(x => x.Id == actividadId);

        if (actividad == null)
            return NotFound("La actividad no existe.");

        var resultado = reservas
            .Where(x => x.ActividadId == actividadId)
            .Join(
                ParticipanteController.participantes,
                reserva => reserva.ParticipanteId,
                participante => participante.Id,
                (reserva, participante) => participante
            )
            .ToList();

        return Ok(resultado);
    }
}