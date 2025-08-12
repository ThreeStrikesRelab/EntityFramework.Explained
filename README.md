# EntityFramework.Explained
> Because We Need to Talk About Kevin.
#### Dependencies
* [QuickPulse.Explains](https://github.com/kilfour/QuickPulse.Explains)
* [QuickPulse.Show](https://github.com/kilfour/QuickPulse.Show)
## Runtime Behaviour
### List Properties
EF Core does not detect in-place mutations to, for instance, a `List<string>` when only a value converter is used. The property reference remains unchanged, so change tracking is not triggered and `SaveChanges()` persists nothing.  
```csharp
var converter = new ValueConverter<List<string>, string>(
        v => string.Join(';', v),
        v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
    );
entityTypeBuilder.Property(c => c.StringListProperty)
    .HasConversion(converter);
```
Adding a `ValueComparer<List<string>>` to the mapping allows EF Core to detect in-place list mutations. The comparer inspects the list's contents, so `SaveChanges()` correctly persists changes without replacing the list instance.  
```csharp
var converter = new ValueConverter<List<string>, string>(
        v => string.Join(';', v),
        v => v.Split(';', StringSplitOptions.RemoveEmptyEntries).ToList()
    );
var comparer = new ValueComparer<List<string>>(
    (c1, c2) => c1!.SequenceEqual(c2!),
    c => c.Aggregate(0, (a, v) => HashCode.Combine(a, v.GetHashCode())),
    c => c.ToList()
);
entityTypeBuilder.Property(c => c.StringListProperty)
    .HasConversion(converter)
    .Metadata.SetValueComparer(comparer);
```
## Schema
### Default String Length
#### Sql Server
Generates `nvarchar(max)`.
#### Sqlite
Generates `TEXT`.
### Table Per Hierarchy
#### Sql Server
has tph with AnimalType as discriminator and inherited properties of base class and properties of derived classes.
#### Sqlite
has tph with AnimalType as discriminator and inherited properties of base class and properties of derived classes
### Class Nullability
#### Sql Server
`NullThing` Generates `NULL`.
`SomeThing` Generates `NOT NULL`.
`SomeThingId` adds `ON DELETE CASCADE` to the foreign key definition
`NullThing` does not add `ON DELETE CASCADE` to the foreign key definition
#### Sq Lite
`NullThing` Generates `NULL`.
`SomeThing` Generates `NOT NULL`.
`SomeThingId` adds `ON DELETE CASCADE` to the foreign key definition
`NullThing` does not add `ON DELETE CASCADE` to the foreign key definition
### Int Nullability
#### Sql Server
`int` Generates `int NOT NULL`.
`int?` Generates `int NULL`.
#### Sqlite
`int` Generates `INTEGER NOT NULL`.
`int?` Generates `INTEGER NULL`
### String Nullability
#### Sql Server
`string` Generates `NOT NULL`.
`string?` Generates `NULL`.
#### Sqlite
`string` Generates `NOT NULL`.
`string?` Generates `NULL`.
### Bi Directional One To Many
Because the entity used in the `DbSet` has a collection of another entity type, the latter are mapped as well.
#### Sql Server - bidirectional
EF infers and includes related entities in bidirectional relationship in the schema even when only one side is explicitly registered in the `DbContext`.
#### Sql Server - bidirectional - reversed
EF infers and includes related entities in bidirectional relationship in the schema even when only one side is explicitly registered in the `DbContext`, but reversed this time. CREATE TABLE switches to different DBSets to create tables from.
#### Sqlite - bidirectional
Same behavior as SqlServer with bidirectional relationship, relationship discovery pulls in the `Blog` entity despite only registering `Post`
### Uni Directional One To Many With One Db Set
Because the entity used in the `DbSet` has a collection of another entity type, the latter are mapped as well.
#### Sql Server
EF infers and includes related entities in the schema even when only one side is explicitly registered in the `DbContext`.
#### Sqlite
Same behavior as SqlServer, relationship discovery pulls in the `Blog` entity despite only registering `Post`
