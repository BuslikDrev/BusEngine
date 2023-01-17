/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

/* C# 6.0+              https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history */
/* NET.Framework 4.7.1+ https://learn.microsoft.com/ru-ru/dotnet/framework/migration-guide/versions-and-dependencies */
/* MSBuild 15.0+        https://en.wikipedia.org/wiki/MSBuild#Versions */

/** дорожная карта
- написать лаунчер BusEngine
*/

#define BUSENGINE_WINFORMS
#define BUSENGINE_WINDOWS
/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
System.Windows.Forms
BusEngine.Engine
BusEngine.UI
BusEngine.Browser
*/
	internal class Start {
		private static System.Threading.Mutex Mutex;

		/** функция запуска приложения */
		//[System.STAThread] // если однопоточное приложение
		private static void Main(string[] args) {
			// инициализируем API BusEngine
			BusEngine.Engine.Platform = "Windows";
			BusEngine.Engine.Initialize();

			// допускаем только один запуск
			bool createdNew;
			Mutex = new System.Threading.Mutex(true, "81145500-44c6-41c1-816d-be751929b38d", out createdNew);
			if (!createdNew) {
				string title;
				string desc;

				if (BusEngine.Localization.GetLanguage("error_warning") != "error_warning") {
					title = BusEngine.Localization.GetLanguage("error_warning");
				} else {
					title = "Увага!";
				}
				if (BusEngine.Localization.GetLanguage("error_warning_is_already_running") != "error_warning_is_already_running") {
					desc = BusEngine.Localization.GetLanguage("error_warning_is_already_running");
				} else {
					desc = "Праграма ўжо запушчана.";
				}

				System.Windows.Forms.MessageBox.Show(desc, title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);

				System.Windows.Forms.Application.Exit();

				return;
			}

			// создаём форму System.Windows.Forms
			BusEngine.Form form = new Form();

			// подключаем API BusEngine.UI.Canvas
			BusEngine.UI.Canvas.WinForm = form;
			BusEngine.UI.Canvas.Initialize();

			// запускаем браузер
			BusEngine.Browser.Start("index.html");
			BusEngine.Browser.PostMessage += OnPostMessage;

			BusEngine.Log.Info("dddddddddddddddddddddd");
			System.Reflection.Assembly curAssembly = typeof(BusEngine.Engine).Assembly;
			BusEngine.Log.Info("The current executing assembly is {0}.", curAssembly);

			System.Reflection.Module[] mods = curAssembly.GetModules();
			foreach (System.Reflection.Module md in mods) {
				BusEngine.Log.Info("This assembly contains the Game.exe {0} module", md.Name);
			}
			BusEngine.Log.Info("dddddddddddddddddddddd");
			System.Reflection.Assembly mainAssembly = typeof(BusEngine.Start).Assembly;
			BusEngine.Log.Info("The executing assembly is {0}.", mainAssembly);
			System.Reflection.Module[] modss = mainAssembly.GetModules();
			BusEngine.Log.Info("\tModules in the assembly:");
			foreach (System.Reflection.Module m in modss) {
				BusEngine.Log.Info("\t{0}", m);
			}
			BusEngine.Log.Info("dddddddddddddddddddddd");

			// запускаем приложение System.Windows.Forms
			System.Windows.Forms.Application.Run(form);
		}
		/** функция запуска приложения */

		private static void OnPostMessage(object sender, string message) {
			if (message == "Exit") {
				BusEngine.Engine.Shutdown();
			} else if (message == "Debug") {
				BusEngine.Log.Info("JavaScript: Привет CSharp!");
				BusEngine.Log.Info("На команду: " + message);
				BusEngine.Browser.ExecuteJS("document.dispatchEvent(new CustomEvent('BusEngineMessage', {bubbles: true, detail: {hi: 'CSharp: Прювэт JavaScript!', data: 'Получил твою команду! Вось яна: " + message + "'}}));");
			}
		}
	}

	// https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.forms.form?view=netframework-4.8
	internal class Form : System.Windows.Forms.Form {
		/** функция запуска окна приложения */
		public Form() {
			// название окна
			this.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " BusEngine v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

			// устанавливаем нашу иконку, есди она есть по пути exe, в противном случае устанавливаем системную
			if (System.IO.File.Exists(BusEngine.Engine.DataDirectory + "Icons/BusEngine.ico")) {
				this.Icon = new System.Drawing.Icon(System.IO.Path.Combine(BusEngine.Engine.DataDirectory, "Icons/BusEngine.ico"), 128, 128);
			} else {
				this.Icon = new System.Drawing.Icon(System.Drawing.SystemIcons.Exclamation, 128, 128);
			}

			// устанавливаем размеры окна
			this.Width = 1024;
			this.Height = 768;

			// центрируем окно
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

			// открываем окно на весь экран
			//this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

			// убираем линии, чтобы окно было полностью на весь экран
			//this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;

			// устанавливаем чёрный цвет фона окна
			this.BackColor = System.Drawing.Color.Black;

			// устанавливаем событие нажатий клавиш
			this.KeyPreview = true;
			//this.KeyDown += OnKeyDown;

			// устанавливаем событие закрытия окна
			//this.FormClosed += OnClosed;
			//this.Disposed += new System.EventHandler(OnDisposed);
			//ClientSize = this.ClientSize;

			// показываем форму\включаем\запускаем\стартуем показ окна
			//this.ShowDialog();
		}
		/** функция запуска окна приложения */
	}
}
/** API BusEngine */