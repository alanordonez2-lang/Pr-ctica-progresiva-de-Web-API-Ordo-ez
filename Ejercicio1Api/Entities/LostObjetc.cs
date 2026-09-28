using System.Data;

public class LostObject
{
    private int id;
    private string descripcion;
    private string categoria;
    private string lugarEncontrado;
    private DateTime fechaEncontrado;
    private bool reclamado;
    private string nombrePersonaQueRetiro;

    public int Id{get{return this.id;}set{this.id = value;}}
    public string Descripcion{get{return this.descripcion;}set{this.descripcion = value;}}
    public string Categoria{get{return this.categoria;}set{this.categoria = value;}}
    public string LugarEncontrado{get{return this.lugarEncontrado;}set{this.lugarEncontrado = value;}}
    public DateTime FechaEncontrado{get{return this.fechaEncontrado;}set{this.fechaEncontrado = value;}}
    public bool Reclamado{get{return this.reclamado;}set{this.reclamado = value;}}
    
    public string NombrePersonaQueRetiro{get{return this.nombrePersonaQueRetiro;}set{this.nombrePersonaQueRetiro = value;}}
}