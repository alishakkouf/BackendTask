# ToDoTask API

A secure task management API with user roles and permissions built with .NET 8.

## Project description

1. check connection string and run:
Access Swagger UI

Login with these test users:

Admin:	user@Admin.org	P@ssw0rd
Guest:	user@guest.org	P@ssw0rd

Key Features
Create/read/update/delete tasks
Create/Update/GetAll/Login Users

JWT authentication

Two user roles (Admin/Guest)

Permission-based authorization

Automatic database seeding

Logging for all requests using serilog

Swagger documentation

Database Setup
Update appsettings.json with your SQL Server connection

Permissions
Admin can: 

View all tasks

Delete any task

Manage users

Guest can:

View tasks

Create their tasks
