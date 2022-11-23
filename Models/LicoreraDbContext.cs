using Microsoft.EntityFrameworkCore;
namespace TestLicorera.Models;

public class LicoreraDbContext : DbContext
{
  public LicoreraDbContext(DbContextOptions<LicoreraDbContext> options) : base(options)
  {

  }
  public DbSet<Clientes> clientes { get; set; } = null!;
}