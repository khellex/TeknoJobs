# 🚀 TeknoJobs

**TeknoJobs** is a modular, cleanly architected **.NET 8 Web API** project for managing job listings, departments, and locations. It is built following **Clean Architecture** principles to ensure maintainability, testability, and scalability.

---

## 🔧 Tech Stack

- **.NET 8**
- **ASP.NET Core Web API**
- **Entity Framework Core**
- **JWT Authentication**
- **AutoMapper**
- **SQL Server**
- **Swagger / OpenAPI**

### 🧱 Clean Architecture Layers

- **Domain** — Entities and domain interfaces  
- **Application** — DTOs, services, interfaces, business logic  
- **Infrastructure** — EF Core, repositories, database context, authentication  
- **Web** — API controllers, middleware, Swagger setup  

---

## 📁 Project Structure

```
TeknoJobs/
│
├── TeknoJobs.Domain/         # Core entities and domain interfaces
├── TeknoJobs.Application/    # DTOs, service contracts, mapping profiles
├── TeknoJobs.Infrastructure/ # EF Core setup, repositories, auth
└── TeknoJobs.Web/            # Web API layer, controllers, program configuration
```

---

## 🛠️ Setup Instructions

### 1. Configure the Connection String

Open the file:

```
TeknoJobs.Web/appsettings.json
```

Update the `DefaultConnection` string with your local SQL Server details:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=YOUR_SERVER_NAME;Database=TeknoJobsDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=true"
}
```

> Replace `YOUR_SERVER_NAME` with your actual SQL Server name  
> (e.g., `localhost`, `DESKTOP-ABC123\SQLEXPRESS`).

---

### 2. Build the Project

Use Visual Studio or CLI:

```bash
dotnet build
```

---

### 3. Apply Database Migrations

1. Open **Tools > NuGet Package Manager > Package Manager Console**.
2. Set the **Default Project** to:

```
TeknoJobs.Infrastructure
```

3. Run the migration:

```powershell
Update-Database
```

Wait for the output `Done`. Your schema is now created and ready to use.

---

### 4. Run the Application

Set `TeknoJobs.Web` as the **Startup Project**.  
Run the application and navigate to:

```
https://localhost:<port>/swagger
```

Swagger UI will list all available endpoints and their documentation.

---

### 5. Authenticate to Use the APIs

Most APIs are **secured using JWT**.

- Use the `/api/v1/login` endpoint with the provided credentials.
- Generate a token.
- Click **Authorize** in Swagger and enter:

```
Bearer <your-token-here>
```

> After this, all authorized endpoints will be available to test via Swagger.

---

## ✅ Notes

- Make sure SQL Server is running before applying migrations.
- Authentication credentials are shared via email.
- For issues, feel free to raise a ticket.
