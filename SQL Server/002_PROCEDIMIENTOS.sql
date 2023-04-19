

 use DB_VOTACIONONLINE

 go

 CREATE PROC usp_ObtenerUsuarios
 as
 begin
	select * from USUARIOS
 end

 GO

--PROCEDIMIENTO PARA REGISTRAR USUARIO
CREATE PROC usp_RegistrarUsuario(
@DocumentoIdentidad varchar(50),
@Nombres varchar(50),
@Apellidos varchar(50),
@Correo varchar(50),
@Clave varchar(300),
@Resultado bit output
)as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM USUARIOS WHERE Correo = @Correo)

		insert into USUARIOS(DocumentoIdentidad,Nombres,Apellidos,Correo,Clave,Activo) values (
		@DocumentoIdentidad,@Nombres,@Apellidos,@Correo,@Clave,1)
	ELSE
		SET @Resultado = 0
	
end
go


--PROCEDIMIENTO PARA MODIFICAR USUARIO
create procedure usp_ModificarUsuario(
@IdUsuario int,
@DocumentoIdentidad varchar(50),
@Nombres varchar(50),
@Apellidos varchar(50),
@Correo varchar(50),
@Activo bit,
@Resultado bit output
)
as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM USUARIOS WHERE Correo = @Correo and IdUsuario != @IdUsuario)
		
		update USUARIOS set 
		DocumentoIdentidad = @DocumentoIdentidad,
		Nombres = @Nombres,
		Apellidos = @Apellidos,
		Correo = @Correo,
		Activo = @Activo
		where IdUsuario = @IdUsuario

	ELSE
		SET @Resultado = 0

end

go

--PROCEDIMIENTO PARA ELIMINAR USUARIO
create procedure usp_EliminarUsuario(
@IdUsuario int,
@Resultado bit output
)
as
begin

	delete from USUARIOS where IdUsuario = @IdUsuario
	SET @Resultado = 1
end

go

--PROCEDIMIENTO PARA LISTAR ELECCIONES

 CREATE PROC usp_ObtenerElecciones
 as
 begin
	SET DATEFORMAT dmy; 
	select e.IdEleccion,e.Descripcion,e.Cargo,e.Activo,e.FechaRegistro,
	u.IdUsuario,u.Correo  from ELECCIONES e
	join USUARIOS u on u.IdUsuario = e.IdUsuarioRegistro
 end

 GO

--PROCEDIMIENTO PARA REGISTRAR ELECCIONES
CREATE PROC usp_RegistrarEleccion(
@Descripcion varchar(100),
@Cargo varchar(100),
@IdUsuarioRegistro int,
@Resultado bit output
)as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM ELECCIONES WHERE Descripcion = @Descripcion)

		insert into ELECCIONES(Descripcion,Cargo,Activo,FechaRegistro,IdUsuarioRegistro) values (
		@Descripcion,@Cargo,1,GETDATE(),@IdUsuarioRegistro)
	ELSE
		SET @Resultado = 0
	
end
go 


--PROCEDIMIENTO PARA MODIFICAR ELECCIONES
CREATE PROC usp_ModificarEleccion(
@IdEleccion int,
@Descripcion varchar(100),
@Cargo varchar(100),
@Activo bit,
@Resultado bit output
)as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM ELECCIONES WHERE Descripcion = @Descripcion and IdEleccion != @IdEleccion)
		update ELECCIONES set 
		Descripcion = @Descripcion,
		Cargo = @Cargo,
		Activo = @Activo
		where IdEleccion = @IdEleccion

	ELSE
		SET @Resultado = 0
	
end
go

--PROCEDIMIENTO PARA LISTAR CONFIGURACIONES

 CREATE PROC usp_ObtenerConfiguraciones
 as
 begin
	select IdConfiguracion,Descripcion,Valor from CONFIGURACION
 end

 GO

 --PROCEDIMIENTO PARA LISTAR CANDIDATOS

 CREATE PROC usp_ObtenerCandidatos
 as
 begin
	SET DATEFORMAT dmy; 
	select c.IdCandidato,c.NombresCompleto,c.Mensaje,c.RutaImagen,c.IdEleccion,c.Activo,
	c.FechaRegistro,u.IdUsuario,u.Correo
	from CANDIDATOS c
	join USUARIOS u on u.IdUsuario = c.IdUsuarioRegistro

 end

 GO

--PROCEDIMIENTO PARA REGISTRAR CANDIDATOS
CREATE PROC usp_RegistrarCandidato(
@NombresCompleto varchar(100),
@Mensaje varchar(100),
@IdEleccion int,
@IdUsuarioRegistro int,
@Resultado int output
)as
begin
	SET @Resultado = 0
	begin try
		insert into CANDIDATOS(NombresCompleto,Mensaje,IdEleccion,IdUsuarioRegistro,Activo,FechaRegistro,RutaImagen) values (
		@NombresCompleto,@Mensaje,@IdEleccion,@IdUsuarioRegistro,1,getdate(),'')
	
		SET @Resultado  = Scope_identity()
	end try
	begin catch
		SET @Resultado = 0
	end catch

end
go 


--PROCEDIMIENTO PARA MODIFICAR CANDIDATO
CREATE PROC usp_ModificarCandidato(
@IdCandidato int,
@NombresCompleto varchar(100),
@Mensaje varchar(100),
@Activo bit,
@Resultado bit output
)as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM CANDIDATOS WHERE NombresCompleto = @NombresCompleto and IdCandidato != @IdCandidato)
		update CANDIDATOS set 
		NombresCompleto = @NombresCompleto,
		Mensaje = @Mensaje,
		Activo = @Activo
		where IdCandidato = @IdCandidato

	ELSE
		SET @Resultado = 0
	
end
go

--PROCEDIMIENTO PARA ACTUALIZAR RUTA IMAGEN CANDIDATOS
CREATE PROC usp_ActualizarRutaImagenCandidato(
@IdCandidato int,
@RutaImagen varchar(100)
)
as
begin
	update CANDIDATOS set RutaImagen = @RutaImagen where IdCandidato = @IdCandidato
end

go

CREATE TYPE [dbo].[EstructuraCarga] AS TABLE(
	[DocumentoIdentidad] [varchar](max) NULL,
	[Nombres] [varchar](max) NULL,
	[Apellidos] [varchar](max) NULL,
	[IdEleccion] [varchar](max) NULL,
	[IdUsuarioRegistro] [varchar](max) NULL
)
GO


 create procedure usp_cargarVotantes(
@EstructuraCarga EstructuraCarga READONLY
)
as
begin
	DECLARE @TABLE TABLE(
		DocumentoIdentidad varchar(100),
		Nombres varchar(100),
		Apellidos varchar(100),
		IdEleccion int,
		IdUsuarioRegistro int,
		Estado bit,
		Mensaje varchar(900))

	BEGIN TRY

		insert into @TABLE(DocumentoIdentidad,Nombres,Apellidos,IdEleccion,IdUsuarioRegistro,Estado,Mensaje)
		select DocumentoIdentidad,Nombres,Apellidos,CONVERT(int, IdEleccion),CONVERT(int, IdUsuarioRegistro),
		1,''
		from @EstructuraCarga

		update t set t.Estado = 0 , t.Mensaje = 'Ya se encuentra registrado' from VOTANTES v
		inner join @TABLE t on t.IdEleccion = v.IdEleccion and v.DocumentoIdentidad = t.DocumentoIdentidad

		insert into VOTANTES(DocumentoIdentidad,Nombres,Apellidos,IdEleccion,IdUsuarioRegistro,Activo,FechaRegistro)
		select DocumentoIdentidad,Nombres,Apellidos,IdEleccion,IdUsuarioRegistro,1,GETDATE() from @TABLE where Estado = 1

		update t set t.Mensaje = 'OK' from VOTANTES v
		inner join @TABLE t on t.IdEleccion = v.IdEleccion and v.DocumentoIdentidad = t.DocumentoIdentidad
		and t.Estado = 1

	END TRY
	BEGIN CATCH
		insert into @TABLE(DocumentoIdentidad,Nombres,Apellidos,IdEleccion,IdUsuarioRegistro,Estado,Mensaje)
		select DocumentoIdentidad,Nombres,Apellidos,0,0,0,ERROR_MESSAGE()
		from @EstructuraCarga
	END CATCH

	select Estado,Mensaje,DocumentoIdentidad,Nombres,Apellidos from @TABLE

end

go

 create PROC usp_ObtenerVotantes
 as
 begin
	SET DATEFORMAT dmy; 
	select c.IdVotante,c.DocumentoIdentidad,c.Nombres,c.Apellidos,c.IdEleccion,c.Activo,
	c.FechaRegistro,u.IdUsuario,u.Correo,c.EmitioVotacion
	from VOTANTES c
	join USUARIOS u on u.IdUsuario = c.IdUsuarioRegistro

 end

go

--PROCEDIMIENTO PARA MODIFICAR CANDIDATO
CREATE PROC usp_ModificarVotante(
@IdVotante int,
@DocumentoIdentidad varchar(100),
@Nombres varchar(100),
@Apellidos varchar(100),
@Activo bit,
@Resultado bit output
)as
begin
	SET @Resultado = 1
	IF NOT EXISTS (SELECT * FROM VOTANTES WHERE DocumentoIdentidad = @DocumentoIdentidad and IdVotante != @IdVotante)
		update VOTANTES set 
		DocumentoIdentidad = @DocumentoIdentidad,
		Nombres = @Nombres,
		Apellidos = @Apellidos,
		Activo = @Activo
		where IdVotante = @IdVotante

	ELSE
		SET @Resultado = 0
	
end
go

--PROCEDIMIENTO PARA REGISTRAR VOTACION
create PROC usp_RegistrarVotacion(
@IdEleccion int,
@DocumentoIdentidad varchar(50),
@IdCandidato int,
@Resultado bit output
)as
begin
	SET @Resultado = 0
	begin try
		
		declare @IdVotante int = (select IdVotante from VOTANTES where DocumentoIdentidad = @DocumentoIdentidad and IdEleccion = @IdEleccion)

		insert into VOTACION(IdEleccion,IdVotante,IdCandidato,FechaRegistro)
		values(@IdEleccion,@IdVotante,@IdCandidato,GETDATE())

		update VOTANTES set EmitioVotacion = 1 where IdVotante = @IdVotante and IdEleccion =  @IdEleccion
	
		SET @Resultado = 1
	end try
	begin catch
		SET @Resultado = 0
	end catch

end
go 

 create PROC usp_ObtenerResultados(
 @IdEleccion int
 )
 as
 begin

	select c.NombresCompleto,c.RutaImagen,count(v.IdVotacion)[Votos] from CANDIDATOS c
	left join VOTACION v on c.IdEleccion = v.IdEleccion and c.IdCandidato = v.IdCandidato
	where c.IdEleccion = @IdEleccion
	group by c.NombresCompleto,c.RutaImagen

 end

go

