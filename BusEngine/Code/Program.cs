/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

/* C# 6.0+              https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history */
/* NET.Framework 4.5.1+ https://learn.microsoft.com/ru-ru/dotnet/framework/migration-guide/versions-and-dependencies */
/* MSBuild 12.0+        https://en.wikipedia.org/wiki/MSBuild#Versions */

/** Открытое API BusEngine */
namespace BusEngine {
	/** Открытое API BusEngine.Engine */
	public class Engine {
		public static BusEngine.UI.Canvas UI { get; set; }

		// UI движка
		private static BusEngine.UI.Canvas _canvas;

		/** функция запуска приложения */
		//[System.STAThread] // если однопоточное приложение
		private static void Main(string[] args) {
			_canvas = new BusEngine.UI.Canvas();
			BusEngine.Log.ConsoleShow();
			BusEngine.Log.Info(args);
			
			var version = System.Environment.Version;

			BusEngine.Log.Info("Тип: " + version.GetType());
			BusEngine.Log.Info("Моя версия .NET Framework: " + version.ToString());
			BusEngine.Log.Info("Значение переменной v: " + (System.Version)version.Clone());

			// https://highload.today/tipy-dannyh-c-sharp/
			// https://metanit.com/sharp/tutorial/2.1.php
			/** переменные разных типов в одно значение (не массив)*/
			// Строка
			string _string = "Строка"; // или System.String _string = "Строка";
			BusEngine.Log.Info(_string);
			// цифра (цело число)
			int _int = 10;
			BusEngine.Log.Info(_int);
			// цифра (цело число)
			long _long = 10;
			BusEngine.Log.Info(_long);
			// цифра с плавающей запятой (6 - 9 цифр после запятой)
			double _double = 3D;
			BusEngine.Log.Info(_double);
			_double = 4d;
			//_double = 3.934_001; // _ c C# 7.0+ https://learn.microsoft.com/ru-ru/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types
			// цифра с плавающей запятой (15 - 17 цифр после запятой)
			float _float = 3000.5F;
			BusEngine.Log.Info(_float);
			_float = 5.4f;
			// цифра с плавающей запятой (28 - 29 цифр после запятой)
			decimal _decimal = 3000.5m;
			BusEngine.Log.Info(_decimal);
			_decimal = 400.75M;


			/** переменные разных типов (массив)*/
			// массив строк
			string[] _array_string = {"Строка", "Строка.Строка", "Строка,Строка.Строка"};
			BusEngine.Log.Info(_array_string);
			// массив целых чисел
			int[] _array_int = {10, 10, 10};
			BusEngine.Log.Info(_array_int);
			// массив значений
			bool[] _array_bool = {false, true, true};
			BusEngine.Log.Info(_array_bool);




			object[] object1 = {"string", 23};
			BusEngine.Log.Info(object1);

			object object3 = new {a = "string", b = 23};

			BusEngine.Log.Info(object3);
			object object2 = new object();
			BusEngine.Log.Info(object2);
			object object4 = new object() {};
			BusEngine.Log.Info(object4);
			//object object4 = new {"string", "string"}; - error
			//object object5 = new {1, 2}; - error

			object object6 = new object();
			//object4.
			BusEngine.Log.Info(object6);

			try {
				// Get the Type object corresponding to MyClass.
				System.Type myType = typeof(BusEngine.UI.Canvas);
				// Get an array of nested type objects in MyClass.
				System.Type[] nestType = myType.GetNestedTypes();
				BusEngine.Log.Info("The number of nested types is {0}.", nestType.Length);
				foreach (System.Type t in nestType) {
					BusEngine.Log.Info("Nested type is {0}.", t.ToString());
				}
			} catch(System.Exception e) {
				BusEngine.Log.Info("Error"+e.Message);
			}


			System.Windows.Forms.Application.Run(_canvas);
		}
		/** функция запуска приложения */

		/** функция остановки приложения */
		public static void Shutdown() {
			System.Windows.Forms.Application.Exit();
		}
		/** функция остановки приложения */
	}
}

namespace BusEngine {
	/** Открытое API BusEngine.Log */
	public class Log : System.IDisposable {
		/** консоль */
		// статус консоли
		private static bool StatusConsole = false;

		// функция запуска консоли
		[System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool AllocConsole();

		// функция остановки консоли
		[System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool FreeConsole();

		// функция прикрепления консоли к запущенной программе по id процесса
		[System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool AttachConsole(int dwProcessId);

		// функция вывода массива в консоль
		/* public static void InfoArray(string[] args) {
			BusEngine.Log.Info("Command line = {0}", System.Environment.CommandLine);

			for (int i = 0; i < args.Length; ++i) {
				BusEngine.Log.Info("Argument{0} = {1}", i + 1, args[i]);
				BusEngine.Log.Info();
			}
		} */

		// функция вывода строки в консоль
		public static void Info() {
			System.Console.WriteLine();
			System.Console.WriteLine("ничего");
		}
		public static void Info(string args1) {
			System.Console.WriteLine(args1);
			System.Console.WriteLine("string");
		}
		public static void Info(string[] args1) {
			System.Console.WriteLine(args1.ToString());
			System.Console.WriteLine("string[]");
		}
		public static void Info(ulong args1) {
			System.Console.WriteLine(args1);
			System.Console.WriteLine("ulong");
		}
		public static void Info(uint args1) {
			System.Console.WriteLine(args1);
			System.Console.WriteLine("uint");
		}
		public static void Info(float args1) {
			System.Console.WriteLine(args1);
			System.Console.WriteLine("float");
		}
		public static void Info(decimal args1) {
			System.Console.WriteLine(args1);
			System.Console.WriteLine("decimal");
		}
		public static void Info(long args1) {
			System.Console.WriteLine(args1);
			System.Console.WriteLine("long");
		}
		public static void Info(int args1) {
			System.Console.WriteLine(args1);
			System.Console.WriteLine("int");
		}
		public static void Info(double args1) {
			System.Console.WriteLine(args1);
			System.Console.WriteLine("double");
		}
		public static void Info(char args1) {
			System.Console.WriteLine(args1);
			System.Console.WriteLine("char");
		}
		public static void Info(char[] args1) {
			System.Console.WriteLine(args1);
			System.Console.WriteLine("char[]");
		}
		public static void Info(bool args1) {
			System.Console.WriteLine(args1);
			System.Console.WriteLine("bool");
		}
		public static void Info(object args1) {
			System.Console.WriteLine(args1.ToString());
			System.Console.WriteLine("object");
		}
		public static void Info(object[] args1) {
			System.Console.WriteLine(args1.ToString());
			System.Console.WriteLine("object[]");
		}
		public static void Info(string args1, string args2) {
			System.Console.WriteLine(args1, args2);
			System.Console.WriteLine("string string");
		}
		public static void Info(string args1, object args2) {
			System.Console.WriteLine(args1, args2);
			System.Console.WriteLine("string object");
		}
		public static void Info(string args1, int args2) {
			System.Console.WriteLine(args1, args2);
			System.Console.WriteLine("string int");
		}
		public static void Info(string args1, long args2) {
			System.Console.WriteLine(args1, args2);
			System.Console.WriteLine("string long");
		}

/* 		public static void Info(params object[] args) {
			string args1 = "";
			dynamic args2 = null;

			if (args[0] != null && args[0].GetType() == typeof(string)) {
				args1 = System.Convert.ToString(args[0]);
			}

			if (args[1] != null && args[1].GetType() == typeof(int)) {
				args2 = System.Convert.ToInt32(args[1]);
			}

			if (args2 != null) {
				System.Console.WriteLine(args1, args2);
			} else {
				System.Console.WriteLine(args1);
			}
			foreach (object o in args) {
				//if (o.GetType() == typeof(int)) {
					//System.Console.WriteLine(o);
				//}
			}
		} */

		// функция запуска консоли
		public static void ConsoleShow() {
				if (BusEngine.Log.StatusConsole == false && !BusEngine.Log.AttachConsole(-1)) {
					BusEngine.Log.AllocConsole();
					BusEngine.Log.StatusConsole = true;
				}
		}

		// функция остановки консоли
		public static void ConsoleHide() {
				if (BusEngine.Log.StatusConsole == true) {
					BusEngine.Log.FreeConsole();
					BusEngine.Log.StatusConsole = false;
				}
		}

		// функция запуска\остановки консоли
		public static void ConsoleToggle() {
				if (BusEngine.Log.StatusConsole == false && !BusEngine.Log.AttachConsole(-1)) {
					BusEngine.Log.AllocConsole();
					BusEngine.Log.StatusConsole = true;
				} else {
					BusEngine.Log.FreeConsole();
					BusEngine.Log.StatusConsole = false;
				}
		}
		/** консоль */

		public static void Shutdown() {

		}

		public void Dispose() {

		}
	}
}

namespace BusEngine.UI {
	/** Открытое API BusEngine.UI.Canvas */
	public class Canvas : System.Windows.Forms.Form {
		/** видео */
		private LibVLCSharp.Shared.LibVLC _VLC;
		private LibVLCSharp.Shared.MediaPlayer _VLC_MP;
		private LibVLCSharp.WinForms.VideoView _VLC_VideoView;
		/** видео */

		/** событие нажатия любой кнопки */
		// https://learn.microsoft.com/en-us/dotnet/api/system.consolekey?view=netframework-4.8
		private void OnKeyDown(object o, System.Windows.Forms.KeyEventArgs e) {
			BusEngine.Log.Info("клавиатура клик");
			BusEngine.Log.Info();
			// Выключаем движок по нажатию на Esc
			if (e.KeyCode == System.Windows.Forms.Keys.Escape) {
				this.KeyDown -= new System.Windows.Forms.KeyEventHandler(OnKeyDown);
				this.Dispose();
				BusEngine.Engine.Shutdown();
			}
			// Вкл\Выкл консоль движка по нажатию на ~
			if (e.KeyCode == System.Windows.Forms.Keys.Oem3) {
				BusEngine.Log.ConsoleToggle();
			}
		}
		/** событие нажатия любой кнопки */
		
		public void Bgds() {

		}

		private void OnKeyDown2(object o, System.Windows.Forms.KeyEventArgs e) {
			//this.KeyDown -= new System.Windows.Forms.KeyEventHandler(OnKeyDown2);
			this.Dispose();
			BusEngine.Engine.Shutdown();
		}

		/** событие остановки видео */
		private void onVideoStop(object o, object e) {
			BusEngine.Log.Info("Видео остановить");
			this.Controls.Remove(_VLC_VideoView);
			this._VLC_MP.Dispose();
			this._VLC.Dispose();
			this.KeyPreview = true;
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(OnKeyDown);
		}
		/** событие остановки видео */

		/** событие клика из браузера */
		private void onBrowserClick(object o, object e) {
			BusEngine.Log.Info("браузер клик");
		}
		/** событие клика из браузера */

		/** функция запуска видео */
		private void Video(string url = "") {
			// https://www.microsoft.com/en-us/download/details.aspx?id=6812
			// https://www.nuget.org/packages/Microsoft.DXSDK.D3DX#readme-body-tab
			// https://learn.microsoft.com/en-us/previous-versions/windows/desktop/bb318762(v=vs.85)
			//Microsoft.DirectX.AudioVideoPlayback.Audio _mediaPlayer = new Microsoft.DirectX.AudioVideoPlayback.Audio("E:\\Music\\main.mp3");
			/* Microsoft.DirectX.AudioVideoPlayback.Video _mediaPlayer = new Microsoft.DirectX.AudioVideoPlayback.Video(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/../../Data/Videos/BusEngine.mp4");
			_mediaPlayer.Owner = _form;
			_mediaPlayer.Play(); */

			// https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.media.mediaplayer?view=windowsdesktop-7.0
			/* System.Windows.Media.MediaPlayer _mediaPlayer = new System.Windows.Media.MediaPlayer();
			_mediaPlayer.Open(new System.Uri(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/../../Data/Videos/BusEngine.mp4"));
			_mediaPlayer.Play(); */

			// https://www.nuget.org/packages/WMPLib#supportedframeworks-body-tab
			// https://learn.microsoft.com/ru-ru/windows/win32/wmp/creating-the-windows-media-player-control-programmatically?redirectedfrom=MSDN
			// C:\Windows\System32\wmp.dll x86
			// C:\Windows\SysWOW64\wmp.dll x64
			/* WMPLib.WindowsMediaPlayer _mediaPlayer = new WMPLib.WindowsMediaPlayer();
			_mediaPlayer.URL = "H:/CRYENGINE Projects/BusEngine/Data/Videos/BusEngine.mp4";
			_mediaPlayer.controls.play(); */

			// https://www.nuget.org/packages/WMPLib#supportedframeworks-body-tab
			// https://learn.microsoft.com/ru-ru/windows/win32/wmp/axwindowsmediaplayer-object--vb-and-c
			// https://learn.microsoft.com/en-us/previous-versions/aa472935(v=vs.85)
			/* AxWMPLib.AxWindowsMediaPlayer _mediaPlayer = new AxWMPLib.AxWindowsMediaPlayer();
			_mediaPlayer.URL = "H:/CRYENGINE Projects/BusEngine/Data/Videos/BusEngine.mp4";
			_mediaPlayer.Ctlcontrols.play();
			this.Controls.Add(_mediaPlayer); */

			// https://code.videolan.org/videolan/LibVLCSharp/-/blob/master/samples/LibVLCSharp.WinForms.Sample/Form1.cs
			// https://github.com/videolan/libvlcsharp#quick-api-overview
			this._VLC = new LibVLCSharp.Shared.LibVLC();
			this._VLC_MP = new LibVLCSharp.Shared.MediaPlayer(new LibVLCSharp.Shared.Media(_VLC, System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/../../Data/Videos/BusEngine.mp4"));
			this._VLC_VideoView = new LibVLCSharp.WinForms.VideoView();
			this._VLC_VideoView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this._VLC_VideoView.BackColor = System.Drawing.Color.Black;
			this._VLC_VideoView.Size = this.ClientSize;
			//this._VLC_VideoView.TabIndex = 1;
			//this._VLC_VideoView.MediaPlayer = null;
			//this._VLC_VideoView.Name = "VideoView";
			//this._VLC_VideoView.Text = "VideoView";
			//this._VLC_VideoView.Location = new System.Drawing.Point(0, 27);
			//this._VLC_VideoView.Size = new System.Drawing.Size(800, 444);
			this._VLC_VideoView.MediaPlayer = _VLC_MP;
			this._VLC_VideoView.MediaPlayer.EnableKeyInput = true;
			// установить массив функций в дополнительных библиотеках
			this._VLC_VideoView.MediaPlayer.Stopped += onVideoStop;
			//this._VLC_VideoView.MediaPlayer.Stop += videoStop;
			this._VLC_VideoView.MediaPlayer.Play();

			this.Controls.Add(_VLC_VideoView);
		}
		/** функция запуска видео */

		/** функция запуска браузера */
		private void Browser() {
			// выводим браузер на экран
			// https://cefsharp.github.io/api/
			CefSharp.WinForms.CefSettings settings = new CefSharp.WinForms.CefSettings();
			//settings.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) BusEngine/0.2.0 Safari/537.36";
			settings.UserAgent = settings.UserAgent.Replace("Chrome", "BusEngine");
			settings.LogSeverity = CefSharp.LogSeverity.Disable;
			CefSharp.Cef.Initialize(settings);
			CefSharp.WinForms.ChromiumWebBrowser _browser = new CefSharp.WinForms.ChromiumWebBrowser(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/../../Data/index.html");
			_browser.KeyDown += OnKeyDown2;
			//_browser.Dock = System.Windows.Forms.DockStyle.Fill;
			this.Controls.Add(_browser);

			//var _event = CefSharp.DevTools.DOM.DOMClient();

			/* CefSharp.DevTools.DOM.DOMClient _event = new CefSharp.DevTools.DOM.DOMClient();
			_event.DocumentUpdated += onBrowserClick; */



			/* System.Windows.Forms.WebBrowser _browser = new System.Windows.Forms.WebBrowser();
			_browser.Size = this.ClientSize;
			_browser.Dock = System.Windows.Forms.DockStyle.Fill;
			_browser.Navigate(new System.Uri(@"https://www.cryengine.com/"));
			this.Controls.Add(_browser); */

			/* Microsoft.Web.WebView2WebView2 _browser = new Microsoft.Web.WebView2WebView2();
			_browser.Navigate(new System.Uri("https://www.cryengine.com/")); */
		}
		/** функция запуска браузера */

		/** функция запуска окна приложения */
		public Canvas() {
			// https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.forms.form?view=netframework-4.8
			//System.Windows.Forms.Form _form = new System.Windows.Forms.Form();
			// название окна
			this.Text = "Моя гульня!";
			// устанавливаем нашу иконку, есди она есть по пути exe, в противном случае устанавливаем системную
			if (System.IO.File.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/../../Data/Icons/BusEngine.ico")) {
				this.Icon = new System.Drawing.Icon(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/../../Data/Icons/BusEngine.ico", 128, 128);
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
			this.KeyDown += OnKeyDown;

			// запускаем видео
			Video();
			// запускаем браузер
			Browser();

			// показываем форму\включаем\запускаем\стартуем показ окна
			//this.ShowDialog();
		}
		/** функция запуска окна приложения */
	}
}