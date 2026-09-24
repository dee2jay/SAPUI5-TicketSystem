# Docker deployment

## Stack
- ASP.NET Core 9 API
- SQL Server 2022 Express
- MongoDB 8
- Nginx reverse proxy

## Start
Copy `.env.example` to `.env`, set strong secrets, then run:

```bash
docker compose up -d --build
```

Health check:

```
http://localhost:8080/health
```

Logs:

```bash
docker compose logs -f api
```

Data is persisted in Docker volumes. Never commit the real `.env` file.

The mobile PWA should use the same public origin in production and call the API through `/api`.
