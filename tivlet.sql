CREATE DATABASE TicketingSystem;
GO

USE TicketingSystem;
GO
CREATE TABLE Destinations (
    DestinationID INT IDENTITY(1,1) PRIMARY KEY,
    StationName NVARCHAR(100) NOT NULL,
    Zone INT NOT NULL,
    BaseFare DECIMAL(18,2) NOT NULL
);
GO

CREATE TABLE Tickets (
    TicketID INT IDENTITY(1,1) PRIMARY KEY,
    DestinationID INT NOT NULL,
    IsStudentTicket BIT NOT NULL DEFAULT 0, 
    FinalPrice DECIMAL(18,2) NOT NULL,
    BarcodeData NVARCHAR(255) NOT NULL UNIQUE,
    IssueDate DATETIME NOT NULL DEFAULT GETDATE(),
    
    CONSTRAINT FK_Tickets_Destinations FOREIGN KEY (DestinationID) 
        REFERENCES Destinations(DestinationID)
);
GO

CREATE TABLE Transactions (
    TransactionID INT IDENTITY(1,1) PRIMARY KEY,
    TicketID INT NOT NULL,
    Amount DECIMAL(18,2) NOT NULL,
    PaymentMethod NVARCHAR(50) NOT NULL, 
    Status NVARCHAR(50) NOT NULL,        
    TransactionDate DATETIME NOT NULL DEFAULT GETDATE(),

    CONSTRAINT FK_Transactions_Tickets FOREIGN KEY (TicketID) 
        REFERENCES Tickets(TicketID)
);
GO

INSERT INTO Destinations (StationName, Zone, BaseFare)
VALUES 
    ('National University Station', 1, 10000.00),
    ('Thao Dien Station', 1, 15000.00),
    ('Ba Son Station', 1, 15000.00),
    ('Phuoc Long Station', 1, 15000.00),
    ('High Tech Park Station', 2, 20000.00),
    ('Suoi Tien Station', 3, 25000.00);
GO