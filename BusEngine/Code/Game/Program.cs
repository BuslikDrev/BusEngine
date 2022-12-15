/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

/* C# 6.0+              https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history */
/* NET.Framework 4.5.1+ https://learn.microsoft.com/ru-ru/dotnet/framework/migration-guide/versions-and-dependencies */
/* MSBuild 12.0+        https://en.wikipedia.org/wiki/MSBuild#Versions */

/** задание
проставить нормально модификаторы доступа https://metanit.com/sharp/tutorial/3.2.php
максимально весь функционал сделать независимыми плагинами и установить проверки на наличие плагинов перед их использованием
апи перенести в проект библиотеки, а текущий, только для запуска и остановки программы, с возможностью отказа от использования API BusEngine.
*/

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.Log
*/
	/** API BusEngine.Core */
	//https://habr.com/ru/post/196578/
	public class Core {

	}
	/** API BusEngine.Core */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.Core
BusEngine.Log
BusEngine.Plugin
BusEngine.Engine.UI
*/
	/** API BusEngine.Engine */
	public class Engine {
		/** UI движка */
		//public static BusEngine.UI.Canvas UI { get; set; }
		public static string DataDirectory;
		//private static BusEngine.UI.Canvas _canvas;
		// MSBuild v12.0
		private static void generateStatLink() {
			BusEngine.Log.ConsoleShow();
			DataDirectory = System.IO.Path.GetFullPath(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/../../Data/");

			System.Reflection.Assembly curAssembly = typeof(BusEngine.Engine).Assembly;
			BusEngine.Log.Info("The current executing assembly is {0}.", curAssembly);

			System.Reflection.Module[] mods = curAssembly.GetModules();
			foreach (System.Reflection.Module md in mods) {
				BusEngine.Log.Info("This assembly contains the {0} module", md.Name);
			}

			//BusEngine.Log.Info("FirstMethod called from: " + System.Reflection.Module);
			BusEngine.Log.Info("FirstMethod called from: " + System.Reflection.Assembly.GetCallingAssembly().FullName);
		}
		/** UI движка */
		
		//public virtual System.Collections.Generic.IEnumerable<System.Reflection.Module> Modules { get; }

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

			BusEngine.UI.WinForm _form = new BusEngine.UI.WinForm();

			/* System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false); */
			BusEngine.UI.Canvas.Run(winform: _form);
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

		/** функция остановки приложения */
		public static void Shutdown() {
			BusEngine.Plugin _plugin = new BusEngine.Game.Default();
			_plugin.Shutdown();
			BusEngine.Log.ConsoleHide();
			System.Windows.Forms.Application.Exit();
		}
		/** функция остановки приложения */
	}
	/** API BusEngine.Engine */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
Зависимости нет
*/
	/** API BusEngine.Log */
	public class Log : System.IDisposable {
		/** консоль - на первое время берём консоль System.Console.WriteLine и переопределяем на BusEngine.Log.Info */
		// статус консоли
		private static bool StatusConsole = false;

		// функция запуска консоли
		[System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "AllocConsole", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto, CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]
		private static extern bool AllocConsole();

		// функция остановки консоли
		[System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool FreeConsole();

		// функция прикрепления консоли к запущенной программе по id процесса
		[System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool AttachConsole(int dwProcessId);

		// функция получения заголовков из консоли, чтобы можно было записать в файл
		[System.Runtime.InteropServices.DllImport("kernel32.dll", EntryPoint = "GetStdHandle", SetLastError = true, CharSet = System.Runtime.InteropServices.CharSet.Auto, CallingConvention = System.Runtime.InteropServices.CallingConvention.StdCall)]  
		private static extern System.IntPtr GetStdHandle(int nStdHandle);

		// функция отправки заголовков в консоль, чтобы можно было записать из файла
		[System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
		private static extern void SetStdHandle(uint nStdHandle, System.IntPtr handle);

		// функция запуска консоли
		public static void ConsoleShow() {
			if (BusEngine.Log.StatusConsole == false) {
				/* System.Windows.Forms.MessageBox.Show("Сообщение из Windows Forms!"); */

				BusEngine.Log.AttachConsole(-1);
				BusEngine.Log.AllocConsole();
				BusEngine.Log.StatusConsole = true;

				System.Console.CancelKeyPress += new System.ConsoleCancelEventHandler(BusEngine.Log.myHandler);

				//System.Console.Clear();
				BusEngine.Log.Info("Консоль BusEngine");
				while (true) {
					break;
					//if (System.Console.ReadKey(true).Key == System.ConsoleKey.Enter) {
						//string command = System.Console.ReadLine();
						//BusEngine.Log.Info($"Вы ввели команду: {command}");
					//}
					/* if (command == "start" || System.Console.ReadKey().Key == System.ConsoleKey.Oem3) {
						BusEngine.Log.ConsoleHide();
						break;
					} else {
						
					} */
				}

				/* System.Console.In.ReadLine(); */
				/* System.Console.Read(); */
				/* System.Console.ReadKey(); */
			}
		}

		// функция остановки консоли
		public static void ConsoleHide() {
			if (BusEngine.Log.StatusConsole == true) {
				//BusEngine.Log.Info(new System.IO.StreamWriter(System.Console.OpenStandardOutput(), System.Console.OutputEncoding) { AutoFlush = true });
				//BusEngine.Log.Info(new System.IO.StreamReader(System.Console.OpenStandardInput(), System.Console.InputEncoding));
				BusEngine.Log.FreeConsole();
				BusEngine.Log.StatusConsole = false;
				System.Console.SetOut(new System.IO.StreamWriter(System.Console.OpenStandardOutput(), System.Console.OutputEncoding) { AutoFlush = true });
				System.Console.SetError(new System.IO.StreamWriter(System.Console.OpenStandardError(), System.Console.OutputEncoding) { AutoFlush = true });
				System.Console.SetIn(new System.IO.StreamReader(System.Console.OpenStandardInput(), System.Console.InputEncoding));
			}
		}

		// функция запуска\остановки консоли
		public static void ConsoleToggle() {
			if (BusEngine.Log.StatusConsole == false) {
				BusEngine.Log.ConsoleShow();
			} else {
				BusEngine.Log.ConsoleHide();
			}
		}

		protected static void myHandler(object sender, System.ConsoleCancelEventArgs args) {
			/* System.Console.WriteLine("\nThe read operation has been interrupted.");

			System.Console.WriteLine($"  Key pressed: {args.SpecialKey}");

			System.Console.WriteLine($"  Cancel property: {args.Cancel}");

			// Set the Cancel property to true to prevent the process from terminating.
			System.Console.WriteLine("Setting the Cancel property to true...");
			args.Cancel = true;

			// Announce the new value of the Cancel property.
			System.Console.WriteLine($"  Cancel property: {args.Cancel}");
			System.Console.WriteLine("The read operation will resume...\n"); */
		}

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
		public static void Info(System.Type args1) {
			System.Console.WriteLine(args1);
			System.Console.WriteLine("System.Type");
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

		public static void Debug() {
			//System.Type myType = System.Type.GetType("System.Windows.Forms.MyMethod");
			//System.Reflection.MethodInfo myMethod = myType.GetMethod("MyMethod");
			System.Type myType = System.Type.GetType("LibVLCSharp.Shared.Media");
			BusEngine.Log.Info(myType);

			var version = System.Environment.Version;

			BusEngine.Log.Info("Тип: " + version.GetType());
			BusEngine.Log.Info("Моя версия .NET Framework: " + version.ToString());
			BusEngine.Log.Info("Значение переменной v: " + (System.Version)version.Clone());

			// https://highload.today/tipy-dannyh-c-sharp/
			// https://metanit.com/sharp/tutorial/2.1.php
			/** переменные разных типов в одно значение (не массив) */
			// строка
			string _string = "Строка"; // или System.String _string = "Строка";
			BusEngine.Log.Info(_string);

			// цифра (цело число)
			int _int = 10;
			BusEngine.Log.Info(_int);

			// цифра (цело число)
			long _long = 10;
			BusEngine.Log.Info(_long);

			// цифра с плавающей запятой (точкой) (6 - 9 цифр после запятой)
			double _double = 10.0D;
			_double = 10.0d;
			//_double = 3.934_001; // "_" c C# 7.0+ https://learn.microsoft.com/ru-ru/dotnet/csharp/language-reference/builtin-types/floating-point-numeric-types
			BusEngine.Log.Info(_double);

			// цифра с плавающей запятой (точкой) (15 - 17 цифр после запятой)
			float _float = 10.0F;
			_float = 10.0f;
			BusEngine.Log.Info(_float);

			// цифра с плавающей запятой (точкой) (28 - 29 цифр после запятой)
			decimal _decimal = 10.0m;
			_decimal = 10.0M;
			BusEngine.Log.Info(_decimal);

			// цифра с плавающей запятой (точкой) (28 - 29 цифр после запятой)
			bool _bool = false;
			_bool = true;
			BusEngine.Log.Info(_bool);
			/** переменные разных типов в одно значение (не массив) */

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
			/** переменные разных типов (массив) */

			/* try {
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
			} */
		}

		public static void Shutdown() {

		}

		public void Dispose() {
			BusEngine.Log.ConsoleHide();
		}
		/** консоль */
	}
	/** API BusEngine.Log */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.UI.Canvas
BusEngine.Video
*/
	using Audio = BusEngine.Video;
	/** API BusEngine.Audio */
	/* public class Audio : System.IDisposable {
		
	} */
	/** API BusEngine.Audio */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/* 
Зависит от плагинов:
BusEngine.UI.WinForm.GetForm
*/
	/** API BusEngine.Video */
	public class Video : System.IDisposable {
		/** видео */
		public Video(string Url = "") {
			/* System.Type myType = System.Type.GetType("MyNamespace.MyClass");
			System.Reflection.MethodInfo myMethod = myType.GetMethod("MyMethod");

			if (_form == null) {
				if (System.Type.GetType("MyNamespace.MyClass") != null) {
					
				}
				//_form = new System.Windows.Forms.Form();
			} */
		}
		private static Video _video;

		private static LibVLCSharp.Shared.LibVLC _VLC;
		private static LibVLCSharp.Shared.MediaPlayer _VLC_MP;
		private static LibVLCSharp.WinForms.VideoView _VLC_VideoView;
		/** видео */

		/** событие остановки видео */
		public static void onVideoStop(object o, object e) {
			BusEngine.Log.Info("Видео остановилось onVideoStop");
			BusEngine.Video.Shutdown();
		}
		/** событие остановки видео */

		/** функция запуска видео */
		private void VideoForm(string Url = "") {
			// https://www.microsoft.com/en-us/download/details.aspx?id=6812
			// https://www.nuget.org/packages/Microsoft.DXSDK.D3DX#readme-body-tab
			// https://learn.microsoft.com/en-us/previous-versions/windows/desktop/bb318762(v=vs.85)
			//Microsoft.DirectX.AudioVideoPlayback.Audio _mediaPlayer = new Microsoft.DirectX.AudioVideoPlayback.Audio("E:\\Music\\main.mp3");
			/* Microsoft.DirectX.AudioVideoPlayback.Video _mediaPlayer = new Microsoft.DirectX.AudioVideoPlayback.Video(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/../../Data/Videos/BusEngine.mp4");
			_mediaPlayer.Owner = _winform.GetForm;
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
			BusEngine.UI.Canvas.WinForm().ControlsAdd(_mediaPlayer); */

			// https://code.videolan.org/videolan/LibVLCSharp/-/blob/master/samples/LibVLCSharp.WinForms.Sample/Form1.cs
			// https://github.com/videolan/libvlcsharp#quick-api-overview
			_VLC = new LibVLCSharp.Shared.LibVLC();
			LibVLCSharp.Shared.Media media = new LibVLCSharp.Shared.Media(_VLC, Url);
			_VLC_MP = new LibVLCSharp.Shared.MediaPlayer(media);
			media.Dispose();
			_VLC_VideoView = new LibVLCSharp.WinForms.VideoView();
			_VLC_VideoView.MediaPlayer = _VLC_MP;
			((System.ComponentModel.ISupportInitialize)(_VLC_VideoView)).BeginInit();
			//SuspendLayout();
			_VLC_VideoView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			_VLC_VideoView.BackColor = System.Drawing.Color.Black;
			//_VLC_VideoView.TabIndex = 1;
			//_VLC_VideoView.MediaPlayer = null;
			//_VLC_VideoView.Name = "VideoView";
			//_VLC_VideoView.Text = "VideoView";
			//_VLC_VideoView.Location = new System.Drawing.Point(0, 27);
			//_VLC_VideoView.Size = new System.Drawing.Size(800, 444);
			_VLC_VideoView.MediaPlayer.EnableKeyInput = true;
			// установить массив функций в дополнительных библиотеках
			_VLC_VideoView.MediaPlayer.Stopped += BusEngine.Video.onVideoStop;
			//_VLC_VideoView.MediaPlayer.Stop += videoStop;
			//BusEngine.UI.WinForm _winform = BusEngine.UI.WinForm.GetForm();
			_VLC_VideoView.Size = BusEngine.UI.WinForm.GetForm.ClientSize;
			BusEngine.UI.WinForm.GetForm.Controls.Add(_VLC_VideoView);
			_VLC_VideoView.MediaPlayer.Play();
		}

		public static Video Play(string Url = "") {
			BusEngine.Log.Info(Url);
			if (Url.IndexOf(':') == -1) {
				Url = System.IO.Path.Combine(BusEngine.Engine.DataDirectory, Url);
			}

			if (System.IO.File.Exists(Url)) {
				BusEngine.Log.Info(Url);
				if (_video == null) {
					_video = new Video();
				}
				_video.VideoForm(Url);
				//_video.VideoWpf(Url);
			}

			return _video;
		}
		/** функция запуска видео */

		public static void Shutdown() {
			if (_video != null) {
				//BusEngine.UI.WinForm _winform = BusEngine.UI.WinForm.GetForm();
				BusEngine.UI.WinForm.GetForm.Controls.Remove(_VLC_VideoView);
				_video.Dispose();
				_video = null;
				BusEngine.Log.Info("Видео остановилось Shutdown");
			}
		}

		public void Dispose() {
			//BusEngine.Log.ConsoleHide();
			if (_VLC_VideoView != null) {
				_VLC_VideoView.MediaPlayer.Dispose();
				_VLC_VideoView = null;
			}
			if (_VLC_MP != null) {
				_VLC_MP.Dispose();
			}
			if (_VLC != null) {
				_VLC.Dispose();
			}
			BusEngine.Log.Info("Видео остановилось Dispose");
		}
	}
	/** API BusEngine.Video */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.UI.WinForm.GetForm
*/
	/** API BusEngine.Browser */
	public class Browser : System.IDisposable {
		//private Browser() {}
		//private static Browser _browser;

		/** событие клика из браузера */
		private static void onBrowserClick(object o, object e) {
			BusEngine.Log.Info("браузер клик тест 1");
		}

		private static void OnKeyDown(object o, System.Windows.Forms.KeyEventArgs e) {
			//KeyDown -= new System.Windows.Forms.KeyEventHandler(OnKeyDown);
			//BusEngine.Browser.Dispose();
			BusEngine.Log.Info("браузер клик тест 2");
			BusEngine.Engine.Shutdown();
		}
		/** событие клика из браузера */

		/** все события из js браузера */
		private static void OnPostMessage(object sender, CefSharp.JavascriptMessageReceivedEventArgs e) {
			BusEngine.Log.Info("браузер клик");
			string windowSelection = (string)e.Message;
			if (windowSelection == "Exit") {
				BusEngine.Engine.Shutdown();
			}
		}
		/** все события из js браузера */

		/** функция запуска браузера */
		public static void Start(string Url = "") {
			if (Url.IndexOf(':') == -1) {
				Url = System.IO.Path.Combine(BusEngine.Engine.DataDirectory, Url);
			}

			if (!System.IO.File.Exists(Url)) {
				Url = "https://buslikdrev.by/";
			}

			// выводим браузер на экран
			// https://cefsharp.github.io/api/
			//CefSharp.CefSettingsBase.UserAgent;
			
			CefSharp.WinForms.CefSettings settings = new CefSharp.WinForms.CefSettings();
			settings.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) BusEngine/0.2.0 Safari/537.36";
			//settings.UserAgent = settings.UserAgent.Replace("Chrome", "BusEngine");
			settings.LogSeverity = CefSharp.LogSeverity.Disable;
			CefSharp.Cef.EnableHighDPISupport();
			CefSharp.Cef.Initialize(settings);
			CefSharp.WinForms.ChromiumWebBrowser _browser = new CefSharp.WinForms.ChromiumWebBrowser(Url);
			_browser.KeyDown += BusEngine.Browser.OnKeyDown;
			// https://stackoverflow.com/questions/51259813/call-c-sharp-function-from-javascript-using-cefsharp-in-windows-form-app
			_browser.JavascriptMessageReceived += OnPostMessage;

			//BusEngine.UI.WinForm _winform = BusEngine.UI.WinForm.GetForm();
			//_browser.Size = _winform.ClientSize;
			//_browser.Dock = _winform.Dock;
			BusEngine.UI.WinForm.GetForm.Controls.Add(_browser);

			//var _event = CefSharp.DevTools.DOM.DOMClient();

			/* CefSharp.DevTools.DOM.DOMClient _event = new CefSharp.DevTools.DOM.DOMClient();
			_event.DocumentUpdated += onBrowserClick; */

			/* Microsoft.Web.WebView2WebView2 _browser = new Microsoft.Web.WebView2WebView2();
			_browser.Navigate(new System.Uri("https://buslikdrev.by/")); */
		}
		/** функция запуска браузера */

		public static void Shutdown() {

		}

		public void Dispose() {
			BusEngine.Log.ConsoleHide();
		}
	}
	/** API BusEngine.Browser */
}
/** API BusEngine */

/** API BusEngine.UI */
namespace BusEngine.UI {
/*
Зависит от плагинов:
BusEngine.UI
*/
	/** API BusEngine.UI.Canvas */
	public class Canvas : System.IDisposable {
		public static BusEngine.UI.WinForm _winform;
		public static BusEngine.UI.WinForm WinForm() {
			return _winform;
		}

		public Canvas() {

		}

		public static void Run(BusEngine.UI.WinForm winform = null) {
			if (winform != null) {
				_winform = winform;
			}
		}

		public static void Shutdown() {

		}

		public void Dispose() {
			//BusEngine.Log.ConsoleHide();
		}
	}
}
/** API BusEngine.UI */

/** API BusEngine.UI */
namespace BusEngine.UI {
/* 
Зависит от плагинов:
BusEngine.Video
Зависимости нет
*/
	/** API BusEngine.UI.WinForm */
	public class WinForm : System.Windows.Forms.Form {
		public static BusEngine.UI.WinForm GetForm;
		//public static WinForm;
		//public delegate void Form(BusEngine.UI.WinForm form);

		/** событие нажатия любой кнопки */
		// https://learn.microsoft.com/en-us/dotnet/api/system.consolekey?view=netframework-4.8
		private void OnKeyDown(object o, System.Windows.Forms.KeyEventArgs e) {
			BusEngine.Log.Info("клавиатура клик");
			BusEngine.Log.Info();
			// Выключаем движок по нажатию на Esc
			if (e.KeyCode == System.Windows.Forms.Keys.Escape) {
				KeyDown -= new System.Windows.Forms.KeyEventHandler(OnKeyDown);
				//Dispose();
				BusEngine.Engine.Shutdown();
			}
			// Вкл\Выкл консоль движка по нажатию на ~
			if (e.KeyCode == System.Windows.Forms.Keys.Oem3) {
				BusEngine.Log.ConsoleToggle();
				System.Console.WriteLine("Консоль BusEngine");
			}
			// Выкл Видео
			if (e.KeyCode == System.Windows.Forms.Keys.Space) {
				BusEngine.Video.Shutdown();
			}
		}
		/** событие нажатия любой кнопки */

		/** событие закрытия окна */
		private void OnClosed(object o, System.Windows.Forms.FormClosedEventArgs e) {
			this.FormClosed -= OnClosed;
			BusEngine.Video.Shutdown();
			BusEngine.Engine.Shutdown();
		}
		/** событие закрытия окна */

		/** событие уничтожения окна */
		private void OnDisposed(object o, System.EventArgs e) {

		}
		/** событие уничтожения окна */

		/** функция запуска окна приложения */
		//#region Windows Form Designer generated code
		public WinForm() {
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
			this.KeyDown += OnKeyDown;
			// устанавливаем событие закрытия окна
			this.FormClosed += OnClosed;
			this.Disposed += new System.EventHandler(OnDisposed);
			//ClientSize = this.ClientSize;
			System.Console.WriteLine(this.ClientSize);
			GetForm = this;

			// показываем форму\включаем\запускаем\стартуем показ окна
			//this.ShowDialog();
		}

		//#endregion

		/* private static System.Drawing.Size ClientSize;
		public static System.Drawing.Size ClientSize() {
			return ClientSize;
		} */

		/* public static BusEngine.UI.WinForm GetForm() {
			return _GetForm;
		} */

		public void ControlsAdd(System.Windows.Forms.Form e) {
			this.Controls.Add(e);
		}

		public void ControlsRemove(System.Windows.Forms.Form e) {
			this.Controls.Remove(e);
		}

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
	/** API BusEngine.UI.Canvas */
}
/** API BusEngine.UI */

/** API BusEngine */
namespace BusEngine {
	/** API BusEngine.Plugin */
	public abstract class Plugin {
		// при заапуске BusEngine до создания формы
		public virtual void Initialize() { }

		// после загрузки определённого плагина
		public virtual void Initialize(string plugin) { }

		// перед закрытием BusEngine
		public virtual void Shutdown() { }

		// перед загрузкой игрового уровня
		public virtual void OnLevelLoading(string level) { }

		// после загрузки игрового уровня
		public virtual void OnLevelLoaded(string level) { }

		// когда икрок может управлять главным героем - время игры идёт
		public virtual void OnGameStart() { }

		// когда время остановлено - пауза
		public virtual void OnGameStop() { }

		// когда игрок начинает подключаться к серверу
		public virtual void OnClientConnectionReceived(int channelId) { }

		// когда игрок подключился к серверу
		public virtual void OnClientReadyForGameplay(int channelId) { }

		// когда игрока выкинуло из сервера - обрыв связи с сервером
		public virtual void OnClientDisconnected(int channelId) { }
	}
	/** API BusEngine.Plugin */
}
/** API BusEngine */

/** API BusEngine.Game - пользовательский код для теста */
namespace BusEngine.Game {
	/** API BusEngine.Plugin */
	public class Default : BusEngine.Plugin {
		// при заапуске BusEngine до создания формы
		public override void Initialize() {
			BusEngine.Log.Info("Initialize");
		}

		// после загрузки определённого плагина
		public override void Initialize(string plugin) {
			BusEngine.Log.Info("Initialize " + plugin);
		}

		// перед закрытием BusEngine
		public override void Shutdown() {
			BusEngine.Log.Info("Shutdown");
		}

		// перед загрузкой игрового уровня
		public override void OnLevelLoading(string level) {
			BusEngine.Log.Info("OnLevelLoading");
		}

		// после загрузки игрового уровня
		public override void OnLevelLoaded(string level) {
			BusEngine.Log.Info("OnLevelLoaded");
		}

		// когда икрок может управлять главным героем - время игры идёт
		public override void OnGameStart() {
			BusEngine.Log.Info("OnGameStart");
		}

		// когда время остановлено - пауза
		public override void OnGameStop() {
			BusEngine.Log.Info("OnGameStop");
		}

		// когда игрок начинает подключаться к серверу
		public override void OnClientConnectionReceived(int channelId) {
			BusEngine.Log.Info("OnClientConnectionReceived");
		}

		// когда игрок подключился к серверу
		public override void OnClientReadyForGameplay(int channelId) {
			BusEngine.Log.Info("OnClientReadyForGameplay");
		}

		// когда игрока выкинуло из сервера - обрыв связи с сервером
		public override void OnClientDisconnected(int channelId) {
			BusEngine.Log.Info("OnClientDisconnected");
		}
	}
	/** API BusEngine.Plugin */
}
/** API BusEngine.Game - пользовательский код для теста */