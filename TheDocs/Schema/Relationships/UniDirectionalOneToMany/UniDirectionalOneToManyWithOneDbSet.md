# Uni Directional One To Many With One Db Set
Because the entity used in the `DbSet` has a collection of another entity type, the latter are mapped as well.  
EF infers and includes related entities in the schema even when only one side is explicitly registered in the `DbContext`.  

When using the following simple model of *one* `Blog` containing *many* `Posts`: 
```csharp
public class Blog
{
    public int Id { get; set; }
}
```
```csharp
public class Post
{
    public int Id { get; set; }
    public Blog Blog { get; set; } = default!;
}
```
And then adding a `DbSet<Blog>` to the `DbContext` EF generates the following ddl for Sql Server:  

**Blog:**
```sql
CREATE TABLE [Blog] (
    [Id] int NOT NULL IDENTITY,
    CONSTRAINT [PK_Blog] PRIMARY KEY ([Id])
);
```
**Post:**
```sql
CREATE TABLE [Posts] (
    [Id] int NOT NULL IDENTITY,
    [BlogId] int NOT NULL,
    CONSTRAINT [PK_Posts] PRIMARY KEY ([Id]),
    CONSTRAINT [FK_Posts_Blog_BlogId] FOREIGN KEY ([BlogId]) REFERENCES [Blog] ([Id]) ON DELETE CASCADE
);
```
**Index:**
```sql
 CREATE INDEX [IX_Posts_BlogId] ON [Posts] ([BlogId]);
```
*Note:* No other mappings were added.
