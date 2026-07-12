chcp 65001
REM @echo off
REM Указать путь до BusEngine | Specify the path to the BusEngine | http://dl.gsu.by/doc/use/ntcmds.htm
REM cd /d "%~dp0"
SET BusEngineFolder=%~dp0..\..\

REM Указать путь до MSBuild | Specify the path to the MSBuild | https://vk.com/@busengine-sopostavlyaem-versiu-s-csharp-s-kompilyatorom-i-platformoi-n
REM SET MSBuild="C:\Program Files\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
REM SET MSBuild="F:\Microsoft\Microsoft Visual Studio\2022\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
REM SET MSBuild="C:\Program Files (x86)\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
REM SET MSBuild="F:\Microsoft\Microsoft Visual Studio\2019\BuildTools\MSBuild\Current\Bin\MSBuild.exe"
REM SET MSBuild="C:\Program Files (x86)\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\MSBuild.exe"
REM SET MSBuild="F:\Microsoft\Microsoft Visual Studio\2017\BuildTools\MSBuild\15.0\Bin\MSBuild.exe"
REM SET MSBuild="C:\Program Files (x86)\MSBuild\14.0\Bin\amd64\MSBuild.exe"
REM SET MSBuild="C:\Program Files (x86)\MSBuild\12.0\Bin\MSBuild.exe"
SET MSBuild="C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe"
REM SET MSBuild="C:\Windows\Microsoft.NET\Framework64\v4.0.30319\MSBuild.exe"
REM SET MSBuild="C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe"
REM SET MSBuild="C:\Windows\Microsoft.NET\Framework64\v3.5\MSBuild.exe"

REM Указать путь до dotNET (если он нужен) | Specify the path to dotNET (if needed) | MSBuild v17.0+ in dotnet v6
SET dotNET="C:\Program Files\dotnet\dotnet.exe"

REM Cache .NET https://learn.microsoft.com/en-us/dotnet/framework/tools/ngen-exe-native-image-generator#PriorityTable
cd C:\Windows\assembly
SET cache="C:\Windows\Microsoft.NET\Framework\v4.0.30319\ngen.exe"
SET cache64="C:\Windows\Microsoft.NET\Framework64\v4.0.30319\ngen.exe"
REM C:\Windows\Microsoft.NET\Framework\v4.0.30319\ngen.exe install "%BusEngineFolder:"=%/Bin/Win/BusEngine.dll" /Profile /queue:1 /nologo

REM 0=AnyCPU 1=x64 2=x86 3=Android
SET Platform=0

REM 0=BusEngine 1=Launcher 2=Editor 3=Plugin 4=Server 5=Game 6=GameAndroid
SET Type=1

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
SET win64="%BusEngineFolder:"=%/Bin/Win_x64"
SET win86="%BusEngineFolder:"=%/Bin/Win_x86"
SET win="%BusEngineFolder:"=%/Bin/Win"
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



IF %typestatus% == false (
	IF %Platform% == 1 (
		SET ssl=%win64:"=%
	) ELSE IF %Platform% == 2 (
		SET ssl=%win86:"=%
	) ELSE (
		SET ssl=%win:"=%
	)
)



REM Установка сертификата

REM НИКОГДА НЕ ПЕРЕДАВАЙ (Храни в секрете):
REM Эти файлы — твоя цифровая личность. Их нужно держать в защищенном месте (желательно на отдельной флешке или в зашифрованном архиве).

REM .pfx — самый критичный файл. В нем упакованы и открытый сертификат, и твой закрытый ключ. Любой, кто узнает пароль к этому файлу, сможет выдавать себя за тебя.
REM .pvk — это «чистый» закрытый ключ (Private Key). Это сердце твоей подписи.
REM .pem (если в нем содержится ключ, начинающийся с -----BEGIN PRIVATE KEY-----) — это текстовый вариант твоего секретного ключа.
REM .key — еще одно расширение для закрытого ключа.
REM .cnf — содержит только информацию об издателе в качестве настроек, для пользователя - это мусор.

REM МОЖНО И НУЖНО ПЕРЕДАВАТЬ:
REM Эти файлы содержат только публичную информацию. Они нужны пользователю для того, чтобы Windows «узнала» твою подпись.

REM .cer — это открытый сертификат (Public Certificate). Его ты прикладываешь к установщику, чтобы через certutil добавить в доверенные. В нем нет секретных ключей.
REM .der — тот же .cer, просто в бинарном формате. Безопасен.
REM .spc — (Software Publishers Certificate) содержит только открытые ключи и цепочку сертификатов. Безопасен.

:: --- НАСТРОЙКИ ИМЕНИ ---
set "NAME=BuslikDrev"
set "URL=https://buslikdrev.by/"

:: Возвращаемся в папку проекта (раскомментируй, если пути ../../ не сработают)
cd /d "%~dp0"

:: Исправляем слэши в пути к папке (Windows любит \)
set "ssl=%ssl:/=\%"

:: --- ШАГ 1: ПРОВЕРКА СУЩЕСТВУЮЩЕГО СЕРТИФИКАТА ---
echo [BusEngine] Проверка наличия сертификата в системе...
certutil -verifystore Root "%NAME%" >nul 2>&1
if %errorLevel% == 0 (
    if exist "%ssl%\%NAME%.pfx" (
        echo [BusEngine] Сертификат уже создан и установлен. Пропускаем генерацию.
        goto :sign_exe
    )
)

:: --- ШАГ 2: ГЕНЕРАЦИЯ (если не найден) ---
echo [BusEngine] Сертификат не найден. Начинаю создание...

:: Создаем файл конфигурации openssl.cnf
(
echo [ req ]
echo default_bits        = 4096
echo distinguished_name  = req_distinguished_name
echo x509_extensions     = v3_ca
echo prompt              = no
echo.
echo [ req_distinguished_name ]
echo C                      = BE
echo ST                     = Hessen
echo L                      = Frankfurt am Main
echo O                      = BuslikDrev
echo CN                     = BuslikDrev
echo.
echo [ v3_ca ]
echo subjectKeyIdentifier   = hash
echo authorityKeyIdentifier = keyid:always,issuer
echo basicConstraints       = critical, CA:true
echo keyUsage               = critical, digitalSignature, cRLSign, keyCertSign
echo extendedKeyUsage       = codeSigning, 1.3.6.1.5.5.7.3.3
echo subjectAltName         = @alt_names
echo.
echo [ alt_names ]
echo DNS.1                  = buslikdrev.by
) > "%ssl%\openssl.cnf"

:: Определяем OpenSSL
set "BE_Path=%BusEngineFolder:"=%"
IF %Platform% == 1 (
	set "OPENSSL_BIN=%BE_Path%\Tools\OpenSSL\win-x64\native\openssl.exe"
) ELSE (
	set "OPENSSL_BIN=%BE_Path%\Tools\OpenSSL\win-x86\native\openssl.exe"
)

:: Генерация ключей
"%OPENSSL_BIN%" req -x509 -new -nodes -keyout "%ssl%\%NAME%.key" -out "%ssl%\%NAME%.cer" -config "%ssl%\openssl.cnf" -days 3650
"%OPENSSL_BIN%" pkcs12 -export -out "%ssl%\%NAME%.pfx" -inkey "%ssl%\%NAME%.key" -in "%ssl%\%NAME%.cer" -passout pass:BusEngine123

if exist "%ssl%\openssl.cnf" del "%ssl%\openssl.cnf"

:: --- ШАГ 3: УСТАНОВКА В СИСТЕМУ (нужны права админа) ---
net session >nul 2>&1
if %errorLevel% neq 0 (
    echo [BusEngine] Запрашиваю права администратора для регистрации сертификата...
    powershell -Command "Start-Process -FilePath '%~f0' -Verb RunAs"
    exit /b
)

:admin_tasks
echo [BusEngine] Установка сертификата в доверенные хранилища...
certutil -addstore -f "Root" "%ssl%\%NAME%.cer" >nul
certutil -addstore -f "TrustedPublisher" "%ssl%\%NAME%.cer" >nul

:: --- ШАГ 4: ПОДПИСЬ EXE (выполняется всегда) ---
:sign_exe
echo [BusEngine] Поиск signtool.exe для подписи...
set "SDK_BASE=C:\Program Files (x86)\Windows Kits\10\bin"
set "SDK_PATH="

for /f "delims=" %%i in ('dir "%SDK_BASE%\10.*" /b /ad /o-n') do (
    set "SDK_VER=%%i"
    goto :found_ver
)

:found_ver
if exist "%SDK_BASE%\%SDK_VER%\x64\signtool.exe" (
    set "SDK_PATH=%SDK_BASE%\%SDK_VER%\x64"
) else (
    set "SDK_PATH=%SDK_BASE%\%SDK_VER%\x86"
)

if not exist "%ssl%\%Name:"=%.exe" (
    echo [КРИТИЧЕСКАЯ ОШИБКА] EXE файл исчез сразу после сборки! 
    echo Возможно, его удалил антивирус. Добавьте папку в исключения.
    pause
    exit /b
)

if exist "%SDK_PATH%\signtool.exe" (
    echo [BusEngine] Подписываю файл: "%ssl%\%Name:"=%.exe"
    "%SDK_PATH%\signtool.exe" sign /f "%ssl%\%NAME%.pfx" /p BusEngine123 /d "BusEngine" /du "%URL%" /t http://timestamp.digicert.com /v "%ssl%\%Name:"=%.exe"
) else (
    echo [!] Signtool не найден. EXE не подписан.
)

echo [BusEngine] Готово!


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
		%cache64% install %win64% /queue:1
		"c:\windows\explorer.exe" %win64%
	) ELSE IF %Platform% == 2 (
		REM "%BusEngineFolder:"=%/Bin/Win_x86"
		%cache% install %win86% /queue:1
		"c:\windows\explorer.exe" %win86%
	) ELSE (
		REM "%BusEngineFolder:"=%/Bin/Win"
		%cache% install %win% /queue:1
		"c:\windows\explorer.exe" %win%
	)
) ELSE (
	IF %Platform% == 1 (
		REM %win64%/%Name:"=%.exe
	) ELSE IF %Platform% == 2 (
		REM %win86%/%Name:"=%.exe
	) ELSE IF %Platform% == 3 (
		"c:\windows\explorer.exe" %android%
	) ELSE (
		REM %win%/%Name:"=%.exe
		%cache% install %win% /queue:1
	)
)

PAUSE
REM EXIT