## PFA Onboarding API

ASP.NET Core 8 Web API for the PFA onboarding form with SQL Server and Entity Framework Core.

### Suggested table name for multiple distributors

**`PFAOnboardingRequestDistributors`** — junction table linking each onboarding request to one or more selected dealers from `DealerMaster`.

### API endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | `/api/territories` | Active territories (`IsActive = 1`) |
| GET | `/api/distributors?territoryId={id}` | Distributors where `CustomerTypeID = 3` |
| GET | `/api/users/lookup?mobile={mobile}` | Check `UserDetails` by mobile |
| POST | `/api/onboarding` | Submit onboarding form |

### Setup

1. Install [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0).
2. Update `appsettings.json` connection string.
3. Run `scripts/CreateOnboardingTables.sql` on your database (existing master tables must already exist).
4. If your `UserDetails` or `DealerMaster` column names differ, update the entity classes in `Entities/`.

```bash
cd src/PFAOnboardingApi
dotnet restore
dotnet run
```

Swagger UI: `https://localhost:7080/swagger`

**Test UI:** `http://localhost:5080` (served from `wwwroot/index.html`)

### Frontend flow

1. User enters mobile → call `GET /api/users/lookup?mobile=...`
2. If `found: true`, show prompt: "Use existing profile?" and pre-fill from `userDetails`
3. Load territories → `GET /api/territories`
4. On territory change → `GET /api/distributors?territoryId=...` (multi-select)
5. Submit → `POST /api/onboarding`

### Submit payload example

```json
{
  "name": "Rahul Sharma",
  "mobile": "9876543210",
  "emailId": "rahul@example.com",
  "panNo": "ABCDE1234F",
  "aadhaarNumber": "123456789012",
  "uanNumber": "100000000000",
  "territoryId": 5,
  "dealerIds": [101, 102, 105],
  "useExistingUserDetails": true,
  "userDetailsId": 42
}
```

When not using existing user details, set `useExistingUserDetails: false` and `userDetailsId: null`.

### Security notes

- All DB access uses EF Core parameterized queries (no SQL injection).
- PAN, Aadhaar, and mobile are validated server-side.
- Sensitive fields are not logged.
- Configure `Cors:AllowedOrigins` for your frontend domain in production.
- Use HTTPS and store connection strings in environment variables or Azure Key Vault.

### Column mapping assumptions

Adjust entity properties if your database uses different names:

| Table | Assumed PK / columns |
|-------|----------------------|
| TerritoryMaster | TerritoryId, TerritoryName, IsActive |
| DealerMaster | DealerId, TerritoryId, CustomerTypeID, RetailerShopName, IsActive (optional) |
| UserDetails | UserId, Mobile, Name, EmailId, PanNo, AadhaarNumber, UanNumber, IsActive (optional) |
