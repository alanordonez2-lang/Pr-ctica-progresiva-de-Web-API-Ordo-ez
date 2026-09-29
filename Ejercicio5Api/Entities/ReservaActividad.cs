public class ReservaActividad
{
    private int id;
    private int actividadId;
    private int participanteId;
    private DateTime fechaReserva;

    public int Id
    {
        get { return this.id; }
        set { this.id = value; }
    }

    public int ActividadId
    {
        get { return this.actividadId; }
        set { this.actividadId = value; }
    }

    public int ParticipanteId
    {
        get { return this.participanteId; }
        set { this.participanteId = value; }
    }

    public DateTime FechaReserva
    {
        get { return this.fechaReserva; }
        set { this.fechaReserva = value; }
    }
}