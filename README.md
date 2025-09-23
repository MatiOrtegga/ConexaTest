# 🎬 ConexaApi

Backend service built with **.NET Core** to manage movies and synchronize with the [Star Wars API (SWAPI)](https://www.swapi.tech/).  
Provides **JWT authentication**, **role-based access control**, CRUD operations for movies, and optional synchronization with external sources.

---

## 📖 Overview
ConexaApi is designed as a practical exercise to demonstrate best practices in backend development using .NET Core.

Features:
- User authentication and authorization with JWT.
- Role-based access (Regular Users vs Administrators).
- Movie management endpoints (CRUD).
- Synchronization with SWAPI.
- Unit testing.  
---

## 🔑 Authentication
This API uses **JWT (JSON Web Tokens)**.  
Include the token in the `Authorization` header to access protected endpoints:
Authorization: Bearer <your_token>

Tokens are issued when logging in with valid credentials.

---

## 👥 User Roles
- **Regular User**
  - Register and log in.
  - Retrieve movie lists.
  - Retrieve details of a specific movie.

- **Administrator**
  - All Regular User permissions.
  - Create, update, and delete movies.
  - Trigger or schedule synchronization with SWAPI.

---

## 🚀 Endpoints

### Authentication & User Management
- `POST /api/users/register` → Register a new user.
- `POST /api/users/login` → Authenticate a user and receive a JWT.

### Movie Management
- `GET /api/movies` → Get all movies.
- `GET /api/movies/{id}` → Get details of a specific movie. *(Requires Regular User or Admin role)*  
- `POST /api/movies` → Create a new movie. *(Requires Admin role)*  
- `PUT /api/movies/{id}` → Update an existing movie. *(Requires Admin role)*  
- `DELETE /api/movies/{id}` → Delete a movie. *(Requires Admin role)*  

### External Synchronization
- `POST /api/movies/sync` → Synchronize movies from SWAPI into the local database. *(Requires Admin role)*  

---

## ⚙️ Setup & Running Locally

### Prerequisites
- .NET SDK 9.0 (or latest supported version).
- PostgreSQL or SQL Server.
- Git.

### Installation

Clone the repository:
```bash
git clone https://github.com/MatiOrtegga/ConexaTest.git
cd ConexaTest
```
Update the connection string in appsettings.json.

Apply migrations:
```bash
dotnet ef database update
```
Run the application:
```bash
dotnet run
```
---

## 🧪 Testing
Unit tests validate:
- Authentication and token validation.
- Access restrictions by role.
- Core business logic for movie management.
- Synchronization with SWAPI.

Run tests with:
```bash
dotnet test
```

To read more about the API specification docs: 👉 [ConexaApiSpecification](https://app.theneo.io/c57a68a3-a4b2-43bc-a23e-7b8315f3d085/conexaapi)
