USE master;
GO

IF EXISTS (SELECT * FROM sys.databases WHERE name = 'SistemaAlmacen')
BEGIN
    ALTER DATABASE SistemaAlmacen SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE SistemaAlmacen;
END
GO

CREATE DATABASE SistemaAlmacen;
GO

USE SistemaAlmacen;
GO


CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) PRIMARY KEY,
    UsuarioNombre NVARCHAR(50) NOT NULL,
    Contraseña NVARCHAR(255) NOT NULL,
    Rol NVARCHAR(30)
);

CREATE TABLE Proveedores (
    IdProveedor INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(20),
    Direccion NVARCHAR(200),
    Email NVARCHAR(100)
);

CREATE TABLE Distribuidores (
    IdDistribuidor INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    Telefono NVARCHAR(20),
    Direccion NVARCHAR(200),
    Catalogo NVARCHAR(200),
    Email NVARCHAR(100)
);

CREATE TABLE Clientes (
    IdCliente INT IDENTITY(1,1) PRIMARY KEY,
    Nombre NVARCHAR(100) NOT NULL,
    RFC NVARCHAR(20),
    Direccion NVARCHAR(200),
    Telefono NVARCHAR(20),
    Email NVARCHAR(100)
);

CREATE TABLE Articulos (
    IdArticulo INT IDENTITY(1,1) PRIMARY KEY,
    CodigoInterno NVARCHAR(50),
    CodigoBarras NVARCHAR(50),
    Descripcion NVARCHAR(200) NOT NULL,
    Marca NVARCHAR(100),
    Cantidad INT DEFAULT 0,
    PrecioCompra DECIMAL(10,2) DEFAULT 0,
    PrecioVenta DECIMAL(10,2) DEFAULT 0,
    IdProveedor INT NULL REFERENCES Proveedores(IdProveedor),
    IdDistribuidor INT NULL REFERENCES Distribuidores(IdDistribuidor)
);

CREATE TABLE Pedidos (
    IdPedido INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATETIME DEFAULT GETDATE(),
    TipoPedido NVARCHAR(30),
    Estado NVARCHAR(30),
    IdProveedor INT REFERENCES Proveedores(IdProveedor)
);

CREATE TABLE DetallePedidos (
    IdDetallePedido INT IDENTITY(1,1) PRIMARY KEY,
    IdPedido INT REFERENCES Pedidos(IdPedido),
    IdArticulo INT REFERENCES Articulos(IdArticulo),
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2)
);

CREATE TABLE Facturas (
    IdFactura INT IDENTITY(1,1) PRIMARY KEY,
    Fecha DATETIME DEFAULT GETDATE(),
    Total DECIMAL(10,2) DEFAULT 0,
    IdCliente INT REFERENCES Clientes(IdCliente)
);

CREATE TABLE DetalleFacturas (
    IdDetalleFactura INT IDENTITY(1,1) PRIMARY KEY,
    IdFactura INT REFERENCES Facturas(IdFactura),
    IdArticulo INT REFERENCES Articulos(IdArticulo),
    Cantidad INT NOT NULL,
    PrecioUnitario DECIMAL(10,2)
);
GO


-- ---> USUARIOS
INSERT INTO Usuarios (UsuarioNombre, Contraseña, Rol) VALUES 
('admin', '1234', 'Administrador'),
('empleado', '1234', 'Empleado'),
('cajero', '1234', 'Empleado');

-- ---> DISTRIBUIDORES (Genéricos)
INSERT INTO Distribuidores (Nombre, Telefono, Email) VALUES 
('Logística Nacional', '55-0000-0000', 'logistica@mx.com'),
('Transportes Rápidos', '55-1111-1111', 'envios@rapidos.com');

-- ---> PROVEEDORES (10 Reales)
INSERT INTO Proveedores (Nombre, Telefono, Direccion, Email) VALUES 
('Samsung Electronics', '55-1234-5678', 'Av. Santa Fe 455, CDMX', 'contacto@samsung.com'),
('Sony México', '55-9876-5432', 'Paseo de la Reforma 222, CDMX', 'ventas@sony.com.mx'),
('Logitech Distributors', '33-4567-8901', 'Vallarta 2440, Guadalajara', 'partners@logitech.com'),
('Coca-Cola FEMSA', '81-8151-1400', 'Alfonso Reyes 2202, Monterrey', 'pedidos@kof.com.mx'),
('Bimbo SA de CV', '55-5268-6600', 'Prolongación Paseo 100, CDMX', 'ventas@grupobimbo.com'),
('Procter & Gamble', '55-5724-2000', 'Loma Florida 32, Naucalpan', 'atencion@pg.com'),
('Sabritas (PepsiCo)', '55-2020-3030', 'Bosques de Duraznos 67, CDMX', 'ventas@pepsico.com'),
('Nestlé Profesional', '55-5262-5000', 'Miguel de Cervantes 301, CDMX', 'servicios@nestle.com'),
('Papelería Mayorista S.A.', '33-3636-7890', 'Lázaro Cárdenas 1200, GDL', 'contacto@papeleriamayorista.com'),
('TechData Solutions', '55-1111-2222', 'Insurgentes Sur 800, CDMX', 'sales@techdata.com');

-- ---> CLIENTES (10 Diversos)
INSERT INTO Clientes (Nombre, RFC, Direccion, Telefono, Email) VALUES 
('Público en General', 'XAXX010101000', 'Mostrador', '000-0000', NULL),
('Juan Pérez López', 'PELJ800101H20', 'Calle 10 #45, Centro', '55-1122-3344', 'juan.perez@gmail.com'),
('Abarrotes La Esperanza', 'ALE901212I56', 'Av. Revolución 500', '33-2211-4455', 'contacto@laesperanza.com'),
('María González', 'GOMA950505M10', 'Privada Los Pinos 8', '81-8899-7766', 'maria.gonzalez@outlook.com'),
('Constructora del Norte', 'CNO150320AB1', 'Carretera Nacional Km 5', '81-1234-5678', 'compras@constructora.com'),
('Escuela Primaria Benito Juárez', 'EPB450909KH2', 'Hidalgo 200, Centro', '55-9988-7766', 'direccion@escuelabj.edu.mx'),
('Roberto Gómez Bolaños', 'GOBR290221CH8', 'Vecindad del Chavo 8', '55-1234-4321', 'chespirito@televisa.com'),
('Cybercafé Matrix', 'CMA101010WE1', 'Plaza Tecnológica Local 4', '33-3333-3333', 'admin@matrix.com'),
('Ana Karenina', 'KAAA880101UR5', 'Calle Rusia 1877', '55-0000-9999', 'ana.k@libros.com'),
('Restaurante El Buen Sazón', 'RBS200101QA9', 'Av. Gastronómica 55', '664-123-4567', 'chef@buensazon.com');

-- ELECTRÓNICA
INSERT INTO Articulos (CodigoInterno, CodigoBarras, Descripcion, Marca, Cantidad, PrecioCompra, PrecioVenta, IdProveedor) VALUES
('ELEC-001', '750100000001', 'Smart TV 55 Pulgadas 4K', 'Samsung', 10, 8000.00, 12500.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Samsung%')),
('ELEC-002', '750100000002', 'Galaxy S23 Ultra 256GB', 'Samsung', 15, 18000.00, 24000.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Samsung%')),
('ELEC-003', '750100000003', 'Audífonos Bluetooth Buds2', 'Samsung', 30, 1500.00, 2300.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Samsung%')),
('ELEC-005', '750100000005', 'PlayStation 5 Consola', 'Sony', 20, 9500.00, 11999.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Sony%')),
('ELEC-006', '750100000006', 'Audífonos Noise Cancelling', 'Sony', 12, 5000.00, 7500.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Sony%')),
('ELEC-009', '750100000009', 'Mouse MX Master 3', 'Logitech', 40, 1500.00, 2100.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Logitech%')),
('ELEC-010', '750100000010', 'Teclado Mecánico RGB', 'Logitech', 35, 1800.00, 2600.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Logitech%')),
('ELEC-014', '750100000014', 'Memoria USB 64GB', 'Kingston', 100, 120.00, 250.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%TechData%'));

-- BEBIDAS Y ABARROTES
INSERT INTO Articulos (CodigoInterno, CodigoBarras, Descripcion, Marca, Cantidad, PrecioCompra, PrecioVenta, IdProveedor) VALUES
('BEB-001', '7501055300075', 'Coca-Cola 600ml', 'Coca-Cola', 200, 12.00, 18.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Coca-Cola%')),
('BEB-002', '7501055300082', 'Coca-Cola 2.5 Litros', 'Coca-Cola', 100, 28.00, 42.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Coca-Cola%')),
('BEB-003', '7501055300099', 'Sprite 600ml', 'Sprite', 150, 12.00, 18.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Coca-Cola%')),
('BEB-004', '7501055300105', 'Agua Ciel 1L', 'Ciel', 300, 8.00, 14.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Coca-Cola%')),
('PAN-001', '7501000111206', 'Pan Blanco Grande', 'Bimbo', 50, 32.00, 48.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Bimbo%')),
('PAN-002', '7501000111213', 'Pan Integral', 'Bimbo', 40, 35.00, 52.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Bimbo%')),
('PAN-005', '7501000111244', 'Nito', 'Bimbo', 80, 10.00, 16.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Bimbo%')),
('SNA-001', '7501011100010', 'Sabritas Sal 45g', 'Sabritas', 100, 13.00, 19.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Sabritas%')),
('SNA-003', '7501011100034', 'Doritos Nacho 50g', 'Sabritas', 120, 14.00, 20.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Sabritas%')),
('SNA-004', '7501011100041', 'Cheetos Torciditos 45g', 'Sabritas', 110, 12.00, 17.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Sabritas%')),
('GRO-001', '7501022200015', 'Nescafé Clásico 225g', 'Nescafé', 40, 85.00, 115.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Nestlé%')),
('GRO-002', '7501022200022', 'Leche Condensada La Lechera', 'Nestlé', 60, 22.00, 30.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Nestlé%'));

-- HOGAR
INSERT INTO Articulos (CodigoInterno, CodigoBarras, Descripcion, Marca, Cantidad, PrecioCompra, PrecioVenta, IdProveedor) VALUES
('LIM-001', '7501033300019', 'Detergente Ariel 1kg', 'Ariel', 50, 35.00, 52.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Procter%')),
('LIM-003', '7501033300033', 'Shampoo Head & Shoulders', 'H&S', 60, 55.00, 85.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Procter%')),
('LIM-004', '7501033300040', 'Pasta Dental Crest', 'Crest', 80, 20.00, 35.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Procter%')),
('LIM-005', '7501033300057', 'Rastrillos Gillette Mach3', 'Gillette', 40, 110.00, 160.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Procter%'));

-- PAPELERÍA
INSERT INTO Articulos (CodigoInterno, CodigoBarras, Descripcion, Marca, Cantidad, PrecioCompra, PrecioVenta, IdProveedor) VALUES
('PAP-001', '7501044400013', 'Cuaderno Profesional Raya', 'Scribe', 200, 18.00, 28.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Papelería%')),
('PAP-004', '7501044400044', 'Bolígrafo Negro', 'Bic', 500, 4.00, 7.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Papelería%')),
('PAP-007', '7501044400075', 'Tijeras Escolares', 'Maped', 80, 25.00, 40.00, (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Papelería%'));

-- GENERADOR AUTOMÁTICO PARA LLEGAR A 100 ARTÍCULOS
DECLARE @i INT = 1;
WHILE @i <= 70
BEGIN
    INSERT INTO Articulos (CodigoInterno, CodigoBarras, Descripcion, Marca, Cantidad, PrecioCompra, PrecioVenta, IdProveedor)
    VALUES (
        'GEN-' + RIGHT('000' + CAST(@i AS NVARCHAR), 3), 
        '750999' + RIGHT('000000' + CAST(@i AS NVARCHAR), 6),
        'Producto Variado #' + CAST(@i AS NVARCHAR), 
        'Genérico', 
        FLOOR(RAND()*100) + 10, 
        CAST(FLOOR(RAND()*500) + 10 AS DECIMAL(10,2)), 
        0, 
        (SELECT TOP 1 IdProveedor FROM Proveedores ORDER BY NEWID()) -- Proveedor Aleatorio
    );
    SET @i = @i + 1;
END
-- Ajustar precios de venta de los genéricos (30% ganancia)
UPDATE Articulos SET PrecioVenta = PrecioCompra * 1.3 WHERE Marca = 'Genérico';
GO

-- Pedido 1 (Coca Cola)
INSERT INTO Pedidos (Fecha, TipoPedido, Estado, IdProveedor) VALUES 
(DATEADD(day, -10, GETDATE()), 'Resurtido', 'Completado', (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Coca-Cola%'));

INSERT INTO DetallePedidos (IdPedido, IdArticulo, Cantidad, PrecioUnitario) VALUES
((SELECT MAX(IdPedido) FROM Pedidos), (SELECT TOP 1 IdArticulo FROM Articulos WHERE Marca='Coca-Cola'), 100, 12.00);

-- Pedido 2 (Sabritas)
INSERT INTO Pedidos (Fecha, TipoPedido, Estado, IdProveedor) VALUES 
(DATEADD(day, -5, GETDATE()), 'Resurtido', 'Completado', (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Sabritas%'));

INSERT INTO DetallePedidos (IdPedido, IdArticulo, Cantidad, PrecioUnitario) VALUES
((SELECT MAX(IdPedido) FROM Pedidos), (SELECT TOP 1 IdArticulo FROM Articulos WHERE Marca='Sabritas'), 50, 13.00);

-- Pedido 3 (Samsung)
INSERT INTO Pedidos (Fecha, TipoPedido, Estado, IdProveedor) VALUES 
(DATEADD(day, -2, GETDATE()), 'Especial', 'Pendiente', (SELECT TOP 1 IdProveedor FROM Proveedores WHERE Nombre LIKE '%Samsung%'));

INSERT INTO DetallePedidos (IdPedido, IdArticulo, Cantidad, PrecioUnitario) VALUES
((SELECT MAX(IdPedido) FROM Pedidos), (SELECT TOP 1 IdArticulo FROM Articulos WHERE Marca='Samsung'), 5, 8000.00);
GO

-- Venta 1 (Público General)
INSERT INTO Facturas (Fecha, IdCliente, Total) VALUES (DATEADD(day, -3, GETDATE()), 1, 0);
INSERT INTO DetalleFacturas (IdFactura, IdArticulo, Cantidad, PrecioUnitario) VALUES
((SELECT MAX(IdFactura) FROM Facturas), (SELECT TOP 1 IdArticulo FROM Articulos WHERE Marca='Coca-Cola'), 2, 18.00),
((SELECT MAX(IdFactura) FROM Facturas), (SELECT TOP 1 IdArticulo FROM Articulos WHERE Marca='Sabritas'), 1, 19.00);

-- Venta 2 (Cliente Frecuente)
INSERT INTO Facturas (Fecha, IdCliente, Total) VALUES (GETDATE(), 2, 0);
INSERT INTO DetalleFacturas (IdFactura, IdArticulo, Cantidad, PrecioUnitario) VALUES
((SELECT MAX(IdFactura) FROM Facturas), (SELECT TOP 1 IdArticulo FROM Articulos WHERE Marca='Samsung'), 1, 12500.00);

-- Actualizar Totales de Facturas automáticamente
UPDATE Facturas
SET Total = (SELECT ISNULL(SUM(Cantidad * PrecioUnitario), 0) FROM DetalleFacturas WHERE DetalleFacturas.IdFactura = Facturas.IdFactura);
GO

PRINT 'Base de datos generada y poblada correctamente.';