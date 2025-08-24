# README.md
// filepath: c:\Proyectos\dotnet\mddr\README.md

# Extensiones de Servicio para Mddr

Este proyecto incluye extensiones para registrar los servicios de Mddr en el contenedor de dependencias usando `IServiceCollection`.

## Métodos disponibles

### 1. Registrar usando una lista de ensamblados

```csharp
services.AddMddr(new List<Assembly> { typeof(MiHandler).Assembly, typeof(OtroHandler).Assembly });
```

### 2. Registrar usando un solo ensamblado

```csharp
services.AddMddr(typeof(MiHandler).Assembly);
```

### 3. Registrar usando nombres de ensamblado

```csharp
services.AddMddr(new List<string> { "Jim.Mddr.Handlers", "Jim.Mddr.Otros" });
```

## ¿Qué servicios se registran?

- `ISender` como `MddrSender` (Scoped)
- Todos los handlers que implementen `IRequestHandler<TRequest, TResponse>` como interfaces implementadas (Scoped)

## Ejemplo de uso en `Program.cs`

```csharp
using Jim.Mddr.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Registrar Mddr usando el ensamblado actual
builder.Services.AddMddr(typeof(Program).Assembly);

var app = builder.Build();
// ...
```

## Requisitos

- .NET 6 o superior
- Paquete `Microsoft.Extensions.DependencyInjection`

## Notas

- Si usas nombres de ensamblado, asegúrate de que estén correctamente referenciados en tu proyecto.
- Los handlers deben implementar la interfaz `IRequestHandler<TRequest, TResponse>