# Oogi

Repository pattern for DocumentDB.

## Connection string

You can set a global connection string in *.config file:

```xml
<appSettings>
<add key="Oogi.DocumentDbEndPoint" value="https://something.documents.azure.com:443" />
<add key="Oogi.DocumentDbAuthorizationKey" value="somEThiNG==" />
<add key="Oogi.DocumentDbUserAgentSuffix" value="Something" />
<add key="Oogi.DocumentDbDatabase" value="somedatabase" />
<add key="Oogi.DocumentDbCollection" value="somecollection" />
</appSettings>
```

Or when instancing a connection:

```csharp
var c = new Connection
(
    "https://something.documents.azure.com:443",
    "somEThiNG==",
    "somedatabase",
    "somecollection"
);
```

## Features

- Oogi automatically takes care of Status codes 429 + 503 and retries queries when exceeding RUs of your plan
- Oogi automatically takes care of an "infamous" error when your stored procedure exceeded RUs in history and is disabled and re-creates SP again

## Base entity

All "documentdb objects" are iherited from BaseEntity:

```csharp
public class Robot : BaseEntity
{            
    public override string Id { get; set; }
    public override string Entity { get; set; } = _entity;

    // Other properties
}
```

**Entity** property is our internal type for distinguishing document types.

## Repository

Init a repository is a easy task:

```csharp
var repo = new Repository<Robot>();
```

## Queries

Here's a list of methods. Each one has a corresponding _async_ variant.

### Create document

```csharp
repo.Create(new Robot { Name = "Tsumugi" });
```

### Delete document

```csharp
repo.Delete(robot);
```

### Get all documents

```csharp
var robots = repo.GetAll();
```

In this case there's an automatic _where_ clause: ```entity = '...'``` used.

### Get a filtered list of documents

```csharp
var q = new DynamicQuery<Robot>
    (
        "select * from c where c.entity = @entity and c.state = @state",             
        new
        {
            entity = "robot",
            state = State.Destroyed
        }
    );

var robots = repo.GetList(q);
```

### Get first document

Useful when you have only one record of a given entity.

```csharp
var robot = repo.GetFirstOrDefault();
```

In this case there's an automatic _where_ clause: ```entity = '...'``` used.

Or with _where_ clause:

```csharp
var robot = repo.GetFirstOrDefault(new DynamicQuery<Robot>("..."));
```

### Upsert documents from json string

```csharp
var file = File.ReadAllText("document.json");
var con = new Connection();
con.UpsertJson(file);
```

## Queries

You can use three types of predefined SQL queries.

### DynamicQuery

```csharp
var q = new DynamicQuery<Robot>
    (
        "select * from c where c.entity = @entity",             
        new
        {
            entity = "robot",            
        }
    );
```

### SqlQuerySpec

```csharp
var q = new SqlQuerySpec
    (
        "select * from c where c.entity = @entity",
        new SqlParameterCollection
        {
            new SqlParameter("@entity", "robot"),
        }
    );
```

### Pure string 

```csharp
var q = "select * from c where c.entity = 'robot'";
```

## Stored procedures

There are some methods for stored procedures:

- ReadStoredProcedure
- CreateStoredProcedure
- DeleteStoredProcedure

_TODO: more info_

## Tools

**Newtonsoft.Json** uses default settings for se/deserialization

``Tools.SetJsonDefaultSettings();`` forces to use a camel case casing.

So basically you don't need to set attributes for properties like that:

```csharp
[JsonProperty("name")]
public string Name { get; set; }
```

It's being handled automatically for you.

## Tokens

Because of a little bit complicated handlings of DateTimes in JSON and especially queries.
There are two helper class for that matter:

- SimpleStamp
- Stamp

JSON output looks like that:

```csharp
public SimpleStamp Created { get; set; }
```

```json
"created": {
          "dateTime": "2017-03-29T15:58:50.8828571+02:00",
          "epoch": 1490803130
        }
```

```csharp
public Stamp Created { get; set; }
```

```json
"created": {
      "dateTime": "2016-09-13T20:06:22.3214018+02:00",
      "epoch": 1473797182,
      "year": 2016,
      "month": 9,
      "day": 13,
      "hour": 20,
      "minute": 6,
      "second": 22
    }
```

## Tests

To run tests on your own you need to provide connection string.
I use a simple XML node injecting into app.config. But for obvious reasons Oogi.xml is not included in the repository.

## Something more

Yeah, sure. I will add some more "gems" later. And ofc I &#10084; DocumentDB :)

## Nuget

To install Oogi, run the following command in the Package Manager Console:

```Install-Package Oogi```

## TODO

- SP tests
- possibility to set own JsonSettings
- Stamps can be used in queries automatically with forcing epoch comparasion