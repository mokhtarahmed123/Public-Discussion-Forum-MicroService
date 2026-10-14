# Public Discussion Forum

A discussion forum built with a **microservices architecture** on .NET 8. It features JWT authentication, posts and comments, up/down voting, and a ranking system for the top-voted posts and comments. The whole system runs with a single Docker command.

## Architecture

```
Client ──▶ API Gateway (YARP + JWT validation)
              ├──▶ User Service      (SQL Server, ASP.NET Identity, JWT)
              ├──▶ Posts & Comments  (MongoDB)
              ├──▶ Votes Service     (MongoDB) ──publish──▶ RabbitMQ
              └──▶ Ranking Service   (MongoDB) ◀─consume──── RabbitMQ
```

![Architecture]<img width="965" height="471" alt="image" src="https://github.com/user-attachments/assets/861d0f52-5b97-44ac-aa55-bf7dce80b1dd" />
<img width="1224" height="512" alt="image" src="https://github.com/user-attachments/assets/27c6e291-cc46-4f34-a011-1b7aefa8b13f" />



| Service | Responsibility | Storage |
|---|---|---|
| **API Gateway** | Single entry point, JWT validation, request routing | n/a |
| **User Service** | Sign up, login, refresh tokens, roles, email confirmation | SQL Server |
| **Posts & Comments** | CRUD for posts, comments, and replies | MongoDB |
| **Votes Service** | Voting on posts and comments, publishes events | MongoDB + RabbitMQ |
| **Ranking Service** | Computes scores and builds the Top Ten for posts and comments | MongoDB + RabbitMQ |

## Tech Stack

- .NET 8, ASP.NET Core, Clean Architecture, CQRS (MediatR)
- FluentValidation, AutoMapper
- SQL Server + EF Core (Users), MongoDB (other services)
- RabbitMQ + MassTransit (event-driven communication)
- YARP reverse proxy
- JWT authentication + role-based authorization
- Docker & Docker Compose

## Key Design Decisions

- **Event-driven ranking**: the Votes service publishes `VotePostMessage` / `VoteCommentMessage`; the Ranking service consumes them and updates scores.
- **Idempotency**: every event carries an `EventId` stored in `ProcessedEvents`, so duplicate deliveries are ignored.
- **Atomic score updates**: MongoDB `$inc` with upsert.
- **Authentication at the gateway**: routes are protected by policies (`anonymous` / `default` / `admin`), and the `UserId` is taken from the token, never from the request body.
- **Internal services are not exposed**: only the gateway is reachable from outside.

## Getting Started

### Requirements
- Docker Desktop

### 1. Create a `.env` file in the repository root

```
SQL_PASSWORD=<strong-password>
RABBIT_PASSWORD=<password>
JWT_SECRET=<random-string-of-32-chars-or-more>
EMAIL_PASSWORD=
GOOGLE_CLIENT_ID=
```

> The SQL password must be at least 8 characters and contain 3 of 4: uppercase, lowercase, digit, symbol.

### 2. Run the project

```bash
docker compose up --build -d
```

### 3. Try it

The gateway is available at `http://localhost:5238`.

```
POST /api/Authentication/SignUp
POST /api/Authentication/LogIn
GET  /api/Posts
```

RabbitMQ management UI: `http://localhost:15672`

## API Overview

Base URL: `http://localhost:5238` (🔒 = requires `Authorization: Bearer <token>`)

**Authentication**
- `POST /api/Authentication/SignUp`
- `POST /api/Authentication/LogIn`
- `POST /api/Authentication/RefreshToken`
- `GET  /api/Authentication/MyProfile` 🔒

**Posts**
- `GET /api/Posts`, `GET /api/Posts/{id}`, `GET /api/Posts/latest`
- `POST /api/Posts` 🔒, `PUT /api/Posts/{id}` 🔒, `DELETE /api/Posts/{id}` 🔒

**Comments**
- `POST /api/posts/{postId}/comments` 🔒
- `GET /api/post/{postId}/comments`
- `PATCH` / `DELETE /api/posts/{postId}/comments/{id}` 🔒

**Votes**
- `POST /api/posts/{postId}/vote` 🔒
- `POST /api/posts/{postId}/comments/{commentId}/vote` 🔒
- `GET  /api/posts/{postId}/vote`

**Ranking**
- `GET /api/Ranking/top-ten-posts`
- `GET /api/Ranking/posts/{postId}/top-comments`

**Admin** 🔒 (role `Admin`)
- `/api/Role`, `/api/Authorization`

## Project Structure

```
├── ApiGateway/
├── UserService/
├── PostsCommentsService/
├── VotesService/
├── RankingService/
├── Forum.Contracts/        # shared RabbitMQ message contracts
├── docker-compose.yml
└── Dockerfile.*
```

## Roadmap

- [ ] Frontend web app
- [ ] HTTPS (Caddy) and deployment on AWS
- [ ] Email provider (SendGrid / SES)
- [ ] Unit and integration tests
- [ ] Caching layer at the gateway

## Author

**Mokhtar**: Backend Developer (C#, ASP.NET Core, Clean Architecture)
`<GitHub / LinkedIn link>`
