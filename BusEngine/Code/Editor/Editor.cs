/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

/* C# 6.0+              https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history */
/* NET.Framework 4.7.1+ https://learn.microsoft.com/ru-ru/dotnet/framework/migration-guide/versions-and-dependencies */
/* MSBuild 15.0+        https://en.wikipedia.org/wiki/MSBuild#Versions */

/** дорожная карта
- написать редактор BusEngine
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

		private static void Run() {
			// инициализируем API BusEngine
			BusEngine.Engine.Platform = "WindowsEditor";
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

			// подключаем API BusEngine.UI.Canvas
			BusEngine.UI.Canvas.WinForm = new BusEngine.Form();

			// инициализируем API BusEngine.UI.Canvas
			BusEngine.UI.Canvas.Initialize();

			// запускаем приложение System.Windows.Forms
			//System.Windows.Forms.Application.EnableVisualStyles();
			//System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
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
			// поверх всех окон
			this.TopMost = true;

			// название окна
			this.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " BusEngine v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

			// иконка
			if (System.IO.File.Exists(BusEngine.Engine.DataDirectory + "Icons/BusEngine.ico")) {
				this.Icon = new System.Drawing.Icon(System.IO.Path.Combine(BusEngine.Engine.DataDirectory, "Icons/BusEngine.ico"), 128, 128);
			} else {
				this.Icon = new System.Drawing.Icon(System.Drawing.SystemIcons.Exclamation, 128, 128);
			}

			// размеры окна
			this.Width = 800;
			this.Height = 480;

			string r_Width;
			if (BusEngine.Engine.SettingEngine["console_commands"].TryGetValue("r_Width", out r_Width)) {
				this.Width = System.Convert.ToInt32(r_Width);
			}
			string r_Height;
			if (BusEngine.Engine.SettingEngine["console_commands"].TryGetValue("r_Height", out r_Height)) {
				this.Height = System.Convert.ToInt32(r_Height);
			}

			// учёт Dpi
			// https://learn.microsoft.com/ru-ru/windows/win32/learnwin32/dpi-and-device-independent-pixels#converting-physical-pixels-to-dips
			this.Width = this.Width * this.DeviceDpi / 96;
			this.Height = this.Height * this.DeviceDpi / 96;

			BusEngine.Log.Info(this.Width);

			this.MinimumSize = new System.Drawing.Size(this.Width, this.Height);

			// цинтровка окна
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

			// кнопка развернуть
			this.MaximizeBox = false;

			// кнопка свернуть
			//this.MinimizeBox = false;

			// панель управления
			//this.ControlBox = false;

			string r_Fullscreen;
			if (BusEngine.Engine.SettingEngine["console_commands"].TryGetValue("r_Fullscreen", out r_Fullscreen)) {
				// убираем линии, чтобы окно было полностью на весь экран
				if (System.Convert.ToInt32(r_Fullscreen) == -1 || System.Convert.ToInt32(r_Fullscreen) == 1) {
					this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
					//this.ControlBox = false;
				} else if (System.Convert.ToInt32(r_Fullscreen) < -2 || System.Convert.ToInt32(r_Fullscreen) == 0 || System.Convert.ToInt32(r_Fullscreen) == 2) {
					this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
				} else {
					this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
				}

				// открываем окно на весь экран
				if (System.Convert.ToInt32(r_Fullscreen) > 0) {
					this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
				} else {
					this.WindowState = System.Windows.Forms.FormWindowState.Normal;
					if (System.Convert.ToInt32(r_Fullscreen) < 0) {
						this.MaximizeBox = true;
					}
				}
			} else {
				this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
				this.WindowState = System.Windows.Forms.FormWindowState.Normal;
			}

			// цвет фона окна
			this.BackColor = System.Drawing.Color.Black;

			// cобытие нажатий клавиш
			this.KeyPreview = true;
			//this.KeyDown += OnKeyDown;

			// скрытие иконки в системном меню
			//this.ShowInTaskbar = false;

			// событие закрытия окна
			//this.FormClosed += OnClosed;
			//this.Disposed += new System.EventHandler(OnDisposed);
			//ClientSize = this.ClientSize;

			/* System.Windows.Forms.Panel panel1 = new System.Windows.Forms.Panel();
			panel1.Location = this.Location;
			panel1.Size = this.Size;
			panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.Controls.Add(panel1); */

			// показываем форму\включаем\запускаем\стартуем показ окна
			//this.ShowDialog();

			// фикс создания дескриптора раньше плагинов
			System.IntPtr hWnd = this.Handle;
		}
		/** функция запуска окна приложения */

		// https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.forms.createparams?view=netframework-4.8
		// Style https://learn.microsoft.com/en-us/windows/win32/winmsg/window-styles
		// ClassStyle https://learn.microsoft.com/ru-ru/windows/win32/winmsg/window-class-styles
		protected override System.Windows.Forms.CreateParams CreateParams {
			get {
				System.Windows.Forms.CreateParams cp = base.CreateParams;

				if (this.FormBorderStyle == System.Windows.Forms.FormBorderStyle.None) {
					// рамка
					//cp.Style |= 0x40000;
					// тень рамки
					//cp.ClassStyle |= 0x20000;
					// Update the button Style.
					//cp.Style |= 0x00000040;
					// Double-buffering
					//cp.ExStyle |= 0x02000000;
				}

				return cp;
			}
		}
	}
}
/** API BusEngine */