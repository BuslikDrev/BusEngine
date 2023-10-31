# BusEngine [![BusEngine Version](https://img.shields.io/badge/Release-v0.4.0-black.svg?cacheSeconds=31536000)](https://github.com/BuslikDrev/BusEngine) [![YouTube](https://img.shields.io/youtube/views/2he4vAn6ZkQ?style=social)](https://www.youtube.com/watch?v=2he4vAn6ZkQ)

### BusEngine Editor и Launcher
![Platform](https://img.shields.io/badge/Platform-Win7+--x64%20|%20Win7+--x86-purple.svg?cacheSeconds=31536000)

### BusEngine Game
![Platform](https://img.shields.io/badge/Platform-Windows%207+%20|%20Android%205+-purple.svg?cacheSeconds=31536000)

### Минимальные требования для успешной работы
[![Minimum MSBuild Tools Version](https://img.shields.io/badge/MSBuild%20Tools-%20%3E%3D%20v12.0-orange.svg?cacheSeconds=31536000)](https://www.microsoft.com/ru-ru/download/details.aspx?id=40760)
[![Maximum C# Version](https://img.shields.io/badge/C%23%20%28CSharp%29-%20%3C%3D%20v5.0-blueviolet.svg?cacheSeconds=31536000)](https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history#c-version-50)
[![Minimum .NET Framework Version](https://img.shields.io/badge/.NET%20Framework-%20%3E%3D%20v4.7.1-blue.svg?cacheSeconds=31536000)](https://dotnet.microsoft.com/en-us/download/dotnet-framework)
[![Minimum Visual C++ Version](https://img.shields.io/badge/Visual%20C++-%20%3E%3D%202015-purple.svg?cacheSeconds=31536000)](https://learn.microsoft.com/ru-ru/cpp/windows/latest-supported-vc-redist?view=msvc-170)

### Рекомендуемые требования для успешной работы
[![Minimum MSBuild Tools Version](https://img.shields.io/badge/MSBuild%20Tools-%20%3E%3D%20v15.8-orange.svg?cacheSeconds=31536000)](https://learn.microsoft.com/ru-ru/visualstudio/releasenotes/vs2017-relnotes-history#installing-the-earlier-release)
[![Maximum C# Version](https://img.shields.io/badge/C%23%20%28CSharp%29-%20%3C%3D%20v7.3-blueviolet.svg?cacheSeconds=31536000)](https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history#c-version-73)
[![Minimum .NET Framework Version](https://img.shields.io/badge/.NET%20Framework-%20%3E%3D%20v4.8.0-blue.svg?cacheSeconds=31536000)](https://dotnet.microsoft.com/en-us/download/dotnet-framework)
[![Minimum Visual C++ Version](https://img.shields.io/badge/Visual%20C++-%20%3E%3D%202015-purple.svg?cacheSeconds=31536000)](https://learn.microsoft.com/ru-ru/cpp/windows/latest-supported-vc-redist?view=msvc-170)

## Описание

Проект разрабатывается в целях развлечения и обучения (аж 10 раз, естественно в целях заработка). Возможно кто-то что-то для себя подчеркнёт.

Для кроссплатформенности: для сборки игры под Android планируется использовать MSBuild 2017 и Xamarin с Android SDK API 21, а для WebGL OpenSilver или аналоги. Лаунчер движка будет объяснять, что необходимо скачать и(или) установить.

![Иллюстрация к проекту](https://github.com/BuslikDrev/BusEngine/blob/main/.github/image_1.jpg)

## Инструкция по установке

- скачать установщик лаунчера из официального сайта: https://busengine.buslikdrev.by/download.html  и установить его;
- зарегистрировать аккаунт;
- следовать инструкциям лаунчера по скачиванию движка и других необходимых программ к нему;
- создать новый проект, далее можно изменять всё, что в папке нового проекта;
- собирать и компилировать проект из меню лаунчера или компилировать через BAT файл из папки нахождения скрипта.

## Лицензии

BusEngine выпускается под лицензией [MIT](https://github.com/BuslikDrev/BusEngine/blob/main/LICENSE) до версии 0.20.0, с версии 0.20.0 будет видно. Коротко говоря, если будете использовать, то указывайте автора и источник (ссылку на [сайт автора](https://buslikdrev.by/) или данный репозиторий).
Лицензия распространяется на все файлы, создаваемые мной.

#### Список ссылок на файлы других авторов на которые, не распространяется данная лицензия, так как у них имеется своя
[MicroSoft dotNET](https://github.com/dotnet) (для работы BusEngine)

[CefSharp](https://github.com/cefsharp/CefSharp) 109.1.110 (для использования в BusEngine.Browser)

[Chromium](https://github.com/chromium/chromium) 109.1.11 (109.0.5414.87) (для использования в BusEngine.Browser)

[VideoLAN | libvlcsharp](https://github.com/videolan) 3.0.18 | 3.8.1 (для использования в BusEngine.Audio, BusEngine.Video)

[Newtonsoft.Json](https://github.com/JamesNK/Newtonsoft.Json) 13.0.3 (для использования в BusEngine.Tools.Json)

[Drawflow](https://github.com/jerosoler/Drawflow) 0.0.59 (для использования в BusEngine.FlowGraph)

[three.js](https://github.com/mrdoob/three.js) r158 (для использования в тестовом проекте BusEngine)

[DeepSpeech](https://github.com/mozilla/DeepSpeech) 0.9.3 (планируется - для возможности бесплатно распознавать голос и переводить в текст)

[Silk.NET](https://github.com/dotnet/Silk.NET) 2.17.1 (планируется - для удобного использования возможностей видеокарт различных устройств)

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

.NET Framework 4.8 Developer Pack и языки для перевода дебагера

https://dotnet.microsoft.com/en-us/download/dotnet-framework/

Microsoft Visual C++ Redistributable v14.0+ 2015-2022 (для некоторых сторонних библиотек)

https://learn.microsoft.com/ru-ru/cpp/windows/latest-supported-vc-redist?view=msvc-170

Совместимость по API

https://learn.microsoft.com/ru-ru/dotnet/standard/net-standard?tabs=net-standard-1-0#select-net-standard-version

Учебные материалы

https://professorweb.ru/my/csharp/charp_theory/level2/2_2.php

https://metanit.com/sharp/
