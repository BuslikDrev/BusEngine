REM Указать путь до BusEngine | Specify the path to the BusEngine
SET BusEngineFolder="H:\CRYENGINE Projects\BusEngine"

REM Указать путь до MSBuild | Specify the path to the MSBuild
SET MSBuild="C:\Program Files (x86)\MSBuild\14.0\Bin\amd64\MSBuild.exe"
REM SET MSBuild="C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
REM SET MSBuild="C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"

REM Указать путь до dotNET (если он нужен) | Specify the path to dotNET (if needed)
SET dotNET="C:\Program Files\dotnet\dotnet.exe"

REM 0=AnyCPU 1=x64 2=x86
SET Platform=1

REM 0=Game 1=Game 2=Editor 3=Server
SET Type=1

REM 0=MSBuild 1=MSBuild 2=dotNET
SET TypeBuild=1

SET NameGame="Game"
SET NameEditor="Editor"
SET NameServer="Server"
SET Params=""



















REM Выбираем наш проект | Choose our project
IF %Type% == 1 (
	SET Name=%NameGame%
) ELSE IF %Type% == 2 (
	SET Name=%NameEditor%
) ELSE IF %Type% == 3 (
	SET Name=%NameServer%
) ELSE (
	SET Name=%NameGame%
)

REM Убираем кавычки и формируем пути | Remove quotes and form paths
SET win64="%BusEngineFolder:"=%/Bin/Win_x64/%Name:"=%.exe"
SET win86="%BusEngineFolder:"=%/Bin/Win_x86/%Name:"=%.exe"
SET win="%BusEngineFolder:"=%/Bin/Win/%Name:"=%.exe"
SET xbox="%BusEngineFolder:"=%/Bin/xbox/%Name:"=%.exe"
SET Dump="%BusEngineFolder:"=%/Code/Dump.csproj"
SET Plagin="%BusEngineFolder:"=%/Code/%Name:"=%.csproj"

REM Убираем повторы слэшей | Removing repeated slashes
SET win64=%win64:/=\%
SET win64=%win64:\\=\%
SET win86=%win86:/=\%
SET win86=%win86:\\=\%
SET win=%win:/=\%
SET win=%win:\\=\%
SET xbox=%xbox:/=\%
SET xbox=%xbox:\\=\%
SET Dump=%Dump:/=\%
SET Dump=%Dump:\\=\%
SET Plagin=%Plagin:/=\%
SET Plagin=%Plagin:\\=\%
SET MSBuild=%MSBuild:/=\%
SET MSBuild=%MSBuild:\\=\%
SET dotNET=%dotNET:/=\%
SET dotNET=%dotNET:\\=\%

REM Параметры для сборки | Options for assemblie
IF %Platform% == 1 (
	SET Params=%Params% -p:Platform=x64
) ELSE IF %Platform% == 2 (
	SET Params=%Params% -p:Platform=x86
) ELSE IF %Platform% == 3 (
	SET Params=%Params% -p:Platform=Xbox
) ELSE IF %Platform% == 4 (
	SET Params=%Params% -p:Platform=XboxOne
) ELSE IF %Platform% == 5 (
	SET Params=%Params% -p:Platform=linux
) ELSE IF %Platform% == 6 (
	SET Params=%Params% -p:Platform=ps4
) ELSE IF %Platform% == 7 (
	SET Params=%Params% -p:Platform=Android
) ELSE IF %Platform% == 8 (
	SET Params=%Params% -p:Platform=macos
) ELSE IF %Platform% == 9 (
	SET Params=%Params% -p:Platform=ios
) ELSE (
	SET Params=%Params% -p:Platform=AnyCPU
)

REM Пересоздаём csproj для удаления из кэша MSBuild | Rebuilding csproj to remove from MSBuild cache
COPY /b nul+%Plagin% %Dump%
MOVE /Y %Dump% %Plagin%

REM Сборка для BusEngine | Building for BusEngine
IF %TypeBuild% == 2 (
	%dotNET% build %Plagin% %Params%
) ELSE (
	%MSBuild% %Plagin% %Params%
)

REM Запуск BusEngine | Start BusEngine
IF %Platform% == 1 (
	%win64%
) ELSE IF %Platform% == 2 (
	%win86%
) ELSE (
	%win%
)

REM Log | Log
PAUSE