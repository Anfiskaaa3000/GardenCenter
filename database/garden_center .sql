CREATE TABLE roles (
    role_id int IDENTITY(1,1),
    role_name nvarchar(50) NOT NULL,
    CONSTRAINT pk_roles PRIMARY KEY (role_id)
);

CREATE TABLE users (
    user_id int IDENTITY(1,1),
    login nvarchar(50) NOT NULL,
    last_name nvarchar(60) NOT NULL,
    first_name nvarchar(60) NOT NULL,
    middle_name nvarchar(60),
    role_id int NOT NULL,
    CONSTRAINT pk_users PRIMARY KEY (user_id),
    CONSTRAINT fk_users_role FOREIGN KEY (role_id) REFERENCES roles (role_id)
);

CREATE TABLE categories (
    category_id int IDENTITY(1,1),
    category_name nvarchar(100) NOT NULL,
    CONSTRAINT pk_categories PRIMARY KEY (category_id)
);

CREATE TABLE products (
    product_id int IDENTITY(1,1),
    product_code nvarchar(20) NOT NULL,
    product_name nvarchar(150) NOT NULL,
    category_id int NOT NULL,
    description nvarchar(max) NULL,
    image_path nvarchar(150) NULL,
    base_price decimal(10, 2) NOT NULL,
    CONSTRAINT pk_products PRIMARY KEY (product_id),
    CONSTRAINT fk_products_category FOREIGN KEY (category_id) REFERENCES categories (category_id)
);

CREATE TABLE product_variants (
    variant_id int IDENTITY(1,1),
    product_id int NOT NULL,
    variant_name nvarchar(30) NOT NULL,
    available_quantity int NOT NULL DEFAULT (0),
    CONSTRAINT pk_product_variants PRIMARY KEY (variant_id),
    CONSTRAINT chk_variants_quantity CHECK (available_quantity >= 0),
    CONSTRAINT fk_variants_product FOREIGN KEY (product_id) REFERENCES products (product_id) ON DELETE CASCADE
);

CREATE TABLE orders (
    order_id int IDENTITY(1,1),
    order_date date NOT NULL DEFAULT (CAST(GETDATE() AS date)),
    client_id int NOT NULL,
    CONSTRAINT pk_orders PRIMARY KEY (order_id),
    CONSTRAINT fk_orders_client FOREIGN KEY (client_id) REFERENCES users (user_id)
);

CREATE TABLE order_items (
    order_item_id int IDENTITY(1,1),
    order_id int NOT NULL,
    variant_id int NOT NULL,
    quantity int NOT NULL,
    unit_price decimal(10, 2) NOT NULL,
    CONSTRAINT pk_order_items PRIMARY KEY (order_item_id),
    CONSTRAINT chk_items_quantity CHECK (quantity > 0),
    CONSTRAINT fk_items_order FOREIGN KEY (order_id) REFERENCES orders (order_id) ON DELETE CASCADE,
    CONSTRAINT fk_items_variant FOREIGN KEY (variant_id) REFERENCES product_variants (variant_id)
);


INSERT INTO roles (role_name) VALUES 
(N'Гость'), 
(N'Клиент'), 
(N'Менеджер'), 
(N'Администратор');

INSERT INTO categories (category_name) VALUES 
(N'Саженцы'), 
(N'Семена'), 
(N'Инвентарь'), 
(N'Удобрения');

INSERT INTO users (login, last_name, first_name, middle_name, role_id) VALUES 
(N'client_ivan', N'Иванов', N'Иван', N'Иванович', 2),
(N'client_anna', N'Смирнова', N'Анна', N'Сергеевна', 2),
(N'manager_petr', N'Петров', N'Петр', N'Алексеевич', 3),
(N'admin_sys', N'Сидоров', N'Алексей', N'Владимирович', 4);

-- category_id = 1 (Саженцы), category_id = 4 (Удобрения)
INSERT INTO products (product_code, product_name, category_id, base_price, image_path) VALUES 
(N'PL-001', N'Саженец Яблони', 1, 1200.00, N'apple.png'),
(N'PL-002', N'Саженец Груши', 1, 1500.00, N'pear.png'),
(N'FR-001', N'Удобрение для роз', 4, 450.00, N'rose_fert.png'),
(N'FR-002', N'Грунт универсальный', 4, 800.00, NULL);

-- product_id = 1 (Яблоня), product_id = 3 (Удобрение)
INSERT INTO product_variants (product_id, variant_name, available_quantity) VALUES 
(1, N'Высота 1 метр', 15),
(1, N'Высота 1.5 метра', 8),
(2, N'Высота 1 метр', 10),
(3, N'Фасовка 1 кг', 50),
(3, N'Фасовка 5 кг', 20),
(4, N'Мешок 50 литров', 30);

-- Заполнение тестового заказа (клиент Иванов, order_id будет 1)
INSERT INTO orders (order_date, client_id) VALUES 
('2026-10-20', 1);

-- Заполнение позиций заказа (Иванов купил 2 яблони по 1200 и 1 удобрение по 450)
-- variant_id = 1 (Яблоня 1м), variant_id = 4 (Удобрение 1кг)
INSERT INTO order_items (order_id, variant_id, quantity, unit_price) VALUES 
(1, 1, 2, 1200.00),
(1, 4, 1, 450.00);
USE garden_center;
SELECT product_name, image_path FROM products;