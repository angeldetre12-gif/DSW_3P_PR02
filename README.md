# DSW_3P_PR02 — Gestor de Eventos Tecnológicos

API REST de eventos con ASP.NET Core MVC, Repository, Service, SQL Server y Entity Framework Core.

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

La aplicación aplica las migraciones al iniciar e indica el host en la consola.
Con el perfil `http`, abre `http://localhost:5153/swagger` (no abras un archivo
HTML directamente). La conexión y las consultas de Swagger usan la misma base
`DSW_3P_PR02` en `localhost\SQLEXPRESS`.

Las consultas `SELECT` y ejemplos para insertar eventos manualmente están en
[`sql/consultas_eventos.sql`](sql/consultas_eventos.sql).

Para generar futuras migraciones:

```powershell
dotnet ef migrations add NombreMigracion
dotnet ef database update
```

## Endpoints

| Método | Ruta | Resultado |
|---|---|---|
| GET | `/api/eventos` | Lista eventos |
| GET | `/api/eventos/{id}` | Obtiene un evento |
| POST | `/api/eventos` | Crea un evento (`201`) |
| PUT | `/api/eventos/{id}` | Actualiza un evento (`204`) |
| DELETE | `/api/eventos/{id}` | Elimina un evento (`204`) |

El nombre es obligatorio, el cupo máximo debe ser mayor que cero y la fecha debe
ser una fecha válida en formato `yyyy-MM-dd`. Las validaciones inválidas
responden `400`; los identificadores inexistentes responden `404`.

Ejemplo de cuerpo:

```json
{
  "nombre": "Hackaton de IA",
  "categoria": "Hackaton",
  "fecha": "2026-12-10",
  "lugar": "Monterrey",
  "cupoMaximo": 120,
  "activo": true
}
```

## Versiones en Git

El historial conserva la versión inicial con persistencia JSON y el cambio a
SQL Server/EF Core. Al migrar se sustituye el repositorio JSON por
`SqlEventoRepository`, se agrega `EventosDbContext` y se configura SQL Server.
El Controller, el Service y la interfaz `IEventoRepository` mantienen su
contrato.
