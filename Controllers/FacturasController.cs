using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestLicorera.Models;
using Newtonsoft.Json;

namespace TestLicorera;

[ApiController]
[Route("[controller]")]
public class FacturasController : ControllerBase
{
  private readonly LicoreraDbContext _dbLicorera;
  public FacturasController(LicoreraDbContext dbLicorera)
  {
    _dbLicorera = dbLicorera;
  }

  [HttpGet]
  [Route("listarFacturas")]
  public async Task<IActionResult> listarFacturas()
  {
    List<Facturas> facturas = await _dbLicorera.facturas.ToListAsync();
    List<String> detallesFacturas = new List<string>();
    foreach (Facturas factura in facturas)
    {
      var objct = new
      {
        codigo = factura.codigo,
        importeTotal = factura.importeTotal,
        cliente = _dbLicorera.clientes.Where(c => c.id == factura.idCliente).First().nombre,
        detalleFactura = from df in _dbLicorera.facturadetalle
                         where (df.codigoFactura == factura.codigo)
                         select new
                         {
                           precioUnitario = df.precioUnitario,
                           precioDolar = df.precioUnitarioDolar,
                           cantidad = df.cantidad,
                           subtotal = df.subTotal
                         }
      };
      detallesFacturas.Add(JsonConvert.SerializeObject(objct));
    }

    return Ok(new { Exito = true, facturas = detallesFacturas });
  }

  [HttpGet]
  [Route("listarFacturasCodigo/{codigoFactura}")]
  public IActionResult listarFacturasCodigo(string codigoFactura)
  {
    List<Facturas> facturas = new List<Facturas>();
    facturas = _dbLicorera.facturas.Where(f => f.codigo == codigoFactura)
                                         .ToList();

    List<String> detallesFacturas = new List<string>();

    foreach (Facturas factura in facturas)
    {
      var objct = new
      {
        codigo = factura.codigo,
        importeTotal = factura.importeTotal,
        cliente = _dbLicorera.clientes.Where(c => c.id == factura.idCliente).First().nombre,
        detalleFactura = from df in _dbLicorera.facturadetalle
                         where (df.codigoFactura == factura.codigo)
                         select new
                         {
                           precioUnitario = df.precioUnitario,
                           precioDolar = df.precioUnitarioDolar,
                           cantidad = df.cantidad,
                           subtotal = df.subTotal
                         }
      };
      detallesFacturas.Add(JsonConvert.SerializeObject(objct));
    }

    return Ok(new { Exito = true, facturas = detallesFacturas });
  }

  [HttpGet]
  [Route("listarFacturasCliente")]
  public IActionResult listarFacturasCliente(int idCliente)
  {
    List<Facturas> facturas = new List<Facturas>();

    facturas = _dbLicorera.facturas.Where(f => f.idCliente == idCliente)
                                         .ToList();

    List<String> detallesFacturas = new List<string>();
    foreach (Facturas factura in facturas)
    {
      var objct = new
      {
        codigo = factura.codigo,
        importeTotal = factura.importeTotal,
        cliente = _dbLicorera.clientes.Where(c => c.id == factura.idCliente).First().nombre,
        detalleFactura = from df in _dbLicorera.facturadetalle
                         where (df.codigoFactura == factura.codigo)
                         select new
                         {
                           precioUnitario = df.precioUnitario,
                           precioDolar = df.precioUnitarioDolar,
                           cantidad = df.cantidad,
                           subtotal = df.subTotal
                         }
      };
      detallesFacturas.Add(JsonConvert.SerializeObject(objct));
    }
    return Ok(new { Exito = true, facturas = detallesFacturas });
  }


  [HttpPost]
  [Route("crearFactura")]
  public IActionResult crearFactura(List<Facturacion> data)
  {
    try
    {
      var tasaCambio = (from tc in _dbLicorera.tasaCambio
                        where tc.fecha == data.First().fecha
                        select tc).First();


      Facturas factura = new Facturas();
      factura.codigo = data.First().codigoFactura;
      factura.idCliente = data.First().idCliente;
      factura.fecha = data.First().fecha;
      factura.tasaCambio = tasaCambio.valor;
      factura.descripcion = data.First().descripcion;
      factura.estado = 'A';
      factura.estadoRegistro = true;
      decimal iTotalBruto = crearDetalleFactura(data, tasaCambio.valor); //Devuelve el importe total bruto
      if (data.First().iva)
      {
        factura.iva = iTotalBruto * (decimal)0.15;
      }
      factura.importeTotal = iTotalBruto + factura.iva;

      _dbLicorera.Add(factura);
      _dbLicorera.SaveChanges();
      return Ok(new { exito = true, msj = "factura creada correctamente!" });
    }
    catch (System.Exception ex)
    {
      return Problem(detail: "false", title: "Error" + ex.Message);
    }
  }

  private decimal crearDetalleFactura(List<Facturacion> data, decimal tasaCambio)
  {
    decimal importeTotal = 0;
    List<FacturaDetalle> facturaDetalle = new List<FacturaDetalle>();
    foreach (Facturacion linea in data)
    {
      FacturaDetalle nuevaLinea = new FacturaDetalle();
      nuevaLinea.precioUnitario = _dbLicorera.productos.Where(p => p.id == linea.idProducto).First().precioUnitario;
      nuevaLinea.cantidad = linea.cantidad;
      nuevaLinea.codigoFactura = linea.codigoFactura;
      nuevaLinea.idProducto = linea.idProducto;
      nuevaLinea.subTotal = (nuevaLinea.precioUnitario * linea.cantidad);
      nuevaLinea.precioUnitarioDolar = (nuevaLinea.precioUnitario / tasaCambio);
      nuevaLinea.iva = (nuevaLinea.precioUnitario * linea.cantidad) * (decimal)0.15;
      importeTotal += nuevaLinea.subTotal;
      _dbLicorera.Add(nuevaLinea);
      _dbLicorera.SaveChanges();
    }

    return importeTotal;
  }

  [HttpDelete]
  [Route("eliminarFactura")]
  public IActionResult eliminarFactura(int id)
  {
    try
    {
      var facturaActual = _dbLicorera.facturas.Find(id);
      if (!(facturaActual.id == id))
      {
        throw new Exception("Registro No encontrado!");
      }

      Facturas facturaEliminar = (from f in _dbLicorera.facturas where f.id == id select f).First();
      facturaEliminar.estadoRegistro = false;

      _dbLicorera.Update(facturaEliminar);
      _dbLicorera.SaveChanges();
      return Ok(new { exito = true, msj = "factura borrada!" });
    }
    catch (System.Exception ex)
    {
      return Problem(detail: "false", title: "Error" + ex.Message);
    }
  }

  [HttpPost]
  [Route("prueba")]
  public IActionResult prueba(List<Facturas> data)
  {
    try
    {
      return Ok(new { exito = true, msj = data });
    }
    catch (System.Exception ex)
    {
      return Problem(detail: "false", title: "Error" + ex.Message);
    }
  }

}