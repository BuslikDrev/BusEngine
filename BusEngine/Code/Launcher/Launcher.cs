/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

/* C# 6.0+              https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history */
/* NET.Framework 4.5.1+ https://learn.microsoft.com/ru-ru/dotnet/framework/migration-guide/versions-and-dependencies */
/* MSBuild 12.0+        https://en.wikipedia.org/wiki/MSBuild#Versions */

/** API BusEngine.Game - пользовательский код */
namespace BusEngine.Game {
	/** API BusEngine.Plugin */
	public class MyPlugin : Plugin {
		// при заапуске BusEngine до создания формы
		public override void Initialize() {
			BusEngine.Log.Info("Initialize");
		}

		// после загрузки определённого плагина
		public override void Initialize(string plugin) {
			BusEngine.Log.Info("Initialize " + plugin);
		}

		// перед закрытием BusEngine
		public override void Shutdown() {
			BusEngine.Log.Info("Shutdown");
		}

		// перед загрузкой игрового уровня
		public override void OnLevelLoading(string level) {
			BusEngine.Log.Info("OnLevelLoading");
		}

		// после загрузки игрового уровня
		public override void OnLevelLoaded(string level) {
			BusEngine.Log.Info("OnLevelLoaded");
		}

		// когда икрок может управлять главным героем - время игры идёт
		public override void OnGameStart() {
			BusEngine.Log.Info("OnGameStart");
		}

		// когда время остановлено - пауза
		public override void OnGameStop() {
			BusEngine.Log.Info("OnGameStop");
		}

		// когда игрок начинает подключаться к серверу
		public override void OnClientConnectionReceived(int channelId) {
			BusEngine.Log.Info("OnClientConnectionReceived");
		}

		// кога игрок подключился к серверу
		public override void OnClientReadyForGameplay(int channelId) {
			BusEngine.Log.Info("OnClientReadyForGameplay");
		}

		// когда игрока выкинуло из сервера - обрыв связи с сервером
		public override void OnClientDisconnected(int channelId) {
			BusEngine.Log.Info("OnClientDisconnected");
		}
	}
	/** API BusEngine.Plugin */
}
/** API BusEngine.Game - пользовательский код */
