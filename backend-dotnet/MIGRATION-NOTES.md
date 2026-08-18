# backend-dotnet — نسخة طبق الأصل من CoreApi

هذا المجلد **نسخة حرفية** من مشروع `garas-core-transportation-api` (الـ CoreApi الحقيقي)،
بنفس الستركتشر وتقسيمة الملفات وأسماء الـ API والانترفيسات. البنية:

```
backend-dotnet/
├── NewGaras.Domain/          # Helper, Mappers, Services (TransportationLineService = 11k سطر)
├── NewGaras.Infrastructure/  # Entities (606), Models, DTO, Interfaces, Repositories, UnitOfWork, TenantService
├── NewGarasAPI/              # Controllers, Hubs, Program.cs, appsettings.json
└── NewGarasAPI.sln
```

## التشغيل

```bash
cd backend-dotnet/NewGarasAPI && dotnet run --launch-profile http
```

يسمع على `http://0.0.0.0:8888` وSwagger على `/swagger`.

## اللوجين والشركات (multi-tenant)

`POST /User/Login` بالـ body:

```json
{ "Email": "...", "Password": "...", "CompanyName": "garastest" }
```

`CompanyName` هو الـ `TID` في `appsettings.json → TenantSettings.Tenants`، وهو اللي بيحدد
قاعدة البيانات. كل الطلبات بعد كده بتبعت هيدر `CompanyName` + `UserToken`.

حساب شغّال للتجربة: `system@system.com` / `4321` مع الشركة `garastest`.

## الفروقات عن النسخة المرجعية (إعدادات بيئة فقط، مش كود)

| التغيير | السبب |
|---|---|
| `appsettings.json`: كل `DESKTOP-8TJO6OM` → `localhost` | السيرفر المحلي |
| `launchSettings.json`: بروفايل http → `http://0.0.0.0:8888` | البورت اللي الفرونت بينده عليه |
| `NewGarasAPI.csproj`: `<RollForward>LatestMajor</RollForward>` | مفيش .NET 8 runtime على الجهاز، بس 9 و10 |

## ملفات مضافة من الـ CoreApi الكامل (`D:\hany\garas-core-api`)

النسخة المرجعية اتقصّت على موديول النقل، فكانت ناقصة كنترولر الموردين اللي الفرونت محتاجه.
اتنسخ من المشروع الكامل بنفس الكود والأسماء:

- `NewGarasAPI/Controllers/SupplierController.cs` — `AddNewSupplier`, `GetSuppliersCards`,
  `GetSupplierList`, `GetSupplierDataResponse`, `GetSupplierContactPersonsResponse` …
- `NewGaras.Domain/Services/SupplierService.cs` + `LogService.cs`
- الموديلات اللي بتعتمد عليها فقط (Supplier/Client/Inventory/HR/Log) — مش الموديول كامل.
- `ISupplierService`: أُضيف تعريف `AddSupplierAttachments` (كان ناقص في النسخة المقصوصة
  رغم وجود التنفيذ) — منقول حرفيًا من الانترفيس الأصلي.
- شيلنا 4 أسطر `using` لموديولات غير موجودة (TaskMangerProject / Client / PurchaseOrder /
  DTO.Contract) وكانت **غير مستخدمة** في الملفات دي أصلًا.

`Controllers/HR/HrUserController.cs` **لم يُضف**: كنترولر موديول HR (إجازات، مرفقات، مسميات
وظيفية) وبيتطلب `IHrUserService` كامل. احتياجات الركاب في النقل مغطّاة أصلًا داخل
`TransportationController` (`getAllUsers`, `CreateHrUserWithAllRoutes`, `GetTransportationEmployeeList` …)
و`DDLController/MaritalStatus`.

## قاعدة البيانات

`D:\hany\GarasTest-2026-07-06.bak` تم استعادته باسم **GARASTest2** (496 جدول، 228 مورد،
15 مستخدم، 216 رول). جداول النقل فاضية — الداتا اللي في سكرين شوت النظام القديم على سيرفر
الإنتاج مش في الباك أب ده.

`Attendance-2026-07-06.bak` استُعيد باسم `PharmaTransportation` لكنه يحتوي جدول `bus` فقط.

## خريطة الفرونت → الأسماء الحقيقية

الأسماء اللي كانت متخترعة في الفرونت اتظبطت كلها على الـ API الحقيقي:

| الفرونت (قديم) | الحقيقي |
|---|---|
| `getSuppliers` | `/Supplier/GetSuppliersCards` (هيدرز `CurrentPage`/`NumberOfItemsPerPage`/`SupplierName`) |
| `getSupplier` | `/Supplier/GetSupplierDataResponse` (هيدر `SupplierId`) |
| `addSupplier` / `updateSupplier` | `/Supplier/AddNewSupplier` (+ `AddNewSupplierContactPerson` للسائقين) |
| `getAllHrUsers` | `getAllUsers` — وفلاتر النطاق (`RouteId`/`SupplierId`/`serialBus`/`supplierContactPersonId`) مدمجة فيه أصلًا |
| `getHrUser` | `/HrUser/GetHrUser` (هيدر `HrUserId`) |
| `AddHrUser` | `CreateHrUserWithAllRoutes` (multipart `[FromForm]`) |
| `UpdateHrUser` | `/HrUser/EditHrEmployee` (multipart) |
| `GetMaritalStatus` | `/DDL/MaritalStatus` |
| `GetNotifications` | `/Notification/GetNotifications` (الحقل `New` = غير مقروء) |
| `GetUnreadNotificationsCount` | مشتق من `GetNotifications` (مفيش endpoint للعدّ) |
| `MarkNotificationRead` / `MarkAllNotificationsRead` | `/Notification/EditNotifications` (نداء لكل صف) |
| `DashBoardForBusAttendenceDuration` | `BusAttendance` |
| `BusAttendanceExcell` | `BusAttendanceExcel` |
| `RoutesWithUsersExcel` | `RoutesWithUsersExcell` |
| `downloadExcelUsersList` | `downloadExcelUserWithRoutesTemplete` |
| `ApproveTransportationLine` | `UpdateTransportationLine` بحقل `IsApproved` (مفيش endpoint اعتماد للخط) |
| `/User/GetAllUsers` | `/User/GetUserList` (بيرجّع `DDLList`) |

أُضيف `NewGarasAPI/Controllers/HR/HrUserController.cs` — نسخة مصغّرة من كنترولر
الـ HR بنفس الأسماء والكود (`GetHrUser` / `CreateHrUser` / `EditHrEmployee` /
`AddHrEmployeeToUser`)، وكلها ميثودز معرّفة أصلًا في `IHrUserService`.

كمان أُضيف `apiForm` في `app/lib/api/client.ts` عشان الـ endpoints اللي `[FromForm]`.

### شاشة المستخدمين — محتاجة قرار
الـ CoreApi مفيهوش `AddUser`/`UpdateUser`/`DeleteUser`. إنشاء حساب دخول في النظام
الحقيقي بيتم على مرحلتين: إنشاء موظف (`CreateHrUser`) ثم منحه دخول
(`AddHrEmployeeToUser`)، والأدوار بتتظبط من موديول الـ HR. العرض شغّال
(`/User/GetUserList`)، والإضافة/التعديل بترجّع رسالة واضحة لحد ما نقرر الشكل.

## تعديل على الداتابيز (مش على الكود)

النسخة الاحتياطية (2026-07-06) **أقدم من الكود**: الكيان `TransportationVehicleRoute`
فيه عمود `currentCost` مش موجود في الجدول، فكان `getAllTransportationRoute` و
`getAllRoutes` بيفشلوا. الحل كان على الداتابيز عشان الكود يفضل مطابق للمرجع:

```sql
ALTER TABLE TransportationVehicleRoute ADD currentCost decimal(10,2) NULL;
```

الخطأ كان متخبّي لأن الـ `catch` في `TransportationLineService.cs:3209` بيقرأ
`ex.InnerException.Message` والـ InnerException كان `null` — فبيرمي
`NullReferenceException` بدل رسالة SQL الحقيقية (`Invalid column name 'currentCost'`).
ده سلوك الكود المرجعي وسبناه زي ما هو.

قارنّا كل أعمدة كيانات النقل بالجداول — ده كان الاختلاف الوحيد.

## دروب داون الموردين والسائقين

مفيش endpoint واحد بيرجّع الموردين ومعاهم جهات الاتصال. الصفحات كانت بتفترض
`getSuppliers` بيرجّع `contacts` جوه كل مورد. دلوقتي:
- قائمة الموردين ← `/Supplier/GetSuppliersCards`
- سائقو المورد ← `/Supplier/GetSupplierContactPersonsResponse` عند اختيار المورد

الاتنين متغلّفين في `app/lib/hooks/useSuppliers.ts` (`useSupplierOptions` /
`useDriverOptions`) وبيستخدمهم الداشبورد وتقرير الحضور وحضور الاتوبيسات
والتكاليف ونموذج الخط.

## أخطاء في الـ payloads (كانت بتمنع الحفظ)

مقارنة آلية بين كل `apiPost` body في الفرونت وبين الـ DTO المقابل في الباك كشفت:

| المكان | كان | الصح | الأثر |
|---|---|---|---|
| `routes.ts` → `AddTransportationRoute` / `Update…` | `FromDate` | `FromoDate` (غلطة مجمّدة) | وقت الذهاب كان بيتجاهل |
| نفسه | `"06:00"` (وقت فقط) | `DateTime` كامل | **400 Bad Request** — الحفظ بيفشل تمامًا |
| نفسه | `PeriodFrom` مش مبعوت | مبعوت | `01/01/0001` بيترفض من SQL |
| نفسه | كان بيبعت `Serial` | مش مبعوت | الباك بيولّده تلقائيًا (آخر مسلسل + 1) |
| `repricing.ts` → `ModifyPriceOfTransportationLine` | `IsPercent` | `IsPrecent` | النسبة المئوية كانت بتتحفظ كمبلغ ثابت |

⚠ ملاحظة: الـ **read** model بيرجّع `IsPercent` بالكتابة الصحيحة، والـ **write** DTO
بيتوقع `IsPrecent` — الاتنين مختلفين وده مقصود في الكود المرجعي.

باقي الـ writes (`AddException` / `AddDeduction` / `AddTransportationDirection` /
`AddUsersAttedance` / `AddTransportationVehicle` / `AddVehicleType`) اتأكدنا منها
واحدة واحدة وكلها مطابقة.

### محطة التجمع مش إجبارية — الباك بيعملها لوحده

`AddTransportationRoute` فيه المنطق ده:

```csharp
if (dto.TransportationLineId <= 0 && string.IsNullOrEmpty(dto.TransportationLineName))
    → "add parameter"

if (!string.IsNullOrEmpty(dto.TransportationLineName))
    NewTransportationLine = _unitOfWork.TransportationLines.Add(new TransportationLine { LineName = dto.TransportationLineName, … });

TransportationLineId = NewTransportationLine.Id > 0 ? NewTransportationLine.Id : dto.TransportationLineId
```

يعني لو المستخدم مختارش محطة، بيتبعت **`TransportationLineName`** (= اسم الخط)
والباك بينشئ محطة بنفس الاسم ويربط الخط بيها. الفرونت **ماكانش بيبعت الحقل ده
خالص** — فكان بيرجّع "add parameter" ومعاه خطأ FK.

`UpdateTransportationRoute` **مفيهوش** المسار ده (بياخد `TransportationLineId`
مباشرة)، فالاسم بيتبعت في الإضافة بس.

## ملاحظة على سلوك موروث

`GET /api/Transportation/getAllTransportationRoute` بيرجّع
`Err10: Object reference not set` لما تكون جداول الخطوط فاضية — ده سلوك الكود المرجعي نفسه
(مش تعديل منّا)، وبيختفي أول ما تتسجل خطوط.
