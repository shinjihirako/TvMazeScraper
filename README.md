
# TvMaze Scraper

## Overview
The TvMaze Scraper is a .NET 8 Web API application that ingests TV show and cast metadata from the public TVMaze API, stores it in a local database, and exposes this data through a RESTful API. This service is designed to enrich metadata systems with structured show and cast information.

### Features
- Scrapes TV show and cast data from TVMaze API.
- Persists data in a inmemory-database (e.g., SQL Server, PostgreSQL, or SQLite).
- Provides a paginated REST API endpoint to retrieve shows with their cast.
- Cast members are ordered by birthday (descending) in the API response.
- Built with .NET 8 and Entity Framework Core.
- Rate-limiting friendly (TVMaze API is publicly accessible but limited).

### API Endpoint Example
```bash
GET /api/shows?page=1&pageSize=10
```

### Sample Response:
```bash
[
  {
    "id": 1,
    "name": "Game of Thrones",
    "cast": [
      {
        "id": 7,
        "name": "Mike Vogel",
        "birthday": "1979-07-17"
      },
      {
        "id": 9,
        "name": "Dean Norris",
        "birthday": "1963-04-08"
      }
    ]
  },
  {
    "id": 4,
    "name": "Big Bang Theory",
    "cast": [
      {
        "id": 6,
        "name": "Michael Emerson",
        "birthday": "1950-01-01"
      }
    ]
  }
]
```

### How to Run Locally
#### Prerequisites
- .NET 8 SDK


### Project Structure

#### Layer	Purpose
Application	DTOs, 
Service Interfaces, Application Logic
Domain	Core Domain Entities & Base Classes
Infrastructure	Database Context, Repositories
API	REST Controllers, Startup Configuration


Considerations
The application respects TVMaze API rate-limiting.

Designed to be extended with background jobs (e.g., Hangfire) for periodic updates.

DTOs are clean, without serialization artifacts ($id, $values) in responses.

Mapping strategies (AutoMapper) keep domain and API contracts decoupled.
