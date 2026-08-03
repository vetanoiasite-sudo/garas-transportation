import 'dotenv/config';
import 'reflect-metadata';
import { NestFactory } from '@nestjs/core';
import { AppModule } from './app.module';

async function bootstrap() {
  const app = await NestFactory.create(AppModule);
  app.enableCors(); // open policy — matches the old backend
  const port = Number(process.env.PORT ?? 4000);
  await app.listen(port, '0.0.0.0'); // bind all interfaces (containers/hosts)
  // eslint-disable-next-line no-console
  console.log(`Garas transportation backend listening on port ${port}`);
}
bootstrap();
