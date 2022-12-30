/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

/* C# 6.0+              https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history */
/* NET.Framework 4.6.1+ https://learn.microsoft.com/ru-ru/dotnet/framework/migration-guide/versions-and-dependencies */
/* MSBuild 14.0+        https://en.wikipedia.org/wiki/MSBuild#Versions */
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
- добавить поддержку форматов .dae https://docs.fileformat.com/ru/3d/dae/, .png, .mtl, .obj
- написать сборку игры для windows 7+ и Android 5+
*/

#define BUSENGINE
/** API BusEngine */
namespace BusEngine {
/*
Зависимости нет
*/
	/** API BusEngine.ProjectSettingDefault */
	internal class ProjectSettingDefault {
		public static object console_commands;
		public static object console_variables;
		public static string version;
		public static string type;
		public static object info;
		public static object content;
		public static object require;
		public static BusEngine.ProjectSettingDefault Setting;

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

			BusEngine.ProjectSettingDefault.Setting = this;
		}
	}
	/** API BusEngine.ProjectSettingDefault */
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
BusEngine.UI.Canvas
*/
	/** API BusEngine.Browser */
	public class Browser : System.IDisposable {
		//private Browser() {}
		//private static Browser _browser;
		public delegate void PostMessageHandler(object sender, CefSharp.JavascriptMessageReceivedEventArgs e);
		//public static event PostMessageHandler PostMessage;
		public static event System.EventHandler<CefSharp.JavascriptMessageReceivedEventArgs> PostMessage;

		/** событие клика из браузера */
		private static void OnBrowserClick(object o, object e) {
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
		// https://cefsharp.github.io/api/
		public static void Start(string Url = "") {
			// если ссылка не обсалютный адрес, то делаем его обсалютным
			if (Url.IndexOf(':') == -1) {
				if (System.IO.File.Exists(System.IO.Path.Combine(BusEngine.Engine.DataDirectory, Url))) {
					Url = "https://BusEngine/" + Url;
				} else {
					Url = "https://buslikdrev.by/";
				}
			}

			//CefSharp.BrowserSubprocess.SelfHost.Main(args);

			// подгружаем объект настроек CefSharp по умолчанияю, чтобы внести свои правки
			CefSharp.WinForms.CefSettings settings = new CefSharp.WinForms.CefSettings();

			// включаем поддержку экранов с высоким разрешением
			CefSharp.Cef.EnableHighDPISupport();

			// устанавливаем свой юзер агент
			settings.UserAgent = "Mozilla/5.0 (Windows NT 6.1; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) BusEngine/0.2.0 Safari/537.36";
			//settings.UserAgent = settings.UserAgent.Replace("Chrome", "BusEngine");

			// отключаем создание файла лога
			settings.LogSeverity = CefSharp.LogSeverity.Disable;

			// https://github.com/cefsharp/CefSharp/wiki/General-Usage#scheme-handler
			// регистрируем свою схему
			settings.RegisterScheme(new CefSharp.CefCustomScheme {
				SchemeName = "https",
				DomainName = "BusEngine",
				SchemeHandlerFactory = new CefSharp.SchemeHandler.FolderSchemeHandlerFactory (
					rootFolder: BusEngine.Engine.DataDirectory,
					hostName: "BusEngine",
					defaultPage: "index.html"
				)
			});

			// применяем наши настройки до запуска браузера
			CefSharp.Cef.Initialize(settings);

			// запускаем браузер
			CefSharp.WinForms.ChromiumWebBrowser _browser = new CefSharp.WinForms.ChromiumWebBrowser(Url);

			// просто подключаем левое событие - можно удалить
			_browser.KeyDown += BusEngine.Browser.OnKeyDown;

			// https://stackoverflow.com/questions/51259813/call-c-sharp-function-from-javascript-using-cefsharp-in-windows-form-app
			// подключаем событие сообщения из javascript
			_browser.JavascriptMessageReceived += OnPostMessage;
			_browser.JavascriptMessageReceived += PostMessage;

			// устанавливаем размер окана браузера, как в нашей программе
			//_browser.Size = BusEngine.UI.Canvas.WinForm.ClientSize;
			//_browser.Dock = BusEngine.UI.Canvas.WinForm.Dock;
			// подключаем браузер к нашей программе
			BusEngine.UI.Canvas.WinForm.Controls.Add(_browser);
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
		public static string Platform;

		//private static BusEngine.UI.Canvas _canvas;
		// MSBuild v12.0
		public static void GenerateStatLink() {
			// включаем консоль
			BusEngine.Log.ConsoleShow();

			// устанавливаем данные проекта по умолчанию
			BusEngine.ProjectSettingDefault _setting_project = new BusEngine.ProjectSettingDefault();

			// https://metanit.com/sharp/tutorial/5.4.php
			// https://metanit.com/sharp/tutorial/6.4.php
			// https://dir.by/developer/csharp/serialization_json/?lang=eng
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
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(BusEngine.Tools.Json.SerializeObject(_setting_project));
					fstream.Write(buffer, 0, buffer.Length);
				}
			} else {
				BusEngine.Log.Info(_files[0]);

				// запись
				/* using (System.IO.StreamWriter fstream = new System.IO.StreamWriter(_files[0], false, System.Text.Encoding.UTF8)) {
					fstream.Write(BusEngine.Tools.Json.SerializeObject(_setting_project));
				} */

				// запись
				using (System.IO.FileStream fstream = System.IO.File.OpenWrite(_files[0])) {
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(BusEngine.Tools.Json.SerializeObject(BusEngine.Tools.Json.DeserializeObject(BusEngine.Tools.Json.SerializeObject(_setting_project))));

					fstream.Write(buffer, 0, buffer.Length);
				}

				// запись
				/* using (System.IO.FileStream fstream = new System.IO.FileStream(_files[0], System.IO.FileMode.OpenOrCreate)) {
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(BusEngine.Tools.Json.SerializeObject(_setting_project));
					fstream.WriteAsync(buffer, 0, buffer.Length);
				} */

				// запись
				//System.IO.File.WriteAllText(_files[0], BusEngine.Tools.Json.SerializeObject(_setting_project));

				// чтение
				/* using (System.IO.FileStream fstream = new System.IO.FileStream(_files[0], System.IO.FileMode.OpenOrCreate)) {
					byte[] buffer = new byte[fstream.Length];
					fstream.ReadAsync(buffer, 0, buffer.Length);
					// декодируем байты в строку
					BusEngine.Tools.Json.DeserializeObject(System.Text.Encoding.UTF8.GetString(buffer));
				} */

				// чтение
				//BusEngine.Tools.Json.DeserializeObject(System.IO.File.ReadAllText(_files[0]));
			}

			// тестирование плагина - прогонка кода
			//Newtonsoft.Json.JsonConvert.DeserializeObject(Newtonsoft.Json.JsonConvert.SerializeObject(new BusEngine.ProjectSettingDefault(), Newtonsoft.Json.Formatting.Indented));
			BusEngine.Tools.Json.DeserializeObject(BusEngine.Tools.Json.SerializeObject(_setting_project));

			_files = System.IO.Directory.GetFiles(_path, "busengine.busengine");

			if (_files.Length == 0) {
				BusEngine.Log.Info(_files.Length);

				// запись
				using (System.IO.FileStream fstream = System.IO.File.OpenWrite(_path + "busengine.busengine")) {
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(BusEngine.Tools.Json.SerializeObject(_setting_project));
					fstream.Write(buffer, 0, buffer.Length);
				}
			} else {
				BusEngine.Log.Info(_files[0]);

				using (System.IO.FileStream fstream = new System.IO.FileStream(_files[0], System.IO.FileMode.OpenOrCreate)) {
					byte[] buffer = new byte[fstream.Length];
					fstream.ReadAsync(buffer, 0, buffer.Length);
					// декодируем байты в строку
					object _ProjectSettingDefault = BusEngine.Tools.Json.DeserializeObject(System.Text.Encoding.UTF8.GetString(buffer));
					//BusEngine.Log.Info(_setting_project.content.assets);
					//BusEngine.ProjectSettingDefault.content = _ProjectSettingDefault.content;
				}
			}

			BusEngine.Engine.DataDirectory = _path + "Data/";










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
		public bool IncludeInactive = false;
		//[Tooltip("Replace Resources.load with Bundle.load?")]
		private bool BundleStatus = false;
		public delegate void Call();
		private Call CallbackStart = null;
		private static System.Collections.Generic.Dictionary<string, string> GetLanguages = new System.Collections.Generic.Dictionary<string, string>();

		public static string GetLanguage(string key) {
			if (GetLanguages.ContainsKey(key)) {
				return GetLanguages[key];
			} else {
				return key;
			}
		}

		/* public Localization() {
			language = language;
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

		public static bool CallBack(Call callback = null) {
			if (callback != null) {
				Call CallbackStart = callback;
			}
			
			return false;
		}

		public void Start() {
			StartLocalization(Language);
		}

		public void Load(string Language = null) {
			StartLocalization(Language);
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
			string Path, Platform, Files;

			Files = "";
			if (Language == null || Language == "") {
				Language = System.Globalization.CultureInfo.CurrentCulture.EnglishName.ToString();
			}
			// https://docs.unity3d.com/ScriptReference/RuntimePlatform.html
			Platform = BusEngine.Engine.Platform.ToString();
			//Platform = "WebGLPlayer";

			if (Platform == "WindowsEditor" || Platform == "WindowsPlayer") {
				Path = BusEngine.Engine.DataDirectory + "../Localization/";
				if (!System.IO.Directory.Exists(Path)) {
					Path = BusEngine.Engine.DataDirectory + "/Localization/";
				}
				if (!System.IO.Directory.Exists(Path)) {
					Path = BusEngine.Engine.DataDirectory + "/Resources/Localization/";
				}
				if (!System.IO.Directory.Exists(Path)) {
					Path = BusEngine.Engine.DataDirectory + "/Localization/";
				}
			} else {
				if (Platform == "WebGLPlayer" && !BundleStatus) {
					Path = "Localization/";
				} else {
					// https://docs.unity3d.com/Manual/StreamingAssets.html
					// https://docs.unity3d.com/ScriptReference/Application-streamingAssetsPath.html
					Path = BusEngine.Engine.DataDirectory + "/Localization/";
				}
			}

			if (Platform == "WebGLPlayer") {
				// https://learn.microsoft.com/en-us/visualstudio/msbuild/common-msbuild-project-items?view=vs-2022#embeddedresource
				// https://learn.microsoft.com/en-us/xamarin/xamarin-forms/data-cloud/data/files?tabs=windows
				/* if (BundleStatus) {
					//AssetBundle bundle = myLoadedAssetBundle = AssetBundle.LoadFromFile(Path + Language + File + "." + format);
					//TextAsset resources = bundle.Load<TextAsset>(File + "." + format);
				} else {
					// https://docs.unity3d.com/2022.2/Documentation/Manual/class-TextAsset.html
					TextAsset resources = Resources.Load(Path + Language + File, typeof(TextAsset)) as TextAsset;
					if (resources != null) {
						Files = resources.text;
						//Files = System.Text.Encoding.UTF8.GetString(resources.bytes);
						//Resources.UnloadAsset(resources);
					} else {
						Language = LanguageDefault;
						resources = Resources.Load(Path + Language + File, typeof(TextAsset)) as TextAsset;
						if (resources != null) {
							Files = resources.text;
							//Files = System.Text.Encoding.UTF8.GetString(resources.bytes);
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
				Files = StartCoroutine(GetText("https://buslikdrev.by/game/StreamingAssets/Localization/Belarusian.txt")).ToString();
				UnityEngine.Debug.Log(Files); */
			} else {
				if (System.IO.File.Exists(Path + Language + File + "." + Format)) {
					Files = System.IO.File.ReadAllText(Path + Language + File + "." + Format);
					//byte[] Bytes = File.ReadAllBytes(Path + Language + file + "." + Format);
					//Files = System.Text.Encoding.UTF8.GetString(Bytes);
				} else {
					Language = LanguageDefault;
					if (System.IO.File.Exists(Path + Language + File + "." + Format)) {
						Files = System.IO.File.ReadAllText(Path + Language + File + "." + Format);
						//byte[] Bytes = System.IO.File.ReadAllBytes(Path + Language + File + "." + Format);
						//Files = System.Text.Encoding.UTF8.GetString(Bytes);
					}
				}
			}

			GetLanguages["text_debug"] = "" + Path + Language + File + "." + Format + "";

			if (Files != "") {
				GetLanguages["text_debug"] = Files;
				string[] lines, pairs;
				int i;

				lines = Files.Split(new string[] {"\r\n", "\n\r", "\n"}, System.StringSplitOptions.RemoveEmptyEntries);
				Files = null;
				System.GC.Collect();

				for (i = 0; i < lines.Length; ++i) {
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

		public static void Shutdown() {

		}

		public void Dispose() {

		}
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

		// функция запуска консоли
		public static void ConsoleShow() {
			if (BusEngine.Log.StatusConsole == false) {
				/* System.Windows.Forms.MessageBox.Show("Сообщение из Windows Forms!"); */

				BusEngine.Log.AttachConsole(-1);
				BusEngine.Log.AllocConsole();
				BusEngine.Log.StatusConsole = true;

				System.Console.CancelKeyPress += new System.ConsoleCancelEventHandler(BusEngine.Log.MyHandler);

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
			BusEngine.UI.Canvas.WinForm.ControlsAdd(_mediaPlayer); */

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
			// проверяем существование поля
			// https://learn.microsoft.com/ru-ru/dotnet/api/system.type.getfield?view=netframework-4.8
			if (typeof(BusEngine.UI.Canvas).GetField("WinForm") != null) {
				_VLC_VideoView.Size = BusEngine.UI.Canvas.WinForm.ClientSize;
				BusEngine.UI.Canvas.WinForm.Controls.Add(_VLC_VideoView);
			}
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
				BusEngine.UI.Canvas.WinForm.Controls.Remove(_VLC_VideoView);
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
		public Ajax(string engine = null, string url = null, string[] urlAlternative = null, string metod = "POST", dynamic data = null, string responseType = "text", string dataType = "text", string headers = null, bool async = true, bool cache = false, string user = null, string password = null, BeforeSend beforeSend = null, Success success = null, Error error = null, Complete complete = null) {
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
									metod = metod.ToLower();
									dataType = dataType.ToLower();
									if (data != null && metod == "post") {
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
									} else if (data != null && metod == "put") {
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
											success(BusEngine.Tools.Json.DeserializeObject(await Result.Content.ReadAsStringAsync()), Result);
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
									metod = metod.ToLower();
									dataType = dataType.ToLower();
									if (metod == "get") {
										Result = client.GetAsync(baseAddress).Result;
									} else if (metod == "put" || metod == "post") {
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
											success(BusEngine.Tools.Json.DeserializeObject(Result.Content.ReadAsStringAsync().Result), Result);
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
				/* data: new[] {
					new KeyValuePair<string, string>("user", "user1"),
					new KeyValuePair<string, string>("pass", "pass1"),
				}, */
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

		public static void Shutdown() {

		}

		public void Dispose() {

		}
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
	public class Json : System.IDisposable {
		public static string SerializeObject(object t) {
			return Newtonsoft.Json.JsonConvert.SerializeObject(t, Newtonsoft.Json.Formatting.Indented);
		}

		public static object DeserializeObject(string t) {
			return Newtonsoft.Json.JsonConvert.DeserializeObject(t);
		}

		public static void Shutdown() {
			
		}

		public void Dispose() {
			
		}
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
		#if (BUSENGINE_WINFORM == true)
		public static System.Windows.Forms.Form WinForm;
		#else
		public static System.Windows.Forms.Form WinForm;
		//public static BusEngine.UI.Canvas Canvas;
		#endif

		/** событие нажатия любой кнопки */
		// https://learn.microsoft.com/en-us/dotnet/api/system.consolekey?view=netframework-4.8
		private void OnKeyDown(object o, System.Windows.Forms.KeyEventArgs e) {
			BusEngine.Log.Info("клавиатура клик");
			BusEngine.Log.Info();
			// Выключаем движок по нажатию на Esc
			if (e.KeyCode == System.Windows.Forms.Keys.Escape) {
				#if (BUSENGINE_WINFORM == true)
				BusEngine.UI.Canvas.WinForm.KeyDown -= new System.Windows.Forms.KeyEventHandler(OnKeyDown);
				#endif
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

		public static Canvas _canvas;

		public Canvas() {
			if (typeof(BusEngine.UI.Canvas).GetField("WinForm") != null) {
				BusEngine.UI.Canvas.WinForm.KeyPreview = true;
				BusEngine.UI.Canvas.WinForm.KeyDown += OnKeyDown;
				// устанавливаем событи закрытия окна
				BusEngine.UI.Canvas.WinForm.FormClosed += OnClosed;
				BusEngine.UI.Canvas.WinForm.Disposed += new System.EventHandler(OnDisposed);
				//BusEngine.UI.ClientSize = BusEngine.UI.ClientSize;
				System.Console.WriteLine("Консоль BusEngine4444444444444444444444");
			}
		}

		public Canvas(System.Windows.Forms.Form _form) {
			//#if (BUSENGINE_WINFORM == true)
			//if (typeof(BusEngine.UI.Canvas).GetField("WinForm") != null) {
				if (_form != null) {
					BusEngine.UI.Canvas.WinForm = _form;
				}
				BusEngine.UI.Canvas.WinForm.KeyPreview = true;
				BusEngine.UI.Canvas.WinForm.KeyDown += OnKeyDown;
				// устанавливаем событи закрытия окна
				BusEngine.UI.Canvas.WinForm.FormClosed += OnClosed;
				BusEngine.UI.Canvas.WinForm.Disposed += new System.EventHandler(OnDisposed);
				//BusEngine.UI.ClientSize = BusEngine.UI.ClientSize;
				System.Console.WriteLine("Консоль BusEngine4444444444444444444444");
			//}
			//#endif
		}

		public static void Initialize() {
			if (_canvas == null) {
				_canvas = new Canvas();
			}
		}

		[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0117:", Target="~T:BusEngine.UI.Canvas")]
		public static void Initialize(System.Windows.Forms.Form _form) {
			if (_canvas == null) {
				//#pragma error disable//, CS0117
				if (1 == 2) {
				//BusEngine.UI.Canvas.WinForm = _form;
				}
				_canvas = new Canvas(_form);
			}
		}

		public static void Shutdown() {

		}

		public void Dispose() {
			//BusEngine.Log.ConsoleHide();
		}
	}
	/** API BusEngine.UI.Canvas */
}
/** API BusEngine.UI */