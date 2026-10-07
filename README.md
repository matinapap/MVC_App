# Phone Service Management System

A web application for managing a mobile phone service provider, built with **ASP.NET Core MVC (.NET 8)**, **Entity Framework Core** and **SQL Server**.

The app supports three user roles — **Clients**, **Sellers** and **Admins** — each with its own set of features, covering customer accounts, calling plans, call history and billing.

> The user interface is in Greek.

## Features

| Role | What they can do |
| --- | --- |
| **Client** | View their account details, browse their call history, view and pay their bills |
| **Seller** | Register new clients, issue a client's bill, change a client's calling plan |
| **Admin** | Create new sellers, create new calling plans, edit existing plans (benefits, monthly charge) |

Other highlights:

- **Role-based access** using server-side sessions: each controller action checks the logged-in user's role and redirects unauthorized users to the login page
- **Relational data model** with one-to-many and many-to-many relationships (bills ↔ calls) mapped with EF Core data annotations and the Fluent API
- **Code-first migrations** that create the whole database schema in one step
- **CRUD views** with Razor and Bootstrap 5, including client-side validation

## Tech stack

- C# / .NET 8
- ASP.NET Core MVC, Razor views
- Entity Framework Core 9 (SQL Server provider)
- Microsoft SQL Server
- Bootstrap 5, jQuery

## Data model

```
Users ──┬── Clients ── (phone number) ── Phones ── Programms (calling plans)
        ├── Sellers                        │
        └── Admin                        Bills ── BillsCalls ── Calls
```

- A **User** is a Client, a Seller or an Admin.
- Each **Phone** number is subscribed to one **Programm** (calling plan).
- **Bills** belong to a phone number and are linked to **Calls** through the `BillsCalls` join table.

## Project structure

```
MVC_App/
├── Controllers/      # One controller per entity, plus HomeController (login/logout)
├── Models/           # EF Core entities and the MVCApp DbContext
├── Views/            # Razor views per controller and the shared layout
├── Migrations/       # EF Core migration that creates the schema
└── wwwroot/          # Static files (CSS, JS, Bootstrap, jQuery)
Documentation/
├── EgxeiridioXristi.pdf    # User manual (Greek)
└── TexnikoEgxiridio.pdf    # Technical manual (Greek)
```

## Getting started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- SQL Server: LocalDB (included with Visual Studio on Windows), or SQL Server in Docker on macOS/Linux
- EF Core CLI: `dotnet tool install --global dotnet-ef`

### 1. Clone the repository

```bash
git clone https://github.com/matinapap/MVC_App.git
cd MVC_App/MVC_App
```

### 2. Configure the database connection

The default connection string in `appsettings.json` uses SQL Server LocalDB, so no change is needed on Windows.

On macOS/Linux, start SQL Server in Docker and point the app at it with an environment variable:

```bash
docker run -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=YourStrong!Passw0rd" \
  -p 1433:1433 -d mcr.microsoft.com/mssql/server:2022-latest

export ConnectionStrings__DefaultConnection="Server=localhost,1433;Database=PhoneServiceDB;User Id=sa;Password=YourStrong!Passw0rd;TrustServerCertificate=True;MultipleActiveResultSets=true"
```

### 3. Create the database

```bash
dotnet ef database update
```

### 4. Run the app

```bash
dotnet run
```

Then open http://localhost:5068.

### 5. Add sample users

The database starts empty. To log in, add a user and give them a role, for example:

```sql
INSERT INTO Programms (ProgrammName, Benfits, Charge) VALUES ('Basic', '100 minutes, 1GB data', 15.00);
INSERT INTO Phones (PhoneNumber, ProgrammName) VALUES ('6900000001', 'Basic');

INSERT INTO Users (First_Name, Last_Name, Username) VALUES ('Admin', 'User', 'admin');    -- User_id 1
INSERT INTO Users (First_Name, Last_Name, Username) VALUES ('Seller', 'User', 'seller');  -- User_id 2
INSERT INTO Users (First_Name, Last_Name, Username) VALUES ('Client', 'User', 'client');  -- User_id 3

INSERT INTO Admin (User_id) VALUES (1);
INSERT INTO Sellers (User_id) VALUES (2);
INSERT INTO Clients (AFM, phoneNumber, User_id) VALUES ('123456789', '6900000001', 3);
```

On the login page, sign in with the **username** and the **user ID** (e.g. `admin` / `1`).

## Notes

This was built as an academic project to practise the MVC pattern, EF Core and relational database design. Authentication is deliberately simple (username + user ID stored in the session). A production version would use ASP.NET Core Identity with hashed passwords and `[Authorize]` role policies.

## Documentation

Full user and technical manuals (in Greek) are in the [`Documentation`](Documentation/) folder.

## Author

**Matina Papadakou** — [GitHub](https://github.com/matinapap)
