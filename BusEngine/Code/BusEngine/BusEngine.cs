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
https://learn.microsoft.com/ru-ru/dotnet/standard/collections/thread-safe/
*/

/** дорожная карта
- проставить нормально модификаторы доступа https://metanit.com/sharp/tutorial/3.2.php
- максимально весь функционал сделать независимыми плагинами и установить проверки
 на наличие плагинов перед их использованием
- создать: генерацию сцены (карты), камеру, консольные команды, консоль, настройка проекта
- добавить поддержку форматов .dae https://docs.fileformat.com/ru/3d/dae/, .png, .mtl, .obj
- написать сборку игры для windows 7+ и Android 5+
- сделать максимально под ООП (инициализировать через new, чтобы можно было в случае event 
получить, уничтожить этот же объект, если он не нужен, можно установить статические свойства, 
методы и события которые выполняют отдельную работу от объекта или имеют постоянные данные) 
те объекты которые можно загружать несколько раз. Если объект можно загрузить 1 раз, 
то можно static с проверкой на null.
- сторонние библиотеки обвернуть в исключения try catch - нужно от них ожидать только ошибки.
- наладить многопоточность - потокобезопасность.
*/

//#define AUDIO_LOG
//#define BROWSER_LOG
//#define LOG_TYPE
//#define VIDEO_LOG
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
				name = (System.Reflection.Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(System.Reflection.AssemblyTitleAttribute), false)[0] as System.Reflection.AssemblyTitleAttribute).Title,
				guid = "ddc2049b-3a86-425b-9713-ee1babec5365"
			},
			content = new {
				assets = new string[] {"Assets"},
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
Зависимости нет
*/
	/** API BusEngine.TooltipAttribute */
	// https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes/creating-custom-attributes
	[System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Property | System.AttributeTargets.Field | System.AttributeTargets.Assembly)]
	public class TooltipAttribute : System.Attribute {
		private string Name;
		private string Language;

		public TooltipAttribute(string name) {
			BusEngine.Log.Info("TooltipAttribute: {0}", name);  
		}
		public TooltipAttribute(string name, string language) {
			BusEngine.Log.Info("TooltipAttribute: {0} {1}", name, language);  
		}
	}
	/** API BusEngine.TooltipAttribute */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.Log
*/
	/** API BusEngine.Audio */
	public class Audio : System.IDisposable {
		/** aудио */
		private LibVLCSharp.Shared.LibVLC _VLC;
		private LibVLCSharp.Shared.MediaPlayer _mediaPlayer;
		/** aудио */

		// события и его дилегат
		public delegate void AudioHandler(BusEngine.Audio sender, string url);

		public event AudioHandler OnPlay;
		public event AudioHandler OnLoop;
		public event AudioHandler OnPause;
		public event AudioHandler OnStop;
		public event AudioHandler OnEnd;
		public event AudioHandler OnDispose;
		public event AudioHandler OnNotFound;

		// состояния
		public bool IsPlay { get; private set; }
		public bool IsPause { get; private set; }
		public bool IsStop { get; private set; }
		public bool IsEnd { get; private set; }
		public bool IsDispose { get; private set; }
		/* private bool IsPlay;
		private bool IsPause;
		private bool IsStop;
		private bool IsEnd;
		private bool IsDispose; */

		// список ссылок
		public string[] Urls;
		private string[] UrlsArray;
		// ссылка
		public string Url = "";
		// вкл\выкл повтор воспроизведения
		public bool Loop = false;
		// громкость звука от 0 до 100
		public byte Volume = 100;
		// баланс колонок от - 100 до 100
		public sbyte Balance = 0;
		// вкл\выкл звука
		public bool Mute = false;
		// длина файла секунды
		public double Duration { get; private set; }
		// текущая позиция секунды
		public double Position {
			get {
				return (_mediaPlayer != null ? _mediaPlayer.Time : 0);
			} set {}
		}
		// авто удаление объектов по времени
		public double DisposeAuto = 500;

		/** событие запуска aудио */
		private void OnPlaying(object o, object e) {
			#if AUDIO_LOG
			BusEngine.Log.Info("Аудио OnPlay {0}", this.Duration);
			#endif

			this.IsPlay = true;
			this.IsStop = false;
			this.IsDispose = false;

			if (this.OnPlay != null) {
				//this.OnPlay.Invoke(this, this.Url);
				BusEngine.UI.Canvas.WinForm.Invoke(this.OnPlay, new object[2] {this, this.Url});
			}
		}
		/** событие запуска aудио */

		/** событие повтора aудио */
		private void OnLooping(object o, object e) {
			#if AUDIO_LOG
			BusEngine.Log.Info("Аудио OnLoop {0}", this.Duration);
			#endif

			this.Play(this.Url);
			if (this.OnLoop != null) {
				//this.OnLoop.Invoke(this, this.Url);
				BusEngine.UI.Canvas.WinForm.Invoke(this.OnLoop, new object[2] {this, this.Url});
			}
		}
		/** событие повтора aудио */

		/** событие временной остановки aудио */
		private void OnPausing(object o, object e) {
			#if AUDIO_LOG
			BusEngine.Log.Info("Аудио OnPause {0}", this.Position);
			#endif

			if (this.OnPause != null) {
				//this.OnPause.Invoke(this, this.Url);
				BusEngine.UI.Canvas.WinForm.Invoke(this.OnPause, new object[2] {this, this.Url});
			}
		}
		/** событие временной остановки aудио */

		/** событие ручной остановки aудио */
		private void OnStopping(object o, object e) {
			#if AUDIO_LOG
			BusEngine.Log.Info("Аудио OnStop {0}", this.Position);
			#endif

			this.IsPlay = false;

			if (this.OnStop != null) {
				//this.OnStop.Invoke(this, this.Url);
				BusEngine.UI.Canvas.WinForm.Invoke(this.OnStop, new object[2] {this, this.Url});
			}

			if (this.DisposeAuto > 0) {
				if (this.DisposeAuto < 100) {
					this.DisposeAuto = 100;
				}
				this.Dispose();
			}
		}
		/** событие ручной остановки aудио */

		/** событие автоматической остановки aудио */
		private void OnEnding(object o, object e) {
			#if AUDIO_LOG
			BusEngine.Log.Info("Аудио OnEnd {0}", this.Position);
			#endif

			this.IsEnd = true;
			this.IsPlay = false;

			if (this.OnEnd != null) {
				//this.OnEnd.Invoke(this, this.Url);
				BusEngine.UI.Canvas.WinForm.Invoke(this.OnEnd, new object[2] {this, this.Url});
			}

			if (this.DisposeAuto > 0) {
				if (this.DisposeAuto < 100) {
					this.DisposeAuto = 100;
				}
				this.Dispose();
			}
		}
		/** событие автоматической остановки aудио */

		/** событие уничтожения aудио */
		private void OnDisposing(object o, object e) {
			#if AUDIO_LOG
			BusEngine.Log.Info("Аудио OnDispose");
			#endif

			if (this.OnDispose != null) {
				//this.OnDispose.Invoke(this, this.Url);
				BusEngine.UI.Canvas.WinForm.Invoke(this.OnDispose, new object[2] {this, this.Url});
			}
		}
		/** событие уничтожения aудио */

		/** функция запуска aудио */
		public Audio() {
			#if AUDIO_LOG
			_VLC = new LibVLCSharp.Shared.LibVLC(false, new[] { "--verbose=2" });
			#else
			_VLC = new LibVLCSharp.Shared.LibVLC(false);
			#endif
			/* _VLC.Log += (o, e) => {
				BusEngine.Log.Info("1 Log 1 {0}", e.Message);
			}; */
			_VLC.CloseLogFile();
			_VLC.ClearLibVLCError();
			_VLC.SetUserAgent(BusEngine.Engine.SettingEngine["info"]["name"], BusEngine.Engine.Device.UserAgent);
			_mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_VLC);

			_mediaPlayer.Playing += this.OnPlaying;
			if (this.Loop) {
				_mediaPlayer.EndReached += this.OnLooping;
			}
			_mediaPlayer.Paused += this.OnPausing;
			_mediaPlayer.Stopped += this.OnStopping;
			//_mediaPlayer.Disposed += this.OnDisposing;
			_mediaPlayer.EndReached += this.OnEnding;
		}
		public Audio(string url = "") : this() {
			this.Url = url;
			this.Urls = new string[1] {url};
		}
		public Audio(string[] urls) : this() {
			if (urls.Length > 0) {
				this.Urls = urls;
				this.UrlsArray = urls;
				this.Url = urls[0];
				this.OnStop += (BusEngine.Audio a, string url) => {
					#if AUDIO_LOG
					BusEngine.Log.Info("Audio OnStopAudio: {0}", url);
					BusEngine.Log.Info("Audio OnStopAudio: {0}", a.Url);
					#endif

					if (this.UrlsArray.Length > 0) {
						System.Array.Reverse(this.UrlsArray);
						System.Array.Resize(ref this.UrlsArray, this.UrlsArray.Length - 1);
						System.Array.Reverse(this.UrlsArray);
					}

					if (this.UrlsArray.Length > 0) {
						this.Play(this.UrlsArray[0]);
					}
				};
				this.OnEnd += (BusEngine.Audio a, string url) => {
					#if AUDIO_LOG
					BusEngine.Log.Info("Audio OnStopAudio: {0}", url);
					BusEngine.Log.Info("Audio OnStopAudio: {0}", a.Url);
					#endif

					if (this.UrlsArray.Length > 0) {
						System.Array.Reverse(this.UrlsArray);
						System.Array.Resize(ref this.UrlsArray, this.UrlsArray.Length - 1);
						System.Array.Reverse(this.UrlsArray);
					}

					if (this.UrlsArray.Length > 0) {
						this.Play(UrlsArray[0]);
					}
				};
				this.OnNotFound += (BusEngine.Audio a, string url) => {
					#if AUDIO_LOG
					BusEngine.Log.Info("Audio OnStopAudio: {0}", url);
					BusEngine.Log.Info("Audio OnStopAudio: {0}", a.Url);
					#endif

					if (this.UrlsArray.Length > 0) {
						System.Array.Reverse(this.UrlsArray);
						System.Array.Resize(ref this.UrlsArray, this.UrlsArray.Length - 1);
						System.Array.Reverse(this.UrlsArray);
					}

					if (this.UrlsArray.Length > 0) {
						this.Play(UrlsArray[0]);
					}
				};
			}
		}
		public BusEngine.Audio Play() {
			return this.Play(this.Url);
		}
		public BusEngine.Audio Play(string url = "") {
			if (this.IsPlay) {
				return this;
			}
			this.Url = url;

			#if AUDIO_LOG
			BusEngine.Log.Info("Аудио Play()");
			BusEngine.Log.Info(url);
			#endif

			if (url.IndexOf(':') == -1) {
				url = System.IO.Path.Combine(BusEngine.Engine.DataDirectory, url);
			}

			System.Uri uriResult;
			if (!BusEngine.Browser.ValidURLStatic(url, out uriResult)) {
				url = System.IO.Path.GetFullPath(url);
			}

			if (System.IO.File.Exists(url) || BusEngine.Browser.ValidURLStatic(url, out uriResult)) {
				#if AUDIO_LOG
				BusEngine.Log.Info(url);
				#endif

				try {
					// https://code.videolan.org/videolan/LibVLCSharp/-/blob/master/samples/LibVLCSharp.WinForms.Sample/Form1.cs
					// https://github.com/videolan/libvlcsharp#quick-api-overview
					// https://codesailer.com/tutorials/simple_video_player/
					System.Threading.Tasks.Task.Run(() => {
						LibVLCSharp.Shared.Media media = new LibVLCSharp.Shared.Media(_VLC, new System.Uri(url));

						_mediaPlayer.Time = (long)this.Position;
						if (this.Volume > 100) {
							this.Volume = 100;
						}
						if (this.Volume < 0) {
							this.Volume = 0;
						}
						_mediaPlayer.Volume = this.Volume; // 0 - 100
						/* if (this.Balance > 100) {
							this.Balance = 100;
						}
						if (this.Balance < -100) {
							this.Balance = -100;
						}
						_mediaPlayer.Balance = (int)(this.Balance * 100); */
						_mediaPlayer.Mute = this.Mute;
						_mediaPlayer.EnableKeyInput = true;
						this.Duration = media.Duration;

						_mediaPlayer.Play(media);
						media.Dispose();
					});
				} catch (System.Exception e) {
					BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("error") + " " + BusEngine.Localization.GetLanguageStatic("error_audio_format") + ": {0}", e.Message);
				}
			} else {
				#if AUDIO_LOG
				BusEngine.Log.Info("Аудио OnNotFound");
				#endif

				if (this.OnNotFound != null) {
					this.IsDispose = true;
					//this.OnNotFound.Invoke(this, this.Url);
					BusEngine.UI.Canvas.WinForm.Invoke(this.OnNotFound, new object[2] {this, this.Url});
				}
			}

			return this;
		}
		/** функция запуска aудио */

		/** функция временной остановки aудио */
		public void Pause() {
			#if AUDIO_LOG
			BusEngine.Log.Info("Аудио Pause()");
			#endif

			if (!_mediaPlayer.CanPause) {
				_mediaPlayer.Play();
				this.IsPause = false;
			} else {
				_mediaPlayer.Pause();
				this.IsPause = true;
			}
		}
		/** функция временной остановки aудио */

		/** функция остановки aудио */
		public void Stop() {
			if (this.IsStop) {
				return;
			}
			this.IsStop = true;

			#if AUDIO_LOG
			BusEngine.Log.Info("Аудио Stop()");
			#endif

			_mediaPlayer.Stop();
		}
		/** функция остановки aудио */

		/** функция уничтожения объекта aудио */
		private System.Timers.Timer DisposeTimer;

		public void Dispose() {
			if (this.IsDispose) {
				return;
			}
			this.IsDispose = true;

			#if VIDEO_LOG
			BusEngine.Log.Info("Аудио Dispose()");
			#endif

			if (this.DisposeAuto > 0) {
				System.Timers.ElapsedEventHandler onTime = (o, e) => {
					this.Dispose(true);
				};

				if (this.DisposeTimer == null) {
					this.DisposeTimer = new System.Timers.Timer(this.DisposeAuto);
				}
				//this.DisposeTimer.Interval = this.DisposeAuto;
				this.DisposeTimer.Elapsed -= onTime;
				this.DisposeTimer.Elapsed += onTime;
				this.DisposeTimer.AutoReset = false;
				this.DisposeTimer.Enabled = true;
			} else {
				System.Threading.Tasks.Task.Run(() => {
					this.Dispose(true);
				});
			}

			System.GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if (!this.IsPlay && (this.IsStop || this.IsEnd) && this.IsDispose) {
				_mediaPlayer.Playing -= this.OnPlaying;
				if (this.Loop) {
					_mediaPlayer.EndReached -= this.OnLooping;
				}
				_mediaPlayer.Paused -= this.OnPausing;
				_mediaPlayer.Stopped -= this.OnStopping;
				//_mediaPlayer.Disposed -= this.OnDisposing;
				_mediaPlayer.EndReached -= this.OnEnding;

				_mediaPlayer.Dispose();
				_VLC.Dispose();

				if (this.DisposeTimer != null) {
					this.DisposeTimer.Dispose();
				}
				if (this.OnDispose != null) {
					BusEngine.UI.Canvas.WinForm.Invoke(this.OnDispose, new object[2] {this, this.Url});
				}
			}
		}
		/** функция уничтожения объекта aудио */

		/** функция уничтожения объекта aудио */
		~Audio() {
			// async
			//new System.Threading.Thread(new System.Threading.ThreadStart(delegate {
				#if VIDEO_LOG
				BusEngine.Log.Info("Аудио ========== Finalize()");
				#endif
			//})).Start();
		}
		/** функция уничтожения объекта aудио */
	}
	/** API BusEngine.Audio */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.UI.Canvas
BusEngine.Tools.Json
*/
	/** API BusEngine.Browser */
	public class Browser : System.IDisposable {
		private static CefSharp.WinForms.ChromiumWebBrowser browser;
		public delegate void OnPostMessageHandler(string e);
		public static event OnPostMessageHandler OnPostMessageStatic;
		public delegate void OnLoadHandler();
		public static event OnLoadHandler OnLoadStatic;
		public Browser() {}

		/** все события из PostMessage js браузера */
		// https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions#13-how-do-you-handle-a-javascript-event-in-c
		private static void OnCefPostMessage(object sender, CefSharp.JavascriptMessageReceivedEventArgs e) {
			if (OnPostMessageStatic != null) {
				#if BROWSER_LOG
				BusEngine.Log.Info("BusEngine.Browser.{0}", "OnPostMessageStatic");
				#endif
				OnPostMessageStatic.Invoke((string)e.Message);
			}
		}
		/** все события из PostMessage js браузера */

		/** событие загрузки страницы браузера */
		// https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions#13-how-do-you-handle-a-javascript-event-in-c
		private static void OnCefFrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e) {
			if (e.Frame.IsMain && OnLoadStatic != null) {
				#if BROWSER_LOG
				BusEngine.Log.Info("BusEngine.Browser.{0}", "OnLoadStatic");
				#endif
				OnLoadStatic.Invoke();
				//e.Frame.Dispose();
			}
		}
		/** событие загрузки страницы браузера */

		/** функция выполнения js кода в браузере */
		public static void ExecuteJSStatic(string js = "") {
			if (browser != null) {
				CefSharp.WebBrowserExtensions.ExecuteScriptAsync(browser, @js);
			} else {
				BusEngine.Log.Info("Ошибка! {0}", "Браузер ещё не запущен!");
			}
		}
		/** функция выполнения js кода в браузере */

		/* public static void ExecuteJSStatic(Browser browser, string js = "") {
			if (browser != null) {
				CefSharp.WebBrowserExtensions.ExecuteScriptAsync(browser, @js);
			} else {
				BusEngine.Log.Info("Ошибка! {0}", "Браузер ещё не запущен!");
			}
		} */

		internal static bool ValidURLStatic(string s, out System.Uri url) {
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
				if (ValidURLStatic(url, out uriResult) && url.IndexOf(':') == -1) {
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

				// включаем поддержку экранов с высоким разрешением
				CefSharp.Cef.EnableHighDPISupport();

				// подгружаем объект настроек CefSharp по умолчанияю, чтобы внести свои правки
				CefSharp.WinForms.CefSettings settings = new CefSharp.WinForms.CefSettings();

				// консольные команды хромиум
				settings.CefCommandLineArgs.Add("disable-gpu-shader-disk-cache");
				settings.CefCommandLineArgs.Add("disable-gpu-vsync");
				//settings.CefCommandLineArgs.Add("disable-gpu");

				// настройка имён файлов
				//settings.BrowserSubprocessPath = "CefSharp.BrowserSubprocess.exe";
				//settings.CachePath = "";

				// устанавливаем свой юзер агент
				settings.UserAgent = BusEngine.Engine.Device.UserAgent;

				// установка языка
				//settings.AcceptLanguageList = new BusEngine.Localization().Language.Substring(0, 2).ToLower();
				settings.Locale = BusEngine.Localization.LanguageStatic.Substring(0, 2).ToLower();

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

				// в одном потоке (отключить асинхронность)
				/* settings.MultiThreadedMessageLoop = true;
				//settings.ExternalMessagePump = true;
				System.Timers.Timer timer = new System.Timers.Timer();
				timer.Interval = 1000 / 30;
				timer.Elapsed += (o, e) => {
					BusEngine.Log.Info(1);
					CefSharp.Cef.DoMessageLoopWork();
				};
				timer.Start(); */

				// применяем наши настройки до запуска браузера
				CefSharp.Cef.Initialize(settings);
				settings.Dispose();

				// запускаем браузер
				browser = new CefSharp.WinForms.ChromiumWebBrowser(url);

				if (url != null && !ValidURLStatic(url, out uriResult)) {
					CefSharp.WebBrowserExtensions.LoadHtml(browser, url, true);
				} else if (url == null) {
					if (BusEngine.Localization.GetLanguageStatic("error_browser_url") != "error_browser_url") {
						url = "<meta charset=\"UTF-8\"><b>" + BusEngine.Localization.GetLanguageStatic("error_browser_url") + "</b>";
					} else {
						url = "<meta charset=\"UTF-8\"><b>ПРАВЕРЦЕ ШЛЯХ ДА ФАЙЛУ!</b>";
					}

					CefSharp.WebBrowserExtensions.LoadHtml(browser, url, true);
				}

				//ExecuteJSStatic("BusEngine.PostMessage = ('CefSharp' in window ? CefSharp.PostMessage : function(m) {});");

				// https://stackoverflow.com/questions/51259813/call-c-sharp-function-from-javascript-using-cefsharp-in-windows-form-app
				// подключаем событие сообщения из javascript
				browser.JavascriptMessageReceived += OnCefPostMessage;
				// подключаем событие консоли
				browser.ConsoleMessage += (object s, CefSharp.ConsoleMessageEventArgs e) => {
					string level = e.Level.ToString().ToLower();
					System.ConsoleColor cc = System.Console.ForegroundColor;
					if (level == "error") {
						System.Console.ForegroundColor = System.ConsoleColor.Red;
						BusEngine.Log.Info("Console Browser {0}: \"{1}\" {2}:{3}", level, e.Message, e.Source, e.Line);
					} else if (level == "warning") {
						System.Console.ForegroundColor = System.ConsoleColor.Yellow;
						BusEngine.Log.Info("Console Browser {0}: \"{1}\" {2}:{3}", level, e.Message, e.Source, e.Line);
					} else if (level == "info") {
						System.Console.ForegroundColor = System.ConsoleColor.Cyan;
						BusEngine.Log.Info("Console Browser {0}: \"{1}\"", level, e.Message, e.Source, e.Line);
					} else {
						System.Console.ForegroundColor = System.ConsoleColor.Cyan;
						BusEngine.Log.Info("Console Browser {0}: \"{1}\" {2}:{3}", level, e.Message, e.Source, e.Line);
					}
					System.Console.ForegroundColor = cc;
				};
				// подключаем событие загрузки страницы
				/* browser.LoadingStateChanged += (object s, CefSharp.LoadingStateChangedEventArgs e) => {
					//CefSharp.WebBrowserExtensions.ExecuteScriptAsync(e.Browser, "if (!('BusEngine' in window)) {window.BusEngine = {};} window.BusEngine.PostMessage = ('CefSharp' in window ? CefSharp.PostMessage : function(m) {}); CefSharp = null;");
					#if BROWSER_LOG
					BusEngine.Log.Info("LoadingStateChanged! {0}", e);
					#endif
				}; */
				/* browser.IsBrowserInitializedChanged += (object s, System.EventArgs e) => {
					//CefSharp.WebBrowserExtensions.ExecuteScriptAsync(browser, "if (!('BusEngine' in window)) {window.BusEngine = {};} window.BusEngine.PostMessage = ('CefSharp' in window ? CefSharp.PostMessage : function(m) {}); CefSharp = null;");
					#if BROWSER_LOG
					BusEngine.Log.Info("IsBrowserInitializedChanged! {0}", e);
					#endif
				}; */
				// https://cefsharp.github.io/api/107.1.x/html/T_CefSharp_StatusMessageEventArgs.htm
				/* browser.StatusMessage += (object s, CefSharp.StatusMessageEventArgs e) => {
					//CefSharp.WebBrowserExtensions.ExecuteScriptAsync(e.Browser, "if (!('BusEngine' in window)) {window.BusEngine = {};} window.BusEngine.PostMessage = ('CefSharp' in window ? CefSharp.PostMessage : function(m) {}); CefSharp = null;");
					#if BROWSER_LOG
					BusEngine.Log.Info("StatusMessage! {0}", e.Value);
					#endif
				}; */
				/** заменяем на своё CefSharp.PostMessage на BusEngine.PostMessage */
				// https://cefsharp.github.io/api/107.1.x/html/T_CefSharp_FrameLoadStartEventArgs.htm
				browser.FrameLoadStart += (object s, CefSharp.FrameLoadStartEventArgs e) => {
					if (e.Frame.IsMain) {

						CefSharp.WebBrowserExtensions.ExecuteScriptAsync(e.Browser, @"
	if (!('BusEngine' in window)) {
		window.BusEngine = {};
	}
	if ('CefSharp' in window && 'PostMessage' in window.CefSharp) {
		window.BusEngine.postMessage = CefSharp.PostMessage;
	} else if ('CefSharp' in window && 'postMessage' in window.CefSharp) {
		window.BusEngine.postMessage = CefSharp.postMessage;
	} else {
		window.BusEngine.postMessage = function(m) {};
	}
	CefSharp = null;
	if (!('localization' in window.BusEngine)) {
		BusEngine.localization = {};
	}
	BusEngine.localization.getLanguages = " + BusEngine.Tools.Json.Encode(BusEngine.Localization.GetLanguages) + @";
	if (!('engine' in window.BusEngine)) {
		BusEngine.engine = {};
	}
	/*BusEngine.engine.settingEngine = " + BusEngine.Tools.Json.Encode(BusEngine.Engine.SettingEngine) + @";*/
	BusEngine.engine.settingProject = " + BusEngine.Tools.Json.Encode(BusEngine.Engine.SettingProject) + @";
");
						#if BROWSER_LOG
						BusEngine.Log.Info("FrameLoadStart {0}", e.Frame);
						#endif
						e.Frame.Dispose();
					}
				};
				/* browser.FrameLoadEnd += (object s, CefSharp.FrameLoadEndEventArgs e) => {
					if (e.Frame.IsMain) {
						#if BROWSER_LOG
						BusEngine.Log.Info("FrameLoadEnd {0}", e.Frame);
						#endif
						//e.Frame.Dispose();
					}
				}; */
				/** заменяем на своё CefSharp.PostMessage на BusEngine.PostMessage */
				/** событие клика из браузера */
				/* browser.KeyDown += (object o, System.Windows.Forms.KeyEventArgs e) => {
					BusEngine.Log.Info("Browser KeyDown");
				};
				browser.MouseClick += (object o, System.Windows.Forms.MouseEventArgs e) => {
					BusEngine.Log.Info("Browser MouseClick");
				}; */
				/** событие клика из браузера */

				browser.FrameLoadEnd += OnCefFrameLoadEnd;
				browser.UseParentFormMessageInterceptor = false;

				// устанавливаем размер окана браузера, как в нашей программе
				//browser.Size = BusEngine.UI.Canvas.WinForm.ClientSize;
				//browser.Dock = BusEngine.UI.Canvas.WinForm.Dock;

				// подключаем браузер к нашей программе
				BusEngine.UI.Canvas.WinForm.Controls.Add(browser);
				browser.BringToFront();
			}
		}
		/** функция запуска браузера */

		public static void ShutdownStatic() {
			if (browser != null && !browser.IsDisposed) {
				browser.JavascriptMessageReceived -= OnCefPostMessage;
				browser.FrameLoadEnd -= OnCefFrameLoadEnd;
				browser.Dispose();
				BusEngine.UI.Canvas.WinForm.Controls.Remove(browser);
			}
			/* System.Threading.Tasks.Task.Run(() => {
				CefSharp.Cef.Shutdown();
			}); */
		}

		public void Shutdown() {
			Dispose();
			/* System.Threading.Tasks.Task.Run(() => {
				CefSharp.Cef.Shutdown();
			}); */
		}

		public void Dispose() {
			if (browser != null && !browser.IsDisposed) {
				browser.JavascriptMessageReceived -= OnCefPostMessage;
				browser.FrameLoadEnd -= OnCefFrameLoadEnd;
				browser.Dispose();
				BusEngine.UI.Canvas.WinForm.Controls.Remove(browser);
			}
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
	
/* TValue this[TKey key]
		{
			
			get;
			
			set;
		} */
	
	/** API BusEngine.Array */
	// https://learn.microsoft.com/ru-ru/dotnet/csharp/language-reference/keywords/this
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
		public delegate void EngineHandler();
		public static event EngineHandler OnInitialize;
		public static event EngineHandler OnShutdown;

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

				dynamic info;

				if (setting.TryGetValue("info", out info) && info.GetType().GetProperty("Type") != null && !info.GetType().IsArray) {
					foreach (var i in info) {
						if (i is object && i.GetType().GetProperty("Name") != null && i.Name is string) {
							BusEngine.ProjectDefault.Setting2["info"][i.Name] = (string)info[i.Name];
						}
					}

					//BusEngine.Log.Info("info {0}", BusEngine.Tools.Json.Encode(BusEngine.ProjectDefault.Setting2["info"]));
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
			BusEngine.Engine.SettingProject = BusEngine.ProjectDefault.Setting2;

			// инициализируем плагины
			new BusEngine.IPlugin("Initialize");

			// запускаем окно BusEngine
			if (OnInitialize != null) {
				OnInitialize.Invoke();
				OnInitialize = null;
			}
		}
		/** функция запуска API BusEngine */

		/** функция остановки API BusEngine  */
		public static void Shutdown() {
			// отключаем плагины
			new BusEngine.IPlugin("Shutdown");
			// закрываем окно консоли
			BusEngine.Log.ConsoleHide();
			// закрываем окно BusEngine
			if (BusEngine.Engine.OnShutdown != null) {
				BusEngine.Engine.OnShutdown.Invoke();
				BusEngine.Engine.OnShutdown = null;
			}
		}
		/** функция остановки API BusEngine  */
	}
	/** API BusEngine.Engine */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
	public class Localization : System.IDisposable {
		//[BusEngine.Tooltip("Loading a language if the desired one is not available.", "English")]
		public string LanguageDefault = "Belarusian";
		//[BusEngine.Tooltip("Forced language loading", "English")]
		public string Language = "";
		public static string LanguageStatic { get; private set; }
		//[BusEngine.Tooltip("Provide a name for the translation file to use different files for different scenes. Example, 'level_1' - as a result, the path to the file will become: 'Assets/Localization/lang_name/level_1.cfg.", "English")]
		public string File = "";
		//[BusEngine.Tooltip("Format lang file. For mobiles and sites Unity Support: txt, html, htm, xml, bytes, json, csv, yaml, fnt", "English")]
		public string Format = "cfg";
		//[BusEngine.Tooltip("Translate components located in inactive objects?", "English")]
		private bool IncludeInactive = false;
		//[BusEngine.Tooltip("Replace Resources.load with Bundle.load?", "English")]
		private bool BundleStatus = false;

		public delegate void LocalizationHandler(Localization sender, string language);
		public event LocalizationHandler OnLoad;
		public static event LocalizationHandler OnLoadStatic;
		public delegate void Call();
		private Call CallbackStart = null;
		// https://learn.microsoft.com/ru-ru/dotnet/standard/collections/thread-safe/how-to-add-and-remove-items
		internal static System.Collections.Concurrent.ConcurrentDictionary<string, string> GetLanguages = new System.Collections.Concurrent.ConcurrentDictionary<string, string>();
		private static string Value = "";
		private Localization _Localization;

		public static string GetLanguageStatic(string key) {
			if (GetLanguages.TryGetValue(key, out Value)) {
				return Value;
			} else {
				return key;
			}
		}

		/* public Localization() {
			_Localization = this;
		} */

		public static void SetLanguageStatic(string key, string value) {
			// C# 6.0+
			GetLanguages[key] = value;
			// C# 4.0+
			/* if (GetLanguages.ContainsKey(key)) {
				GetLanguages.Remove(key);
			}
			GetLanguages.Add(key, value); */
		}

		public string GetLanguage(string key) {
			if (GetLanguages.TryGetValue(key, out Value)) {
				return Value;
			} else {
				return key;
			}
		}

		public void SetLanguage(string key, string value) {
			GetLanguages[key] = value;
		}

		public static bool CallBack(Call callback = null) {
			if (callback != null) {
				Call CallbackStart = callback;
			}
			
			return false;
		}

		public Localization Initialize() {
			if (Language == null || Language == "") {
				Language = LanguageDefault.ToString();
			}
			StartLocalization(Language);
			if (OnLoad != null) {
				OnLoad.Invoke(this, Language);
			}
			if (OnLoadStatic != null) {
				OnLoadStatic.Invoke(this, Language);
			}

			return this;
		}

		public void Load(string Language = null) {
			StartLocalization(Language);
			if (OnLoad != null) {
				OnLoad.Invoke(this, Language);
			}
			if (OnLoadStatic != null) {
				OnLoadStatic.Invoke(this, Language);
			}
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
					//files = System.Text.Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(path + Language + File + "." + Format));
				} else {
					Language = LanguageDefault;
					if (System.IO.File.Exists(path + Language + File + "." + Format)) {
						files = System.IO.File.ReadAllText(path + Language + File + "." + Format);
						//files = System.Text.Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(path + Language + File + "." + Format));
					}
				}
			}

			LanguageStatic = Language;

			if (files != "") {
				string[] lines, pairs;
				int i, ii;

				lines = files.Split(new string[] {"\r\n", "\n\r", "\n"}, System.StringSplitOptions.RemoveEmptyEntries);
				ii = lines.Length;

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
			this.Dispose();
		}

		public static void Shutdown() {}

		public void Dispose() {
			System.GC.Collect();
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

				System.Console.Title = BusEngine.Localization.GetLanguageStatic("text_name_console") + " v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
				BusEngine.Localization.OnLoadStatic += OnLoadLanguage;
				System.Console.CancelKeyPress += new System.ConsoleCancelEventHandler(BusEngine.Log.MyHandler);

				//System.Console.Clear();
				BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("text_name_console"));
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
				BusEngine.Localization.OnLoadStatic -= OnLoadLanguage;
				System.Console.CancelKeyPress -= new System.ConsoleCancelEventHandler(BusEngine.Log.MyHandler);
				//System.Console.OutputEncoding = new System.Text.UTF8Encoding();
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

		/** событие загрузки языка */
		private static void OnLoadLanguage(BusEngine.Localization l, string language) {
			System.Console.Title = l.GetLanguage("text_name_console") + " v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
		}
		/** событие загрузки языка */

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
			#if LOG_TYPE
			System.Console.WriteLine("System.Type");
			#endif
		}
		public static void Info(string args1) {
			System.Console.WriteLine(args1);
			#if LOG_TYPE
			System.Console.WriteLine("string");
			#endif
		}
		/* public static void Info(string[] args1) {
			System.Console.WriteLine(args1.ToString());
			#if LOG_TYPE
			System.Console.WriteLine("string[]");
			#endif
		} */
		public static void Info(ulong args1) {
			System.Console.WriteLine(args1);
			#if LOG_TYPE
			System.Console.WriteLine("ulong");
			#endif
		}
		public static void Info(uint args1) {
			System.Console.WriteLine(args1);
			#if LOG_TYPE
			System.Console.WriteLine("uint");
			#endif
		}
		public static void Info(float args1) {
			System.Console.WriteLine(args1);
			#if LOG_TYPE
			System.Console.WriteLine("float");
			#endif
		}
		public static void Info(decimal args1) {
			System.Console.WriteLine(args1);
			#if LOG_TYPE
			System.Console.WriteLine("decimal");
			#endif
		}
		public static void Info(long args1) {
			System.Console.WriteLine(args1);
			#if LOG_TYPE
			System.Console.WriteLine("long");
			#endif
		}
		public static void Info(int args1) {
			System.Console.WriteLine(args1);
			#if LOG_TYPE
			System.Console.WriteLine("int");
			#endif
		}
		/* public static void Info(int[] args1) {
			System.Console.WriteLine(args1);
			#if LOG_TYPE
			System.Console.WriteLine("int[]");
			#endif
		} */
		public static void Info(double args1) {
			System.Console.WriteLine(args1);
			#if LOG_TYPE
			System.Console.WriteLine("double");
			#endif
		}
		public static void Info(byte args1) {
			System.Console.WriteLine(args1);
			#if LOG_TYPE
			System.Console.WriteLine("double");
			#endif
		}
		public static void Info(char args1) {
			System.Console.WriteLine(args1);
			#if LOG_TYPE
			System.Console.WriteLine("char");
			#endif
		}
		public static void Info(char[] args1) {
			System.Console.WriteLine(args1);
			#if LOG_TYPE
			System.Console.WriteLine("char[]");
			#endif
		}
		public static void Info(bool args1) {
			System.Console.WriteLine(args1);
			#if LOG_TYPE
			System.Console.WriteLine("bool");
			#endif
		}
		public static void Info(object args1) {
			System.Console.WriteLine(args1.ToString());
			#if LOG_TYPE
			System.Console.WriteLine("object");
			#endif
		}
		/* public static void Info(object[] args1) {
			System.Console.WriteLine(args1.ToString());
			#if LOG_TYPE
			System.Console.WriteLine("object[]");
			#endif
		} */
		public static void Info(string args1, string args2) {
			System.Console.WriteLine(args1, args2);
			#if LOG_TYPE
			System.Console.WriteLine("string string");
			#endif
		}
		public static void Info(string args1, object args2) {
			System.Console.WriteLine(args1, args2);
			#if LOG_TYPE
			System.Console.WriteLine("string object");
			#endif
		}
		public static void Info(string args1, int args2) {
			System.Console.WriteLine(args1, args2);
			#if LOG_TYPE
			System.Console.WriteLine("string int");
			#endif
		}
		public static void Info(string args1, long args2) {
			System.Console.WriteLine(args1, args2);
			#if LOG_TYPE
			System.Console.WriteLine("string long");
			#endif
		}
		
		/* public static void Info<A>(A a) {
			System.Console.WriteLine(a);
		}
		public static void Info<A, B>(A a, B b) {
			System.Console.WriteLine(a, b);
		}
		public static void Info<A, B, C>(A a, B b, C c) {
			System.Console.WriteLine(a, b, c);
		} */
		/* public static void Info(int arg) {
			System.Console.WriteLine(arg);
		} */
		/* public static void Info(params System.Type[] arg) {
			System.Console.WriteLine(arg);
		} */
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
				arg = arg.Replace("{" + i + "}", args[i].ToString());
			}
			System.Console.WriteLine(arg);
		}
		public static void Info(string arg, params int[] args) {
			int i, ii = args.Length;
			for (i = 0; i < ii; ++i) {
				arg = arg.Replace("{" + i + "}", args[i].ToString());
			}
			System.Console.WriteLine(arg);
		}
		public static void Info(string arg, params string[] args) {
			int i, ii = args.Length;
			for (i = 0; i < ii; ++i) {
				arg = arg.Replace("{" + i + "}", args[i]);
			}
			System.Console.WriteLine(arg);
		}

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
		public virtual void InitializeAsync() {BusEngine.Log.Info("Plugin InitializeAsync");}
		/* public async virtual System.Threading.Tasks.Task Initialize() {
			BusEngine.Log.Info("Plugin Initialize");

			return System.Threading.Tasks.Task.Run(() => {});
		} */

		// после загрузки определённого плагина
		public virtual void Initialize(string plugin) {}
		public virtual void InitializeAsync(string plugin) {}
		public virtual void Initialize(string plugin, string state) {}
		public virtual void InitializeAsync(string plugin, string state) {}

		// при запуске BusEngine после создания формы Canvas
		public virtual void InitializeСanvas() {}
		public virtual void InitializeСanvasAsync() {}

		// перед закрытием BusEngine
		public virtual void Shutdown() {}
		public virtual void ShutdownAsync() {}

		// перед загрузкой игрового уровня
		public virtual void OnLevelLoading(string level) {}
		public virtual void OnLevelLoadingAsync(string level) {}

		// после загрузки игрового уровня
		public virtual void OnLevelLoaded(string level) {}
		public virtual void OnLevelLoadedAsync(string level) {}

		// когда игрок может управлять главным героем - время игры идёт
		public virtual void OnGameStart() {}
		public virtual void OnGameStartAsync() {}

		// когда время остановлено - пауза или закрытие уровня
		public virtual void OnGameStop() {}
		public virtual void OnGameStopAsync() {}

		// вызывается при отрисовки каждого кадра
		public virtual void OnGameUpdate() {}
		public virtual void OnGameUpdateAsync() {}

		// когда игрок начинает подключаться к серверу
		public virtual void OnClientConnectionReceived(int channelId) {}
		public virtual void OnClientConnectionReceivedAsync(int channelId) {}

		// когда игрок подключился к серверу
		public virtual void OnClientReadyForGameplay(int channelId) {}
		public virtual void OnClientReadyForGameplayAsync(int channelId) {}

		// когда игрока выкинуло из сервера - обрыв связи с сервером
		public virtual void OnClientDisconnected(int channelId) {}
		public virtual void OnClientDisconnectedAsync(int channelId) {}
	}
	/** API BusEngine.Plugin */

	/** API BusEngine.IPlugin */
	internal class IPlugin : System.IDisposable {
		private static int Count = 0;
		private static string[] Plugins = new string[0];
		private bool IsAsync(System.Reflection.MethodInfo method) {
			foreach (object o in method.GetCustomAttributes(false)) {
				if (o.GetType() == typeof(System.Runtime.CompilerServices.AsyncStateMachineAttribute)) {
					return true;
				}
			}

			return false;
		}

		// при запуске BusEngine до создания формы
		public IPlugin(string stage = "Initialize") {
			stage = stage.ToLower();
			BusEngine.Log.Info( "============================ System Plugins Start ============================" );

			int i, i2, i3, ii = BusEngine.Engine.SettingEngine["require"]["plugins"].Count;
			string m;

			for (i = 0; i < ii; ++i) {
				if (BusEngine.Engine.SettingEngine["require"]["plugins"][i]["path"] != "") {
					System.Array.Resize(ref Plugins, ii);
					Plugins.SetValue(BusEngine.Engine.SettingEngine["require"]["plugins"][i]["path"], i);
					// https://learn.microsoft.com/ru-ru/dotnet/framework/deployment/best-practices-for-assembly-loading
					foreach (System.Type type in System.Reflection.Assembly.LoadFile(BusEngine.Engine.SettingEngine["require"]["plugins"][i]["path"]).GetTypes()) {
						if (type.IsSubclassOf(typeof(BusEngine.Plugin))) {
							foreach (System.Reflection.MethodInfo method in type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly)) {
								m = method.Name.ToLower();
								if (m == stage || m == stage + "async") {
									Count++;
									BusEngine.Log.Info(BusEngine.Engine.SettingEngine["require"]["plugins"][i]["path"]);
									BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("text_name_class") + ": {0}", type.FullName);
									BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("text_name_method") + ": {0}", method.Name);

									if (m == stage + "async" || IsAsync(method)) {
										BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("text_name_method_start") + ": {0}", "Async");
										// https://learn.microsoft.com/ru-ru/dotnet/api/system.threading.thread?view=net-7.0
										System.Threading.Thread thread = new System.Threading.Thread(() => {
										// https://learn.microsoft.com/ru-ru/dotnet/api/system.threading.tasks.task?view=net-7.0
										//System.Threading.Tasks.Task.Run(() => {
											i2 = method.GetParameters().Length;
											if (i2 == 0) {
												method.Invoke(System.Activator.CreateInstance(type), null);
												if (stage == "initialize") {
													for (i3 = 0; i3 < ii; ++i3) {
														foreach (System.Type tp in System.Reflection.Assembly.LoadFile(BusEngine.Engine.SettingEngine["require"]["plugins"][i3]["path"]).GetTypes()) {
															if (tp.IsSubclassOf(typeof(BusEngine.Plugin))) {
																System.Reflection.MethodInfo md = tp.GetMethod("initialize", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.IgnoreCase, null, new System.Type[] { typeof(string) }, null);
																if (md != null) {
																	object[] x = new object[1];
																	x[0] = BusEngine.Engine.SettingEngine["require"]["plugins"][i]["path"];
																	md.Invoke(System.Activator.CreateInstance(tp), x);
																}
																md = tp.GetMethod("initialize", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.IgnoreCase, null, new System.Type[] { typeof(string), typeof(string) }, null);
																if (md != null) {
																	object[] x = new object[2];
																	x[0] = BusEngine.Engine.SettingEngine["require"]["plugins"][i]["path"];
																	x[1] = stage;
																	md.Invoke(System.Activator.CreateInstance(tp), x);
																}
															}
														}
													}
												}
											}
											if (stage != "initialize") {
												for (i3 = 0; i3 < ii; ++i3) {
													foreach (System.Type tp in System.Reflection.Assembly.LoadFile(BusEngine.Engine.SettingEngine["require"]["plugins"][i3]["path"]).GetTypes()) {
														if (tp.IsSubclassOf(typeof(BusEngine.Plugin))) {
															System.Reflection.MethodInfo md = tp.GetMethod("initialize", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.IgnoreCase, null, new System.Type[] { typeof(string), typeof(string) }, null);
															if (md != null) {
																object[] x = new object[2];
																x[0] = BusEngine.Engine.SettingEngine["require"]["plugins"][i]["path"];
																x[1] = stage;
																md.Invoke(System.Activator.CreateInstance(tp), x);
															}
														}
													}
												}
											}
										});
										thread.Name = BusEngine.ProjectDefault.Setting2["require"]["plugins"][i]["path"];
										thread.Priority = System.Threading.ThreadPriority.Lowest;
										thread.Start();
									} else {
										BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("text_name_method_start") + ": {0}", "Sync");
										i2 = method.GetParameters().Length;
										if (i2 == 0) {
											method.Invoke(System.Activator.CreateInstance(type), null);
											if (stage == "initialize") {
												for (i3 = 0; i3 < ii; ++i3) {
													foreach (System.Type tp in System.Reflection.Assembly.LoadFile(BusEngine.Engine.SettingEngine["require"]["plugins"][i3]["path"]).GetTypes()) {
														if (tp.IsSubclassOf(typeof(BusEngine.Plugin))) {
															System.Reflection.MethodInfo md = tp.GetMethod("initialize", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.IgnoreCase, null, new System.Type[] { typeof(string) }, null);
															if (md != null) {
																object[] x = new object[1];
																x[0] = BusEngine.Engine.SettingEngine["require"]["plugins"][i]["path"];
																md.Invoke(System.Activator.CreateInstance(tp), x);
															}
															md = tp.GetMethod("initialize", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.IgnoreCase, null, new System.Type[] { typeof(string), typeof(string) }, null);
															if (md != null) {
																object[] x = new object[2];
																x[0] = BusEngine.Engine.SettingEngine["require"]["plugins"][i]["path"];
																x[1] = stage;
																md.Invoke(System.Activator.CreateInstance(tp), x);
															}
														}
													}
												}
											}
										}
										if (stage != "initialize") {
											for (i3 = 0; i3 < ii; ++i3) {
												foreach (System.Type tp in System.Reflection.Assembly.LoadFile(BusEngine.Engine.SettingEngine["require"]["plugins"][i3]["path"]).GetTypes()) {
													if (tp.IsSubclassOf(typeof(BusEngine.Plugin))) {
														System.Reflection.MethodInfo md = tp.GetMethod("initialize", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.IgnoreCase, null, new System.Type[] { typeof(string), typeof(string) }, null);
														if (md != null) {
															object[] x = new object[2];
															x[0] = BusEngine.Engine.SettingEngine["require"]["plugins"][i]["path"];
															x[1] = stage;
															md.Invoke(System.Activator.CreateInstance(tp), x);
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
				}
			}

			BusEngine.Log.Info( "============================ System Plugins Stop  ============================" );
			this.Dispose();
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
BusEngine.Log
BusEngine.UI.Canvas
*/
	/** API BusEngine.Video */
	public class Video : System.IDisposable {
		/** видео */
		//private readonly object Lock = new object();
		private LibVLCSharp.Shared.LibVLC _VLC;
		private LibVLCSharp.Shared.MediaPlayer _mediaPlayer;
		private LibVLCSharp.WinForms.VideoView _winForm;
		/** видео */

		// события и его дилегат
		public delegate void VideoHandler(Video sender, string url);

		public event VideoHandler OnPlay;
		public event VideoHandler OnLoop;
		public event VideoHandler OnPause;
		public event VideoHandler OnStop;
		public event VideoHandler OnEnd;
		public event VideoHandler OnDispose;
		public event VideoHandler OnNotFound;

		// состояния
		public bool IsPlay { get; private set; }
		public bool IsPause { get; private set; }
		public bool IsStop { get; private set; }
		public bool IsEnd { get; private set; }
		public bool IsDispose { get; private set; }
		/* private bool IsPlay;
		private bool IsPause;
		private bool IsStop;
		private bool IsEnd;
		private bool IsDispose; */

		// список ссылок
		public string[] Urls;
		private string[] UrlsArray;
		// ссылка
		public string Url = "";
		// вкл\выкл повтор воспроизведения
		public bool Loop = false;
		// громкость звука от 0 до 100
		public byte Volume = 100;
		// баланс колонок от - 100 до 100
		public sbyte Balance = 0;
		// вкл\выкл звука
		public bool Mute = false;
		// длина файла секунды
		public double Duration { get; private set; }
		// текущая позиция секунды
		public double Position {
			get {
				return (_mediaPlayer != null ? _mediaPlayer.Time : 0);
			} set {}
		}
		// авто удаление объектов по времени
		public double DisposeAuto = 500;

		/** событие запуска видео */
		private void OnPlaying(object o, object e) {
			#if VIDEO_LOG
			BusEngine.Log.Info("Видео OnPlay {0}", this.Duration);
			#endif

			this.IsPlay = true;
			this.IsStop = false;
			this.IsDispose = false;

			if (this.OnPlay != null) {
				//this.OnPlay.Invoke(this, this.Url);
				BusEngine.UI.Canvas.WinForm.Invoke(this.OnPlay, new object[2] {this, this.Url});
			}
		}
		/** событие запуска видео */

		/** событие повтора видео */
		private void OnLooping(object o, object e) {
			#if VIDEO_LOG
			BusEngine.Log.Info("Видео OnLoop {0}", this.Duration);
			#endif

			this.Play(this.Url);
			if (this.OnLoop != null) {
				//this.OnLoop.Invoke(this, this.Url);
				BusEngine.UI.Canvas.WinForm.Invoke(this.OnLoop, new object[2] {this, this.Url});
			}
		}
		/** событие повтора видео */

		/** событие временной остановки видео */
		private void OnPausing(object o, object e) {
			#if VIDEO_LOG
			BusEngine.Log.Info("Видео OnPause {0}", this.Position);
			#endif

			if (this.OnPause != null) {
				//this.OnPause.Invoke(this, this.Url);
				BusEngine.UI.Canvas.WinForm.Invoke(this.OnPause, new object[2] {this, this.Url});
			}
		}
		/** событие временной остановки видео */

		/** событие ручной остановки видео */
		private void OnStopping(object o, object e) {
			#if VIDEO_LOG
			BusEngine.Log.Info("Видео OnStop {0}", this.Position);
			#endif

			this.IsPlay = false;

			if (this.OnStop != null) {
				//this.OnStop.Invoke(this, this.Url);
				BusEngine.UI.Canvas.WinForm.Invoke(this.OnStop, new object[2] {this, this.Url});
			}

			if (this.DisposeAuto > 0) {
				if (this.DisposeAuto < 100) {
					this.DisposeAuto = 100;
				}
				this.Dispose();
			}
		}
		/** событие ручной остановки видео */

		/** событие автоматической остановки видео */
		private void OnEnding(object o, object e) {
			#if VIDEO_LOG
			BusEngine.Log.Info("Видео OnEnd {0}", this.Position);
			#endif

			this.IsEnd = true;
			this.IsPlay = false;

			if (this.OnEnd != null) {
				//this.OnEnd.Invoke(this, this.Url);
				BusEngine.UI.Canvas.WinForm.Invoke(this.OnEnd, new object[2] {this, this.Url});
			}

			if (this.DisposeAuto > 0) {
				if (this.DisposeAuto < 100) {
					this.DisposeAuto = 100;
				}
				this.Dispose();
			}
		}
		/** событие автоматической остановки видео */

		/** событие уничтожения видео */
		private void OnDisposing(object o, object e) {
			#if VIDEO_LOG
			BusEngine.Log.Info("Видео OnDispose");
			#endif

			if (this.OnDispose != null) {
				//this.OnDispose.Invoke(this, this.Url);
				BusEngine.UI.Canvas.WinForm.Invoke(this.OnDispose, new object[2] {this, this.Url});
			}
		}
		/** событие уничтожения видео */

		/** функция запуска видео */
		public Video() {
			#if VIDEO_LOG
			_VLC = new LibVLCSharp.Shared.LibVLC(false, new[] { "--verbose=2" });
			#else
			_VLC = new LibVLCSharp.Shared.LibVLC(false);
			#endif
			/* _VLC.Log += (o, e) => {
				BusEngine.Log.Info("1 Log 1 {0}", e.Message);
			}; */
			_VLC.CloseLogFile();
			_VLC.ClearLibVLCError();
			_VLC.SetUserAgent(BusEngine.UI.Canvas.WinForm.Text, BusEngine.Engine.Device.UserAgent);
			_mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_VLC);

			_mediaPlayer.Playing += this.OnPlaying;
			if (this.Loop) {
				_mediaPlayer.EndReached += this.OnLooping;
			}
			_mediaPlayer.Paused += this.OnPausing;
			_mediaPlayer.Stopped += this.OnStopping;
			//_mediaPlayer.Disposed += this.OnDisposing;
			_mediaPlayer.EndReached += this.OnEnding;

			_winForm = new LibVLCSharp.WinForms.VideoView();
			((System.ComponentModel.ISupportInitialize)(_winForm)).BeginInit();
			BusEngine.UI.Canvas.WinForm.SuspendLayout();

			_winForm.MediaPlayer = _mediaPlayer;
			_winForm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			//_winForm.BackColor = System.Drawing.Color.Black;
			//_winForm.TabIndex = 1;
			//_winForm.MediaPlayer = null;
			//_winForm.Name = "Я хочу сожрать 7 Мб!";
			//_winForm.Name = BusEngine.Engine.SettingEngine["info"]["name"];
			//_winForm.Name = BusEngine.Engine.SettingEngine["info"]["name"];
			//_winForm.Text = BusEngine.Engine.SettingEngine["info"]["name"];
			//_winForm.Location = new System.Drawing.Point(0, 27);
			//_winForm.Size = new System.Drawing.Size(800, 444);
			//_winForm.CurrentPosition = position;
			_winForm.Size = BusEngine.UI.Canvas.WinForm.ClientSize;
			//BusEngine.Log.Info("Видео name {0}", BusEngine.Engine.SettingEngine["info"]["name"]);
			//BusEngine.UI.Canvas.WinForm.Controls.Clear();
			//BusEngine.UI.Canvas.WinForm.Update();
			//BusEngine.UI.Canvas.WinForm.Refresh();
			//BusEngine.UI.Canvas.WinForm.ResumeLayout(false);

			if (!BusEngine.UI.Canvas.WinForm.Controls.Contains(_winForm)) {
				#if VIDEO_LOG
				BusEngine.Log.Info("_winForm eeeeeeeeeeeeee {0}", _winForm.GetHashCode());
				#endif
				BusEngine.UI.Canvas.WinForm.Controls.Add(_winForm);
				_winForm.BringToFront();
				//BusEngine.UI.Canvas.WinForm.Controls.AddRange(new System.Windows.Forms.Control[]{_winForm});
			}

			((System.ComponentModel.ISupportInitialize)(_winForm)).EndInit();
			BusEngine.UI.Canvas.WinForm.ResumeLayout(false);
		}
		public Video(string url = "") : this() {
			this.Url = url;
			this.Urls = new string[1] {url};
		}
		public Video(string[] urls) : this() {
			if (urls.Length > 0) {
				this.Urls = urls;
				this.UrlsArray = urls;
				this.Url = urls[0];
				this.OnStop += (BusEngine.Video v, string url) => {
					#if VIDEO_LOG
					BusEngine.Log.Info("Video OnStopVideo: {0}", url);
					BusEngine.Log.Info("Video OnStopVideo: {0}", v.Url);
					#endif

					if (this.UrlsArray.Length > 0) {
						System.Array.Reverse(this.UrlsArray);
						System.Array.Resize(ref this.UrlsArray, this.UrlsArray.Length - 1);
						System.Array.Reverse(this.UrlsArray);
					}

					if (this.UrlsArray.Length > 0) {
						this.Play(this.UrlsArray[0]);
					}
				};
				this.OnEnd += (BusEngine.Video v, string url) => {
					#if VIDEO_LOG
					BusEngine.Log.Info("Video OnStopVideo: {0}", url);
					BusEngine.Log.Info("Video OnStopVideo: {0}", v.Url);
					#endif

					if (this.UrlsArray.Length > 0) {
						System.Array.Reverse(this.UrlsArray);
						System.Array.Resize(ref this.UrlsArray, this.UrlsArray.Length - 1);
						System.Array.Reverse(this.UrlsArray);
					}

					if (this.UrlsArray.Length > 0) {
						this.Play(UrlsArray[0]);
					}
				};
				this.OnNotFound += (BusEngine.Video v, string url) => {
					#if VIDEO_LOG
					BusEngine.Log.Info("Video OnStopVideo: {0}", url);
					BusEngine.Log.Info("Video OnStopVideo: {0}", v.Url);
					#endif

					if (this.UrlsArray.Length > 0) {
						System.Array.Reverse(this.UrlsArray);
						System.Array.Resize(ref this.UrlsArray, this.UrlsArray.Length - 1);
						System.Array.Reverse(this.UrlsArray);
					}

					if (this.UrlsArray.Length > 0) {
						this.Play(UrlsArray[0]);
					}
				};
			}
		}
		public Video Play() {
			return this.Play(this.Url);
		}

		public Video Play(string url = "") {
			if (this.IsPlay) {
				return this;
			}
			this.Url = url;

			#if VIDEO_LOG
			BusEngine.Log.Info("Видео Play()");
			BusEngine.Log.Info(url);
			#endif

			if (url.IndexOf(':') == -1) {
				url = System.IO.Path.Combine(BusEngine.Engine.DataDirectory, url);
			}

			System.Uri uriResult;
			if (!BusEngine.Browser.ValidURLStatic(url, out uriResult)) {
				url = System.IO.Path.GetFullPath(url);
			}

			if (System.IO.File.Exists(url) || BusEngine.Browser.ValidURLStatic(url, out uriResult)) {
				#if VIDEO_LOG
				BusEngine.Log.Info(url);
				#endif

				try {
					// https://code.videolan.org/videolan/LibVLCSharp/-/blob/master/samples/LibVLCSharp.WinForms.Sample/Form1.cs
					// https://github.com/videolan/libvlcsharp#quick-api-overview
					// https://codesailer.com/tutorials/simple_video_player/
					System.Threading.Tasks.Task.Run(() => {
						LibVLCSharp.Shared.Media media = new LibVLCSharp.Shared.Media(_VLC, new System.Uri(url));

						_mediaPlayer.Time = (long)this.Position;
						if (this.Volume > 100) {
							this.Volume = 100;
						}
						if (this.Volume < 0) {
							this.Volume = 0;
						}
						_mediaPlayer.Volume = this.Volume; // 0 - 100
						/* if (this.Balance > 100) {
							this.Balance = 100;
						}
						if (this.Balance < -100) {
							this.Balance = -100;
						}
						_mediaPlayer.Balance = (int)(this.Balance * 100); */
						_mediaPlayer.Mute = this.Mute;
						_mediaPlayer.EnableKeyInput = true;
						this.Duration = media.Duration;
						
						#if VIDEO_LOG
						/* foreach (System.Reflection.EventInfo method in _mediaPlayer.GetType().GetEvents(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly)) {
							BusEngine.Log.Info("Видео _mediaPlayer GetEvent {0}", method.Name);
						}
						foreach (System.Reflection.MethodInfo method in _mediaPlayer.GetType().GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly)) {
							BusEngine.Log.Info("Видео _mediaPlayer GetMethod {0}", method.Name);
						}
						foreach (System.Reflection.PropertyInfo method in _mediaPlayer.GetType().GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly)) {
							BusEngine.Log.Info("Видео _mediaPlayer Property {0}", method.Name + " " + method.GetValue(_mediaPlayer));
						}
						foreach (System.Reflection.FieldInfo method in _mediaPlayer.GetType().GetFields(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly)) {
							BusEngine.Log.Info("Видео _mediaPlayer Field {0}", method + " " + _mediaPlayer.GetType().GetField(method.Name));
						} */

						BusEngine.Log.Info("Видео Mrl {0}", media.Mrl);
						BusEngine.Log.Info("Видео Fps {0}", _mediaPlayer.Fps);
						BusEngine.Log.Info("Видео ПОЛНОЕ ВРЕМЯ {0}", media.Duration);
						BusEngine.Log.Info("Видео ВРЕМЯ {0}", _mediaPlayer.Time);
						BusEngine.Log.Info("Видео Position {0}", _mediaPlayer.Position);
						#endif

						_mediaPlayer.Play(media);
						#if VIDEO_LOG
						/* media.MetaChanged += (o, e) => {
							BusEngine.Log.Info("5 MetaChanged 5");
						};
						media.ParsedChanged += (o, e) => {
							BusEngine.Log.Info("5 ParsedChanged 5");
						};
						media.SubItemAdded += (o, e) => {
							BusEngine.Log.Info("5 SubItemAdded 5");
						};
						media.DurationChanged += (o, e) => {
							BusEngine.Log.Info("5 DurationChanged 5");
						};
						media.MediaFreed += (o, e) => {
							BusEngine.Log.Info("5 MediaFreed 5");
						};
						media.StateChanged += (o, e) => {
							BusEngine.Log.Info("5 StateChanged 5 {0}", e.State.ToString());
							if (e.State.ToString() == "Ended") {

							}
						};
						media.SubItemTreeAdded += (o, e) => {
							BusEngine.Log.Info("5 SubItemTreeAdded 5");
						}; */
						#endif
						media.Dispose();

						#if VIDEO_LOG
						if (_VLC != null) {
							BusEngine.Log.Info("_VLC +++++++++++++++ {0}", _VLC);
						}
						if (_mediaPlayer != null) {
							BusEngine.Log.Info("_mediaPlayer +++++++++++++++ {0}", _mediaPlayer);
						}
						if (_winForm != null) {
							BusEngine.Log.Info("_winForm +++++++++++++++ {0}", _winForm);
							BusEngine.Log.Info("_winForm +++++++++++++++ {0}", _winForm.IsDisposed);
						}
						BusEngine.Log.Info("_mediaPlayer eeeeeeeee {0}", _mediaPlayer.GetHashCode());
						BusEngine.Log.Info("_VLC eeeeeeeeee {0}", _VLC.GetHashCode());
						BusEngine.Log.Info("_winForm eeeeeeeeeeee {0}", _winForm.GetHashCode());
						#endif

						/* _mediaPlayer.MediaChanged += (o, e) => {
							BusEngine.Log.Info("4 MediaChanged 4");
						};
						_mediaPlayer.NothingSpecial += (o, e) => {
							BusEngine.Log.Info("4 NothingSpecial 4");
						};
						_mediaPlayer.Opening += (o, e) => {
							BusEngine.Log.Info("4 Opening 4");
						};
						_mediaPlayer.Buffering += (o, e) => {
							BusEngine.Log.Info("4 Buffering 4");
						}; */
						/* _mediaPlayer.Playing += (o, e) => {
							BusEngine.Log.Info("4 Playing 4");
						}; */
						/* _mediaPlayer.Paused += (o, e) => {
							BusEngine.Log.Info("4 Paused 4");
						};
						_mediaPlayer.Stopped += (o, e) => {
							BusEngine.Log.Info("4 Stopped 4");
						};
						_mediaPlayer.Forward += (o, e) => {
							BusEngine.Log.Info("4 Forward 4");
						};
						_mediaPlayer.Backward += (o, e) => {
							BusEngine.Log.Info("4 Backward 4");
						};
						_mediaPlayer.EndReached += (o, e) => {
							BusEngine.Log.Info("4 EndReached 4");
						};
						_mediaPlayer.EncounteredError += (o, e) => {
							BusEngine.Log.Info("4 EncounteredError 4");
						};
						_mediaPlayer.SeekableChanged += (o, e) => {
							BusEngine.Log.Info("4 SeekableChanged 4");
						};
						_mediaPlayer.PausableChanged += (o, e) => {
							BusEngine.Log.Info("4 PausableChanged 4");
						};
						_mediaPlayer.TitleChanged += (o, e) => {
							BusEngine.Log.Info("4 TitleChanged 4");
						};
						_mediaPlayer.ChapterChanged += (o, e) => {
							BusEngine.Log.Info("4 ChapterChanged 4");
						};
						_mediaPlayer.SnapshotTaken += (o, e) => {
							BusEngine.Log.Info("4 SnapshotTaken 4");
						};
						_mediaPlayer.LengthChanged += (o, e) => {
							BusEngine.Log.Info("4 LengthChanged 4");
						};
						_mediaPlayer.Vout += (o, e) => {
							BusEngine.Log.Info("4 Vout 4");
						};
						_mediaPlayer.ScrambledChanged += (o, e) => {
							BusEngine.Log.Info("4 ScrambledChanged 4");
						};
						_mediaPlayer.ESAdded += (o, e) => {
							BusEngine.Log.Info("4 ESAdded 4");
						};
						_mediaPlayer.ESDeleted += (o, e) => {
							BusEngine.Log.Info("4 ESDeleted 4");
						};
						_mediaPlayer.ESSelected += (o, e) => {
							BusEngine.Log.Info("4 ESSelected 4");
						};
						_mediaPlayer.AudioDevice += (o, e) => {
							BusEngine.Log.Info("4 AudioDevice 4");
						};
						_mediaPlayer.Corked += (o, e) => {
							BusEngine.Log.Info("4 Corked 4");
						};
						_mediaPlayer.Uncorked += (o, e) => {
							BusEngine.Log.Info("4 Uncorked 4");
						};
						_mediaPlayer.Muted += (o, e) => {
							BusEngine.Log.Info("4 Muted 4");
						};
						_mediaPlayer.Unmuted += (o, e) => {
							BusEngine.Log.Info("4 Unmuted 4");
						}; */
						/* _mediaPlayer.TimeChanged += (o, e) => {
							if (_mediaPlayer.Time+2000 > _mediaPlayer.Media.Duration) {
								BusEngine.Log.Info("4 TimeChanged 4 {0}", e.Time);
								BusEngine.Log.Info("Видео ПОЛНОЕ ВРЕМЯ {0}", _mediaPlayer.Media.Duration);
								BusEngine.Log.Info("Видео ВРЕМЯ {0}", _mediaPlayer.Time);
								BusEngine.Log.Info("Видео Position {0}", _mediaPlayer.Position);
								BusEngine.Log.Info("Видео Fps {0}", _mediaPlayer.Fps);
							}
						}; */
						/* _mediaPlayer.PositionChanged += (o, e) => {
							BusEngine.Log.Info("4 PositionChanged 4 {0}", e.Position);
							BusEngine.Log.Info("Видео ПОЛНОЕ ВРЕМЯ {0}", _mediaPlayer.Media.Duration);
							BusEngine.Log.Info("Видео ВРЕМЯ {0}", _mediaPlayer.Time);
							BusEngine.Log.Info("Видео Position {0}", _mediaPlayer.Position);
							BusEngine.Log.Info("Видео Fps {0}", _mediaPlayer.Fps);
						}; */
						/* _mediaPlayer.VolumeChanged += (o, e) => {
							BusEngine.Log.Info("4 VolumeChanged 4");
						}; */
					});
				} catch (System.Exception e) {
					BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("error") + " " + BusEngine.Localization.GetLanguageStatic("error_audio_format") + ": {0}", e.Message);
				}
			} else {
				#if VIDEO_LOG
				BusEngine.Log.Info("Видео OnNotFound");
				#endif

				if (this.OnNotFound != null) {
					this.IsDispose = true;
					//this.OnNotFound.Invoke(this, this.Url);
					BusEngine.UI.Canvas.WinForm.Invoke(this.OnNotFound, new object[2] {this, this.Url});
				}
			}

			return this;
		}
		/** функция запуска видео */

		/** функция временной остановки видео */
		public void Pause() {
			#if VIDEO_LOG
			BusEngine.Log.Info("Видео Pause()");
			#endif

			if (!_mediaPlayer.CanPause) {
				_mediaPlayer.Play();
				this.IsPause = false;
			} else {
				_mediaPlayer.Pause();
				this.IsPause = true;
			}
		}
		/** функция временной остановки видео */

		/** функция остановки видео */
		public void Stop() {
			if (this.IsStop) {
				return;
			}
			this.IsStop = true;

			#if VIDEO_LOG
			BusEngine.Log.Info("Видео Stop()");
			#endif

			_mediaPlayer.Stop();
		}
		/** функция остановки видео */

		/** функция уничтожения объекта видео */
		// https://metanit.com/sharp/tutorial/8.2.php
		//private bool Disposed = false;
		private System.Timers.Timer DisposeTimer;

		public void Dispose() {
			if (this.IsDispose) {
				return;
			}
			this.IsDispose = true;

			#if VIDEO_LOG
			BusEngine.Log.Info("Видео Dispose()");
			#endif

			if (this.DisposeAuto > 0) {
				System.Timers.ElapsedEventHandler onTime = (o, e) => {
					this.Dispose(true);
				};

				if (this.DisposeTimer == null) {
					this.DisposeTimer = new System.Timers.Timer(this.DisposeAuto);
				}
				//this.DisposeTimer.Interval = this.DisposeAuto;
				this.DisposeTimer.Elapsed -= onTime;
				this.DisposeTimer.Elapsed += onTime;
				this.DisposeTimer.AutoReset = false;
				this.DisposeTimer.Enabled = true;
			} else {
				System.Threading.Tasks.Task.Run(() => {
					this.Dispose(true);
				});
			}

			System.GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if (!this.IsPlay && (this.IsStop || this.IsEnd) && this.IsDispose) {
				((System.ComponentModel.ISupportInitialize)(_winForm)).BeginInit();
				BusEngine.UI.Canvas.WinForm.SuspendLayout();
				//BusEngine.UI.Canvas.WinForm.Controls.Remove(_winForm);
				((System.ComponentModel.ISupportInitialize)(_winForm)).EndInit();
				BusEngine.UI.Canvas.WinForm.ResumeLayout(false);

				_mediaPlayer.Playing -= this.OnPlaying;
				if (this.Loop) {
					_mediaPlayer.EndReached -= this.OnLooping;
				}
				_mediaPlayer.Paused -= this.OnPausing;
				_mediaPlayer.Stopped -= this.OnStopping;
				//_mediaPlayer.Disposed -= this.OnDisposing;
				_mediaPlayer.EndReached -= this.OnEnding;

				_mediaPlayer.Dispose();
				_VLC.Dispose();
				_winForm.Dispose();

				if (this.DisposeTimer != null) {
					this.DisposeTimer.Dispose();
				}
				if (this.OnDispose != null) {
					//this.OnDispose.Invoke(this, this.Url);
					// https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.forms.control.invoke?view=windowsdesktop-7.0
					BusEngine.UI.Canvas.WinForm.Invoke(this.OnDispose, new object[2] {this, this.Url});
				}
			}
		}
		/** функция уничтожения объекта видео */

		/** функция уничтожения объекта видео */
		~Video() {
			// async
			//new System.Threading.Thread(new System.Threading.ThreadStart(delegate {
				#if VIDEO_LOG
				BusEngine.Log.Info("Видео ========== Finalize()");
				#endif
			//})).Start();
		}
		/** функция уничтожения объекта видео */
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
			//BusEngine.Localization.GetLanguageStatic("error_server_not")
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
				BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("error") + " " + BusEngine.Localization.GetLanguageStatic("error_json_encode") + ": {0}", e.Message);
				return "[]";
			}
		}

		// массив php
		public static System.Collections.Generic.Dictionary<string, dynamic> Decode(string t) {
			try {
				return Newtonsoft.Json.JsonConvert.DeserializeObject<System.Collections.Generic.Dictionary<string, dynamic>>(t);
				//return Newtonsoft.Json.JsonConvert.DeserializeObject(t);
			} catch (System.Exception e) {
				BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("error") + " " + BusEngine.Localization.GetLanguageStatic("error_json_decode") + ": {0}", e.Message);
				return new System.Collections.Generic.Dictionary<string, dynamic>();
			}
		}

		// object c#
		public static object Decode(string t, bool o = true) {
			try {
				return Newtonsoft.Json.JsonConvert.DeserializeObject(t);
			} catch (System.Exception e) {
				BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("error") + " " + BusEngine.Localization.GetLanguageStatic("error_json_decode") + ": {0}", e.Message);
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
		public static System.Windows.Forms.Form WinForm;
		//public static System.Windows.Forms.Form WPF;
		//public static BusEngine.UI.Canvas Canvas;

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