
 use DB_VOTACIONONLINE

 go


 insert into USUARIOS(DocumentoIdentidad,Nombres,Apellidos,Correo,Clave,Activo) values
 ('78787878','USER','TEST','USER@GMAIL.COM','985d8b7897db3f8c531afd95883f827b3ed247f0ace7c0cc05f8a81b9352d4e5',1)

 go

 insert into CONFIGURACION(IdConfiguracion,Descripcion,Valor)
 values(1,'Imagenes Candidatos','D:\ImagenesCandidatos\')

