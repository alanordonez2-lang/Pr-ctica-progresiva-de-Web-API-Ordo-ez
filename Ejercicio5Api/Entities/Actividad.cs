public class Actividad
{
    private int id;
    private string nombre;
    private string tipo;
    private DateTime horario;
    private int duracionMinutos;
    private int capacidad;
    private bool activa;

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

    public string Tipo
    {
        get { return this.tipo; }
        set { this.tipo = value; }
    }

    public DateTime Horario
    {
        get { return this.horario; }
        set { this.horario = value; }
    }

    public int DuracionMinutos
    {
        get { return this.duracionMinutos; }
        set { this.duracionMinutos = value; }
    }

    public int Capacidad
    {
        get { return this.capacidad; }
        set { this.capacidad = value; }
    }

    public bool Activa
    {
        get { return this.activa; }
        set { this.activa = value; }
    }
}