## Culled from an Udemy class entitled "Build An App With ASPNET Core And Angular From Scratch."
### By Neil Cummings.

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
- Security:
    ```javascript
        dotnet ef migrations add UserEntity
    ```
    - No project was found. Change the current working directory or use the --project option.
    ```javascript
        dotnet ef migrations add ux --project "C:\ODonnchadha\ng-net-core-ef-dating-app-server\app.api"
        dotnet ef database update --project "C:\ODonnchadha\ng-net-core-ef-dating-app-server\app.api"
    ```
    - And ensure the following within the stupid project file. (Snippet:)
    ```xml
        <PackageReference Include=Microsoft.EntityFrameworkCore Version=3.1.7 />
        <PackageReference Include=Microsoft.EntityFrameworkCore.Design Version=3.1.7 />
        <PackageReference Include=Microsoft.EntityFrameworkCore.SqlServer Version=3.1.7 />
        <PackageReference Include=Microsoft.EntityFrameworkCore.Tools Version=3.1.7 />
    ```

      - How we store passwords in the database:
        - Hashing a password. One-way process. Using an algorithm to scramble the password.
        - Same password? Same hash. Beware: Precomputed decryption of the hash. Rainbow tables.
        - So we also add a salt. Randomly generated. Added to the hash.
        - And we will store as a byte[].
      - Associated User model:
      - The Repository Pattern:
        - EF already offers a level of abstraction. Why another?
        - a. Mediates between the data source and the business layer.
        - b. Queries the data, maps the data, and persists changes from entity to data source.
        - So: IIS/Kestrel | Controller | Repository Interface (& Concrete Implementation) | DbContext | EF | Database.
        - Why? 
            - Minimize duplicate query logic.
            - Decouples application from persistence framework.
            - All DB queries in the same place.
            - Promotes testability.
        - services.AddTransient: Lightweight stateless services. Created aech time requested.
      - The authentication controller:
      - DTOs:
        - If not: [ApiController()]
        - Then: Register([FromBody]UserForRegister userForRegister) & if (!ModelState.IsValid) return BadRequest();

      - Token authentication:
        - JSON Web Token. Industry standard. Self-contained. Credentials, Claims, Et al. JWT.
        - The server does not need to go back to the database in order to authenticate.
        - Structure: 
            - Header:
            ```javascript
                { "alg": "HS512", "typ": "JWT" }
            ```
            - Payload:
            ```javascript
                { "nameid": "8", "unique_name": "D", "nbf": 11578831001, "exp": 11578832001, "iat": 11578831001 }
            ```
            - Header and payload can be decoded by anyone. So do be careful.
            - Secret:
            ```javascript
                HMACSHA256(
                    base64UrlEncode(header + "." +
                    base64UrlEncod(payload),
                    secret
                 )
            ```
            - 1. Client sends username and password to the server.
            - 2. Password hashed on the server and compared to the database. 
            - 3. Server will create a token and send to the client, which stores the thing locally.
            - 4. With future requests, JWT is sent to the server.
            - 5. And the server validates the JWT and sends back a response.

      - Authentication middleware:
        - Pipeline code. NOTE: With HTTPS and certificates, we become more secure, rather than Postman and plain text.
        - Recommended to place appsettings.json within .gitignore file due to the 'secret.'
        - In production, we can use environment variables.
        - Another option is for use to enable secret storage.

    - Extending the API:
        - Entending the user class:
        - Migrations:
        - Cascade Delete:
        - Seeding data into our database:
        - Using Automapper.