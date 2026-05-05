This project is licensed under the terms of the Apache License 2.0.

# Elmah.NetCore

ELMAH for ASP.NET Core (.NET Standard 2.0 / .NET 8+)

> **Forked from [ElmahCore/ElmahCore](https://github.com/ElmahCore/ElmahCore)** and republished as `Elmah.NetCore` v3.0.0 with updated dependencies, security fixes, and .NET 8/10 support.

![Elmah.Core UI](https://github.com/lawrence8358/Elmah.Core/raw/master/images/elmah-new-ui.png)

## Installation

Install the main NuGet package:

```
dotnet add package Elmah.NetCore
```

Optional storage providers:

| Package | Storage |
|---|---|
| [Elmah.NetCore.Sql](https://www.nuget.org/packages/Elmah.NetCore.Sql) | MS SQL Server |
| [Elmah.NetCore.MySql](https://www.nuget.org/packages/Elmah.NetCore.MySql) | MySQL |
| [Elmah.NetCore.Postgresql](https://www.nuget.org/packages/Elmah.NetCore.Postgresql) | PostgreSQL |

## Supported Frameworks

- .NET Standard 2.0 (compatible with .NET Framework 4.6.1+, .NET Core 2.0+)
- .NET 8.0 and above

## Simple Usage

**Program.cs** (minimal hosting, .NET 6+):
```csharp
builder.Services.AddElmah();  // in service registration
app.UseElmah();               // in middleware pipeline
```

**Startup.cs** (traditional):
```csharp
// ConfigureServices
services.AddElmah();

// Configure - must be after UseExceptionHandler / UseDeveloperExceptionPage
app.UseElmah();
```

Default Elmah path: `~/elmah`

## Change URL Path
```csharp
services.AddElmah(options => options.Path = "errors")
```

## Restrict Access
```csharp
services.AddElmah(options =>
{
    options.OnPermissionCheck = context => context.User.Identity.IsAuthenticated;
});
```

**Note:** `app.UseElmah()` must come after `app.UseAuthentication()` and `app.UseAuthorization()`.

## Change Error Log Type

Implement your own:
```csharp
class MyErrorLog : ErrorLog { ... }
```

Built-in options:
- `MemoryErrorLog` — in-memory (default)
- `XmlFileErrorLog` — XML files on disk
- `SqlErrorLog` — MS SQL Server (requires `Elmah.NetCore.Sql`)
- `MySqlErrorLog` — MySQL (requires `Elmah.NetCore.MySql`)
- `PgsqlErrorLog` — PostgreSQL (requires `Elmah.NetCore.Postgresql`)

```csharp
services.AddElmah<XmlFileErrorLog>(options =>
{
    options.LogPath = "~/log"; // or options.LogPath = @"C:\errors";
});
```

```csharp
services.AddElmah<SqlErrorLog>(options =>
{
    options.ConnectionString = "connection_string";
    options.SqlServerDatabaseSchemaName = "Errors";   // default: dbo
    options.SqlServerDatabaseTableName  = "ElmahError"; // default: ELMAH_Error
});
```

## Raise Exception Manually
```csharp
public IActionResult Test()
{
    HttpContext.RaiseError(new InvalidOperationException("Test"));
    ...
}
```

## Microsoft.Extensions.Logging Support

Since v2.0, Elmah.NetCore integrates with `Microsoft.Extensions.Logging`.

![Logging](https://github.com/lawrence8358/Elmah.Core/raw/master/images/elmah-log.png)

## Source Preview

Since v2.0.1 — configure source file paths:
```csharp
services.AddElmah(options =>
{
    options.SourcePaths = new[]
    {
        @"D:\src\MyProject",
        @"D:\src\MyProject.Mvc"
    };
});
```

## Log the Request Body

Since v2.0.5, Elmah.NetCore can log the HTTP request body.

## SQL Query Logging

Since v2.0.6, Elmah.NetCore intercepts and logs SQL commands via `DiagnosticSource`.

![SQL Log](https://github.com/lawrence8358/Elmah.Core/raw/master/images/elmah-4.png)

## Method Parameter Logging

Since v2.0.6:
```csharp
using ElmahCore;

public void TestMethod(string p1, int p2)
{
    this.LogParams((nameof(p1), p1), (nameof(p2), p2));
    // ...
}
```

![Parameters](https://github.com/lawrence8358/Elmah.Core/raw/master/images/elmah-5.png)

## Developer Exception Page

```csharp
if (app.Environment.IsDevelopment())
{
    // app.UseDeveloperExceptionPage();
    app.UseElmahExceptionPage();
}
```

## Notifiers

Implement `IErrorNotifier` or `IErrorNotifierWithId` and register:
```csharp
services.AddElmah<XmlFileErrorLog>(options =>
{
    options.Notifiers.Add(new ErrorMailNotifier("Email", emailOptions));
});
```

## Filters

XML-based or code-based error filtering:
```csharp
services.AddElmah<XmlFileErrorLog>(options =>
{
    options.FiltersConfig = "elmah.xml";
    options.Filters.Add(new MyFilter());
});
```

XML filter example:
```xml
<?xml version="1.0" encoding="utf-8" ?>
<elmah>
  <errorFilter>
    <notifiers>
      <notifier name="Email"/>
    </notifiers>
    <test>
      <and>
        <greater binding="HttpStatusCode" value="399" type="Int32" />
        <lesser  binding="HttpStatusCode" value="500" type="Int32" />
      </and>
    </test>
  </errorFilter>
</elmah>
```

See more at [elmah.github.io](https://elmah.github.io/a/error-filtering/examples/).

## Search and Filters

Since v2.2.0 — full-text search and multi-column filtering.

![Filters 1](https://github.com/lawrence8358/Elmah.Core/raw/master/images/elmah-filters-1.png)
![Filters 2](https://github.com/lawrence8358/Elmah.Core/raw/master/images/elmah-filters-2.png)
![Filters 3](https://github.com/lawrence8358/Elmah.Core/raw/master/images/elmah-filters-3.png)

Currently supported by Memory and XmlFile error logs only.

## Demo Projects

| Project | Framework | Description |
|---|---|---|
| `Demos/ElmahCore.DemoCore8` | .NET 8 | Startup.cs + SqlErrorLog |
| `Demos/ElmahCore.DemoCore10` | .NET 10 | Minimal hosting + XmlFileErrorLog + Notifiers + Filters |

## License

[Apache License 2.0](LICENSE) — Copyright 2018 ElmahCore, Portions Copyright © 2026 Lawrence Shen