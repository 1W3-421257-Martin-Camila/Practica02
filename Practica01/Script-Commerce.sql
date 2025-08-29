CREATE DATABASE Commerce;
GO

USE Commerce;
GO


-- Métodos de pago
CREATE TABLE PaymentMethods (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(50) NOT NULL
);
GO

-- Artículos
CREATE TABLE Articles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL
);
GO

-- Facturas
CREATE TABLE Invoices (
    Number INT IDENTITY(1,1) PRIMARY KEY,
    Date DATE NOT NULL,
    PaymentMethodId INT NOT NULL,
    Customer VARCHAR(100) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Invoices_PaymentMethods FOREIGN KEY (PaymentMethodId)
        REFERENCES PaymentMethods(Id)
);
GO

-- Detalles de factura
CREATE TABLE InvoiceDetails (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    ArticleId INT NOT NULL,
    Quantity INT NOT NULL,
    InvoiceNumber INT NOT NULL,
    CONSTRAINT FK_InvoiceDetails_Articles FOREIGN KEY (ArticleId)
        REFERENCES Articles(Id),
    CONSTRAINT FK_InvoiceDetails_Invoices FOREIGN KEY (InvoiceNumber)
        REFERENCES Invoices(Number)
);
GO

-- Baja lógica de factura
CREATE PROCEDURE SP_SOFTDELETE_INVOICE
    @Number INT
AS
BEGIN
    UPDATE Invoices
    SET IsActive = 0
    WHERE Number = @Number;
END
GO

-- Obtener factura por número (con detalles)
CREATE PROCEDURE SP_GET_INVOICE
    @Number INT
AS
BEGIN
    SELECT 
        i.Number,
        i.Date,
        i.Customer,
        pm.Name AS PaymentMethod,
        d.Quantity,
        a.Name AS Article,
        a.UnitPrice,
        (d.Quantity * a.UnitPrice) AS Subtotal
    FROM Invoices i
    INNER JOIN PaymentMethods pm ON i.PaymentMethodId = pm.Id
    INNER JOIN InvoiceDetails d ON i.Number = d.InvoiceNumber
    INNER JOIN Articles a ON d.ArticleId = a.Id
    WHERE i.Number = @Number AND i.IsActive = 1;
END
GO

-- Obtener todas las facturas activas
CREATE PROCEDURE SP_GET_ALL_INVOICES
AS
BEGIN
    SELECT 
        i.Number,
        i.Date,
        i.Customer,
        pm.Id AS PaymentMethodId,   -- <-- agregamos Id
        pm.Name AS PaymentMethod
    FROM Invoices i
    INNER JOIN PaymentMethods pm ON i.PaymentMethodId = pm.Id
    WHERE i.IsActive = 1;
END

-- Guardar nueva factura
CREATE PROCEDURE SP_SAVE_INVOICE
    @Date DATE,
    @PaymentMethodId INT,
    @Customer VARCHAR(100),
    @NewNumber INT OUTPUT
AS
BEGIN
    INSERT INTO Invoices(Date, PaymentMethodId, Customer)
    VALUES(@Date, @PaymentMethodId, @Customer);

    SET @NewNumber = SCOPE_IDENTITY();
END
GO

-- Métodos de pago
INSERT INTO PaymentMethods(Name) VALUES ('Cash');
INSERT INTO PaymentMethods(Name) VALUES ('Credit Card');
INSERT INTO PaymentMethods(Name) VALUES ('Debit Card');

-- Artículos
INSERT INTO Articles(Name, UnitPrice) VALUES ('Laptop', 1200.50);
INSERT INTO Articles(Name, UnitPrice) VALUES ('Mouse', 25.00);
INSERT INTO Articles(Name, UnitPrice) VALUES ('Keyboard', 45.00);
INSERT INTO Articles(Name, UnitPrice) VALUES ('Monitor', 300.00);

-- Facturas
INSERT INTO Invoices(Date, PaymentMethodId, Customer) VALUES ('2025-08-25', 1, 'John Doe');
INSERT INTO Invoices(Date, PaymentMethodId, Customer) VALUES ('2025-08-26', 2, 'Jane Smith');

-- Detalles de facturas
INSERT INTO InvoiceDetails(ArticleId, Quantity, InvoiceNumber) VALUES (1, 1, 1); -- Laptop en factura 1
INSERT INTO InvoiceDetails(ArticleId, Quantity, InvoiceNumber) VALUES (2, 2, 1); -- 2 Mouse en factura 1
INSERT INTO InvoiceDetails(ArticleId, Quantity, InvoiceNumber) VALUES (3, 1, 2); -- Keyboard en factura 2
INSERT INTO InvoiceDetails(ArticleId, Quantity, InvoiceNumber) VALUES (4, 2, 2); -- 2 Monitores en factura 2
