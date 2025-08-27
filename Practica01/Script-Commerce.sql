CREATE DATABASE commerce;
GO

USE commerce;
GO


CREATE TABLE paymentMethods (
    id INT IDENTITY(1,1),
    name VARCHAR(50),
    CONSTRAINT pk_paymentMethods PRIMARY KEY(id)
);
GO

CREATE TABLE articles (
    id INT IDENTITY(1,1),
    name VARCHAR(100),
    unitPrice DECIMAL(10,2),
    CONSTRAINT pk_articles PRIMARY KEY(id)
);
GO

CREATE TABLE invoices (
    number INT IDENTITY(1,1),
    date DATE,
    paymentMethodId INT,
    customer VARCHAR(100),
    CONSTRAINT pk_invoices PRIMARY KEY(number),
    CONSTRAINT fk_invoices_paymentMethods FOREIGN KEY (paymentMethodId)
        REFERENCES paymentMethods(id)
);
GO

CREATE TABLE invoiceDetails (
    id INT IDENTITY(1,1),
    articleId INT,
    quantity INT,
    invoiceNumber INT,
    CONSTRAINT pk_invoiceDetails PRIMARY KEY(id),
    CONSTRAINT fk_invoiceDetails_articles FOREIGN KEY (articleId)
        REFERENCES articles(id),
    CONSTRAINT fk_invoiceDetails_invoices FOREIGN KEY (invoiceNumber)
        REFERENCES invoices(number)
);
GO

-- STORED PROCEDURES

-- Get Article by Name
CREATE PROCEDURE [dbo].[spGetArticle]
@name VARCHAR(100)
AS
BEGIN
    SELECT * 
    FROM articles
    WHERE name = @name;
END
GO

-- Insert Article
CREATE PROCEDURE [dbo].[spInsertArticle]
@name VARCHAR(100),
@unitPrice DECIMAL(10,2),
@newId INT OUTPUT
AS
BEGIN
    INSERT INTO articles(name, unitPrice)
    VALUES(@name, @unitPrice);

    SET @newId = SCOPE_IDENTITY();
END
GO

-- Update Article Price
CREATE PROCEDURE [dbo].[spUpdateArticlePrice]
@id INT,
@newPrice DECIMAL(10,2)
AS
BEGIN
    UPDATE articles
    SET unitPrice = @newPrice
    WHERE id = @id;
END
GO

-- Get Sales by Article
CREATE PROCEDURE [dbo].[spGetSalesByArticle]
@articleId INT
AS
BEGIN
    SELECT a.name, 
           SUM(d.quantity) AS totalSold,
           SUM(d.quantity * a.unitPrice) AS totalBilled
    FROM invoiceDetails d
    INNER JOIN articles a ON d.articleId = a.id
    WHERE a.id = @articleId
    GROUP BY a.name;
END
GO

-- Get Invoice with Details
CREATE PROCEDURE [dbo].[spGetInvoice]
@number INT
AS
BEGIN
    SELECT i.number, i.date, i.customer, pm.name AS paymentMethod,
           d.quantity, a.name AS article, a.unitPrice,
           (d.quantity * a.unitPrice) AS subtotal
    FROM invoices i
    INNER JOIN paymentMethods pm ON i.paymentMethodId = pm.id
    INNER JOIN invoiceDetails d ON i.number = d.invoiceNumber
    INNER JOIN articles a ON d.articleId = a.id
    WHERE i.number = @number;
END
GO

-- Insert Invoice
CREATE PROCEDURE [dbo].[spInsertInvoice]
@date DATE,
@paymentMethodId INT,
@customer VARCHAR(100),
@newNumber INT OUTPUT
AS
BEGIN
    INSERT INTO invoices(date, paymentMethodId, customer)
    VALUES(@date, @paymentMethodId, @customer);

    SET @newNumber = SCOPE_IDENTITY();
END
GO

CREATE PROCEDURE [dbo].[spInsertInvoiceDetail]
@articleId INT,
@quantity INT,
@invoiceNumber INT,
@newId INT OUTPUT
AS
BEGIN
    INSERT INTO invoiceDetails(articleId, quantity, invoiceNumber)
    VALUES(@articleId, @quantity, @invoiceNumber);

    SET @newId = SCOPE_IDENTITY();
END
GO


-- Payment Methods
INSERT INTO paymentMethods(name) VALUES ('Cash');
INSERT INTO paymentMethods(name) VALUES ('Credit Card');
INSERT INTO paymentMethods(name) VALUES ('Debit Card');

-- Articles
INSERT INTO articles(name, unitPrice) VALUES ('Laptop', 1200.50);
INSERT INTO articles(name, unitPrice) VALUES ('Mouse', 25.00);
INSERT INTO articles(name, unitPrice) VALUES ('Keyboard', 45.00);
INSERT INTO articles(name, unitPrice) VALUES ('Monitor', 300.00);

-- Invoices
INSERT INTO invoices(date, paymentMethodId, customer) VALUES ('2025-08-25', 1, 'John Doe');
INSERT INTO invoices(date, paymentMethodId, customer) VALUES ('2025-08-26', 2, 'Jane Smith');

-- Invoice Details
INSERT INTO invoiceDetails(articleId, quantity, invoiceNumber) VALUES (1, 1, 1); -- Laptop on Invoice 1
INSERT INTO invoiceDetails(articleId, quantity, invoiceNumber) VALUES (2, 2, 1); -- 2 Mouse on Invoice 1
INSERT INTO invoiceDetails(articleId, quantity, invoiceNumber) VALUES (3, 1, 2); -- Keyboard on Invoice 2
INSERT INTO invoiceDetails(articleId, quantity, invoiceNumber) VALUES (4, 2, 2); -- 2 Monitors on Invoice 2