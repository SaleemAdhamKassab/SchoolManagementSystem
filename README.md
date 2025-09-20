# School Management System API 🏫

A RESTful API for managing a school's core functions, including user authentication, course management, and assignments. Built with ASP.NET Core, following a **Clean Architecture** approach.

## Architecture

This project is structured using **Clean Architecture** principles to separate concerns and ensure maintainability.

* **`SchoolManagementSystem.API`**: The presentation layer. It contains the web API controllers, middleware, and startup configuration.
* **`SchoolManagementSystem.Application`**: The application layer. It contains the core business logic, services, and DTOs.
* **`SchoolManagementSystem.Core`**: The domain layer. It holds the core entities and interfaces.
* **`SchoolManagementSystem.Infrastructure`**: The infrastructure layer. It handles external dependencies, such as database access, repository implementations, and migrations.

## Features

* **User Management**: `Admin`, `Teacher`, and `Student` roles.
* **Authentication**: JWT-based authentication.
* **Course Management**: Teachers can create courses; students can view their enrolled courses.
* **Assignments**: Teachers can create assignments linked to a course.
* **Submissions**: Students can submit assignments.
* **Grading**: Teachers can grade submitted assignments.
* **Logging**: Structured logging with **Serilog**.
* **Testing**: Basic unit tests for the application's core logic.

---

## Getting Started

### Prerequisites

* [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
* [SQL Server](https://www.microsoft.com/sql-server/sql-server-downloads) (or Docker container)
* A code editor like [Visual Studio](https://visualstudio.microsoft.com/vs/) or [Visual Studio Code](https://code.visualstudio.com/)

---

### Setup Instructions

1.  **Clone the repository:**

    ```bash
    git clone [https://github.com/SaleemAdhamKassab/SchoolManagementSystem.git](https://github.com/SaleemAdhamKassab/SchoolManagementSystem.git)
    cd SchoolManagementSystem
    ```

2.  **Configure the Database Connection:**
    * Open `appsettings.json` in the `SchoolManagementSystem.API` project.
    * Update the `DefaultConnection` string to point to your SQL Server instance.


    ```json
    "ConnectionStrings": {
      "DefaultConnection": "Server=localhost;Database=SchoolManagementSystemDb;Trusted_Connection=True;TrustServerCertificate=True"
    }
    ```

3.  **Run Database Migrations:**
    * Migrations are located in the `SchoolManagementSystem.Infrastructure` project. You must run the `dotnet ef` command from the API project while specifying the infrastructure project.
    * Open a terminal in the `SchoolManagementSystem.API` directory and run:

    ```bash
    dotnet ef database update --project ..\SchoolManagementSystem.Infrastructure
    ```

4.  **Database Initialization (Seeding):**
    * This project uses a database seeder to automatically create an initial **Admin** user when the application starts.
    * No manual steps are required for seeding. The `DbSeeder` class in the `SchoolManagementSystem.Infrastructure` project handles this automatically upon startup.

5.  **Run the Application:**

    ```bash
    dotnet run --project SchoolManagementSystem.API
    ```

The application will start, and the Swagger UI will automatically open in your browser, ready for use.

---

## Contact

For any questions or support, please don't hesitate to contact me:

**Saleem Kassab**
**Email:** saleemkassab5@gmail.com
**Phone:** 00963 959 270 763

Or you can open an issue in the repository.
