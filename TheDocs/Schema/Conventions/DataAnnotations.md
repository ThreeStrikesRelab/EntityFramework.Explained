# Data Annotations: `[Range(...)]`
**Given:**  
```csharp
public class Thing
{
    public int Id { get; set; }
    [Range(0, 10)] // <= We are checking this 
    public int SecondInt { get; set; }
}
```
## Sql Server
`[Range(0,10)]` gets ignored : `[SecondInt] int NOT NULL`.  
## Sqlite
Same behaviour as Sql Server: `"SecondInt" INTEGER NOT NULL`.  
