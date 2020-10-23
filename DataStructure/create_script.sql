CREATE DATABASE Iso810
GO
USE Iso810
GO

BEGIN TRANSACTION
GO

/* CREATE TABLE START */

CREATE TABLE Provincias
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL
)
GO

CREATE TABLE Sectores
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL
)
GO

CREATE TABLE Secciones
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL
)
GO

CREATE TABLE Grados
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL
)
GO

CREATE TABLE Secciones_Grados
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    SeccionId INT NOT NULL FOREIGN KEY REFERENCES Secciones(Id),
    GradoId INT NOT NULL FOREIGN KEY REFERENCES Grados(Id)
)
GO

CREATE TABLE Escuelas
(
    Id INT NOT NULL PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Direccion VARCHAR(150),
    SectorId INT NOT NULL FOREIGN KEY REFERENCES Sectores(Id),
    ProvinciaId INT NOT NULL FOREIGN KEY REFERENCES Provincias(Id)
)
GO

CREATE TABLE Estudiantes
(
    Matricula INT NOT NULL PRIMARY KEY,
    Nombre VARCHAR(200) NOT NULL,
    EscuelaId INT NOT NULL FOREIGN KEY REFERENCES Escuelas(Id),
    Seccion_GradoId INT NOT NULL FOREIGN KEY REFERENCES Secciones_Grados(Id)
)
GO

CREATE TABLE Tandas
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    Hora_inicio VARCHAR(20),
    Hora_fin VARCHAR(20)
)
GO

CREATE TABLE Asignaturas
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Nombre VARCHAR(50) NOT NULL,
    TandaId INT NOT NULL FOREIGN KEY REFERENCES Tandas(Id)
)
GO

CREATE TABLE Asignaturas_Estudiantes
(
    Id INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    Calificacion INT NOT NULL,
    Condicion_academica VARCHAR(50) NOT NULL,
    EstudianteId INT NOT NULL FOREIGN KEY REFERENCES Estudiantes(Matricula),
    AsignaturaId INT NOT NULL FOREIGN KEY REFERENCES Asignaturas(Id)
)
GO

/* CREATE TABLE ENDS */

/* CREATE VIEW START */

CREATE OR ALTER VIEW Students_view
AS 
SELECT [c].[Id] AS [CODIGO DEL CENTRO]
    ,[c].[Nombre] AS [NOMBRE DEL CENTRO] 
    ,[s].[Nombre] AS [SECTOR]
    ,[p].[Nombre] AS [PROVINCIA]
    ,[e].[Matricula] AS [MATRICULA]
    ,[e].[Nombre] AS [NOMBRE DEL ESTUDIANTE]
    ,[a].[Nombre] AS [ASIGNATURA]
    ,[t].[Nombre] AS [TANDA]
    ,[ae].[Calificacion] AS [CALIFICACION]
    ,[ae].[Condicion_academica] AS [CONDICION ACADEMICA]
    ,[se].[Nombre] AS [SECCION]
    ,[g].[Nombre] AS [GRADO] 
FROM [Escuelas] AS [c]
LEFT JOIN [Sectores] [s] ON [s].[Id] = [c].[SectorId]
LEFT JOIN [Provincias] [p] ON [p].[Id] = [c].[ProvinciaId]
LEFT JOIN [Estudiantes] [e] ON [c].[Id] = [e].[EscuelaId]
LEFT JOIN [Asignaturas_Estudiantes] [ae] ON [e].[Matricula] = [ae].[EstudianteId]
LEFT JOIN [Asignaturas] [a] ON [ae].[AsignaturaId] = [a].[Id]
LEFT JOIN [Tandas] [t] ON [t].[Id] = [a].[TandaId]
LEFT JOIN [Secciones_Grados] [sg] ON [e].[Seccion_GradoId] = [sg].[Id]
LEFT JOIN [Secciones] [se] ON [se].[Id] = [sg].[SeccionId]
LEFT JOIN [Grados] [g] ON [g].[Id] = [sg].[GradoId]
GO

/* CREATE VIEW ENDS */

COMMIT