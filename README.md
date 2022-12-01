# BusEngine
[![BusEngine Version](https://img.shields.io/badge/Release-v0.1.0-black.svg?cacheSeconds=31536000)](https://github.com/BuslikDrev/BusEngine)
![Platform](https://img.shields.io/badge/Platform-Win--x64-purple.svg?cacheSeconds=31536000)
[![Minimum MSBuild Tools Version](https://img.shields.io/badge/MSBuild%20Tools-%20%3E%3D%20v14.0-orange.svg?cacheSeconds=31536000)](https://github.com/BuslikDrev/OpenCart.CMS-2.3.0.2.6)
[![Minimum C# Version](https://img.shields.io/badge/C%23%20%28CSharp%29-%20%3E%3D%20v6.0-blueviolet.svg?cacheSeconds=31536000)](https://github.com/BuslikDrev/OpenCart.CMS-2.3.0.2.6)
[![Minimum NET.Framework Version](https://img.shields.io/badge/NET.Framework-%20%3E%3D%20v4.5.2-blue.svg?cacheSeconds=31536000)](https://dotnet.microsoft.com/en-us/download/dotnet-framework)
[![YouTube](https://img.shields.io/youtube/views/R1MwBJZzpsk?style=social)](https://www.youtube.com/watch?v=R1MwBJZzpsk)

## Описание

Проект разрабатывается в целях развлечения и обучения. Возможно кто-то что-то для себя подчеркнёт.

## Инструкция по сборке

- установить [MicroSoft Build Tools 2015 v14.0](https://www.microsoft.com/ru-ru/download/confirmation.aspx?id=48159) (возможно новее версия тоже подойдёт);
- установить [NET.Framework 4.8 Developer Pack и языки для перевода дебагера](https://dotnet.microsoft.com/en-us/download/dotnet-framework/thank-you/net48-developer-pack-offline-installer);
- в файлах *.cmd, *.csproj заменить все пути на свои;
- запустить Build.cmd;
- при успешности компиляции, переместить файлы из папки Build в папку Bin;
- скачать и положить в папку Bin библиотеки (Bin/*.dll):
https://www.nuget.org/packages/CefSharp.Common/107.1.90
https://www.nuget.org/packages/CefSharp.WinForms/107.1.90
https://www.nuget.org/packages/cef.redist.x64/107.1.9

## Лицензии

BusEngine выпускается под лицензией [MIT](https://github.com/BuslikDrev/BusEngine/blob/main/LICENSE). Коротко говоря, если будете использовать, то указывайте автора и источник (ссылку на сайт автора или ютуб канал, или данный репозитрий).
Лицензия распространяется на все файлы, создаваемые мной.

#### Список ссылок на файлы других авторов на которые, не распространяется данная лицензия, так как у них имеется своя лицензия
[MicroSoft dotNET](https://github.com/dotnet)

[CefSharp](https://github.com/cefsharp/CefSharp/tree/v107.1.90)

[Chromium](https://github.com/chromium/chromium)
