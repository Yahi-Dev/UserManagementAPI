# User Management System

Este sistema de gestión de usuarios permite crear y administrar usuarios y sus teléfonos asociados a través de una API RESTful. La base de datos está diseñada para soportar operaciones esenciales como el registro de usuarios y la gestión de la información de contacto.

## Configuración de la Base de Datos

La base de datos `UserManagementDB` incluye dos tablas principales: `Users` y `Phones`. La tabla `Users` almacena la información del usuario, mientras que `Phones` guarda información de contacto relacionada. Se utiliza SQL Server como sistema de gestión de base de datos.

### Estructura de Tablas

- **Users**: Almacena los datos del usuario, incluyendo un identificador único, correo electrónico, hash de la contraseña, y fechas de creación, modificación y último acceso.
- **Phones**: Almacena los números de teléfono asociados a cada usuario junto con los códigos de ciudad y país.

### Creación de la Base de Datos y Tablas

```sql
CREATE DATABASE UserManagementDB;
USE UserManagementDB;

-- Creación de la tabla Users
CREATE TABLE Users(
    UserID VARCHAR(50) NOT NULL,
    NameUser VARCHAR(MAX) NOT NULL,
    Email VARCHAR(100) NOT NULL,
    PasswordHash VARCHAR(200) NOT NULL,
    IsActive bit,
    Created DATETIME2(7),
    Modified DATETIME2(7),
    Last_login DATETIME2(7),
    PRIMARY KEY (UserID),
    UNIQUE (Email)
);

-- Creación de la tabla Phones
CREATE TABLE Phones(
    PhoneID INT IDENTITY(1,1) PRIMARY KEY,
    UserID VARCHAR(50) NOT NULL,
    Number VARCHAR(20) NOT NULL,
    City_Code VARCHAR(MAX),
    Country_Code VARCHAR(MAX)
);

-- Establecimiento de la relación de clave foránea
ALTER TABLE Phones 
ADD CONSTRAINT FK_Phones_UsuariosID
FOREIGN KEY (UserID) REFERENCES Users(UserID);
```

## Configuración del Proyecto

- **ConnectionStrings**: Asegúrese de actualizar el `Server` en la cadena de conexión con el nombre del servidor donde se ejecutará la aplicación.

```json
"ConnectionStrings": {
  "Default": "Server=MI_SERVIDOR\SQLEXPRESS;Database=UserManagementDB;Trusted_Connection=True;MultipleActiveResultSets=True;TrustServerCertificate=True"
}
```
