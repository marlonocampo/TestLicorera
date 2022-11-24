namespace TestLicorera.Models;
public class Facturas
{
  public int id {get; set;}
  public string? codigo {set; get;}
  public string? descripcion {set; get;}
  public int idCliente {set; get;}
  public bool estadoRegistro {set; get;}
  public float iva {set; get;}
  public float importeTotal {set; get;}
  public DateTime fecha {set; get;}
  public char estado {set; get;}
  public decimal tasaCambio {set; get;}

}