SET Dump="H:\CRYENGINE Projects\BusEngine\Code\Dump.csproj"
SET Plagin="H:\CRYENGINE Projects\BusEngine\Code\Game.csproj"
SET MSBuild="C:\Program Files (x86)\MSBuild\14.0\Bin\amd64\MSBuild.exe" %Plagin%
SET Params=""
SET Platform=1

REM Пересоздаём csproj для удаления из кэша MSBuild | Rebuilding csproj to remove from MSBuild cache
COPY NUL %Dump%
COPY /b %Dump%+%Plagin% %Dump%
DEL %Plagin%
COPY %Dump% %Plagin%
DEL %Dump%

REM Параметры для сборки | Options for assemblie
IF %Platform% == 1 (
SET Params=%Params% -p:Platform=x64
) ELSE IF %Platform% == 2 (
SET Params=%Params% -p:Platform=x86
) ELSE (
SET Params=%Params% -p:Platform=AnyCPU
)

REM Сборка Game.dll для CryEngine | Building Game.dll for CryEngine
%MSBuild% %Params%
IF %Platform% == 1 (
REM MOVE /Y "H:\CRYENGINE Projects\BusEngine\Build\Game.exe" "H:\CRYENGINE Projects\BusEngine\Bin\Win_x64\Game.exe"
"H:\CRYENGINE Projects\BusEngine\Bin\Win_x64\Game.exe"
) ELSE IF %Platform% == 2 (
REM MOVE /Y "H:\CRYENGINE Projects\BusEngine\Build\Game.exe" "H:\CRYENGINE Projects\BusEngine\Bin\Win_x86\Game.exe"
"H:\CRYENGINE Projects\BusEngine\Bin\Win_x86\Game.exe"
) ELSE (
REM MOVE /Y "H:\CRYENGINE Projects\BusEngine\Build\Game.exe" "H:\CRYENGINE Projects\BusEngine\Bin\Win_x86\Game.exe"
"H:\CRYENGINE Projects\BusEngine\Bin\Win_x86\Game.exe"
)

PAUSE