# EntityFramework.Explained
> Because We Need to Talk About Kevin.
#### Dependencies
* [QuickPulse.Explains](https://github.com/kilfour/QuickPulse.Explains)
* [QuickPulse.Show](https://github.com/kilfour/QuickPulse.Show)
## Runtime Behaviour
## Schema
### Column Attributes Overrides
#### Sql Server
Generates table with chosen column names of the right type in the given order.
#### Sqlite
Generates table with chosen column names of the right type (look at date :O) in the given order.
### Data Annotations: `[Range(...)]`
**Given:**
```csharp
public class Thing
{
    public int Id { get; set; }
    [Range(0, 10)] // <= We are checking this 
    public int SecondInt { get; set; }
}
```
#### Sql Server
`[Range(0,10)]` gets ignored : `[SecondInt] int NOT NULL`.
#### Sqlite
Same behaviour as Sql Server: `"SecondInt" INTEGER NOT NULL`.
### Default Index Names
#### Sql Server
has entity with a default index
#### Sql Server
has entity with combined sorted index
#### Sqlite
Generates `TEXT`.
### Table Per Hierarchy
#### Sql Server
has tph with AnimalType as discriminator and inherited properties of base class and properties of derived classes.
#### Sqlite
has tph with AnimalType as discriminator and inherited properties of base class and properties of derived classes
=======
has entity with a default index
#### Sqlite
has entity with combined sorted index
### Generic Identity
#### Sql Server - Generic Identity
Using a Generic Identity without mapping it in DbContext throws an `InvalidOperationException`.
#### Sqlite - Generic Identity
Same behaviour as Sql Server
#### Sql Server - Generic Identity
Successfully generates database for Generic Identity with mapping
#### Sqlite - Generic Identity
Successfully generates database for Generic Identity with mapping
### Required Attributes
#### Sql Server
Generates required properties that will be created even if they are null or empty.
#### Sqlite
Generates required properties that will be created even if they are null or empty.
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
