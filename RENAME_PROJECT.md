# Project Renaming Guide

## New Names
- **Project Name**: CompanyWebsite
- **Namespace**: CompanyWebsite
- **Database Name**: CompanyWebsiteDb

## Steps to Rename

### 1. Rename Project Files
- FreeLance.csproj → CompanyWebsite.csproj
- FreeLance.sln → CompanyWebsite.sln

### 2. Update Namespaces
All C# files with `namespace FreeLance` → `namespace CompanyWebsite`

### 3. Update Database Connection
appsettings.json: Database=FreeLance → Database=CompanyWebsiteDb

### 4. Update Display Names
Views and layouts: "FreeLance" → "Company Website"

## Files That Will Be Updated
- All .cs files (namespace)
- appsettings.json (database name)
- Views (display names)
- Project files (.csproj, .sln)

