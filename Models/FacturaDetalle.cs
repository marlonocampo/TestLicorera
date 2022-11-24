namespace TestLicorera.Models;
public class FacturaDetalle
{
  public int id { set; get; }
  public string? codigoFactura { set; get; }
  public decimal precioUnitario { set; get; }
  public decimal precioUnitarioDolar { set; get; }
  public int idProducto { set; get; }
  public decimal iva { set; get; }

  public int cantidad { set; get; }
  public decimal subTotal { set; get; }
}