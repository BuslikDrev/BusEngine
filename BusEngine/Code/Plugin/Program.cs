/** API BusEngine.Game - пользовательский код */
#if BUSENGINE_WINFORMS
namespace BusEngine.Game {
	/** API BusEngine.Plugin */
	public class MyPlugin : BusEngine.Plugin {
		private static System.Windows.Forms.Form SplashScreen;
		private static BusEngine.Video video;
		private BusEngine.Localization lang;

		// при запуске BusEngine до создания формы
		public override void Initialize() {
			BusEngine.Localization.OnLoad += OnLoadLanguage;
			BusEngine.Log.Info("2 {0}", BusEngine.Localization.GetLanguage("error"));
			lang = new BusEngine.Localization();
			lang.Load("Ukrainian");
			BusEngine.Log.Info("2 {0}", BusEngine.Localization.GetLanguage("error"));

			// добавляем обложку
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
			SplashScreen.Close();
			SplashScreen.Dispose();
			BusEngine.UI.Canvas.WinForm.KeyDown += new System.Windows.Forms.KeyEventHandler(OnKeyDown);

			// запускаем видео
			if (BusEngine.Engine.Platform == "Windows" || BusEngine.Engine.Platform == "WindowsLauncher") {
				video = new BusEngine.Video().Play("Videos/BusEngine.mp4");
			}

			// запускаем браузер
			/* if (BusEngine.Engine.Platform == "WindowsEditor") {
				BusEngine.Browser.Initialize("https://threejs.org/editor/");
				BusEngine.Browser.Initialize("https://buslikdrev.by/");
			} else if (BusEngine.Engine.Platform == "WindowsLauncher") {
				BusEngine.Browser.Initialize("index.html");
				BusEngine.Browser.OnPostMessage += OnPostMessage;
			} */
		}

		/** событие нажатия любой кнопки */
		// https://learn.microsoft.com/en-us/dotnet/api/system.consolekey?view=netframework-4.8
		private void OnKeyDown(object o, System.Windows.Forms.KeyEventArgs e) {
			BusEngine.Log.Info("клавиатура клик");
			BusEngine.Log.Info();
			// выключаем движок по нажатию на Esc
			if (e.KeyCode == System.Windows.Forms.Keys.Escape) {
				#if BUSENGINE_WINFORMS
				BusEngine.UI.Canvas.WinForm.KeyDown -= new System.Windows.Forms.KeyEventHandler(OnKeyDown);
				#endif
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
				//video.Shutdown();
				//video.Play("Videos/BusEngine.mp4");
			}
		}
		/** событие нажатия любой кнопки */

		/** событие загрузки языка */
		private void OnLoadLanguage(object sender, string language) {
			BusEngine.Log.Info("Язык именился: {0}", language);
			BusEngine.Log.Info("Язык именился: {0}", sender);
			BusEngine.Log.Info("Язык именился: {0}", lang.Get("error"));
		}
		/** событие загрузки языка */

		/** событие получения сообщения из браузера */
		private static void OnPostMessage(string message) {
			if (message == "Exit") {
				BusEngine.Engine.Shutdown();
			} else if (message == "Debug") {
				BusEngine.Log.Info("JavaScript: Привет CSharp!");
				BusEngine.Log.Info("На команду: " + message);
				BusEngine.Browser.ExecuteJS("document.dispatchEvent(new CustomEvent('BusEngineMessage', {bubbles: true, detail: {hi: 'CSharp: Прювэт JavaScript!', data: 'Получил твою команду! Вось яна: " + message + "'}}));");
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
#endif