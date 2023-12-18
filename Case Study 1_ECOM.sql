CREATE DATABASE ECOM;

USE ECOM;

CREATE TABLE customers (
    customer_id INT PRIMARY KEY,
    name VARCHAR(25),
    email VARCHAR(25),
    password VARCHAR(25)
);

CREATE TABLE products (
    product_id INT PRIMARY KEY,
    name VARCHAR(25),
    price DECIMAL(10, 2),
    description VARCHAR(100),
    stockQuantity INT
);

CREATE TABLE cart (
    cart_id INT PRIMARY KEY,
    customer_id INT,
    product_id INT,
    quantity INT,
    FOREIGN KEY (customer_id) REFERENCES customers(customer_id),
    FOREIGN KEY (product_id) REFERENCES products(product_id)
);

CREATE TABLE orders (
    order_id INT PRIMARY KEY,
    customer_id INT,
    order_date DATE,
    total_price DECIMAL(10, 2),
    shipping_address VARCHAR(100),
    FOREIGN KEY (customer_id) REFERENCES customers(customer_id)
);

CREATE TABLE order_items (
    order_item_id INT PRIMARY KEY,
    order_id INT,
    product_id INT,
    quantity_supplied INT,
    FOREIGN KEY (order_id) REFERENCES orders(order_id),
    FOREIGN KEY (product_id) REFERENCES products(product_id)
);

------------------------------------------------------------------------------------------------------------

INSERT INTO customers (customer_id, name, email, password)
VALUES 
(1, 'Ravi Kumar', 'ravi.kumar@example.com', 'password1'),
(2, 'Sunita Sharma', 'sunita.sharma@example.com', 'password2'),
(3, 'Amit Patel', 'amit.patel@example.com', 'password3'),
(4, 'Priya Singh', 'priya.singh@example.com', 'password4'),
(5, 'Vijay Verma', 'vijay.verma@example.com', 'password5'),
(6, 'Anita Yadav', 'anita.yadav@example.com', 'password6'),
(7, 'Rajesh Gupta', 'rajesh.gupta@example.com', 'password7'),
(8, 'Pooja Rana', 'pooja.rana@example.com', 'password8'),
(9, 'Sanjay Thakur', 'sanjay.thakur@example.com', 'password9'),
(10, 'Neha Khatri', 'neha.khatri@example.com', 'password10');


INSERT INTO products (product_id, name, price, description, stockQuantity)
VALUES 
(1, 'Samsung Galaxy S21', 70000.00, 'Electronics product', 10),
(2, 'HRX Jeans', 3000.00, 'Clothing product', 20),
(3, 'Prestige Pressure Cooker', 2000.00, 'Home & Kitchen product', 30),
(4, 'Apple MacBook Pro', 120000.00, 'Electronics product', 5),
(5, 'Raymond Suit', 8000.00, 'Clothing & Accessories product', 7),
(6, 'Hawkins Pressure Cooker', 1500.00, 'Home & Kitchen product', 15),
(7, 'Dell Inspiron Laptop', 50000.00, 'Electronics product', 20),
(8, 'Peter England Shirt', 2000.00, 'Clothing & Accessories product', 25),
(9, 'Butterfly Gas Stove', 3000.00, 'Home & Kitchen product', 30),
(10, 'HP Pavilion Laptop', 60000.00, 'Electronics product', 10);


INSERT INTO cart (cart_id, customer_id, product_id, quantity)
VALUES 
(1, 1, 1, 1),
(2, 2, 2, 2),
(3, 3, 3, 1),
(4, 4, 4, 1),
(5, 5, 5, 2),
(6, 6, 6, 3),
(7, 7, 7, 2),
(8, 8, 8, 4),
(9, 9, 9, 2),
(10, 10, 10, 3);

INSERT INTO orders (order_id, customer_id, order_date, total_price, shipping_address)
VALUES 
(1, 1, '2023-12-01', 70000.00, 'Mumbai'),
(2, 2, '2023-12-02', 6000.00, 'Kolkata'),
(3, 3, '2023-12-03', 2000.00, 'Bangalore'),
(4, 4, '2023-12-04', 120000.00, 'Chennai'),
(5, 5, '2023-12-05', 16000.00, 'Hyderabad'),
(6, 6, '2023-12-06', 4500.00, 'Pune'),
(7, 7, '2023-12-07', 100000.00, 'Ahmedabad'),
(8, 8, '2023-12-08', 8000.00, 'Kolkata'),
(9, 9, '2023-12-09', 6000.00, 'Mumbai'),
(10, 10, '2023-12-10', 180000.00, 'Kolkata');

INSERT INTO order_items (order_item_id, order_id, product_id, quantity_supplied)
VALUES 
(1, 1, 1, 1),
(2, 2, 2, 2),
(3, 3, 3, 1),
(4, 4, 4, 1),
(5, 5, 5, 2),
(6, 6, 6, 3),
(7, 7, 7, 2),
(8, 8, 8, 4),
(9, 9, 9, 2),
(10, 10, 10, 3);

--------------------------------------------------------------------------------

SELECT * FROM customers;
SELECT * FROM products;
SELECT * FROM cart;
SELECT * FROM orders;
SELECT * FROM order_items;

--total price of each order
SELECT order_id, SUM(price * quantity_supplied) AS total_price
FROM order_items o
JOIN products p ON o.product_id = p.product_id
GROUP BY order_id;

SELECT * FROM products where product_id IN (Select product_id from cart where customer_id = 1)
 