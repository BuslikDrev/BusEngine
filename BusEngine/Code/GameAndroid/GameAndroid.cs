/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

/* C# 6.0+              https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history */
/* NET.Framework 4.7.1+ https://learn.microsoft.com/ru-ru/dotnet/framework/migration-guide/versions-and-dependencies */
/* MSBuild 15.0+        https://en.wikipedia.org/wiki/MSBuild#Versions */

/** дорожная карта
- написать лаунчер BusEngine
*/

#define BUSENGINE_XAMARINFORMS
#define BUSENGINE_ANDROID
/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
System.Windows.Forms
BusEngine.Engine
BusEngine.UI
BusEngine.Browser
*/
	internal class Initialize {
		private static void Run() {
			// инициализируем API BusEngine
			BusEngine.Engine.Platform = "Android";
			BusEngine.Engine.Initialize();

			// создаём форму System.Windows.Forms
			BusEngine.Form form = new BusEngine.Form();

			// устанавливаем нашу иконку
			if (System.IO.File.Exists(BusEngine.Engine.DataDirectory + "Icons/BusEngine.ico")) {
				form.Icon = new System.Drawing.Icon(System.IO.Path.Combine(BusEngine.Engine.DataDirectory, "Icons/BusEngine.ico"), 128, 128);
			}

			// устанавливаем размеры окна
			if (BusEngine.Engine.SettingEngine["console_commands"]["r_Width"] != null) {
				form.Width = System.Convert.ToInt32(BusEngine.Engine.SettingEngine["console_commands"]["r_Width"]);
			}
			if (BusEngine.Engine.SettingEngine["console_commands"]["r_Height"] != null) {
				form.Height = System.Convert.ToInt32(BusEngine.Engine.SettingEngine["console_commands"]["r_Height"]);
			}

			// открываем окно на весь экран
			if (System.Convert.ToInt32(BusEngine.Engine.SettingEngine["console_commands"]["r_Fullscreen"]) > 0) {
				form.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			}

			// убираем линии, чтобы окно было полностью на весь экран
			if (System.Convert.ToInt32(BusEngine.Engine.SettingEngine["console_commands"]["r_Fullscreen"]) == -1 || System.Convert.ToInt32(BusEngine.Engine.SettingEngine["console_commands"]["r_Fullscreen"]) == 1) {
				form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			} else if (System.Convert.ToInt32(BusEngine.Engine.SettingEngine["console_commands"]["r_Fullscreen"]) == -2) {
				form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
				form.MaximizeBox = true;
			}

			// подключаем API BusEngine.UI.Canvas
			BusEngine.UI.Canvas.WinForm = form;

			// инициализируем API BusEngine.UI.Canvas
			BusEngine.UI.Canvas.Initialize();

			// запускаем приложение System.Windows.Forms
			System.Windows.Forms.Application.Run(form);
		}

		/** функция запуска приложения */
		// https://www.cyberforum.ru/cmd-bat/thread940960.html
		// https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process.start?view=net-7.0
		//[System.STAThread] // если однопоточное приложение
		private static void Main(string[] args) {
			Run();
		}
		/** функция запуска приложения */
	}

	// https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.forms.form?view=netframework-4.8
	internal class Form : System.Windows.Forms.Form {
		/** функция запуска окна приложения */
		public Form() {
			// название окна
			this.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " BusEngine v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

			// системная иконка
			this.Icon = new System.Drawing.Icon(System.Drawing.SystemIcons.Exclamation, 128, 128);

			// устанавливаем размеры окна
			this.Width = 800;
			this.Height = 480;

			// центрируем окно
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

			// открываем окно на весь экран
			//this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

			// устанавливаем стиль границ окна
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;

			// убираем кнопку развернуть
			this.MaximizeBox = false;

			// убираем кнопку свернуть
			//this.MinimizeBox = false;

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