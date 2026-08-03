/* Canonical icon system for the app. A single, consistent inline set:
   24×24 grid, 2px stroke, round caps/joins, currentColor — so weight, sizing
   and metaphor stay uniform without an external dependency. This is the
   project's standard set by decision; migrating to the Figma "Icons – Platforms
   Code" community kit is an optional future step and would require importing
   those assets 1:1. Decorative by default (aria-hidden); relabel where meaningful. */
import type { SVGProps } from "react";

type P = SVGProps<SVGSVGElement>;
const base = (p: P) => ({
  viewBox: "0 0 24 24",
  fill: "none",
  stroke: "currentColor",
  strokeWidth: 2,
  strokeLinecap: "round" as const,
  strokeLinejoin: "round" as const,
  "aria-hidden": true,   // decorative by default; callers relabel where meaningful
  focusable: false,
  ...p,
});

export const IconDashboard = (p: P) => (<svg {...base(p)}><rect x="3" y="3" width="7" height="9"/><rect x="14" y="3" width="7" height="5"/><rect x="14" y="12" width="7" height="9"/><rect x="3" y="16" width="7" height="5"/></svg>);
export const IconBus = (p: P) => (<svg {...base(p)}><path d="M8 6v6"/><path d="M15 6v6"/><path d="M2 12h19.6"/><path d="M18 18h3s.5-1.7.8-2.8c.1-.4.2-.8.2-1.2V7a2 2 0 0 0-2-2H3a2 2 0 0 0-2 2v7c0 .4.1.8.2 1.2.3 1.1.8 2.8.8 2.8h3"/><circle cx="7" cy="18" r="2"/><circle cx="17" cy="18" r="2"/></svg>);
export const IconRoute = (p: P) => (<svg {...base(p)}><circle cx="6" cy="19" r="3"/><path d="M9 19h8.5a3.5 3.5 0 0 0 0-7h-11a3.5 3.5 0 0 1 0-7H15"/><circle cx="18" cy="5" r="3"/></svg>);
export const IconUsers = (p: P) => (<svg {...base(p)}><path d="M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2"/><circle cx="9" cy="7" r="4"/><path d="M22 21v-2a4 4 0 0 0-3-3.87"/><path d="M16 3.13a4 4 0 0 1 0 7.75"/></svg>);
export const IconUser = (p: P) => (<svg {...base(p)}><path d="M19 21v-2a4 4 0 0 0-4-4H9a4 4 0 0 0-4 4v2"/><circle cx="12" cy="7" r="4"/></svg>);
export const IconMoney = (p: P) => (<svg {...base(p)}><rect x="2" y="5" width="20" height="14" rx="2"/><circle cx="12" cy="12" r="3"/><path d="M6 12h.01M18 12h.01"/></svg>);
export const IconReceipt = (p: P) => (<svg {...base(p)}><path d="M4 2v20l2-1 2 1 2-1 2 1 2-1 2 1 2-1 2 1V2l-2 1-2-1-2 1-2-1-2 1-2-1-2 1Z"/><path d="M8 7h8M8 11h8M8 15h5"/></svg>);
export const IconSwap = (p: P) => (<svg {...base(p)}><path d="m16 3 4 4-4 4"/><path d="M20 7H4"/><path d="m8 21-4-4 4-4"/><path d="M4 17h16"/></svg>);
export const IconTag = (p: P) => (<svg {...base(p)}><path d="M12.586 2.586A2 2 0 0 0 11.172 2H4a2 2 0 0 0-2 2v7.172a2 2 0 0 0 .586 1.414l8.704 8.704a2.426 2.426 0 0 0 3.42 0l6.58-6.58a2.426 2.426 0 0 0 0-3.42z"/><circle cx="7.5" cy="7.5" r="1"/></svg>);
export const IconSettings = (p: P) => (<svg {...base(p)}><path d="M12.22 2h-.44a2 2 0 0 0-2 2v.18a2 2 0 0 1-1 1.73l-.43.25a2 2 0 0 1-2 0l-.15-.08a2 2 0 0 0-2.73.73l-.22.38a2 2 0 0 0 .73 2.73l.15.1a2 2 0 0 1 1 1.72v.51a2 2 0 0 1-1 1.74l-.15.09a2 2 0 0 0-.73 2.73l.22.38a2 2 0 0 0 2.73.73l.15-.08a2 2 0 0 1 2 0l.43.25a2 2 0 0 1 1 1.73V20a2 2 0 0 0 2 2h.44a2 2 0 0 0 2-2v-.18a2 2 0 0 1 1-1.73l.43-.25a2 2 0 0 1 2 0l.15.08a2 2 0 0 0 2.73-.73l.22-.39a2 2 0 0 0-.73-2.73l-.15-.08a2 2 0 0 1-1-1.74v-.5a2 2 0 0 1 1-1.74l.15-.09a2 2 0 0 0 .73-2.73l-.22-.38a2 2 0 0 0-2.73-.73l-.15.08a2 2 0 0 1-2 0l-.43-.25a2 2 0 0 1-1-1.73V4a2 2 0 0 0-2-2z"/><circle cx="12" cy="12" r="3"/></svg>);
export const IconCalendar = (p: P) => (<svg {...base(p)}><rect x="3" y="4" width="18" height="18" rx="2"/><path d="M16 2v4M8 2v4M3 10h18"/></svg>);
export const IconClock = (p: P) => (<svg {...base(p)}><circle cx="12" cy="12" r="9"/><path d="M12 7v5l3 2"/></svg>);
export const IconPin = (p: P) => (<svg {...base(p)}><path d="M20 10c0 6-8 12-8 12s-8-6-8-12a8 8 0 0 1 16 0Z"/><circle cx="12" cy="10" r="3"/></svg>);
export const IconPlus = (p: P) => (<svg {...base(p)}><path d="M5 12h14M12 5v14"/></svg>);
export const IconEdit = (p: P) => (<svg {...base(p)}><path d="M12 20h9"/><path d="M16.5 3.5a2.12 2.12 0 0 1 3 3L7 19l-4 1 1-4Z"/></svg>);
export const IconTrash = (p: P) => (<svg {...base(p)}><path d="M3 6h18M8 6V4a2 2 0 0 1 2-2h4a2 2 0 0 1 2 2v2m2 0v14a2 2 0 0 1-2 2H8a2 2 0 0 1-2-2V6"/><path d="M10 11v6M14 11v6"/></svg>);
export const IconEye = (p: P) => (<svg {...base(p)}><path d="M2 12s3.5-7 10-7 10 7 10 7-3.5 7-10 7-10-7-10-7Z"/><circle cx="12" cy="12" r="3"/></svg>);
export const IconCheck = (p: P) => (<svg {...base(p)}><path d="M20 6 9 17l-5-5"/></svg>);
export const IconX = (p: P) => (<svg {...base(p)}><path d="M18 6 6 18M6 6l12 12"/></svg>);
export const IconSearch = (p: P) => (<svg {...base(p)}><circle cx="11" cy="11" r="8"/><path d="m21 21-4.3-4.3"/></svg>);
export const IconDownload = (p: P) => (<svg {...base(p)}><path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"/><path d="M7 10l5 5 5-5"/><path d="M12 15V3"/></svg>);
export const IconUpload = (p: P) => (<svg {...base(p)}><path d="M21 15v4a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2v-4"/><path d="M17 8l-5-5-5 5"/><path d="M12 3v12"/></svg>);
export const IconPrint = (p: P) => (<svg {...base(p)}><path d="M6 9V2h12v7"/><path d="M6 18H4a2 2 0 0 1-2-2v-5a2 2 0 0 1 2-2h16a2 2 0 0 1 2 2v5a2 2 0 0 1-2 2h-2"/><rect x="6" y="14" width="12" height="8"/></svg>);
export const IconChevronDown = (p: P) => (<svg {...base(p)}><path d="m6 9 6 6 6-6"/></svg>);
export const IconChevronEnd = (p: P) => (<svg {...base(p)} className={`dir-icon${p.className ? ` ${p.className}` : ""}`}><path d="m9 18 6-6-6-6"/></svg>);
export const IconMap = (p: P) => (<svg {...base(p)}><path d="M14.106 5.553a2 2 0 0 0 1.788 0l3.659-1.83A1 1 0 0 1 21 4.619v12.764a1 1 0 0 1-.553.894l-4.553 2.277a2 2 0 0 1-1.788 0l-4.212-2.106a2 2 0 0 0-1.788 0l-3.659 1.83A1 1 0 0 1 3 19.381V6.618a1 1 0 0 1 .553-.894l4.553-2.277a2 2 0 0 1 1.788 0z"/><path d="M9 4v13M15 7v13"/></svg>);
export const IconCopy = (p: P) => (<svg {...base(p)}><rect x="9" y="9" width="13" height="13" rx="2"/><path d="M5 15H4a2 2 0 0 1-2-2V4a2 2 0 0 1 2-2h9a2 2 0 0 1 2 2v1"/></svg>);
export const IconLogout = (p: P) => (<svg {...base(p)} className={`dir-icon${p.className ? ` ${p.className}` : ""}`}><path d="M9 21H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h4"/><path d="m16 17 5-5-5-5"/><path d="M21 12H9"/></svg>);
export const IconAlert = (p: P) => (<svg {...base(p)}><path d="m21.73 18-8-14a2 2 0 0 0-3.48 0l-8 14A2 2 0 0 0 4 21h16a2 2 0 0 0 1.73-3Z"/><path d="M12 9v4M12 17h.01"/></svg>);
export const IconBell = (p: P) => (<svg {...base(p)}><path d="M6 8a6 6 0 0 1 12 0c0 7 3 9 3 9H3s3-2 3-9"/><path d="M10.3 21a1.94 1.94 0 0 0 3.4 0"/></svg>);
export const IconInbox = (p: P) => (<svg {...base(p)}><path d="M22 12h-6l-2 3h-4l-2-3H2"/><path d="M5.45 5.11 2 12v6a2 2 0 0 0 2 2h16a2 2 0 0 0 2-2v-6l-3.45-6.89A2 2 0 0 0 16.76 4H7.24a2 2 0 0 0-1.79 1.11Z"/></svg>);
export const IconBuilding = (p: P) => (<svg {...base(p)}><rect x="4" y="2" width="16" height="20" rx="2"/><path d="M9 22v-4h6v4M8 6h.01M16 6h.01M8 10h.01M16 10h.01M8 14h.01M16 14h.01"/></svg>);
export const IconPercent = (p: P) => (<svg {...base(p)}><path d="M19 5 5 19"/><circle cx="6.5" cy="6.5" r="2.5"/><circle cx="17.5" cy="17.5" r="2.5"/></svg>);
export const IconTruck = (p: P) => (<svg {...base(p)}><path d="M14 18V6a2 2 0 0 0-2-2H4a2 2 0 0 0-2 2v11a1 1 0 0 0 1 1h2"/><path d="M14 9h4l4 4v4a1 1 0 0 1-1 1h-1"/><circle cx="7" cy="18" r="2"/><circle cx="17" cy="18" r="2"/></svg>);
export const IconTrend = (p: P) => (<svg {...base(p)}><path d="M3 3v18h18"/><path d="m19 9-5 5-4-4-3 3"/></svg>);
export const IconMenu = (p: P) => (<svg {...base(p)}><path d="M4 6h16M4 12h16M4 18h16"/></svg>);
