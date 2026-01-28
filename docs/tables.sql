CREATE TABLE IF NOT EXISTS Customer (
    Id TEXT PRIMARY KEY,
    Name TEXT NOT NULL,
    Email TEXT NOT NULL,
    Phone TEXT NOT NULL,
    Address TEXT NOT NULL,
    IsActive INTEGER NOT NULL,
    IsDeleted INTEGER NOT NULL,
    CreationTime TEXT NOT NULL,
    UpdateTime TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS Product (
    Id TEXT PRIMARY KEY,
    Name TEXT NOT NULL,
    PricePerUnit TEXT NOT NULL,
    Weight REAL NOT NULL,
    IsAvailable INTEGER NOT NULL,
    IsDeleted INTEGER NOT NULL,
    CreationTime TEXT NOT NULL,
    UpdateTime TEXT NOT NULL
);

CREATE TABLE IF NOT EXISTS "Order" (
    Id TEXT PRIMARY KEY,
    CustomerId TEXT NOT NULL,
    Status TEXT NOT NULL,
    IsDeleted INTEGER NOT NULL,
    CreationTime TEXT NOT NULL,
    UpdateTime TEXT NOT NULL,
    CONSTRAINT FK_Order_Customer FOREIGN KEY (CustomerId) REFERENCES Customer (Id) ON DELETE RESTRICT
);

CREATE TABLE IF NOT EXISTS OrderItem (
    Id TEXT PRIMARY KEY,
    OrderId TEXT NOT NULL,
    ProductId TEXT NOT NULL,
    Quantity INTEGER NOT NULL,
    IsDeleted INTEGER NOT NULL,
    CreationTime TEXT NOT NULL,
    UpdateTime TEXT NOT NULL,
    CONSTRAINT FK_OrderItem_Order FOREIGN KEY (OrderId) REFERENCES "Order" (Id) ON DELETE CASCADE,
    CONSTRAINT FK_OrderItem_Product FOREIGN KEY (ProductId) REFERENCES Product (Id) ON DELETE RESTRICT,
    CONSTRAINT UQ_OrderItem_Order_Product UNIQUE (OrderId, ProductId)
);
