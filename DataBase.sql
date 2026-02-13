USE [master]
GO

CREATE DATABASE [JN_DB]
GO

USE [JN_DB]
GO

CREATE TABLE [dbo].[tUsuarios](
	[Consecutivo] [int] IDENTITY(1,1) NOT NULL,
	[Identificacion] [varchar](15) NOT NULL,
	[Nombre] [varchar](200) NOT NULL,
	[CorreoElectronico] [varchar](100) NOT NULL,
	[Contrasenna] [varchar](200) NOT NULL,
	[Estado] [bit] NOT NULL,
 CONSTRAINT [PK_tUsuarios] PRIMARY KEY CLUSTERED 
(
	[Consecutivo] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
GO

SET IDENTITY_INSERT [dbo].[tUsuarios] ON 
GO
INSERT [dbo].[tUsuarios] ([Consecutivo], [Identificacion], [Nombre], [CorreoElectronico], [Contrasenna], [Estado]) VALUES (1, N'304590415', N'EDUARDO JOSE CALVO CASTILLO', N'ecalvo90415@ufide.ac.cr', N'90415', 1)
GO
INSERT [dbo].[tUsuarios] ([Consecutivo], [Identificacion], [Nombre], [CorreoElectronico], [Contrasenna], [Estado]) VALUES (3, N'304590416', N'EDUARDO JOSE CALVO CASTILLO', N'ecalvo90416@ufide.ac.cr', N'90416', 1)
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

CREATE PROCEDURE [dbo].[sp_IniciarSesion]
	@CorreoElectronico  varchar(100),
    @Contrasenna        varchar(200)
AS
BEGIN
	
    SELECT  Consecutivo,
            Identificacion,
            Nombre,
            CorreoElectronico,
            Estado
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
	
        INSERT INTO dbo.tUsuarios (Identificacion,Nombre,CorreoElectronico,Contrasenna,Estado)
        VALUES (@Identificacion,@Nombre,@CorreoElectronico,@Contrasenna,1)

    END
END
GO