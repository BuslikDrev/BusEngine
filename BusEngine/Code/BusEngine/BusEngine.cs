/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

/* C# 6.0+              https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history */
/* NET.Framework 4.7.1+ https://learn.microsoft.com/ru-ru/dotnet/framework/migration-guide/versions-and-dependencies */
/* MSBuild 14.0+        https://en.wikipedia.org/wiki/MSBuild#Versions */
/* MSBuild 15.0+        https://learn.microsoft.com/en-us/xamarin/android/app-fundamentals/android-api-levels?tabs=windows#android-versions */
/* Mono                 https://learn.microsoft.com/ru-ru/xamarin/android/deploy-test/building-apps/abi-specific-apks */
/* важные ссылки
https://metanit.com/sharp/patterns/2.3.php
https://habr.com/ru/post/125421/
https://learn.microsoft.com/ru-ru/xamarin/android/app-fundamentals/permissions?tabs=windows
https://learn.microsoft.com/ru-ru/dotnet/csharp/fundamentals/coding-style/coding-conventions
https://learn.microsoft.com/ru-ru/dotnet/csharp/language-reference/keywords/event
*/

/** дорожная карта
- проставить нормально модификаторы доступа https://metanit.com/sharp/tutorial/3.2.php
- максимально весь функционал сделать независимыми плагинами и установить проверки
 на наличие плагинов перед их использованием
- создать: генерацию сцены (карты), камеру, консольные команды, консоль, настройка проекта
- добавить поддержку форматов .dae https://docs.fileformat.com/ru/3d/dae/, .png, .mtl, .obj
- написать сборку игры для windows 7+ и Android 5+
- сделать максимально под ООП (инициализировать через new, чтобы можно было в случае event получить, уничтожить этот же объект, если он не нужен, можно установить статические свойства, методы и события которые выполняют отдельную работу от объекта или имеют постоянные данные) те объекты которые
можно загружать несколько раз. Если объект можно загрузить 1 раз, то можно static с проверкой на null.
- сторонние библиотеки обвернуть в исключения try catch - нужно от них ожидать только ошибки.
*/

#define BUSENGINE
/** API BusEngine */
namespace BusEngine {
/*
Зависимости нет
*/
	/** API BusEngine.ProjectDefault */
	internal class ProjectDefault {
		public ProjectDefault(object setting) {
			Setting = setting;
		}

		public static object Setting = new {
			console_commands = new {
				sys_spec = "1",
				e_WaterOcean = "0",
				r_WaterOcean = "0",
				r_VolumetricClouds = "1",
				r_Displayinfo = "0",
				r_Fullscreen = "0",
				r_Width = "1280",
				r_Height = "720"
			},
			console_variables = new {
				sys_spec = "1",
				e_WaterOcean = "0",
				r_WaterOcean = "0",
				r_VolumetricClouds = "1",
				r_Displayinfo = "0",
				r_Fullscreen = "0",
				r_Width = "1280",
				r_Height = "720"
			},
			version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
			type = "",
			info = new {
				name = "Game",
				guid = "ddc2049b-3a86-425b-9713-ee1babec5365"
			},
			content = new {
				assets = new string[] {"GameData"},
				code = new string[] {"Code"},
				libs = new {
					name = "BusEngine",
					shared = new {
						Any = "",
						Android = "",
						Win = "",
						Win_x64 = "",
						Win_x86 = ""
					},
				},
			},
			require = new {
				engine = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(),
				plugins = new object[] {
					new {
						guid = "",
						type = "EType::Managed",
						path = "Bin/Android/Game.dll",
						platforms = new string[] {"Android"}
					},
					new {
						guid = "",
						type = "EType::Managed",
						path = "Bin/Win/Game.dll",
						platforms = new string[] {"win_x86"}
					},
					new {
						guid = "",
						type = "EType::Managed",
						path = "Bin/Win_x86/Game.dll",
						platforms = new string[] {"win_x86"}
					},
					new {
						guid = "",
						type = "EType::Managed",
						path = "Bin/Win_x64/Game.dll",
						platforms = new string[] {"Win_x64"}
					}
				},
			},
		};

		public static System.Collections.Generic.Dictionary<string, dynamic> Setting2 = new System.Collections.Generic.Dictionary<string, dynamic>() {
			{"console_commands", new System.Collections.Generic.Dictionary<string, string>() {
				{"sys_spec", "1"},
				{"e_WaterOcean", "0"},
				{"r_WaterOcean", "0"},
				{"r_VolumetricClouds", "1"},
				{"r_Displayinfo", "0"},
				{"r_Fullscreen", "0"},
				{"r_Width", "1280"},
				{"r_Height", "720"}
			}},
			{"console_variables", new System.Collections.Generic.Dictionary<string, string>() {
				{"sys_spec", "1"},
				{"e_WaterOcean", "0"},
				{"r_WaterOcean", "0"},
				{"r_VolumetricClouds", "1"},
				{"r_Displayinfo", "0"},
				{"r_Fullscreen", "0"},
				{"r_Width", "1280"},
				{"r_Height", "720"}
			}},
			{"version", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()},
			{"type", ""},
			{"info", new System.Collections.Generic.Dictionary<string, string>() {
				{"name", "Game"},
				{"guid", "ddc2049b-3a86-425b-9713-ee1babec5365"}
			}},
			{"content", new System.Collections.Generic.Dictionary<string, object>() {
				{"assets", new string[] {"GameData"}},
				{"code", new string[] {"Code"}},
				{"libs", new System.Collections.Generic.Dictionary<string, object>() {
					{"name", "BusEngine"},
					{"shared", new System.Collections.Generic.Dictionary<string, string>() {
						{"Any", ""},
						{"Android", ""},
						{"Win", ""},
						{"Win_x64", ""},
						{"Win_x86", ""}
					}}
				}}
			}},
			{"require", new System.Collections.Generic.Dictionary<string, object>() {
				{"engine", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()},
				{"plugins", new System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>() {
					new System.Collections.Generic.Dictionary<string, object>() {
						{"System", ""},
						{"type", "EType::Managed"},
						{"path", "Bin/Android/Game.dll"},
						{"platforms", new string[] {"Android"}}
					},
					new System.Collections.Generic.Dictionary<string, object>() {
						{"System", ""},
						{"type", "EType::Managed"},
						{"path", "Bin/Win/Game.dll"},
						{"platforms", new string[] {"win_x86"}}
					},
					new System.Collections.Generic.Dictionary<string, object>() {
						{"System", ""},
						{"type", "EType::Managed"},
						{"path", "Bin/Win_x86/Game.dll"},
						{"platforms", new string[] {"win_x86"}}
					},
					new System.Collections.Generic.Dictionary<string, object>() {
						{"System", ""},
						{"type", "EType::Managed"},
						{"path", "Bin/Win_x64/Game.dll"},
						{"platforms", new string[] {"Win_x64"}}
					}
				}}
			}}
		};
	}
	/** API BusEngine.ProjectDefault */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.UI.Canvas
BusEngine.Video
*/
	//using Audio = BusEngine.Video;
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
BusEngine.UI.Canvas
*/
	/** API BusEngine.Browser */
	public class Browser : System.IDisposable {
		private static CefSharp.WinForms.ChromiumWebBrowser browser;
		public delegate void OnPostMessageHandler(string e);
		public static event OnPostMessageHandler OnPostMessage;
		public delegate void OnLoadHandler();
		public static event OnLoadHandler OnLoad;
		public Browser() {}

		/** событие клика из браузера */
		/* private static void OnKeyDown(object o, System.Windows.Forms.KeyEventArgs e) {
			//KeyDown -= new System.Windows.Forms.KeyEventHandler(OnKeyDown);
			//BusEngine.Browser.Dispose();
			BusEngine.Log.Info("браузер клик тест 2");
		}

		private static void OnMouseClick(object o, System.Windows.Forms.MouseEventArgs e) {
			BusEngine.Log.Info("браузер Mouse клик тест 2");
		} */
		/** событие клика из браузера */

		/** все события из PostMessage js браузера */
		// https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions#13-how-do-you-handle-a-javascript-event-in-c
		private static void OnCefPostMessage(object sender, CefSharp.JavascriptMessageReceivedEventArgs e) {
			BusEngine.Log.Info("BusEngine.Browser.{0}", "OnPostMessage");
			OnPostMessage.Invoke((string)e.Message);
		}
		/** все события из PostMessage js браузера */

		/** событие загрузки страницы браузера */
		// https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions#13-how-do-you-handle-a-javascript-event-in-c
		private static void OnCefFrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e) {
			if (e.Frame.IsMain && OnLoad != null) {
				BusEngine.Log.Info("BusEngine.Browser.{0}", "OnLoad");
				OnLoad.Invoke();
			}
		}
		/** событие загрузки страницы браузера */

		/** заменяем на своё CefSharp.PostMessage на BusEngine.PostMessage */
		// https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions#13-how-do-you-handle-a-javascript-event-in-c
		private static void OnCefSharpReplace(object sender, CefSharp.FrameLoadEndEventArgs e) {
			if (e.Frame.IsMain) {
				ExecuteJS("if ('CefSharp' in window) {BusEngine.PostMessage = CefSharp.PostMessage;} else {BusEngine.PostMessage = function(m) {};}");
			}
		}
		/** заменяем на своё CefSharp.PostMessage на BusEngine.PostMessage */

		/** функция выполнения js кода в браузере */
		public static void ExecuteJS(string js = "") {
			if (browser != null) {
				CefSharp.WebBrowserExtensions.ExecuteScriptAsync(browser, @js);
			} else {
				BusEngine.Log.Info("Ошибка! {0}", "Браузер ещё не запущен!");
			}
		}
		/** функция выполнения js кода в браузере */

		/* public static void StaticExecuteJS(Browser browser, string js = "") {
			if (browser != null) {
				CefSharp.WebBrowserExtensions.ExecuteScriptAsync(browser, @js);
			} else {
				BusEngine.Log.Info("Ошибка! {0}", "Браузер ещё не запущен!");
			}
		} */

		private static bool ValidURL(string s, out System.Uri url) {
			if (!System.Text.RegularExpressions.Regex.IsMatch(s, @"^https?:\/\/", System.Text.RegularExpressions.RegexOptions.IgnoreCase)) {
				s = "http://" + s;
			}

			if (System.Uri.TryCreate(s, System.UriKind.Absolute, out url)) {
				return (url.Scheme == System.Uri.UriSchemeHttp || url.Scheme == System.Uri.UriSchemeHttps);
			}

			return false;
		}

		/** функция запуска браузера */
		// https://cefsharp.github.io/api/
		public static void Initialize(string url = "") {
			Initialize(url, BusEngine.Engine.DataDirectory);
		}
		public static void Initialize(string url = "", string root = "") {
			if (browser != null) {
				BusEngine.Log.Info("Ошибка! {0}", "Браузер уже запущен!");
			} else {
				// если ссылка не абсолютный адрес, то делаем его абсолютным
				System.Uri uriResult;
				if (ValidURL(url, out uriResult) && url.IndexOf(':') == -1) {
					if (System.IO.File.Exists(System.IO.Path.Combine(BusEngine.Engine.DataDirectory, url))) {
						url = "https://BusEngine/" + url;
					} else {
						url = null;
					}
				}

				if (System.IO.Directory.Exists(System.IO.Path.Combine(BusEngine.Engine.DataDirectory, root))) {
					root = System.IO.Path.Combine(BusEngine.Engine.DataDirectory, root);
				} else {
					root = BusEngine.Engine.DataDirectory;
				}

				//CefSharp.BrowserSubprocess.SelfHost.Main(args);

				// подгружаем объект настроек CefSharp по умолчанияю, чтобы внести свои правки
				CefSharp.WinForms.CefSettings settings = new CefSharp.WinForms.CefSettings();

				// включаем поддержку экранов с высоким разрешением
				CefSharp.Cef.EnableHighDPISupport();

				// устанавливаем свой юзер агент
				settings.UserAgent = BusEngine.Engine.Device.UserAgent;

				// отключаем создание файла лога
				settings.LogSeverity = CefSharp.LogSeverity.Disable;

				// https://github.com/cefsharp/CefSharp/wiki/General-Usage#scheme-handler
				// регистрируем свою схему
				settings.RegisterScheme(new CefSharp.CefCustomScheme {
					SchemeName = "https",
					DomainName = "BusEngine",
					SchemeHandlerFactory = new CefSharp.SchemeHandler.FolderSchemeHandlerFactory (
						rootFolder: root,
						hostName: "BusEngine",
						defaultPage: "index.html"
					)
				});

				// применяем наши настройки до запуска браузера
				CefSharp.Cef.Initialize(settings);

				// запускаем браузер
				browser = new CefSharp.WinForms.ChromiumWebBrowser(url);

				if (url != null && !ValidURL(url, out uriResult)) {
					CefSharp.WebBrowserExtensions.LoadHtml(browser, url, true);
				} else if (url == null) {
					if (BusEngine.Localization.GetLanguage("error_browser_url") != "error_browser_url") {
						url = "<meta charset=\"UTF-8\"><b>" + BusEngine.Localization.GetLanguage("error_browser_url") + "</b>";
					} else {
						url = "<meta charset=\"UTF-8\"><b>ПРАВЕРЦЕ ШЛЯХ ДА ФАЙЛУ!</b>";
					}

					CefSharp.WebBrowserExtensions.LoadHtml(browser, url, true);
				}

				// просто подключаем левое событие - можно удалить, оно не работает =) только через PostMessage
				/* browser.KeyDown += OnKeyDown;
				browser.MouseClick += OnMouseClick; */

				// https://stackoverflow.com/questions/51259813/call-c-sharp-function-from-javascript-using-cefsharp-in-windows-form-app
				// подключаем событие сообщения из javascript
				browser.JavascriptMessageReceived += OnCefPostMessage;
				// подключаем событие загрузки страницы
				browser.FrameLoadEnd += OnCefSharpReplace;
				browser.FrameLoadEnd += OnCefFrameLoadEnd;

				// устанавливаем размер окана браузера, как в нашей программе
				//browser.Size = BusEngine.UI.Canvas.WinForm.ClientSize;
				//browser.Dock = BusEngine.UI.Canvas.WinForm.Dock;

				// подключаем браузер к нашей программе
				BusEngine.UI.Canvas.WinForm.Controls.Add(browser);
			}
		}
		/** функция запуска браузера */

		public static void Shutdown() {
			// одключаем браузер от нашей программы
			BusEngine.UI.Canvas.WinForm.Controls.Remove(browser);
		}

		public void Dispose() {browser = null;}
	}
	/** API BusEngine.Browser */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.Log
*/
	/** API BusEngine.Core */
	// https://habr.com/ru/post/196578/
	public class Core {

	}
	/** API BusEngine.Core */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
	
/* TValue this[TKey key]
		{
			
			get;
			
			set;
		} */
	
	/** API BusEngine.Array */
	public interface IArray<TKey, TValue> {
		/* public Array(TKey key, TKey value) {
			BusEngine.Log.Info("Array {0}");
		} */

		TValue this[TKey key] {	get; set; }
		
		bool ContainsKey(TKey key);
		
		void Add(TKey key, TValue value);
		
		bool Remove(TKey key);
		
		bool TryGetValue(TKey key, out TValue value);
	}

	/* public class Array<TKey, TValue> : BusEngine.IArray<TKey, TValue>, System.Collections.Generic.ICollection<System.Collections.Generic.KeyValuePair<TKey, TValue>>, System.Collections.Generic.IEnumerable<System.Collections.Generic.KeyValuePair<TKey, TValue>>, System.Collections.IEnumerable, System.Collections.IDictionary, System.Collections.ICollection, System.Collections.Generic.IReadOnlyDictionary<TKey, TValue>, System.Collections.Generic.IReadOnlyCollection<System.Collections.Generic.KeyValuePair<TKey, TValue>>, System.Runtime.Serialization.ISerializable, System.Runtime.Serialization.IDeserializationCallback {
		public Array() : this(0, null)	{}

		public TValue this[TKey key] {
			get {return default(TValue);}
			set {}
		}

		public Array(BusEngine.IArray<TKey, TValue> dictionary) : this(dictionary, null) {}

		public Array(IArray<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) : this((dictionary != null) ? dictionary.Count : 0, comparer) {

		}

		public bool ContainsKey(TKey key) {
			return false;
		}

		public void Add(TKey key, TValue value) {}

		public bool Remove(TKey key) {
			return false;
		}
	} */
	/** API BusEngine.Array */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.Core
BusEngine.Log
BusEngine.Plugin
BusEngine.Tools
*/
	/** API BusEngine.Engine */
	public class Engine {
		// https://www.manojphadnis.net/need-to-know-general-topics/listkeyvaluepair-vs-dictionary
		public static System.Collections.Generic.Dictionary<string, dynamic> SettingEngine = new System.Collections.Generic.Dictionary<string, dynamic>();
		public static System.Collections.Generic.Dictionary<string, dynamic> SettingProject = new System.Collections.Generic.Dictionary<string, dynamic>();
		//public virtual System.Collections.Generic.IEnumerable<System.Reflection.Module> Modules { get; }
		public static string EngineDirectory;
		public static string ExeDirectory;
		public static string BinDirectory;
		public static string EditorDirectory;
		public static string DataDirectory;
		public static string ToolsDirectory;
		public static string Platform = "BusEngine";
		// определяем платформу, версию, архитектуру процессора (NET.Framework 4.7.1+)
		public class Device {
			public static string Name;
			public static string Version;
			public static string Processor;
			public static byte ProcessorCount;
			public static string UserAgent;
			static Device() {
				var os = System.Environment.OSVersion;

				switch (os.Platform) {
					case System.PlatformID.Win32NT:
					case System.PlatformID.Win32S:
					case System.PlatformID.Win32Windows:
					case System.PlatformID.WinCE:
						Name = "Windows";
						break;
					case System.PlatformID.MacOSX:
					case System.PlatformID.Unix:
						Name = "MacOSX";
						break;
					default:
						Name = "Other";
						break;
				}

				/* switch ((0).GetType().ToString()) {
					case "Int32":
						Processor = "x32";
						break;
					case "Int64":
						Processor = "x64";
						break;
					case "Int128":
						Processor = "x128";
						break;
					case "Int256":
						Processor = "x256";
						break;
					case "Int512":
						Processor = "x512";
						break;
					default:
						Processor = "Other";
						break;
				} */

				Version = os.Version.Major + "." + os.Version.Minor;
				Processor = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString();
				ProcessorCount = (byte)System.Environment.ProcessorCount;
				UserAgent = "Mozilla/5.0 (" + Name + " NT " + Version + "; " + System.Convert.ToString(os.Platform) + "; " + Processor + ") AppleWebKit/537.36 (KHTML, like Gecko) BusEngine/" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version + " Safari/537.36";
			}
		}

		/** функция запуска API BusEngine */
		public static void Initialize() {
			// устанавливаем ссылку на рабочий каталог
			BusEngine.Engine.ExeDirectory = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);

			string path = BusEngine.Engine.ExeDirectory + "\\..\\..\\Bin\\";

			if (!System.IO.Directory.Exists(path)) {
				path = BusEngine.Engine.ExeDirectory + "\\..\\Bin\\";

				if (!System.IO.Directory.Exists(path)) {
					path = BusEngine.Engine.ExeDirectory + "\\Bin\\";
				}
			}

			path = System.IO.Path.GetFullPath(path + "..\\");

			BusEngine.Engine.EngineDirectory = path;
			BusEngine.Engine.BinDirectory = path + "Bin\\";
			BusEngine.Engine.EditorDirectory = path + "Editor\\";
			BusEngine.Engine.DataDirectory = path + "Data\\";

			// определяем устройство
			new BusEngine.Engine.Device();

			// инициализируем язык
			new BusEngine.Localization().Initialize();

			// включаем консоль
			BusEngine.Log.ConsoleShow();

			/* BusEngine.Log.Info("Device {0}", BusEngine.Engine.Device.UserAgent);
			BusEngine.Log.Info("Device {0}", BusEngine.Engine.Device.Name);
			BusEngine.Log.Info("Device {0}", BusEngine.Engine.Device.Version);
			BusEngine.Log.Info("Device {0}", BusEngine.Engine.Device.Processor);
			BusEngine.Log.Info("Device {0}", BusEngine.Engine.Device.ProcessorCount); */

			/* BusEngine.Log.Info("Setting {0}", BusEngine.ProjectDefault.Setting.GetType().GetProperty("version").GetValue(BusEngine.ProjectDefault.Setting));
			BusEngine.Log.Info("Setting {0}", BusEngine.ProjectDefault.Setting.GetType().GetProperty("console_commands").GetValue(BusEngine.ProjectDefault.Setting).GetType().GetProperty("sys_spec").GetValue(BusEngine.ProjectDefault.Setting.GetType().GetProperty("console_commands").GetValue(BusEngine.ProjectDefault.Setting)));
			BusEngine.Log.Info("Setting {0}", BusEngine.Tools.Json.Decode(BusEngine.Tools.Json.Encode(BusEngine.ProjectDefault.Setting), true));
			BusEngine.Log.Info("Setting {0}", BusEngine.Tools.Json.Encode(BusEngine.Tools.Json.Decode(BusEngine.Tools.Json.Encode(BusEngine.ProjectDefault.Setting))));

			BusEngine.Log.Info("Setting2 {0}", BusEngine.ProjectDefault.Setting2["version"]);
			BusEngine.Log.Info("Setting2 {0}", BusEngine.ProjectDefault.Setting2["console_commands"]["sys_spec"]);
			BusEngine.Log.Info("Setting2 {0}", BusEngine.Tools.Json.Encode(BusEngine.Tools.Json.Decode(BusEngine.Tools.Json.Encode(BusEngine.ProjectDefault.Setting2)))); */

			// https://metanit.com/sharp/tutorial/5.4.php
			// https://metanit.com/sharp/tutorial/5.5.php
			// https://metanit.com/sharp/tutorial/6.4.php
			// https://dir.by/developer/csharp/serialization_json/?lang=eng
			// ищем, загружаем и обрабатываем настройки проекта
			string[] files;

			files = System.IO.Directory.GetFiles(path, "*.busproject");

			if (files.Length == 0) {
				// запись
				using (System.IO.FileStream fstream = System.IO.File.OpenWrite(path + "Game.busproject")) {
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(BusEngine.Tools.Json.Encode(BusEngine.ProjectDefault.Setting));
					fstream.Write(buffer, 0, buffer.Length);
				}
			} else {
				//BusEngine.Log.Info(files[0]);

				// запись
				/* using (System.IO.StreamWriter fstream = new System.IO.StreamWriter(files[0], false, System.Text.Encoding.UTF8)) {
					fstream.Write(BusEngine.Tools.Json.Encode(BusEngine.ProjectDefault.Setting));
				} */

				// запись
				/* using (System.IO.FileStream fstream = System.IO.File.OpenWrite(files[0])) {
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(BusEngine.Tools.Json.Encode(BusEngine.Tools.Json.Decode(BusEngine.Tools.Json.Encode(BusEngine.ProjectDefault.Setting), true)));

					fstream.Write(buffer, 0, buffer.Length);
				} */

				// запись
				/* using (System.IO.FileStream fstream = new System.IO.FileStream(files[0], System.IO.FileMode.OpenOrCreate)) {
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(BusEngine.Tools.Json.Encode(BusEngine.ProjectDefault.Setting));
					fstream.WriteAsync(buffer, 0, buffer.Length);
				} */

				// запись
				//System.IO.File.WriteAllText(files[0], BusEngine.Tools.Json.Encode(BusEngine.ProjectDefault.Setting));

				// чтение
				/* using (System.IO.FileStream fstream = new System.IO.FileStream(files[0], System.IO.FileMode.OpenOrCreate)) {
					byte[] buffer = new byte[fstream.Length];
					fstream.ReadAsync(buffer, 0, buffer.Length);
					// декодируем байты в строку
					BusEngine.Tools.Json.Decode(System.Text.Encoding.UTF8.GetString(buffer));
				} */

				// чтение
				//BusEngine.Tools.Json.Decode(System.IO.File.ReadAllText(files[0]));
			}

			// тестирование плагина - прогонка кода
			//Newtonsoft.Json.JsonConvert.DeserializeObject(Newtonsoft.Json.JsonConvert.SerializeObject(new BusEngine.ProjectSettingDefault(), Newtonsoft.Json.Formatting.Indented));
			//BusEngine.Tools.Json.Decode(BusEngine.Tools.Json.Encode(BusEngine.ProjectDefault.Setting), true);

			files = System.IO.Directory.GetFiles(path, "busengine.busengine");

			if (files.Length == 0) {
				//BusEngine.Log.Info(files.Length);

				// запись
				using (System.IO.FileStream fstream = System.IO.File.OpenWrite(path + "busengine.busengine")) {
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(BusEngine.Tools.Json.Encode(BusEngine.ProjectDefault.Setting));
					fstream.Write(buffer, 0, buffer.Length);
				}
			} else {
				// улаляем массивы данных по умолчанию т.к. они не нужны
				BusEngine.ProjectDefault.Setting2["require"]["plugins"].Clear();

				// получаем новые данные
				var setting = BusEngine.Tools.Json.Decode(System.IO.File.ReadAllText(files[0]));

				dynamic console_commands;

				if (setting.TryGetValue("console_commands", out console_commands) && console_commands.GetType().GetProperty("Type") != null && !console_commands.GetType().IsArray) {
					foreach (var i in console_commands) {
						if (i is object && i.GetType().GetProperty("Name") != null && i.Name is string) {
							BusEngine.ProjectDefault.Setting2["console_commands"][i.Name] = (string)console_commands[i.Name];
						}
					}

					//BusEngine.Log.Info("console_commands {0}", BusEngine.Tools.Json.Encode(BusEngine.ProjectDefault.Setting2["console_commands"]));
				}

				dynamic console_variables;

				if (setting.TryGetValue("console_variables", out console_variables) && console_variables.GetType().GetProperty("Type") != null && !console_variables.GetType().IsArray) {
					foreach (var i in console_variables) {
						if (i is object && i.GetType().GetProperty("Name") != null && i.Name is string) {
							BusEngine.ProjectDefault.Setting2["console_variables"][i.Name] = (string)console_variables[i.Name];
						}
					}

					//BusEngine.Log.Info("console_variables {0}", BusEngine.Tools.Json.Encode(BusEngine.ProjectDefault.Setting2["console_variables"]));
				}

				dynamic require;

				if (setting.TryGetValue("require", out require) && require.GetType().GetProperty("Type") != null && require.ContainsKey("plugins") && require["plugins"].Type.ToString() == "Array") {
					int i, ii = require["plugins"].Count;

					for (i = 0; i < ii; ++i) {
						if (require["plugins"][i].ContainsKey("path") && require["plugins"][i]["path"].Type.ToString() == "String" && require["plugins"][i]["path"] != "") {
							if (System.IO.File.Exists(System.IO.Path.GetFullPath(BusEngine.Engine.ExeDirectory + require["plugins"][i]["path"] + ".dll"))) {
								require["plugins"][i]["path"] = System.IO.Path.GetFullPath(BusEngine.Engine.ExeDirectory + require["plugins"][i]["path"] + ".dll");
							} else if (System.IO.File.Exists(System.IO.Path.GetFullPath(BusEngine.Engine.ExeDirectory + require["plugins"][i]["path"]))) {
								require["plugins"][i]["path"] = System.IO.Path.GetFullPath(BusEngine.Engine.ExeDirectory + require["plugins"][i]["path"]);
							} else if (System.IO.File.Exists(System.IO.Path.GetFullPath(BusEngine.Engine.EngineDirectory + require["plugins"][i]["path"]))) {
								require["plugins"][i]["path"] = System.IO.Path.GetFullPath(BusEngine.Engine.EngineDirectory + require["plugins"][i]["path"]);
							} else {
								require["plugins"][i]["path"] = "";
							}

							if (require["plugins"][i]["path"] != "") {
								BusEngine.ProjectDefault.Setting2["require"]["plugins"].Add(new System.Collections.Generic.Dictionary<string, object>() {
									{"path", System.Convert.ToString(require["plugins"][i]["path"])},
									{"guid", (require["plugins"][i].ContainsKey("guid") && require["plugins"][i]["guid"].Type.ToString() == "String" ? System.Convert.ToString(require["plugins"][i]["guid"]) : "")},
									{"type", (require["plugins"][i].ContainsKey("type") && require["plugins"][i]["type"].Type.ToString() == "String" ? System.Convert.ToString(require["plugins"][i]["type"]) : "")},
									{"platforms", (require["plugins"][i].ContainsKey("platforms") && require["plugins"][i]["platforms"].Type.ToString() == "Array" ? require["plugins"][i]["platforms"] : new string[] {})}
								});
							}
						}
					}

					//BusEngine.Log.Info("plugins {0}", BusEngine.Tools.Json.Encode(BusEngine.ProjectDefault.Setting2["require"]["plugins"]));
				}
			}

			BusEngine.Engine.SettingEngine = BusEngine.ProjectDefault.Setting2;

			// инициализируем плагины
			new BusEngine.IPlugin("Initialize");
		}
		/** функция запуска API BusEngine */

		/** функция остановки API BusEngine  */
		public static void Shutdown() {
			// отключаем плагины
			new BusEngine.IPlugin("Shutdown");
			// закрываем окно консоли
			BusEngine.Log.ConsoleHide();
			// закрываем окно BusEngine
			System.Windows.Forms.Application.Exit();
		}
		/** функция остановки API BusEngine  */
	}
	/** API BusEngine.Engine */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
	public class Localization : System.IDisposable {
		//[Tooltip("Loading a language if the desired one is not available.")]
		public string LanguageDefault = "Belarusian";
		//[Tooltip("Forced language loading")]
		public string Language = "";
		//[Tooltip("Provide a name for the translation file to use different files for different scenes. Example, 'level_1' - as a result, the path to the file will become: 'Assets/Localization/lang_name/level_1.cfg.")]
		public string File = "";
		//[Tooltip("Format lang file. For mobiles and sites Unity Support: txt, html, htm, xml, bytes, json, csv, yaml, fnt")]
		public string Format = "cfg";
		//[Tooltip("Translate components located in inactive objects?")]
		private bool IncludeInactive = false;
		//[Tooltip("Replace Resources.load with Bundle.load?")]
		private bool BundleStatus = false;

		public delegate void LoadHandler(Localization sender, string e);
		public static event LoadHandler OnLoad;
		public delegate void Call();
		private Call CallbackStart = null;
		private static System.Collections.Generic.Dictionary<string, string> GetLanguages = new System.Collections.Generic.Dictionary<string, string>();
		private static string Value = "";
		private Localization _Localization;

		public static string GetLanguage(string key) {
			if (GetLanguages.TryGetValue(key, out Value)) {
				return Value;
			} else {
				return key;
			}
		}

		/* public Localization() {
			_Localization = this;
		} */

		public static void SetLanguage(string key, string value) {
			// C# 6.0+
			GetLanguages[key] = value;
			// C# 4.0+
			/* if (GetLanguages.ContainsKey(key)) {
				GetLanguages.Remove(key);
			}
			GetLanguages.Add(key, value); */
		}

		public string Get(string key) {
			if (GetLanguages.TryGetValue(key, out Value)) {
				return Value;
			} else {
				return key;
			}
		}

		public void Set(string key, string value) {
			GetLanguages[key] = value;
		}

		public static bool CallBack(Call callback = null) {
			if (callback != null) {
				Call CallbackStart = callback;
			}
			
			return false;
		}

		public void Initialize() {
			if (Language == null || Language == "") {
				Language = LanguageDefault.ToString();
			}
			//OnLoad.Invoke(this, Language);
			StartLocalization(Language);
		}

		public void Load(string Language = null) {
			StartLocalization(Language);
			OnLoad.Invoke(this, Language);
		}

		public void ReLoad() {
			/* if (GetLanguages.Count > 0) {
				Component[] results = GetComponentsInChildren(typeof(Text), includeInactive);

				if (results != null) {
					foreach (Text reslut in results) {
						if (GetLanguages.ContainsKey(reslut.text)) {
							reslut.text = GetLanguages[reslut.text].ToString();
						}
					}
				}

				Component[] results_mesh_pro = GetComponentsInChildren(typeof(TMPro.TextMeshProUGUI), includeInactive);

				if (results_mesh_pro != null) {
					foreach (TMPro.TextMeshProUGUI reslut in results_mesh_pro) {
						if (GetLanguages.ContainsKey(reslut.text)) {
							reslut.text = GetLanguages[reslut.text].ToString();
						}
					}
				}
			} */
		}

		private void StartLocalization(string Language = null) {
			int n = File.Length;
			if (n > 0) {
				File = "/" + File;
			}
			string path, platform, files;

			files = "";
			if (Language == null || Language == "") {
				Language = System.Globalization.CultureInfo.CurrentCulture.EnglishName.ToString();
			}
			// https://docs.unity3d.com/ScriptReference/RuntimePlatform.html
			platform = BusEngine.Engine.Platform;

			if (platform.IndexOf("Windows") != -1 || 1 == 1) {
				path = BusEngine.Engine.DataDirectory + "../Localization/";
				if (!System.IO.Directory.Exists(path)) {
					path = BusEngine.Engine.DataDirectory + "/Localization/";
				}
				if (!System.IO.Directory.Exists(path)) {
					path = BusEngine.Engine.DataDirectory + "/Resources/Localization/";
				}
				if (!System.IO.Directory.Exists(path)) {
					path = BusEngine.Engine.DataDirectory + "/Localization/";
				}
			} else {
				if (platform == "WebGLPlayer" && !BundleStatus) {
					path = "Localization/";
				} else {
					// https://docs.unity3d.com/Manual/StreamingAssets.html
					// https://docs.unity3d.com/ScriptReference/Application-streamingAssetsPath.html
					path = BusEngine.Engine.DataDirectory + "/Localization/";
				}
			}

			if (platform == "WebGLPlayer") {
				// https://learn.microsoft.com/en-us/visualstudio/msbuild/common-msbuild-project-items?view=vs-2022#embeddedresource
				// https://learn.microsoft.com/en-us/xamarin/xamarin-forms/data-cloud/data/files?tabs=windows
				/* if (BundleStatus) {
					//AssetBundle bundle = myLoadedAssetBundle = AssetBundle.LoadFromFile(path + Language + File + "." + Format);
					//TextAsset resources = bundle.Load<TextAsset>(File + "." + Format);
				} else {
					// https://docs.unity3d.com/2022.2/Documentation/Manual/class-TextAsset.html
					TextAsset resources = Resources.Load(path + Language + File, typeof(TextAsset)) as TextAsset;
					if (resources != null) {
						files = resources.text;
						//files = System.Text.Encoding.UTF8.GetString(resources.bytes);
						//Resources.UnloadAsset(resources);
					} else {
						Language = LanguageDefault;
						resources = Resources.Load(path + Language + File, typeof(TextAsset)) as TextAsset;
						if (resources != null) {
							files = resources.text;
							//files = System.Text.Encoding.UTF8.GetString(resources.bytes);
							//Resources.UnloadAsset(resources);
						}
					}
				} */

				/* IEnumerator GetText(string url) {
					UnityWebRequest www = UnityWebRequest.Get(url);
					yield return www.Send();

					if(www.isError) {
						Debug.Log(www.error);
					} else {
						// Show results as text
						Debug.Log(www.downloadHandler.text);

						// Or retrieve results as binary data
						byte[] results = www.downloadHandler.data;
					}
				}
				files = StartCoroutine(GetText("https://buslikdrev.by/game/StreamingAssets/Localization/Belarusian.txt")).ToString();
				UnityEngine.Debug.Log(files); */
			} else {
				if (System.IO.File.Exists(path + Language + File + "." + Format)) {
					files = System.IO.File.ReadAllText(path + Language + File + "." + Format);
					//files = System.Text.Encoding.UTF8.GetString(File.ReadAllBytes(path + Language + file + "." + Format));
				} else {
					Language = LanguageDefault;
					if (System.IO.File.Exists(path + Language + File + "." + Format)) {
						files = System.IO.File.ReadAllText(path + Language + File + "." + Format);
						//files = System.Text.Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(path + Language + File + "." + Format));
					}
				}
			}

			if (files != "") {
				string[] lines, pairs;
				int i, ii;

				lines = files.Split(new string[] {"\r\n", "\n\r", "\n"}, System.StringSplitOptions.RemoveEmptyEntries);
				ii = lines.Length;
				files = null;
				System.GC.Collect();

				for (i = 0; i < ii; ++i) {
					pairs = lines[i].Split(new char[] {'='}, 2);
					if (pairs.Length == 2) {
						GetLanguages[pairs[0].Trim()] = pairs[1].Trim();
					}
				}
			}

			ReLoad();
			if (CallbackStart != null) {
				CallbackStart();
			}
		}

		public static void Shutdown() {}

		public void Dispose() {}
	}
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

		// функия запуска команды
		public static void RunCommand(string command, double value = 0) {
			//ConsoleCommand.RegisterManagedConsoleCommandFunction(string commandName, uint nFlags, string commandHelpText, ManagedConsoleCommandFunctionDelegate consoleCmd Delegate)
		}

		// функция добавление (регистрации) команды
		public static void AddCommand(string command, double value = 0, string description = "") {
			
		}

		// функция запуска консоли
		public static void ConsoleShow() {
			if (BusEngine.Log.StatusConsole == false) {
				/* System.Windows.Forms.MessageBox.Show("Сообщение из Windows Forms!"); */

				BusEngine.Log.AttachConsole(-1);
				BusEngine.Log.AllocConsole();
				BusEngine.Log.StatusConsole = true;
				System.Console.Title = BusEngine.Localization.GetLanguage("text_name_console") + " v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
				System.Console.CancelKeyPress += new System.ConsoleCancelEventHandler(BusEngine.Log.MyHandler);

				//System.Console.Clear();
				BusEngine.Log.Info(BusEngine.Localization.GetLanguage("text_name_console"));
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

		protected static void MyHandler(object sender, System.ConsoleCancelEventArgs args) {
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

		// http://dir.by/developer/csharp/class_template/
		//public static delegate void String;

		// функция вывода строки в консоль
		public static void Info() {
			System.Console.WriteLine();
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
		public static void Info(byte args1) {
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
		
		/* public static void Info<A>(A a) {
			System.Console.WriteLine(a);
		}
		public static void Info<A, B>(A a, B b) {
			System.Console.WriteLine(a, b);
		}
		public static void Info<A, B, C>(A a, B b, C c) {
			System.Console.WriteLine(a, b, c);
		}
		public static void Info(int arg) {
			System.Console.WriteLine(arg);
		}
		public static void Info(params System.Type[] arg) {
			System.Console.WriteLine(arg);
		}
		public static void Info(params object[] args) {
			int i, ii = args.Length;
			for (i = 0; i < ii; ++i) {
				System.Console.Write(args[i] + " ");
			}
			System.Console.WriteLine();
		}
		public static void Info(params int[] args) {
			int i, ii = args.Length;
			for (i = 0; i < ii; ++i) {
				System.Console.Write(args[i] + " ");
			}
			System.Console.WriteLine();
		}
		public static void Info(params string[] args) {
			int i, ii = args.Length;
			for (i = 0; i < ii; ++i) {
				System.Console.Write(args[i] + " ");
			}
			System.Console.WriteLine();
		}
		public static void Info(string arg, params object[] args) {
			int i, ii = args.Length;
			for (i = 0; i < ii; ++i) {
				System.Console.Write(args[i] + " ");
			}
			System.Console.WriteLine();
		}
		public static void Info(string arg, params int[] args) {
			int i, ii = args.Length;
			for (i = 0; i < ii; ++i) {
				System.Console.Write(args[i] + " ");
			}
			System.Console.WriteLine();
		}
		public static void Info(string arg, params string[] args) {
			int i, ii = args.Length;
			for (i = 0; i < ii; ++i) {
				System.Console.Write(args[i] + " ");
			}
			System.Console.WriteLine();
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
		}

		public static void Shutdown() {}

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
	/** API BusEngine.Plugin */
	public abstract class Plugin {
		// при запуске BusEngine до создания формы
		public virtual void Initialize() {BusEngine.Log.Info("Plugin Initialize");}

		// после загрузки определённого плагина
		public virtual void Initialize(string plugin) {}

		// при запуске BusEngine после создания формы Canvas
		public virtual void InitializeСanvas() {}

		// перед закрытием BusEngine
		public virtual void Shutdown() {}

		// перед загрузкой игрового уровня
		public virtual void OnLevelLoading(string level) {}

		// после загрузки игрового уровня
		public virtual void OnLevelLoaded(string level) {}

		// когда игрок может управлять главным героем - время игры идёт
		public virtual void OnGameStart() {}

		// когда время остановлено - пауза или закрытие уровня
		public virtual void OnGameStop() {}

		// вызывается при отрисовки каждого кадра
		public virtual void OnGameUpdate() {}

		// когда игрок начинает подключаться к серверу
		public virtual void OnClientConnectionReceived(int channelId) {}

		// когда игрок подключился к серверу
		public virtual void OnClientReadyForGameplay(int channelId) {}

		// когда игрока выкинуло из сервера - обрыв связи с сервером
		public virtual void OnClientDisconnected(int channelId) {}
	}
	/** API BusEngine.Plugin */

	/** API BusEngine.IPlugin */
	internal class IPlugin : System.IDisposable {
		private static int Count = 0;

		// при запуске BusEngine до создания формы
		public IPlugin(string stage = "Initialize") {
			BusEngine.Log.Info("=============================================================================");
			BusEngine.Log.Info("System Plugins " + stage);

			int i, i2, i3, ii = BusEngine.Engine.SettingEngine["require"]["plugins"].Count;
			string s = "";

			for (i = 0; i < ii; ++i) {
				if (BusEngine.Engine.SettingEngine["require"]["plugins"][i]["path"] != "") {
					// https://learn.microsoft.com/ru-ru/dotnet/framework/deployment/best-practices-for-assembly-loading
					System.Reflection.Assembly plugin = System.Reflection.Assembly.LoadFile(BusEngine.Engine.SettingEngine["require"]["plugins"][i]["path"]);

					foreach (System.Type type in plugin.GetTypes()) {
						if (type.IsSubclassOf(typeof(BusEngine.Plugin))) {
							// https://learn.microsoft.com/ru-ru/dotnet/api/system.reflection.methodinfo?view=netframework-1.1
							// чтобы получить public методы без базовых(наследованных от object)
							/* System.Reflection.MethodInfo method = type.GetMethod("initialize", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly, null, type.GetGenericArguments(), null);
							BusEngine.Log.Info("Название метода {0}", method);
							i2 = method.GetParameters().Length;
							if (i2 == 0) {
								method.Invoke(System.Activator.CreateInstance(type), null);
							} else {
								method.Invoke(System.Activator.CreateInstance(type), new object[i2]);
							} */

							System.Reflection.MethodInfo[] methods = type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly);
							foreach (System.Reflection.MethodInfo method in methods) {
								if (method.Name.ToLower() == stage.ToLower()) {
									if (s == "") {
										s = type.FullName;
										Count++;
										BusEngine.Log.Info(BusEngine.Engine.SettingEngine["require"]["plugins"][i]["path"]);
										BusEngine.Log.Info(BusEngine.Localization.GetLanguage("text_name_class") + ": {0}", type.FullName);
										BusEngine.Log.Info(BusEngine.Localization.GetLanguage("text_name_method") + ": {0}", method.Name);
									}
									i2 = method.GetParameters().Length;
									if (i2 == 0) {
										method.Invoke(System.Activator.CreateInstance(type), null);
									} else {
										if (method.Name.ToLower() == "initialize") {
											for (i3 = 0; i3 < ii; ++i3) {
												if (BusEngine.ProjectDefault.Setting2["require"]["plugins"][i3]["path"]) {
													object[] x = new object[i2];
													x[0] = System.IO.Path.GetFileName(BusEngine.ProjectDefault.Setting2["require"]["plugins"][i3]["path"]);
													method.Invoke(System.Activator.CreateInstance(type), x);
												}
											}
										}
									}
								}
							}
						}
					}
				}
			}

			BusEngine.Log.Info("=============================================================================");
		}

		public void Dispose() {}
	}
	/** API BusEngine.IPlugin */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/* 
Зависит от плагинов:
BusEngine.UI.Canvas
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

		private LibVLCSharp.Shared.LibVLC _VLC;
		private LibVLCSharp.Shared.MediaPlayer _VLC_MP;
		private LibVLCSharp.WinForms.VideoView _VLC_VideoView;
		/** видео */

		/** событие остановки видео */
		public void onVideoStop(object o, object e) {
			BusEngine.Log.Info("Видео остановилось onVideoStop {0}", o.GetType());
			if (this._VLC_VideoView != null) {
				BusEngine.UI.Canvas.WinForm.Controls.Remove(this._VLC_VideoView);
				this._VLC_VideoView.MediaPlayer.Dispose();
				this._VLC_VideoView.MediaPlayer.Stopped -= this.onVideoStop;
				this._VLC_VideoView.MediaPlayer = null;
				this._VLC_VideoView = null;
				this.Dispose();
			}
			
		}
		/** событие остановки видео */

		/** функция запуска видео */
		private void VideoForm(string url = "") {
			// https://www.microsoft.com/en-us/download/details.aspx?id=6812
			// https://www.nuget.org/packages/Microsoft.DXSDK.D3DX#readme-body-tab
			// https://learn.microsoft.com/en-us/previous-versions/windows/desktop/bb318762(v=vs.85)
			//Microsoft.DirectX.AudioVideoPlayback.Audio _mediaPlayer = new Microsoft.DirectX.AudioVideoPlayback.Audio("E:\\Music\\main.mp3");
			/* Microsoft.DirectX.AudioVideoPlayback.Video _mediaPlayer = new Microsoft.DirectX.AudioVideoPlayback.Video(BusEngine.Engine.ExeDirectory + "/../../Data/Videos/BusEngine.mp4");
			_mediaPlayer.Owner = _winform.GetForm;
			_mediaPlayer.Play(); */

			// https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.media.mediaplayer?view=windowsdesktop-7.0
			/* System.Windows.Media.MediaPlayer _mediaPlayer = new System.Windows.Media.MediaPlayer();
			_mediaPlayer.Open(new System.Uri(BusEngine.Engine.ExeDirectory + "/../../Data/Videos/BusEngine.mp4"));
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
			BusEngine.UI.Canvas.WinForm.ControlsAdd(_mediaPlayer); */

			// https://code.videolan.org/videolan/LibVLCSharp/-/blob/master/samples/LibVLCSharp.WinForms.Sample/Form1.cs
			// https://github.com/videolan/libvlcsharp#quick-api-overview
			this._VLC = new LibVLCSharp.Shared.LibVLC();
			LibVLCSharp.Shared.Media media = new LibVLCSharp.Shared.Media(this._VLC, url);
			this._VLC_MP = new LibVLCSharp.Shared.MediaPlayer(media);
			media.Dispose();
			this._VLC_VideoView = new LibVLCSharp.WinForms.VideoView();
			this._VLC_VideoView.MediaPlayer = _VLC_MP;
			((System.ComponentModel.ISupportInitialize)(this._VLC_VideoView)).BeginInit();
			//SuspendLayout();
			this._VLC_VideoView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			this._VLC_VideoView.BackColor = System.Drawing.Color.Black;
			//this._VLC_VideoView.TabIndex = 1;
			//this._VLC_VideoView.MediaPlayer = null;
			//this._VLC_VideoView.Name = "VideoView";
			//this._VLC_VideoView.Text = "VideoView";
			//this._VLC_VideoView.Location = new System.Drawing.Point(0, 27);
			//this._VLC_VideoView.Size = new System.Drawing.Size(800, 444);
			this._VLC_VideoView.MediaPlayer.EnableKeyInput = true;
			// установить массив функций в дополнительных библиотеках
			this._VLC_VideoView.MediaPlayer.Stopped += this.onVideoStop;
			//this._VLC_VideoView.MediaPlayer.Stop += videoStop;
			// проверяем существование поля
			// https://learn.microsoft.com/ru-ru/dotnet/api/system.type.getfield?view=netframework-4.8
			if (typeof(BusEngine.UI.Canvas).GetField("WinForm") != null) {
				_VLC_VideoView.Size = BusEngine.UI.Canvas.WinForm.ClientSize;
				BusEngine.UI.Canvas.WinForm.Controls.Add(this._VLC_VideoView);
			}
			this._VLC_VideoView.MediaPlayer.Play();
		}

		public Video Play(string url = "") {
			BusEngine.Log.Info(url);
			if (url.IndexOf(':') == -1) {
				url = System.IO.Path.Combine(BusEngine.Engine.DataDirectory, url);
			}

			url = System.IO.Path.GetFullPath(url);

			if (System.IO.File.Exists(url)) {
				BusEngine.Log.Info(url);
				this.VideoForm(url);
				//this.VideoWpf(url);
			}

			return this;
		}
		/** функция запуска видео */

		public void Stop() {
			BusEngine.Log.Info("Видео остановилось Stop");
			if (this._VLC_VideoView != null) {
				this._VLC_VideoView.MediaPlayer.Stopped -= this.onVideoStop;
				this._VLC_VideoView.MediaPlayer.Stop();
			}
			if (this._VLC_MP != null) {
				this._VLC_MP.Stop();
			}
			this.Dispose();
		}

		/* public void Shutdown() {
			BusEngine.Log.Info("Видео остановилось Shutdown");
			this.Dispose();
		} */

		public void Dispose() {

				if (this._VLC_VideoView != null && BusEngine.UI.Canvas.WinForm != null) {
					BusEngine.UI.Canvas.WinForm.Controls.Remove(this._VLC_VideoView);
					this._VLC_VideoView.MediaPlayer.Dispose();
					this._VLC_VideoView.MediaPlayer = null;
					this._VLC_VideoView = null;
				}
				if (this._VLC_MP != null) {
					this._VLC_MP.Dispose();
					this._VLC_MP = null;
				}
				if (this._VLC != null) {
					this._VLC.Dispose();
					this._VLC = null;
				}
				BusEngine.Log.Info("Видео остановилось Dispose");

		}
	}
	/** API BusEngine.Video */
}
/** API BusEngine */

/** API BusEngine.Tools */
namespace BusEngine.Tools {
/*
Зависит от плагинов:
Newtonsoft.Json
*/
	/** API BusEngine.Tools.Ajax */
	public class Ajax : System.IDisposable {
		public delegate void BeforeSend();
		public delegate void Success(dynamic data = null, dynamic xhr = null);
		public delegate void Error(dynamic xhr = null, string textStatus = null, dynamic thrownError = null);
		public delegate void Complete(dynamic xhr = null, string textStatus = null, dynamic thrownError = null);
		//private static dynamic E { get; set; }
		//private HttpRequestException Ex { get; set; }
		private System.Net.Http.HttpResponseMessage Result { get; set; }
		public delegate void Call();
		private Call HttpClientAsync = null;

		// https://metanit.com/sharp/tutorial/2.9.php
		public Ajax(string engine = null, string url = null, string[] urlAlternative = null, string method = "POST", dynamic data = null, string responseType = "text", string dataType = "text", string headers = null, bool async = true, bool cache = false, string user = null, string password = null, BeforeSend beforeSend = null, Success success = null, Error error = null, Complete complete = null) {
			if (urlAlternative == null) {
				urlAlternative = new string[] {"https://buslikdrev.by/", "111111"};
			}
			beforeSend();
			//BusEngine.Localization.getLanguage("error_server_not")
			dynamic E = new G();

			if (url != "" && url != null) {
				try {
					if (System.Uri.IsWellFormedUriString(url, System.UriKind.Absolute)) {
						var result = System.Net.HttpWebRequest.Create(url).GetResponse();
					}
				} catch (System.Net.WebException e) {
					if (e.GetType().GetProperty("Status") != null) {
						E.Status = e.Status.ToString();
						E.StatusCode = e.ToString();
					}/*  else {
						E.Status = "Status crash";
						E.StatusCode = "StatusCode crash";
					} */
					url = "";
				}
			}

			if (urlAlternative != null && (url == "" || url == null)) {
				int i;

				for (i = 0; i < urlAlternative.Length; ++i) {
					try {
						if (System.Uri.IsWellFormedUriString(urlAlternative[i], System.UriKind.Absolute)) {
							var result = System.Net.HttpWebRequest.Create(urlAlternative[i]).GetResponse();
							if (result != null) {
								url = urlAlternative[i];
							}
						}
					} catch (System.Net.WebException e) {
						// https://docs.microsoft.com/en-us/dotnet/api/system.net.webexception?view=net-6.0
						if (e.GetType().GetProperty("Status") != null) {
							E.Status = e.Status.ToString();
							E.StatusCode = e.ToString();
						}/*  else {
							E.Status = "Status crash ";
							E.StatusCode = "StatusCode crash ";
						} */
						url = "";
					}
				}
			}

			if (url != "" && url != null) {
				// https://stackoverflow.com/questions/20530152/deciding-between-httpclient-and-webclient
				if (engine == null || engine.ToLower() != "webclient") {
					if (async) {
						HttpClientAsync = async () => {
							var baseAddress = new System.Uri(url);
							var cookieContainer = new System.Net.CookieContainer();
							using (var handler = new System.Net.Http.HttpClientHandler() {
								CookieContainer = cookieContainer
							})
							using (var client = new System.Net.Http.HttpClient(handler) {
								BaseAddress = baseAddress
							}) {
								cookieContainer.Add(baseAddress, new System.Net.Cookie("PHPSESSID", "cookie_value"));

								try {
									method = method.ToLower();
									dataType = dataType.ToLower();
									if (data != null && method == "post") {
										if (dataType == "object") {
											/* object keys = new [] {};

											var i = 0;
											UnityEngine.Debug.Log(data);
											foreach (var property in data) {
												keys[i] = new KeyValuePair<string, string>(property.Key, property.Value);
												i += 1;
											}
											UnityEngine.Debug.Log(keys);

											Result = await client.PostAsync(baseAddress, new System.Net.Http.FormUrlEncodedContent(keys)); */
										} else if (dataType == "pair" || dataType == "list") {
											Result = await client.PostAsync(baseAddress, new System.Net.Http.FormUrlEncodedContent(data));
										} else {
											Result = await client.PostAsync(baseAddress, data);
										}
									} else if (data != null && method == "put") {
										if (dataType == "object") {
											/* object keys = new [] {};

											var i = 0;
											UnityEngine.Debug.Log(data);
											foreach (var property in data) {
												keys[i] = new KeyValuePair<string, string>(property.Key, property.Value);
												i += 1;
											}
											UnityEngine.Debug.Log(keys);

											Result = await client.PutAsync(baseAddress, new System.Net.Http.FormUrlEncodedContent(keys)); */
										} else if (dataType == "pair" || dataType == "list") {
											Result = await client.PutAsync(baseAddress, new System.Net.Http.FormUrlEncodedContent(data));
										} else {
											Result = await client.PutAsync(baseAddress, data);
										}
									} else {
										Result = client.GetAsync(baseAddress).Result;
									}
									Result.EnsureSuccessStatusCode();
									if (Result.IsSuccessStatusCode) {
										//UnityEngine.Debug.Log(Result);
									}
									
									
									if (success != null && Result != null) {
										responseType = responseType.ToLower();
										if (responseType == "dictionary") {
											success(BusEngine.Tools.Json.Decode(await Result.Content.ReadAsStringAsync()), Result);
										} else if (responseType == "list") {
											
										} else if (responseType == "json") {
											success(Result.Content.ReadAsStringAsync(), Result);
										}
									}
								} catch (System.Net.Http.HttpRequestException e) {
									if (error != null && e != null) {
										if (e.GetType().GetProperty("StatusCode") != null) {
											E = e;
										}
										string textStatus = "";

										if (textStatus == "" && E.GetType().GetProperty("StatusCode") != null) {
											textStatus = E.StatusCode.ToString();
										}

										error(Result, textStatus, E);
										success = null;
									}
								} finally {


									string textStatus = "";

									if (Result != null && Result.GetType().GetProperty("StatusCode") != null) {
										textStatus = Result.StatusCode.ToString();
									}
									if (textStatus == "" && E.GetType().GetProperty("StatusCode") != null) {
										textStatus = E.StatusCode.ToString();
									}

									if (Result != null && complete != null) {
										complete(Result, textStatus, E);
									}
								}
							}
						};
						HttpClientAsync();
					} else {
						HttpClientAsync = () => {
							var baseAddress = new System.Uri(url);
							var cookieContainer = new System.Net.CookieContainer();
							using (var handler = new System.Net.Http.HttpClientHandler() {
								CookieContainer = cookieContainer
							})
							using (var client = new System.Net.Http.HttpClient(handler) {
								BaseAddress = baseAddress
							}) {
								cookieContainer.Add(baseAddress, new System.Net.Cookie("PHPSESSID", "cookie_value"));

								try {
									method = method.ToLower();
									dataType = dataType.ToLower();
									if (method == "get") {
										Result = client.GetAsync(baseAddress).Result;
									} else if (method == "put" || method == "post") {
										if (dataType == "object") {

											Result = client.PutAsync(baseAddress, new System.Net.Http.FormUrlEncodedContent(data)).Result;
										} else if (dataType == "pair" || dataType == "list") {
											Result = client.PutAsync(baseAddress, new System.Net.Http.FormUrlEncodedContent(data)).Result;
										} else {
											Result = client.PutAsync(baseAddress, data).Result;
										}
									} else {
										Result = client.GetAsync(baseAddress).Result;
									}
									Result.EnsureSuccessStatusCode();
									if (Result.IsSuccessStatusCode) {
										//UnityEngine.Debug.Log(Result);
									}
								} catch (System.Net.Http.HttpRequestException e) {
									if (error != null && e != null) {
										if (e.GetType().GetProperty("StatusCode") != null) {
											E = e;
										}
										string textStatus = "";

										if (textStatus == "" && E.GetType().GetProperty("StatusCode") != null) {
											textStatus = E.StatusCode.ToString();
										}

										error(Result, textStatus, E);
										success = null;
									}
								} finally {
									if (success != null && Result != null) {
										responseType = responseType.ToLower();
										if (responseType == "dictionary") {
											success(BusEngine.Tools.Json.Decode(Result.Content.ReadAsStringAsync().Result), Result);
										} else if (responseType == "list") {
											
										} else if (responseType == "json") {
											
										}
									}

									string textStatus = "";

									if (Result != null && Result.GetType().GetProperty("StatusCode") != null) {
										textStatus = Result.StatusCode.ToString();
									}
									if (textStatus == "" && E.GetType().GetProperty("StatusCode") != null) {
										textStatus = E.StatusCode.ToString();
									}

									if (complete != null) {
										complete(Result, textStatus, E);
									}
								}
							}
						};
						HttpClientAsync();
					}
				} else {
					if (async) {

					} else {

					}
				}
			} else {
				if (error != null) {
					string textStatus = "";

					if (Result != null && Result.GetType().GetProperty("StatusCode") != null) {
						textStatus = Result.StatusCode.ToString();
					}
					if (textStatus == "" && E.GetType().GetProperty("StatusCode") != null) {
						textStatus = E.StatusCode.ToString();
					}

					error(Result, textStatus, E);
					if (complete != null) {
						complete(Result, textStatus, E);
					}
				}
			}
		}

		public static bool Test(string url = "https://buslikdrev.by/") {
			bool status = false;

			new BusEngine.Tools.Ajax(
				url: url,
				async: false,
				dataType: "pair",
				responseType: "dictionary",
				beforeSend: () => {
					System.Console.WriteLine("beforeSend");
				},
				data: new System.Collections.Generic.Dictionary<string, string>() {
					{"user", "user1"},
					{"pass", "pass1"},
				},
				success: (dynamic data, dynamic xhr) => {
					//data https://docs.microsoft.com/ru-ru/dotnet/api/system.net.http.httpresponsemessage.Content
					//xhr https://docs.microsoft.com/ru-ru/dotnet/api/system.net.http.httpresponsemessage

					status = true;
					System.Console.WriteLine("success");
				},
				error: (dynamic xhr, string textStatus, dynamic thrownError) => {
					//xhr https://docs.microsoft.com/ru-ru/dotnet/api/system.net.http.httpresponsemessage
					//textStatus request server;
					//thrownError https://docs.microsoft.com/ru-ru/dotnet/api/system.net.http.httpresponsemessage.ensuresuccessstatuscode
					//UnityEngine.Debug.Log(thrownError.GetType());

					System.Console.WriteLine("Login.message.error", thrownError.StatusCode);
				}
			);

			return status;
		}

		public static void Shutdown() {}

		public void Dispose() {}
	}

	// заглушка
	internal class G {
		//public virtual System.Collections.IDictionary Data { get; }
		//public virtual string? HelpLink { get; set; }
		//public int HResult { get; set; }
		//public Exception? InnerException { get; }
		//public virtual string Message { get; }
		//public virtual string? Source { get; set; }
		//public virtual string? StackTrace { get; }
		////public System.Net.WebExceptionStatus Status { get; }
		////public System.Net.HttpStatusCode? StatusCode { get; }
		public string Status { get; set; }
		public string StatusCode { get; set; }
		//public System.Reflection.MethodBase? TargetSite { get; }

		public G(string text = "") {
			StatusCode = text;
			Status = text;
		}
	}
}
/** API BusEngine.Tools */

/** API BusEngine.Tools */
namespace BusEngine.Tools {
/*
Зависит от плагинов:
Newtonsoft.Json
*/
	/** API BusEngine.Tools.Json */
	//https://learn.microsoft.com/en-us/dotnet/standard/serialization/system-text-json/migrate-from-newtonsoft?pivots=dotnet-7-0
	//https://www.nuget.org/packages/System.Text.Json#readme-body-tab
	public class Json : System.IDisposable {
		// System.Type|object|string|int|Dictionary|List c#
		public static string Encode(object t) {
			try {
				return Newtonsoft.Json.JsonConvert.SerializeObject(t, Newtonsoft.Json.Formatting.Indented);
			} catch (System.Exception e) {
				BusEngine.Log.Info(BusEngine.Localization.GetLanguage("error") + " " + BusEngine.Localization.GetLanguage("error_json_encode") + ": {0}", e.Message);
				return "[]";
			}
		}

		// массив php
		public static System.Collections.Generic.Dictionary<string, dynamic> Decode(string t) {
			try {
				return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, dynamic>>(t);
				//return Newtonsoft.Json.JsonConvert.DeserializeObject(t);
			} catch (System.Exception e) {
				BusEngine.Log.Info(BusEngine.Localization.GetLanguage("error") + " " + BusEngine.Localization.GetLanguage("error_json_decode") + ": {0}", e.Message);
				return new System.Collections.Generic.Dictionary<string, dynamic>();
			}
		}

		// object c#
		public static object Decode(string t, bool o = true) {
			try {
				return Newtonsoft.Json.JsonConvert.DeserializeObject(t);
			} catch (System.Exception e) {
				BusEngine.Log.Info(BusEngine.Localization.GetLanguage("error") + " " + BusEngine.Localization.GetLanguage("error_json_decode") + ": {0}", e.Message);
				return new {};
			}
		}

		public static void Shutdown() {}

		public void Dispose() {}
	}
	/** API BusEngine.Tools.Json */
}
/** API BusEngine.Tools */

/** API BusEngine.UI */
namespace BusEngine.UI {
/*
Зависит от плагинов:
BusEngine.UI
*/
	/** API BusEngine.UI.Canvas */
	public class Canvas : System.IDisposable {
		#if BUSENGINE_WINFORMS
		public static System.Windows.Forms.Form WinForm;
		#else
		public static System.Windows.Forms.Form WPF;
		//public static BusEngine.UI.Canvas Canvas;
		#endif

		/** событие уничтожения окна */
		private void OnDisposed(object o, System.EventArgs e) {

		}
		/** событие уничтожения окна */

		/** событие закрытия окна */
		private void OnClosed(object o, System.Windows.Forms.FormClosedEventArgs e) {
			BusEngine.UI.Canvas.WinForm.FormClosed -= OnClosed;
			//BusEngine.Video.Shutdown();
			BusEngine.Engine.Shutdown();
		}
		/** событие закрытия окна */

		private static Canvas _canvas;

		public Canvas() {
			if (typeof(BusEngine.UI.Canvas).GetField("WinForm") != null) {
				BusEngine.UI.Canvas.WinForm.KeyPreview = true;
				// устанавливаем событи закрытия окна
				BusEngine.UI.Canvas.WinForm.FormClosed += OnClosed;
				BusEngine.UI.Canvas.WinForm.Disposed += new System.EventHandler(OnDisposed);
				//BusEngine.UI.ClientSize = BusEngine.UI.ClientSize;
			}
		}

		public Canvas(System.Windows.Forms.Form _form) {
			//#if (BUSENGINE_WINFORM == true)
			//if (typeof(BusEngine.UI.Canvas).GetField("WinForm") != null) {
				if (_form != null) {
					BusEngine.UI.Canvas.WinForm = _form;
				}
				BusEngine.UI.Canvas.WinForm.KeyPreview = true;
				// устанавливаем событи закрытия окна
				BusEngine.UI.Canvas.WinForm.FormClosed += OnClosed;
				BusEngine.UI.Canvas.WinForm.Disposed += new System.EventHandler(OnDisposed);
				//BusEngine.UI.ClientSize = BusEngine.UI.ClientSize;
			//}
			//#endif
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0117:", Target="~T:BusEngine.UI.Canvas")]
		public static void Initialize() {
			if (_canvas == null) {
				_canvas = new Canvas();
				// инициализируем плагины
				new BusEngine.IPlugin("InitializeСanvas");
			}
		}

		public static void Shutdown() {}

		public void Dispose() {}
	}
	/** API BusEngine.UI.Canvas */
}
/** API BusEngine.UI */