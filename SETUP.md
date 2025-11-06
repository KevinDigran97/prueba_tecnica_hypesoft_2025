# Setup Instructions

## Quick Start

### 1. Clone and Configure

```bash
# Clone the repository
git clone <repository-url>
cd Desafio-Tecnico-Hypesoft

# Copy example configuration files
cp backend/src/Hypesoft.API/appsettings.Example.json backend/src/Hypesoft.API/appsettings.Development.json
cp docker-compose.example.yml docker-compose.yml
```

### 2. Update Configuration

Edit `backend/src/Hypesoft.API/appsettings.Development.json` and replace:
- `YOUR_CLIENT_SECRET_HERE` with your actual Keycloak client secret
- Update MongoDB connection string if needed
- Update Keycloak settings if different

### 3. Run with Docker

```bash
# Create .env file (optional, for Docker)
cat > .env << EOF
KEYCLOAK_ADMIN=admin
KEYCLOAK_ADMIN_PASSWORD=your_secure_password
KEYCLOAK_CLIENT_SECRET=your_client_secret
MONGODB_CONNECTION_STRING=mongodb://mongo:27017
EOF

# Start services
docker-compose up -d
```

### 4. Run Directly (without Docker)

```bash
# Ensure MongoDB is running on localhost:27017
# Ensure Keycloak is running on localhost:8080 (optional)

# Run the API
cd backend/src/Hypesoft.API
dotnet run
```

## Important Notes

- **Never commit** `appsettings.Development.json` or `.env` files
- **Use environment variables** for production
- **Change default passwords** before deploying
- See `README.SECURITY.md` for security best practices

## Git Configuration

The repository uses `.gitattributes` to ensure consistent line endings (LF). If you see warnings about CRLF, run:

```bash
git config core.autocrlf false
git config core.eol lf
```

