using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestLicorera.Models;

namespace TestLicorera;

[ApiController]
[Route("[controller]")]
public class ClienteController : ControllerBase
{
  private readonly LicoreraDbContext _dbLicorera;
  public ClienteController(LicoreraDbContext dbLicorera)
  {
    _dbLicorera = dbLicorera;
  }

  [HttpGet]
  [Route("listarClients")]
  public async Task<IActionResult> listarClientes()
  {
    List<Clientes> clientes = await _dbLicorera.clientes.ToListAsync();
    return Ok(new { Exito = true, clientes = clientes });
  }

  [HttpGet]
  [Route("buscarClienteId")]
  public async Task<IActionResult> buscarClienteId(int id)
  {
    try
    {
      var cliente = await _dbLicorera.clientes.FindAsync(id);
      return Ok(new { Exito = true, cliente = cliente });
    }
    catch (System.Exception e)
    {
      return Problem(detail: "false", title: "Error: " + e.Message, statusCode: 999);
    }

  }

  [HttpPost]
  [Route("crearCliente")]
  public IActionResult crearCliente(Clientes cliente)
  {
    try
    {
      _dbLicorera.Add(cliente);
      _dbLicorera.SaveChanges();
      return Ok(new { Exito = true, msj = "Registro insertado correctamente!" });
    }
    catch (System.Exception e)
    {
      return Problem(detail: "false", title: "Error: " + e.Message);
    }
  }

  [HttpPut]
  [Route("modificarCliente")]
  public IActionResult modificarCliente(Clientes cliente)
  {
    try
    {
      var clienteAcutal = _dbLicorera.clientes.Find(cliente.id);
      if (!(clienteAcutal.id == cliente.id))
      {
        throw new Exception("Registro No encontrado!");
      }

      Clientes nuevoCliente = (from c in _dbLicorera.clientes where c.id == cliente.id select c).First();
      nuevoCliente.nombre = cliente.nombre;
      nuevoCliente.apellido = cliente.apellido;
      nuevoCliente.fechaNacimiento = cliente.fechaNacimiento;

      _dbLicorera.Update(nuevoCliente);
      _dbLicorera.SaveChanges();

      return Ok(new { Exito = true, msj = "Registro modificado con éxito!" });
    }
    catch (System.Exception e)
    {
      return Problem(detail: "false", title: "Error: " + e.Message);
    }
  }

  [HttpDelete]
  [Route("eliminarCliente")]
  public IActionResult eliminarCliente(int id)
  {
    try
    {
      var clienteAcutal = _dbLicorera.clientes.Find(id);
      if (!(clienteAcutal.id == id))
      {
        throw new Exception("Registro No encontrado!");
      }

      Clientes clienteEliminar = (from c in _dbLicorera.clientes where c.id == id select c).First();
      clienteEliminar.estadoRegistro = false;

      _dbLicorera.Update(clienteEliminar);
      _dbLicorera.SaveChanges();

      return Ok(new { Exito = true, msj = "Registro eliminado con éxito!" });
    }
    catch (System.Exception e)
    {
      return Problem(detail: "false", title: "Error: " + e.Message);
    }
  }


  [HttpGet]
  [Route("buscarClienteCodigo")]
  public async Task<IActionResult> buscarCliente(string? codigo)
  {
    List<Clientes>? clientes = new List<Clientes>();
    try
    {

      clientes = await _dbLicorera.clientes.Where(c => c.codigo == codigo)
                                           .Where(c => c.estadoRegistro == true)
                                           .ToListAsync();
      if (!(clientes.Count > 0))
      {
        throw new("No se ha encontrado el Cliente!");
      }

      return Ok(new { Exito = true, clientes = clientes });
    }
    catch (System.Exception e)
    {
      return Problem(detail: "false", title: "Error: " + e.Message, statusCode: 999);
    }
  }

}