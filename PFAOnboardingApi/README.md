## PFA Onboarding API

ASP.NET Core 8 Web API for the PFA onboarding form with SQL Server (`cc` schema) and Entity Framework Core.

### Database alignment

| Table | Schema | Notes |
|-------|--------|-------|
| `PFAOnboardingRequests` | `cc` | No `Status` column |
| `PFAOnboardingRequestDistributors` | `cc` | `DistributorId` NVARCHAR(30) → `DealerMaster.ContactId` |
| `TerritoryMaster` | `cc` | `territoryId` PK |
| `DealerMaster` | `cc` | `ContactId` PK, `CustomerTypeID = 3` for distributors |
| `UserDetails` | `cc` | `UserId`, `Mobile`, `FirstName`, `EmailId`, `Active` (`Y`/`N`, not bit) |

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
3. Run `scripts/CreateOnboardingTables.sql` on your database.
4. Start the API:

```bash
cd src/PFAOnboardingApi
dotnet restore
dotnet run
```

Swagger UI: `https://localhost:7080/swagger`  
Test UI: `http://localhost:5080`

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
  "distributorIds": ["CNT001", "CNT002"],
  "useExistingUserDetails": true,
  "userDetailsId": 42
}
```

### Project structure

```
Constants/          — Schema and business constants
Data/
  Configurations/   — EF Core entity mappings (modular)
Entities/           — Database entities
Services/
  Validation/       — Business-rule validation
Validators/         — Request DTO validation (FluentValidation)
```
