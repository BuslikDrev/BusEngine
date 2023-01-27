/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

/** API BusEngine.Game - пользовательский код */
namespace BusEngine.Game {
	/** API BusEngine.Plugin */
	public class MyPlugin : BusEngine.Plugin {
		private static System.Windows.Forms.Form SplashScreen;
		private static BusEngine.Video video;

		// при запуске BusEngine до создания формы
		public override void Initialize() {
			// загружаем свой язык
			BusEngine.Localization.SOnLoad += OnLoadLanguage;
			BusEngine.Log.Info("2 {0}", BusEngine.Localization.SGetLanguage("error"));
			BusEngine.Localization localization = new BusEngine.Localization();
			localization.Load("Ukrainian");
			BusEngine.Log.Info("2 {0}", localization.GetLanguage("error"));
			BusEngine.Log.Info("2 {0}", BusEngine.Localization.SGetLanguage("error"));

			// добавляем стартовую обложку
			SplashScreen = new System.Windows.Forms.Form();
			SplashScreen.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			SplashScreen.Width = 640;
			SplashScreen.Height = 360;
			SplashScreen.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			if (System.IO.File.Exists(System.IO.Path.GetFullPath(BusEngine.Engine.DataDirectory + "Textures/UI/splashscreen.png"))) {
				SplashScreen.BackgroundImage = System.Drawing.Image.FromFile(BusEngine.Engine.DataDirectory + "Textures/UI/splashscreen.png");
			}
			SplashScreen.Show();
			System.Threading.Thread.Sleep(1000);
		}

		// при запуске BusEngine после создания формы Canvas
		public override void InitializeСanvas() {
			// убираем стартовую обложку
			SplashScreen.Close();
			SplashScreen.Dispose();
			BusEngine.UI.Canvas.WinForm.KeyDown += new System.Windows.Forms.KeyEventHandler(OnKeyDown);

			// запускаем видео
			if (BusEngine.Engine.Platform == "Windows" || BusEngine.Engine.Platform == "WindowsLauncher") {
				video = new BusEngine.Video();
				video.OnPlay += OnPlayVideo;
				video.OnStop += OnStopVideo;
				video.Play("Videos/BusEngine.mp4");
			}

			// запускаем браузер
			if (BusEngine.Engine.Platform == "WindowsEditor") {
				BusEngine.Browser.Initialize("https://threejs.org/editor/");
				BusEngine.Browser.Initialize("https://buslikdrev.by/");
			} else if (BusEngine.Engine.Platform == "WindowsLauncher") {
				BusEngine.Browser.Initialize("index.html");
				BusEngine.Browser.SOnPostMessage += OnPostMessage;
			}
		}

		/** событие нажатия любой кнопки */
		// https://learn.microsoft.com/en-us/dotnet/api/system.consolekey?view=netframework-4.8
		private void OnKeyDown(object o, System.Windows.Forms.KeyEventArgs e) {
			BusEngine.Log.Info("клавиатура клик {0}", o.GetType());
			BusEngine.Log.Info();
			// выключаем движок по нажатию на Esc
			if (e.KeyCode == System.Windows.Forms.Keys.Escape) {
				BusEngine.UI.Canvas.WinForm.KeyDown -= new System.Windows.Forms.KeyEventHandler(OnKeyDown);
				//Dispose();
				BusEngine.Engine.Shutdown();
			}
			// Вкл\Выкл консоль движка по нажатию на ~
			if (e.KeyCode == System.Windows.Forms.Keys.Oem3) {
				BusEngine.Log.ConsoleToggle();
			}
			// Выкл Видео
			if (e.KeyCode == System.Windows.Forms.Keys.Space) {
				video.Stop();
			}
		}
		/** событие нажатия любой кнопки */

		/** событие загрузки языка */
		private void OnLoadLanguage(BusEngine.Localization localization, string language) {
			BusEngine.Log.Info("Язык именился: {0}", language);
			BusEngine.Log.Info("Язык именился: {0}", localization);
			BusEngine.Log.Info("Язык именился: {0}", localization.GetLanguage("error"));
		}
		/** событие загрузки языка */

		/** событие запуска видео */
		private void OnPlayVideo(BusEngine.Video video, string url) {
			BusEngine.Log.Info("Видео запустилось: {0}", url);
			BusEngine.Log.Info("Видео запустилось: {0}", video);
			BusEngine.Log.Info("Видео запустилось: {0}", video.Url);
		}
		/** событие запуска видео */

		/** событие остановки видео */
		private void OnStopVideo(BusEngine.Video video, string url) {
			BusEngine.Log.Info("Видео остановилось: {0}", url);
			BusEngine.Log.Info("Видео остановилось: {0}", video);
			BusEngine.Log.Info("Видео остановилось: {0}", video.Url);
		}
		/** событие остановки видео */

		/** событие получения сообщения из браузера */
		private void OnPostMessage(string message) {
			if (message == "Exit") {
				BusEngine.Engine.Shutdown();
			} else if (message == "Debug") {
				BusEngine.Log.Info("JavaScript: Привет CSharp!");
				BusEngine.Log.Info("На команду: " + message);
				BusEngine.Browser.SExecuteJS("document.dispatchEvent(new CustomEvent('BusEngineMessage', {bubbles: true, detail: {hi: 'CSharp: Прювэт JavaScript!', data: 'Получил твою команду! Вось яна: " + message + "'}}));");
			} else {
				if (message.Substring(0, 8) == "console|") {
					BusEngine.Log.Info(message.Substring(8));
				}
			}
		}
		/** событие получения сообщения из браузера */
	}
	/** API BusEngine.Plugin */
}
/** API BusEngine.Game - пользовательский код */