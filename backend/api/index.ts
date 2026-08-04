/* Vercel serverless entry for the NestJS API.
 *
 * The Nest app is compiled by `nest build` (tsc → decorator metadata present),
 * and this thin wrapper imports the COMPILED module from dist and serves it via
 * Express. It has no decorators itself, so Vercel's bundler handles it fine.
 * The app is created once and cached across warm invocations. */
import 'reflect-metadata';
import express from 'express';
import { NestFactory } from '@nestjs/core';
import { ExpressAdapter } from '@nestjs/platform-express';
// eslint-disable-next-line @typescript-eslint/no-var-requires
import { AppModule } from '../dist/app.module';

const server = express();
let ready: Promise<void> | null = null;

async function bootstrap(): Promise<void> {
  const app = await NestFactory.create(AppModule, new ExpressAdapter(server), {
    logger: ['error', 'warn'],
  });
  app.enableCors(); // open policy — the frontend lives on a different origin
  await app.init(); // no listen() on serverless
}

export default async function handler(req: express.Request, res: express.Response) {
  if (!ready) ready = bootstrap();
  await ready;
  server(req, res);
}
