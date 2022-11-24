create table Clientes
(
    id              int identity
        primary key,
    nombre          varchar(50),
    apellido        varchar(50),
    fechaNacimiento date,
    codigo          varchar(20),
    estadoRegistro  bit
)
go

create table Facturas
(
    fecha          date,
    iva            money,
    importeTotal   money,
    codigo         varchar(20),
    idCliente      int
        constraint facturaCliente
            references Clientes,
    estado         char,
    id             int identity
        primary key,
    estadoRegistro bit,
    descripcion    varchar(50),
    tasaCambio     money
)
go

create table Productos
(
    id                int identity
        primary key,
    codigo            varchar(20),
    precioUnitario    decimal(8, 2),
    fechaIngreso      date,
    disponible        int,
    estadoRegistro    bit,
    descripcion       varchar(50),
    precioUnitarioDol money,
    tasaCambio        money
)
go

create table FacturaDetalle
(
    cantidad            int,
    idProducto          int
        constraint FacturaDetalle_Productos__fk
            references Productos,
    subtotal            money,
    PrecioUnitario      decimal(8, 2),
    id                  int identity
        primary key,
    codigoFactura       varchar(10),
    precioUnitarioDolar money,
    iva                 money
)
go

create table TasaCambio
(
    fecha          date,
    mes            int,
    valor          money,
    estadoRegistro bit,
    id             int identity
        primary key
)
go
