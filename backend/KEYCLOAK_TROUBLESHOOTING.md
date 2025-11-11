# Guía de Solución de Problemas - Keycloak Authentication

## 🔍 Problemas Comunes y Soluciones

### 1. Error 401 Unauthorized

#### Causas posibles:

**A. Authority incorrecto**
- ❌ **Incorrecto**: `http://localhost:8080`
- ✅ **Correcto**: `http://localhost:8080/realms/hypesoft`

**Solución**: Verifica que `appsettings.Development.json` tenga:
```json
{
  "Keycloak": {
    "Authority": "http://localhost:8080/realms/hypesoft",
    "ClientId": "hypesoft-api",
    "ClientSecret": "eX3TUVacQlgldQ6FwqUsAqhxY2zrQ3as"
  }
}
```

**B. Audience no coincide**
- El token de Keycloak puede tener `aud: "account"` en lugar de `aud: "hypesoft-api"`
- **Solución temporal**: `ValidateAudience = false` (ya configurado)
- **Solución permanente**: Configurar Audience Mapper en Keycloak (ver más abajo)

**C. Token no enviado correctamente**
- Verifica que el header tenga el formato: `Authorization: Bearer <token>`
- El token no debe tener espacios extra
- El token no debe estar expirado

### 2. Probar la Autenticación

#### Paso 1: Obtener un token de Keycloak

```bash
# Para client_credentials (service-to-service)
curl -X POST http://localhost:8080/realms/hypesoft/protocol/openid-connect/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "client_id=hypesoft-api" \
  -d "client_secret=eX3TUVacQlgldQ6FwqUsAqhxY2zrQ3as" \
  -d "grant_type=client_credentials"
```

```bash
# Para password grant (usuario real)
curl -X POST http://localhost:8080/realms/hypesoft/protocol/openid-connect/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=password" \
  -d "client_id=hypesoft-client" \
  -d "username=admin" \
  -d "password=admin123"
```

#### Paso 2: Verificar el token en jwt.io

1. Copia el `access_token` de la respuesta
2. Ve a https://jwt.io
3. Pega el token
4. Verifica los campos:
   - `iss`: Debe ser `http://localhost:8080/realms/hypesoft`
   - `aud`: Puede ser `account`, `hypesoft-api`, o `hypesoft-client`
   - `exp`: Verifica que no esté expirado

#### Paso 3: Probar el endpoint protegido

```bash
# Reemplaza TU_ACCESS_TOKEN con el token obtenido
curl -X GET http://localhost:5000/api/products \
  -H "Authorization: Bearer TU_ACCESS_TOKEN"
```

### 3. Configurar Audience Mapper en Keycloak (Recomendado)

Para que el token incluya `aud: "hypesoft-api"`:

1. Accede a Keycloak Admin Console
2. Ve a **Clients** → `hypesoft-api`
3. Ve a la pestaña **Client Scopes**
4. Haz clic en **Add mapper** → **By configuration**
5. Selecciona **Audience**
6. Configura:
   - **Name**: `hypesoft-api-audience`
   - **Included Client Audience**: `hypesoft-api`
   - **Add to access token**: `ON`
7. Guarda

**Para el cliente frontend (hypesoft-client):**

1. Ve a **Clients** → `hypesoft-client`
2. Ve a la pestaña **Client Scopes**
3. Haz clic en **Add mapper** → **By configuration**
4. Selecciona **Audience**
5. Configura:
   - **Name**: `hypesoft-api-audience`
   - **Included Client Audience**: `hypesoft-api`
   - **Add to access token**: `ON`
6. Guarda

Después de esto, puedes activar `ValidateAudience = true` en `Program.cs` y configurar:
```csharp
options.Audience = "hypesoft-api";
options.TokenValidationParameters = new TokenValidationParameters
{
    ValidateAudience = true,
    ValidAudience = "hypesoft-api",
    // ... otros parámetros
};
```

### 4. Verificar Logs del Backend

El backend ahora tiene logging mejorado. Revisa los logs para ver:
- Errores de autenticación detallados
- Tipo de error (InvalidAudience, InvalidIssuer, etc.)
- Valores esperados vs. actuales

### 5. CORS (Si es necesario desactivar temporalmente)

Si necesitas desactivar CORS temporalmente para pruebas:

```csharp
// En Program.cs, comenta la sección de CORS:
// builder.Services.AddCors(...);
// app.UseCors();
```

**Nota**: No es recomendable en producción. Mejor configura CORS correctamente.

### 6. Verificar que el Frontend envíe el Token

En el frontend, verifica que el interceptor esté funcionando:

```typescript
// En lib/api/client.ts
apiClient.interceptors.request.use((config) => {
  const token = localStorage.getItem('access_token');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});
```

### 7. Checklist de Diagnóstico

- [ ] Keycloak está corriendo en `http://localhost:8080`
- [ ] El realm `hypesoft` existe
- [ ] Los clientes `hypesoft-client` y `hypesoft-api` existen
- [ ] `appsettings.Development.json` tiene el Authority correcto
- [ ] El token se obtiene correctamente desde Keycloak
- [ ] El token no está expirado
- [ ] El header `Authorization: Bearer <token>` se envía correctamente
- [ ] Los logs del backend muestran errores detallados
- [ ] CORS está configurado correctamente (o desactivado para pruebas)

## 📝 Ejemplo de Token JWT Decodificado

```json
{
  "iss": "http://localhost:8080/realms/hypesoft",
  "aud": "account",
  "sub": "12345678-1234-1234-1234-123456789012",
  "email": "admin@hypesoft.com",
  "preferred_username": "admin",
  "realm_access": {
    "roles": ["Admin", "Manager", "User"]
  },
  "exp": 1234567890,
  "iat": 1234567890
}
```

## 🆘 Si el problema persiste

1. Revisa los logs del backend (archivo `logs/hypesoft-*.log`)
2. Verifica los logs de Keycloak
3. Prueba obtener un token directamente desde Keycloak
4. Decodifica el token en jwt.io y verifica los claims
5. Verifica que el endpoint esté protegido con `[Authorize]`

