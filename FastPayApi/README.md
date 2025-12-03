# Fast-Pay Minimal Demo API

This folder contains a minimal ASP.NET Core API (in-memory) demonstrating a small Fast-Pay backend for demos and student projects.

Run the API:

```powershell
cd "c:\Users\Adonis Sena\ComunidadApp\FastPayApi"
dotnet run
```

The API listens on `http://localhost:5000` by default (if free). Then open the client:

```powershell
cd "c:\Users\Adonis Sena\ComunidadApp\FastPayClient"
# serve this folder with any static server, for example:
# Simple way using PowerShell + .NET SDK (experimental):
python -m http.server 8000
# then open http://localhost:8000
```

End-points:
- POST /api/users  - create user
- GET /api/users/{phone} - get user
- GET /api/users - all users
- POST /api/payments/send - send payment
- GET /api/transactions/{phone} - transactions for user

This is intentionally small and clear for learning purposes. Replace in-memory stores with EF Core for production.
