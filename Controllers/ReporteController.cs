using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestLicorera.Models;
using Newtonsoft.Json;

namespace TestLicorera;

[ApiController]
[Route("[controller]")]
public class ReporteController : ControllerBase
{
  private readonly LicoreraDbContext _dbLicorera;
  public ReporteController(LicoreraDbContext dbLicorera)
  {
    _dbLicorera = dbLicorera;
  }

  [HttpPost]
  [Route("reporteVentas")]
  public async Task<IActionResult> reporteVentas(ParametrosFiltro filtros)
  {
    try
    {
      List<ReporteVentas> reporteGlobal = await _dbLicorera.reportes.FromSqlRaw("exec dbo.ReporteVentas").ToListAsync();
      List<ReporteVentas> reporteFiltrado = reporteGlobal;
      if (filtros.codCliente != null)
      {
        reporteFiltrado = reporteGlobal.Where(r => r.codCliente == filtros.codCliente).ToList();
      }

      if (filtros.anio > 0)
      {
        reporteFiltrado = reporteGlobal.Where(r => r.anio == filtros.anio).ToList();
      }

      if (filtros.mes > 0)
      {
        reporteFiltrado = reporteGlobal.Where(r => r.mes == filtros.mes).ToList();
      }

      if (filtros.Cliente.Length > 0)
      {
        reporteFiltrado = reporteGlobal.Where(r => r.Cliente == filtros.Cliente).ToList();
      }

      if (filtros.codProducto.Length > 0)
      {
        reporteFiltrado = reporteGlobal.Where(r => r.codProducto == filtros.codProducto).ToList();
      }
      if (filtros.producto.Length > 0)
      {
        reporteFiltrado = reporteGlobal.Where(r => r.producto == filtros.producto).ToList();
      }

      return Ok(new { exito = true, reporteFiltrado = reporteFiltrado });
    }
    catch (System.Exception ex)
    {
      return BadRequest("Ha ocurrido un error en la busqueda!" + ex);
    }
  }
}