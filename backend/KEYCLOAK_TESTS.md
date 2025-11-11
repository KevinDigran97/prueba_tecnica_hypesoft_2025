# Pruebas de autenticación Keycloak

## 1. Obtener un token de Keycloak

### Client Credentials (service-to-service)

```bash
curl -X POST http://localhost:8080/realms/hypesoft/protocol/openid-connect/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "client_id=hypesoft-api" \
  -d "client_secret=a9IdKPnMjGFKa8sIjVnZy8Gc9nIkS06t" \
  -d "grant_type=client_credentials"
```

### Password Grant (usuario real)

```bash
curl -X POST http://localhost:8080/realms/hypesoft/protocol/openid-connect/token \
  -H "Content-Type: application/x-www-form-urlencoded" \
  -d "grant_type=password" \
  -d "client_id=hypesoft-client" \
  -d "username=admin" \
  -d "password=admin123"
```

## 2. Verificar el token en jwt.io

- Copia el `access_token` de la respuesta.
- Ve a https://jwt.io y verifica:
  - `iss`: `http://localhost:8080/realms/hypesoft`
  - `aud`: `hypesoft-api` (debe coincidir)
  - `exp`: No expirado

## 3. Probar el endpoint protegido

```bash
curl -X GET http://localhost:5000/api/products \
  -H "Authorization: Bearer TU_ACCESS_TOKEN"
```

## 4. Revisar logs del backend

- Verifica en `logs/hypesoft-*.log` los mensajes de autenticación.
- Debes ver logs de éxito o detalles de error (InvalidAudience, InvalidIssuer, etc).

## 5. Prueba con token inválido

- Modifica el token (por ejemplo, cambia un carácter) y repite la petición.
- La API debe responder con 401 Unauthorized.

---

Si todas las pruebas pasan, la autenticación con Keycloak está funcionando correctamente en tu API.
