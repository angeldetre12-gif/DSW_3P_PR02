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
