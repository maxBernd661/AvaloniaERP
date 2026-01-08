@echo off
setlocal enabledelayedexpansion

set "DBPROJECT=.\AvaloniaERP.Core\AvaloniaERP.Core.csproj"
set "STARTUPPROJECT=.\AvaloniaERP.Win\AvaloniaERP.Win.csproj"
set "DBCONTEXT=EntityContext"
set "MIGRATIONS_DIR=Migrations"

REM --- Ask for migration name
set "MIGRATION_NAME="
set /p MIGRATION_NAME=Migration name (e.g. AddOrdersTable) ^> 

if "%MIGRATION_NAME%"=="" (
  echo No migration name entered. Aborting.
)

echo.
echo == Adding migration: %MIGRATION_NAME%
echo.

dotnet ef migrations add "%MIGRATION_NAME%" ^
  --project "%DBPROJECT%" ^
  --startup-project "%STARTUPPROJECT%" ^
  --context "%DBCONTEXT%" ^
  --output-dir "%MIGRATIONS_DIR%"

if errorlevel 1 (
  echo.
  echo Migration add failed.
)

echo.
set "DO_UPDATE="
set /p DO_UPDATE=Apply to database now? (y/N) ^> 

if /I "%DO_UPDATE%"=="y" (
  echo.
  echo == Updating database
  echo.
  dotnet ef database update ^
    --project "%DBPROJECT%" ^
    --startup-project "%STARTUPPROJECT%" ^
    --context "%DBCONTEXT%"

  if errorlevel 1 (
    echo.
    echo Database update failed.
  )
)

echo.
echo Done.