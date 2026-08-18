# CandleCore

CandleCore is a Bitcoin candle analytics stack: a .NET API over PostgreSQL market data, and a React dashboard for overview metrics, statistical analysis, and interactive charts.

## Features

- **Dashboard** — period filters, market statistics, dataset summary
- **Analysis** — overview metrics (summary, price change, risk, behaviour, probability) with timeframe + period filters
- **Charts** — full-history candlestick charts with timeframe switching
- **Market data API** — candles and dataset bounds; ~10k `BTCUSDT` `1h` candles seed from Binance on first API start


## Tech stack

| Layer | Stack |
|---|---|
| API | .NET 9, ASP.NET Core, EF Core, Npgsql |
| Database | PostgreSQL 16 |
| Client | React, Vite, TanStack Router / Query, Tailwind |
| Containers | Docker Compose, nginx (SPA + `/api` reverse proxy) |

## Architecture

Server: folders by role (HTTP, business logic, database), then by feature inside services. Client: pages and features for dashboard, charts, and analysis.

```text
Browser → client (nginx :5173)
            ├─ /         → React SPA
            └─ /api/...  → api :8080 → PostgreSQL
```

**Backend**

- Candles are stored as **1h** rows in PostgreSQL; other timeframes are built by aggregating those rows.
- Reads go: controller → query service → candle load/aggregate → metrics.
- On first API start, `CandleSeeder` downloads up to 10k `BTCUSDT` `1h` candles from Binance in the background; later starts skip when that many exist.
- API responses use `*Dto`; inputs live under `Requests/` as `*Request`.

**Frontend**

- Pages for Dashboard, Charts, and Analysis load data with TanStack Query.
- UI lives under `client/src/features/`; HTTP helpers under `client/src/api/`.
- Browser calls `/api` on the same host; nginx proxies to the API.

## Project structure

```text
.
├── src/CandleCore.Api/
│   ├── Controllers/          # HTTP endpoints by feature
│   ├── Services/             # Dashboard, Analysis, MarketData
│   ├── Infrastructure/       # EF Core + Binance client
│   ├── DTOs/ · Requests/ · Entities/
│   └── Program.cs
├── client/
│   ├── src/
│   │   ├── routes/           # TanStack Router pages
│   │   ├── features/         # UI by feature
│   │   ├── api/              # HTTP clients + query options
│   │   └── shared/           # UI primitives, formatting
│   ├── Dockerfile
│   └── nginx.conf
├── tests/
├── docker-compose.yml
└── .env.example
```

## Prerequisites

- Docker Desktop (or a compatible Compose runtime)

.NET SDK is only needed if you develop the API on the host (see Local API development).

## Quick Start

### 1. Env

```bash
cp .env.example .env
```

`.env.example` includes a demo password (`changeme`) for local/portfolio use. Change it if you want. Prefer passwords without `$`.

Optional: change `POSTGRES_DB`, user, or host ports. If host port **5432** is already taken (e.g. a local Postgres), set `POSTGRES_HOST_PORT` (e.g. `5433`).

### 2. Start

```bash
docker compose up --build
```

Compose starts Postgres, applies EF migrations (one-shot `migrate` service), then starts the API and client. Re-running `up` is safe when migrations are already applied.

### 3. Market data

The first API start needs outbound HTTPS to Binance and can take a short while to pull up to 10,000 `BTCUSDT` `1h` candles; the UI may be empty until that finishes. Later starts skip when that many exist. A failed seed does not block the API (empty UI until retry/restart). The client may 502 until the API is listening.

### 4. Open the app

**http://localhost:5173** (override with `CLIENT_HOST_PORT`).

| Service | Default URL |
|---|---|
| Client (nginx) | http://localhost:5173 |
| API (direct) | http://localhost:5059 |
| Postgres | localhost:5432 |

nginx proxies `/api/` to the API (same-origin). There is **no** API readiness healthcheck in v1 — the client may briefly return 502 until the API is listening.

## Local API development

For day-to-day API work without rebuilding the API image:

1. Start only Postgres: `docker compose up db -d`
2. Point `dotnet user-secrets` at `localhost` + your `.env` user/db/password/`POSTGRES_HOST_PORT`
3. Run `dotnet watch run --project src/CandleCore.Api` (or `dotnet run`)

Keep real local secrets in user-secrets — do not commit them. Full-stack Docker still uses Compose env for the API container.

## Environment configuration

Copy `.env.example` to `.env` (do not commit `.env`).

| Key | Notes |
|---|---|
| `POSTGRES_USER` | DB user (required by Compose) |
| `POSTGRES_PASSWORD` | DB password (required; demo default `changeme`) |
| `POSTGRES_DB` | Database name |
| `POSTGRES_HOST_PORT` | Host → Postgres (default `5432`) |
| `API_HOST_PORT` | Host → API (default `5059`) |
| `CLIENT_HOST_PORT` | Host → client (default `5173`) |
| `ASPNETCORE_ENVIRONMENT` | API environment in Compose (default `Production`) |

## API overview

| Group | Purpose |
|---|---|
| `/api/dashboard` | Dashboard overview |
| `/api/analysis` | Analysis overview metrics |
| `/api/market-data` | Candles and dataset bounds |

## Security / known limitations (v1)

- **Demo `.env` password** (`changeme`) is for local demos only — not for real deployment.
- **No API `/health` endpoint** yet. Compose does not wait on API readiness; brief startup 502s from nginx are possible.

## Future improvements / roadmap

- `/health` (or readiness) endpoint + Compose healthcheck