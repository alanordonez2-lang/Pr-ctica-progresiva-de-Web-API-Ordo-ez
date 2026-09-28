using Microsoft.AspNetCore.Mvc;

namespace Ejercicio1.Controllers;

[ApiController]
[Route("[controller]")]
public class LostObjectControllers : ControllerBase
{
    private static readonly List<LostObject> lostObjects = new()
    {
        new LostObject
        {
            Id= 1,
            Descripcion= "buzo",
            Categoria= "ropa",
            LugarEncontrado= "almacen",
            FechaEncontrado= new DateTime(2026, 7, 20),
            Reclamado= true,
            NombrePersonaQueRetiro= "Carlos"
        }
    };

    private readonly ILogger<LostObjectControllers> _logger;

    public LostObjectControllers(ILogger<LostObjectControllers> logger)
    {
        _logger = logger;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(lostObjects);
    }
    [HttpPost]
    public IActionResult CreateLostObject([FromBody] LostObject NewLostObject)
    {
        if (NewLostObject.Descripcion is null || NewLostObject.Descripcion.Trim() == "")
        {
            return BadRequest("La descripcion es obligatoria.");
        }
        if (NewLostObject.FechaEncontrado > DateTime.Now)
        {
            return BadRequest("La fecha encontrada no puede ser futura.");
        }
        NewLostObject.Id = lostObjects.Count + 1;
        NewLostObject.Reclamado = false;
        NewLostObject.NombrePersonaQueRetiro = "";

        lostObjects.Add(NewLostObject);

        return Ok("Lost Object created successfully");
    }
    [HttpGet("ID")]
    public IActionResult GetID(int id)
    {
        var lostObject = lostObjects.FirstOrDefault(x => x.Id == id);
        if (lostObject == null)
        {
            return NotFound("No se encontró el objeto perdido.");
        }
        return Ok(lostObject);
    }
    [HttpPut("{id}")]
    public IActionResult Update(int id, LostObject perdidoActualizado)
    {
        try
        {
            var perdido = lostObjects.FirstOrDefault(x => x.Id == id);

            if (perdido == null)
            {
                return NotFound("No se encontró el objeto perdido.");
            }
            if (perdidoActualizado.Descripcion is null || perdidoActualizado.Descripcion.Trim() == "")
            {
                return BadRequest("La descripcion es obligatoria.");
            }
            if (perdidoActualizado.FechaEncontrado > DateTime.Now)
            {
                return BadRequest("La fecha encontrada no puede ser futura.");
            }

            perdido.Descripcion = perdidoActualizado.Descripcion;
            perdido.Categoria = perdidoActualizado.Categoria;
            perdido.LugarEncontrado = perdidoActualizado.LugarEncontrado;
            perdido.FechaEncontrado = perdidoActualizado.FechaEncontrado;
            perdido.Reclamado = perdidoActualizado.Reclamado;
            perdido.NombrePersonaQueRetiro = perdidoActualizado.NombrePersonaQueRetiro;

            return Ok(perdido);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpDelete("{ID}")]
    public IActionResult Delete(int ID)
    {
        try
        {
            var lostObject = lostObjects.FirstOrDefault(x => x.Id == ID);
            if (lostObject is null)
                return NotFound();

            lostObjects.Remove(lostObject);
            return Ok("Lost Object deleted");
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    [HttpGet("buscar/{descripcion}")]
    public IActionResult BuscarPorDescripcion(string descripcion)
    {
        var resultado = lostObjects.Where(x => x.Descripcion.ToLower().Contains(descripcion.ToLower())).ToList();

        return Ok(resultado);
    }
    [HttpGet("categoria/{categoria}")]
    public IActionResult FiltrarPorCategoria(string categoria)
    {
        var resultado = lostObjects.Where(x => x.Categoria.ToLower() == categoria.ToLower()).ToList();

        return Ok(resultado);
    }
    [HttpGet("no-reclamados")]
    public IActionResult ObtenerNoReclamados()
    {
        var resultado = lostObjects.Where(x => x.Reclamado == false).ToList();

        return Ok(resultado);
    }
    [HttpGet("despues-de/{fecha}")]
    public IActionResult ObtenerDespuesDe(DateTime fecha)
    {
        var resultado = lostObjects.Where(x => x.FechaEncontrado > fecha).ToList();

        return Ok(resultado);
    }
    [HttpGet("ordenados")]
    public IActionResult OrdenarPorFecha()
    {
        var resultado = lostObjects
            .OrderByDescending(x => x.FechaEncontrado)
            .ToList();

        return Ok(resultado);
    }
    [HttpPut("reclamar/{id}")]
    public IActionResult Reclamar(int id, string nombrePersona)
    {
        var perdido = lostObjects.FirstOrDefault(x => x.Id == id);

        if (perdido == null)
        {
            return NotFound("No se encontró el objeto perdido.");
        }

        if (perdido.Reclamado)
        {
            return BadRequest("El objeto ya fue retirado.");
        }

        if (perdido.NombrePersonaQueRetiro is null || perdido.NombrePersonaQueRetiro.Trim() == "")
            {
                return BadRequest("Debe indicar quién retiró el objeto.");
            }

        perdido.Reclamado = true;
        perdido.NombrePersonaQueRetiro = nombrePersona;

        return Ok(perdido);
    }
    [HttpGet("cantidad")]
    public IActionResult CantidadObjetos()
    {
        int cantidad = lostObjects.Count();

        return Ok(cantidad);
    }
    [HttpGet("existe-categoria/{categoria}")]
    public IActionResult ExisteCategoria(string categoria)
    {
        bool existe = lostObjects.Any(x => x.Categoria.ToLower() == categoria.ToLower());

        return Ok(existe);
    }


}
