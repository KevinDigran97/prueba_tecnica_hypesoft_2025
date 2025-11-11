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