DROP DATABASE DiplomaDB;
GO

CREATE DATABASE DiplomaDB;
GO

USE DiplomaDB;
GO
ALTER TABLE ������ DROP CONSTRAINT CK__������__������__5AEE82B9;

ALTER TABLE ������ ADD CONSTRAINT CK__������__������__5AEE82B9
CHECK (������ IN ('�������', '�����������', '������� ��������', '� ��������', '���������', '������� ����������', '�������', '��������'));

ALTER TABLE ������ ALTER COLUMN �������� NVARCHAR(10) NOT NULL;
ALTER TABLE ������������ ALTER COLUMN ������� NVARCHAR(20) NOT NULL;

ALTER TABLE ������������ ADD ������ DECIMAL(10,2) NOT NULL DEFAULT 0;
ALTER TABLE ������������ ALTER COLUMN ������ DECIMAL(10,2);

CREATE TABLE ���� (
    id INT IDENTITY(1,1) PRIMARY KEY,
    ���� NVARCHAR(20) UNIQUE NOT NULL
);

CREATE TABLE ������������ (
    id INT IDENTITY(1,1) PRIMARY KEY,
    ��� NVARCHAR(50) NOT NULL,
    ������� NVARCHAR(50) NOT NULL,
    �����������_����� NVARCHAR(100) UNIQUE NOT NULL,
    ����� NVARCHAR(50) UNIQUE NOT NULL,
    ���_������ NVARCHAR(MAX) NOT NULL,
    ����_id INT NOT NULL FOREIGN KEY REFERENCES ����(id),
    ����_����������� DATETIME DEFAULT GETDATE(),
	���������_����� NVARCHAR(MAX) NULL
);

CREATE TABLE ��������� (
    id INT IDENTITY(1,1) PRIMARY KEY,
    ������������_id INT NOT NULL FOREIGN KEY REFERENCES ������������(id),
    �������� NVARCHAR(100) NOT NULL,
    �������� NVARCHAR(MAX),
    ����� NVARCHAR(255) NOT NULL,
    ������� NVARCHAR(15) NOT NULL,
    ������� DECIMAL(3, 2) DEFAULT 0 CHECK (������� BETWEEN 0 AND 5),
    ������_������ TIME NOT NULL,
    ����������_�����_�_���� INT NOT NULL CHECK (����������_�����_�_���� BETWEEN 1 AND 24),
	������� NVARCHAR(MAX) NULL
);

CREATE TABLE ���� (
    id INT IDENTITY(1,1) PRIMARY KEY,
    ��������_id INT NOT NULL FOREIGN KEY REFERENCES ���������(id),
    �������� NVARCHAR(100) NOT NULL,
    �������� NVARCHAR(MAX),
    ���� DECIMAL(10, 2) NOT NULL CHECK (���� >= 0),
    ������_��_����������� NVARCHAR(MAX),
    ����������� BIT NOT NULL DEFAULT 1
);

CREATE TABLE ������� (
    id INT IDENTITY(1,1) PRIMARY KEY,
    ������������_id INT NOT NULL FOREIGN KEY REFERENCES ������������(id),
    ������ NVARCHAR(20) NOT NULL CHECK (������ IN ('��������', '���������')),
    ����_�������� DATETIME DEFAULT GETDATE(),
    ��������_��������� DECIMAL(10, 2) DEFAULT 0 CHECK (��������_��������� >= 0)
);

CREATE TABLE ������_������� (
    id INT IDENTITY(1,1) PRIMARY KEY,
    �������_id INT NOT NULL FOREIGN KEY REFERENCES �������(id),
    �����_id INT NOT NULL FOREIGN KEY REFERENCES ����(id),
    ���������� INT NOT NULL CHECK (���������� > 0),
    ���� DECIMAL(10, 2) NOT NULL CHECK (���� >= 0)
);

CREATE TABLE ������ (
    id INT IDENTITY(1,1) PRIMARY KEY,
    ������_id INT NOT NULL FOREIGN KEY REFERENCES ������������(id),
    ��������_id INT NOT NULL FOREIGN KEY REFERENCES ���������(id),
    ������_id INT FOREIGN KEY REFERENCES ������������(id),
    �����_�������� NVARCHAR(255) NOT NULL,
    ��������_��������� DECIMAL(10, 2) NOT NULL CHECK (��������_��������� >= 0),
    ������ NVARCHAR(20) NOT NULL CHECK (������ IN ('�������', '�����������', '������� ��������', '� ��������', '���������', '������� ����������', '�������', '��������')), -- � �������� �������
    ������_������ NVARCHAR(20) NOT NULL,
    ����_�������� DATETIME DEFAULT GETDATE(),
    ����_�������� DATETIME
);

CREATE TABLE ������ (
    id INT IDENTITY(1,1) PRIMARY KEY,
    ������������_id INT NOT NULL FOREIGN KEY REFERENCES ������������(id),
    ��������_id INT NOT NULL FOREIGN KEY REFERENCES ���������(id),
    ������ INT NOT NULL CHECK (������ BETWEEN 1 AND 5),
    UNIQUE (������������_id, ��������_id)
);

CREATE TABLE ������_������ (
    id INT IDENTITY(1,1) PRIMARY KEY,
    �����_id INT NOT NULL FOREIGN KEY REFERENCES ������(id) ON DELETE CASCADE,
    �����_id INT NOT NULL FOREIGN KEY REFERENCES ����(id),
    ���������� INT NOT NULL CHECK (���������� > 0),
    ���� DECIMAL(10,2) NOT NULL CHECK (���� >= 0)
);

CREATE TABLE ������ (
    Id INT PRIMARY KEY IDENTITY(1,1), 
    ����Id INT NOT NULL,             
    ������������� DECIMAL(5,2) NOT NULL CHECK (������������� BETWEEN 1 AND 100),  
    ���������� DATE NOT NULL,         
    ������������� DATE NOT NULL,       
    FOREIGN KEY (����Id) REFERENCES ����(Id) ON DELETE CASCADE
);

INSERT INTO ���� (����) VALUES
('������'),
('������'),
('��������'),
('�����');