# جاراس — نظام إدارة نقل الموظفين

| | |
|---|---|
| **الفرونت** | `app/` — React 19 + Vite + TypeScript (عربي RTL / إنجليزي) |
| **الباك** | `backend-dotnet/` — ASP.NET Core (.NET 8) + EF Core + SQL Server |

الباك نسخة طبق الأصل من الـ **CoreApi** الحقيقي (`NewGaras.Domain` / `NewGaras.Infrastructure` /
`NewGarasAPI`) — متعدد الشركات (multi-tenant)، كل شركة قاعدة بيانات مستقلة.

---

## المتطلبات

| | ملاحظة |
|---|---|
| **.NET SDK 8** أو أحدث | لو عندك .NET 9/10 بس، الـ csproj فيه `RollForward=LatestMajor` فهيشتغل عادي |
| **SQL Server 2019+** | أي نسخة (Developer / Express) |
| **Node.js 20+** | للفرونت |
| نسخة احتياطية `.bak` لقاعدة البيانات | اطلبها من فريق العمل |

---

## 1) قاعدة البيانات

استعِد النسخة الاحتياطية. **اسم قاعدة البيانات مهم**: الكود فيه جدول ثابت في
`NewGarasAPI/Helper/Helper.cs` بيحوّل اسم الشركة لاسم قاعدة بيانات، فلازم يتطابقوا.

| اسم الشركة (تسجيل الدخول) | اسم قاعدة البيانات |
|---|---|
| `elwaseem` | `GARASElWaseem` |
| `garastest` | `GARASTest2` |
| `pharma` | `GARASTransportation` ⚠️ اقرأ التحذير تحت |

```sql
RESTORE DATABASE [GARASElWaseem] FROM DISK = 'C:\path\to\backup.bak'
WITH MOVE 'GARAS'     TO 'C:\...\MSSQL\DATA\GARASElWaseem.mdf',
     MOVE 'GARAS_log' TO 'C:\...\MSSQL\DATA\GARASElWaseem_log.ldf';
```

> **لو ظهر `Operating system error 5 (Access is denied)`** — خدمة SQL Server مش بتقدر
> تقرا من مجلد التنزيلات. انقل ملف `.bak` لمجلد زي `C:\SqlRestore` وأدِّ صلاحية قراءة
> لحساب الخدمة: `icacls C:\SqlRestore /grant "NT Service\MSSQLSERVER:(OI)(CI)R" /T`

### تعديلان لازمان بعد الاستعادة

النسخ الاحتياطية أقدم من الكود، فناقصها حاجتين — من غيرهم شاشات هتقع:

```sql
USE [GARASElWaseem];

-- 1) عمود بيتوقعه الكيان ومش موجود في النسخ القديمة.
--    من غيره: شاشة الخطوط ولوحة المعلومات بترجّع "Invalid column name 'currentCost'".
IF COL_LENGTH('TransportationVehicleRoute','currentCost') IS NULL
    ALTER TABLE TransportationVehicleRoute ADD currentCost decimal(10,2) NULL;
GO

-- 2) الـ view ده وصلة لسيرفر جهاز البصمة (attsql) ومش موجود برة الشبكة.
--    الكنترولر بيقراه في أول سطر من getAllVehicleType (والنتيجة مش مستخدمة أصلًا)،
--    فمن غير ده شاشة المركبات كلها بتفشل.
ALTER VIEW dbo.v_Bus_Program AS
SELECT CAST(NULL AS int)           AS Employee_Number,
       CAST(NULL AS nvarchar(200)) AS Emp_Name_Ara,
       CAST(NULL AS nvarchar(200)) AS BusType,
       CAST(NULL AS nvarchar(200)) AS [TYPE],
       CAST(NULL AS nvarchar(50))  AS Check_Date,
       CAST(NULL AS nvarchar(50))  AS Check_time
WHERE 1 = 0;
GO
```

> لو وصّلت سيرفر البصمة فعلًا، رجّع تعريف الـ view الأصلي:
> `SELECT … FROM attsql.Attendance.dbo.v_Bus_Program`

### (اختياري) تنضيف الجداول غير المستخدمة

النسخة الاحتياطية بتيجي بـ 492 جدول لأنها من الـ ERP الكامل. السكريبت ده بيشيل
**52 جدول** مالهاش أي علاقة بالنقل (BOM، فواتير إلكترونية، مشتريات، HR، مشاريع):

```
backend-dotnet/db/drop-unused-tables.sql
```

كل جدول فيه فاضي، ومفيش مفتاح أجنبي بيشاور عليه، **واسمه مش موجود في كود C# خالص**.
السكريبت آمن لو اتشغّل أكتر من مرة، وفي آخره فحص بيطبع جداول النقل عشان تتأكد إنها سليمة.

> **مش مطلوب للتشغيل** — التطبيق شغال عادي من غيره. غرضه إن قاعدة البيانات تبقى
> متطابقة عند كل الفريق. خد نسخة احتياطية قبله زي ما مكتوب في أول الملف.

---

## 2) الباك إند

عدّل `backend-dotnet/NewGarasAPI/appsettings.json` → `TenantSettings.Tenants` وخلّي
سطر شركتك بيشاور على السيرفر بتاعك:

```json
{
  "Name": "elwaseem",
  "TID": "elwaseem",
  "ConnectionString": "Server=localhost;Initial Catalog=GARASElWaseem;Integrated Security=True;MultipleActiveResultSets=True;TrustServerCertificate=True"
}
```

التشغيل:

```bash
cd backend-dotnet/NewGarasAPI
dotnet run --launch-profile http
```

يسمع على **http://0.0.0.0:8888** — و Swagger على http://localhost:8888/swagger

### قوالب الإكسيل
التصدير بيفتح قوالب `.xlsx` جاهزة من `NewGarasAPI/wwwroot/Attachments/PharmaTemplate/`
(متضمَّنة في الريبو). لو مسحتها هتظهر رسالة **"The Templete File is not exsists"**.

---

## 3) الفرونت إند

```bash
cd app
npm install
npm run dev
```

يفتح على **http://localhost:3400**.

عنوان الـ API بيتقرا من متغير بيئة — اعمل `app/.env.development`:

```
VITE_API_URL=http://localhost:8888
```

للبناء للنشر: `npm run build` (بيقرا `app/.env.production`) والناتج في `app/dist/`.

---

## 4) تسجيل الدخول

شاشة الدخول بتطلب **اسم الشركة** لأن النظام متعدد الشركات — الاسم ده بيحدد قاعدة
البيانات اللي هتشتغل عليها، وبيتبعت مع كل طلب في هيدر `CompanyName`.

```
اسم الشركة : elwaseem
البريد     : ibrahim@system.com
كلمة السر  : 654321
```

كلمات السر متشفّرة في قاعدة البيانات بـ AES (نفس منطق `Encrypt_Decrypt`)،
والاسم بيتحفظ محليًا فمش هتكتبه كل مرة.

> ### ⚠️ لا تستخدم اسم الشركة `pharma` للتطوير
> `UserController.Login` فيه فحص ترخيص **بيشتغل مع الاسم ده بالتحديد** وبيقارن بصمة
> اللوحة الأم والـ UUID بجهاز واحد بعينه. على أي جهاز تاني هيرفض بـ
> `Err-P8 — Unauthorized Hardware ID`. استخدم `elwaseem` (اسم موجود أصلًا في قائمة
> `ValidateCompanyName` المسموحة) وسمِّ قاعدة البيانات `GARASElWaseem`.

---

## البنية

```
app/                        الفرونت (React + Vite)
  lib/services/             طبقة نداءات الـ API — كل ملف يقابل شاشة
  lib/api/client.ts         الـ HTTP client (هيدرز CompanyName + UserToken، multipart)
  pages/ · components/      الشاشات والمكوّنات

backend-dotnet/
  NewGaras.Domain/          الخدمات (TransportationLineService ≈ 11 ألف سطر)
  NewGaras.Infrastructure/  الكيانات · الموديلات · Repositories · UnitOfWork · TenantService
  NewGarasAPI/              الكنترولرات · Program.cs · appsettings.json
```

## توثيق إضافي

| | |
|---|---|
| [`backend-dotnet/MIGRATION-NOTES.md`](backend-dotnet/MIGRATION-NOTES.md) | خريطة أسماء الـ API، والفروقات عن النسخة المرجعية (إعدادات بيئة فقط) |
| [`API-AUDIT.md`](API-AUDIT.md) | فحص شاشة-بشاشة لكل نداء API (39 مشكلة وحلولها) |
| [`UNUSED-TABLES.md`](UNUSED-TABLES.md) | الجداول غير المستخدمة في قاعدة البيانات (عرض فقط) |
| [`docs/`](docs/) | وثائق المنتج والشاشات والنماذج |

---

## أخطاء شائعة

| الرسالة | السبب والحل |
|---|---|
| `Err-P7 — Invalid Company Name` | الاسم مش في قائمة `ValidateCompanyName` المسموحة |
| `Err-P8 — Unauthorized Hardware ID` | استخدمت `pharma` — استخدم `elwaseem` |
| `Cannot open database "…"` | اسم قاعدة البيانات مش مطابق للجدول في `Helper.cs` |
| `Invalid column name 'currentCost'` | نفّذ تعديل قاعدة البيانات رقم (1) فوق |
| `Could not find server 'attsql'` | نفّذ تعديل قاعدة البيانات رقم (2) فوق |
| `The Templete File is not exsists` | قوالب الإكسيل ناقصة من `wwwroot` |
| الباك مش بيشتغل / البورت مشغول | فيه نسخة شغالة: `taskkill /IM NewGarasAPI.exe /F` |
