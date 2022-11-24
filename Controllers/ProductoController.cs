using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestLicorera.Models;

namespace TestLicorera;

[ApiController]
[Route("[controller]")]
public class ProductoController : ControllerBase
{
  private readonly LicoreraDbContext _dbLicorera;
  public ProductoController(LicoreraDbContext dbLicorera)
  {
    _dbLicorera = dbLicorera;
  }

  [HttpGet]
  [Route("listarProductos")]
  public async Task<IActionResult> listarProductos()
  {
    List<Productos> productos = await _dbLicorera.productos.ToListAsync();
    return Ok(new { Exito = true, productos = productos });
  }

  [HttpGet]
  [Route("buscarProductoId")]
  public async Task<IActionResult> buscarProductoId(int id)
  {
    try
    {
      var producto = await _dbLicorera.productos.FindAsync(id);
      return Ok(new { Exito = true, producto = producto });
    }
    catch (System.Exception e)
    {
      return Problem(detail: "false", title: "Error: " + e.Message, statusCode: 999);
    }

  }

  [HttpGet]
  [Route("buscarProductoCodigo")]
  public async Task<IActionResult> buscarProductoCodigo(string codigo)
  {
    List<Productos>? productos = new List<Productos>();
    try
    {
      productos = await _dbLicorera.productos.Where(c => c.codigo == codigo)
                                             .Where(c => c.estadoRegistro == true)
                                             .ToListAsync();
      if (!(productos.Count > 0))
      {
        throw new("No se ha encontrado el Producto!");
      }

      return Ok(new { Exito = true, productos = productos });
    }
    catch (System.Exception e)
    {
      return Problem(detail: "false", title: "Error: " + e.Message, statusCode: 999);
    }
  }

  [HttpPost]
  [Route("crearProducto")]
  public IActionResult crearProducto(Productos producto)
  {
    try
    {
      producto.tasaCambio = obtenerTasaCambio(producto.fechaIngreso);
      producto.precioUnitarioDol = producto.precioUnitario / obtenerTasaCambio(producto.fechaIngreso);
      _dbLicorera.Add(producto);
      _dbLicorera.SaveChanges();
      return Ok(new { Exito = true, msj = "Registro insertado correctamente!" });
    }
    catch (System.Exception e)
    {
      return Problem(detail: "false", title: "Error: " + e.ToString());
    }
  }

  [HttpPut]
  [Route("modificarProducto")]
  public IActionResult modificarProducto(Productos producto)
  {
    try
    {
      var productoActual = _dbLicorera.productos.Find(producto.id);
      if (!(productoActual.id == producto.id))
      {
        throw new Exception("Registro No encontrado!");
      }

      Productos nuevoProducto = (from p in _dbLicorera.productos where p.id == producto.id select p).First();
      nuevoProducto.descripcion = producto.descripcion;
      nuevoProducto.fechaIngreso = producto.fechaIngreso;
      nuevoProducto.disponible = producto.disponible;
      nuevoProducto.precioUnitario = producto.precioUnitario;

      _dbLicorera.Update(nuevoProducto);
      _dbLicorera.SaveChanges();

      return Ok(new { Exito = true, msj = "Registro modificado con éxito!" });
    }
    catch (System.Exception e)
    {
      return Problem(detail: "false", title: "Error: " + e.Message);
    }
  }

  [HttpDelete]
  [Route("eliminarProducto")]
  public IActionResult eliminarProducto(int id)
  {
    try
    {
      var productoActual = _dbLicorera.productos.Find(id);
      if (!(productoActual.id == id))
      {
        throw new Exception("Registro No encontrado!");
      }

      Productos productoEliminar = (from c in _dbLicorera.productos where c.id == id select c).First();
      productoEliminar.estadoRegistro = false;

      _dbLicorera.Update(productoEliminar);
      _dbLicorera.SaveChanges();

      return Ok(new { Exito = true, msj = "Registro eliminado con éxito!" });
    }
    catch (System.Exception e)
    {
      return Problem(detail: "false", title: "Error: " + e.Message);
    }
  }

  private decimal obtenerTasaCambio(DateTime fecha)
  {
    try
    {
      var tasaCambio = (from tc in _dbLicorera.tasaCambio
                        where tc.fecha == fecha
                        select tc).First();
      return Convert.ToDecimal(tasaCambio.tipoCambio);
    }
    catch (System.Exception)
    {
      return 1;
    }
  }
}