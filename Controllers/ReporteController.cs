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

  [HttpGet]
  [Route("reporteVentas")]
  public IActionResult reporteVentas(string busqueda = "", int id = 0)
  {
    try
    {
      
      
      return Ok(new { exito = true, msj = "" });
    }
    catch (System.Exception ex)
    {
      return Problem(detail: "false", title: "Error" + ex.Message);
    }
  }


}