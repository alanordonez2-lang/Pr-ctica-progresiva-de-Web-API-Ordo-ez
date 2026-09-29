public class Expedicion
{
    private int id;
    private string nombre = string.Empty;
    private string destino = string.Empty;
    private DateTime fechaInicio;
    private DateTime fechaFin;
    private int capacidad;
    private string estado = "Futura";
    public int Id
    {
        get { return this.id; }
        set { this.id = value; }
    }
    public string Nombre
    {
        get { return this.nombre; }
        set { this.nombre = value; }
    }
    public string Destino
    {
        get { return this.destino; }
        set { this.destino = value; }
    }
    public DateTime FechaInicio
    {
        get { return this.fechaInicio; }
        set { this.fechaInicio = value; }
    }
    public DateTime FechaFin
    {
        get { return this.fechaFin; }
        set { this.fechaFin = value; }
    }
    public int Capacidad
    {
        get { return this.capacidad; }
        set { this.capacidad = value; }
    }
    public string Estado
    {
        get { return this.estado; }
        set { this.estado = value; }
    }
}