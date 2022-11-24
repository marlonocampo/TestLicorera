namespace TestLicorera.Models;
public class ReporteVentas
{
  public int id { set; get; }
  public string codCliente { get; set; }
  public string Cliente { get; set; }
  public int mes { get; set; }
  public int anio { get; set; }
  public decimal total_cordoba { get; set; }
  public decimal total_dolares { get; set; }
  public string codProducto { get; set; }
  public string producto { get; set; }
}