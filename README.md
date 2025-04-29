# Todo List Application

A comprehensive ASP.NET Core application for managing to-do lists and tasks with a user-friendly interface and RESTful API backend.

## Overview

This Todo List Application is a full-stack web application built using ASP.NET Core 6.0 that allows users to create, manage, and organize their tasks in customizable to-do lists. The application follows a modern architecture with separate frontend and backend components for scalability and maintainability.

## Architecture

The solution consists of two main applications:

- **TodoListApp.WebApp**: An ASP.NET Core MVC application providing the user interface
- **TodoListApp.WebApi**: A RESTful Web API application that handles data operations

The system follows a layered architecture with clear separation of concerns:
- Presentation Layer (Web UI)
- Service Layer (APIs and business logic)
- Data Access Layer (Entity Framework Core)

## Technologies

- **.NET 6**: Modern, cross-platform framework for building web applications
- **ASP.NET Core MVC**: For building the user interface
- **ASP.NET Core Web API**: For the RESTful backend services
- **Entity Framework Core**: ORM for database operations using a code-first approach
- **SQL Server**: Database for storing application data
- **ASP.NET Core Identity**: For user authentication and authorization
- **JSON Web Tokens (JWT)**: For securing the API with bearer authentication

## Features

- **To-do List Management**:
  - View all to-do lists
  - Create new to-do lists
  - Edit existing to-do lists
  - Delete to-do lists

- **Task Management**:
  - View tasks within a list
  - View detailed task information
  - Add new tasks to lists
  - Edit task details
  - Mark tasks as completed
  - Delete tasks

- **User Authentication**:
  - User registration
  - User login/logout
  - User profile management
  - Access control to user-specific lists and tasks

- **Responsive UI**:
  - Modern, intuitive user interface
  - Mobile-friendly design

## Getting Started

### Prerequisites

- [.NET 6 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads) (or SQL Server Express)
- [Visual Studio 2022](https://visualstudio.microsoft.com/vs/) (recommended) or any other code editor

### Installation

1. Clone the repository:
   

git clone https://github.com/yourusername/todo-list-app.git
cd todo-list-app


2. Set up the database connection string in the `appsettings.json` file of the TodoListApp.WebApi project.

3. Apply database migrations:

cd TodoListApp.WebApi
dotnet ef database update


4. Run the Web API project:

dotnet run


5. In another terminal, run the Web Application:

cd ../TodoListApp.WebApp
dotnet run



6. Navigate to `https://localhost:5001` in your browser to use the application.

## Project Structure

- **TodoListApp.WebApp**: Frontend MVC application
  - Controllers: Handles HTTP requests and user interactions
  - Views: Razor pages for rendering UI
  - Models: Data models for the views
  - Services: Client services for API communication

- **TodoListApp.WebApi**: Backend RESTful API
  - Controllers: API endpoints for CRUD operations
  - Models: Request and response models
  - Services: Business logic implementation
  - Data: Entity Framework context and entity models

## Development Guidelines

- Follow the RESTful API principles for all API endpoints
- Implement proper error handling with meaningful HTTP status codes
- Use dependency injection for service resolution
- Document public classes and methods with XML comments
- Follow code quality standards enforced by StyleCop and .NET analyzers
- Use Entity Framework migrations for database schema updates

## Code Quality

The project enforces high code quality standards through:
- .NET code analyzers
- StyleCop for style consistency
- XML documentation requirements
- Adherence to framework design guidelines
