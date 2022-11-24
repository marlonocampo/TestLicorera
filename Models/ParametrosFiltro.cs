namespace TestLicorera.Models;
public class ParametrosFiltro
{
  public int id { set; get; }
  public string? codCliente { get; set; } = null;
  public string? Cliente { get; set; }
  public int mes { get; set; }
  public int anio { get; set; }
  public string? codProducto { get; set; } = null;
  public string? producto { get; set; } = null;
}