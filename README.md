# DSW_3P_PR02 — API de personajes

API REST de personajes con ASP.NET Core, Repository, Service, SQL Server y Entity Framework Core.

## Requisitos

- .NET 10 SDK
- SQL Server Express LocalDB o una instancia SQL Server accesible
- `dotnet-ef` 10 (`dotnet tool install --global dotnet-ef --version 10.0.15`)

La cadena de conexión de ejemplo usa autenticación integrada para
`localhost\SQLEXPRESS`; ajusta `ConnectionStrings:SQLSERVERCONNECTION` en
`appsettings.Development.json` o mediante la variable de entorno
`ConnectionStrings__SQLSERVERCONNECTION` para tu servidor. No guardes contraseñas
en archivos versionados.

## Crear la base y ejecutar

Desde la carpeta del proyecto:

```powershell
dotnet restore
dotnet run --launch-profile http
```

La aplicación aplica las migraciones al iniciar e indica el host en la consola,
sincronizando el esquema con personajes y conservando los personajes registrados
en bases existentes. Con el perfil `http`, abre `http://localhost:5153/swagger`.

## Ajustes en SQL Server

La aplicación ejecuta las migraciones automáticamente al iniciar. También puedes
aplicarlas manualmente desde la carpeta del proyecto:

```powershell
dotnet ef database update
```

La migración deja la tabla `dbo.Personajes` lista y conserva sus filas existentes.
Para consultar o editar datos desde SQL Server Management Studio:

```sql
USE [DSW_3P_PR02];

SELECT Id, Nombre, Anime, Tipo, Poder, Habilidad, Favorito
FROM dbo.Personajes
ORDER BY Id;

-- Sustituye 3 por el Id que quieras modificar.
UPDATE dbo.Personajes
SET Poder = 98, Favorito = 1
WHERE Id = 3;
```

La cadena de conexión se configura con `ConnectionStrings:SQLSERVERCONNECTION`
en `appsettings.Development.json` o con la variable de entorno
`ConnectionStrings__SQLSERVERCONNECTION`.

Para usar autenticación de SQL Server en desarrollo, guarda la cadena localmente
con User Secrets, reemplazando `USUARIO_SQL` y `CONTRASENA_SQL` por tus datos:

```powershell
dotnet user-secrets set "ConnectionStrings:SQLSERVERCONNECTION" 'Server=localhost\SQLEXPRESS;Database=DSW_3P_PR02;User ID=USUARIO_SQL;Password=CONTRASENA_SQL;Encrypt=True;TrustServerCertificate=True'
```

Después inicia la API con `dotnet run --launch-profile http`. El secreto local
reemplaza la cadena de autenticación integrada de `appsettings.json` y no se
sube a GitHub. Para otros entornos, configura `ConnectionStrings__SQLSERVERCONNECTION`
en el administrador de secretos del servidor.

## Conectar la interfaz

Inicia la API con `dotnet run --launch-profile http`, sirve
`QuestLog_Frontend.html` desde `http://localhost:5500` (por ejemplo, con Live
Server en VS Code) y configura `http://localhost:5153` como URL base. Si la
interfaz está publicada, configura la URL pública del backend, no la URL de
GitHub Pages.

Para generar futuras migraciones:

```powershell
dotnet ef migrations add NombreMigracion
dotnet ef database update
```

## Endpoints

| Método | Ruta | Resultado |
|---|---|---|
| GET | `/api/personajes` | Lista personajes |
| GET | `/api/personajes/{id}` | Obtiene un personaje |
| POST | `/api/personajes` | Crea un personaje (`201`) |
| PUT | `/api/personajes/{id}` | Actualiza un personaje (`204`) |
| DELETE | `/api/personajes/{id}` | Elimina un personaje (`204`) |

El nombre es obligatorio y el poder debe estar entre 0 y 100. Las solicitudes
inválidas responden `400`; los identificadores inexistentes responden `404`.

Ejemplo de cuerpo:

```json
{
  "nombre": "Ariadna",
  "anime": "Crónicas del Alba",
  "tipo": "Principal",
  "poder": 85,
  "habilidad": "Control del viento",
  "favorito": true
}
```
