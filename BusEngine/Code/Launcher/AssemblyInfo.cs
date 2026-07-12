/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2024; BuslikDrev - Усе правы захаваны. */

/* C# 6.0+              https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history */
/* NET.Framework 4.7.1+ https://learn.microsoft.com/ru-ru/dotnet/framework/migration-guide/versions-and-dependencies */
/* MSBuild 15.0+        https://en.wikipedia.org/wiki/MSBuild#Versions */

//https://learn.microsoft.com/en-us/dotnet/api/system.reflection?view=netframework-4.7.1
[assembly: System.Reflection.AssemblyTitle("BusEngine Launcher")]
[assembly: System.Reflection.AssemblyDescription("The main C# interface for the BusEngine.")]
[assembly: System.Reflection.AssemblyConfiguration("")]
[assembly: System.Reflection.AssemblyCompany("BuslikDrev")]
[assembly: System.Reflection.AssemblyProduct("BusEngine")]
[assembly: System.Reflection.AssemblyCopyright("Copyright 2016-2026 BuslikDrev - Усе правы захаваны.")]
[assembly: System.Reflection.AssemblyTrademark("")]
[assembly: System.Reflection.AssemblyCulture("")]
[assembly: System.Reflection.AssemblyVersion("0.4.0.0")]
[assembly: System.Reflection.AssemblyFileVersion("0.4.0.0")]
[assembly: System.Runtime.InteropServices.ComVisible(false)]
[assembly: System.Runtime.InteropServices.GuidAttribute("36edf534-2219-4de4-8491-68b4aa3f69dc")]
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("BusEngine")]
//https://professorweb.ru/my/csharp/base_net/level2/2_5.php
//[assembly: System.Security.SecurityRules(System.Security.SecurityRuleSet.Level1)]



/* 2. AssemblyTitle и SmartScreen
То, что ты указал AssemblyTitle("BusEngine Launcher") и AssemblyCompany("BuslikDrev"), очень поможет при подписи. Когда ты подпишешь этот EXE сертификатом на имя "BuslikDrev", Windows сопоставит поле "Company" из кода и "Subject" из сертификата. Это дает дополнительные очки доверия.

3. Нюанс с InternalsVisibleTo
Строка [assembly: System.Runtime.CompilerServices.InternalsVisibleTo("BusEngine")] — это грамотное решение для модульного движка. Но помни: если ты в будущем решишь использовать строгие имена (Strong Name Signing) для своих библиотек (ключи .snk), то в этой строке нужно будет указывать еще и публичный токен ключа, иначе «дружба» между DLL перестанет работать.
 */