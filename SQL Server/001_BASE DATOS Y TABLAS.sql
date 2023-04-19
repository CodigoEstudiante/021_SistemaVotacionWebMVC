
CREATE DATABASE DB_VOTACIONONLINE
GO
USE DB_VOTACIONONLINE

GO

CREATE TABLE USUARIOS(
IdUsuario int primary key identity,
DocumentoIdentidad varchar(50),
Nombres varchar(50),
Apellidos varchar(50),
Correo varchar(60),
Clave varchar(300),
Activo bit
)

go

CREATE TABLE ELECCIONES(
IdEleccion int primary key identity,
Descripcion varchar(100),
Cargo varchar(100),
Activo bit,
FechaRegistro datetime,
IdUsuarioRegistro int references USUARIOS(IdUsuario)
)

GO

CREATE TABLE CANDIDATOS(
IdCandidato int primary key identity,
NombresCompleto varchar(100),
Mensaje varchar(100),
RutaImagen varchar(100),
IdEleccion int references ELECCIONES(IdEleccion),
IdUsuarioRegistro int references USUARIOS(IdUsuario),
Activo bit,
FechaRegistro datetime
)


GO

CREATE TABLE VOTANTES(
IdVotante int primary key identity,
DocumentoIdentidad varchar(50),
Nombres varchar(50),
Apellidos varchar(50),
IdEleccion int references ELECCIONES(IdEleccion),
IdUsuarioRegistro int references USUARIOS(IdUsuario),
Activo bit,
FechaRegistro datetime,
EmitioVotacion bit default 0
)

GO


CREATE TABLE CONFIGURACION(
IdConfiguracion int primary key,
Descripcion varchar(100),
Valor varchar(100)
)

GO

CREATE TABLE VOTACION(
IdVotacion int primary key identity,
IdEleccion int references ELECCIONES(IdEleccion),
IdVotante int references VOTANTES(IdVotante),
IdCandidato int references CANDIDATOS(IdCandidato),
FechaRegistro datetime default getdate()
)

go

