// Transitional global usings: modules not yet migrated to the Clean Architecture
// layers still name AppDbContext / entities / Envelope / Fmt unqualified. These
// map the old single-project namespaces onto their new layer homes; each shrinks
// away as modules move to Application services, and this file is deleted when
// the last module migrates.
global using GarasTransport.Domain.Entities;
global using GarasTransport.Infrastructure.Persistence;
global using GarasTransport.Infrastructure.Security;
global using GarasTransport.Application.Responses;
global using GarasTransport.Application.Helpers;
