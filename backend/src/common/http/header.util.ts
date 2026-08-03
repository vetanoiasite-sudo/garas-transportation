/**
 * Decode a percent-encoded HTTP header value.
 *
 * Filters ride in HTTP headers (faithful to the documented contract), but header
 * values must be ASCII — non-ASCII (e.g. Arabic search text) gets mangled in
 * transit. Clients percent-encode such values; this reverses it. Falls back to
 * the raw value if it was not encoded (or is malformed), so ASCII stays untouched.
 */
export function decodeHeader(value?: string): string | undefined {
  if (value === undefined || value === null || value === '') return undefined;
  try {
    return decodeURIComponent(value);
  } catch {
    return value;
  }
}
