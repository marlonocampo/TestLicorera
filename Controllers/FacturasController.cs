using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestLicorera.Models;

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
  [Route("listarFactura")]
  public async Task<IActionResult> listarFacturas()
  {
    List<Facturas> facturas = await _dbLicorera.facturas.ToListAsync();
    return Ok(new { Exito = true, facturas = facturas });
  }

  [HttpGet]
  [Route("listarFacturas")]
  public async Task<IActionResult> listarFacturas(string codigo)
  {
    var facturas = await (from f in _dbLicorera.facturas
                          join fd in _dbLicorera.facturaDetalle on f.id equals fd.idFactura
                          join c in _dbLicorera.clientes on f.idCliente equals c.id
                          select new
                          {
                            codigo = f.codigo
                          }).ToListAsync();

    return Ok(new { Exito = true, facturas = facturas });
  }

}