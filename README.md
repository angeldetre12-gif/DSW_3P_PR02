# CharacterHub

API REST MVC para administrar personajes de videojuegos con Repository/Service,
SQL Server, Entity Framework Core, JWT, CORS y Swagger.

## Requisitos

- .NET 10 SDK
- SQL Server Express (`localhost\SQLEXPRESS`) o una instancia SQL Server accesible
- Entity Framework CLI 10.0.15 (`dotnet tool install --global dotnet-ef --version 10.0.15`)

## Conectar con SQL Server

1. Inicia SQL Server y ajusta la cadena de conexión local. La configuración
   incluida usa autenticación integrada y la instancia `localhost\SQLEXPRESS`:

   ```powershell
   dotnet user-secrets set "ConnectionStrings:SQLSERVERCONNECTION" 'Server=localhost\SQLEXPRESS;Database=CharacterHub;Integrated Security=True;Encrypt=True;TrustServerCertificate=True'
   ```

   Para LocalDB, usa por ejemplo:
   `Server=(localdb)\MSSQLLocalDB;Database=CharacterHub;Integrated Security=True;TrustServerCertificate=True`.
   Para autenticación SQL, guarda el usuario y la contraseña en User Secrets,
   nunca en `appsettings.json` ni en Git:

   ```powershell
   dotnet user-secrets set "ConnectionStrings:SQLSERVERCONNECTION" 'Server=localhost\SQLEXPRESS;Database=CharacterHub;User ID=USUARIO_SQL;Password=CONTRASENA_SQL;Encrypt=True;TrustServerCertificate=True'
   ```

2. Configura los secretos de JWT y el usuario de demostración. La clave de firma
   debe ser aleatoria y tener al menos 32 bytes. Este comando de PowerShell
   genera y guarda una clave local:

   ```powershell
   $rng = [System.Security.Cryptography.RandomNumberGenerator]::Create()
   $bytes = New-Object byte[] 48
   $rng.GetBytes($bytes)
   $key = [Convert]::ToBase64String($bytes)
   $rng.Dispose()
   dotnet user-secrets set "Jwt:SigningKey" $key
   dotnet user-secrets set "DemoUser:Username" "demo"
   dotnet user-secrets set "DemoUser:Password" "elige-una-contrasena-local"
   ```

   Cambia la contraseña de ejemplo por una propia; no la publiques. User Secrets
   se guarda fuera del repositorio. En producción, define esos mismos valores
   como variables `Jwt__SigningKey`, `DemoUser__Username` y `DemoUser__Password`
   en el gestor de secretos del servidor.

3. Restaura, aplica la migración y ejecuta:

   ```powershell
   dotnet restore
   dotnet ef database update
   dotnet run --launch-profile http
   ```

   También se aplican migraciones al iniciar la API. La migración
   `CharacterHubSchema` conserva personajes existentes, renombra `Anime` a
   `Videojuego` y `Tipo` a `Clase`, y asigna valores iniciales para `Nivel`,
   `Disponible` e `ImagenUrl`. Las antiguas columnas `Habilidad` y `Favorito`
   permanecen en SQL para no perder sus datos históricos, pero ya no forman
   parte del modelo ni de las respuestas de CharacterHub.

Con el perfil `http`, la API queda en `http://localhost:5153`; Swagger UI está
en `http://localhost:5153/swagger`.

## Autenticación

Llama `POST /api/auth/login` con el usuario y la contraseña configurados arriba.
La respuesta contiene un JWT con expiración de 60 minutos. En Swagger usa
**Authorize** y pega el token; al llamar la API desde otro cliente, envía:

```http
Authorization: Bearer <token>
```

Todos los endpoints `/api/personajes` requieren un token válido. El endpoint de
login no requiere autenticación.

## CORS

Los orígenes permitidos están en `Cors:AllowedOrigins` de `appsettings.json`.
Agrega ahí el origen exacto del cliente que indique el docente, sin una ruta
posterior; también se pueden definir por configuración de entorno. La política
permite los métodos HTTP y encabezados necesarios para la API.

## Personajes y rutas

Los campos son `id`, `nombre`, `videojuego`, `clase`, `nivel`, `poder`,
`disponible` e `imagenUrl`. Nombre, videojuego, clase e imagen son obligatorios;
nivel debe estar entre 1 y 100, poder debe ser mayor que cero, y `imagenUrl`
debe ser una URL absoluta HTTP/HTTPS. Al crear, `disponible` siempre inicia en
`true`.

| Método | Ruta | Resultado |
|---|---|---|
| POST | `/api/auth/login` | Inicia sesión y entrega un JWT |
| GET | `/api/personajes` | Consulta todos |
| GET | `/api/personajes/{id}` | Consulta por ID |
| GET | `/api/personajes/videojuego/{nombre}` | Filtra por videojuego |
| GET | `/api/personajes/nivel/{nivel}` | Nivel igual o superior |
| POST | `/api/personajes` | Registra (`201 Created`) |
| PUT | `/api/personajes/{id}` | Actualiza (`204 No Content`) |
| DELETE | `/api/personajes/{id}` | Elimina (`204 No Content`) |

Las solicitudes inválidas responden `400`; credenciales/token inválidos,
`401`; y personajes inexistentes, `404`. Consulta `DSW_3P_PR02.http` para
ejemplos de todas las operaciones.

Ejemplo de creación:

```json
{
  "nombre": "Ariadna",
  "videojuego": "Crónicas del Alba",
  "clase": "Exploradora",
  "nivel": 85,
  "poder": 900,
  "imagenUrl": "https://example.com/ariadna.png"
}
```

Para crear migraciones posteriores, ejecuta:

```powershell
dotnet ef migrations add NombreMigracion
dotnet ef database update
```
