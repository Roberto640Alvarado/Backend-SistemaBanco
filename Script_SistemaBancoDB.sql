--###### BASE DE DATO #######
CREATE DATABASE SistemaBanco;

USE SistemaBanco;

--###### TABLA CLIENTES #######
CREATE TABLE Cliente (
    CodigoCliente INT PRIMARY KEY IDENTITY(1,1), 
    Nombre NVARCHAR(100) NOT NULL,
    Apellido NVARCHAR(100) NOT NULL,
    NumeroTarjeta NVARCHAR(16) NOT NULL UNIQUE,
    LimiteCredito DECIMAL(10,2) NOT NULL,
    SaldoDisponible DECIMAL(10,2) NOT NULL DEFAULT 0
);

--###### TABLA TIPO TRANSACCION #######
CREATE TABLE TipoTransaccion (
    CodigoTipoTransaccion INT PRIMARY KEY IDENTITY(1,1),
    Descripcion NVARCHAR(50) NOT NULL
);


--###### TABLA TRANSACCION #######
CREATE TABLE Transaccion (
    CodigoTransaccion INT PRIMARY KEY IDENTITY(1,1),
    CodigoCliente INT NOT NULL,
    CodigoTipoTransaccion INT NOT NULL,
    Fecha DATETIME NOT NULL,
    Descripcion NVARCHAR(255) NULL,
    Monto DECIMAL(10,2) NOT NULL,
    FOREIGN KEY (CodigoCliente) REFERENCES Cliente(CodigoCliente),
    FOREIGN KEY (CodigoTipoTransaccion) REFERENCES TipoTransaccion(CodigoTipoTransaccion)
);


--###### TABLA CONFIGURACION #######
CREATE TABLE Configuracion (
    CodigoConfiguracion INT PRIMARY KEY IDENTITY(1,1),
    PorcentajeInteres DECIMAL(5,2) NOT NULL,
    PorcentajeSaldoMinimo DECIMAL(5,2) NOT NULL
);

--###### PROCEDIMIENTO ALMACENADO - ESTADO DE CUENTA #######
CREATE PROCEDURE ObtenerEstadoGeneralCliente
    @CodigoCliente INT
AS
BEGIN
    DECLARE @SaldoTotal DECIMAL(10, 2);
    DECLARE @PorcentajeInteres DECIMAL(5, 2);
    DECLARE @InteresBonificable DECIMAL(10, 2);

    SELECT --Obtener configuración
        @PorcentajeInteres = PorcentajeInteres
    FROM Configuracion;

    --Calcular saldo total
    SELECT @SaldoTotal = 
        SUM(CASE 
                WHEN tt.CodigoTipoTransaccion = 1 THEN t.Monto  -- Compras(Cargos)
                WHEN tt.CodigoTipoTransaccion = 2 THEN -t.Monto -- Pagos(Abonos)
                ELSE 0
            END)
    FROM Transaccion t
    INNER JOIN TipoTransaccion tt ON t.CodigoTipoTransaccion = tt.CodigoTipoTransaccion
    WHERE t.CodigoCliente = @CodigoCliente;

    --Si no hay transacciones, asignar un saldo total de 0
    SET @SaldoTotal = ISNULL(@SaldoTotal, 0);

    --Calcular el interes bonificable
    SET @InteresBonificable = @SaldoTotal * (@PorcentajeInteres / 100);

    --Mostrar información general del cliente
    SELECT 
        c.Nombre + ' ' + c.Apellido AS NombreCompleto,
        c.NumeroTarjeta,
        @SaldoTotal AS SaldoActual,
        c.LimiteCredito,
        @InteresBonificable AS InteresBonificable,
        c.LimiteCredito - @SaldoTotal AS SaldoDisponible
    FROM Cliente c
    WHERE c.CodigoCliente = @CodigoCliente;
END;


--###### PROCEDIMIENTO ALMACENADO - TRANSACCIONES DEL MES ACTUAL #######
CREATE PROCEDURE ObtenerTransaccionesMesActual
    @CodigoCliente INT
AS
BEGIN
    SELECT 
        t.CodigoTransaccion AS NumeroAutorizacion,
        t.Fecha,
        t.Descripcion,
        CASE 
            WHEN tt.CodigoTipoTransaccion = 1 THEN t.Monto --Compras
            ELSE 0 
        END AS Cargo,
        CASE 
            WHEN tt.CodigoTipoTransaccion = 2 THEN t.Monto --Pagos
            ELSE 0 
        END AS Abono
    FROM Transaccion t
    INNER JOIN TipoTransaccion tt ON t.CodigoTipoTransaccion = tt.CodigoTipoTransaccion
    WHERE t.CodigoCliente = @CodigoCliente
        AND MONTH(t.Fecha) = MONTH(GETDATE())
        AND YEAR(t.Fecha) = YEAR(GETDATE())
    ORDER BY t.Fecha DESC;
END;

--###### PROCEDIMIENTO ALMACENADO - COMPRAS MES ACTUAL Y ANTERIOR #######
CREATE PROCEDURE ObtenerTotalesCompras
    @CodigoCliente INT
AS
BEGIN
    --Mostrar montos totales de compras
    SELECT 
        SUM(CASE WHEN MONTH(t.Fecha) = MONTH(GETDATE()) THEN t.Monto ELSE 0 END) AS ComprasMesActual,
        SUM(CASE WHEN MONTH(t.Fecha) = MONTH(DATEADD(MONTH, -1, GETDATE())) THEN t.Monto ELSE 0 END) AS ComprasMesAnterior
    FROM Transaccion t
    INNER JOIN TipoTransaccion tt ON t.CodigoTipoTransaccion = tt.CodigoTipoTransaccion
    WHERE t.CodigoCliente = @CodigoCliente
        AND tt.CodigoTipoTransaccion = 1; --compras
END;


--############################## AGREGANDO DATOS A TABLAS ############################################
INSERT INTO TipoTransaccion (Descripcion)
VALUES ('Compra'), ('Pago');


INSERT INTO Cliente (Nombre, Apellido, NumeroTarjeta, LimiteCredito, SaldoDisponible)
VALUES ('Roberto', 'Alvarado', '5454123456781234', 10000.00, 9000.00);


INSERT INTO Transaccion (CodigoCliente, CodigoTipoTransaccion, Fecha, Descripcion, Monto)
VALUES (1, 1, GETDATE(), 'Compra de Regalos', 100.00);


INSERT INTO Transaccion (CodigoCliente, CodigoTipoTransaccion, Fecha, Descripcion, Monto)
VALUES (1, 2, GETDATE(), 'Pago mensual tarjeta de crédito', 200.00);

--configuraciones iniciales
INSERT INTO Configuracion (PorcentajeInteres, PorcentajeSaldoMinimo)
VALUES (25, 5);

--Ejecutar los procedimientos
EXEC ObtenerEstadoGeneralCliente @CodigoCliente = 1;
EXEC ObtenerTransaccionesMesActual @CodigoCliente = 1;
EXEC ObtenerTotalesCompras @CodigoCliente = 1;

--########################################################################################
SELECT * FROM Cliente
SELECT * FROM TipoTransaccion
SELECT * FROM Transaccion