# BusEngine [![BusEngine Version](https://img.shields.io/badge/Release-v0.3.0-black.svg?cacheSeconds=31536000)](https://github.com/BuslikDrev/BusEngine) [![YouTube](https://img.shields.io/youtube/views/2FowrV3cpZo?style=social)](https://www.youtube.com/watch?v=2FowrV3cpZo)

### BusEngine Editor и Launcher
![Platform](https://img.shields.io/badge/Platform-Win7+--x64%20|%20Win7+--x86-purple.svg?cacheSeconds=31536000)

### BusEngine Game
![Platform](https://img.shields.io/badge/Platform-Windows%207+%20|%20Android%205+-purple.svg?cacheSeconds=31536000)

### Минимальные требования для успешной сборки
[![Minimum MSBuild Tools Version](https://img.shields.io/badge/MSBuild%20Tools-%20%3E%3D%20v12.0-orange.svg?cacheSeconds=31536000)](https://www.microsoft.com/ru-ru/download/details.aspx?id=40760)
[![Minimum C# Version](https://img.shields.io/badge/C%23%20%28CSharp%29-%20%3E%3D%20v5.0-blueviolet.svg?cacheSeconds=31536000)](https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history#c-version-50)
[![Minimum NET.Framework Version](https://img.shields.io/badge/NET.Framework-%20%3E%3D%20v4.5.2-blue.svg?cacheSeconds=31536000)](https://dotnet.microsoft.com/en-us/download/dotnet-framework)

### Рекомендуемые требования для успешной сборки
[![Minimum MSBuild Tools Version](https://img.shields.io/badge/MSBuild%20Tools-%20%3E%3D%20v14.0-orange.svg?cacheSeconds=31536000)](https://www.microsoft.com/ru-ru/download/details.aspx?id=48159)
[![Minimum C# Version](https://img.shields.io/badge/C%23%20%28CSharp%29-%20%3E%3D%20v6.0-blueviolet.svg?cacheSeconds=31536000)](https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history#c-version-60)
[![Minimum NET.Framework Version](https://img.shields.io/badge/NET.Framework-%20%3E%3D%20v4.6.1-blue.svg?cacheSeconds=31536000)](https://dotnet.microsoft.com/en-us/download/dotnet-framework)

## Описание

Проект разрабатывается в целях развлечения и обучения. Возможно кто-то что-то для себя подчеркнёт.

Для кроссплатформенности (для сборки игры под Android) планируется использовать MSBuild 2017 и Xamarin с Android SDK API 25. Лаунчер движка будет объяснять, что необходимо скачать.

## Инструкция по сборке

- установить [MicroSoft Build Tools 2013 v12.0](https://www.microsoft.com/ru-ru/download/details.aspx?id=40760) (возможно новее версия тоже подойдёт);
- скачать библиотеки и положить в папку "Code/BusEngine/BusContent/Win_*/":

  https://www.nuget.org/packages/cef.redist.x64/107.1.9 (из "CEF/" в "Code/BusEngine/BusContent/Win_x64/CefSharp/")

  https://www.nuget.org/packages/cef.redist.x86/107.1.9 (из "CEF/" в "Code/BusEngine/BusContent/Win_x86/CefSharp/")

  https://www.nuget.org/packages/CefSharp.Common/107.1.90 (из "CefSharp/x*/" в "Code/BusEngine/BusContent/Win_x*/CefSharp/")

  https://www.nuget.org/packages/CefSharp.Common/107.1.90 (из "lib/net452/" в "Code/BusEngine/BusContent/Win_x*/CefSharp/" и в "Code/BusEngine/BusPlugins/CefSharp/")

  https://www.nuget.org/packages/CefSharp.WinForms/107.1.90 (из "lib/net452/" в "Code/BusEngine/BusContent/Win_x*/CefSharp/" и в "Code/BusEngine/BusPlugins/CefSharp/")

  https://www.nuget.org/packages/CefSharp.Wpf/107.1.90 (из "lib/net452/" в "Code/BusEngine/BusContent/Win_x*/CefSharp/" и в "Code/BusEngine/BusPlugins/CefSharp/")

  https://www.nuget.org/packages/VideoLAN.LibVLC.Windows/3.0.17.4 (из "build/x*/" в "Code/BusEngine/BusContent/Win_x*/LibVLC/", потом папку "plugins" и файлы "libvlccore.dll,libvlccore.lib" из "Code/BusEngine/BusContent/Win_x*/LibVLC/" в "Code/BusEngine/BusContent/Win_x*/")

  https://www.nuget.org/packages/LibVLCSharp/3.6.7 (из "lib/net40/" в "Code/BusEngine/BusContent/Win_x*/LibVLC/" и в "Code/BusEngine/BusPlugins/LibVLC/")

  https://www.nuget.org/packages/LibVLCSharp.WinForms/3.6.7 (из "lib/net40/" в "Code/BusEngine/BusContent/Win_x*/LibVLC/" и в "Code/BusEngine/BusPlugins/LibVLC/")

  https://www.nuget.org/packages/LibVLCSharp.WPF/3.6.7 (из "lib/net461/" в "Code/BusEngine/BusContent/Win_x*/LibVLC/" и в "Code/BusEngine/BusPlugins/LibVLC/")
  
  https://www.nuget.org/packages/Newtonsoft.Json/13.0.2 (из "lib/net40/" в "Code/BusEngine/BusContent/Win_x*/Newtonsoft.Json/" и в "Code/BusEngine/BusPlugins/Newtonsoft.Json/")
- в файлe "Code/BusEngine/Build.cmd" и "Code/Game/Build.cmd" заменить все пути на свои и указать цифру переменной "Platform" в зависимости от нужной разрядности системы;
- запустить "Code/BusEngine/Build.cmd" потом "Code/Game/Build.cmd";
- потом можно копировать папки Bin и Data в любую папку разработки игры и изменять всё, что в папке Data.

## Лицензии

BusEngine выпускается под лицензией [MIT](https://github.com/BuslikDrev/BusEngine/blob/main/LICENSE) до версии 1.0.0, с версии 1.0.0 будет видно. Коротко говоря, если будете использовать, то указывайте автора и источник (ссылку на сайт автора или ютуб канал, или данный репозитрий).
Лицензия распространяется на все файлы, создаваемые мной.

#### Список ссылок на файлы других авторов на которые, не распространяется данная лицензия, так как у них имеется своя лицензия
[MicroSoft dotNET](https://github.com/dotnet)

[CefSharp](https://github.com/cefsharp/CefSharp/tree/v107.1.90)

[Chromium](https://github.com/chromium/chromium)

[VideoLAN | libvlcsharp](https://github.com/videolan)

[Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json)

## Полезные ссылки

Microsoft Build Tools 2005 v2.0

[C:\Windows\Microsoft.NET\Framework\v2.0.50727\MSBuild.exe](file:///C:/Windows/Microsoft.NET/Framework/v2.0.50727)

Microsoft Build Tools 2008 v3.5

[C:\Windows\Microsoft.NET\Framework\v3.5\MSBuild.exe](file:///C:/Windows/Microsoft.NET/Framework/v3.5)

Microsoft Build Tools 2012 v4.8

[C:\Windows\Microsoft.NET\Framework\v4.0.30319\MSBuild.exe](file:///C:/Windows/Microsoft.NET/Framework/v4.0.30319)

Microsoft Build Tools 2013 v12.0

https://www.microsoft.com/ru-ru/download/details.aspx?id=40760

Microsoft Build Tools 2015 v14.0

https://www.microsoft.com/ru-ru/download/details.aspx?id=48159

Microsoft Build Tools 2017 v15.0

https://learn.microsoft.com/ru-ru/visualstudio/releasenotes/vs2017-relnotes-history#installing-the-earlier-release

Microsoft Build Tools 2019 v16.0 (Входит в NET.Core 3.1+)

https://learn.microsoft.com/ru-ru/visualstudio/releases/2019/history#release-dates-and-build-numbers

Microsoft Build Tools 2022 v17.0 (Входит в NET.Core 6.0+)

https://learn.microsoft.com/ru-ru/visualstudio/releases/2022/release-history#evergreen-bootstrappers

*.NET Framework 4.8 Developer Pack и языки для перевода дебагера*

https://dotnet.microsoft.com/en-us/download/dotnet-framework/
