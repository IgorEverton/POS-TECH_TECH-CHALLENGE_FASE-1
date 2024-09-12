#!/bin/bash
# Wait for SQL Server to start
sleep 30s

# Run the setup script to create the DB and the schema in the DB
/opt/mssql-tools/bin/sqlcmd -S localhost -U sa -P $SQLSERVER_SA_PASSWORD -Q "CREATE DATABASE [$SQLSERVER_DB]"