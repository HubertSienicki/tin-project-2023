CREATE TABLE Roles (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL
);

CREATE TABLE Users (
    id INT AUTO_INCREMENT PRIMARY KEY,
    username VARCHAR(255) NOT NULL,
    password VARCHAR(255) NOT NULL,
    email VARCHAR(255) NOT NULL,
    role_id INT,
    FOREIGN KEY (role_id) REFERENCES Roles(id)
);

CREATE TABLE Products (
    id INT AUTO_INCREMENT PRIMARY KEY,
    name VARCHAR(255) NOT NULL,
    price DECIMAL(10, 2) NOT NULL
);

CREATE TABLE Orders (
    id INT AUTO_INCREMENT PRIMARY KEY,
    user_id INT,
    order_date DATE NOT NULL,
    FOREIGN KEY (user_id) REFERENCES Users(id)
);

CREATE TABLE OrderDetails (
    order_id INT,
    product_id INT,
    quantity INT,
    additional_column VARCHAR(255),
    PRIMARY KEY (order_id, product_id),
    FOREIGN KEY (order_id) REFERENCES Orders(id),
    FOREIGN KEY (product_id) REFERENCES Products(id)
);

INSERT INTO Roles (name) VALUES ('Admin'), ('User'), ('Guest');

INSERT INTO Users (username, password, email, role_id) VALUES 
('user1', 'pass1', 'user1@example.com', 2),
('user2', 'pass2', 'user2@example.com', 2),
('admin', 'adminpass', 'admin@example.com', 1);

INSERT INTO Products (name, price) VALUES 
('Product 1', 10.00),
('Product 2', 15.50),
('Product 3', 7.25);

INSERT INTO Orders (user_id, order_date) VALUES 
(1, '2024-01-01'),
(2, '2024-01-02');

INSERT INTO OrderDetails (order_id, product_id, quantity, additional_column) VALUES 
(1, 1, 2, 'Extra details 1'),
(1, 2, 1, 'Extra details 2'),
(2, 3, 3, 'Extra details 3');