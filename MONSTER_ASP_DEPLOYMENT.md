# دليل النشر على Monster ASP.NET
# Monster ASP.NET Deployment Guide

## معلومات قاعدة البيانات / Database Information

✅ **تم التحديث في appsettings.Production.json**

- **Server**: db33059.databaseasp.net
- **Database**: db33059
- **Username**: db33059
- **Password**: 5Zg-F%6p9Kw_
- **Connection String**: تم إضافته في appsettings.Production.json

---

## خطوات النشر على Monster ASP.NET / Deployment Steps

### الخطوة 1: بناء المشروع / Step 1: Build Project

```powershell
# بناء المشروع للإنتاج
dotnet publish --configuration Release --output "C:\Publish\HavitGroup"
```

### الخطوة 2: رفع الملفات / Step 2: Upload Files

1. قم بتسجيل الدخول إلى لوحة تحكم Monster ASP.NET
2. افتح File Manager أو استخدم FTP
3. ارفع جميع الملفات من مجلد `C:\Publish\HavitGroup` إلى مجلد الموقع على الخادم

**ملاحظة مهمة**: تأكد من رفع:
- ✅ جميع ملفات .dll
- ✅ مجلد wwwroot (بما فيه الصور والـ CSS)
- ✅ مجلد Views
- ✅ مجلد Areas
- ✅ ملف web.config
- ✅ ملف appsettings.Production.json

### الخطوة 3: تطبيق Migrations على قاعدة البيانات / Step 3: Apply Migrations

#### الطريقة الأولى: استخدام dotnet ef محلياً

```powershell
# من جهازك المحلي
dotnet ef database update --connection "Server=db33059.databaseasp.net;Database=db33059;User Id=db33059;Password=5Zg-F%6p9Kw_;Encrypt=False;MultipleActiveResultSets=True;TrustServerCertificate=True"
```

#### الطريقة الثانية: استخدام WebMSSQL Interface

1. افتح: https://webmssql.monsterasp.net
2. سجل الدخول باستخدام بيانات قاعدة البيانات
3. قم بتنفيذ SQL Scripts يدوياً من مجلد Migrations

### الخطوة 4: إعدادات IIS على Monster ASP.NET / Step 4: IIS Settings

1. في لوحة تحكم Monster ASP.NET:
   - تأكد من أن Application Pool يستخدم .NET 9.0
   - تأكد من أن ASP.NET Core Module مثبت
   - تحقق من أن Environment Variable `ASPNETCORE_ENVIRONMENT` = `Production`

2. تأكد من أن ملف `web.config` موجود في المجلد الرئيسي

### الخطوة 5: اختبار الموقع / Step 5: Test Website

1. افتح الموقع في المتصفح
2. تحقق من أن الصفحة الرئيسية تظهر بشكل صحيح
3. اختبر الصفحات المختلفة:
   - Home
   - About Us
   - Services
   - Contact Us
   - Admin Dashboard (/Admin/Dashboard)

---

## استكشاف الأخطاء / Troubleshooting

### مشكلة: خطأ في الاتصال بقاعدة البيانات

**الحل:**
- تأكد من أن Connection String صحيح في appsettings.Production.json
- تأكد من أن قاعدة البيانات نشطة في لوحة تحكم Monster ASP.NET
- تحقق من أن Password يحتوي على `%` ويجب أن يكون `5Zg-F%6p9Kw_`

### مشكلة: الصور لا تظهر

**الحل:**
- تأكد من رفع مجلد `wwwroot/Images` بالكامل
- تحقق من صلاحيات القراءة على المجلدات
- تأكد من أن المسارات في الكود صحيحة (`~/Images/...`)

### مشكلة: Migrations لم تُطبق

**الحل:**
- استخدم `dotnet ef database update` مع Connection String الكامل
- أو قم بتنفيذ SQL Scripts يدوياً من WebMSSQL Interface

### مشكلة: الموقع يعرض خطأ 500

**الحل:**
- تحقق من سجلات الأخطاء في لوحة تحكم Monster ASP.NET
- تأكد من أن جميع ملفات .dll موجودة
- تحقق من أن web.config موجود وصحيح

---

## نصائح مهمة / Important Tips

1. **الأمان / Security:**
   - لا تشارك كلمة مرور قاعدة البيانات
   - استخدم HTTPS إذا كان متاحاً
   - تأكد من أن Admin Dashboard محمي

2. **النسخ الاحتياطي / Backup:**
   - قم بعمل نسخة احتياطية من قاعدة البيانات بانتظام
   - احتفظ بنسخة من ملفات النشر

3. **الأداء / Performance:**
   - قم بتمكين Caching في IIS
   - استخدم CDN للصور إذا أمكن

---

## الدعم / Support

إذا واجهت أي مشاكل:
- تحقق من سجلات الأخطاء في لوحة تحكم Monster ASP.NET
- راجع ملفات السجلات في مجلد logs (إذا كان stdoutLogEnabled = true)
- تواصل مع دعم Monster ASP.NET

---

## ملاحظات إضافية / Additional Notes

- ✅ Connection String محدث في appsettings.Production.json
- ✅ web.config جاهز للنشر
- ✅ المشروع يبني بنجاح
- ⚠️ تذكر تطبيق Migrations على قاعدة البيانات قبل تشغيل الموقع

