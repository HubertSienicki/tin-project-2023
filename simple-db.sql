create table Clients
(
    Id         int auto_increment
        primary key,
    first_name varchar(50) null,
    last_name  varchar(50) null,
    phone      varchar(9)  null,
    email      varchar(50) null
);

create table Orders
(
    id         int auto_increment
        primary key,
    client_id  int  null,
    order_date date not null,
    constraint Orders_Clients_Id_fk
        foreign key (client_id) references Clients (Id)
);

create table Products
(
    id    int auto_increment
        primary key,
    name  varchar(255)   not null,
    price decimal(10, 2) not null
);

create table OrderDetails
(
    order_id          int          not null,
    product_id        int          not null,
    quantity          int          null,
    additional_column varchar(255) null,
    primary key (order_id, product_id),
    constraint OrderDetails_ibfk_1
        foreign key (order_id) references Orders (id),
    constraint OrderDetails_ibfk_2
        foreign key (product_id) references Products (id)
);

create index product_id
    on OrderDetails (product_id);

create table Roles
(
    roleId int auto_increment
        primary key,
    name   varchar(255) not null
);

create table Users
(
    id            int auto_increment
        primary key,
    username      varchar(255) not null,
    password      varchar(255) not null,
    email         varchar(255) not null,
    role_id       int          null,
    password_salt varchar(255) not null,
    constraint Users_ibfk_1
        foreign key (role_id) references Roles (roleId)
);

create index role_id
    on Users (role_id);



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