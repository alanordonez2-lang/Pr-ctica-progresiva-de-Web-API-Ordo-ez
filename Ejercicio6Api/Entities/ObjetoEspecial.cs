public class ObjetoEspacial
{
    private int id;
    private string nombre;
    private string tipo;
    private double distancia;
    private int nivelRiesgo;
    private DateTime fechaDescubrimiento;
    private bool activo;

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

    public double Distancia
    {
        get { return this.distancia; }
        set { this.distancia = value; }
    }

    public int NivelRiesgo
    {
        get { return this.nivelRiesgo; }
        set { this.nivelRiesgo = value; }
    }

    public DateTime FechaDescubrimiento
    {
        get { return this.fechaDescubrimiento; }
        set { this.fechaDescubrimiento = value; }
    }

    public bool Activo
    {
        get { return this.activo; }
        set { this.activo = value; }
    }
}