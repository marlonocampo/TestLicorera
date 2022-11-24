namespace TestLicorera.Models;
public class FacturaDetalle
{
  public int id {get; set;}
  public int idFactura {set; get;}
  public int precioUnitario {set; get;}
  public int cantidad {set; get;}
  public int idProducto {set; get;}
  public float subTotal {set; get;}
}