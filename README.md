# BusEngine
[![BusEngine Version](https://img.shields.io/badge/Release-v0.2.0-black.svg?cacheSeconds=31536000)](https://github.com/BuslikDrev/BusEngine)
![Platform](https://img.shields.io/badge/Platform-Win--x64%20|%20Win--x86-purple.svg?cacheSeconds=31536000)
[![Minimum MSBuild Tools Version](https://img.shields.io/badge/MSBuild%20Tools-%20%3E%3D%20v12.0-orange.svg?cacheSeconds=31536000)](https://github.com/BuslikDrev/OpenCart.CMS-2.3.0.2.6)
[![Minimum C# Version](https://img.shields.io/badge/C%23%20%28CSharp%29-%20%3E%3D%20v5.0-blueviolet.svg?cacheSeconds=31536000)](https://github.com/BuslikDrev/OpenCart.CMS-2.3.0.2.6)
[![Minimum NET.Framework Version](https://img.shields.io/badge/NET.Framework-%20%3E%3D%20v4.5.2-blue.svg?cacheSeconds=31536000)](https://dotnet.microsoft.com/en-us/download/dotnet-framework)
[![YouTube](https://img.shields.io/youtube/views/R1MwBJZzpsk?style=social)](https://www.youtube.com/watch?v=R1MwBJZzpsk)

## Описание

Проект разрабатывается в целях развлечения и обучения. Возможно кто-то что-то для себя подчеркнёт.

## Инструкция по сборке

- установить [MicroSoft Build Tools 2013 v12.0](https://www.microsoft.com/ru-ru/download/details.aspx?id=40760) (возможно новее версия тоже подойдёт);
- скачать библиотеки и положить в папку "Code/BusContent/Win_*/":

  https://www.nuget.org/packages/cef.redist.x64/107.1.9 (из "CEF/" в "Code/Game/BusContent/Win_x64/CefSharp/")

  https://www.nuget.org/packages/cef.redist.x86/107.1.9 (из "CEF/" в "Code/Game/BusContent/Win_x86/CefSharp/")

  https://www.nuget.org/packages/CefSharp.Common/107.1.90 (из "CefSharp/x*/" в "Code/Game/BusContent/Win_x*/CefSharp/")

  https://www.nuget.org/packages/CefSharp.Common/107.1.90 (из "lib/net452/" в "Code/Game/BusContent/Win_x*/CefSharp/" и в "Code/Game/BusPlugins/CefSharp/")

  https://www.nuget.org/packages/CefSharp.WinForms/107.1.90 (из "lib/net452/" в "Code/Game/BusContent/Win_x*/CefSharp/" и в "Code/Game/BusPlugins/CefSharp/")

  https://www.nuget.org/packages/CefSharp.Wpf/107.1.90 (из "lib/net452/" в "Code/Game/BusContent/Win_x*/CefSharp/" и в "Code/Game/BusPlugins/CefSharp/")

  https://www.nuget.org/packages/VideoLAN.LibVLC.Windows/3.0.17.4 (из "build/x*/" в "Code/Game/BusContent/Win_x*/LibVLC/", потом папку "plugins" и файлы "libvlccore.dll,libvlccore.lib" из "Code/Game/BusContent/Win_x*/LibVLC/" в "Code/Game/BusContent/Win_x*/")

  https://www.nuget.org/packages/LibVLCSharp/3.6.7 (из "lib/net40/" в "Code/Game/BusContent/Win_x*/LibVLC/" и в "Code/Game/BusPlugins/LibVLC/")

  https://www.nuget.org/packages/LibVLCSharp.WinForms/3.6.7 (из "lib/net40/" в "Code/Game/BusContent/Win_x*/LibVLC/" и в "Code/Game/BusPlugins/LibVLC/")

  https://www.nuget.org/packages/LibVLCSharp.WPF/3.6.7 (из "lib/net461/" в "Code/Game/BusContent/Win_x*/LibVLC/" и в "Code/Game/BusPlugins/LibVLC/")
- в файлe "Code/Build.cmd" заменить все пути на свои и указать цифру переменной "Platform" в зависимости от нужной разрядности системы;
- запустить Build.cmd;

## Лицензии

BusEngine выпускается под лицензией [MIT](https://github.com/BuslikDrev/BusEngine/blob/main/LICENSE). Коротко говоря, если будете использовать, то указывайте автора и источник (ссылку на сайт автора или ютуб канал, или данный репозитрий).
Лицензия распространяется на все файлы, создаваемые мной.

#### Список ссылок на файлы других авторов на которые, не распространяется данная лицензия, так как у них имеется своя лицензия
[MicroSoft dotNET](https://github.com/dotnet)

[CefSharp](https://github.com/cefsharp/CefSharp/tree/v107.1.90)

[Chromium](https://github.com/chromium/chromium)

[VideoLAN | libvlcsharp](https://github.com/videolan)
