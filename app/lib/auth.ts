/** Shared auth constants usable from BOTH server and client modules.
 *  Kept out of the "use client" AuthContext on purpose: importing a value from
 *  a client module into a Server Component yields a client-reference proxy
 *  (a function), not the value itself. */
export const AUTH_COOKIE = "garas.auth";
