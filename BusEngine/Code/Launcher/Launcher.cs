/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

/* C# 6.0+              https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history */
/* NET.Framework 4.7.1+ https://learn.microsoft.com/ru-ru/dotnet/framework/migration-guide/versions-and-dependencies */
/* MSBuild 15.0+        https://en.wikipedia.org/wiki/MSBuild#Versions */

/** дорожная карта
- написать лаунчер с возможностью: безопасной регистрации, безопасной авторизации,
 скачать движок, восстановить файлы движка, удалить движок, создать проект, собрать проект,
 сгенерировать csproj, удалить проект, изменить язык лаунчера, настроить путь компилятора
 под каждый проект, настроить платформу для проекта с помощью чекбокса, добавить универсальное
 поле указания конфигурации csproj, вывести блок информации из сайта с ограничением в 4-6 шт
 (новости index.php?route=api/busengine/information&order=DESC&limit=4, последние товары кроме
 плагинов index.php?route=api/busengine/product&order=DESC&limit=4&type_exception=["plugin"],
 последние плагины index.php?route=api/busengine/product&order=DESC&limit=4&type=["plugin"])
*/

//#define RUN_LOG
/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
System.Windows.Forms
BusEngine.Engine
BusEngine.Log
BusEngine.UI
*/
	internal class Initialize {
		private static System.Threading.Mutex Mutex;
		//[System.STAThread]
		private static void Run() {
			// инициализируем API BusEngine
			BusEngine.Engine.Platform = "WindowsLauncher";
			BusEngine.Engine.OnInitialize += BusEngine.Initialize.OnRun;
			BusEngine.Engine.OnShutdown += BusEngine.Initialize.OnExit;
			BusEngine.Engine.Initialize();
		}

		private static void OnRun() {
			#if RUN_LOG
			BusEngine.Log.Info("OnRun");
			#endif
			// допускаем только один запуск
			bool createdNew;
			Mutex = new System.Threading.Mutex(true, "81145500-44c6-41c1-816d-be751929b38d", out createdNew);
			if (!createdNew) {
				string title;
				string desc;

				if (BusEngine.Localization.GetLanguageStatic("error_warning") != "error_warning") {
					title = BusEngine.Localization.GetLanguageStatic("error_warning");
				} else {
					title = "Увага!";
				}
				if (BusEngine.Localization.GetLanguageStatic("error_is_already_running") != "error_is_already_running") {
					desc = BusEngine.Localization.GetLanguageStatic("error_is_already_running");
				} else {
					desc = "Праграма ўжо запушчана.";
				}

				System.Windows.Forms.MessageBox.Show(desc, title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);

				//System.Windows.Forms.Application.Exit();

				return;
			}

			// создаём форму System.Windows.Forms
			BusEngine.Form form = new BusEngine.Form();

			// фикс создания дескриптора раньше плагинов
			System.IntPtr hWnd = form.Handle;

			// поверх всех окон
			//form.TopMost = true;

			// устанавливаем нашу иконку
			if (System.IO.File.Exists(BusEngine.Engine.DataDirectory + "Icons/BusEngine.ico")) {
				form.Icon = new System.Drawing.Icon(System.IO.Path.Combine(BusEngine.Engine.DataDirectory, "Icons/BusEngine.ico"), 128, 128);
			}

			// устанавливаем размеры окна
			string r_Width;
			if (BusEngine.Engine.SettingEngine["console_commands"].TryGetValue("r_Width", out r_Width)) {
				form.Width = System.Convert.ToInt32(r_Width);
			}
			string r_Height;
			if (BusEngine.Engine.SettingEngine["console_commands"].TryGetValue("r_Height", out r_Height)) {
				form.Height = System.Convert.ToInt32(r_Height);
			}

			string r_Fullscreen;
			if (BusEngine.Engine.SettingEngine["console_commands"].TryGetValue("r_Fullscreen", out r_Fullscreen)) {
				// открываем окно на весь экран
				if (System.Convert.ToInt32(r_Fullscreen) > 0) {
					form.WindowState = System.Windows.Forms.FormWindowState.Maximized;
				}

				// убираем линии, чтобы окно было полностью на весь экран
				if (System.Convert.ToInt32(r_Fullscreen) == -1 || System.Convert.ToInt32(r_Fullscreen) == 1) {
					form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
				} else if (System.Convert.ToInt32(r_Fullscreen) == -2) {
					form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
					form.MaximizeBox = true;
				}
			}

			// скрываем иконку в системном меню
			//form.ShowInTaskbar = false;

			// подключаем API BusEngine.UI.Canvas
			BusEngine.UI.Canvas.WinForm = form;

			// инициализируем API BusEngine.UI.Canvas
			BusEngine.UI.Canvas.Initialize();

			// запускаем приложение System.Windows.Forms
			System.Windows.Forms.Application.Run(BusEngine.UI.Canvas.WinForm);
		}

		private static void OnExit() {
			#if RUN_LOG
			BusEngine.Log.Info("OnExit");
			#endif

			//System.Windows.Forms.Application.EnableVisualStyles();
			//System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			// закрываем приложение System.Windows.Forms
			System.Windows.Forms.Application.Exit();
		}

		/** функция запуска приложения */
		// https://www.cyberforum.ru/cmd-bat/thread940960.html
		// https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process.start?view=net-7.0
		//[System.STAThread] // если однопоточное приложение
		[System.Runtime.ExceptionServices.HandleProcessCorruptedStateExceptions]
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

				//System.Windows.Forms.Application.Exit();

				return;
			} else {
				#if RUN_LOG
				try {
				#endif
					Run();
				#if RUN_LOG
				} catch (System.AccessViolationException e) {
					BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("error") + " " + BusEngine.Localization.GetLanguageStatic("error_audio_format") + ": {0}", e.Message);
					System.Console.Beep();
					System.Console.ReadLine();
				}
				#endif
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