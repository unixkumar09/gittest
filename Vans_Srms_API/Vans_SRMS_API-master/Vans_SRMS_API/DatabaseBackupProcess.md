Create a SQL dump of entire database:
```
pg_dump -Fc SRMS > file.dump
```

Drop and restore the database:
```
dropdb SRMS
pg_restore -C -d postgres file.dump
```

Note: The -C option signifies that we want to create a new database instead of restoring to an existing one.  
The `-d postgres` option does not restore to the postgres database, it just uses the existing postgres database to issue the new `CREATE DATABASE SRMS` command.