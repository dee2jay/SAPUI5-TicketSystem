# GHCR + VPS Deployment

The production deployment uses:

GitHub Actions -> GitHub Container Registry (GHCR) -> VPS -> Docker Compose

## 1. Prepare the VPS

Install Docker Engine and Git. Create an application directory, for example:

```bash
sudo mkdir -p /opt/ticketsystem
sudo chown $USER:$USER /opt/ticketsystem
cd /opt/ticketsystem
git clone https://github.com/dee2jay/SAPUI5-TicketSystem.git .
```

Create the production environment file:

```bash
cp .env.prod.example .env
nano .env
```

Set strong values for `SQLSERVER_SA_PASSWORD` and `JWT_SECRET_KEY`. Set `CORS_ALLOWED_ORIGINS` to the public HTTPS origin of the application.

## 2. Configure GitHub Actions secrets

Repository -> Settings -> Secrets and variables -> Actions:

- `VPS_HOST`: public DNS name or IP of the VPS
- `VPS_USER`: deployment SSH user
- `VPS_SSH_KEY`: private SSH key used by GitHub Actions
- `VPS_APP_DIR`: e.g. `/opt/ticketsystem`
- `GHCR_USERNAME`: GitHub username
- `GHCR_TOKEN`: GitHub token/PAT with package read access

Do not commit the private SSH key, GHCR token, or production `.env`.

## 3. Deployment

Every push to `master`:

1. Builds the API image.
2. Builds the mobile image.
3. Publishes both images to GHCR.
4. Connects to the VPS over SSH.
5. Updates the repository to `master`.
6. Pulls the new images.
7. Restarts/updates the Compose stack.
8. Removes unused Docker images.

The same workflow can also be started manually with **Run workflow**.

## 4. Verify

On the VPS:

```bash
cd /opt/ticketsystem
docker compose -f docker-compose.prod.yml ps
curl http://127.0.0.1/health
```

The current Nginx configuration exposes HTTP on port 80. For a public production deployment, put HTTPS/Let's Encrypt in front of the stack before exposing the application to users.
