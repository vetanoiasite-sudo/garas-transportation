import { NextResponse } from "next/server";
import type { NextRequest } from "next/server";

// Kept in sync with AUTH_COOKIE in contexts/AuthContext.tsx. Inlined here so
// this edge/proxy module doesn't pull in the client auth component.
const AUTH_COOKIE = "garas.auth";
const locales = ["ar", "en"] as const;
const defaultLocale = "ar";

/** Server-side auth gate. Redirects unauthenticated visitors to the login page
 *  and authenticated visitors away from it — before any page renders — so the
 *  browser never paints a loading spinner or a dashboard→login flash. */
export function proxy(request: NextRequest) {
  const { pathname } = request.nextUrl;
  const segments = pathname.split("/");
  const locale = (locales as readonly string[]).includes(segments[1])
    ? segments[1]
    : defaultLocale;
  const isLoginPage = segments[2] === "login";
  const authed = Boolean(request.cookies.get(AUTH_COOKIE)?.value);

  if (!authed && !isLoginPage) {
    return NextResponse.redirect(new URL(`/${locale}/login`, request.url));
  }
  if (authed && isLoginPage) {
    return NextResponse.redirect(new URL(`/${locale}/dashboard`, request.url));
  }
  return NextResponse.next();
}

export const config = {
  // Run on every route except Next internals, the favicon, and static assets.
  matcher: ["/((?!_next/static|_next/image|favicon.ico|.*\\..*).*)"],
};
