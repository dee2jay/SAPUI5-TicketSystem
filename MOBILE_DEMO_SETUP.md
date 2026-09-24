# Mobile API Integration – Development Setup

## 1. Prerequisites

- .NET 9 SDK
- SQL Server Express / LocalDB as configured in `appsettings.Development.json`
- MongoDB for application logging
- A test user in the TicketSystem database

## 2. Configure the JWT secret locally

The JWT secret is intentionally not stored in Git.

From the repository root:

```powershell
dotnet user-secrets --project .\TicketSystem.API\TicketManagementSystem.API.csproj set "Jwt:SecretKey" "<GENERATE-A-LONG-RANDOM-SECRET>"
```

You can generate a suitable value with PowerShell:

```powershell
[Convert]::ToBase64String((1..48 | ForEach-Object { Get-Random -Maximum 256 }))
```

Also verify:

- `Jwt:Issuer = TicketManagementAPI`
- `Jwt:Audience = TicketManagementAPI`

## 3. Start the API

```powershell
dotnet run --project .\TicketSystem.API --launch-profile https
```

Expected local API:

- HTTPS: `https://localhost:7187`
- Health: `https://localhost:7187/health`
- Swagger (Development): `https://localhost:7187/`

## 4. Mobile PoC

The public PoC is in the separate repository:

`dee2jay/SAPUI5-TicketSystem-Mobile-PoC`

For local development, serve the static files through an HTTP server instead of opening `index.html` directly.

The PoC defaults to:

`https://localhost:7187`

The API URL can also be changed on the login screen.

## 5. End-to-end demo

1. Start SQL Server and MongoDB.
2. Start the API.
3. Open Swagger and verify `/health`.
4. Open the Mobile PoC.
5. Login with a test account.
6. Verify the ticket list.
7. Create a ticket.
8. Take/select one or more photos.
9. Submit the ticket.
10. Verify the ticket and uploaded attachments.
11. Open the dashboard and show the resulting ticket.
12. Logout and verify that protected API calls require authentication.

## 6. GitHub Pages

GitHub Pages must be enabled once in the PoC repository:

Settings -> Pages -> Build and deployment -> Source -> GitHub Actions

The Pages workflow then deploys the `main` branch.

Important: a GitHub Pages frontend cannot access `https://localhost:7187` from a smartphone. For a real remote/mobile presentation, deploy the API to a public HTTPS test environment or use a secure temporary tunnel. Do not expose the development database or development secrets.

## 7. Before public presentation

At minimum:

- rotate the previously committed JWT secret
- use a production secret outside Git
- restrict CORS to the actual frontend origin
- enforce object-level authorization for tickets and attachments
- validate upload size and MIME/type
- add API integration tests
- use safe exception responses
- avoid storing long-lived JWTs in localStorage for production
