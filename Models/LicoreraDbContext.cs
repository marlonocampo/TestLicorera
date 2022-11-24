using Microsoft.EntityFrameworkCore;
namespace TestLicorera.Models;

public class LicoreraDbContext : DbContext
{
  public LicoreraDbContext(DbContextOptions<LicoreraDbContext> options) : base(options)
  {

  }
  public DbSet<Clientes> clientes { get; set; } = null!;
  public DbSet<Productos> productos { get; set; } = null!;
  public DbSet<TasaCambio> tasaCambio { get; set; } = null!;
  public DbSet<Facturas> facturas { get; set; } = null!;
  public DbSet<FacturaDetalle> facturadetalle { get; set; } = null!;
  public DbSet<ReporteVentas> reportes { get; set; } = null!;

}