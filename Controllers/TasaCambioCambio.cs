using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TestLicorera.Models;

namespace TestLicorera;

[ApiController]
[Route("[controller]")]
public class TasaCambioController : ControllerBase
{
  private readonly LicoreraDbContext _dbLicorera;
  public TasaCambioController(LicoreraDbContext dbLicorera)
  {
    _dbLicorera = dbLicorera;
  }

  [HttpGet]
  [Route("tasaCambio")]
  public async Task<IActionResult> listarTasaCambio(DateTime fecha)
  {
    try
    {
      List<TasaCambio>? tasaCambios = new List<TasaCambio>();
      tasaCambios = await _dbLicorera.tasaCambio.Where(tc => tc.fecha == fecha)
                                             .Where(tc => tc.estadoRegistro == true)
                                             .ToListAsync();

      return Ok(new { Exito = true, tipoCambio = tasaCambios });
    }
    catch (System.Exception ex)
    {
      return Problem(detail: "false", title: "Error: " + ex);
    }

  }

  [HttpGet]
  [Route("tasaCambioMes")]
  public async Task<IActionResult> listarTasaCambioMes(int mes)
  {
    try
    {
      List<TasaCambio>? tasaCambios = new List<TasaCambio>();
      tasaCambios = await _dbLicorera.tasaCambio.Where(tc => tc.mes == mes)
                                                .Where(tc => tc.estadoRegistro == true)
                                                .ToListAsync();

      return Ok(new { Exito = true, tasaCambios = tasaCambios });
    }
    catch (System.Exception ex)
    {
      return Problem(detail: "false", title: "Error: " + ex);
    }
  }

  [HttpPost]
  [Route("crearTasaCambio")]
  public IActionResult crearTasaCambio(TasaCambio tasaCambio)
  {
    try
    {
      _dbLicorera.Add(tasaCambio);
      _dbLicorera.SaveChanges();
      return Ok(new { Exito = true, msj = "Se ha creado el registro con exito!" });
    }
    catch (System.Exception ex)
    {
      return Problem(detail: "false", title: "Error: " + ex);
    }
  }

  [HttpPut]
  [Route("modificarTasaCambio")]
  public IActionResult modificarTasaCambio(TasaCambio tasaCambio)
  {
    try
    {
      var tasaCambioActual = _dbLicorera.tasaCambio.Find(tasaCambio.id);
      if (!(tasaCambioActual.id == tasaCambio.id))
      {
        throw new Exception("Registro No encontrado!");
      }

      TasaCambio nuevaTasaCambio = (from p in _dbLicorera.tasaCambio where p.id == tasaCambio.id select p).First();
      nuevaTasaCambio.mes = tasaCambio.mes;
      nuevaTasaCambio.fecha = tasaCambio.fecha;
      nuevaTasaCambio.mes = tasaCambio.mes;
      nuevaTasaCambio.tipoCambio = tasaCambio.tipoCambio;

      _dbLicorera.Update(nuevaTasaCambio);
      _dbLicorera.SaveChanges();
      return Ok(new { Exito = true, msj = "Se ha creado el registro con exito!" });
    }
    catch (System.Exception ex)
    {
      return Problem(detail: "false", title: "Error: " + ex);
    }
  }

  [HttpDelete]
  [Route("eliminarTasaCambio")]
  public IActionResult eliminarTasaCambio(int id)
  {
    try
    {
      var tasaCambioActual = _dbLicorera.tasaCambio.Find(id);
      if (!(tasaCambioActual.id == id))
      {
        throw new Exception("Registro No encontrado!");
      }

      TasaCambio nuevaTasaCambio = (from p in _dbLicorera.tasaCambio where p.id == id select p).First();
      nuevaTasaCambio.estadoRegistro = false;

      _dbLicorera.Update(nuevaTasaCambio);
      _dbLicorera.SaveChanges();
      return Ok(new { Exito = true, msj = "Se ha elimiado el registro!" });
    }
    catch (System.Exception ex)
    {
      return Problem(detail: "false", title: "Error: " + ex);
    }
  }
}