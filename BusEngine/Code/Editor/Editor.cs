/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

/* C# 6.0+              https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history */
/* NET.Framework 4.7.1+ https://learn.microsoft.com/ru-ru/dotnet/framework/migration-guide/versions-and-dependencies */
/* MSBuild 15.0+        https://en.wikipedia.org/wiki/MSBuild#Versions */

/** дорожная карта
- написать редактор BusEngine
*/

#define BUSENGINE_WINFORMS
#define BUSENGINE_WINDOWSEDITOR
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
		private static System.Threading.Mutex Mutex;
		private static void Run() {
			// инициализируем API BusEngine
			BusEngine.Engine.Platform = "WindowsEditor";
			BusEngine.Engine.Initialize();

			// допускаем только один запуск
			bool createdNew;
			Mutex = new System.Threading.Mutex(true, "28cb03ec-5416-439d-81a7-b530e7a54c2a", out createdNew);
			if (!createdNew) {
				string title;
				string desc;

				if (BusEngine.Localization.SGetLanguage("error_warning_busasd") != "error_warning_busasd") {
					title = BusEngine.Localization.SGetLanguage("error_warning_busasd");
				} else {
					title = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " BusEngine ужо запушчаны";
				}
				if (BusEngine.Localization.SGetLanguage("error_warning_is_already_running_Yes") != "error_warning_is_already_running_Yes") {
					desc = BusEngine.Localization.SGetLanguage("error_warning_is_already_running_Yes");
				} else {
					desc = "Праграма ўжо запушчана. Працягнуць запуск копіі?";
				}

				if (System.Windows.Forms.MessageBox.Show(desc, title, System.Windows.Forms.MessageBoxButtons.YesNo, System.Windows.Forms.MessageBoxIcon.Exclamation) == System.Windows.Forms.DialogResult.No) {
					System.Windows.Forms.Application.Exit();

					return;
				}
			}

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
			// проверяем целостность библиотек движка
			if (!System.IO.File.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\BusEngine.dll")) {
				string title;
				string desc;

				if (System.Globalization.CultureInfo.CurrentCulture.EnglishName == "English") {
					title = "Memory Manager";
					desc = "Memory Manager: Unable to bind memory management functions. Cloud not access BusEngine.dll (check working directory)";
				} else if (System.Globalization.CultureInfo.CurrentCulture.EnglishName == "Russian") {
					title = "Диспетчер памяти";
					desc = "Диспетчер памяти: невозможно связать функции управления памятью. Облако не имеет доступа к BusEngine.dll (проверьте рабочий каталог)";
				} else if (System.Globalization.CultureInfo.CurrentCulture.EnglishName == "Ukrainian") {
					title = "Менеджер пам'яті";
					desc = "Менеджер пам'яті: не можна зв'язати функції керування пам'яттю. Хмара не має доступу до BusEngine.dll (перевірте робочий каталог)";
				} else {
					title = "Дыспетчар памяці";
					desc = "Дыспетчар памяці: немагчыма звязаць функцыі кіравання памяццю. Воблака не мае доступу да BusEngine.dll (праверце працоўны каталог)";
				}

				System.Windows.Forms.MessageBox.Show(desc, title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);

				System.Windows.Forms.Application.Exit();

				return;
			} else {
				Run();
			}
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

			// https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.forms.controlstyles?view=netframework-4.6.2#system-windows-forms-controlstyles-userpaint
			// убираем мерцание и доступна настройка только в этом месте.
			/* this.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(System.Windows.Forms.ControlStyles.FixedHeight, false);
			this.SetStyle(System.Windows.Forms.ControlStyles.FixedWidth, false); */

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