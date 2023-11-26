/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2024; BuslikDrev - Усе правы захаваны. */

/* C# 5.0+              https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history */
/* NET.Framework 4.7.1+ https://learn.microsoft.com/ru-ru/dotnet/framework/migration-guide/versions-and-dependencies */
/* MSBuild 14.0+        https://vk.com/@busengine-sopostavlyaem-versiu-s-csharp-s-kompilyatorom-i-platformoi-n */
/* MSBuild 15.0+        https://learn.microsoft.com/en-us/xamarin/android/app-fundamentals/android-api-levels?tabs=windows#android-versions */
/* Mono                 https://learn.microsoft.com/ru-ru/xamarin/android/deploy-test/building-apps/abi-specific-apks */
/* важные ссылки
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

/*
internal class ProjectDefault
public class AI https://busengine.buslikdrev.by/api/cs/AI.html
public class Audio https://busengine.buslikdrev.by/api/cs/Audio.html
public class Benchmark https://busengine.buslikdrev.by/api/cs/Benchmark.html
public class Browser https://busengine.buslikdrev.by/api/cs/Browser.html
public class Camera https://busengine.buslikdrev.by/api/cs/Camera.html
public class Core https://busengine.buslikdrev.by/api/cs/Core.html
public class Engine https://busengine.buslikdrev.by/api/cs/Engine.html
public class FlowGraph https://busengine.buslikdrev.by/api/cs/FlowGraph.html
public class Layer https://busengine.buslikdrev.by/api/cs/Layer.html
public class Level https://busengine.buslikdrev.by/api/cs/Level.html
public class Localization https://busengine.buslikdrev.by/api/cs/Localization.html
public class Log https://busengine.buslikdrev.by/api/cs/Log.html
public class Material https://busengine.buslikdrev.by/api/cs/Material.html
public class Model https://busengine.buslikdrev.by/api/cs/Model.html
public class Physics https://busengine.buslikdrev.by/api/cs/Physics.html
public abstract class Plugin https://busengine.buslikdrev.by/api/cs/Plugin.html
internal class IPlugin
public class Rendering https://busengine.buslikdrev.by/api/cs/Rendering.html
public class Ajax https://busengine.buslikdrev.by/api/cs/Tools.Ajax.html
public class Json https://busengine.buslikdrev.by/api/cs/Tools.Json.html
public class FileFolderDialog
public class Canvas https://busengine.buslikdrev.by/api/cs/UI.Canvas.html
public class Vector https://busengine.buslikdrev.by/api/cs/Vector.html
public class Video https://busengine.buslikdrev.by/api/cs/Video.html
*/

//#define AUDIO_LOG
//#define BUSENGINE_BENCHMARK
//#define BROWSER_LOG
//#define LOG_TYPE
//#define VIDEO_LOG
#define CODE_ANALYSIS

/** API BusEngine.Experemental */
namespace BusEngine.Experemental {
	internal class Log : System.IDisposable {
		private static System.Collections.Concurrent.BlockingCollection<string> _blockingCollection = new System.Collections.Concurrent.BlockingCollection<string>();
		private static System.Threading.Tasks.Task _task = System.Threading.Tasks.Task.Factory.StartNew(() => {
			if (!System.IO.Directory.Exists(BusEngine.Engine.LogDirectory)) {
				System.IO.Directory.CreateDirectory(BusEngine.Engine.LogDirectory);
			}

			using (System.IO.StreamWriter streamWriter = new System.IO.StreamWriter(BusEngine.Engine.LogDirectory + "Benchmark.log", true, BusEngine.Engine.UTF8NotBOM)) {
				streamWriter.AutoFlush = true;
				streamWriter.WriteLine("------------------------------------------------------------");

				foreach (string s in _blockingCollection.GetConsumingEnumerable()) {
					streamWriter.WriteLine(s);
				}
			}
		}, System.Threading.Tasks.TaskCreationOptions.LongRunning);

		/* static Log() {

		} */

		public static void File(string action) {
			_blockingCollection.Add(System.DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss.fff") + ": " + action);
		}

		public void Dispose() {
			_blockingCollection.CompleteAdding();
			_task.Wait();
		}
	}
}
/** API BusEngine.Experemental */

/** API BusEngine */
namespace BusEngine {
/*
Зависимости нет
*/
	/** API BusEngine.ProjectDefault */
	internal class ProjectDefault : System.IDisposable {
		public System.Collections.Generic.Dictionary<string, dynamic> Setting;

		public ProjectDefault() {
			Setting = new System.Collections.Generic.Dictionary<string, dynamic>(5, System.StringComparer.OrdinalIgnoreCase) {
				{"console_commands", new System.Collections.Generic.Dictionary<string, string>(20, System.StringComparer.OrdinalIgnoreCase) {
					{"sys_Spec", "1"},                    // Выбор уровня настроек графики
					{"sys_FPS", "256"},                   // Ограничение частоты кадров в секунду
					{"sys_MemoryClearTime", "5"},         // Установка промежутка времени для освобождения оперативной памяти в секундах
					{"sys_MemoryClearAuto", "1"},         // Статус автоматического освобождения оперативной памяти (принудительный вызов System.GC.Collect)
					{"r_WaterOcean", "0"},                // Статус работы океана
					{"r_VolumetricClouds", "1"},          // Статус работы облаков
					{"r_DisplayInfo", "2"},               // Статус работы окна информации
					{"r_FullScreen", "0"},                // Выбор режима работы окна приложения
					{"r_Width", "1280"},                  // Ширина окна приложения
					{"r_Height", "720"},                  // Высота окна приложения
					{"google_api_key", ""},               // Секретный ключ API приложения Google
					{"google_default_client_id", ""},     // ID пользователя API приложения Google
					{"google_default_client_secret", ""}, // Секретный ключ пользователя API приложения Google
				}},
				{"console_variables", new System.Collections.Generic.Dictionary<string, string>(20, System.StringComparer.OrdinalIgnoreCase) {
					{"sys_Spec", "1"},
					{"e_WaterOcean", "0"},
					{"r_WaterOcean", "0"},
					{"r_VolumetricClouds", "1"},
					{"r_DisplayInfo", "0"},
					{"r_FullScreen", "0"},
					{"r_Width", "1280"},
					{"r_Height", "720"},
					{"google_api_key", ""},
					{"google_default_client_id", ""},
					{"google_default_client_secret", ""},
				}},
				{"info", new System.Collections.Generic.Dictionary<string, string>(5) {
					{"name", ""},
					{"version", "1.0.0.0"},
					{"icon", "[data]/Icons/BusEngine.ico"},
					{"type", ""},
					{"guid", ""},
				}},
				{"content", new System.Collections.Generic.Dictionary<string, object>(7) {
					{"bin", "Bin"},
					{"code", "Code"},
					{"data", "Data"},
					{"localization", "Localization"},
					{"log", "Log"},
					{"tools", "Tools"},
					/* {"libs",  new System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>(1) {
						new System.Collections.Generic.Dictionary<string, object>(2) {
							{"name", "BusEngine"},
							{"shared", new System.Collections.Generic.Dictionary<string, string>(5) {
								{"Any", ""},
								{"Android", ""},
								{"Win", ""},
								{"Win_x64", ""},
								{"Win_x86", ""}
							}}
						}
					}} */
				}},
				{"require", new System.Collections.Generic.Dictionary<string, object>(2) {
					{"engine", ""},
					{"plugins", new System.Collections.Generic.List<System.Collections.Generic.Dictionary<string, object>>(1) {
						new System.Collections.Generic.Dictionary<string, object>() {
							{"System", ""},
							{"type", "EType::Managed"},
							{"path", "Bin/Android/Game.dll"},
							{"platforms", new System.Collections.Generic.Dictionary<string, string>(1) {
								{"Android", "Android"}
							}}
						},
						new System.Collections.Generic.Dictionary<string, object>(4) {
							{"System", ""},
							{"type", "EType::Managed"},
							{"path", "Bin/Win/Game.dll"},
							{"platforms", new System.Collections.Generic.Dictionary<string, string>(1) {
								{"Win", "Windows"}
							}}
						},
						new System.Collections.Generic.Dictionary<string, object>(4) {
							{"System", ""},
							{"type", "EType::Managed"},
							{"path", "Bin/Win_x86/Game.dll"},
							{"platforms", new System.Collections.Generic.Dictionary<string, string>(1) {
								{"Win_x86", "Windows"}
							}}
						},
						new System.Collections.Generic.Dictionary<string, object>(4) {
							{"System", ""},
							{"type", "EType::Managed"},
							{"path", "Bin/Win_x64/Game.dll"},
							{"platforms", new System.Collections.Generic.Dictionary<string, string>(1) {
								{"Win_x64", "Windows"}
							}}
						},
						new System.Collections.Generic.Dictionary<string, object>(4) {
							{"System", ""},
							{"type", "EType::Managed"},
							{"path", "Game"},
							{"platforms", new System.Collections.Generic.Dictionary<string, string>(1) {
								{"Any", "Any"}
							}}
						}
					}}
				}}
			};
		}

		public void Dispose() {
			Setting = null;
			//BusEngine.Log.Info("ProjectDefault Dispose");
		}

		~ProjectDefault() {
			//BusEngine.Log.Info("ProjectDefault ~");
		}
	}
	/** API BusEngine.ProjectDefault */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/*
Зависимости нет
*/
	/** API BusEngine.AI */
	// https://www.assemblyai.com/blog/the-top-free-speech-to-text-apis-and-open-source-engines/
	// https://cloud.google.com/speech-to-text
	// https://www.chromium.org/developers/how-tos/api-keys/
	public class AI : System.IDisposable {
		public void Dispose() {

		}

		~AI() {
			//BusEngine.Log.Info("AI ~");
		}
	}
	/** API BusEngine.AI */
}
/** API BusEngine */

/** API BusEngine */
//https://stackoverflow.com/questions/12841845/clear-the-windows-7-standby-memory-programmatically
//https://learn.microsoft.com/ru-ru/dotnet/api/system.runtime.interopservices.gchandle.alloc?view=net-7.0
//https://metanit.com/sharp/patterns/2.3.php
//https://habr.com/ru/post/125421/
//https://habr.com/ru/articles/589005/
//https://learn.microsoft.com/ru-ru/windows-hardware/drivers/debugger/finding-a-memory-leak
//https://metanit.com/sharp/tutorial/8.1.php
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.Log
LibVLCSharp
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
		public event AudioHandler OnDuration;
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
				this.OnPlay.Invoke(this, this.Url);
				//BusEngine.UI.Canvas.WinForm.Invoke(this.OnPlay, new object[2] {this, this.Url});
			}
		}
		/** событие запуска aудио */

		/** событие времени в секундах */
		private System.Timers.Timer OnDuratingTimer;
		private void OnDurating(object o, object e) {
			#if AUDIO_LOG
			BusEngine.Log.Info("Аудио OnDuration {0}", this.Position);
			#endif

			this.Duration = _mediaPlayer.Time;

			if (this.OnDuration != null) {
				this.OnDuration.Invoke(this, this.Url);
				//BusEngine.UI.Canvas.WinForm.Invoke(this.OnDuration, new object[2] {this, this.Url});
			}
		}
		/** событие времени в секундах */

		/** событие повтора aудио */
		private void OnLooping(object o, object e) {
			#if AUDIO_LOG
			BusEngine.Log.Info("Аудио OnLoop {0}", this.Duration);
			#endif

			this.Play(this.Url);
			if (this.OnLoop != null) {
				this.OnLoop.Invoke(this, this.Url);
				//BusEngine.UI.Canvas.WinForm.Invoke(this.OnLoop, new object[2] {this, this.Url});
			}
		}
		/** событие повтора aудио */

		/** событие временной остановки aудио */
		private void OnPausing(object o, object e) {
			#if AUDIO_LOG
			BusEngine.Log.Info("Аудио OnPause {0}", this.Position);
			#endif

			if (this.OnPause != null) {
				this.OnPause.Invoke(this, this.Url);
				//BusEngine.UI.Canvas.WinForm.Invoke(this.OnPause, new object[2] {this, this.Url});
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
				this.OnStop.Invoke(this, this.Url);
				//BusEngine.UI.Canvas.WinForm.Invoke(this.OnStop, new object[2] {this, this.Url});
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
				this.OnEnd.Invoke(this, this.Url);
				//BusEngine.UI.Canvas.WinForm.Invoke(this.OnEnd, new object[2] {this, this.Url});
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
				this.OnDispose.Invoke(this, this.Url);
				//BusEngine.UI.Canvas.WinForm.Invoke(this.OnDispose, new object[2] {this, this.Url});
			}
		}
		/** событие уничтожения aудио */

		/** функция запуска aудио */
		public Audio() {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Audio")) {
			#endif
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
			_VLC.SetUserAgent(BusEngine.Engine.SettingProject["info"]["name"], BusEngine.Engine.Device.UserAgent);
			_mediaPlayer = new LibVLCSharp.Shared.MediaPlayer(_VLC);

			_mediaPlayer.Playing += this.OnPlaying;
			if (this.Loop) {
				_mediaPlayer.EndReached += this.OnLooping;
			}
			_mediaPlayer.Paused += this.OnPausing;
			_mediaPlayer.Stopped += this.OnStopping;
			//_mediaPlayer.Disposed += this.OnDisposing;
			_mediaPlayer.EndReached += this.OnEnding;
			//_mediaPlayer.PositionChanged += this.OnDurating;
			OnDuratingTimer = new System.Timers.Timer();
			OnDuratingTimer.Interval = 100;
			OnDuratingTimer.Elapsed += this.OnDurating;
			//OnDuratingTimer.AutoReset = true;
			//OnDuratingTimer.Enabled = true;

			#if BUSENGINE_BENCHMARK
			}
			#endif
		}
		public Audio(string url = "") : this() {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Audio url")) {
			#endif
			this.Url = url;
			this.Urls = new string[1] {url};
			#if BUSENGINE_BENCHMARK
			}
			#endif
		}
		public Audio(string[] urls) : this() {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Audio urls")) {
			#endif
			if (urls.Length > 0) {
				this.Urls = urls;
				this.UrlsArray = urls;
				this.Url = urls[0];
				if (this.OnStop == null) {
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
				}
				if (this.OnEnd == null) {
					this.OnEnd += (BusEngine.Audio a, string url) => {
						#if AUDIO_LOG
						BusEngine.Log.Info("Audio OnEndAudio: {0}", url);
						BusEngine.Log.Info("Audio OnEndAudio: {0}", a.Url);
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
				if (this.OnNotFound == null) {
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
			#if BUSENGINE_BENCHMARK
			}
			#endif
		}

		public Audio Play() {
			return this.Play(this.Url);
		}

		public Audio Play(string url = "") {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Audio.Play url")) {
			#endif
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

						OnDuratingTimer.Start();
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
					this.OnNotFound.Invoke(this, this.Url);
					//BusEngine.UI.Canvas.WinForm.Invoke(this.OnNotFound, new object[2] {this, this.Url});
				}
			}

			return this;
			#if BUSENGINE_BENCHMARK
			}
			#endif
		}

		public Audio Play(string[] urls) {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Audio.Play urlS")) {
			#endif
			if (urls.Length > 0) {
				this.Urls = urls;
				this.UrlsArray = urls;
				this.Url = urls[0];
				if (this.OnStop == null) {
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
				}
				if (this.OnEnd == null) {
					this.OnEnd += (BusEngine.Audio a, string url) => {
						#if AUDIO_LOG
						BusEngine.Log.Info("Audio OnEndAudio: {0}", url);
						BusEngine.Log.Info("Audio OnEndAudio: {0}", a.Url);
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
				if (this.OnNotFound == null) {
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

				return this.Play(this.Url);
			} else {
				return this;
			}
			#if BUSENGINE_BENCHMARK
			}
			#endif
		}
		/** функция запуска aудио */

		/** функция временной остановки aудио */
		public void Pause() {
			#if AUDIO_LOG
			BusEngine.Log.Info("Аудио Pause()");
			#endif

			if (!_mediaPlayer.CanPause) {
				OnDuratingTimer.Start();
				_mediaPlayer.Play();
				this.IsPause = false;
			} else {
				OnDuratingTimer.Stop();
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

			OnDuratingTimer.Stop();
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

			#if AUDIO_LOG
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
				if (this.OnDuratingTimer != null) {
					OnDuratingTimer.Dispose();
				}
				if (_mediaPlayer != null) {
					_mediaPlayer.Playing -= this.OnPlaying;
					_mediaPlayer.EndReached -= this.OnLooping;
					_mediaPlayer.Paused -= this.OnPausing;
					_mediaPlayer.Stopped -= this.OnStopping;
					//_mediaPlayer.Disposed -= this.OnDisposing;
					_mediaPlayer.EndReached -= this.OnEnding;
					//_mediaPlayer.PositionChanged -= this.OnDurating;
					_mediaPlayer.Dispose();
				}
				if (_VLC != null) {
					_VLC.Dispose();
				}
				if (this.DisposeTimer != null) {
					this.DisposeTimer.Dispose();
				}
				if (this.OnDispose != null) {
					this.OnDispose.Invoke(this, this.Url);
					//BusEngine.UI.Canvas.WinForm.Invoke(this.OnDispose, new object[2] {this, this.Url});
				}
			}
		}
		/** функция уничтожения объекта aудио */

		/** функция уничтожения объекта aудио */
		~Audio() {
			#if AUDIO_LOG
			BusEngine.Log.Info("Audio ~");
			#endif
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
BusEngine.Log
*/
	/** API BusEngine.Benchmark */
	// https://learn.microsoft.com/en-us/dotnet/csharp/programming-guide/concepts/attributes/creating-custom-attributes
	/* [System.AttributeUsage(System.AttributeTargets.Class | System.AttributeTargets.Property | System.AttributeTargets.Field | System.AttributeTargets.Assembly)] */
	[System.AttributeUsage(System.AttributeTargets.All, AllowMultiple = true)]
	public sealed class BenchmarkAttribute : System.Attribute {
		/* public static string Benchmark2 {
			get {
				return BusEngine.Log.Info("Benchmark");
			} set {
				BusEngine.Log.Info("Benchmark");
			}
		} */
		public BenchmarkAttribute() {
			BusEngine.Log.Info("BenchmarkAttribute");  
		}

		public void Dispose() {
			BusEngine.Log.Info("BenchmarkAttribute");
		}
	}

	public class Benchmark : System.IDisposable {
		private System.Diagnostics.Stopwatch _sw = System.Diagnostics.Stopwatch.StartNew();
		private string _label;

		/* public static Benchmark2 Job(string lable) {
			return new Benchmark2(lable);
		} */

		public Benchmark(string label) {
			_label = label;
			//_sw = System.Diagnostics.Stopwatch.StartNew();
		}

		public void Dispose() {
			//BusEngine.Log.Info("Generation Benchmark Dispose: {0}", System.GC.GetGeneration(_sw));
			//BusEngine.Log.Info("Total Memory Benchmark Dispose: {0}", System.GC.GetTotalMemory(false));
			_sw.Stop();
			System.Threading.Tasks.Task.Run(() => {
				BusEngine.Log.Info("Benchmark, " + BusEngine.Localization.GetLanguageStatic("text_operation") + ": [{0}] " + BusEngine.Localization.GetLanguageStatic("text_loading_speed") + ": {1}", _label, _sw.Elapsed);
				BusEngine.Experemental.Log.File("Benchmark, " + BusEngine.Localization.GetLanguageStatic("text_operation") + ": [" + _label + "] " + BusEngine.Localization.GetLanguageStatic("text_loading_speed") + ": " + _sw.Elapsed);
				/* System.TimeSpan ts = _sw.Elapsed;
				int c = (int)System.Math.Log10(ts.Milliseconds) + 1;
				BusEngine.Log.Info("Benchmark, " + BusEngine.Localization.GetLanguageStatic("text_operation") + ": [{0}] " + BusEngine.Localization.GetLanguageStatic("text_loading_speed") + ": {1}", _label, ts.Hours + ":" + ts.Minutes + ":" + ts.Seconds + "." + (c == 1 ? "00" : "") + (c == 2 ? "0" : "") + ts.Milliseconds);
				BusEngine.Experemental.Log.File("Benchmark, " + BusEngine.Localization.GetLanguageStatic("text_operation") + ": [" + _label + "] " + BusEngine.Localization.GetLanguageStatic("text_loading_speed") + ": " + ts.Hours + ":" + ts.Minutes + ":" + ts.Seconds + "." + (c == 1 ? "00" : "") + (c == 2 ? "0" : "") + ts.Milliseconds); */
				//_label = null;
				//_sw = null;
				//BusEngine.Log.Info("Generation Benchmark Dispose2: {0}", System.GC.GetGeneration(_sw));
				//BusEngine.Log.Info("Total Memory Benchmark Dispose2: {0}", System.GC.GetTotalMemory(false));
			});
		}

		~Benchmark() {
			//BusEngine.Log.Info("Benchmark ~ " + _label);
			//BusEngine.Log.Info("Generation Benchmark ~: {0}", System.GC.GetGeneration(_sw));
			//BusEngine.Log.Info("Total Memory Benchmark ~: {0}", System.GC.GetTotalMemory(false));
		}
	}
	/** API BusEngine.Benchmark */
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
		private CefSharp.WinForms.ChromiumWebBrowser browser;
		private string Url;
		public delegate void OnPostMessageHandler(Browser o, string e);
		public event OnPostMessageHandler OnPostMessage;
		public delegate OnDownloadArgs OnDownloadHandler(OnDownloadArgs e);
		public event OnDownloadHandler OnDownload;
		public struct OnDownloadArgs {
			public string ContentDisposition;
			public long CurrentSpeed;
			public System.Nullable<System.DateTime> EndTime;
			public string FullPath;
			public int Id;
			public bool IsCancelled;
			public bool IsComplete;
			public bool IsInProgress;
			public bool IsValid;
			public string MimeType;
			public string OriginalUrl;
			public int PercentComplete;
			public long ReceivedBytes;
			public System.Nullable<System.DateTime> StartTime;
			public string SuggestedFileName;
			public long TotalBytes;
			public string Url;
			public OnDownloadArgs (CefSharp.DownloadItem download) {
				#if BUSENGINE_BENCHMARK
				using (new BusEngine.Benchmark("BusEngine.Browser.OnDownloadArgs")) {
				#endif
				ContentDisposition = download.ContentDisposition;
				CurrentSpeed = download.CurrentSpeed;
				EndTime = download.EndTime;
				FullPath = download.FullPath;
				Id = download.Id;
				IsCancelled = download.IsCancelled;
				IsComplete = download.IsComplete;
				IsInProgress = download.IsInProgress;
				IsValid = download.IsValid;
				MimeType = download.MimeType;
				OriginalUrl = download.OriginalUrl;
				PercentComplete = download.PercentComplete;
				ReceivedBytes = download.ReceivedBytes;
				StartTime = download.StartTime;
				SuggestedFileName = download.SuggestedFileName;
				TotalBytes = download.TotalBytes;
				Url = download.Url;
				#if BUSENGINE_BENCHMARK
				}
				#endif
			}
		}
		public delegate void OnLoadHandler();
		//public event OnLoadHandler OnLoad;
		public event OnLoadHandler OnLoad;
		private string _DownloadPath = BusEngine.Engine.LogDirectory + "Browser\\download";
		public string DownloadPath {
			get {
				return _DownloadPath;
			} set {
				_DownloadPath = value;
			}
		}
		public CefSharp.IDownloadHandler DownloadHandler { get; set; }
		/* public CefSharp.IDownloadHandler Download(CefSharp.IDownloadHandler downloadHandler) {
			return downloadHandler;
		}
		public CefSharp.IDownloadHandler Download(string path, CefSharp.IDownloadHandler downloadHandler) {
			return downloadHandler;
		} */

		public Browser() {

		}

		public Browser(string url) : this() {
			Url = url;
		}

		/** все события из PostMessage js браузера */
		// https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions#13-how-do-you-handle-a-javascript-event-in-c
		private void OnCefPostMessage(object sender, CefSharp.JavascriptMessageReceivedEventArgs e) {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Browser.OnCefPostMessage")) {
			#endif
			if (OnPostMessage != null) {
				#if BROWSER_LOG
				BusEngine.Log.Info("BusEngine.Browser.{0}", "OnPostMessageStatic");
				#endif
				OnPostMessage.Invoke(this, (string)e.Message);
			}
			#if BUSENGINE_BENCHMARK
			}
			#endif
		}
		/** все события из PostMessage js браузера */

		/** событие загрузки страницы браузера */
		// https://github.com/cefsharp/CefSharp/wiki/Frequently-asked-questions#13-how-do-you-handle-a-javascript-event-in-c
		private void OnCefFrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e) {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Browser.OnLoad")) {
			#endif
			if (e.Frame.IsMain && OnLoad != null) {
				#if BROWSER_LOG
				BusEngine.Log.Info("BusEngine.Browser.{0}", "OnLoad");
				#endif
				OnLoad.Invoke();
				//e.Frame.Dispose();
			}
			#if BUSENGINE_BENCHMARK
			}
			#endif
		}
		/** событие загрузки страницы браузера */

		/** функция выполнения js кода в браузере */
		public void ExecuteJS(string js = "") {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Browser.ExecuteJS")) {
			#endif
			if (browser != null) {
				CefSharp.WebBrowserExtensions.ExecuteScriptAsync(browser, @js);
			} else {
				BusEngine.Log.Info("Ошибка! {0}", "Браузер ещё не запущен!");
			}
			#if BUSENGINE_BENCHMARK
			}
			#endif
		}
		/** функция выполнения js кода в браузере */

		/** функция скачивания файла в браузере */
		private void typeDownload(bool status = true) {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Browser.typeDownload")) {
			#endif
			CefSharp.IDownloadHandler x;

			x = browser.DownloadHandler;

			if (status) {
				browser.DownloadHandler = CefSharp.Fluent.DownloadHandler.UseFolder(DownloadPath, (a, b, download, d) => {
					OnDownloadArgs onDownloadArgs = new OnDownloadArgs(download);
					if (OnDownload != null) {
						onDownloadArgs = OnDownload.Invoke(onDownloadArgs);
					}
					//BusEngine.Log.Info("Скачивание: {0}", BusEngine.Tools.Json.Encode(onDownloadArgs));
					if (x != null && (onDownloadArgs.IsComplete || onDownloadArgs.IsCancelled)) {
						browser.DownloadHandler = x;
					}
				});
			} else {
				browser.DownloadHandler = CefSharp.Fluent.DownloadHandler.AskUser((a, b, download, d) => {
					OnDownloadArgs onDownloadArgs = new OnDownloadArgs(download);
					if (OnDownload != null) {
						onDownloadArgs = OnDownload.Invoke(onDownloadArgs);
					}
					//BusEngine.Log.Info("Скачивание: {0}", BusEngine.Tools.Json.Encode(onDownloadArgs));
					if (x != null && (onDownloadArgs.IsComplete || onDownloadArgs.IsCancelled)) {
						browser.DownloadHandler = x;
					}
				});
			}
			#if BUSENGINE_BENCHMARK
			}
			#endif
		}

		public void Download(string url = "") {
			if (browser != null) {
				typeDownload(false);
				if (url.IndexOf("://") == -1) {
					url = "https://bd.busengine/" + url;
				}
				CefSharp.WebBrowserExtensions.StartDownload(browser, url);
			} else {
				BusEngine.Log.Info("Ошибка! {0}", "Браузер ещё не запущен!");
			}
		}

		public void Download(string url = "", string path = "") {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Browser.Download")) {
			#endif
			if (browser != null) {
				string x = DownloadPath;
				if (path != "") {
					DownloadPath = path;
				}
				typeDownload(true);
				DownloadPath = x;
				if (url.IndexOf("://") == -1) {
					url = "https://bd.busengine/" + url;
				}
				CefSharp.WebBrowserExtensions.StartDownload(browser, url);
			} else {
				BusEngine.Log.Info("Ошибка! {0}", "Браузер ещё не запущен!");
			}
			#if BUSENGINE_BENCHMARK
			}
			#endif
		}
		/** функция скачивания файла в браузере */

		internal static bool ValidURLStatic(string s, out System.Uri url) {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Browser.ValidURLStatic")) {
			#endif
			if (!System.Text.RegularExpressions.Regex.IsMatch(s, @"^https?:\/\/", System.Text.RegularExpressions.RegexOptions.IgnoreCase)) {
				s = "http://" + s;
			}

			if (System.Uri.TryCreate(s, System.UriKind.Absolute, out url)) {
				return (url.Scheme == System.Uri.UriSchemeHttp || url.Scheme == System.Uri.UriSchemeHttps);
			}

			return false;
			#if BUSENGINE_BENCHMARK
			}
			#endif
		}

		/** функция запуска браузера */
		// https://cefsharp.github.io/api/
		public Browser Load() {
			return this.Load(this.Url, BusEngine.Engine.DataDirectory);
		}

		public Browser Load(string url = "") {
			if (url == null || url == "") {
				url = this.Url;
			}

			return this.Load(url, BusEngine.Engine.DataDirectory);
		}

		public Browser Load(string url = "", string root = "") {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Browser.Load")) {
			#endif
			if (browser != null) {
				BusEngine.Log.Info("Ошибка! {0}", "Браузер уже запущен!");
			} else {
				if (url == null || url == "") {
					url = "index.html";
				}

				// если ссылка не абсолютный адрес, то делаем его абсолютным
				System.Uri uriResult;
				if (BusEngine.Browser.ValidURLStatic(url, out uriResult) && url.IndexOf(':') == -1) {
					if (System.IO.File.Exists(System.IO.Path.Combine(BusEngine.Engine.DataDirectory, url))) {
						url = "https://bd.busengine/" + url;
					} else {
						url = null;
					}
				}

				if (System.IO.Directory.Exists(System.IO.Path.Combine(BusEngine.Engine.DataDirectory, root))) {
					root = System.IO.Path.Combine(BusEngine.Engine.DataDirectory, root);
				} else {
					root = BusEngine.Engine.DataDirectory;
				}

				// включаем поддержку экранов с высоким разрешением
				//CefSharp.Cef.EnableHighDPISupport();
				//new CefSharp.CefLibraryHafle(BusEngine.Engine.ExeDirectory + "CefSharp\\libcef.dll");
				//CefSharp.CefRuntime.SubscribeAnyCpuAssemblyResolver(BusEngine.Engine.ExeDirectory + "");

				// https://www.chromium.org/developers/how-tos/run-chromium-with-flags/
				// https://peter.sh/experiments/chromium-command-line-switches/
				//CefSharp.BrowserSubprocess.SelfHost.Main(BusEngine.Engine.Commands);

				// Google Speech API
				if (BusEngine.Engine.SettingProject["console_commands"]["google_api_key"] != "") {
					System.Environment.SetEnvironmentVariable("google_api_key", BusEngine.Engine.SettingProject["console_commands"]["google_api_key"]);
				}
				if (BusEngine.Engine.SettingProject["console_commands"]["google_default_client_id"] != "") {
					System.Environment.SetEnvironmentVariable("google_default_client_id", BusEngine.Engine.SettingProject["console_commands"]["google_api_key"]);
				}
				if (BusEngine.Engine.SettingProject["console_commands"]["google_default_client_secret"] != "") {
					System.Environment.SetEnvironmentVariable("google_default_client_secret", BusEngine.Engine.SettingProject["console_commands"]["google_api_key"]);
				}
				//System.Environment.SetEnvironmentVariable("USE_PROPRIETARY_CODECS", "1");

				// подгружаем объект настроек CefSharp по умолчанияю, чтобы внести свои правки
				CefSharp.WinForms.CefSettings settings = new CefSharp.WinForms.CefSettings() /* {
					LogFile = System.IO.Path.Combine(BusEngine.Engine.LogDirectory, "cef_log.txt"),
					CachePath = System.IO.Path.Combine(BusEngine.Engine.LogDirectory, "cache"),
					UserDataPath = System.IO.Path.Combine(BusEngine.Engine.LogDirectory, "userdata")
				} */;

				// консольные команды хромиум
				//settings.ChromeRuntime = true;
				settings.CommandLineArgsDisabled = false;
				//settings.CefCommandLineArgs.Add("disable-gpu-shader-disk-cache");
				//settings.CefCommandLineArgs.Add("disable-gpu-vsync");
				//settings.CefCommandLineArgs.Add("disable-gpu");
				//settings.CefCommandLineArgs.Add("disable-speech-synthesis-api");
				//settings.CefCommandLineArgs.Add("disable-features=SameSiteByDefaultCookies");

				// воспроизводим аудио автоматом
				settings.CefCommandLineArgs.Add("autoplay-policy", "no-user-gesture-required");
				settings.CefCommandLineArgs.Add("enable-media-stream");
				settings.CefCommandLineArgs.Add("enable-speech-input");
				settings.CefCommandLineArgs.Add("ignore-certificate-errors");

				// настройка имён файлов
				settings.LogFile = System.IO.Path.Combine(BusEngine.Engine.LogDirectory, "Browser\\cef_log.txt");
				settings.RootCachePath = System.IO.Path.Combine(BusEngine.Engine.LogDirectory, "Browser\\cache");
				settings.CachePath = System.IO.Path.Combine(BusEngine.Engine.LogDirectory, "Browser\\cache");
				settings.UserDataPath = System.IO.Path.Combine(BusEngine.Engine.LogDirectory, "Browser\\userdata");
				string subprocess = BusEngine.Engine.NameProject + " Browser.exe";
				foreach (string currentFile in System.IO.Directory.EnumerateFiles(BusEngine.Engine.ExeDirectory, "CefSharp.BrowserSubprocess.exe", System.IO.SearchOption.AllDirectories)) {
					subprocess = System.IO.Path.GetDirectoryName(currentFile) + "\\" + subprocess;

					if (!System.IO.File.Exists(subprocess)) {
						System.IO.File.Copy(currentFile, subprocess);
					}

					settings.BrowserSubprocessPath = subprocess;
				}
				//settings.LocalesDirPath = BusEngine.Engine.ExeDirectory + "CefSharp\\locales\\";
				//settings.ResourcesDirPath = BusEngine.Engine.ExeDirectory + "CefSharp\\";
				//settings.WindowlessRenderingEnabled = true;
				//settings.RemoteDebuggingPort = 8080;

				// отключаем создание файла лога
				settings.LogSeverity = CefSharp.LogSeverity.Disable;

				settings.PersistSessionCookies = true;
				settings.CookieableSchemesExcludeDefaults = false;
				//settings.CookieableSchemesList = "";
				//settings.PersistUserPreferences = true;

				// устанавливаем свой юзер агент
				settings.UserAgent = BusEngine.Engine.Device.UserAgent;

				// установка языка
				settings.AcceptLanguageList = BusEngine.Localization.LanguageStatic.Substring(0, 2).ToLower() + "," + BusEngine.Localization.LanguageStatic.ToLower();
				//settings.Locale = BusEngine.Localization.LanguageStatic.Substring(0, 2).ToLower();

				// https://github.com/cefsharp/CefSharp/wiki/General-Usage#scheme-handler
				// регистрируем свою схему
				settings.RegisterScheme(new CefSharp.CefCustomScheme {
					SchemeName = "https",
					DomainName = "bd.busengine",
					SchemeHandlerFactory = new CefSharp.SchemeHandler.FolderSchemeHandlerFactory (
						rootFolder: root,
						hostName: "bd.busengine",
						defaultPage: "index.html"
					)
				});

				// в одном потоке (отключить асинхронность)
				settings.MultiThreadedMessageLoop = true;
				/* //settings.ExternalMessagePump = true;
				System.Timers.Timer timer = new System.Timers.Timer();
				timer.Interval = 1000 / 30;
				timer.Elapsed += (o, e) => {
					BusEngine.Log.Info(1);
					CefSharp.Cef.DoMessageLoopWork();
				};
				timer.Start(); */

				// применяем наши настройки до запуска браузера
				CefSharp.Cef.Initialize(settings, performDependencyCheck: true, browserProcessHandler: null);
				//settings.Dispose();

				// запускаем браузер
				browser = new CefSharp.WinForms.ChromiumWebBrowser(url);

				if (url != null && !BusEngine.Browser.ValidURLStatic(url, out uriResult)) {
					CefSharp.WebBrowserExtensions.LoadHtml(browser, url, true);
				} else if (url == null) {
					if (BusEngine.Localization.GetLanguageStatic("error_browser_url") != "error_browser_url") {
						url = "<meta charset=\"UTF-8\"><b>" + BusEngine.Localization.GetLanguageStatic("error_browser_url") + "</b>";
					} else {
						url = "<meta charset=\"UTF-8\"><b>ПРАВЕРЦЕ ШЛЯХ ДА ФАЙЛУ!</b>";
					}

					CefSharp.WebBrowserExtensions.LoadHtml(browser, url, true);
				}

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
				// подключаем событие скачивания файлов
				if (DownloadHandler != null) {
					browser.DownloadHandler = DownloadHandler;
				}

				if (DownloadPath == "") {
					typeDownload(false);
				} else {
					typeDownload(true);
				}

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
				browser.FrameLoadStart += (object b, CefSharp.FrameLoadStartEventArgs e) => {
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

				// устанавливаем размер окана браузера, как в нашей программе
				//browser.Size = BusEngine.UI.Canvas.WinForm.ClientSize;
				//browser.Dock = BusEngine.UI.Canvas.WinForm.Dock;

				// https://cefsharp.github.io/api/109.1.x/html/P_CefSharp_WinForms_ChromiumWebBrowser_UseParentFormMessageInterceptor.htm
				browser.UseParentFormMessageInterceptor = false;

				// подключаем браузер к нашей программе
				/* BusEngine.Log.Info("1 Owner: {0}", BusEngine.UI.Canvas.WinForm.Owner);
				BusEngine.Log.Info("1 Owner Controls: {0}", BusEngine.UI.Canvas.WinForm.Controls.Owner);
				BusEngine.Log.Info("1 TopLevelControl: {0}", BusEngine.UI.Canvas.WinForm.TopLevelControl);
				BusEngine.Log.Info("1 ActiveControl: {0}", BusEngine.UI.Canvas.WinForm.ActiveControl);
				BusEngine.Log.Info("1 TopLevel: {0}", BusEngine.UI.Canvas.WinForm.TopLevel);
				BusEngine.Log.Info("1 TabIndex: {0}", BusEngine.UI.Canvas.WinForm.TabIndex);
				BusEngine.Log.Info("1 Contains: {0}", BusEngine.UI.Canvas.WinForm.Contains(browser)); */

				BusEngine.UI.Canvas.WinForm.Controls.Add(browser);

				/* BusEngine.Log.Info("2 Owner: {0}", BusEngine.UI.Canvas.WinForm.Owner);
				BusEngine.Log.Info("2 Owner Controls: {0}", BusEngine.UI.Canvas.WinForm.Controls.Owner);
				BusEngine.Log.Info("2 TopLevelControl: {0}", BusEngine.UI.Canvas.WinForm.TopLevelControl);
				BusEngine.Log.Info("2 ActiveControl: {0}", BusEngine.UI.Canvas.WinForm.ActiveControl);
				BusEngine.Log.Info("2 TopLevel: {0}", BusEngine.UI.Canvas.WinForm.TopLevel);
				BusEngine.Log.Info("2 TabIndex: {0}", BusEngine.UI.Canvas.WinForm.TabIndex);
				BusEngine.Log.Info("2 Contains: {0}", BusEngine.UI.Canvas.WinForm.Contains(browser)); */

				browser.BringToFront();

				/* BusEngine.Log.Info("3 Owner: {0}", BusEngine.UI.Canvas.WinForm.Owner);
				BusEngine.Log.Info("3 Owner Controls: {0}", BusEngine.UI.Canvas.WinForm.Controls.Owner);
				BusEngine.Log.Info("3 TopLevelControl: {0}", BusEngine.UI.Canvas.WinForm.TopLevelControl);
				BusEngine.Log.Info("3 ActiveControl: {0}", BusEngine.UI.Canvas.WinForm.ActiveControl);
				BusEngine.Log.Info("3 TopLevel: {0}", BusEngine.UI.Canvas.WinForm.TopLevel);
				BusEngine.Log.Info("3 TabIndex: {0}", BusEngine.UI.Canvas.WinForm.TabIndex);
				BusEngine.Log.Info("3 Contains: {0}", BusEngine.UI.Canvas.WinForm.Contains(browser)); */
			}

			return this;
			#if BUSENGINE_BENCHMARK
			}
			#endif
		}
		/** функция запуска браузера */

		/* public static void ShutdownStatic() {
			if (browser != null && !browser.IsDisposed) {
				browser.JavascriptMessageReceived -= this.OnCefPostMessage;
				browser.FrameLoadEnd -= this.OnCefFrameLoadEnd;
				browser.Dispose();
				//CefSharp.Cef.Shutdown();
				BusEngine.UI.Canvas.WinForm.Controls.Remove(browser);
			}
		} */

		public void Shutdown() {
			this.Dispose();
			/* System.Threading.Tasks.Task.Run(() => {
				CefSharp.Cef.Shutdown();
			}); */
		}

		public void Dispose() {
			BusEngine.Log.Info("Browser Dispose {0}", browser);
			if (browser != null && !browser.IsDisposed) {
				browser.JavascriptMessageReceived -= this.OnCefPostMessage;
				browser.FrameLoadEnd -= this.OnCefFrameLoadEnd;
				browser.Dispose();
				//CefSharp.Cef.Shutdown();
				BusEngine.UI.Canvas.WinForm.Controls.Remove(browser);
			}
		}

		~Browser() {
			BusEngine.Log.Info("Browser ~");
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
	/** API BusEngine.Camera */
	public class Camera : System.IDisposable {
		public void Dispose() {

		}

		~Camera() {
			BusEngine.Log.Info("Camera ~");
		}
	}
	/** API BusEngine.Camera */
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
	public class Core : System.IDisposable {
		public void Dispose() {

		}

		~Core() {
			BusEngine.Log.Info("Core ~");
		}
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
	/* public interface IArray<TKey, TValue> {
		public Array(TKey key, TKey value) {
			BusEngine.Log.Info("Array {0}");
		}

		TValue this[TKey key] {	get; set; }
		
		bool ContainsKey(TKey key);
		
		void Add(TKey key, TValue value);
		
		bool Remove(TKey key);
		
		bool TryGetValue(TKey key, out TValue value);
	} */

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
	public class Engine : System.IDisposable {
		private static System.Timers.Timer Timer;
		private static Engine _instance;
		internal static System.Text.UTF8Encoding UTF8NotBOM;
		public delegate void EngineHandler();
		public static event EngineHandler OnInitialize;
		public static event EngineHandler OnShutdown;

		// https://www.manojphadnis.net/need-to-know-general-topics/listkeyvaluepair-vs-dictionary
		public static System.Collections.Generic.Dictionary<string, dynamic> SettingEngine;
		public static System.Collections.Generic.Dictionary<string, dynamic> SettingProject;
		public static string NameProject { get; private set; }
		public static string BinDirectory { get; private set; }
		public static string ExeDirectory { get; private set; }
		public static string EditorDirectory { get; private set; }
		public static string EngineDirectory { get; private set; }
		public static string CodeDirectory { get; private set; }
		public static string DataDirectory { get; private set; }
		public static string LocalizationDirectory { get; private set; }
		public static string LogDirectory { get; private set; }
		public static string ToolsDirectory { get; private set; }
		public static string Platform { get; private set; }
		public static bool IsShutdown { get; private set; }
		public static string[] Commands;

		// определяем платформу, версию, архитектуру процессора (NET.Framework 4.7.1+)
		public class Device {
			public static string Name { get; private set; }
			public static string Version { get; private set; }
			public static string Processor { get; private set; }
			public static byte ProcessorCount { get; private set; }
			public static string UserAgent;
			static Device() {
				#if BUSENGINE_BENCHMARK
				using (new BusEngine.Benchmark("BusEngine.Initialize.Device")) {
				#endif
				System.OperatingSystem os = System.Environment.OSVersion;

				switch (os.Platform) {
					case System.PlatformID.Win32NT:
					case System.PlatformID.Win32S:
					case System.PlatformID.Win32Windows:
					case System.PlatformID.WinCE:
						Name = "windows";
						break;
					case System.PlatformID.MacOSX:
					case System.PlatformID.Unix:
						Name = "macos";
						break;
					default:
						Name = "other";
						break;
				}

				Version = os.Version.Major + "." + os.Version.Minor;
				Processor = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture.ToString();
				ProcessorCount = (byte)System.Environment.ProcessorCount;
				UserAgent = "Mozilla/5.0 (" + Name + " NT " + Version + "; " + System.Convert.ToString(os.Platform) + "; " + Processor + ") AppleWebKit/537.36 (KHTML, like Gecko) BusEngine/" + BusEngine.Engine.SettingProject["require"]["engine"] + " Safari/537.36";
				#if BUSENGINE_BENCHMARK
				}
				#endif
			}
		}

		// изменить путь для .NET 5+
		/* private static string[] GetProbingPathData = new string[0];

		private static string[] GetProbingPath() {
			if (GetProbingPathData.Length == 0 && System.IO.File.Exists(System.AppDomain.CurrentDomain.SetupInformation.ConfigurationFile)) {
				System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
				xmlDoc.Load(System.AppDomain.CurrentDomain.SetupInformation.ConfigurationFile);
				System.Xml.XmlNode p = xmlDoc.SelectSingleNode("/*[name()='configuration']/*[name()='runtime']/*[name()='assemblyBinding']/*[name()='probing']/@privatePath");

				if (p != null) {
					GetProbingPathData = p.Value.Split(';');
				}
			}

			return GetProbingPathData;
		} */

		private Engine() {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Engine.Initialize")) {
			#endif

			if (BusEngine.Engine.Platform == null) {
				BusEngine.Engine.Platform = "BusEngine";
			}

			UTF8NotBOM = new System.Text.UTF8Encoding(false);

			// устанавливаем ссылку на рабочий каталог
			System.Reflection.Assembly assembly = System.Reflection.Assembly.GetEntryAssembly();
			BusEngine.Engine.ExeDirectory = System.IO.Path.GetDirectoryName(assembly.Location) + "\\";

			BusEngine.Engine.EngineDirectory = BusEngine.Engine.ExeDirectory + "..\\..\\Bin\\";

			if (!System.IO.Directory.Exists(BusEngine.Engine.EngineDirectory)) {
				BusEngine.Engine.EngineDirectory = BusEngine.Engine.ExeDirectory + "..\\Bin\\";

				if (!System.IO.Directory.Exists(BusEngine.Engine.EngineDirectory)) {
					BusEngine.Engine.EngineDirectory = BusEngine.Engine.ExeDirectory + "Bin\\";
				}
			}

			BusEngine.Engine.EngineDirectory = System.IO.Path.GetFullPath(BusEngine.Engine.EngineDirectory + "..\\");

			BusEngine.Engine.BinDirectory = BusEngine.Engine.EngineDirectory + "Bin\\";
			BusEngine.Engine.CodeDirectory = BusEngine.Engine.EngineDirectory + "Code\\";
			BusEngine.Engine.DataDirectory = BusEngine.Engine.EngineDirectory + "Data\\";
			BusEngine.Engine.EditorDirectory = BusEngine.Engine.EngineDirectory + "Editor\\";
			BusEngine.Engine.LocalizationDirectory = BusEngine.Engine.EngineDirectory + "Localization\\";
			BusEngine.Engine.LogDirectory = BusEngine.Engine.EngineDirectory + "Log\\";
			BusEngine.Engine.ToolsDirectory = BusEngine.Engine.EngineDirectory + "Tools\\";

			// ищем зависимости
			/* System.AppDomain.CurrentDomain.AssemblyLoad  += new System.AssemblyLoadEventHandler((o, e) => {
				BusEngine.Log.Info("AssemblyLoad... {0}", e.LoadedAssembly.FullName);
			}); */
			/* System.AppDomain.CurrentDomain.AssemblyResolve += new System.ResolveEventHandler((o, e) => {
				BusEngine.Log.Info("AssemblyResolve... {0}", e.Name);

				foreach (string i in GetProbingPath()) {
					foreach (string currentFile in System.IO.Directory.EnumerateFiles(BusEngine.Engine.ExeDirectory + i, e.Name.Split(',')[0] + ".dll", System.IO.SearchOption.AllDirectories)) {
						if (System.IO.File.Exists(currentFile)) {
							return System.Reflection.Assembly.LoadFile(currentFile);
						}
					}
				}

				return System.Reflection.Assembly.LoadFile(e.Name);
			}); */
			/* System.AppDomain.CurrentDomain.ResourceResolve += new System.ResolveEventHandler((o, e) => {
				BusEngine.Log.Info("ResourceResolve... {0}", e.Name);
			}); */
			/* System.AppDomain.CurrentDomain.FirstChanceException += (o, e) => {
				BusEngine.Log.Info("FirstChanceException... {0}", e.Exception);
				BusEngine.Log.Info("FirstChanceException2... {0}", e.Exception.Message);
			}; */
			/* System.AppDomain.CurrentDomain.ReflectionOnlyAssemblyResolve += new System.ResolveEventHandler((o, e) => {
				BusEngine.Log.Info("ReflectionOnlyAssemblyResolve... {0}", e.LoadedAssembly.FullName);
			}); */
			/* System.AppDomain.CurrentDomain.TypeResolve += new System.ResolveEventHandler((o, e) => {
				BusEngine.Log.Info("TypeResolve... {0}", e.LoadedAssembly.FullName);
			}); */
			/* System.AppDomain.CurrentDomain.UnhandledException += new System.UnhandledExceptionEventHandler((o, e) => {
				BusEngine.Log.Info("UnhandledException... {0}", e.ExceptionObject);
			}); */

			// https://learn.microsoft.com/ru-ru/dotnet/standard/base-types/best-practices-strings
			// https://metanit.com/sharp/tutorial/5.4.php
			// https://metanit.com/sharp/tutorial/5.5.php
			// https://metanit.com/sharp/tutorial/6.4.php
			// https://dir.by/developer/csharp/serialization_json/?lang=eng
			// ищем, загружаем и обрабатываем настройки проекта
			// https://learn.microsoft.com/en-us/dotnet/api/system.io.memorymappedfiles.memorymappedfile?redirectedfrom=MSDN&view=net-7.0
			string[] project_files;

			project_files = System.IO.Directory.GetFiles(BusEngine.Engine.EngineDirectory, "*.busproject", System.IO.SearchOption.TopDirectoryOnly);

			if (project_files.Length == 0) {
				project_files = System.IO.Directory.GetFiles(BusEngine.Engine.EngineDirectory, "busengine.busengine", System.IO.SearchOption.TopDirectoryOnly);
			}

			System.Collections.Generic.Dictionary<string, dynamic> setting;
			BusEngine.ProjectDefault settingDefaultO = new BusEngine.ProjectDefault();
			System.Collections.Generic.Dictionary<string, dynamic> settingDefault = settingDefaultO.Setting;
			settingDefaultO.Dispose();

			if (project_files.Length == 0) {
				BusEngine.Engine.NameProject = System.IO.Path.GetFileNameWithoutExtension(assembly.Location);

				settingDefault["info"]["name"] = BusEngine.Engine.NameProject;
				//https://www.appsloveworld.com/csharp/100/6/how-do-i-programmatically-get-the-guid-of-an-application-in-c-with-net
				settingDefault["info"]["guid"] = ((System.Runtime.InteropServices.GuidAttribute)assembly.GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false)[0]).Value;
				settingDefault["require"]["engine"] =  System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

				// запись
				using (System.IO.FileStream fstream = System.IO.File.OpenWrite(BusEngine.Engine.EngineDirectory + BusEngine.Engine.NameProject + ".busproject")) {
					byte[] buffer = System.Text.Encoding.UTF8.GetBytes(BusEngine.Tools.Json.Encode(settingDefault));
					fstream.Write(buffer, 0, buffer.Length);
				}

				// улаляем массивы данных по умолчанию т.к. они не нужны
				settingDefault["require"]["plugins"].Clear();

				setting = BusEngine.Tools.Json.Decode(BusEngine.Tools.Json.Encode(settingDefault));
			} else {
				BusEngine.Engine.NameProject = System.IO.Path.GetFileNameWithoutExtension(project_files[0]);

				// улаляем массивы данных по умолчанию т.к. они не нужны
				settingDefault["require"]["plugins"].Clear();

				// получаем новые данные
				setting = BusEngine.Tools.Json.Decode(System.IO.File.ReadAllText(project_files[0]));
				//System.Collections.Generic.Dictionary<string, dynamic> setting = BusEngine.Tools.Json.Decode(System.Text.Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(project_files[0])));
			}

			//project_files = null;

			dynamic content;

			if (setting.TryGetValue("content", out content) && content.GetType().GetProperty("Count") != null) {
				foreach (dynamic i in content) {
					if (i is object) {
						string n = i.Name.ToString();
						string v = i.Value.ToString();

						settingDefault["content"][n] = v;

						/* if (v.IndexOf("[bin]", System.StringComparison.OrdinalIgnoreCase) != -1) {
							v = v.Replace("[bin]", BusEngine.Engine.BinDirectory);
						} else if (v.IndexOf("[code]", System.StringComparison.OrdinalIgnoreCase) != -1) {
							v = v.Replace("[code]", BusEngine.Engine.CodeDirectory);
						} else if (v.IndexOf("[data]", System.StringComparison.OrdinalIgnoreCase) != -1) {
							v = v.Replace("[data]", BusEngine.Engine.DataDirectory);
						} else if (v.IndexOf("[editor]", System.StringComparison.OrdinalIgnoreCase) != -1) {
							v = v.Replace("[editor]", BusEngine.Engine.EditorDirectory);
						} else if (v.IndexOf("[engine]", System.StringComparison.OrdinalIgnoreCase) != -1) {
							v = v.Replace("[engine]", BusEngine.Engine.EngineDirectory);
						} else if (v.IndexOf("[exe]", System.StringComparison.OrdinalIgnoreCase) != -1) {
							v = v.Replace("[exe]", BusEngine.Engine.ExeDirectory);
						} else if (v.IndexOf("[localization]", System.StringComparison.OrdinalIgnoreCase) != -1) {
							v = v.Replace("[localization]", BusEngine.Engine.LocalizationDirectory);
						} else if (v.IndexOf("[log]", System.StringComparison.OrdinalIgnoreCase) != -1) {
							v = v.Replace("[log]", BusEngine.Engine.LogDirectory);
						} else if (v.IndexOf("[tools]", System.StringComparison.OrdinalIgnoreCase) != -1) {
							v = v.Replace("[tools]", BusEngine.Engine.ToolsDirectory);
						} */

						if (v.IndexOf(":") == -1) {
							v = BusEngine.Engine.EngineDirectory + v;
						}

						if (n == "exe") {
							BusEngine.Engine.ExeDirectory = v + "\\";
						} else if (n == "bin") {
							BusEngine.Engine.BinDirectory = v + "\\";
						} else if (n == "code") {
							BusEngine.Engine.CodeDirectory = v + "\\";
						} else if (n == "data") {
							BusEngine.Engine.DataDirectory = v + "\\";
						} else /* if (n == "editor") {
							BusEngine.Engine.EditorDirectory = v + "\\";
						} else */ if (n == "localization") {
							BusEngine.Engine.LocalizationDirectory = v + "\\";
						} else if (n == "log") {
							BusEngine.Engine.LogDirectory = v + "\\";
						} else if (n == "tools") {
							BusEngine.Engine.ToolsDirectory = v + "\\";
						}
					}
				}
			}

			//content = null;

			dynamic console_commands;

			if (setting.TryGetValue("console_commands", out console_commands) && console_commands.GetType().GetProperty("Count") != null) {
				foreach (dynamic i in console_commands) {
					if (i is object) {
						settingDefault["console_commands"][i.Name.ToString()] = i.Value.ToString();
					}
				}
			}

			//console_commands = null;

			dynamic console_variables;

			if (setting.TryGetValue("console_variables", out console_variables) && console_variables.GetType().GetProperty("Count") != null) {
				foreach (dynamic i in console_variables) {
					if (i is object) {
						settingDefault["console_variables"][i.Name.ToString()] = i.Value.ToString();
					}
				}
			}

			//console_variables = null;

			dynamic info;

			if (setting.TryGetValue("info", out info) && info.GetType().GetProperty("Count") != null) {
				foreach (dynamic i in info) {
					if (i is object) {
						string n = i.Name.ToString();
						string v = i.Value.ToString();

						if (n == "icon") {
							settingDefault["info"][n] = (v)
							.Replace("[bin]", BusEngine.Engine.BinDirectory)
							.Replace("[code]", BusEngine.Engine.CodeDirectory)
							.Replace("[data]", BusEngine.Engine.DataDirectory)
							.Replace("[editor]", BusEngine.Engine.EditorDirectory)
							.Replace("[engine]", BusEngine.Engine.EngineDirectory)
							.Replace("[exe]", BusEngine.Engine.ExeDirectory)
							.Replace("[localization]", BusEngine.Engine.LocalizationDirectory)
							.Replace("[log]", BusEngine.Engine.LogDirectory)
							.Replace("[tools]", BusEngine.Engine.ToolsDirectory);
						} else {
							settingDefault["info"][n] = v;
						}
					}
				}
			}

			if (settingDefault["info"]["name"] == "") {
				settingDefault["info"]["name"] = BusEngine.Engine.NameProject;
			}

			if (settingDefault["info"]["guid"] == "") {
				settingDefault["info"]["guid"] = ((System.Runtime.InteropServices.GuidAttribute)assembly.GetCustomAttributes(typeof(System.Runtime.InteropServices.GuidAttribute), false)[0]).Value;
			}

			//info = null;

			dynamic require;

			if (setting.TryGetValue("require", out require) && require.GetType().GetProperty("Count") != null) {
				if (require.ContainsKey("engine") && require["engine"].GetType().GetProperty("Count") == null && require["engine"].Type.ToString() == "String") {
					settingDefault["require"]["engine"] = require["engine"];
				}

				if (require.ContainsKey("plugins") && require["plugins"].Type.ToString() == "Array") {
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
								settingDefault["require"]["plugins"].Add(new System.Collections.Generic.Dictionary<string, object>(4) {
									{"path", System.Convert.ToString(require["plugins"][i]["path"])},
									{"guid", (require["plugins"][i].ContainsKey("guid") && require["plugins"][i]["guid"].Type.ToString() == "String" ? System.Convert.ToString(require["plugins"][i]["guid"]) : "")},
									{"type", (require["plugins"][i].ContainsKey("type") && require["plugins"][i]["type"].Type.ToString() == "String" ? System.Convert.ToString(require["plugins"][i]["type"]) : "")},
									{"platforms", (require["plugins"][i].ContainsKey("platforms") && require["plugins"][i]["platforms"].GetType().GetProperty("Count") != null ? require["plugins"][i]["platforms"] : settingDefault["require"]["plugins"][i]["platforms"])}
								});
							}
						}
					}
				}
			}

			if (settingDefault["require"]["engine"] == "") {
				settingDefault["require"]["engine"] = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
			}

			//require = null;

			//setting.Clear();
			//setting = null;

			//BusEngine.Engine.SettingEngine = settingDefault;
			BusEngine.Engine.SettingProject = settingDefault;

			//settingDefault = null;

			// инициализируем язык
			//BusEngine.Log.ConsoleShow();
			new BusEngine.Localization().Load();

			// включаем консоль
			int r_DisplayInfo = System.Convert.ToInt32(BusEngine.Engine.SettingProject["console_commands"]["r_DisplayInfo"]);

			if (r_DisplayInfo > 0) {
				BusEngine.Log.ConsoleShow();

				if (r_DisplayInfo > 1) {
					BusEngine.Log.Info("Device UserAgent: {0}", BusEngine.Engine.Device.UserAgent);
					BusEngine.Log.Info("Device OS: {0}", BusEngine.Engine.Device.Name);
					BusEngine.Log.Info("Device Version OS: {0}", BusEngine.Engine.Device.Version);
					BusEngine.Log.Info("Device Processor: {0}", BusEngine.Engine.Device.Processor);
					BusEngine.Log.Info("Device ProcessorCount: {0}", BusEngine.Engine.Device.ProcessorCount);
					BusEngine.Log.Info("Language file: {0}", BusEngine.Localization.LanguageStatic);
					// https://csharp.webdelphi.ru/kak-izmerit-vremya-vypolneniya-operacii-v-c/
					//BusEngine.Log.Info("Time: {0}", BusEngine.Localization.LanguageStatic);
				}
			}

			//r_DisplayInfo = 0;

			// чистим память в автоматическом режиме
			//System.IntPtr hglobal = System.Runtime.InteropServices.Marshal.AllocHGlobal(1024*1024*1000);
			if (BusEngine.Engine.SettingProject["console_commands"]["sys_MemoryClearAuto"] == "1" && BusEngine.Engine.Timer == null) {
				int sys_MemoryClearTime;
				if (!int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["sys_MemoryClearTime"], out sys_MemoryClearTime) || sys_MemoryClearTime < 1) {
					sys_MemoryClearTime = 1;
				}
				BusEngine.Engine.Timer = new System.Timers.Timer(sys_MemoryClearTime*1000);
				System.Timers.ElapsedEventHandler onTime = null;
				onTime = (o, e) => {
					if (BusEngine.Engine.IsShutdown) {
						BusEngine.Engine.Timer.Elapsed -= onTime;
						BusEngine.Engine.Timer.Dispose();
					}

					System.GC.Collect();
					System.GC.WaitForPendingFinalizers();
					System.GC.Collect();

					//BusEngine.Log.Info("Вызов сборщика мусора. {0}", BusEngine.Engine.Timer);
				};
				BusEngine.Engine.Timer.Elapsed += onTime;
				BusEngine.Engine.Timer.AutoReset = true;
				BusEngine.Engine.Timer.Enabled = true;
			}

			#if BUSENGINE_BENCHMARK
			}
			#endif
		}

		/** функция запуска API BusEngine */
		public static void Initialize() {
			if (_instance == null) {
				_instance = new Engine();
			}

			// инициализируем плагины
			BusEngine.Engine.IsShutdown = false;
			new BusEngine.IPlugin("Initialize");

			// запускаем окно BusEngine
			if (OnInitialize != null && !BusEngine.Engine.IsShutdown) {
				OnInitialize.Invoke();
			}
		}

		public static void Initialize(string Platform = null, string[] Commands = null, EngineHandler OnInitialize = null, EngineHandler OnShutdown = null) {
			if (Platform != null) {
				BusEngine.Engine.Platform = Platform;
			}
			if (Commands != null) {
				BusEngine.Engine.Commands = Commands;
			}
			if (OnInitialize != null) {
				BusEngine.Engine.OnInitialize = OnInitialize;
			}
			if (OnShutdown != null) {
				BusEngine.Engine.OnShutdown = OnShutdown;
			}

			BusEngine.Engine.Initialize();
		}
		/** функция запуска API BusEngine */

		/** функция остановки API BusEngine  */
		public static void Shutdown() {
			BusEngine.Engine.ShutdownStatic(false);
		}

		public static void Shutdown(bool nagibator = false) {
			BusEngine.Engine.ShutdownStatic(nagibator);
		}

		public static void ShutdownStatic() {
			BusEngine.Engine.ShutdownStatic(false);
		}

		public static void ShutdownStatic(bool nagibator = false) {
			if (_instance != null) {
				_instance.Dispose();
				_instance = null;
			}
			// отключаем плагины
			new BusEngine.IPlugin("Shutdown");

			// закрываем окно BusEngine
			if (BusEngine.Engine.OnShutdown != null) {
				BusEngine.Engine.OnShutdown.Invoke();
			}

			if (nagibator) {
				System.Environment.Exit(0);
			}

			// предотвращаем повторное использование
			BusEngine.Engine.IsShutdown = true;
		}

		public void Dispose() {
			BusEngine.Log.Info("Engine Stop");
		}

		~Engine() {
			BusEngine.Log.Info("Engine Stop ~");
			BusEngine.Experemental.Log.File("Engine Stop ~");
		}
		/** функция остановки API BusEngine  */
	}
	/** API BusEngine.Engine */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine
*/
	/** API BusEngine.FlowGraph */
	public class FlowGraph : System.IDisposable {
		public void Dispose() {

		}

		~FlowGraph() {
			BusEngine.Log.Info("FlowGraph ~");
		}
	}
	/** API BusEngine.FlowGraph */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.Log
*/
	/** API BusEngine.Layer */
	public class Layer : System.IDisposable {
		public void Dispose() {

		}

		~Layer() {
			BusEngine.Log.Info("Layer ~");
		}
	}
	/** API BusEngine.Layer */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.Log
*/
	/** API BusEngine.Level */
	public class Level : System.IDisposable {
		public void Dispose() {

		}

		~Level() {
			BusEngine.Log.Info("Level ~");
		}
	}
	/** API BusEngine.Level */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
	public class Localization {
		//[BusEngine.Tooltip("Loading a language if the desired one is not available.", "English")]
		public string LanguageDefault = "Belarusian";
		public string Language;
		public string File = "";
		public string Format = "cfg";
		public delegate void OnLoadHandler(Localization sender, string language);
		public event OnLoadHandler OnLoad;
		public static event OnLoadHandler OnLoadStatic;
		// https://learn.microsoft.com/ru-ru/dotnet/standard/collections/thread-safe/how-to-add-and-remove-items
		internal static System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Concurrent.ConcurrentDictionary<string, string>> GetLanguages;
		private static string Value;
		private static string _LanguageStatic = "Belarusian";
		public static string LanguageStatic {
			get {
				return _LanguageStatic;
			} private set {
				_LanguageStatic = value;
			}
		}

		public Localization() {

		}

		public Localization(string language = null) : this() {
			this.Language = language;
		}

		public static string GetLanguageStatic(string key) {
			if (GetLanguages != null && GetLanguages.ContainsKey(LanguageStatic) && GetLanguages[LanguageStatic].TryGetValue(key, out Value)) {
				return Value;
			} else {
				return key;
			}
		}

		/* public static string GetLanguageStatic(string key) {
			if (GetLanguages != null) {
				foreach (System.Collections.Generic.KeyValuePair<string, dynamic> i in GetLanguages) {
					if (i.Value.TryGetValue(key, out Value)) {
						return Value;
					}
				}
			}

			return key;
		} */

		public static void SetLanguageStatic(string key, string value) {
			if (GetLanguages != null && GetLanguages.ContainsKey(LanguageStatic)) {
				GetLanguages[LanguageStatic][key] = value;
			}
		}

		public string GetLanguage(string key) {
			if (GetLanguages != null && GetLanguages.ContainsKey(LanguageStatic) && GetLanguages[LanguageStatic].TryGetValue(key, out Value)) {
				return Value;
			} else {
				return key;
			}
		}

		public void SetLanguage(string key, string value) {
			if (GetLanguages != null && GetLanguages.ContainsKey(LanguageStatic)) {
				GetLanguages[LanguageStatic][key] = value;
			}
		}

		public Localization Load() {
			return this.Load(this.Language);
		}

		public Localization Load(string language = null) {
			StartLocalization(language);

			return this;
		}

		private void StartLocalization(string Language = null) {
			string path = BusEngine.Engine.LocalizationDirectory, file = "", files = "";

			if (BusEngine.Engine.Platform.IndexOf("Windows", System.StringComparison.OrdinalIgnoreCase) != -1 || 1 == 1) {
				if (!System.IO.Directory.Exists(path)) {
					path = path.Replace(BusEngine.Engine.EngineDirectory, BusEngine.Engine.DataDirectory);
				}
			} else {
				if (BusEngine.Engine.Platform == "WebGL") {
					path = "Localization/";
				} else {
					path = BusEngine.Engine.DataDirectory + "/Localization/";
				}
			}

			if (Language == null || Language == "") {
				Language = System.Globalization.CultureInfo.CurrentCulture.EnglishName.ToString();
			}

			if (File != "") {
				file = "/" + File.Replace("/", "");
			}

			if (Format != "") {
				file = "." + Format;
			}

			if (System.IO.File.Exists(path + Language + file)) {
				files = System.IO.File.ReadAllText(path + Language + file);
				//files = System.Text.Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(path + Language + file));
			} else {
				Language = LanguageDefault;
				if (System.IO.File.Exists(path + Language + file)) {
					files = System.IO.File.ReadAllText(path + Language + file);
					//files = System.Text.Encoding.UTF8.GetString(System.IO.File.ReadAllBytes(path + Language + file));
				}
			}

			LanguageStatic = Language;

			if (files != "") {
				string[] lines, pairs;
				int i, l;
				char[] c = new char[] {'='};

				lines = files.Split(new string[] {"\r\n", "\n\r", "\n"}, System.StringSplitOptions.RemoveEmptyEntries);
				l = lines.Length;

				if (GetLanguages == null) {
					GetLanguages = new System.Collections.Concurrent.ConcurrentDictionary<string, System.Collections.Concurrent.ConcurrentDictionary<string, string>>(System.Environment.ProcessorCount, 1);
				}

				if (!GetLanguages.ContainsKey(Language)) {
					GetLanguages[Language] = new System.Collections.Concurrent.ConcurrentDictionary<string, string>(System.Environment.ProcessorCount, l);
				}

				for (i = 0; i < l; i++) {
					pairs = lines[i].Split(c, 2);

					if (pairs.Length == 2) {
						GetLanguages[Language][pairs[0].Trim()] = pairs[1].Trim();
					}
				}
			}

			if (OnLoad != null) {
				OnLoad.Invoke(this, Language);
			}

			if (OnLoadStatic != null) {
				OnLoadStatic.Invoke(this, Language);
			}

			//System.GC.SuppressFinalize(this);
		}

		public void Shutdown() {
			GetLanguages.Clear();
		}

		public static void ShutdownStatic() {
			GetLanguages.Clear();
		}

		public void Dispose() {

		}

		~Localization() {
			//BusEngine.Log.Info("Localization ~");
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
		public static bool ConsoleStatus = false;

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

		[System.Runtime.InteropServices.DllImport("kernel32.dll", SetLastError = true)]
		private static extern bool SetConsoleDisplayMode(System.IntPtr ConsoleHandle, uint Flags, System.IntPtr NewScreenBufferDimensions);

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
			if (BusEngine.Log.ConsoleStatus == false) {
				//https://ru.stackoverflow.com/questions/1098793/%D0%A1%D0%BE%D1%85%D1%80%D0%B0%D0%BD%D0%B5%D0%BD%D0%B8%D0%B5-%D0%B4%D0%B0%D0%BD%D0%BD%D1%8B%D1%85-%D0%B2-%D0%BA%D0%BE%D0%BD%D1%81%D0%BE%D0%BB%D1%8C%D0%BD%D0%BE%D0%BC-%D0%BF%D1%80%D0%B8%D0%BB%D0%BE%D0%B6%D0%B5%D0%BD%D0%B8%D0%B8-net-framework-c
				/* System.Windows.Forms.MessageBox.Show("Сообщение из Windows Forms!"); */

				BusEngine.Log.AttachConsole(-1);
				BusEngine.Log.AllocConsole();
				BusEngine.Log.ConsoleStatus = true;

				//System.Console.WindowHeight = System.Console.LargestWindowHeight;
				//System.Console.WindowWidth = System.Console.LargestWindowWidth;
				//System.Console.WindowTop = 0;
				//System.Console.WindowLeft = 0;
				//System.Console.SetBufferSize(System.Console.WindowWidth, System.Console.WindowHeight);

				System.Console.Title = BusEngine.Localization.GetLanguageStatic("text_name_console") + " v" + BusEngine.Engine.SettingProject["require"]["engine"];
				BusEngine.Localization.OnLoadStatic += OnLoadLanguage;
				System.Console.CancelKeyPress += new System.ConsoleCancelEventHandler(BusEngine.Log.MyHandler);

				//System.Console.Clear();
				//BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("text_name_console"));
				//while (true) {
					//break;
					//if (System.Console.ReadKey(true).Key == System.ConsoleKey.Enter) {
						//string command = System.Console.ReadLine();
						//BusEngine.Log.Info($"Вы ввели команду: {command}");
					//}
					/* if (command == "start" || System.Console.ReadKey().Key == System.ConsoleKey.Oem3) {
						BusEngine.Log.ConsoleHide();
						break;
					} else {
						
					} */
				//}

				/* System.Console.In.ReadLine(); */
				/* System.Console.Read(); */
				/* System.Console.ReadKey(); */
			}
		}

		// функция остановки консоли
		public static void ConsoleHide() {
			if (BusEngine.Log.ConsoleStatus == true) {
				//BusEngine.Log.Info(new System.IO.StreamWriter(System.Console.OpenStandardOutput(), System.Console.OutputEncoding) { AutoFlush = true });
				//BusEngine.Log.Info(new System.IO.StreamReader(System.Console.OpenStandardInput(), System.Console.InputEncoding));
				BusEngine.Log.FreeConsole();
				BusEngine.Log.ConsoleStatus = false;
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
			if (BusEngine.Log.ConsoleStatus == false) {
				BusEngine.Log.ConsoleShow();
			} else {
				BusEngine.Log.ConsoleHide();
			}
		}

		/** событие загрузки языка */
		private static void OnLoadLanguage(BusEngine.Localization l, string language) {
			System.Console.Title = l.GetLanguage("text_name_console") + " v" + BusEngine.Engine.SettingProject["require"]["engine"];
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
		
		public static string Text = "";

		// функция вывода строки в консоль
		public static void Info() {
			System.Console.WriteLine();
			//Text += (string)System.Console.ReadLine();
		}
		public static void Info(System.Type args1) {
			System.Console.WriteLine(args1);
			//Text += (string)System.Console.ReadLine();
			#if LOG_TYPE
			System.Console.WriteLine("System.Type");
			#endif
		}
		public static void Info(string args1) {
			System.Console.WriteLine(args1);
			//Text += (string)System.Console.ReadLine();
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
			//Text += (string)System.Console.ReadLine();
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

		private static void Debug() {
			//System.Type myType = System.Type.GetType("System.Windows.Forms.MyMethod");
			//System.Reflection.MethodInfo myMethod = myType.GetMethod("MyMethod");
			System.Type myType = System.Type.GetType("LibVLCSharp.Shared.Media");
			BusEngine.Log.Info(myType);

			System.Version version = System.Environment.Version;

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

		public static void Shutdown() {
			BusEngine.Log.ConsoleHide();
		}

		public void Dispose() {
			BusEngine.Log.ConsoleHide();
		}

		~Log() {
			BusEngine.Log.Info("Log ~");
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
BusEngine.Log
*/
	/** API BusEngine.Material */
	public class Material : System.IDisposable {
		public void Dispose() {

		}

		~Material() {
			BusEngine.Log.Info("Material ~");
		}
	}
	/** API BusEngine.Material */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.Log
*/
	/** API BusEngine.Model */
	public class Model : System.IDisposable {
		public void Dispose() {

		}

		~Model() {
			BusEngine.Log.Info("Model ~");
		}
	}
	/** API BusEngine.Model */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.Log
*/
	/** API BusEngine.Physics */
	public class Physics : System.IDisposable {
		public void Dispose() {

		}

		~Physics() {
			BusEngine.Log.Info("Physics ~");
		}
	}
	/** API BusEngine.Physics */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
	/** API BusEngine.Plugin */
	public abstract class Plugin : System.IDisposable {
		// при запуске BusEngine до создания формы
		public virtual void Initialize() {BusEngine.Log.Info("BASE Plugin Initialize");}
		public virtual void InitializeAsync() {}

		// после загрузки определённого плагина
		public virtual void Initialize(string plugin) {}
		public virtual void InitializeAsync(string plugin) {}
		public virtual void Initialize(string plugin, string method) {}
		public virtual void InitializeAsync(string plugin, string method) {}

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

		public void Dispose() {
			BusEngine.Log.Info("Plugin Dispose {0}");
		}

		~Plugin() {
			//BusEngine.Log.Info("Plugin ~");
		}
	}
	/** API BusEngine.Plugin */

	/** API BusEngine.IPlugin */
	internal class IPlugin : System.IDisposable {
		public static System.Collections.Concurrent.ConcurrentDictionary<string, System.Type[]> Modules;
		public static System.Collections.Concurrent.ConcurrentDictionary<string, string> Moduless;
		private static string IsShutdown;
		private bool IsAsync(System.Reflection.MethodInfo method) {
			foreach (object o in method.GetCustomAttributes(typeof(System.Runtime.CompilerServices.AsyncStateMachineAttribute), false)) {
				return true;
			}

			return false;
		}
		private string Stage;
		//private delegate int Operation();

		// при запуске BusEngine до создания формы
		public IPlugin(string stage = "Initialize") {
			if (BusEngine.Engine.IsShutdown && stage != "Shutdown") {
				return;
			}
			Stage = stage;
			BusEngine.Log.Info( "============================ System Plugins Start ============================" );

			int i, i2, i3, l = BusEngine.Engine.SettingProject["require"]["plugins"].Count;
			string m, path;
			object[] x1 = new object[0];
			object[] x2 = new object[1];
			object[] x3 = new object[2];
			System.Type[] t1 = new System.Type[] {};
			System.Type[] t2 = new System.Type[] { typeof(string) };
			System.Type[] t3 = new System.Type[] { typeof(string), typeof(string) };
			stage = stage.ToLower();
			int typ = 1;
			
			if (typ == 1 && Modules == null) {
				#if BUSENGINE_BENCHMARK
				using (new BusEngine.Benchmark("Modules " + stage + " " + System.Threading.Thread.CurrentThread.ManagedThreadId)) {
				#endif

					string ap;
					Modules = new System.Collections.Concurrent.ConcurrentDictionary<string, System.Type[]>(System.Environment.ProcessorCount, l);

					//System.Collections.Generic.List<System.Threading.Tasks.Task> tasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();
					System.Threading.Tasks.Task[] tasks = new System.Threading.Tasks.Task[l];

					for (i = 0; i < l; ++i) {
						int g = i;
						tasks[g] = System.Threading.Tasks.Task.Factory.StartNew(() => {
							ap = BusEngine.Engine.SettingProject["require"]["plugins"][g]["path"];
							//if (!Modules.ContainsKey(ap)) {
								Modules[ap] = System.Reflection.Assembly.LoadFile(ap).GetTypes();
							//}
						}, System.Threading.Tasks.TaskCreationOptions.AttachedToParent);
					}

					System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Factory.ContinueWhenAll(tasks, wordCountTasks => {
						//BusEngine.Log.Info("Modules {0}", wordCountTasks.Length);
						foreach (System.Threading.Tasks.Task tt in wordCountTasks) {
							tt.Dispose();
						}
						System.Array.Clear(wordCountTasks, 0, wordCountTasks.Length);
						wordCountTasks = null;
						foreach (System.Threading.Tasks.Task tt in tasks) {
							tt.Dispose();
						}
						//BusEngine.Log.Info("Modules {0}", tasks.Count);
						//tasks.Clear();
						System.Array.Clear(tasks, 0, tasks.Length);
						tasks = null;
					});
					task.Wait();
					task.Dispose();
					ap = null;
					//System.Threading.Tasks.Task.Dispose();

				#if BUSENGINE_BENCHMARK
				}
				#endif
			} else if (typ == 2 && Modules == null) {
				#if BUSENGINE_BENCHMARK
				using (new BusEngine.Benchmark("Modules " + stage + " " + System.Threading.Thread.CurrentThread.ManagedThreadId)) {
				#endif

					string ap;
					Modules = new System.Collections.Concurrent.ConcurrentDictionary<string, System.Type[]>(System.Environment.ProcessorCount, l);

					System.Collections.Generic.List<System.Threading.Tasks.Task> tasks = new System.Collections.Generic.List<System.Threading.Tasks.Task>();

					for (i = 0; i < l; ++i) {
						int g = i;
						tasks.Add(System.Threading.Tasks.Task.Run(() => {
							ap = BusEngine.Engine.SettingProject["require"]["plugins"][g]["path"];
							//if (!Modules.ContainsKey(ap)) {
								Modules[ap] = System.Reflection.Assembly.LoadFile(ap).GetTypes();
							//}
						}));
					}

					System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Factory.ContinueWhenAll(tasks.ToArray(), (wordCountTasks) => {
						BusEngine.Log.Info("Modules {0}", Modules.Count);
						BusEngine.Log.Info("Modules {0}", wordCountTasks.Length);
						/* foreach (System.Threading.Tasks.Task tt in wordCountTasks) {
							tt.Dispose();
						} */
						System.Array.Clear(wordCountTasks, 0, wordCountTasks.Length);
						wordCountTasks = null;
						/* foreach (System.Threading.Tasks.Task tt in tasks) {
							tt.Dispose();
						} */
						//BusEngine.Log.Info("Modules {0}", tasks.Count);
						tasks.Clear();
						tasks = null;
						ap = null;
					});
					task.Wait();
					task.Dispose();

				#if BUSENGINE_BENCHMARK
				}
				#endif
			} else if (typ == 3 && Modules == null) {
				#if BUSENGINE_BENCHMARK
				using (new BusEngine.Benchmark("Modules " + stage + " " + System.Threading.Thread.CurrentThread.ManagedThreadId)) {
				#endif

					Modules = new System.Collections.Concurrent.ConcurrentDictionary<string, System.Type[]>(System.Environment.ProcessorCount, l, System.StringComparer.OrdinalIgnoreCase);

					System.Threading.Tasks.Task<dynamic>[] tasks = new System.Threading.Tasks.Task<dynamic>[l];

					for (i = 0; i < l; ++i) {
						tasks[i] = System.Threading.Tasks.Task.Factory.StartNew((ifx) => {
							return System.Reflection.Assembly.LoadFile(BusEngine.Engine.SettingProject["require"]["plugins"][(int)ifx]["path"]).GetTypes();
						}, i/* , System.Threading.Tasks.TaskCreationOptions.AttachedToParent */);
					}

					System.Threading.Tasks.Task.WaitAll(tasks);

					for (i = 0; i < l; ++i) {
						Modules[BusEngine.Engine.SettingProject["require"]["plugins"][i]["path"]] = tasks[i].Result;
					}

				#if BUSENGINE_BENCHMARK
				}
				#endif
			} else if (typ == 4 && Modules == null) {
				#if BUSENGINE_BENCHMARK
				using (new BusEngine.Benchmark("Modules " + stage + " " + System.Threading.Thread.CurrentThread.ManagedThreadId)) {
				#endif

					string ap;
					Modules = new System.Collections.Concurrent.ConcurrentDictionary<string, System.Type[]>(System.Environment.ProcessorCount, l);

					for (i = 0; i < l; ++i) {
						ap = BusEngine.Engine.SettingProject["require"]["plugins"][i]["path"];
						//if (!Modules.ContainsKey(ap)) {
							Modules[ap] = System.Reflection.Assembly.LoadFile(ap).GetTypes();
						//}
					}

					ap = null;

				#if BUSENGINE_BENCHMARK
				}
				#endif
			}

			for (i = 0; i < l; ++i) {
				path = BusEngine.Engine.SettingProject["require"]["plugins"][i]["path"];
				if (BusEngine.Engine.IsShutdown && stage == "shutdown" && BusEngine.IPlugin.IsShutdown != "" && BusEngine.IPlugin.IsShutdown != path) {
					continue;
				}
				#if BUSENGINE_BENCHMARK
				using (new BusEngine.Benchmark(path + " " + stage)) {
				#endif
					// https://learn.microsoft.com/ru-ru/dotnet/framework/deployment/best-practices-for-assembly-loading
					foreach (System.Type type in Modules[path]) {
						if (type.IsSubclassOf(typeof(BusEngine.Plugin))) {
							foreach (System.Reflection.MethodInfo method in type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly)) {
								m = method.Name.ToLower();
								if (m == stage || m == stage + "async") {
									if (BusEngine.Log.ConsoleStatus == true) {
										BusEngine.Log.Info(path);
										BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("text_name_class") + ": {0}", type.FullName);
										BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("text_name_method") + ": {0}", method.Name);
									}

									if (m == stage + "async" || IsAsync(method)) {
										BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("text_name_method_start") + ": {0}", "Async");
										// https://learn.microsoft.com/ru-ru/dotnet/api/system.threading.thread?view=net-7.0
										// https://www.youtube.com/watch?v=D9qcKV4j75U&list=PLWCoo5SF-qAMDIAqikhB2hvIytrMiR5TC
										System.Threading.ThreadPool.QueueUserWorkItem((object stateInfo) => {
										//System.Threading.Thread thread = new System.Threading.Thread(() => {
										// https://learn.microsoft.com/ru-ru/dotnet/api/system.threading.tasks.task?view=net-7.0
										//System.Threading.Tasks.Task.Run(() => {
											i2 = method.GetParameters().Length;
											if (i2 == 0) {
												method.Invoke(System.Activator.CreateInstance(type), null);
												if (stage == "initialize") {
													for (i3 = 0; i3 < l; ++i3) {
														path = BusEngine.Engine.SettingProject["require"]["plugins"][i3]["path"];
														foreach (System.Type tp in Modules[path]) {
															if (tp.IsSubclassOf(typeof(BusEngine.Plugin))) {
																System.Reflection.MethodInfo md = tp.GetMethod("initialize", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.IgnoreCase, null, t2, null);
																if (md != null) {
																	x2[0] = path;
																	md.Invoke(System.Activator.CreateInstance(tp), x2);
																}
																md = tp.GetMethod("initialize", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.IgnoreCase, null, t3, null);
																if (md != null) {
																	x3[0] = path;
																	x3[1] = stage;
																	md.Invoke(System.Activator.CreateInstance(tp), x3);
																}
															}
														}
													}
												}
											}
											if (stage != "initialize") {
												for (i3 = 0; i3 < l; ++i3) {
													path = BusEngine.Engine.SettingProject["require"]["plugins"][i3]["path"];
													foreach (System.Type tp in Modules[path]) {
														if (tp.IsSubclassOf(typeof(BusEngine.Plugin))) {
															System.Reflection.MethodInfo md = tp.GetMethod("initialize", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.IgnoreCase, null, t3, null);
															if (md != null) {
																x3[0] = path;
																x3[1] = stage;
																md.Invoke(System.Activator.CreateInstance(tp), x3);
															}
														}
													}
												}
											}
										});
										//thread.Name = path;
										//thread.Priority = System.Threading.ThreadPriority.Lowest;
										//thread.Start(System.Threading.SynchronizationContext.Current);
									} else {
										BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("text_name_method_start") + ": {0}", "Sync");
										i2 = method.GetParameters().Length;
										if (i2 == 0) {
											method.Invoke(System.Activator.CreateInstance(type), null);
											if (stage == "initialize") {
												for (i3 = 0; i3 < l; ++i3) {
													path = BusEngine.Engine.SettingProject["require"]["plugins"][i3]["path"];
													foreach (System.Type tp in Modules[path]) {
														if (tp.IsSubclassOf(typeof(BusEngine.Plugin))) {
															System.Reflection.MethodInfo md = tp.GetMethod("initialize", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.IgnoreCase, null, t2, null);
															if (md != null) {
																x2[0] = path;
																md.Invoke(System.Activator.CreateInstance(tp), x2);
															}
															md = tp.GetMethod("initialize", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.IgnoreCase, null, t3, null);
															if (md != null) {
																x3[0] = path;
																x3[1] = stage;
																md.Invoke(System.Activator.CreateInstance(tp), x3);
															}
														}
													}
												}
											}
										}
										if (stage != "initialize") {
											for (i3 = 0; i3 < l; ++i3) {
												path = BusEngine.Engine.SettingProject["require"]["plugins"][i3]["path"];
												foreach (System.Type tp in Modules[path]) {
													if (tp.IsSubclassOf(typeof(BusEngine.Plugin))) {
														System.Reflection.MethodInfo md = tp.GetMethod("initialize", System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly | System.Reflection.BindingFlags.IgnoreCase, null, t3, null);
														if (md != null) {
															x3[0] = path;
															x3[1] = stage;
															md.Invoke(System.Activator.CreateInstance(tp), x3);
														}
													}
												}
											}
										}
									}

									if (BusEngine.Engine.IsShutdown && BusEngine.IPlugin.IsShutdown == "") {
										BusEngine.IPlugin.IsShutdown = path;
									}
								}
							}
						}
					}
				#if BUSENGINE_BENCHMARK
				}
				#endif
			}

			/* x1 = null;
			x2 = null;
			x3 = null; */

			BusEngine.Log.Info( "============================ System Plugins Stop  ============================" );

			//System.GC.SuppressFinalize(this);
		}

		public void Dispose() {
			BusEngine.Log.Info("IPlugin Dispose {0}", Stage);
		}

		~IPlugin() {
			BusEngine.Log.Info("IPlugin ~ {0}", Stage);
		}
	}
	/** API BusEngine.IPlugin */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.Log
*/
	/** API BusEngine.Rendering */
	public class Rendering : System.IDisposable {
		public void Dispose() {

		}

		~Rendering() {
			BusEngine.Log.Info("Rendering ~");
		}
	}
	/** API BusEngine.Rendering */
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
						System.Net.HttpWebRequest.Create(url).GetResponse();
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
											foreach (dynamic property in data) {
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
											foreach (dynamic property in data) {
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
				data: new System.Collections.Generic.Dictionary<string, string>(2) {
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

		~Ajax() {
			BusEngine.Log.Info("Ajax ~");
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

		~Json() {
			BusEngine.Log.Info("Json ~");
		}
	}
	/** API BusEngine.Tools.Json */
}
/** API BusEngine.Tools */

/** API BusEngine.Tools */
namespace BusEngine.Tools {
/*
Зависит от плагинов:
System.Windows.Forms
*/
	/** API BusEngine.Tools.FileFolderDialog */
	//https://stackoverflow.com/questions/11624298/how-do-i-use-openfiledialog-to-select-a-folder
	public class FileFolderDialog : System.IDisposable {
		private System.Windows.Forms.OpenFileDialog dialog = new System.Windows.Forms.OpenFileDialog();

		public System.Windows.Forms.OpenFileDialog Dialog {
			get { return dialog; }
			set { dialog = value; }
		}

		public static string _name;

		public System.Windows.Forms.DialogResult ShowDialog() {
			return this.ShowDialog(null);
		}

		public System.Windows.Forms.DialogResult ShowDialog(System.Windows.Forms.IWin32Window owner) {
			// Set validate names to false otherwise windows will not let you select "Folder Selection."
			dialog.AddExtension = false;
			//dialog.AutoUpgradeEnabled = false;
			dialog.ValidateNames = false;
			dialog.CheckFileExists = false;
			dialog.CheckPathExists = true;
			dialog.RestoreDirectory = true;
			if (BusEngine.Localization.GetLanguageStatic("text_select_folder_title") != "text_select_folder_title") {
				dialog.Title = BusEngine.Localization.GetLanguageStatic("text_select_folder_title");
			} else {
				dialog.Title = "Откройте папку которую хотите выбрать.";
			}

			try {
				if (dialog.FileName != null && dialog.FileName != "") {
					if (System.IO.Directory.Exists(dialog.FileName)) {
						dialog.InitialDirectory = dialog.FileName;
					} else {
						dialog.InitialDirectory = System.IO.Path.GetDirectoryName(dialog.FileName);
					}
				}
			} catch (System.Exception ex) {
				BusEngine.Log.Info("FileFolderDialog: {0}", ex);
			}

			if (BusEngine.Localization.GetLanguageStatic("text_select_folder") != "text_select_folder") {
				dialog.FileName = BusEngine.Localization.GetLanguageStatic("text_select_folder");
			} else {
				dialog.FileName = "Выбор папки";
			}
 
			if (owner == null) {
				return dialog.ShowDialog(BusEngine.UI.Canvas.WinForm.TopLevelControl);
			} else {
				return dialog.ShowDialog(owner);
			}
		}

		public string SelectedPath {
			get {
				try {
					if (dialog.FileName != null) {
						return System.IO.Path.GetDirectoryName(dialog.FileName);
					} else {
						return "";
					}
				} catch (System.Exception ex) {
					BusEngine.Log.Info("FileFolderDialog: {0}", ex);

					return dialog.FileName;
				}
			} set {
				if (value != null && value != "") {
					dialog.FileName = value;
				}
			}
		}

		public string SelectedPaths {
			get {
				if (dialog.FileNames != null && dialog.FileNames.Length > 1) {
					System.Text.StringBuilder sb = new System.Text.StringBuilder();

					foreach (string fileName in dialog.FileNames) {
						try {
							if (System.IO.File.Exists(fileName)) {
								sb.Append(fileName + ";");
							}
						} catch (System.Exception ex) {
							BusEngine.Log.Info("FileFolderDialog: {0}", ex);
						}
					}

					return sb.ToString();
				} else {
					return null;
				}
			}
		}

		public void Reset() {
			dialog.Reset();
		}

		protected bool RunDialog(System.IntPtr hwndOwner) {
			return true;
		}

		public void Dispose() {
			dialog.Dispose();
		}

		~FileFolderDialog() {
			//BusEngine.Log.Info("FileFolderDialog ~");
		}
	}
	/** API BusEngine.Tools.FileFolderDialog */
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
		//public static System.Collections.Generic.Dictionary<string, System.Type[]> Modules = new System.Collections.Generic.Dictionary<string, System.Type[]>(2);
		public static System.Windows.Forms.Form WinForm;
		public static System.Type Type;
		/* public class WinForm {
			public static dynamic Controls { get; set; }
			public static dynamic TopLevelControl { get; private set; }
			public static string Text { get; set; }
			public static void SuspendLayout() {}
			public static dynamic ClientSize { get; set; }
			public static void ResumeLayout(bool a) {}
			public static dynamic Dispose { get; private set; }
		}; */
		private static dynamic Form;
		//public static System.Windows.Forms.Form WPF;
		//public static dynamic Canvas;

		/** событие уничтожения окна */
		/* private void OnDisposed(object o, System.EventArgs e) {

		} */
		/** событие уничтожения окна */

		/** событие закрытия окна */
		/* private void OnClosed(object o, System.Windows.Forms.FormClosedEventArgs e) {
			BusEngine.UI.Canvas.WinForm.FormClosed -= OnClosed;
			//BusEngine.Video.Shutdown();
			//BusEngine.Engine.Shutdown();
		} */
		/** событие закрытия окна */

		//private static Canvas _canvas;

		public Canvas() {

		}

		public Canvas(System.Windows.Forms.Form _form) : this() {
			//#if (BUSENGINE_WINFORM == true)
			//if (typeof(BusEngine.UI.Canvas).GetField("WinForm") != null) {
				BusEngine.UI.Canvas.WinForm = _form;
				BusEngine.UI.Canvas.Form = _form;
				//BusEngine.UI.Canvas.WinForm.KeyPreview = true;
				// устанавливаем событи закрытия окна
				//BusEngine.UI.Canvas.WinForm.FormClosed += OnClosed;
				//BusEngine.UI.Canvas.WinForm.Disposed += new System.EventHandler(OnDisposed);
				//BusEngine.UI.ClientSize = BusEngine.UI.ClientSize;
			//}
			//#endif
		}

		//[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.CoreCompile", "CS0117", MessageId = "isChecked", Justification="Для кроссплатформенности")]
		//[System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.CoreCompile", "CS0234", Target="~T:BusEngine.UI.Canvas", Justification="Для кроссплатформенности")]
		//[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CS0234", Target="~T:BusEngine.UI.Canvas", Justification="Для кроссплатформенности")]
		public static void Initialize() {
			/* BusEngine.Log.Info("Device Version OS: {0}", typeof(BusEngine.UI.Canvas).GetProperty("WinForm"));
			BusEngine.Log.Info("Device Version OS: {0}", typeof(BusEngine.UI.Canvas).GetField("WinForm")); */
			if (1 == 0) {
				//var x = BusEngine.UI.Canvas.D21;
			}
			//if (_canvas == null) {
				//_canvas = new Canvas();

				// инициализируем плагины
				//System.Threading.Tasks.Task.Run(() => {
					new BusEngine.IPlugin("InitializeСanvas");
				//});
			//}
		}

		public static void Shutdown() {
			BusEngine.UI.Canvas.ShutdownStatic();
		}

		public static void ShutdownStatic() {
			//BusEngine.UI.Canvas.WinForm.Close();
			//BusEngine.UI.Canvas.WinForm.Dispose();
		}

		public void Dispose() {
			BusEngine.Log.Info("Canvas Dispose");
			BusEngine.UI.Canvas.WinForm.Dispose();
		}

		~Canvas() {
			BusEngine.Log.Info("Canvas ~");
		}
	}
	/** API BusEngine.UI.Canvas */
}
/** API BusEngine.UI */

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.Log
https://habr.com/ru/articles/274605/
https://habr.com/ru/articles/312078/
*/
	/** API BusEngine.Vector */
	public class Vector : System.IDisposable {
		public void Dispose() {

		}

		~Vector() {
			BusEngine.Log.Info("Vector ~");
		}
	}
	/** API BusEngine.Vector */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/* 
Зависит от плагинов:
BusEngine.Log
BusEngine.UI.Canvas
LibVLCSharp
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
		public event VideoHandler OnDuration;
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
				this.OnPlay.Invoke(this, this.Url);
				//BusEngine.UI.Canvas.WinForm.Invoke(this.OnPlay, new object[2] {this, this.Url});
			}
		}
		/** событие запуска видео */

		/** событие времени в секундах */
		private System.Timers.Timer OnDuratingTimer;
		private void OnDurating(object o, object e) {
			#if AUDIO_LOG
			BusEngine.Log.Info("Аудио OnDuration {0}", this.Position);
			#endif

			this.Duration = _mediaPlayer.Time;

			if (this.OnDuration != null) {
				this.OnDuration.Invoke(this, this.Url);
				//BusEngine.UI.Canvas.WinForm.Invoke(this.OnDuration, new object[2] {this, this.Url});
			}
		}
		/** событие времени в секундах */

		/** событие повтора видео */
		private void OnLooping(object o, object e) {
			#if VIDEO_LOG
			BusEngine.Log.Info("Видео OnLoop {0}", this.Duration);
			#endif

			this.Play(this.Url);
			if (this.OnLoop != null) {
				this.OnLoop.Invoke(this, this.Url);
				//BusEngine.UI.Canvas.WinForm.Invoke(this.OnLoop, new object[2] {this, this.Url});
			}
		}
		/** событие повтора видео */

		/** событие временной остановки видео */
		private void OnPausing(object o, object e) {
			#if VIDEO_LOG
			BusEngine.Log.Info("Видео OnPause {0}", this.Position);
			#endif

			if (this.OnPause != null) {
				this.OnPause.Invoke(this, this.Url);
				//BusEngine.UI.Canvas.WinForm.Invoke(this.OnPause, new object[2] {this, this.Url});
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
				this.OnStop.Invoke(this, this.Url);
				//BusEngine.UI.Canvas.WinForm.Invoke(this.OnStop, new object[2] {this, this.Url});
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
				this.OnEnd.Invoke(this, this.Url);
				//BusEngine.UI.Canvas.WinForm.Invoke(this.OnEnd, new object[2] {this, this.Url});
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
				this.OnDispose.Invoke(this, this.Url);
				//BusEngine.UI.Canvas.WinForm.Invoke(this.OnDispose, new object[2] {this, this.Url});
			}
		}
		/** событие уничтожения видео */

		/** функция запуска видео */
		public Video() {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Video")) {
			#endif
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
			//_mediaPlayer.PositionChanged += this.OnDurating;
			OnDuratingTimer = new System.Timers.Timer();
			OnDuratingTimer.Interval = 100;
			OnDuratingTimer.Elapsed += this.OnDurating;
			//OnDuratingTimer.AutoReset = true;
			//OnDuratingTimer.Enabled = true;

			_winForm = new LibVLCSharp.WinForms.VideoView();
			((System.ComponentModel.ISupportInitialize)(_winForm)).BeginInit();
			BusEngine.UI.Canvas.WinForm.SuspendLayout();

			_winForm.MediaPlayer = _mediaPlayer;
			_winForm.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) | System.Windows.Forms.AnchorStyles.Left) | System.Windows.Forms.AnchorStyles.Right)));
			//_winForm.BackColor = System.Drawing.Color.Black;
			//_winForm.TabIndex = 1;
			//_winForm.MediaPlayer = null;
			//_winForm.Name = "Я хочу сожрать 7 Мб!";
			//_winForm.Name = BusEngine.Engine.SettingProject["info"]["name"];
			//_winForm.Text = BusEngine.Engine.SettingProject["info"]["name"];
			//_winForm.Location = new System.Drawing.Point(0, 27);
			//_winForm.Size = new System.Drawing.Size(800, 444);
			//_winForm.CurrentPosition = position;
			_winForm.Size = BusEngine.UI.Canvas.WinForm.ClientSize;
			//BusEngine.Log.Info("Видео name {0}", BusEngine.Engine.SettingProject["info"]["name"]);
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
			#if BUSENGINE_BENCHMARK
			}
			#endif
		}
		public Video(string url = "") : this() {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Video url")) {
			#endif
			this.Url = url;
			this.Urls = new string[1] {url};
			#if BUSENGINE_BENCHMARK
			}
			#endif
		}
		public Video(string[] urls) : this() {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Video urls")) {
			#endif
			if (urls.Length > 0) {
				this.Urls = urls;
				this.UrlsArray = urls;
				this.Url = urls[0];
				if (this.OnStop == null) {
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
				}
				if (this.OnEnd == null) {
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
				}
				if (this.OnNotFound == null) {
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
			#if BUSENGINE_BENCHMARK
			}
			#endif
		}

		public Video Play() {
			return this.Play(this.Url);
		}

		public Video Play(string url = "") {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Video url")) {
			#endif
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

						OnDuratingTimer.Start();
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
					this.OnNotFound.Invoke(this, this.Url);
					//BusEngine.UI.Canvas.WinForm.Invoke(this.OnNotFound, new object[2] {this, this.Url});
				}
			}

			return this;
			#if BUSENGINE_BENCHMARK
			}
			#endif
		}

		public Video Play(string[] urls) {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Video urls")) {
			#endif
			if (urls.Length > 0) {
				this.Urls = urls;
				this.UrlsArray = urls;
				this.Url = urls[0];
				if (this.OnStop == null) {
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
				}
				if (this.OnEnd == null) {
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
				}
				if (this.OnNotFound == null) {
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

				return this.Play(this.Url);
			} else {
				return this;
			}
			#if BUSENGINE_BENCHMARK
			}
			#endif
		}
		/** функция запуска видео */

		/** функция временной остановки видео */
		public void Pause() {
			#if VIDEO_LOG
			BusEngine.Log.Info("Видео Pause()");
			#endif

			if (!_mediaPlayer.CanPause) {
				OnDuratingTimer.Start();
				_mediaPlayer.Play();
				this.IsPause = false;
			} else {
				OnDuratingTimer.Stop();
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

			OnDuratingTimer.Stop();
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
				if (this.OnDuratingTimer != null) {
					OnDuratingTimer.Dispose();
				}
				if (_mediaPlayer != null) {
					_mediaPlayer.Playing -= this.OnPlaying;
					_mediaPlayer.EndReached -= this.OnLooping;
					_mediaPlayer.Paused -= this.OnPausing;
					_mediaPlayer.Stopped -= this.OnStopping;
					//_mediaPlayer.Disposed -= this.OnDisposing;
					_mediaPlayer.EndReached -= this.OnEnding;
					//_mediaPlayer.PositionChanged -= this.OnDurating;
					_mediaPlayer.Dispose();
				}
				if (_VLC != null) {
					_VLC.Dispose();
				}
				if (_winForm != null) {
					_winForm.Dispose();
				}
				if (this.DisposeTimer != null) {
					this.DisposeTimer.Dispose();
				}
				if (this.OnDispose != null) {
					this.OnDispose.Invoke(this, this.Url);
					// https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.forms.control.invoke?view=windowsdesktop-7.0
					//BusEngine.UI.Canvas.WinForm.Invoke(this.OnDispose, new object[2] {this, this.Url});
				}
			}
		}
		/** функция уничтожения объекта видео */

		/** функция уничтожения объекта видео */
		~Video() {
			#if VIDEO_LOG
			BusEngine.Log.Info("Video ~");
			#endif
		}
		/** функция уничтожения объекта видео */
	}
	/** API BusEngine.Video */
}
/** API BusEngine */