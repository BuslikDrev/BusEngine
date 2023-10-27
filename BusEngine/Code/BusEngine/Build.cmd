REM Указать путь до BusEngine | Specify the path to the BusEngine | http://dl.gsu.by/doc/use/ntcmds.htm
REM SET BusEngineFolder="H:\BusEngine"
SET BusEngineFolder=%~dp0..\..\

REM Указать путь до MSBuild | Specify the path to the MSBuild | https://en.wikipedia.org/wiki/MSBuild#Versions
REM SET MSBuild="C:\Program Files\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
REM SET MSBuild="C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
REM SET MSBuild="F:\Microsoft\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
REM SET MSBuild="C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\MSBuild.exe"
REM SET MSBuild="F:\Microsoft\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\MSBuild.exe"
REM SET MSBuild="C:\Program Files (x86)\MSBuild\14.0\Bin\amd64\MSBuild.exe"
SET MSBuild="C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe"
REM SET MSBuild="C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
REM SET MSBuild="C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"
REM SET MSBuild="C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe"
REM SET MSBuild="C:\Windows\Microsoft.NET\Framework64\v3.5\MSBuild.exe"

REM Указать путь до dotNET (если он нужен) | Specify the path to dotNET (if needed) | MSBuild v17.0+ in dotnet v6
SET dotNET="C:\Program Files\dotnet\dotnet.exe"

REM Cache .NET https://learn.microsoft.com/en-us/dotnet/framework/tools/ngen-exe-native-image-generator#PriorityTable
cd C:\Windows\assembly
SET cache="C:\Windows\Microsoft.NET\Framework\v4.0.30319\ngen.exe"
SET cache64="C:\Windows\Microsoft.NET\Framework64\v4.0.30319\ngen.exe"
REM C:\Windows\Microsoft.NET\Framework\v4.0.30319\ngen.exe install "H:\BusEngine Launcher\Bin\Win\BusEngine.dll" /Profile /queue:1 /nologo

REM 0=AnyCPU 1=x64 2=x86 3=Android
SET Platform=0

REM 0=BusEngine 1=Launcher 2=Editor 3=Plugin 4=Server 5=Game 6=GameAndroid
SET Type=0

REM 0=MSBuild 1=MSBuild 2=dotNET build 3=dotNET run
SET TypeBuild=1

SET NameBusEngine="BusEngine"
SET NameLauncher="Launcher"
SET NameEditor="Editor"
SET NamePlugin="Plugin"
SET NameServer="Server"
SET NameGame="Game"
SET NameGameAndroid="GameAndroid"
SET AndroidSDK="F:\Android\android-sdk"

SET Params=""



















REM Выбираем наш проект | Choose our project
IF %Type% == 1 (
	SET Name=%NameLauncher%
) ELSE IF %Type% == 2 (
	SET Name=%NameEditor%
) ELSE IF %Type% == 3 (
	SET Name=%NamePlugin%
	SET Params=%Params% -p:OutputType=Library
) ELSE IF %Type% == 4 (
	SET Name=%NameServer%
) ELSE IF %Type% == 5 (
	SET Name=%NameGame%
) ELSE IF %Type% == 6 (
	SET Name=%NameGameAndroid%
) ELSE (
	SET Name=%NameBusEngine%
	SET Params=%Params% -p:OutputType=Library
)
SET Params=%Params% -p:AssemblyName=%Name%

REM Убираем кавычки и формируем пути | Remove quotes and form paths
IF %Type% == 0 (
	SET typestatus=true
) ELSE IF %Type% == 3 (
	SET typestatus=true
) ELSE (
	SET typestatus=false
)
IF %typestatus% == true (
SET win64="%BusEngineFolder:"=%/Bin/Win_x64"
SET win86="%BusEngineFolder:"=%/Bin/Win_x86"
SET win="%BusEngineFolder:"=%/Bin/Win"
) ELSE (
SET win64="%BusEngineFolder:"=%/Bin/Win_x64/%Name:"=%.exe"
SET win86="%BusEngineFolder:"=%/Bin/Win_x86/%Name:"=%.exe"
SET win="%BusEngineFolder:"=%/Bin/Win/%Name:"=%.exe"
)
SET android="%BusEngineFolder:"=%/Bin/Android"
SET xbox="%BusEngineFolder:"=%/Bin/Xbox/%Name:"=%.exe"
SET Dump="%~dp0Dump.csproj"
SET CSProj="%~dp0%Name:"=%.csproj"

REM Убираем повторы слэшей | Removing repeated slashes
SET win64=%win64:/=\%
SET win64=%win64:\\=\%
SET win86=%win86:/=\%
SET win86=%win86:\\=\%
SET win=%win:/=\%
SET win=%win:\\=\%
SET android=%android:/=\%
SET android=%android:\\=\%
SET xbox=%xbox:/=\%
SET xbox=%xbox:\\=\%
SET Dump=%Dump:/=\%
SET Dump=%Dump:\\=\%
SET CSProj=%CSProj:/=\%
SET CSProj=%CSProj:\\=\%
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
	SET Params=%Params% -p:Platform=Android
	SET Params=%Params% -p:AndroidSdkDirectory=%AndroidSDK%
) ELSE IF %Platform% == 4 (
	SET Params=%Params% -p:Platform=linux
) ELSE IF %Platform% == 5 (
	SET Params=%Params% -p:Platform=Xbox
) ELSE IF %Platform% == 6 (
	SET Params=%Params% -p:Platform=XboxOne
) ELSE IF %Platform% == 7 (
	SET Params=%Params% -p:Platform=ps4
) ELSE IF %Platform% == 8 (
	SET Params=%Params% -p:Platform=macOS
) ELSE IF %Platform% == 9 (
	SET Params=%Params% -p:Platform=iOS
) ELSE (
	SET Params=%Params% -p:Platform=AnyCPU
)

REM Пересоздаём csproj для удаления из кэша MSBuild | Rebuilding csproj to remove from MSBuild cache
COPY /b nul+%CSProj% %Dump%
MOVE /Y %Dump% %CSProj%

REM Сборка для BusEngine | Building for BusEngine
IF %TypeBuild% == 3 (
	REM https://learn.microsoft.com/en-us/dotnet/core/tools/dotnet-build
	%dotNET% run --project %CSProj% -c Release -f net471 %Params%
) ELSE IF %TypeBuild% == 2 (
	%dotNET% build %CSProj% -f net5.0-windows %Params%
) ELSE IF %TypeBuild% == 1 (
	%MSBuild% %CSProj% -p:TargetFramework=net471 %Params%
) ELSE (
	%MSBuild% %CSProj% -p:TargetFramework=net471 %Params%
)
REM удалить "obj\*" | delete "obj\*"
rd "%~dp0obj" /s /q

REM Запуск BusEngine | Start BusEngine
IF %Type% == 0 (
	SET typestatus=true
) ELSE IF %Type% == 3 (
	SET typestatus=true
) ELSE (
	SET typestatus=false
)
IF %typestatus% == true (
	IF %Platform% == 1 (
		REM cd "%BusEngineFolder:"=%/Bin/Win_x64"
		REM %cache64% install %win64% /queue:1
		"c:\windows\explorer.exe" %win64%
	) ELSE IF %Platform% == 2 (
		REM "%BusEngineFolder:"=%/Bin/Win_x86"
		REM %cache% install %win86% /queue:1
		"c:\windows\explorer.exe" %win86%
	) ELSE (
		REM "%BusEngineFolder:"=%/Bin/Win"
		REM %cache% install %win% /queue:1
		"c:\windows\explorer.exe" %win%
	)
) ELSE (
	IF %Platform% == 1 (
		%win64%
	) ELSE IF %Platform% == 2 (
		%win86%
	) ELSE IF %Platform% == 3 (
		"c:\windows\explorer.exe" %android%
	) ELSE (
		REM %win%
	)
)

REM Log | Log
PAUSE
REM EXIT