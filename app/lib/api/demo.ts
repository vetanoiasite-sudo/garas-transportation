/* Offline demo mode. When VITE_DEMO=true the HTTP client never hits a backend —
 * it returns canned responses captured from the real backend (demo-data.json),
 * so the whole app runs from a static build (e.g. Vercel) with no server or DB.
 * Writes return a generic success; the session shows the seeded data. */
import demoData from "./demo-data.json";

const DATA = demoData as unknown as Record<string, unknown>;

// For endpoints captured per-id, which (lowercased) header carries the lookup key.
const KEY_HEADER: Record<string, string> = {
  DashBoard: "routeid",
  getTransportationRoute: "routeid",
  GetTransportationDirection: "routeid",
  GetTransportationEmployeeList: "routeid",
  GetCapacityNumbers: "routeid",
  getHrUser: "id",
  getSupplier: "id",
  getTransportationRoutesForHrUser: "hruserid",
};

interface Envelope {
  Result: boolean;
  Errors: { ErrorCode: string; ErrorMSG: string }[];
  Data: unknown;
  Id?: string | number;
  Message?: string;
  [k: string]: unknown;
}

function lower(headers?: Record<string, unknown>): Record<string, string> {
  const o: Record<string, string> = {};
  if (headers) for (const [k, v] of Object.entries(headers)) if (v != null && v !== "") o[k.toLowerCase()] = String(v);
  return o;
}

const ok = (data: unknown = null, extra: Partial<Envelope> = {}): Envelope => ({ Result: true, Errors: [], Data: data, ...extra });

const isEnvelope = (v: unknown): v is Envelope => !!v && typeof v === "object" && "Result" in (v as object);

/** Resolve a canned response for a request (mirrors the real endpoints). */
export function demoResponse(method: "GET" | "POST", path: string, headers?: Record<string, unknown>): Envelope {
  const h = lower(headers);

  // ---- Auth / User ----
  if (path === "/User/Login") {
    const login = DATA.__login as Envelope | undefined;
    return login ?? ok("demo-token", { Name: "أحمد النظام", BranchId: 1, RoleList: [{ RoleID: 216, RoleName: "Transportation Super Admin" }] });
  }
  if (path === "/User/Logout") return ok(true);
  if (path === "/User/GetUserList") return (DATA["/User/GetUserList"] as Envelope) ?? ok([]);
  if (path === "/User/GetAllUsers") return (DATA["/User/GetAllUsers"] as Envelope) ?? ok([]);
  if (path.startsWith("/User/")) return ok(null, { Id: "1", Message: "" }); // AddUser/UpdateUser/DeleteUser

  // ---- Transportation ----
  const action = path.replace(/^\/api\/Transportation\//, "");

  // All writes succeed (no persistence in the demo).
  if (method === "POST") return ok(null, { Id: "1", Message: "" });

  const entry = DATA[action];
  if (entry === undefined) return ok(""); // excel/download or unknown GET → empty
  if (isEnvelope(entry)) return entry;

  // Keyed-by-id map: pick the row for the requested id, with sensible fallbacks.
  const map = entry as Record<string, Envelope>;
  const keyHeader = KEY_HEADER[action];
  const keyVal = keyHeader ? h[keyHeader] : undefined;
  return map[keyVal ?? ""] ?? map[""] ?? map[Object.keys(map)[0]];
}
