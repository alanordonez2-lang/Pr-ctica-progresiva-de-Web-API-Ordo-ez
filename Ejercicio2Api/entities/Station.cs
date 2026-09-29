public class Station
{
    private int id;
    private string nombre;
    private string localidad;
    private bool activa;

    public int Id{get{return this.id;}set{this.id = value;}}
    public string Nombre{get{return this.nombre;}set{this.nombre = value;}}
    public string Localidad{get{return this.localidad;}set{this.localidad = value;}}
    public bool Activa{get{return this.activa;}set{this.activa = value;}}

}