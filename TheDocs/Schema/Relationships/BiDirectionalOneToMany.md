# Bi Directional One To Many
Because the entity used in the `DbSet` has a collection of another entity type, the latter are mapped as well.  
## Sql Server - bidirectional
EF infers and includes related entities in bidirectional relationship in the schema even when only one side is explicitly registered in the `DbContext`.  
## Sql Server - bidirectional - reversed
EF infers and includes related entities in bidirectional relationship in the schema even when only one side is explicitly registered in the `DbContext`, but reversed this time. CREATE TABLE switches to different DBSets to create tables from.  
## Sqlite - bidirectional
Same behavior as SqlServer with bidirectional relationship, relationship discovery pulls in the `Blog` entity despite only registering `Post`  
