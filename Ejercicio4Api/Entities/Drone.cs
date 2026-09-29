public class Drone
{
    private int id;
    private string codigo;
    private string modelo;
    private double bateria;
    private string estado;

    public int Id {get { return this.id; } set { this.id = value; }}

    public string Codigo{get { return this.codigo; }set { this.codigo = value; }}

    public string Modelo{get { return this.modelo; }set { this.modelo = value; }}

    public double Bateria{get { return this.bateria; }set { this.bateria = value; }}

    public string Estado{get { return this.estado; }set { this.estado = value; }}
}