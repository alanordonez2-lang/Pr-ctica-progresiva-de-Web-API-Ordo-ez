public class Mision
{
    private int id;
    private int droneId;
    private string descripcion;
    private double distanciaKm;
    private DateTime fecha;
    private bool completada;

    public int Id
    {get { return this.id; }set { this.id = value; }}

    public int DroneId{get { return this.droneId; }set { this.droneId = value; }}

    public string Descripcion{get { return this.descripcion; }set { this.descripcion = value; }}

    public double DistanciaKm{get { return this.distanciaKm; }set { this.distanciaKm = value; } }

    public DateTime Fecha{get { return this.fecha; }set { this.fecha = value; }}

    public bool Completada{get { return this.completada; }set { this.completada = value; }}
}