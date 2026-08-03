/* Transportation API service layer.
 *
 * Namespaced re-exports so callers can write `api.lines.getLines()` etc.
 * Every module issues real calls to the backend via `@/lib/api/client`; see each
 * module for the documented endpoint each function maps to.
 */
export * as lines from "./lines";
export * as routes from "./routes";
export * as passengers from "./passengers";
export * as users from "./users";
export * as vehicles from "./vehicles";
export * as suppliers from "./suppliers";
export * as deductions from "./deductions";
export * as repricing from "./repricing";
export * as exceptions from "./exceptions";
export * as attendance from "./attendance";
export * as costs from "./costs";

export * from "./http";
