/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

/* C# 6.0+              https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history */
/* NET.Framework 4.7.1+ https://learn.microsoft.com/ru-ru/dotnet/framework/migration-guide/versions-and-dependencies */
/* MSBuild 15.0+        https://en.wikipedia.org/wiki/MSBuild#Versions */

/** API BusEngine.Game - пользовательский код */
namespace BusEngine.Game {
	/** API BusEngine.Plugin */
	public class MyPlugin : BusEngine.Plugin {
		// при заапуске BusEngine до создания формы
		public override void Initialize() {
			BusEngine.Log.Info("MyPlugin Initialize");
		}

		// после загрузки определённого плагина
		public override void Initialize(string plugin) {
			BusEngine.Log.Info("MyPlugin Initialize: {0}", plugin);
		}

		// перед закрытием BusEngine
		public override void Shutdown() {
			BusEngine.Log.Info("MyPlugin Shutdown");
			//base.Shutdown();
		}

		// перед загрузкой игрового уровня
		public override void OnLevelLoading(string level) {
			BusEngine.Log.Info("MyPlugin OnLevelLoading: {0}", level);
		}

		// после загрузки игрового уровня
		public override void OnLevelLoaded(string level) {
			BusEngine.Log.Info("MyPlugin OnLevelLoaded: {0}", level);
		}

		// когда игрок может управлять главным героем - время игры идёт
		public override void OnGameStart() {
			BusEngine.Log.Info("MyPlugin OnGameStart");
		}

		// когда время остановлено - пауза или закрытие уровня
		public override void OnGameStop() {
			BusEngine.Log.Info("MyPlugin OnGameStop");
		}

		// вызывается при отрисовки каждого кадра
		public override void OnGameUpdate() {
			BusEngine.Log.Info("MyPlugin OnGameUpdate");
		}

		// когда игрок начинает подключаться к серверу
		public override void OnClientConnectionReceived(int channelId) {
			BusEngine.Log.Info("MyPlugin OnClientConnectionReceived: {0}", channelId);
		}

		// когда игрок подключился к серверу
		public override void OnClientReadyForGameplay(int channelId) {
			BusEngine.Log.Info("MyPlugin OnClientReadyForGameplay: {0}", channelId);
		}

		// когда игрока выкинуло из сервера - обрыв связи с сервером
		public override void OnClientDisconnected(int channelId) {
			BusEngine.Log.Info("MyPlugin OnClientDisconnected: {0}", channelId);
		}
	}
	/** API BusEngine.Plugin */
}
/** API BusEngine.Game - пользовательский код */