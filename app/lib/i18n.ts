/* Lightweight i18n — Arabic-first (RTL), English (LTR) toggle.
   No external lib; a flat dictionary keyed by string id. */

export const locales = ["ar", "en"] as const;
export type Locale = (typeof locales)[number];
export const defaultLocale: Locale = "ar";

export function isLocale(value: string): value is Locale {
  return (locales as readonly string[]).includes(value);
}

export function dir(locale: Locale): "rtl" | "ltr" {
  return locale === "ar" ? "rtl" : "ltr";
}

type Dict = Record<string, string>;

const ar: Dict = {
  appName: "غراس للنقل",
  appTagline: "نظام إدارة نقل الموظفين",

  // nav
  "nav.dashboard": "لوحة المعلومات",
  "nav.deductions": "الخصومات",
  "nav.statement": "كشف حساب الموردين",
  "nav.employees": "الموظفون (الركاب)",
  "nav.users": "المستخدمون",
  "nav.exceptions": "الاستثناءات",
  "nav.repricing": "إعادة التسعير",
  "nav.transportation": "إدارة النقل",
  "nav.lines": "الخطوط",
  "nav.vehicles": "المركبات",
  "nav.shifts": "أيام وساعات العمل",
  "nav.suppliers": "الموردون",
  "nav.attendance": "تقرير الحضور",

  // common actions
  "action.add": "إضافة",
  "action.addNew": "إضافة جديد",
  "action.edit": "تعديل",
  "action.delete": "حذف",
  "action.save": "حفظ",
  "action.submit": "إرسال",
  "action.cancel": "إلغاء",
  "action.approve": "اعتماد",
  "action.reject": "رفض",
  "action.view": "عرض",
  "action.search": "بحث",
  "action.close": "إغلاق",
  "action.details": "التفاصيل",
  "action.downloadExcel": "تنزيل Excel",
  "action.uploadExcel": "رفع Excel",
  "action.print": "طباعة",
  "action.confirm": "تأكيد",
  "action.pickLocation": "تحديد الموقع",
  "action.viewRoutes": "عرض المسارات",
  "action.addRoute": "إضافة مسار",
  "action.logout": "تسجيل الخروج",
  "action.signIn": "تسجيل الدخول",
  "action.menu": "القائمة",

  // status
  "status.approved": "معتمد",
  "status.notApproved": "غير معتمد",
  "status.pending": "قيد الاعتماد",
  "status.active": "نشط",
  "status.inactive": "غير نشط",
  "status.attended": "حاضر",
  "status.absent": "غائب",
  "status.available": "متاح",

  // periods
  "period.go": "ذهاب",
  "period.return": "عودة",
  "period.both": "ذهاب وعودة",

  // roles
  "role.systemAdmin": "مدير النظام",
  "role.superAdmin": "المشرف العام للنقل",
  "role.lineAdmin": "مدير الخطوط",
  "role.transAdmin": "مسؤول النقل",
  "role.supervisor": "مشرف الحافلة",
  "role.reader": "قارئ",

  // login
  "login.title": "تسجيل الدخول",
  "login.subtitle": "ادخل إلى نظام إدارة النقل",
  "login.email": "البريد الإلكتروني",
  "login.password": "كلمة المرور",
  "login.remember": "تذكرني",
  "login.invalid": "بيانات الدخول غير صحيحة",
  "login.noPerms": "هذا الحساب لا يملك صلاحيات النقل",

  // dashboard
  "dash.title": "لوحة المعلومات",
  "dash.routeRail": "المسارات",
  "dash.lineCostReport": "تقرير تكلفة الخطوط",
  "dash.viewAttendance": "عرض الحضور",
  "dash.touchAttendance": "تسجيل حضور إلكتروني",
  "dash.allRoutes": "كل المسارات",

  // filters
  "filter.supplier": "المورد",
  "filter.driver": "السائق",
  "filter.serial": "رقم الحافلة",
  "filter.date": "التاريخ",
  "filter.line": "الخط",
  "filter.route": "المسار",
  "filter.month": "الشهر",
  "filter.year": "السنة",
  "filter.from": "من تاريخ",
  "filter.to": "إلى تاريخ",
  "filter.selectSupplierFirst": "اختر موردًا أولًا",
  "filter.all": "الكل",

  // empty states
  "empty.routes": "لا توجد مسارات بعد. أضف أول مسار.",
  "empty.attendance": "لا توجد سجلات حضور للمرشحات المحددة.",
  "empty.statement": "لا يوجد نشاط حسابي لهذا الشهر.",
  "empty.repricing": "لا توجد طلبات إعادة تسعير.",
  "empty.generic": "لا توجد بيانات.",

  common_of: "من",
  common_perPage: "لكل صفحة",
  common_required: "مطلوب",
};

const en: Dict = {
  appName: "Garas Transport",
  appTagline: "Employee Transportation Management",

  "nav.dashboard": "Dashboard",
  "nav.deductions": "Deductions",
  "nav.statement": "Suppliers Statement",
  "nav.employees": "Employees",
  "nav.users": "Users",
  "nav.exceptions": "Exceptions",
  "nav.repricing": "Repricing",
  "nav.transportation": "Transportation",
  "nav.lines": "Lines",
  "nav.vehicles": "Vehicles",
  "nav.shifts": "Working Days & Hours",
  "nav.suppliers": "Suppliers",
  "nav.attendance": "Attendance Report",

  "action.add": "Add",
  "action.addNew": "Add New",
  "action.edit": "Edit",
  "action.delete": "Delete",
  "action.save": "Save",
  "action.submit": "Submit",
  "action.cancel": "Cancel",
  "action.approve": "Approve",
  "action.reject": "Reject",
  "action.view": "View",
  "action.search": "Search",
  "action.close": "Close",
  "action.details": "Details",
  "action.downloadExcel": "Download Excel",
  "action.uploadExcel": "Upload Excel",
  "action.print": "Print",
  "action.confirm": "Confirm",
  "action.pickLocation": "Pick Location",
  "action.viewRoutes": "View Routes",
  "action.addRoute": "Add Route",
  "action.logout": "Logout",
  "action.signIn": "Sign In",
  "action.menu": "Menu",

  "status.approved": "Approved",
  "status.notApproved": "Not Approved",
  "status.pending": "Pending",
  "status.active": "Active",
  "status.inactive": "Inactive",
  "status.attended": "Attended",
  "status.absent": "Absent",
  "status.available": "Available",

  "period.go": "Go",
  "period.return": "Return",
  "period.both": "Both",

  "role.systemAdmin": "System Admin",
  "role.superAdmin": "Super Admin",
  "role.lineAdmin": "Line Admin",
  "role.transAdmin": "Transportation Admin",
  "role.supervisor": "Bus Supervisor",
  "role.reader": "Reader",

  "login.title": "Sign In",
  "login.subtitle": "Access the transportation system",
  "login.email": "Email",
  "login.password": "Password",
  "login.remember": "Remember me",
  "login.invalid": "Invalid credentials",
  "login.noPerms": "This account has no transportation permissions",

  "dash.title": "Dashboard",
  "dash.routeRail": "Routes",
  "dash.lineCostReport": "Line Cost Report",
  "dash.viewAttendance": "View Attendance",
  "dash.touchAttendance": "Electronic Touch Attendance",
  "dash.allRoutes": "All routes",

  "filter.supplier": "Supplier",
  "filter.driver": "Driver",
  "filter.serial": "Bus Serial",
  "filter.date": "Date",
  "filter.line": "Line",
  "filter.route": "Route",
  "filter.month": "Month",
  "filter.year": "Year",
  "filter.from": "From",
  "filter.to": "To",
  "filter.selectSupplierFirst": "Select a supplier first",
  "filter.all": "All",

  "empty.routes": "No routes yet. Add the first route.",
  "empty.attendance": "No attendance records for the selected filters.",
  "empty.statement": "No account activity for this month.",
  "empty.repricing": "No repricing requests.",
  "empty.generic": "No data.",

  common_of: "of",
  common_perPage: "per page",
  common_required: "Required",
};

const dictionaries: Record<Locale, Dict> = { ar, en };

export function getDict(locale: Locale): Dict {
  return dictionaries[locale] ?? ar;
}

/** translate key; falls back to the key itself if missing */
export function translate(locale: Locale, key: string): string {
  return getDict(locale)[key] ?? key;
}
