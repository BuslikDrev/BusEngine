/** API BusEngine.Game - пользовательский код */
namespace BusEngine.Game {
	/** API BusEngine.Plugin */
	public class MyPlugin : BusEngine.Plugin {
		private static System.Windows.Forms.Form SplashScreen = new System.Windows.Forms.Form();

		// при запуске BusEngine до создания формы
		public override void Initialize() {
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

			// запускаем видео
			if (BusEngine.Engine.Platform == "Windows" || BusEngine.Engine.Platform == "WindowsLauncher") {
				BusEngine.Video.Play("Videos/BusEngine.mp4");
			}

			// запускаем браузер
			if (BusEngine.Engine.Platform == "WindowsEditor") {
				BusEngine.Browser.Initialize("https://threejs.org/editor/");
			} else if (BusEngine.Engine.Platform == "WindowsLauncher") {
				BusEngine.Browser.Initialize("index.html");
				BusEngine.Browser.PostMessage += OnPostMessage;
			}
		}

		private static void OnPostMessage(object sender, string message) {
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
	}
	/** API BusEngine.Plugin */
}
/** API BusEngine.Game - пользовательский код */