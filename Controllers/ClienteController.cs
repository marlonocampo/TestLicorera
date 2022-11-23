using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace TestLicorera.Models;

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
  [Route("obtenerClientes")]
  public async Task<IActionResult> obtenerClientes()
  {
    var clientes = await _dbLicorera.clientes.ToListAsync();
    return Ok(clientes);
  }
}