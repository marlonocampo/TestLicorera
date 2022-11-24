use LicoreraDb
go

CREATE PROCEDURE dbo.ReporteVentas
AS
BEGIN
    SELECT 
           CAST(ROW_NUMBER() OVER(ORDER BY P.codigo DESC) AS INT) AS id,
           C.nombre AS CLIENTE,
           C.codigo                                   as codCliente,
           DATEPART(MONTH, F.fecha)                   AS MES,
           DATEPART(YEAR, F.fecha)                    AS ANIO,
           SUM((FD.subtotal + FD.iva) / F.tasaCambio) AS total_dolares,
           SUM(FD.subtotal + FD.iva)                  AS total_cordoba,
           P.codigo                                   AS codProducto,
           P.descripcion                              AS producto
    FROM DBO.Clientes C
             INNER JOIN Facturas F
                        on C.id = F.idCliente
             INNER JOIN FacturaDetalle FD
                        ON FD.codigoFactura = F.codigo
             INNER JOIN Productos P
                        on FD.idProducto = P.id
    Where F.estadoRegistro = 1
    GROUP BY c.nombre, C.codigo, DATEPART(Month , F.fecha), DATEPART(YEAR, F.fecha), P.codigo, P.descripcion;;
end
go
