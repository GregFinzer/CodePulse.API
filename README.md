# CodePulse.API
API for learning Angular

From this course:  https://www.udemy.com/course/real-world-app-angular-aspnet-core-web-api-and-sql/

## Development Environment Setup for the API
### Install .NET 7 SDK
https://dotnet.microsoft.com/en-us/download

Or Install with Chocolatey

```dos
choco install dotnet-7.0-sdk
```

### Install Visual Studio 2022
https://visualstudio.microsoft.com

Choose workloads
* ASP.NET and Web Development
* Data Storage and Processing

Or Install with Chocolatey
```dos
choco install visualstudio2022-workload-netweb	
choco install visualstudio2022-workload-data
```

### Install SQL Server Developer Edition
https://www.microsoft.com/en-us/sql-server/sql-server-downloads

Choose Basic

### Install SQL Server Management Studio or Database .NET

**SQL Server Management Studio**

https://learn.microsoft.com/en-us/sql/ssms/download-sql-server-management-studio-ssms?view=sql-server-ver16

Or install with Chocolatey
```dos
choco install sql-server-management-studio
```

**Database.NET**

https://fishcodelib.com/database.htm

Or install with Chocolatey
```dos
choco install databasenet
```

## Preparing the database
1. In SSMS or Database.NET run the script that is in the path: scripts\seeding+blogs.sql
2. In Visual Studio Open the NuGet Package Manager Console.
3. Run the command:  `update-database -context "AuthDbContext"`
4. Run the command:  `update-database -context "ApplicationDbContext"`

## Run the application
Click the play button in Visual Studio to bring up the Swagger for the application.  The Get endpoints do not need authorization but the Post, Put, and Delete operations require an authorization token.  You can use postman to login and get the token and then use the Post, Put and Delete endpoints.  The admin user is:

User Name:  admin@codepulse.com
Password:  Password1!

