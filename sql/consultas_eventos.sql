USE [DSW_3P_PR02];
GO

-- Ver todos los eventos creados desde Swagger o SQL Server.
SELECT Id, Nombre, Categoria, Fecha, Lugar, CupoMaximo, Activo
FROM dbo.Eventos
ORDER BY Id;
GO

-- Ver únicamente los eventos activos, ordenados por fecha.
SELECT Id, Nombre, Categoria, Fecha, Lugar, CupoMaximo, Activo
FROM dbo.Eventos
WHERE Activo = 1
ORDER BY Fecha;
GO

-- Agregar un evento manualmente. Id se genera automáticamente.
INSERT INTO dbo.Eventos (Nombre, Categoria, Fecha, Lugar, CupoMaximo, Activo)
VALUES (N'Hackaton de IA', N'Hackaton', '2026-12-10', N'Monterrey', 120, 1);
GO

-- Confirmar el evento insertado.
SELECT TOP (10) Id, Nombre, Categoria, Fecha, Lugar, CupoMaximo, Activo
FROM dbo.Eventos
ORDER BY Id DESC;
GO

-- Ejemplo opcional: actualizar un evento existente reemplazando 1 por su Id.
-- UPDATE dbo.Eventos
-- SET CupoMaximo = 150, Activo = 1
-- WHERE Id = 1;
--
-- Ejemplo opcional: eliminar un evento existente reemplazando 1 por su Id.
-- DELETE FROM dbo.Eventos
-- WHERE Id = 1;
