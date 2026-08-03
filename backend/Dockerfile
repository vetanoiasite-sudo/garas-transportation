# Production image for the NestJS API (PostgreSQL). Portable to any Docker host
# (Koyeb, Back4App, Railway, a VPS, …). On start it syncs the schema + seeds the
# demo data (roles + one login per role), then serves the API.
FROM node:20-slim

# Prisma's query engine needs openssl at runtime.
RUN apt-get update -y && apt-get install -y openssl && rm -rf /var/lib/apt/lists/*

WORKDIR /app

# Install deps (incl. dev — needed for prisma CLI, ts-node & the Nest build).
COPY package*.json ./
RUN npm install

# Copy the source and build: generate the Postgres client + compile to dist/.
COPY . .
RUN npm run prod:setup && npm run build

ENV NODE_ENV=production
# Hosts inject their own PORT; the app reads process.env.PORT (defaults to 4000).
EXPOSE 4000

# Sync schema → seed (idempotent) → start the server.
CMD ["npm", "run", "start:container"]
