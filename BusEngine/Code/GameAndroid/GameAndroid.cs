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

	internal class Start {
		//public const bool BUSENGINE_WINFORM = true;
		/** функция запуска приложения */
		//[System.STAThread] // если однопоточное приложение
		private static void Main(string[] args) {
			//Memory Manager: Unable to bind memory management functions. Cloud not access BusEngine.dll (check working directory);
			//Диспетчер памяти: невозможно связать функции управления памятью. Облако не имеет доступа к BusEngine.dll (проверьте рабочий каталог)

			BusEngine.Engine.generateStatLink();

			//System.Type myType = System.Type.GetType("BusEngine.Game");
			//System.Reflection.MethodInfo myMethod = myType.GetMethod("MyMethod");
			//System.Type myType = System.Type.GetType("LibVLCSharp.Shared.Media");
			//BusEngine.Log.Info(myType);

			//BusEngine.Log.Info(typeof(System.IO.File).Assembly.FullName);
			//"TestReflection" искомое пространство
			//System.Linq.Where x = System.Linq.Where(t => t.Namespace == "BusEngine.Game").ToArray();
			System.Type[] typelist = System.Reflection.Assembly.GetExecutingAssembly().GetTypes();
			foreach (System.Type type in typelist) {
				BusEngine.Log.Info(type.FullName);
				//создание объекта
				//object targetObject = System.Activator.CreateInstance(System.Type.GetType(type.FullName));
 
				//что бы получить public методы без базовых(наследованных от object)
				/* var methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
				foreach (var methodInfo in methods) {
					BusEngine.Log.Info(methodInfo);
					//вызов
					//methodInfo.Invoke(targetObject, new object[] { });
				} */
			}

			/* BusEngine.Plugin _plugin = new BusEngine.Game.Default();
			_plugin.Initialize(); */

			//BusEngine.Log.Debug();

			Form _form = new Form();
			#if BUSENGINE
			BusEngine.UI.Canvas.WinForm = _form;
			BusEngine.UI.Canvas.Initialize();
			//if (typeof(BusEngine.UI.Canvas).GetField("WinForm") != null) {
				//_canvas.WinForm = _form;
			//}
			#else
				
			#endif

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
			this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			// убираем линии, чтобы окно было полностью на весь экран
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
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