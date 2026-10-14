# CarCare

CarCare je web aplikacija za upravljanje vozilima, praćenje historije održavanja i evidenciju troškova
povezanih s automobilima. Korisnik evidentira svoja vozila, prati servise i popravke, bilježi troškove
i ima pregled nad budućim obavezama održavanja.

Projekat je samostalni portfolio projekat s fokusom na kvalitetnu arhitekturu, održiv kod i realnu
poslovnu logiku.

> **Status:** postavljena je početna arhitektura. Poslovne funkcionalnosti još nisu implementirane.

## Planirani moduli

1. Upravljanje vozilima
2. Historija održavanja
3. Podsjetnici za održavanje (po datumu i kilometraži)
4. Evidencija troškova
5. Dashboard

## Tehnologije

| Sloj      | Tehnologije                                                        |
| --------- | ------------------------------------------------------------------ |
| Backend   | C#, ASP.NET Core Web API (.NET 9), Entity Framework Core 9         |
| Baza      | Microsoft SQL Server (lokalno: SQL Server LocalDB)                 |
| API docs  | OpenAPI (`Microsoft.AspNetCore.OpenApi`) + Swagger UI              |
| Frontend  | Angular 21, TypeScript, Angular Router, HttpClient, SCSS, Vitest   |
| Alati     | Git, npm, `dotnet-ef` (lokalni alat)                               |

## Struktura repozitorija

```
CarCare/
├── CarCare.sln                     # root solution
├── global.json                     # fiksira .NET SDK 9.0.x
├── Directory.Build.props           # zajedničke MSBuild postavke (net9.0, nullable, implicit usings)
├── .config/dotnet-tools.json       # lokalni .NET alati (dotnet-ef)
├── backend/
│   └── src/
│       ├── CarCare.Domain/         # domenski modeli i poslovna pravila
│       ├── CarCare.Application/    # slučajevi upotrebe, servisi, DTO-i, apstrakcije
│       ├── CarCare.Infrastructure/ # EF Core, SQL Server, implementacije apstrakcija
│       │   └── Persistence/        # CarCareDbContext (i kasnije konfiguracije entiteta, migracije)
│       └── CarCare.Api/            # HTTP endpointi, konfiguracija, composition root
└── frontend/                       # Angular aplikacija
    ├── proxy.conf.json             # dev proxy: /api -> backend
    └── src/
        ├── environments/           # konfiguracija po okruženju (apiBaseUrl)
        └── app/
            ├── app.config.ts       # globalni provideri (router, HttpClient)
            ├── app.routes.ts       # root rute; funkcionalnosti se učitavaju lazy
            ├── core/               # singleton servisi i layout aplikacije
            │   ├── layout/shell/   # zaglavlje, navigacija, <router-outlet>
            │   └── services/
            ├── shared/             # zajedničke, ponovo upotrebljive UI komponente, pipe-ovi
            └── features/           # jedna mapa po funkcionalnosti (home, kasnije vehicles, ...)
```

## Arhitektura backenda

Backend koristi slojevitu arhitekturu. Zavisnosti su usmjerene prema unutrašnjim slojevima:

```
CarCare.Api ──► CarCare.Infrastructure ──► CarCare.Application ──► CarCare.Domain
     └──────────────────────────────────────────►┘
```

- **Domain** nema zavisnosti ni prema jednom drugom projektu niti prema NuGet paketima.
- **Application** zavisi samo od Domaina. Ovdje se definišu apstrakcije (npr. interfejsi za pristup
  podacima) koje Infrastructure implementira.
- **Infrastructure** zavisi od Applicationa i sadrži EF Core / SQL Server implementacije.
- **Api** je *composition root*: jedino mjesto koje poznaje sve slojeve i povezuje ih kroz DI.

Svaki sloj registruje svoje servise kroz vlastitu extension metodu, a `Program.cs` ih samo poziva:

```csharp
builder.Services
    .AddApplication()
    .AddInfrastructure(builder.Configuration);
```

Namjerno se **ne koriste** generički repozitoriji, CQRS, MediatR ni mikroservisi; uvode se samo ako se
za njih pojavi konkretna potreba.

## Arhitektura frontenda

- **core/** – stvari koje postoje jednom u aplikaciji: layout, servisi na nivou aplikacije,
  kasnije interceptori i guardovi.
- **shared/** – prezentacijske komponente, direktive i pipe-ovi koje koristi više funkcionalnosti.
  Ne zavise od `features/`.
- **features/** – svaka funkcionalnost ima svoju mapu (komponente, servisi, modeli, rute) i učitava se
  lazy preko routera. Funkcionalnosti ne importuju jedna drugu direktno.

Aplikacija koristi standalone komponente, signale i `OnPush` change detection.

## Preduslovi

- [.NET SDK 9.0](https://dotnet.microsoft.com/download/dotnet/9.0) (≥ 9.0.100)
- [Node.js](https://nodejs.org/) 20.19+ ili 22.12+ i npm
- SQL Server LocalDB (dolazi uz Visual Studio ili SQL Server Express) ili druga SQL Server instanca
- Pouzdan HTTPS razvojni certifikat: `dotnet dev-certs https --trust`

Angular CLI ne mora biti instaliran globalno; komande ispod koriste lokalnu verziju preko `npx`/npm skripti.

## Lokalno pokretanje

### 1. Backend (API)

```powershell
dotnet tool restore
dotnet build
dotnet run --project backend/src/CarCare.Api --launch-profile https
```

| Adresa                                   | Opis                    |
| ---------------------------------------- | ----------------------- |
| https://localhost:7180/swagger           | Swagger UI              |
| https://localhost:7180/openapi/v1.json   | OpenAPI dokument        |
| https://localhost:7180/api/health        | Health check            |

Swagger UI i OpenAPI dokument su dostupni samo u `Development` okruženju.

### 2. Frontend (Angular)

```powershell
cd frontend
npm install
npm start
```

Aplikacija je dostupna na http://localhost:4200. Dev server preusmjerava sve zahtjeve na `/api`
prema `https://localhost:7180` (vidi `frontend/proxy.conf.json`), pa CORS u razvoju nije potreban.
Početna stranica prikazuje da li je API dostupan.

### Build i testovi

```powershell
dotnet build                 # iz root foldera
cd frontend
npm run build                # produkcijski build u frontend/dist/
npm test -- --watch=false    # unit testovi (Vitest)
```

## Konfiguracija i tajne

Connection string se čita iz `ConnectionStrings:CarCare`.

- **Development:** `appsettings.Development.json` koristi LocalDB s Windows autentifikacijom
  (`Server=(localdb)\MSSQLLocalDB;Database=CarCare;Trusted_Connection=True`). Ne sadrži lozinke.
- **Druga SQL Server instanca / SQL autentifikacija:** koristi User Secrets i nikad ne upisuj lozinku
  u `appsettings*.json`:

  ```powershell
  dotnet user-secrets set "ConnectionStrings:CarCare" "Server=...;Database=CarCare;User Id=...;Password=...;TrustServerCertificate=True" --project backend/src/CarCare.Api
  ```

- **Ostala okruženja:** environment varijabla `ConnectionStrings__CarCare`.

Ako connection string nije postavljen, API odbija da se pokrene s jasnom porukom o grešci.

## Entity Framework Core

`CarCareDbContext` se nalazi u `CarCare.Infrastructure/Persistence` i automatski učitava sve
`IEntityTypeConfiguration<T>` klase iz Infrastructure projekta. Entiteti i migracije još ne postoje.
Kada budu dodani, migracije se kreiraju i primjenjuju ovako:

```powershell
dotnet ef migrations add <NazivMigracije> --project backend/src/CarCare.Infrastructure --startup-project backend/src/CarCare.Api --output-dir Persistence/Migrations
dotnet ef database update --project backend/src/CarCare.Infrastructure --startup-project backend/src/CarCare.Api
```
