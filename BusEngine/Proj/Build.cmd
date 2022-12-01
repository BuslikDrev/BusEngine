SET Plagin="H:\CRYENGINE Projects\BusEngine\Proj\Game.csproj"
SET MSBuild="C:\Program Files (x86)\BusEngine\14.0\Bin\amd64\MSBuild.exe" %Plagin%
SET params_cry=""

REM Пересоздаём csproj для удаления из кэша MSBuild | Rebuilding csproj to remove from MSBuild cache
COPY NUL "H:\dump.csproj"
COPY /b "H:\dump.csproj"+%Plagin% "H:\dump.csproj"
DEL %Plagin%
COPY "H:\dump.csproj" %Plagin%
DEL "H:\dump.csproj"

REM Сборка Game.dll для CryEngine | Building Game.dll for CryEngine
%MSBuild% %params_cry%
REM "H:\CRYENGINE Projects\BusEngine\Bin\My Game.exe"

PAUSE