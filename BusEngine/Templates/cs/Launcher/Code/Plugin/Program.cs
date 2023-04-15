/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

//#define BROWSER_LOG
#define LOCALIZATION_LOG
/** API BusEngine.Game - пользовательский код */
namespace BusEngine.Game {
	/** API BusEngine.Plugin */
	public class MyPlugin : BusEngine.Plugin {
		//private static System.Windows.Forms.Form SplashScreen;

		// при запуске BusEngine до создания формы
		public override void Initialize() {
			// загружаем свой язык
			BusEngine.Localization.OnLoadStatic += (BusEngine.Localization l, string language) => {
				#if LOCALIZATION_LOG
				BusEngine.Log.Info("Язык изменился: {0}", language);
				BusEngine.Log.Info("Язык изменился: {0}", l.GetLanguage("error"));
				#endif
			};
			new BusEngine.Localization().Load("Ukrainian");

			// добавляем стартовую обложку
			/* SplashScreen = new System.Windows.Forms.Form();
			SplashScreen.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			SplashScreen.Width = 640;
			SplashScreen.Height = 360;
			SplashScreen.TopMost = true;
			SplashScreen.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			if (System.IO.File.Exists(System.IO.Path.GetFullPath(BusEngine.Engine.DataDirectory + "Textures/UI/splashscreen.png"))) {
				SplashScreen.BackgroundImage = System.Drawing.Image.FromFile(BusEngine.Engine.DataDirectory + "Textures/UI/splashscreen.png");
			}
			SplashScreen.Show(); */

			// устанавливаем настройки
			BusEngine.Engine.SettingEngine["console_commands"]["r_Width"] = "1280";
			BusEngine.Engine.SettingEngine["console_commands"]["r_Height"] = "720";

			//System.Threading.Tasks.Task.Delay(2000).Wait();
		}

		// при запуске BusEngine после создания формы Canvas
		public override void InitializeСanvas() {
			// убираем стартовую обложку
			/* SplashScreen.Close();
			SplashScreen.Dispose(); */
			
			//BusEngine.UI.Canvas.WinForm.TransparencyKey = System.Drawing.Color.Black;
			//BusEngine.UI.Canvas.WinForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			//BusEngine.UI.Canvas.WinForm.Width = 800;
			//BusEngine.UI.Canvas.WinForm.Height = 480;
			// деламем окно не поверх других окон
			BusEngine.UI.Canvas.WinForm.TopMost = false;

			// запускаем браузер
			Browser("index.html");
		}
		
		private static void Paint3(object sender, System.Windows.Forms.PaintEventArgs e) {
			// труба (прямоугольник)
			e.Graphics.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Black), 480, 125, 75, 75);
		}

		// запускаем браузер WinForm только в одном потоке =(
		private void Browser(string url) {
			BusEngine.Browser.Initialize(url);                                                   
			BusEngine.Browser.OnPostMessageStatic += (string message) => {
				if (message == "Collapse") {
					BusEngine.UI.Canvas.WinForm.WindowState = System.Windows.Forms.FormWindowState.Minimized;
				} else if (message == "Expand") {
					if (BusEngine.UI.Canvas.WinForm.WindowState == System.Windows.Forms.FormWindowState.Maximized) {
						BusEngine.UI.Canvas.WinForm.WindowState = System.Windows.Forms.FormWindowState.Normal;
					} else {
						BusEngine.UI.Canvas.WinForm.WindowState = System.Windows.Forms.FormWindowState.Maximized;
					}
				} else if (message == "Exit") {
					BusEngine.Engine.Shutdown();
				} else if (message == "Debug") {
					BusEngine.Log.Info("JavaScript: Привет CSharp!");
					BusEngine.Log.Info("На команду: " + message);
					BusEngine.Browser.ExecuteJSStatic("document.dispatchEvent(new CustomEvent('BusEngineMessage', {bubbles: true, detail: {hi: 'CSharp: Прювэт JavaScript!', data: 'Получил твою команду! Вось яна: " + message + "'}}));");
				} else {
					if (message.Substring(0, 8) == "console|") {
						System.Console.ForegroundColor = System.ConsoleColor.Cyan;
						BusEngine.Log.Info("Console: {0}", message.Substring(8));
						System.Console.ResetColor();
					}
				}
			};
		}
	}
	/** API BusEngine.Plugin */
}
/** API BusEngine.Game - пользовательский код */