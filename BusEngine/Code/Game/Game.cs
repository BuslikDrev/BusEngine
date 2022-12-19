/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

/* C# 6.0+              https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history */
/* NET.Framework 4.6.1+ https://learn.microsoft.com/ru-ru/dotnet/framework/migration-guide/versions-and-dependencies */
/* MSBuild 12.0+        https://en.wikipedia.org/wiki/MSBuild#Versions */
/* MSBuild 15.0+        https://learn.microsoft.com/en-us/xamarin/android/app-fundamentals/android-api-levels?tabs=windows#android-versions */
/* Mono                 https://learn.microsoft.com/ru-ru/xamarin/android/deploy-test/building-apps/abi-specific-apks */
/* ссылки по Android 
https://learn.microsoft.com/ru-ru/xamarin/android/app-fundamentals/permissions?tabs=windows
*/

/** дорожная карта
- проставить нормально модификаторы доступа https://metanit.com/sharp/tutorial/3.2.php
- максимально весь функционал сделать независимыми плагинами и установить проверки
 на наличие плагинов перед их использованием
- апи перенести в проект библиотеки, а текущий, только для запуска и остановки программы,
 с возможностью отказа от использования API BusEngine
- создать: генерацию сцены (карты), камеру, консольные команды, консоль, настройка проекта
- написать лаунчер с возможностью: безопасной регистрации, безопасной авторизации,
 скачать движок, восстановить файлы движка, удалить движок, создать проект, собрать проект,
 сгенерировать csproj, удалить проект, изменить язык лаунчера, настроить путь компилятора
 под каждый проект, настроить платформу для проекта с помощью чекбокса, добавить универсальное
 поле указания конфигурации csproj, вывести блок информации из сайта с ограничением в 4-6 шт
 (новости index.php?route=api/busengine/information&order=DESC&limit=4, последние товары кроме
 плагинов index.php?route=api/busengine/product&order=DESC&limit=4&type_exception=["plugin"],
 последние плагины index.php?route=api/busengine/product&order=DESC&limit=4&type=["plugin"])
- написать редактор игры и добавить управление тем, чем располагает текущий АПИ
- написать сборку игры для windows 7+ и Android 5+
*/

#define BUSENGINE_WINFORM
namespace BusEngine {
	//https://learn.microsoft.com/ru-ru/dotnet/csharp/language-reference/preprocessor-directives
	//https://learn.microsoft.com/ru-ru/dotnet/csharp/programming-guide/classes-and-structs/constants
	public class Global {
		//public const bool BUSENGINE_WINFORM = true;
	}

	internal class ProjectSettingDefault {
		public object console_commands {get; set;}
		public object console_variables {get; set;}
		public string version {get; set;}
		public string type {get; set;}
		public object info {get; set;}
		public object content {get; set;}
		public object require {get; set;}

		public ProjectSettingDefault() {
			console_commands = new {
				sys_spec = "1",
				e_WaterOcean = "0",
				r_WaterOcean = "0",
				r_VolumetricClouds = "1",
				r_Displayinfo = "0",
				r_Fullscreen = "0",
				r_Width = "1280",
				r_Height = "720",
			};
			console_variables = new {
				sys_spec = "1",
				e_WaterOcean = "0",
				r_WaterOcean = "0",
				r_VolumetricClouds = "1",
				r_Displayinfo = "0",
				r_Fullscreen = "0",
				r_Width = "1280",
				r_Height = "720",
			};
			version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			type = "";
			info = new {
				name = "Game",
				guid = "ddc2049b-3a86-425b-9713-ee1babec5365"
			};
			content = new {
				assets = new string[] {"GameData"},
				code = new string[] {"Code"},
				libs = new {
					name = "BusEngine",
					shared = new {
						any = "",
						Android = "",
						win = "",
						win_x64 = "",
						win_x86 = "",
					},
				},
			};
			require = new {
				engine = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
				plugins = new object[] {
					new {
						guid = "",
						type = "EType::Managed",
						path = "bin/Android/Game.dll",
						platforms = new string[] {"Android"},
					},
					new {
						guid = "",
						type = "EType::Managed",
						path = "bin/win/Game.dll",
						platforms = new string[] {"win_x86"},
					},
					new {
						guid = "",
						type = "EType::Managed",
						path = "bin/win_x86/Game.dll",
						platforms = new string[] {"win_x86"},
					},
					new {
						guid = "",
						type = "EType::Managed",
						path = "bin/win_x64/Game.dll",
						platforms = new string[] {"Win_x64"},
					}
				},
			};
		}
	}

	internal class Start {
		//public const bool BUSENGINE_WINFORM = true;
		/** функция запуска приложения */
		//[System.STAThread] // если однопоточное приложение
		private static void Main(string[] args) {
			/** моя мечта
			if (WINXP) {
				System.Windows.Forms.Form _form = new System.Windows.Forms.Form();
				BusEngine.UI.Canvas(_form);
				Android.App.LoadApplication(_form);
			} else if (ANDROID) {
				Xamarin.Forms.Application _form = new Xamarin.Forms.Application();
				BusEngine.UI.Canvas(_form);
				Xamarin.Forms.LoadApplication(_form);
			} else {
				System.Windows.Application _form = new System.Windows.Application();
				BusEngine.UI.Canvas(_form);
				System.Windows.Application.Run(_form);
			}
			*/

			BusEngine.Engine.generateStatLink();

			//https://metanit.com/sharp/tutorial/5.4.php
			//https://metanit.com/sharp/tutorial/6.4.php
			//https://dir.by/developer/csharp/serialization_json/?lang=eng
			string _path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/../../Bin/";

			if (!System.IO.Directory.Exists(_path)) {
				_path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/../Bin/";

				if (!System.IO.Directory.Exists(_path)) {
					_path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/Bin/";
				}
			}

			_path = System.IO.Path.GetFullPath(_path + "../");

			string[] _files;

			_files = System.IO.Directory.GetFiles(_path, "*.busproject");

			if (_files.Length == 0) {
				BusEngine.Log.Info(_files.Length);

				// запись
				using (System.IO.FileStream fstream = System.IO.File.OpenWrite(_path + "Game.busproject")) {
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(new BusEngine.ProjectSettingDefault(), Newtonsoft.Json.Formatting.Indented));
					fstream.Write(buffer, 0, buffer.Length);
				}
			} else {
				BusEngine.Log.Info(_files[0]);

				// запись
				/* using (System.IO.StreamWriter fstream = new System.IO.StreamWriter(_files[0], false, System.Text.Encoding.UTF8)) {
					fstream.Write(Newtonsoft.Json.JsonConvert.SerializeObject(new BusEngine.ProjectSettingDefault(), Newtonsoft.Json.Formatting.Indented));
				} */

				// запись
				using (System.IO.FileStream fstream = System.IO.File.OpenWrite(_files[0])) {
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(new BusEngine.ProjectSettingDefault(), Newtonsoft.Json.Formatting.Indented));
					fstream.Write(buffer, 0, buffer.Length);
				}

				// запись
				/* using (System.IO.FileStream fstream = new System.IO.FileStream(_files[0], System.IO.FileMode.OpenOrCreate)) {
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(new BusEngine.ProjectSettingDefault(), Newtonsoft.Json.Formatting.Indented));
					fstream.WriteAsync(buffer, 0, buffer.Length);
				} */

				// запись
				//System.IO.File.WriteAllText(_files[0], Newtonsoft.Json.JsonConvert.SerializeObject(new BusEngine.ProjectSettingDefault(), Newtonsoft.Json.Formatting.Indented));

				// чтение
				/* using (System.IO.FileStream fstream = new System.IO.FileStream(_files[0], System.IO.FileMode.OpenOrCreate)) {
					byte[] buffer = new byte[fstream.Length];
					fstream.ReadAsync(buffer, 0, buffer.Length);
					// декодируем байты в строку
					Newtonsoft.Json.JsonConvert.DeserializeObject(System.Text.Encoding.UTF8.GetString(buffer));
				} */

				// чтение
				//Newtonsoft.Json.JsonConvert.DeserializeObject(System.IO.File.ReadAllText(_files[0]));
			}

			// тестирование плагина - прогонка кода
			Newtonsoft.Json.JsonConvert.DeserializeObject(Newtonsoft.Json.JsonConvert.SerializeObject(new BusEngine.ProjectSettingDefault(), Newtonsoft.Json.Formatting.Indented));

			_files = System.IO.Directory.GetFiles(_path, "busengine.busengine");

			if (_files.Length == 0) {
				BusEngine.Log.Info(_files.Length);

				using (System.IO.FileStream fstream = System.IO.File.OpenWrite(_path + "busengine.busengine")) {
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(new BusEngine.ProjectSettingDefault(), Newtonsoft.Json.Formatting.Indented));
					fstream.Write(buffer, 0, buffer.Length);
				}
			} else {
				BusEngine.Log.Info(_files[0]);

				using (System.IO.FileStream fstream = System.IO.File.OpenWrite(_files[0])) {
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(new BusEngine.ProjectSettingDefault(), Newtonsoft.Json.Formatting.Indented));
					fstream.Write(buffer, 0, buffer.Length);
				}
			}




			
			
			//Memory Manager: Unable to bind memory management functions. Cloud not access BusEngine.dll (check working directory);
			//Диспетчер памяти: невозможно связать функции управления памятью. Облако не имеет доступа к BusEngine.dll (проверьте рабочий каталог)

			

			System.Reflection.Assembly curAssembly = typeof(BusEngine.Engine).Assembly;
			//BusEngine.Log.Info("The current executing assembly is {0}.", curAssembly);

			System.Reflection.Module[] mods = curAssembly.GetModules();
			foreach (System.Reflection.Module md in mods) {
				//BusEngine.Log.Info("This assembly contains the Game.exe {0} module", md.Name);
			}


			//System.Type myType = System.Type.GetType("BusEngine.Game");
			//System.Reflection.MethodInfo myMethod = myType.GetMethod("MyMethod");
			BusEngine.Log.Info("dddddddddd");
			BusEngine.Log.Info(System.Reflection.BindingFlags.Public);
			BusEngine.Log.Info(System.Type.GetType("BusEngine.UI.Canvas.WinForm"));
			BusEngine.Log.Info("dddddddddd");
			BusEngine.Log.Info("xxxxxxxxxxx");
			BusEngine.Log.Info(typeof(System.IO.File).Assembly.FullName);
			BusEngine.Log.Info("xxxxxxxxxxx");
			
			
			
			
			//проверка https://learn.microsoft.com/ru-ru/dotnet/api/system.reflection.assembly.gettypes?view=net-7.0
			
			
			BusEngine.Log.Info("gggggggggggggg");
			System.Reflection.Assembly mainAssemblyd = typeof(BusEngine.Log).Assembly;
			System.IO.FileStream[] x = mainAssemblyd.GetFiles();
			foreach (System.IO.FileStream m in x) {
			BusEngine.Log.Info(m.Name);
			}
			
			
			System.Reflection.Assembly mainAssembly = typeof(BusEngine.Start).Assembly;
			BusEngine.Log.Info("The executing assembly is {0}.", mainAssembly);
			System.Reflection.Module[] modss = mainAssembly.GetModules();
			BusEngine.Log.Info("\tModules in the assembly:");
			foreach (System.Reflection.Module m in modss) {
				BusEngine.Log.Info("\t{0}", m);
			}
			BusEngine.Log.Info("gggggggggggggg");
			
			//"TestReflection" искомое пространство
			//System.Linq.Where x = System.Linq.Where(t => t.Namespace == "BusEngine.Game").ToArray();
			System.Type[] typelist = System.Reflection.Assembly.GetEntryAssembly().GetTypes();
			
			BusEngine.Log.Info(typelist);
			foreach (System.Type type in typelist) {
				BusEngine.Log.Info("ssssssssssss");
				BusEngine.Log.Info(type.FullName);
				BusEngine.Log.Info("ssssssssssss");
				//создание объекта
				//object targetObject = System.Activator.CreateInstance(System.Type.GetType(type.FullName));
 
				//что бы получить public методы без базовых(наследованных от object)
				var methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
				foreach (var methodInfo in methods) {
					BusEngine.Log.Info("ssssssssssss");
					BusEngine.Log.Info(methodInfo);
					BusEngine.Log.Info("ssssssssssss");
					//вызов
					//methodInfo.Invoke(targetObject, new object[] { });
				}
			}

			/* BusEngine.Plugin _plugin = new BusEngine.Game.Default();
			_plugin.Initialize(); */

			//BusEngine.Log.Debug();

			Form _form = new Form();
			//#if BUSENGINE
			BusEngine.UI.Canvas.WinForm = _form;
			BusEngine.UI.Canvas.Initialize();
			//if (typeof(BusEngine.UI.Canvas).GetField("WinForm") != null) {
				//_canvas.WinForm = _form;
			//}
			
			/* System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false); */

			// запускаем видео
			BusEngine.Video.Play("Videos/BusEngine.mp4");
			//BusEngine.Video.Play("Videos/BusEngine.mp4");
			//BusEngine.Video.Play("Videos/BusEngine.mp4");

			// запускаем браузер
			//CefSharp.BrowserSubprocess.SelfHost.Main(args);
			BusEngine.Browser.Start("index.html");
			System.Windows.Forms.Application.Run(_form);
			//#else
				
			//#endif
		}
		/** функция запуска приложения */
	}

	internal class Form : System.Windows.Forms.Form {
		/** функция запуска окна приложения */
		//#region Windows Form Designer generated code
		public Form() {
			// https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.forms.form?view=netframework-4.8
			//System.Windows.Forms.Form _form = new System.Windows.Forms.Form();
			// название окна
			//System.Windows.Forms.Form _canvas = System.Windows.Forms.Form();
			this.Text = "BusEngine v0.2.0";
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
			// устанавливаем события нажатий клавиш
			this.KeyPreview = true;
			//this.KeyDown += OnKeyDown;
			// устанавливаем событие закрытия окна
			//this.FormClosed += OnClosed;
			//this.Disposed += new System.EventHandler(OnDisposed);
			//ClientSize = this.ClientSize;
			System.Console.WriteLine(this.ClientSize);

			// показываем форму\включаем\запускаем\стартуем показ окна
			//this.ShowDialog();
		}

		//public static void Run(BusEngine.UI.WinForm winform = null) {
			//if (winform != null) {
				//_winform = winform;
			//}
		//}

		//#endregion

		private const int WM_ACTIVATEAPP = 0x001C;
		private bool appActive = true;

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
			if (appActive) {
				e.Graphics.FillRectangle(System.Drawing.SystemBrushes.ActiveCaption, 20, 20, 260, 50);
				e.Graphics.DrawString("Application is active", Font, System.Drawing.SystemBrushes.ActiveCaptionText, 20, 20);
			} else {
				e.Graphics.FillRectangle(System.Drawing.SystemBrushes.InactiveCaption, 20, 20, 260, 50);
				e.Graphics.DrawString("Application is Inactive", Font, System.Drawing.SystemBrushes.ActiveCaptionText, 20, 20);
			}
		}

		protected override void WndProc(ref System.Windows.Forms.Message m) {
			switch (m.Msg) {
				case WM_ACTIVATEAPP:
					appActive = (((int)m.WParam != 0));
					Invalidate();

					break;
			}
			base.WndProc(ref m);
		}

		/* protected override void Callback(System.IntPtr hWnd, System.Int32 msg, System.IntPtr wparam, System.IntPtr lparam) {
			switch (m.Msg) {
				case WM_ACTIVATEAPP:
					appActive = (((int)m.WParam != 0));
					Invalidate();

					break;
			}
			base.WndProc(ref m);
		} */
		/** функция запуска окна приложения */
	}
}
/** API BusEngine */