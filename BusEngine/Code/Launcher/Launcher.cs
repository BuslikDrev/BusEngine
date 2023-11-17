/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2024; BuslikDrev - Усе правы захаваны. */

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

		private /* async */ static void /* System.Threading.Tasks.Task */ Run(string[] args) {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Initialize.Run")) {
			#endif
			// инициализируем API BusEngine
			BusEngine.Engine.Platform = "WindowsLauncher";
			BusEngine.Engine.Commands = args;
			BusEngine.Engine.OnInitialize += BusEngine.Initialize.OnRun;
			BusEngine.Engine.OnShutdown += BusEngine.Initialize.OnExit;
			BusEngine.Engine.Initialize();
			#if BUSENGINE_BENCHMARK
			}
			#endif
		}

		private static void OnRun() {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Initialize.OnRun")) {
			#endif
			#if RUN_LOG
			BusEngine.Log.Info("OnRun");
			#endif

			BusEngine.Engine.OnInitialize -= BusEngine.Initialize.OnRun;

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

				System.Windows.Forms.Application.Exit();

				System.Environment.Exit(0);

				return;
			}

			#if BUSENGINE_BENCHMARK
			}
			#endif

			// запускаем приложение System.Windows.Forms
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.Run(new BusEngine.Form());
		}

		private static void OnExit() {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Initialize.OnExit")) {
			#endif
			#if RUN_LOG
			BusEngine.Log.Info("OnExit");
			#endif

			//System.Windows.Forms.Application.EnableVisualStyles();
			//System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
			// закрываем приложение System.Windows.Forms
			System.Threading.Tasks.Task.Run(() => {
				System.Windows.Forms.Application.Exit();
			});

			#if BUSENGINE_BENCHMARK
			}
			#endif
		}

		/** функция запуска приложения */
		// https://www.cyberforum.ru/cmd-bat/thread940960.html
		// https://learn.microsoft.com/en-us/dotnet/api/system.diagnostics.process.start?view=net-7.0
		// https://learn.microsoft.com/ru-ru/dotnet/desktop/winforms/controls/multithreading-in-windows-forms-controls?view=netframeworkdesktop-4.8
		//[System.STAThread] // если однопоточное приложение
		//[System.Security.SecurityCriticalAttribute]
		private static void Main(string[] args) {
			/** моя мечта
			if (WINXP) {
				System.Windows.Forms.Form form = new System.Windows.Forms.Form();
				BusEngine.UI.Canvas(form);
				Android.App.LoadApplication(form);
			} else if (ANDROID) {
				Xamarin.Forms.Application form = new Xamarin.Forms.Application();
				BusEngine.UI.Canvas(form);
				Xamarin.Forms.LoadApplication(form);
			} else {
				System.Windows.Application form = new System.Windows.Application();
				BusEngine.UI.Canvas(form);
				System.Windows.Application.Run(form);
			}
			*/

			string Location = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

			// проверяем целостность библиотек движка
			if (!System.IO.File.Exists(Location + "\\BusEngine.dll")) {
				string title;
				string desc;
				string lang = System.Globalization.CultureInfo.CurrentCulture.EnglishName;

				if (lang == "English") {
					title = "Memory Manager";
					desc = "Memory Manager: Unable to bind memory management functions. Cloud not access BusEngine.dll (check working directory)";
				} else if (lang == "Russian") {
					title = "Диспетчер памяти";
					desc = "Диспетчер памяти: невозможно связать функции управления памятью. Облако не имеет доступа к BusEngine.dll (проверьте рабочий каталог)";
				} else if (lang == "Ukrainian") {
					title = "Менеджер пам'яті";
					desc = "Менеджер пам'яті: не можна зв'язати функції керування пам'яттю. Хмара не має доступу до BusEngine.dll (перевірте робочий каталог)";
				} else {
					title = "Дыспетчар памяці";
					desc = "Дыспетчар памяці: немагчыма звязаць функцыі кіравання памяццю. Воблака не мае доступу да BusEngine.dll (праверце працоўны каталог)";
				}

				System.Windows.Forms.MessageBox.Show(desc, title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);

				System.GC.WaitForPendingFinalizers();
				System.GC.Collect();
			} else {
				#if RUN_LOG
				try {
				#endif
					// тест оптимизации
					//https://learn.microsoft.com/ru-ru/dotnet/core/extensions/caching
					System.Runtime.ProfileOptimization.SetProfileRoot(Location);
					System.Runtime.ProfileOptimization.StartProfile("JITCache.prof");

					//https://learn.microsoft.com/en-us/dotnet/api/system.runtime.gcsettings?view=netframework-4.8
					/* System.Runtime.GCSettings.LatencyMode = System.Runtime.GCLatencyMode.Batch;
					System.Runtime.GCSettings.LargeObjectHeapCompactionMode = System.Runtime.GCLargeObjectHeapCompactionMode.CompactOnce; */

					BusEngine.Initialize.Run(args);
					//BusEngine.Initialize.Run(args).Wait();
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

	// https://www.cyberforum.ru/blogs/529033/blog3609.html
	// https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.forms.form?view=netframework-4.8
	internal class Form : System.Windows.Forms.Form {
		/** функция запуска окна приложения */
		public Form() {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Form")) {
			#endif
			// поверх всех окон
			this.TopMost = true;
			this.TopLevel = true;

			// цвет фона окна
			this.BackColor = System.Drawing.Color.Black;

			// размеры окна и учёт Dpi
			// https://learn.microsoft.com/ru-ru/windows/win32/learnwin32/dpi-and-device-independent-pixels#converting-physical-pixels-to-dips
			int r_Width;
			this.Width = (int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["r_Width"], out r_Width) ? r_Width : 1280) * this.DeviceDpi / 96;
			int r_Height;
			this.Height = (int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["r_Height"], out r_Height) ? r_Height : 720) * this.DeviceDpi / 96;

			// размер экрана
			//BusEngine.UI.Canvas.Screen.Width = System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width;

			// цинтровка окна
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

			// название окна
			this.Text = BusEngine.Engine.SettingProject["info"]["name"] + " v" + BusEngine.Engine.SettingProject["info"]["version"];

			// иконка
			if (System.IO.File.Exists(BusEngine.Engine.SettingProject["info"]["icon"])) {
				this.Icon = new System.Drawing.Icon(System.IO.Path.Combine(BusEngine.Engine.SettingProject["info"]["icon"]), 128, 128);
			} else {
				this.Icon = new System.Drawing.Icon(System.Drawing.SystemIcons.Exclamation, 128, 128);
			}

			// минимальный размер окна
			this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);

			// кнопка развернуть
			this.MaximizeBox = false;

			// мерцание
			this.DoubleBuffered = true;

			// кнопка свернуть
			//this.MinimizeBox = false;

			// панель управления
			//this.ControlBox = false;

			int r_Fullscreen = System.Convert.ToInt32(BusEngine.Engine.SettingProject["console_commands"]["r_Fullscreen"]);
			// убираем линии, чтобы окно было полностью на весь экран
			if (r_Fullscreen == -1 || r_Fullscreen == 1) {
				this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
				//this.ControlBox = false;
			} else if (r_Fullscreen < -2 || r_Fullscreen == 0 || r_Fullscreen == 2) {
				this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			} else {
				this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
			}

			// открываем окно на весь экран
			if (r_Fullscreen > 0) {
				this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			} else {
				this.WindowState = System.Windows.Forms.FormWindowState.Normal;
				if (r_Fullscreen < 0) {
					this.MaximizeBox = true;
				}
			}

			// cобытие нажатий клавиш
			this.KeyPreview = true;
			//this.KeyDown += OnKeyDown;

			// скрытие иконки в системном меню
			//this.ShowInTaskbar = true;

			// событие закрытия окна
			//this.FormClosed += OnClosed;
			//this.Disposed += new System.EventHandler(OnDisposed);
			//ClientSize = this.ClientSize;

			/* System.Windows.Forms.Panel panel1 = new System.Windows.Forms.Panel();
			panel1.Location = this.Location;
			panel1.Size = this.Size;
			panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(panel1); */

			// фикс создания дескриптора раньше плагинов
			System.IntPtr hWnd = this.Handle;

			// показываем форму\включаем\запускаем\стартуем показ окна
			//this.ShowDialog();
			//this.Show();

			//this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
			#if BUSENGINE_BENCHMARK
			}
			#endif

			// подключаем API BusEngine.UI.Canvas
			BusEngine.UI.Canvas.WinForm = this;

			// инициализируем API BusEngine.UI.Canvas
			BusEngine.UI.Canvas.Initialize();
		}
		/** функция запуска окна приложения */

		// https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.forms.createparams?view=netframework-4.8
		// Style https://learn.microsoft.com/en-us/windows/win32/winmsg/window-styles
		// ClassStyle https://learn.microsoft.com/ru-ru/windows/win32/winmsg/window-class-styles
		/* protected override System.Windows.Forms.CreateParams CreateParams {
			get {
				System.Windows.Forms.CreateParams cp = base.CreateParams;

				if (this.FormBorderStyle == System.Windows.Forms.FormBorderStyle.None) {
					// рамка
					//cp.Style |= 0x00040000;
					//this.Padding = new System.Windows.Forms.Padding(0);
					//this.Margin = new System.Windows.Forms.Padding(-100);
					// тень рамки
					//cp.ClassStyle |= 0x00000200;
					// Update the button Style.
					//cp.Style |= 0x00000040;
					// Double-buffering
					//cp.ExStyle |= 0x00800;
				}

				return cp;
			}
		} */
	}
}
/** API BusEngine */