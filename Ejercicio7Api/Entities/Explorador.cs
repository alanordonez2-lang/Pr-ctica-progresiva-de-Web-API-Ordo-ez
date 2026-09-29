public class Explorador
{
    private int id;
    private string nombre = string.Empty;
    private string especialidad = string.Empty;
    private int experienciaAnios;
    private bool disponible = true;
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
    public string Especialidad
    {
        get { return this.especialidad; }
        set { this.especialidad = value; }
    }
    public int ExperienciaAnios
    {
        get { return this.experienciaAnios; }
        set { this.experienciaAnios = value; }
    }
    public bool Disponible
    {
        get { return this.disponible; }
        set { this.disponible = value; }
    }
}
