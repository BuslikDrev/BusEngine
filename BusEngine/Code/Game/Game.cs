/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

/* C# 6.0+              https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history */
/* NET.Framework 4.6.1+ https://learn.microsoft.com/ru-ru/dotnet/framework/migration-guide/versions-and-dependencies */
/* MSBuild 14.0+        https://en.wikipedia.org/wiki/MSBuild#Versions */

/** дорожная карта
- написать запуск игры BusEngine
*/

#define BUSENGINE_WINFORM
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.ProjectSettingDefault
BusEngine.Engine
BusEngine.Log
BusEngine.UI
*/
	//https://learn.microsoft.com/ru-ru/dotnet/csharp/language-reference/preprocessor-directives
	//https://learn.microsoft.com/ru-ru/dotnet/csharp/programming-guide/classes-and-structs/constants
	public class Global {
		//public const bool BUSENGINE_WINFORM = true;
	}

	internal class Start {
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

			// генерируем BusEngine API
			BusEngine.Engine.GenerateStatLink();

			BusEngine.Engine.Platform = "BUSENGINE_WINFORM";



			
			
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






			BusEngine.Log.Info("============== ajax запустили");
			BusEngine.Tools.Ajax.Test("https://buslikdrev.by/");
			BusEngine.Log.Info("============== ajax запустили");

			// создаём форму System.Windows.Forms
			Form _form = new Form();

			// покдлючаем  BusEngine API
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
			BusEngine.Browser.Start("index.html");
			

			// запускаем приложение System.Windows.Forms
			System.Windows.Forms.Application.Run(_form);
		}
		/** функция запуска приложения */

		/* private static void OnPostMessage(object sender, CefSharp.JavascriptMessageReceivedEventArgs e) {
			BusEngine.Log.Info("браузер клик");
			string windowSelection = (string)e.Message;
			if (windowSelection == "Log") {
				BusEngine.Log.Info("============== Log");
			}
		} */
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

		//public static void Run(BusEngine.UI.WinForm winform = null) {
			//if (winform != null) {
				//_winform = winform;
			//}
		//}

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