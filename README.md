# Mddr - Biblioteca Mediador para CQRS en .NET

`jim.mddr` es una biblioteca que implementa el patrón mediador para facilitar la arquitectura **CQRS** (Command Query Responsibility Segregation) en aplicaciones .NET. Permite desacoplar el envío de comandos y consultas de sus manejadores, así como la ejecución de pipelines y publishers, proporcionando una estructura flexible y extensible para desarrollos modernos.

---

## ¿Qué es el patrón mediador y cómo se usa en CQRS?

El patrón mediador centraliza la comunicación entre objetos, evitando referencias directas entre ellos. En CQRS, esto permite que los comandos y consultas sean enviados a través de un mediador (`ISender`), que resuelve y ejecuta el handler adecuado, aplicando pipelines y publishers si es necesario.

---

## Instalación y requisitos

- .NET 6 o superior
- Referencia al paquete `Jim.Mddr` y sus dependencias

---

## Registro de servicios y configuración

La biblioteca utiliza un **patrón builder** para registrar servicios de forma fluida y encadenada. Ejemplo típico en `Program.cs`:

```csharp
builder.Services.AddMddr(typeof(Program).Assembly)
                .AddPipeline<Pipeline1>()
                .AddPipeline<LoggingPipeline>()
                .AddPublisher<Example1Publisher, TestRequest>()
                .AddPublisher<Example2Publisher, TestRequest>();
```

### Métodos principales de registro

- **AddMddr(Assembly assembly):**  
  Registra el mediador (`ISender`) y todos los handlers (`IRequestHandler<TRequest, TResponse>`) del ensamblado indicado.

- **AddPipeline<TPipeline>():**  
  Registra un pipeline personalizado que puede modificar el flujo de ejecución de comandos y consultas.

- **AddPublisher<TPublisher, TEntity>():**  
  Registra un publisher para un tipo de entidad específico. Todos los publishers registrados para una entidad serán ejecutados al publicar dicha entidad.

---

## Ejemplo de uso en endpoints

En el archivo `Program.cs` del ejemplo se definen dos endpoints principales:

- **GET /Commands**  
  Envía un comando (`TestRequest`) a través del mediador (`ISender`). El mediador localiza y ejecuta el `IRequestHandler` correspondiente para ese comando, retornando la respuesta generada por el handler.

- **GET /Publish**  
  Publica una entidad (`TestRequest`) usando el mediador (`ISender`). El mediador ejecuta todos los publishers (`IPublisher<TestRequest>`) registrados para ese tipo de objeto.

```csharp
app.MapGet("/Commands", async (ISender sender, CancellationToken cancellationToken) =>
{
    return await sender.SendAsync(new TestRequest(), cancellationToken);
});

app.MapGet("/Publish", async (ISender sender, CancellationToken cancellationToken) =>
{
    await sender.PublishAsync(new TestRequest(), cancellationToken);
    return Results.Ok();
});
```

---

## Ejemplo de implementación de componentes

### Handler de comando

```csharp
public class TestRequest : IRequest<TestResponse> { }

public class TestResponse
{
    public string Value { get; set; }
}

public class TestRequestHandler : IRequestHandler<TestRequest, TestResponse>
{
    public async Task<TestResponse> HandleAsync(TestRequest request, CancellationToken cancellationToken)
    {
        return await Task.FromResult(new TestResponse { Value = "Respuesta del handler" });
    }
}
```

### Pipeline personalizado

```csharp
public class LoggingPipeline : IPipeline
{
    public async Task<object> HandleAsync(object request, Func<Task<object>> next, CancellationToken cancellationToken)
    {
        Console.WriteLine("Inicio del pipeline");
        var response = await next();
        Console.WriteLine("Fin del pipeline");
        return response;
    }
}
```

### Publisher de entidad

```csharp
public class Example1Publisher : IPublisher<TestRequest>
{
    public async Task PublishAsync(TestRequest entity, CancellationToken cancellationToken)
    {
        Console.WriteLine("Publisher 1 ejecutado");
        await Task.CompletedTask;
    }
}
```

---

## Extensión y personalización

Puedes agregar tus propios handlers, pipelines y publishers implementando las interfaces:
- `IRequestHandler<TRequest, TResponse>`
- `IPipeline`
- `IPublisher<TEntity>`

Luego regístralos en el contenedor DI usando el builder encadenado.

---

## Pruebas recomendadas

- Verificar el registro y resolución de servicios.
- Comprobar la ejecución de handlers, pipelines y publishers.
- Validar el comportamiento ante errores (por ejemplo, cuando no hay handler registrado).
- Comprobar que el handler y los publishers correctos son llamados al usar los endpoints.

Ejemplo de test de integración:

```csharp
[Fact]
public async Task CommandsEndpoint_CallsHandler()
{
    var client = _factory.CreateClient();
    var response = await client.GetAsync("/Commands");
    var content = await response.Content.ReadAsStringAsync();
    Assert.Contains("Respuesta del handler", content);
}
```

---

## Licencia

Este proyecto está bajo la licencia MIT. Consulta el archivo `LICENSE` para más detalles.

---

¿Tienes dudas o necesitas ejemplos específicos? Consulta la documentación interna o abre una