USE [master]
GO

CREATE DATABASE [JN_DB]
GO

USE [JN_DB]
GO

CREATE TABLE [dbo].[tError](
	[Consecutivo] [int] IDENTITY(1,1) NOT NULL,
	[Error] [varchar](max) NOT NULL,
	[Fecha] [datetime] NOT NULL,
	[Origen] [varchar](100) NOT NULL,
	[Usuario] [int] NOT NULL,
 CONSTRAINT [PK_tError] PRIMARY KEY CLUSTERED 
(
	[Consecutivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO

CREATE TABLE [dbo].[tRol](
	[Consecutivo] [int] IDENTITY(1,1) NOT NULL,
	[NombreRol] [varchar](100) NOT NULL,
 CONSTRAINT [PK_tRol] PRIMARY KEY CLUSTERED 
(
	[Consecutivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tServicio](
	[Consecutivo] [int] IDENTITY(1,1) NOT NULL,
	[Nombre] [varchar](200) NOT NULL,
	[Descripcion] [varchar](1000) NOT NULL,
	[Precio] [decimal](10, 2) NOT NULL,
	[Estado] [bit] NOT NULL,
	[Video] [varchar](200) NOT NULL,
	[ConsecutivoUsuario] [int] NOT NULL,
 CONSTRAINT [PK_tServicio] PRIMARY KEY CLUSTERED 
(
	[Consecutivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

CREATE TABLE [dbo].[tUsuarios](
	[Consecutivo] [int] IDENTITY(1,1) NOT NULL,
	[Identificacion] [varchar](15) NOT NULL,
	[Nombre] [varchar](200) NOT NULL,
	[CorreoElectronico] [varchar](100) NOT NULL,
	[Contrasenna] [varchar](200) NOT NULL,
	[Estado] [bit] NOT NULL,
	[ImagenPerfil] [varchar](200) NULL,
	[ConsecutivoRol] [int] NOT NULL,
 CONSTRAINT [PK_tUsuarios] PRIMARY KEY CLUSTERED 
(
	[Consecutivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[tRol] ON 
GO
INSERT [dbo].[tRol] ([Consecutivo], [NombreRol]) VALUES (1, N'Admin')
GO
INSERT [dbo].[tRol] ([Consecutivo], [NombreRol]) VALUES (2, N'Usuario')
GO
SET IDENTITY_INSERT [dbo].[tRol] OFF
GO

SET IDENTITY_INSERT [dbo].[tUsuarios] ON 
GO
INSERT [dbo].[tUsuarios] ([Consecutivo], [Identificacion], [Nombre], [CorreoElectronico], [Contrasenna], [Estado], [ImagenPerfil], [ConsecutivoRol]) VALUES (1, N'207640592', N'MORALES RAMIREZ LUIS DANIEL', N'lmorales40592@ufide.ac.cr', N'DkF5eJ1UhQwmEXbYNJDqmQ==', 1, N'/uploads/1.png', 2)
GO
INSERT [dbo].[tUsuarios] ([Consecutivo], [Identificacion], [Nombre], [CorreoElectronico], [Contrasenna], [Estado], [ImagenPerfil], [ConsecutivoRol]) VALUES (2, N'304590415', N'CALVO CASTILLO EDUARDO JOSE', N'ecalvo90415@ufide.ac.cr', N'DkF5eJ1UhQwmEXbYNJDqmQ==', 1, N'/uploads/2.png', 2)
GO
SET IDENTITY_INSERT [dbo].[tUsuarios] OFF
GO

ALTER TABLE [dbo].[tUsuarios] ADD  CONSTRAINT [UK_Correo] UNIQUE NONCLUSTERED 
(
	[CorreoElectronico] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tUsuarios] ADD  CONSTRAINT [UK_Identificacion] UNIQUE NONCLUSTERED 
(
	[Identificacion] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
GO

ALTER TABLE [dbo].[tServicio]  WITH CHECK ADD  CONSTRAINT [FK_tServicio_tUsuarios] FOREIGN KEY([ConsecutivoUsuario])
REFERENCES [dbo].[tUsuarios] ([Consecutivo])
GO
ALTER TABLE [dbo].[tServicio] CHECK CONSTRAINT [FK_tServicio_tUsuarios]
GO

ALTER TABLE [dbo].[tUsuarios]  WITH CHECK ADD  CONSTRAINT [FK_tUsuarios_tRol] FOREIGN KEY([ConsecutivoRol])
REFERENCES [dbo].[tRol] ([Consecutivo])
GO
ALTER TABLE [dbo].[tUsuarios] CHECK CONSTRAINT [FK_tUsuarios_tRol]
GO

CREATE PROCEDURE [dbo].[sp_ActualizarContrasenna]
    @Consecutivo  int,
    @Contrasenna  varchar(200)
AS
BEGIN

    UPDATE  dbo.tUsuarios
    SET     Contrasenna = @Contrasenna
    WHERE   Consecutivo = @Consecutivo

END
GO

CREATE PROCEDURE [dbo].[sp_ActualizarPerfil]
    @Consecutivo  int,
    @Identificacion  varchar(15),
    @Nombre  varchar(200),
    @CorreoElectronico  varchar(100),
    @ImagenPerfil  varchar(200)
AS
BEGIN

    UPDATE  dbo.tUsuarios
    SET     Identificacion = @Identificacion,
            Nombre = @Nombre,
            CorreoElectronico = @CorreoElectronico,
            ImagenPerfil = CASE WHEN RTRIM(LTRIM(@ImagenPerfil)) = '' THEN ImagenPerfil ELSE @ImagenPerfil END
    WHERE   Consecutivo = @Consecutivo

END
GO

CREATE PROCEDURE [dbo].[sp_ConsultarUsuario]
	@Consecutivo  int
AS
BEGIN
	
    SELECT  Consecutivo,
            Identificacion,
            Nombre,
            CorreoElectronico,
            Estado
    FROM    dbo.tUsuarios
    WHERE   Consecutivo = @Consecutivo
        AND Estado = 1

END
GO

CREATE PROCEDURE [dbo].[sp_IniciarSesion]
	@CorreoElectronico  varchar(100),
    @Contrasenna        varchar(200)
AS
BEGIN
	
    SELECT  Consecutivo,
            Identificacion,
            Nombre,
            CorreoElectronico,
            Estado,
            ImagenPerfil
    FROM    dbo.tUsuarios
    WHERE   CorreoElectronico = @CorreoElectronico
        AND Contrasenna = @Contrasenna
        AND Estado = 1

END
GO

CREATE PROCEDURE [dbo].[sp_RegistrarCuenta]
    @Identificacion     varchar(15),
    @Nombre             varchar(200),
    @CorreoElectronico  varchar(100),
    @Contrasenna        varchar(200)
AS
BEGIN

    IF NOT EXISTS(
        SELECT  1 FROM tUsuarios
        WHERE   Identificacion = @Identificacion
            OR  CorreoElectronico =  @CorreoElectronico)
    BEGIN
	
        INSERT INTO dbo.tUsuarios (Identificacion,Nombre,CorreoElectronico,Contrasenna,Estado,ConsecutivoRol)
        VALUES (@Identificacion,@Nombre,@CorreoElectronico,@Contrasenna,1,2)

    END
END
GO

CREATE PROCEDURE [dbo].[sp_RegistrarError]
    @Error  varchar(max),
    @Fecha  datetime,
    @Origen varchar(100),
    @Usuario int
AS
BEGIN

    INSERT INTO dbo.tError(Error,Fecha,Origen,Usuario)
    VALUES(@Error,@Fecha,@Origen,@Usuario)

END
GO

CREATE PROCEDURE [dbo].[sp_ValidarCorreo]
	@CorreoElectronico  varchar(100)
AS
BEGIN
	
    SELECT  Consecutivo,
            Identificacion,
            Nombre,
            CorreoElectronico,
            Estado
    FROM    dbo.tUsuarios
    WHERE   CorreoElectronico = @CorreoElectronico
        AND Estado = 1

END
GO