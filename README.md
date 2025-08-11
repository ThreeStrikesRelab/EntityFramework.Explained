# EntityFramework.Explained
> Because We Need to Talk About Kevin.
#### Dependencies
* [QuickPulse.Explains](https://github.com/kilfour/QuickPulse.Explains)
* [QuickPulse.Show](https://github.com/kilfour/QuickPulse.Show)
## Runtime Behaviour
## Schema
### Data Annotations
#### Sql Server - data annotations
looking for differences when using data annotations
#### Sqlite - data annotations
looking for differences when using data annotations
### Default String Length
### Column Attributes Overrides
#### Sql Server
Generates table with chosen column names of the right type in the given order.
#### Sqlite
Generates table with chosen column names of the right type (look at date :O) in the given order.
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
### Bi Directional One To Many With Two Db Sets
Because the entity used in the `DbSet` has a collection of another entity type, the latter are mapped as well.
#### Sql Server - bidirectional
EF infers and includes related entities in bidirectional relationship in the schema even when only one side is explicitly registered in the `DbContext`.
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
