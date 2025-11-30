# InnoShop: A Microservices-Based application

## About The Project

This project serves as a practical example of building a distributed system with modern technologies. The services communicate with each other via RESTful APIs. The solution is structured following **Clean Architecture** principles.  It consists of a frontend application and two primary microservices.

## Features
### ⚛️ Frontend
- React + Vite: Fast and modern frontend tooling.
- React Router: Handles client-side routing.
- Axios: Manages API requests.
- MUI Components: Provides styled UI components.

### 👤 User Management Service
- ASP.NET Core Identity: Manages user authentication and authorization.
- MSSQL: Used as the relational database.
- JWT Authentication: Secure access using JSON Web Tokens.
- Account Management: Email-based verification for new accounts and password reset capabilities. (Note: Email dispatch functionality is currently not implemented).
- Cascading Logic: Deactivating a user automatically hides all associated products, utilizing a soft-delete mechanism to allow for easy reactivation.

### 📦 Product Management Service
- CRUD Operations: Endpoints for creating, reading, updating, and deleting products.
- PostgreSQL: Used as the database.
- Ownership Logic: Users can only create, edit, and delete their own products.
- Data Validation: Implemented using FluentValidation.

### 🧪 Test Projects
- Unit Tests: Verify individual components and business logic in isolation.
- Integration Tests: Validate interactions between components, including database access and API endpoints within each microservice.

## Installation

1.  Clone the repository:
    ```sh
    git clone https://github.com/darkbgit/InnoShop.git
    cd InnoShop
    ```

2.  Build and run the application with Docker Compose:
    ```sh
    docker-compose up --build
    ```
    
3.  To run the tests execute:
    ```sh
    dotnet test
