# Getting Started

This section will guide you through installing and using the library.

## Prerequisites

- [.NET 8.0](https://dotnet.microsoft.com/download/dotnet/8.0) or later
- An IDE (Visual Studio 2022+, Visual Studio Code, or JetBrains Rider)

## Installation

Add the package via NuGet Package Manager:

### Package Manager Console

```powershell
Install-Package MediatorXL
```

### .NET CLI

```bash
dotnet add package MediatorXL
```

### PackageReference in .csproj

```xml
<PackageReference Include="MediatorXL" Version="*.*.*" />
```

## Setup MediatorXL

Here's an example of how to setup MediatorXL:

### 1. Add MediatorXL service via IServiceCollection extention method.

```csharp
builder.Services.AddMediatorXL(cfg =>
{
    cfg.AssembliesToScan = new[] // <- Specify assemlies that
                                 // will be scaned for special classes.
    {
        typeof(Program).Assembly,
    };
});
```

Note: Keep in mind that you will have to determine all types inside assemblies that you put in config's property: AssembliesToScan .

### 2. Inside specified assembly define a message type that you will send through MediatorXL.

```csharp
// You can use IMessage to define event like messages.
public class MediatorEvent : IMessage
{
    public string Data = "{name: 'MediatorXL', messageType: 'event'}";
}

// Or you can use IMessage<TResponse> to define requests.
public class MediatorRequest : IMessage<string>
{
    public string Data = "{name: 'MediatorXL', messageType: 'request'}";
}
```

### 3. Write some handlers for your messages.

Note that IServiceProvider will be provided to MediatorRequestHandler by Dependancy Injection.

```csharp
// We use IEventListener for event like messages.
public class MediatorEventHandler : IEventListener<MediatorEvent>
{
    public async Task Handle(MediatorEvent message, CancellationToken ct = default)
    {
        // Do something cool here.
    }
}

// We use IRequestHandler for request like messages.
public class MediatorRequestHandler(IServiceProvider serviceProvider) : IRequestHandler<MediatorRequest, string> // <- Response type as second argument.
{
    public async Task<string> Handle(MediatorRequest message, CancellationToken ct = default)
    {
        // Should return some response.
    }
}
```

### 4. Send the message via IMediator service.

```csharp
// For events
await mediator.Notify(new MediatorEvent());

// For requests
var response = await mediator.Request(new MediatorRequest());
```

# More futures

Here's a more of MediatorXL futures.

## Middlewares for message handling pipeline.

MediatorXL have three types of middleware at current moment:

- `Global Middleware` (Will be called for every single message).
- `Event Middleware` (Will be called for specified event message type only).
- `Request Middleware` (Will be called for specified request message type only).

### Middlewares Basis

Each type of middlewares have two different types of methods:

- Post-handlers methods (Invoked before any handler).
- Pre-handlers methods (Invoked after all handlers).

Pre-handlers methods should return IMiddlewareResult which is one of followed types:

- `BreakRequestResult` (Stops pipeline and returns result immediately).
  - Have `BreakRequestResult<TResponse>` variant for request type messages.
- `ContinueRequestResult`
- `RetryRequestResult`

MediatorXL allow you to use every single one of methods below for build those results:

- ```csharp
  Continue() // Pipeline will move to the next step.
  ```

- ```csharp
  Break() //Pipeline will be stoped and result will be returned immediately.
  ```

  - ```csharp
    Break<TResponse>(TResponse response) // Will return provided response. If no response provided, null will be returned.`
    ```

- ```csharp
  Retry() // In developing. (Behave as continue for now.)`
  ```

#### NOTE: Middlewares also supports dependancy injection, so you can provide services, but be aware that middlewares are being cached on the first run.

Middlewares supports execution priority, which can be specified with `PriorityAttribute` like this:

```csharp
[Priority(0)] // <- Priority attribute usage
public class SomeGlobalMiddleware : IGlobalMiddleware {}
```

Note: Middlewares with higher priority will be executed first.

MediatorXL also allow you to disable any middleware when it's not requiered anymore with DisabledAttribute:

```csharp
[Priority(0), Disabled] // <- Disabled attribute usage
public class SomeGlobalMiddleware : IGlobalMiddleware {}
```

Note: In this case, middleware marked as Disabled will NOT be loaded and will NOT be executed.

### Global Middlewares

Global middlewares is that kind of middleware which will be executed for every message sended through IMediator.

You can override any of the followed methods to define your logic.

```csharp
public class SomeGlobalMiddleware : IGlobalMiddleware {

    public override async Task<IMiddlewareResult> BeforeEventHandle(IMessage message, CancellationToken ct = default)
    {
        return Continue();
    }
    public override async Task<IMiddlewareResult> BeforeRequestHandle<TResponse>(IMessage<TResponse> message, CancellationToken ct = default)
    {
        return Continue();
    }

    public override async Task AfterEventHandle(IMessage message, CancellationToken ct = default)
    {
        return;
    }

    public override async Task AfterRequestHandle<TResponse>(IMessage message, TResponse response, CancellationToken ct = default)
    {
        return;
    }
}
```

Note: As long as it is global middlewares that should be executed for every message, you will get the message by IMessage interface.

### Event and Request middlewares

This kind of middlewares exist to provide the ability to control specific message handling pipeline. They both works in the same way except of the defenition:

```csharp
// This is how you define event middleware for MediatorEvent message.
    // Note: Here we have access to concrete MediatorEvent message.
    // Note: This middlewares also supports Priority, Disabled attributes.
public class EventMiddleware : BaseEventMiddleware<MediatorEvent>
{
    public override async Task<IMiddlewareResult> BeforeEventHandle(MediatorEvent message, CancellationToken ct = default)
    {
        return Continue();
    }
    public override async Task AfterEventHandle(MediatorEvent message, CancellationToken ct = default)
    {

    }
}

// This is how you define request middleware for MediatorRequest message.
[Priority(0)]
public class RequestMiddleware : BaseRequestMiddleware<MediatorRequest, string>
{
    public override async Task<IMiddlewareResult> BeforeRequestHandle(MediatorRequest message, CancellationToken ct = default)
    {
        return Continue();
    }

    public override async Task AfterRequestHandle(MediatorRequest message, string response, CancellationToken ct = default)
    {
        // Note: We already have access to response from handler here.
    }
}

```
