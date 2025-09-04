# ICT Dashboard Backend

This repository contains the backend service for the ICT Dashboard application. It is a RESTful API built with .NET 9 and ASP.NET Core, providing robust user authentication and profile management services. The application is designed to integrate with a PostgreSQL database and uses JWT for secure, cookie-based authentication.

## Features

-   **User Authentication:** Secure sign-up, sign-in, and sign-out functionality using hashed passwords.
-   **JWT & Cookie-Based Sessions:** Uses JSON Web Tokens (JWTs) stored in secure, `HttpOnly` cookies for managing user sessions.
-   **User Profiles:** Manages extended user profile data (bio, birthday, picture URL) with a one-to-one relationship to the user model.
-   **Role Management:** Supports distinct user roles, including `Member` and `Coach`.
-   **Database Integration:** Utilizes Entity Framework Core to interact with a PostgreSQL database.
-   **Database Migrations:** Includes a complete set of EF Core migrations to initialize and update the database schema.
-   **Docker Support:** Provides a `docker-compose.yml` file for easy setup of the PostgreSQL database in a container.
-   **Centralized Error Handling:** Implements custom middleware to catch exceptions and return consistent, structured JSON error responses.
-   **CORS Policy:** Pre-configured Cross-Origin Resource Sharing (CORS) policy to allow requests from a frontend application (defaults to `http://localhost:4200`).

## Technology Stack

-   **Framework:** .NET 9 / ASP.NET Core
-   **Database:** PostgreSQL
-   **ORM:** Entity Framework Core 9
-   **Authentication:** JWT (JSON Web Tokens)
-   **Containerization:** Docker & Docker Compose

## Getting Started

Follow these instructions to get the project up and running on your local machine for development and testing.

### Prerequisites

-   [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
-   [Docker](https://www.docker.com/products/docker-desktop/)

### Installation & Setup

1.  **Clone the repository:**
    ```sh
    git clone https://github.com/themane04/ict-dashboard-be.git
    cd ict-dashboard-be
    ```

2.  **Start the database:**
    Use Docker Compose to start the PostgreSQL database container. The necessary database, user, and password will be created automatically based on the `docker-compose.yml` file.
    ```sh
    docker-compose up -d
    ```

3.  **Apply Database Migrations:**
    Once the database container is running, apply the Entity Framework migrations to create the tables and relationships.
    ```sh
    dotnet ef database update
    ```
    The connection string in `appsettings.json` is already configured to connect to the Docker container.

4.  **Run the application:**
    ```sh
    dotnet run
    ```
    The API will be available at `http://localhost:5211`.

## API Endpoints

The primary API endpoints for authentication are handled by the `AuthController`.

| Method | Endpoint             | Description                                                   | Authentication |
| :----- | :------------------- | :------------------------------------------------------------ | :------------- |
| `POST` | `/api/auth/signup`   | Registers a new user.                                         | None           |
| `POST` | `/api/auth/signin`   | Authenticates a user and returns an `access_token` in a cookie. | None           |
| `POST` | `/api/auth/signout`  | Clears the `access_token` cookie to log the user out.         | Required       |
| `GET`  | `/api/auth/me`       | Retrieves the profile of the currently authenticated user.      | Required       |

### Request/Response Examples

-   **Sign Up (`POST /api/auth/signup`)**
    ```json
    {
      "firstName": "John",
      "lastName": "Doe",
      "username": "johndoe",
      "email": "john.doe@example.com",
      "role": "Member",
      "password": "yourstrongpassword",
      "confirmPassword": "yourstrongpassword"
    }
    ```

-   **Sign In (`POST /api/auth/signin`)**
    ```json
    {
      "email": "john.doe@example.com",
      "password": "yourstrongpassword"
    }
    ```

-   **Get Me (`GET /api/auth/me`) Response**
    ```json
    {
      "id": 1,
      "firstName": "John",
      "lastName": "Doe",
      "username": "johndoe",
      "email": "john.doe@example.com",
      "role": "Member",
      "profile": {
        "pictureUrl": null,
        "birthday": null,
        "bio": null
      }
    }
    ```

## Project Structure

The project is organized into modules to separate concerns:

-   `Auth/`: Contains all logic related to authentication, including controllers, DTOs, services, and helpers for JWT generation and password hashing.
-   `Profile/`: Defines data models and DTOs for user profiles.
-   `Core/`: Holds shared application components, such as the `IctDbContext` for Entity Framework, dependency injection configurations (`ServiceCollectionExtensions`), and custom middleware for exception handling.
-   `Migrations/`: Contains database migration files generated by EF Core, tracking changes to the data model over time.
-   `Program.cs`: The application's entry point, where services are registered and the HTTP request pipeline is configured.
-   `docker-compose.yml`: Defines the PostgreSQL database service for local development.
