CREATE TABLE Auditoria
(
	auditoriaId BIGINT IDENTITY(1,1) NOT NULL PRIMARY KEY,
	fecha DATETIME NOT NULL,
	entidad NVARCHAR(400) NOT NULL,
	valorKeyEntidad BIGINT NULL,
	usuarioId NVARCHAR(50) NOT NULL,
	usuarioName NVARCHAR(50) NULL,
	operacion NVARCHAR(10) NOT NULL,
	valoresAnterior XML NULL,
	valoresNuevo XML NULL,
	maquinaIp NVARCHAR(50) NULL,
	applicacionId INT NULL
)
GO