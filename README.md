# Authentication API

Este proyecto forma parte del backend de un sistema de **E-Commerce**.

La **Authentication API** permite a los usuarios **registrarse** e **iniciar sesión** de forma segura utilizando **tokens JWT**.

## Tecnologías utilizadas

- ASP.NET Core Web API
- Entity Framework Core
- Clean Architecture (Separación de capas: Entities, DTOs, Repository, Application)
- SQL Server (para persistencia de datos)
- JWT Bearer Authentication (generación y validación de tokens)
- Unit Testing con xUnit y FakeItEasy (pruebas con dependencias mockeadas)
- Ocelot API Gateway (para enrutamiento general de APIs en el proyecto completo)

## Funcionalidades principales

- Registro de nuevos usuarios
- Autenticación de usuarios mediante usuario y contraseña
- Generación de JSON Web Tokens (JWT)
- Seguridad basada en estándares modernos
- Validaciones y control de errores

