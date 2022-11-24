namespace TestLicorera.Models;
public class TasaCambio
{
  public int id { get; set; }
  public DateTime fecha { set; get; }
  public decimal valor { set; get; }
  public int mes { set; get; }
  public bool estadoRegistro { set; get; }
}
