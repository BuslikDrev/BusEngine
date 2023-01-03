REM Пересоздаём csproj для удаления из кэша MSBuild | Rebuilding csproj to remove from MSBuild cache
COPY /b nul+%~dp0WindowsFormsApplication317.csproj %~dp0Dump.csproj
MOVE /Y %~dp0Dump.csproj %~dp0WindowsFormsApplication317.csproj

REM Сборка для BusEngine | Building for BusEngine
"C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe" %~dp0WindowsFormsApplication317.csproj

REM удалить "obj\*" | delete "obj\*"
rd "%~dp0obj" /s /q

REM Запуск BusEngine | Start BusEngine
%~dp0bin\Debug\WindowsFormsApplication317.exe

REM Log | Log
PAUSE
REM EXIT