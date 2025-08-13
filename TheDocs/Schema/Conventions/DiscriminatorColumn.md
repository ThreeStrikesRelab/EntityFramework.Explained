# Discriminator Column
## Sql Server
has tph with AnimalType as discriminator and inherited properties of base class and properties of derived classes. Property with type bool will generate an int in the database in SQL Server (output 0 or 1). So we used type string instead.  
## Sqlite
has tph with AnimalType as discriminator and inherited properties of base class and properties of derived classes  
