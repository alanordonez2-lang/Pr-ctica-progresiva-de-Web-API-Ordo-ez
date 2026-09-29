public class Resolution
{
    private int id;
    private int participanteId;
    private int desafioId;
    private int puntajeObtenido;
    private DateTime fechaEntrega;

    public int Id{get{return this.id;}set{this.id = value;}}
    public int ParticipanteId{get{return this.participanteId;}set{this.participanteId = value;}}
    public int DesafioId{get{return this.desafioId;}set{this.desafioId = value;}}
    public int PuntajeObtenido{get{return this.puntajeObtenido;}set{this.puntajeObtenido = value;}}
    public DateTime FechaEntrega{get{return this.fechaEntrega;}set{this.fechaEntrega = value;}}

}