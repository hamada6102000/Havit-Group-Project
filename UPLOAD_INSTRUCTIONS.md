# تعليمات رفع الملفات على Monster ASP.NET
# Upload Instructions for Monster ASP.NET

## الخطوة 1: بناء المشروع / Step 1: Build Project

قم بتنفيذ الأمر التالي في PowerShell أو Command Prompt:

```powershell
dotnet publish --configuration Release --output "C:\Publish\HavitGroup"
```

هذا سينشئ مجلد `C:\Publish\HavitGroup` يحتوي على جميع الملفات المطلوبة للنشر.

---

## الخطوة 2: رفع الملفات عبر FTP / Step 2: Upload Files via FTP

### 2.1 الحصول على معلومات FTP

من لوحة تحكم Monster ASP.NET:
1. اذهب إلى **FTP Accounts** أو **File Manager**
2. سجل معلومات FTP:
   - FTP Server/Host
   - Username
   - Password
   - Port (عادة 21)

### 2.2 استخدام FileZilla أو أي FTP Client

1. افتح FileZilla أو أي برنامج FTP
2. أدخل معلومات FTP
3. اتصل بالخادم
4. اذهب إلى مجلد الموقع (عادة `wwwroot` أو `httpdocs` أو `public_html`)
5. ارفع جميع الملفات من `C:\Publish\HavitGroup` إلى المجلد على الخادم

**ملاحظة مهمة**: تأكد من رفع:
- ✅ جميع ملفات .dll
- ✅ مجلد wwwroot (بما فيه الصور والـ CSS والـ JS)
- ✅ مجلد Views
- ✅ مجلد Areas
- ✅ ملف web.config
- ✅ ملف appsettings.Production.json
- ✅ ملف appsettings.json

### 2.3 استخدام File Manager في لوحة التحكم

1. سجل الدخول إلى لوحة تحكم Monster ASP.NET
2. افتح **File Manager**
3. اذهب إلى مجلد الموقع
4. ارفع الملفات باستخدام زر Upload

---

## الخطوة 3: تطبيق Migrations على قاعدة البيانات / Step 3: Apply Database Migrations

### الطريقة الأولى: من جهازك المحلي (موصى به)

```powershell
# تأكد من أنك في مجلد المشروع
cd "C:\Users\hm245\source\repos\Havit-Group-Project"

# تطبيق Migrations
dotnet ef database update --connection "Server=db33059.databaseasp.net;Database=db33059;User Id=db33059;Password=5Zg-F%6p9Kw_;Encrypt=False;MultipleActiveResultSets=True;TrustServerCertificate=True"
```

### الطريقة الثانية: استخدام WebMSSQL Interface

1. افتح: https://webmssql.monsterasp.net
2. سجل الدخول:
   - Server: db33059.databaseasp.net
   - Username: db33059
   - Password: 5Zg-F%6p9Kw_
3. افتح قاعدة البيانات `db33059`
4. قم بتنفيذ SQL Scripts من مجلد `Migrations` في المشروع

---

## الخطوة 4: التحقق من الإعدادات / Step 4: Verify Settings

### 4.1 التحقق من web.config

تأكد من أن ملف `web.config` موجود في المجلد الرئيسي ويحتوي على:

```xml
<environmentVariable name="ASPNETCORE_ENVIRONMENT" value="Production" />
```

### 4.2 التحقق من appsettings.Production.json

تأكد من أن Connection String صحيح:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=db33059.databaseasp.net;Database=db33059;User Id=db33059;Password=5Zg-F%6p9Kw_;Encrypt=False;MultipleActiveResultSets=True;TrustServerCertificate=True"
  }
}
```

---

## الخطوة 5: اختبار الموقع / Step 5: Test Website

1. افتح الموقع في المتصفح (الرابط الذي أعطاك إياه Monster ASP.NET)
2. تحقق من أن الصفحة الرئيسية تظهر بشكل صحيح
3. اختبر الصفحات:
   - Home (/)
   - About Us (/Home/About)
   - Services (/Home/Services)
   - Contact Us (/Home/Contact)
   - Admin Dashboard (/Admin/Dashboard)

---

## استكشاف الأخطاء الشائعة / Common Issues

### خطأ: الموقع لا يعمل / Website Not Working

**الحل:**
1. تحقق من أن جميع ملفات .dll موجودة
2. تأكد من وجود ملف web.config
3. تحقق من سجلات الأخطاء في لوحة تحكم Monster ASP.NET

### خطأ: قاعدة البيانات غير متصلة / Database Connection Error

**الحل:**
1. تأكد من أن Connection String صحيح في appsettings.Production.json
2. تحقق من أن قاعدة البيانات نشطة
3. تأكد من تطبيق Migrations

### خطأ: الصور لا تظهر / Images Not Loading

**الحل:**
1. تأكد من رفع مجلد `wwwroot/Images` بالكامل
2. تحقق من صلاحيات القراءة على المجلدات
3. تأكد من أن المسارات في الكود صحيحة

---

## نصائح مهمة / Important Tips

1. **الترتيب الصحيح**:
   - أولاً: ارفع الملفات
   - ثانياً: طبق Migrations
   - ثالثاً: اختبر الموقع

2. **النسخ الاحتياطي**:
   - احتفظ بنسخة من الملفات قبل التعديل
   - قم بعمل نسخة احتياطية من قاعدة البيانات

3. **الأمان**:
   - لا تشارك كلمة مرور قاعدة البيانات
   - تأكد من أن Admin Dashboard محمي

---

## الدعم / Support

إذا واجهت أي مشاكل:
- راجع سجلات الأخطاء في لوحة تحكم Monster ASP.NET
- تواصل مع دعم Monster ASP.NET
- راجع ملف `MONSTER_ASP_DEPLOYMENT.md` للمزيد من التفاصيل

