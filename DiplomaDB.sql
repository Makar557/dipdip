DROP DATABASE DiplomaDB;
GO

CREATE DATABASE DiplomaDB;
GO

USE DiplomaDB;
GO
ALTER TABLE Заказы DROP CONSTRAINT CK__Заказы__статус__5AEE82B9;

ALTER TABLE Заказы ADD CONSTRAINT CK__Заказы__статус__5AEE82B9
CHECK (статус IN ('ожидает', 'приготовлен', 'получен курьером', 'в процессе', 'доставлен', 'получен заказчиком', 'отменен', 'оплачено'));

ALTER TABLE Заказы ALTER COLUMN квартира NVARCHAR(10) NOT NULL;
ALTER TABLE Пользователи ALTER COLUMN телефон NVARCHAR(20) NOT NULL;

ALTER TABLE Пользователи ADD баланс DECIMAL(10,2) NOT NULL DEFAULT 0;
ALTER TABLE Пользователи ALTER COLUMN Баланс DECIMAL(10,2);

CREATE TABLE Роли (
    id INT IDENTITY(1,1) PRIMARY KEY,
    роль NVARCHAR(20) UNIQUE NOT NULL
);

CREATE TABLE Пользователи (
    id INT IDENTITY(1,1) PRIMARY KEY,
    имя NVARCHAR(50) NOT NULL,
    фамилия NVARCHAR(50) NOT NULL,
    электронная_почта NVARCHAR(100) UNIQUE NOT NULL,
    логин NVARCHAR(50) UNIQUE NOT NULL,
    хэш_пароля NVARCHAR(MAX) NOT NULL,
    роли_id INT NOT NULL FOREIGN KEY REFERENCES Роли(id),
    дата_регистрации DATETIME DEFAULT GETDATE(),
	кредитная_карта NVARCHAR(MAX) NULL
);

CREATE TABLE Рестораны (
    id INT IDENTITY(1,1) PRIMARY KEY,
    пользователь_id INT NOT NULL FOREIGN KEY REFERENCES Пользователи(id),
    название NVARCHAR(100) NOT NULL,
    описание NVARCHAR(MAX),
    адрес NVARCHAR(255) NOT NULL,
    телефон NVARCHAR(15) NOT NULL,
    рейтинг DECIMAL(3, 2) DEFAULT 0 CHECK (рейтинг BETWEEN 0 AND 5),
    начало_работы TIME NOT NULL,
    количество_часов_в_день INT NOT NULL CHECK (количество_часов_в_день BETWEEN 1 AND 24),
	логотип NVARCHAR(MAX) NULL
);

CREATE TABLE Меню (
    id INT IDENTITY(1,1) PRIMARY KEY,
    ресторан_id INT NOT NULL FOREIGN KEY REFERENCES Рестораны(id),
    название NVARCHAR(100) NOT NULL,
    описание NVARCHAR(MAX),
    цена DECIMAL(10, 2) NOT NULL CHECK (цена >= 0),
    ссылка_на_изображение NVARCHAR(MAX),
    доступность BIT NOT NULL DEFAULT 1
);

CREATE TABLE Корзины (
    id INT IDENTITY(1,1) PRIMARY KEY,
    пользователь_id INT NOT NULL FOREIGN KEY REFERENCES Пользователи(id),
    статус NVARCHAR(20) NOT NULL CHECK (статус IN ('активная', 'оформлена')),
    дата_создания DATETIME DEFAULT GETDATE(),
    итоговая_стоимость DECIMAL(10, 2) DEFAULT 0 CHECK (итоговая_стоимость >= 0)
);

CREATE TABLE Состав_корзины (
    id INT IDENTITY(1,1) PRIMARY KEY,
    корзина_id INT NOT NULL FOREIGN KEY REFERENCES Корзины(id),
    блюдо_id INT NOT NULL FOREIGN KEY REFERENCES Меню(id),
    количество INT NOT NULL CHECK (количество > 0),
    цена DECIMAL(10, 2) NOT NULL CHECK (цена >= 0)
);

CREATE TABLE Заказы (
    id INT IDENTITY(1,1) PRIMARY KEY,
    клиент_id INT NOT NULL FOREIGN KEY REFERENCES Пользователи(id),
    ресторан_id INT NOT NULL FOREIGN KEY REFERENCES Рестораны(id),
    курьер_id INT FOREIGN KEY REFERENCES Пользователи(id),
    адрес_доставки NVARCHAR(255) NOT NULL,
    итоговая_стоимость DECIMAL(10, 2) NOT NULL CHECK (итоговая_стоимость >= 0),
    статус NVARCHAR(20) NOT NULL CHECK (статус IN ('ожидает', 'приготовлен', 'получен курьером', 'в процессе', 'доставлен', 'получен заказчиком', 'отменен', 'оплачено')), -- в процессе готовки
    способ_оплаты NVARCHAR(20) NOT NULL,
    дата_создания DATETIME DEFAULT GETDATE(),
    дата_доставки DATETIME
);

CREATE TABLE Оценки (
    id INT IDENTITY(1,1) PRIMARY KEY,
    пользователь_id INT NOT NULL FOREIGN KEY REFERENCES Пользователи(id),
    ресторан_id INT NOT NULL FOREIGN KEY REFERENCES Рестораны(id),
    оценка INT NOT NULL CHECK (оценка BETWEEN 1 AND 5),
    UNIQUE (пользователь_id, ресторан_id)
);

CREATE TABLE Состав_заказа (
    id INT IDENTITY(1,1) PRIMARY KEY,
    заказ_id INT NOT NULL FOREIGN KEY REFERENCES Заказы(id) ON DELETE CASCADE,
    блюдо_id INT NOT NULL FOREIGN KEY REFERENCES Меню(id),
    количество INT NOT NULL CHECK (количество > 0),
    цена DECIMAL(10,2) NOT NULL CHECK (цена >= 0)
);

CREATE TABLE Скидки (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    МенюId INT NOT NULL,             
    ПроцентСкидки DECIMAL(5,2) NOT NULL CHECK (ПроцентСкидки BETWEEN 1 AND 100),  
    ДатаНачала DATE NOT NULL,         
    ДатаОкончания DATE NOT NULL,       
    FOREIGN KEY (МенюId) REFERENCES Меню(Id) ON DELETE CASCADE
);

INSERT INTO Роли (роль) VALUES
('Клиент'),
('Курьер'),
('Ресторан'),
('Админ');