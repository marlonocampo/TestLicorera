namespace TestLicorera.Models;
public class Facturacion
{
  public string? codigoFactura {set; get;}
  public int idProducto {set; get;}
  public int cantidad {set; get;}
  public int idCliente {set; get;}
  public DateTime fecha {set; get;}
  public string? descripcion {set; get;}

  public bool iva {set; get;}
}