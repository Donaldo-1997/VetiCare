# VetiCare

Sistema de gestión veterinaria desarrollado con **.NET 9 Web API** (arquitectura por capas) y **React + Vite** como frontend.

---

## Requisitos previos

Antes de clonar y correr el proyecto, asegúrate de tener instalado:

| Herramienta | Versión mínima | Descarga |
|---|---|---|
| .NET SDK | 9.0 | https://dotnet.microsoft.com/download |
| SQL Server Express / LocalDB | Cualquiera reciente | Incluido con Visual Studio, o https://www.microsoft.com/en-us/sql-server/sql-server-downloads |
| Node.js | 18+ | https://nodejs.org |
| npm | 9+ | Incluido con Node.js |
| Git | Cualquiera | https://git-scm.com |

> **¿Tengo LocalDB?** Abre una terminal y ejecuta `sqllocaldb info`. Si lista instancias, ya lo tienes.

---

## Clonar el repositorio

```bash
git clone <URL-del-repositorio>
cd VetiCare
```

---

## Backend — VetiCare API (.NET 9)

### 1. Verificar la cadena de conexión

Abre `VetiCare.API/appsettings.json`. Por defecto apunta a LocalDB:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=VetiCareDB;Integrated Security=true;TrustServerCertificate=true;"
}
```

Si usas una instancia diferente de SQL Server, cambia `Server=(localdb)\\mssqllocaldb` por tu cadena correspondiente.

### 2. Restaurar paquetes y aplicar migraciones

Desde la raíz del repositorio (donde está `VetiCare.sln`):

```bash
dotnet restore

# Aplicar las migraciones para crear la base de datos
dotnet ef database update --project VetiCare.DataAccess --startup-project VetiCare.API
```

> La aplicación también aplica las migraciones automáticamente al iniciar en entorno `Development` gracias al `DataSeeder`.

### 3. Correr la API

```bash
dotnet run --project VetiCare.API
```

La API quedará disponible en:

- **HTTP:** http://localhost:5183
- **Swagger UI:** https://localhost:7288/swagger

### 4. Datos de prueba (seed automático)

Al iniciar por primera vez en modo `Development`, el seeder carga datos de ejemplo y crea el usuario administrador:

| Campo | Valor |
|---|---|
| Email | `admin@veticare.com` |
| Contraseña | `Admin123!` |

---

## Frontend — React + Vite

### 1. Entrar a la carpeta del frontend

```bash
cd veticare-frontend-react
```

### 2. Instalar dependencias

```bash
npm install
```

### 3. Configurar la URL de la API

El archivo `.env` ya viene con la URL por defecto:

```
VITE_API_URL=http://localhost:5183/api
```

Si cambiaste el puerto de la API, actualiza ese valor.

### 4. Correr el frontend

```bash
npm run dev
```

La aplicación abre en http://localhost:5173 (o el puerto que Vite asigne).

---

## Correr todo junto

1. Terminal 1 — Backend:
   ```bash
   dotnet run --project VetiCare.API
   ```
2. Terminal 2 — Frontend:
   ```bash
   cd veticare-frontend-react && npm run dev
   ```
3. Abrir http://localhost:5173 en el navegador.

---

## Estructura del proyecto

```
VetiCare/
├── VetiCare.sln
├── VetiCare.Domain/          # Entidades, interfaces, enums, servicios de dominio
├── VetiCare.DataAccess/      # DbContext, repositorios, migraciones, seeder
├── VetiCare.API/             # Controladores, DTOs, validators, Program.cs
└── veticare-frontend-react/  # Frontend React + Vite + MUI
```

---

## Comandos útiles — EF Core

Todos se ejecutan desde la raíz de la solución:

```bash
# Crear una nueva migración
dotnet ef migrations add <NombreMigracion> --project VetiCare.DataAccess --startup-project VetiCare.API

# Aplicar migraciones pendientes
dotnet ef database update --project VetiCare.DataAccess --startup-project VetiCare.API

# Revertir la última migración
dotnet ef database update <MigracionAnterior> --project VetiCare.DataAccess --startup-project VetiCare.API
```

---

## Configuración CORS para desarrollo local

El backend lee los orígenes permitidos desde el archivo `VetiCare.API/appsettings.Development.json`. Este archivo **debe existir** con la siguiente sección para que el frontend pueda hacer peticiones sin errores de CORS:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:5173"
    ]
  }
}
```

El puerto `5173` es el que Vite usa por defecto. Si Vite arranca en otro puerto (lo indica en la terminal al ejecutar `npm run dev`), actualiza ese valor.

> `appsettings.Development.json` ya viene en el repositorio con esta configuración. Si alguien lo elimina o no lo tiene, la política CORS quedará sin orígenes y **todos los requests desde el frontend serán bloqueados**.

---

## Variables de entorno relevantes

| Variable | Archivo | Descripción |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | `appsettings.json` | Cadena de conexión a SQL Server |
| `Jwt:SecretKey` | `appsettings.json` | Clave secreta para firmar tokens JWT |
| `Cors:AllowedOrigins` | `appsettings.Development.json` | Orígenes permitidos por la política CORS |
| `VITE_API_URL` | `veticare-frontend-react/.env` | URL base de la API consumida por React |

> **Producción:** nunca subas `appsettings.json` con secretos reales. Usa `appsettings.Production.json` o variables de entorno del sistema.
