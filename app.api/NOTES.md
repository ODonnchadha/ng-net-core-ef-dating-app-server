# Culled from an Udemy class entitled "Build An App With ASPNET Core And Angular From Scratch."
# By Neil Cummings.

- Introduction:
    - Tools and technologies:
        - Angular v8.
        - Microsoft ASP.NET Core v2.2.
        - Microsoft Entity Framework v2.2.
        - Supporting cast:
            - HTML5.
            - CSS3.
            - Node.js.
            - DB Browser for SQLite.
            - git.
            - Bootstrap.
            - Postman.
    - Environment setup:
        - SDK .NET Core 2.2
        ```javascript
            dotnet --info
            node --version
            npm --version
        ```
- Building a Walking Skeleton:
    1. A tiny implementation of the system that performs a small end-to-end function.
        - DB - ORM - API - SPA
    2. Creating the .NET Core API using the CLI.
    ```javascript
        dotnet new webapi -h
        dotnet new webapi -n app.api
    ```
    3. Reviewing the project files:
        - Suggested extensions:
            - C# for Visual Studio Code (powered by OmniSharp).
            - C# IDE Extensions for VSCode.
            - NuGet Package Manager.
            - View - Command Palette...
                - "Generate Assets for Build and Debug"
        - .Net Core API files.
    4. Running the .NET Core application.
        - Within Properties/launchSettings.json:
            - Remove HTTPS from the applicationUrl.
            - Set launchBrowser to false.
            ```javascript
                dotnet run
            ```
        - http://localhost:5000/weatherforecast
    5. Environment Settings:
        ```javascript
            dotnet watch run
        ```
    6. First model and data context:
        - Create model/entity and data context.
        ```csharp
            public class DataContext : DbContext
        ```
    7. Configuration and EF:
    ```javascript
        dotnet tool install --global dotnet-ef
    ```
    8. Via Package Manager Console:
    - I cheated. I could not get SqlLite working.
    ```javascript
        dotnet ef migrations add InitialCreate
        dotnet ef database update
    ```