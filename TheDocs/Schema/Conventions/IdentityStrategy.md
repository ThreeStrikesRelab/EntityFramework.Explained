# Identity Strategy
## Sql Server
throws InvalidOperationException if primary key is not defined or there is no property with 'id' in name  
## Sql Server
generates an id as a column. It also has a Primary Key constraint (id can't be null and has to be unique per record) in table when primary key is defined of right type  
## Sql Server
doesn't need a pk if data annotation says so ([Keyless]). You cannot use CRUD on this table.  
## Sqlite
generates an id as a column. It also has a Primary Key constraint (id can't be null and has to be unique per record) in table when primary key is defined of right type  
## Sqlite
only needs an Id for entities, not for valuetypes inside an entity.  
