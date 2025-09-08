CREATE DATABASE Commerce;
GO

USE Commerce;
GO

-- CREACIÓN DE TABLAS
-- Métodos de pago
CREATE TABLE PaymentMethods (
    Id INT IDENTITY(1,1) PRIMARY KEY NOT NULL,
    Name VARCHAR(50) NOT NULL
);
GO

-- Artículos
CREATE TABLE Articles (
    Id INT IDENTITY(1,1) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    UnitPrice DECIMAL(10,2) NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1
);
GO

-- Facturas
CREATE TABLE Invoices (
    Number INT IDENTITY(1,1) PRIMARY KEY,
    InvoiceDate DATE NOT NULL,
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

-- INSERTS
-- Métodos de pago
INSERT INTO PaymentMethods(Name) VALUES ('Efectivo');
INSERT INTO PaymentMethods(Name) VALUES ('Tarjeta de Crédito');
INSERT INTO PaymentMethods(Name) VALUES ('Tarjeta de Débito');
INSERT INTO PaymentMethods(Name) VALUES ('Transferencia Bancaria');
GO

-- Artículos de librería
INSERT INTO Articles(Name, UnitPrice) VALUES ('Cuaderno rayado A4', 1500.00);
INSERT INTO Articles(Name, UnitPrice) VALUES ('Bolígrafo azul', 200.00);
INSERT INTO Articles(Name, UnitPrice) VALUES ('Lápiz HB', 120.00);
INSERT INTO Articles(Name, UnitPrice) VALUES ('Resaltador amarillo', 350.00);
INSERT INTO Articles(Name, UnitPrice) VALUES ('Carpeta anillada', 2500.00);
INSERT INTO Articles(Name, UnitPrice) VALUES ('Block de hojas A4', 1800.00);
GO

-- Facturas
INSERT INTO Invoices(InvoiceDate, PaymentMethodId, Customer) VALUES ('2025-08-25', 1, 'Carlos Pérez');
INSERT INTO Invoices(InvoiceDate, PaymentMethodId, Customer) VALUES ('2025-08-26', 2, 'María López');
INSERT INTO Invoices(InvoiceDate, PaymentMethodId, Customer) VALUES ('2025-08-27', 3, 'Lucía Fernández');
GO

-- Detalles de facturas
INSERT INTO InvoiceDetails(ArticleId, Quantity, InvoiceNumber) VALUES (1, 2, 1); 
INSERT INTO InvoiceDetails(ArticleId, Quantity, InvoiceNumber) VALUES (2, 5, 1); 
INSERT INTO InvoiceDetails(ArticleId, Quantity, InvoiceNumber) VALUES (3, 3, 2); 
INSERT INTO InvoiceDetails(ArticleId, Quantity, InvoiceNumber) VALUES (5, 1, 2); 
INSERT INTO InvoiceDetails(ArticleId, Quantity, InvoiceNumber) VALUES (4, 4, 3); 
INSERT INTO InvoiceDetails(ArticleId, Quantity, InvoiceNumber) VALUES (6, 2, 3); 
GO

-- PROCEDIMIENTOS ALMACENADOS

--FACTURAS
-- Guardar factura (insert/update)
CREATE PROCEDURE SP_SAVE_INVOICE
    @Number INT,
    @InvoiceDate DATE,
    @PaymentMethodId INT,
    @Customer VARCHAR(100)
AS
BEGIN
    IF @Number = 0
    BEGIN
        INSERT INTO Invoices (InvoiceDate, PaymentMethodId, Customer)
        VALUES (@InvoiceDate, @PaymentMethodId, @Customer);

        SELECT SCOPE_IDENTITY() AS NewInvoiceNumber;
    END
    ELSE
    BEGIN
        UPDATE Invoices
        SET InvoiceDate = @InvoiceDate,
            PaymentMethodId = @PaymentMethodId,
            Customer = @Customer
        WHERE Number = @Number;
    END
END
GO

-- Dar de baja factura
CREATE PROCEDURE SP_DEACTIVATE_INVOICE
    @Number INT
AS
BEGIN
    UPDATE Invoices
    SET IsActive = 0
    WHERE Number = @Number;
END
GO

-- Insertar factura con OUTPUT del ID
CREATE PROCEDURE SP_INSERT_INVOICE
    @InvoiceDate DATE,
    @PaymentMethodId INT,
    @Customer VARCHAR(100),
    @Number INT OUTPUT
AS
BEGIN
    INSERT INTO Invoices (InvoiceDate, PaymentMethodId, Customer)
    VALUES (@InvoiceDate, @PaymentMethodId, @Customer);

    SET @Number = SCOPE_IDENTITY();
END
GO

-- Recuperar factura por número
CREATE PROCEDURE SP_GET_INVOICE_BY_NUMBER
    @Number INT
AS
BEGIN
    SELECT i.*, d.Id AS DetailId, d.ArticleId, d.Quantity, a.Name AS ArticleName, a.UnitPrice
    FROM Invoices i
    LEFT JOIN InvoiceDetails d ON d.InvoiceNumber = i.Number
    LEFT JOIN Articles a ON a.Id = d.ArticleId
    WHERE i.Number = @Number;
END
GO

-- Recuperar todas las facturas
CREATE PROCEDURE SP_GET_INVOICES
AS
BEGIN
    SELECT DISTINCT i.*, d.Id AS DetailId, d.ArticleId, d.Quantity, a.Name AS ArticleName, a.UnitPrice
    FROM Invoices i
    INNER JOIN InvoiceDetails d ON d.InvoiceNumber = i.Number
    INNER JOIN Articles a ON a.Id = d.ArticleId
    ORDER BY i.Number;
END
GO


--ARTICULOS

-- Guardar artículo (insert/update)
CREATE PROCEDURE SP_SAVE_ARTICLE
    @Id INT,
    @Name VARCHAR(100),
    @UnitPrice DECIMAL(10,2)
AS
BEGIN
    IF @Id = 0
    BEGIN
        INSERT INTO Articles (Name, UnitPrice, IsActive)
        VALUES (@Name, @UnitPrice, 1);
    END
    ELSE
    BEGIN
        UPDATE Articles
        SET Name = @Name, UnitPrice = @UnitPrice
        WHERE Id = @Id;
    END
END
GO

-- Recuperar artículo por ID
CREATE PROCEDURE SP_GET_ARTICLE_BY_ID
    @Id INT
AS
BEGIN
    SELECT * FROM Articles
    WHERE Id = @Id AND IsActive = 1;
END
GO

-- Recuperar todos los artículos
CREATE PROCEDURE SP_GET_ARTICLES
AS
BEGIN
    SELECT * FROM Articles
    WHERE IsActive = 1;
END
GO

-- Dar de baja artículo
CREATE PROCEDURE SP_DEACTIVATE_ARTICLE
    @Id INT
AS
BEGIN
    UPDATE Articles
    SET IsActive = 0
    WHERE Id = @Id;
END
GO

--DETALLES FACTURAS

-- Recuperar todos los detalles de facturas
CREATE PROCEDURE SP_GET_INVOICEDETAILS
AS
BEGIN
SELECT *
    FROM InvoiceDetails 
	ORDER BY InvoiceNumber, Id;
END
GO

-- Recuperar detalle de factura por id
CREATE PROCEDURE SP_GET_INVOICE_DETAIL_BY_ID
    @Id INT
AS
BEGIN
    SELECT 
        d.Id,
        d.ArticleId,
        a.Name AS ArticleName,
        a.UnitPrice,
        d.Quantity,
        i.Number AS InvoiceNumber,
        i.InvoiceDate,
        i.Customer,
        i.PaymentMethodId
    FROM InvoiceDetails d
    INNER JOIN Articles a ON a.Id = d.ArticleId
    INNER JOIN Invoices i ON i.Number = d.InvoiceNumber
    WHERE d.Id = @Id;
END
GO

CREATE PROCEDURE SP_ADD_OR_UPDATE_INVOICE_DETAIL
    @InvoiceNumber INT,
    @ArticleId INT,
    @Quantity INT
AS
BEGIN
    IF EXISTS (
        SELECT 1 FROM InvoiceDetails
        WHERE InvoiceNumber = @InvoiceNumber AND ArticleId = @ArticleId
    )
    BEGIN
        UPDATE InvoiceDetails
        SET Quantity = Quantity + @Quantity
        WHERE InvoiceNumber = @InvoiceNumber AND ArticleId = @ArticleId;
    END
    ELSE
    BEGIN
        INSERT INTO InvoiceDetails (InvoiceNumber, ArticleId, Quantity)
        VALUES (@InvoiceNumber, @ArticleId, @Quantity);
    END
END
GO