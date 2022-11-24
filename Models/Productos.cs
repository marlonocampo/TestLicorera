namespace TestLicorera.Models;
public class Productos
{
  public int id { get; set; }
  public string? codigo { set; get; }
  public string? descripcion { set; get; }
  public DateTime fechaIngreso { set; get; }
  public bool estadoRegistro { set; get; }
  public int disponible { set; get; }
  public decimal precioUnitarioDol { set; get; } = 0;
  public decimal tasaCambio { set; get; }
  public decimal precioUnitario { set; get; }
}