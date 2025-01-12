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
https://learn.microsoft.com/en-us/dotnet/standard/events/how-to-handle-multiple-events-using-event-properties
https://habr.com/ru/companies/ruvds/articles/784776/
Проверить скорость и взять лучшее для работы с DirectX, если это пойдёт на повышение FPS.
https://github.com/CanTalat-Yakan/3DEngine
https://github.com/amerkoleci/Vortice.Windows
сертификат для программы
https://learn.microsoft.com/ru-ru/windows/win32/appxpkg/how-to-create-a-package-signing-certificate
https://professorweb.ru/my/csharp/base_net/level2/2_6.php
*/

/** дорожная карта
//- проставить нормально модификаторы доступа https://metanit.com/sharp/tutorial/3.2.php
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
					{"sys_VSync", "1"},                   // Вертикальная синхронизация
					{"sys_FPS", "100"},                   // Частота монитора для регулировки единицы измерения (скорости)
					{"sys_FPSAuto", "1"},                 // Отключение зависимости от времени
					{"sys_DoubleBuffered", "1"},          // Двойная буферизация
					{"sys_TripleBuffered", "1"},          // Тройная буферизация
					{"sys_FOV", "60"},                    // Угол обзора - в градусах
					{"sys_ShaderCopyGeom", "16"},         // Количество копий для геометрического шейдера
					{"sys_MemoryClearTime", "5"},         // Установка промежутка времени для освобождения оперативной памяти в секундах
					{"sys_MemoryClearAuto", "1"},         // Статус автоматического освобождения оперативной памяти (принудительный вызов System.GC.Collect)
					{"sys_Benchmark", "1"},               // Benchmark статус
					{"sys_DistanceMin", "0,01"},          // Дальность прорисовки min в метрах
					{"sys_DistanceMax", "1000"},          // Дальность прорисовки max в метрах
					{"r_WaterOcean", "0"},                // Статус работы океана
					{"r_VolumetricClouds", "1"},          // Статус работы облаков
					{"r_DisplayInfo", "2"},               // Статус работы окна информации
					{"r_FullScreen", "0"},                // Выбор режима работы окна приложения
					{"r_Width", "1280"},                  // Ширина окна приложения
					{"r_Height", "720"},                  // Высота окна приложения
					{"google_api_key", ""},               // Секретный ключ API приложения Google
					{"google_default_client_id", ""},     // ID пользователя API приложения Google
					{"google_default_client_secret", ""}, // Секретный ключ пользователя API приложения Google
					{"g_texture_filtering", "0"},         // фильтрация текстур 0 - отключено, 1 - линейная, 2 - билинейная, 3 трилинейная, 4 - анизотропная, 5 - 2х анизотропная, 6 - 4х анизотропная, 7 - 8х анизотропная, 8 - 16х анизотропная
				}},
				{"console_variables", new System.Collections.Generic.Dictionary<string, string>(20, System.StringComparer.OrdinalIgnoreCase) {
					{"sys_Spec", "1"},
					{"sys_VSync", "1"},
					{"sys_FPS", "100"},
					{"sys_FPSAuto", "1"},
					{"sys_DoubleBuffered", "1"},
					{"sys_TripleBuffered", "1"},
					{"sys_FOV", "60"},
					{"sys_MemoryClearTime", "5"},
					{"sys_MemoryClearAuto", "1"},
					{"sys_Benchmark", "1"},
					{"sys_DistanceMin", "0,01"},
					{"sys_DistanceMax", "1000"},
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
				//System.Threading.Tasks.Task.Run(() => {
					this.Dispose(true);
				//});
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
			//System.Threading.Tasks.Task.Run(() => {
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
			//});
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
		public CefSharp.WinForms.ChromiumWebBrowser Control {
			get {
				return browser;
			} private set {}
		}
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
		private CefSharp.IRequestContext _Context;
		public CefSharp.IRequestContext Context {
			get {
				return _Context;
			} private set {
				_Context = value;
			}
		}

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
				settings.ChromeRuntime = false;
				settings.CommandLineArgsDisabled = false;

				//settings.CefCommandLineArgs.Add("disable-features=SameSiteByDefaultCookies");

				// воспроизводим аудио автоматом
				settings.CefCommandLineArgs.Add("autoplay-policy", "no-user-gesture-required");

				// включаем систему распознавания голоса и т.д.
				settings.CefCommandLineArgs.Add("enable-media-stream");
				settings.CefCommandLineArgs.Add("enable-speech-input");
				settings.CefCommandLineArgs.Add("enable-speech-synthesis-api");

				// отключение требования сертификатов
				//settings.CefCommandLineArgs.Add("ignore-certificate-errors");

				// отключение корс проверок
				// https://peter.sh/experiments/chromium-command-line-switches/#disable-web-security
				//settings.CefCommandLineArgs.Add("disable-web-security");

				//settings.CefCommandLineArgs.Add("user-data-dir");
				//System.Environment.SetEnvironmentVariable("user-data-dir", BusEngine.Engine.LogDirectory + "Browser\\userdata");

				// https://peter.sh/experiments/chromium-command-line-switches/#in-process-gpu
				//settings.CefCommandLineArgs.Add("in-process-gpu");
				//System.Environment.SetEnvironmentVariable("in-process-gpu", "1");

				// https://peter.sh/experiments/chromium-command-line-switches/#enable-gpu
				//settings.CefCommandLineArgs.Add("enable-gpu");
				// https://peter.sh/experiments/chromium-command-line-switches/#disable-gpu
				settings.CefCommandLineArgs.Add("disable-gpu");

				// https://peter.sh/experiments/chromium-command-line-switches/#enable-gpu-vsync
				//settings.CefCommandLineArgs.Add("enable-gpu-vsync");
				// https://peter.sh/experiments/chromium-command-line-switches/#disable-gpu-vsync
				settings.CefCommandLineArgs.Add("disable-gpu-vsync");

				// https://peter.sh/experiments/chromium-command-line-switches/#enable-gpu-shader-disk-cache
				settings.CefCommandLineArgs.Add("enable-gpu-shader-disk-cache");
				// https://peter.sh/experiments/chromium-command-line-switches/#disable-gpu-shader-disk-cache
				//settings.CefCommandLineArgs.Add("disable-gpu-shader-disk-cache");

				// https://peter.sh/experiments/chromium-command-line-switches/#enable-gpu-rasterization
				settings.CefCommandLineArgs.Add("enable-gpu-rasterization");
				// https://peter.sh/experiments/chromium-command-line-switches/#disable-gpu-rasterization
				//settings.CefCommandLineArgs.Add("disable-gpu-rasterization");

				// https://peter.sh/experiments/chromium-command-line-switches/#enable-gpu-memory-buffer-compositor-resources
				settings.CefCommandLineArgs.Add("enable-gpu-memory-buffer-compositor-resources");
				// https://peter.sh/experiments/chromium-command-line-switches/#disable-gpu-memory-buffer-compositor-resources
				//settings.CefCommandLineArgs.Add("disable-gpu-memory-buffer-compositor-resources");

				// https://peter.sh/experiments/chromium-command-line-switches/#enable-gpu-memory-buffer-video-frames
				settings.CefCommandLineArgs.Add("enable-gpu-memory-buffer-video-frames");
				// https://peter.sh/experiments/chromium-command-line-switches/#disable-gpu-memory-buffer-video-frames
				//settings.CefCommandLineArgs.Add("disable-gpu-memory-buffer-video-frames");

				// https://peter.sh/experiments/chromium-command-line-switches/#use-angle
				//System.Environment.SetEnvironmentVariable("use-angle", "default"); // default, d3d9, d3d11, warp, gl, gles
				// https://peter.sh/experiments/chromium-command-line-switches/#use-gl
				//System.Environment.SetEnvironmentVariable("use-gl", "desktop"); // desktop, egl, Swiftshader

				// https://peter.sh/experiments/chromium-command-line-switches/#use-vulkan
				//System.Environment.SetEnvironmentVariable("use-vulkan", "1");
				// https://peter.sh/experiments/chromium-command-line-switches/#enable-features
				//System.Environment.SetEnvironmentVariable("enable-features", "Vulkan");

				// https://peter.sh/experiments/chromium-command-line-switches/#trace-startup-file
				System.Environment.SetEnvironmentVariable("trace-startup-file", BusEngine.Engine.LogDirectory + "Browser/trace_event.log");

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
				settings.Dispose();

				// запускаем браузер
				browser = new CefSharp.WinForms.ChromiumWebBrowser(url);

				browser.BrowserSettings.WindowlessFrameRate = 120;

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
				/* browser.StatusMessage += (object s, CefSharp.Status-MessageEventArgs e) => {
					//CefSharp.WebBrowserExtensions.ExecuteScriptAsync(e.Browser, "if (!('BusEngine' in window)) {window.BusEngine = {};} window.BusEngine.PostMessage = ('CefSharp' in window ? CefSharp.PostMessage : function(m) {}); CefSharp = null;");
					#if BROWSER_LOG
					BusEngine.Log.Info("StatusMessage! {0}", e.Value);
					#endif
				}; */
				/** заменяем на своё CefSharp.PostMessage на BusEngine.PostMessage */
				// https://cefsharp.github.io/api/107.1.x/html/T_CefSharp_FrameLoadStartEventArgs.htm
				browser.FrameLoadStart += (object b, CefSharp.FrameLoadStartEventArgs e) => {
					if (e.Frame.IsMain) {
						using (CefSharp.DevTools.DevToolsClient devToolsClient = CefSharp.DevToolsExtensions.GetDevToolsClient(browser)) {
							System.Collections.Generic.List<CefSharp.DevTools.Emulation.UserAgentBrandVersion> brandsList = new System.Collections.Generic.List<CefSharp.DevTools.Emulation.UserAgentBrandVersion>();

							CefSharp.DevTools.Emulation.UserAgentBrandVersion uab = new CefSharp.DevTools.Emulation.UserAgentBrandVersion();

							uab.Brand = "Chromium";
							uab.Version = "109.0.5414.87";

							brandsList.Add(uab);

							uab = new CefSharp.DevTools.Emulation.UserAgentBrandVersion();
							uab.Brand = "BusEngine";
							uab.Version = BusEngine.Engine.SettingProject["require"]["engine"];

							brandsList.Add(uab);

							CefSharp.DevTools.Emulation.UserAgentMetadata ua = new CefSharp.DevTools.Emulation.UserAgentMetadata();

							ua.Brands = brandsList;
							ua.Architecture = BusEngine.Engine.Device.Processor;
							ua.Model = BusEngine.Engine.Platform;
							ua.Platform = BusEngine.Engine.Device.Name;
							ua.PlatformVersion = BusEngine.Engine.Device.Version;
							ua.FullVersion = BusEngine.Engine.SettingProject["require"]["engine"];
							ua.Mobile = true;

							devToolsClient.Emulation.SetUserAgentOverrideAsync(BusEngine.Engine.Device.UserAgent, null, null, ua);
						}

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

	var elementReady = function(parent, selector) {
		return new Promise(function(resolve) {
			var el = parent.querySelector(selector);

			if (el) {
				resolve(el);
			}

			new MutationObserver(function(mutationRecords, observer) {
				var element = parent.querySelector(selector);

				if (element) {
					resolve(element);
					observer.disconnect();
				}
			}).observe(parent, {
				childList: true,
				subtree: true,
			});
		});
	};

	// fix requestPointerLock Chromium
	/* HTMLElement.prototype.requestPointerLock = function() {
		if (window.event && window.event.constructor.name == 'PointerEvent') {
			window.event.stopPropagation();

			this.style['cursor'] = 'none';

			this.addEventListener('mousemove', function(e) {
				e.stopImmediatePropagation();
			});

			Object.defineProperty(HTMLDocument.prototype, 'pointerLockElement', {
				value: this,
				writable: true,
				configurable: true,
				enumerable: true,
			});

			document.dispatchEvent(new CustomEvent('pointerlockchange'));
		} else {
			document.dispatchEvent(new CustomEvent('pointerlockerror'));
		}
	};

	HTMLDocument.prototype.exitPointerLock = function() {
		document.pointerLockElement.style['cursor'] = null;

		document.pointerLockElement.removeEventListener('mousemove', function(e) {
			e.stopImmediatePropagation();
		});

		Object.defineProperty(HTMLDocument.prototype, 'pointerLockElement', {
			value: null,
			writable: true,
			configurable: true,
			enumerable: true,
		});

		document.dispatchEvent(new CustomEvent('pointerlockchange'));
	}; */

	/* Object.defineProperty(HTMLElement.prototype, 'requestPointerLock', {
		value: function() {
			if (window.event && window.event.constructor.name == 'PointerEvent') {
				window.event.stopPropagation();

				//this.style['cursor'] = 'none';

				document.addEventListener('mousemove', function(e) {
					e.stopImmediatePropagation();
				});

				Object.defineProperty(HTMLDocument.prototype, 'pointerLockElement', {
					value: this,
					writable: true,
					configurable: true,
					enumerable: true,
				});

				document.dispatchEvent(new CustomEvent('pointerlockchange'));
			} else {
				document.dispatchEvent(new CustomEvent('pointerlockerror'));
			}
		},
		writable: true,
		configurable: true,
		enumerable: true,
	});

	Object.defineProperty(HTMLDocument.prototype, 'exitPointerLock', {
		value: function() {
			document.pointerLockElement.style['cursor'] = null;

			document.removeEventListener('mousemove', function(e) {
				e.stopImmediatePropagation();
			});

			Object.defineProperty(HTMLDocument.prototype, 'pointerLockElement', {
				value: null,
				writable: true,
				configurable: true,
				enumerable: true,
			});

			document.dispatchEvent(new CustomEvent('pointerlockchange'));
		},
		writable: true,
		configurable: true,
		enumerable: true,
	}); */
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
				//browser.UseParentFormMessageInterceptor = false;

				// подключаем браузер к нашей программе
				/* BusEngine.Log.Info("1 Owner: {0}", BusEngine.UI.Canvas.WinForm.Owner);
				BusEngine.Log.Info("1 Owner Controls: {0}", BusEngine.UI.Canvas.WinForm.Controls.Owner);
				BusEngine.Log.Info("1 TopLevelControl: {0}", BusEngine.UI.Canvas.WinForm.TopLevelControl);
				BusEngine.Log.Info("1 ActiveControl: {0}", BusEngine.UI.Canvas.WinForm.ActiveControl);
				BusEngine.Log.Info("1 TopLevel: {0}", BusEngine.UI.Canvas.WinForm.TopLevel);
				BusEngine.Log.Info("1 TabIndex: {0}", BusEngine.UI.Canvas.WinForm.TabIndex);
				BusEngine.Log.Info("1 Contains: {0}", BusEngine.UI.Canvas.WinForm.Contains(browser)); */

				browser.TabIndex = 0;
				//browser.BringToFront();

				Context = browser.RequestContext;
				//BusEngine.UI.Canvas.WinForm.Controls.Add(browser);

				/* BusEngine.Log.Info("2 Owner: {0}", BusEngine.UI.Canvas.WinForm.Owner);
				BusEngine.Log.Info("2 Owner Controls: {0}", BusEngine.UI.Canvas.WinForm.Controls.Owner);
				BusEngine.Log.Info("2 TopLevelControl: {0}", BusEngine.UI.Canvas.WinForm.TopLevelControl);
				BusEngine.Log.Info("2 ActiveControl: {0}", BusEngine.UI.Canvas.WinForm.ActiveControl);
				BusEngine.Log.Info("2 TopLevel: {0}", BusEngine.UI.Canvas.WinForm.TopLevel);
				BusEngine.Log.Info("2 TabIndex: {0}", BusEngine.UI.Canvas.WinForm.TabIndex);
				BusEngine.Log.Info("2 Contains: {0}", BusEngine.UI.Canvas.WinForm.Contains(browser)); */


			//browser.ResumeLayout(false);
			//browser.PerformLayout();

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
/*
Зависит от плагинов:
*/
	/** API BusEngine.Color */
    [System.Serializable]
    public struct Color : System.IEquatable<Color> {
        public float R;
        public float G;
        public float B;
        public float A;

        public Color(float r, float g, float b, float a) {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public Color(byte r, byte g, byte b, byte a) {
            R = r / (float)System.Byte.MaxValue;
            G = g / (float)System.Byte.MaxValue;
            B = b / (float)System.Byte.MaxValue;
            A = a / (float)System.Byte.MaxValue;
        }

        public int ToArgb() {
            uint value = (uint)(A * System.Byte.MaxValue) << 24 | (uint)(R * System.Byte.MaxValue) << 16 | (uint)(G * System.Byte.MaxValue) << 8 | (uint)(B * System.Byte.MaxValue);

            return unchecked((int)value);
        }

        public static bool operator ==(Color left, Color right) {
            return left.Equals(right);
        }

        public static bool operator !=(Color left, Color right) {
            return !left.Equals(right);
        }

        public static implicit operator Color(System.Drawing.Color color) {
            return new Color(color.R, color.G, color.B, color.A);
        }

        public static explicit operator System.Drawing.Color(BusEngine.Color color) {
            return System.Drawing.Color.FromArgb((int)(color.A * System.Byte.MaxValue), (int)(color.R * System.Byte.MaxValue), (int)(color.G * System.Byte.MaxValue), (int)(color.B * System.Byte.MaxValue));
        }

        public override bool Equals(object obj) {
            if (!(obj is Color)) {
                return false;
            }

            return Equals((Color)obj);
        }

        public override int GetHashCode() {
            return ToArgb();
        }

        public override string ToString() {
            return System.String.Format("{{(R, G, B, A) = ({0}, {1}, {2}, {3})}}", R.ToString(), G.ToString(), B.ToString(), A.ToString());
        }

        public static Color FromSrgb(Color srgb) {
            float r, g, b;

            if (srgb.R <= 0.04045f) {
                r = srgb.R / 12.92f;
            } else {
                r = (float)System.Math.Pow((srgb.R + 0.055f) / (1.0f + 0.055f), 2.4f);
            }

            if (srgb.G <= 0.04045f) {
                g = srgb.G / 12.92f;
            } else {
                g = (float)System.Math.Pow((srgb.G + 0.055f) / (1.0f + 0.055f), 2.4f);
            }

            if (srgb.B <= 0.04045f) {
                b = srgb.B / 12.92f;
            } else {
                b = (float)System.Math.Pow((srgb.B + 0.055f) / (1.0f + 0.055f), 2.4f);
            }

            return new Color(r, g, b, srgb.A);
        }

        public static Color ToSrgb(Color rgb) {
            float r, g, b;

            if (rgb.R <= 0.0031308) {
                r = 12.92f * rgb.R;
            } else {
                r = (1.0f + 0.055f) * (float)System.Math.Pow(rgb.R, 1.0f / 2.4f) - 0.055f;
            }

            if (rgb.G <= 0.0031308) {
                g = 12.92f * rgb.G;
            } else {
                g = (1.0f + 0.055f) * (float)System.Math.Pow(rgb.G, 1.0f / 2.4f) - 0.055f;
            }

            if (rgb.B <= 0.0031308) {
                b = 12.92f * rgb.B;
            } else {
                b = (1.0f + 0.055f) * (float)System.Math.Pow(rgb.B, 1.0f / 2.4f) - 0.055f;
            }

            return new Color(r, g, b, rgb.A);
        }

        public static Color FromHsl(Vector4 hsl) {
            var hue = hsl.X * 360.0f;
            var saturation = hsl.Y;
            var lightness = hsl.Z;
            var C = (1.0f - System.Math.Abs(2.0f * lightness - 1.0f)) * saturation;
            var h = hue / 60.0f;
            var X = C * (1.0f - System.Math.Abs(h % 2.0f - 1.0f));
            float r, g, b;

            if (0.0f <= h && h < 1.0f) {
                r = C;
                g = X;
                b = 0.0f;
            } else if (1.0f <= h && h < 2.0f) {
                r = X;
                g = C;
                b = 0.0f;
            } else if (2.0f <= h && h < 3.0f) {
                r = 0.0f;
                g = C;
                b = X;
            } else if (3.0f <= h && h < 4.0f) {
                r = 0.0f;
                g = X;
                b = C;
            } else if (4.0f <= h && h < 5.0f) {
                r = X;
                g = 0.0f;
                b = C;
            } else if (5.0f <= h && h < 6.0f) {
                r = C;
                g = 0.0f;
                b = X;
            } else {
                r = 0.0f;
                g = 0.0f;
                b = 0.0f;
            }

            var m = lightness - (C / 2.0f);
            return new Color(r + m, g + m, b + m, hsl.W);
        }

        public static Vector4 ToHsl(Color rgb) {
            var M = System.Math.Max(rgb.R, System.Math.Max(rgb.G, rgb.B));
            var m = System.Math.Min(rgb.R, System.Math.Min(rgb.G, rgb.B));
            var C = M - m;
            float h = 0.0f;

            if (M == rgb.R) {
                h = ((rgb.G - rgb.B) / C);
            } else if (M == rgb.G) {
                h = ((rgb.B - rgb.R) / C) + 2.0f;
            } else if (M == rgb.B) {
                h = ((rgb.R - rgb.G) / C) + 4.0f;
            }

            var hue = h / 6.0f;

            if (hue < 0.0f) {
                hue += 1.0f;
            }

            var lightness = (M + m) / 2.0f;
            var saturation = 0.0f;

            if (0.0f != lightness && lightness != 1.0f) {
                saturation = C / (1.0f - System.Math.Abs(2.0f * lightness - 1.0f));
            }

            return new Vector4(hue, saturation, lightness, rgb.A);
        }

        public static Color FromHsv(Vector4 hsv) {
            var hue = hsv.X * 360.0f;
            var saturation = hsv.Y;
            var value = hsv.Z;
            var C = value * saturation;
            var h = hue / 60.0f;
            var X = C * (1.0f - System.Math.Abs(h % 2.0f - 1.0f));
            float r, g, b;

            if (0.0f <= h && h < 1.0f)            {
                r = C;
                g = X;
                b = 0.0f;
            }            else if (1.0f <= h && h < 2.0f)            {
                r = X;
                g = C;
                b = 0.0f;
            }            else if (2.0f <= h && h < 3.0f)            {
                r = 0.0f;
                g = C;
                b = X;
            }            else if (3.0f <= h && h < 4.0f)            {
                r = 0.0f;
                g = X;
                b = C;
            }            else if (4.0f <= h && h < 5.0f)            {
                r = X;
                g = 0.0f;
                b = C;
            }            else if (5.0f <= h && h < 6.0f)            {
                r = C;
                g = 0.0f;
                b = X;
            }            else            {
                r = 0.0f;
                g = 0.0f;
                b = 0.0f;
            }

            var m = value - C;
            return new Color(r + m, g + m, b + m, hsv.W);
        }

        public static Vector4 ToHsv(Color rgb) {
            var M = System.Math.Max(rgb.R, System.Math.Max(rgb.G, rgb.B));
            var m = System.Math.Min(rgb.R, System.Math.Min(rgb.G, rgb.B));
            var C = M - m;
            float h = 0.0f;

            if (M == rgb.R) {
                h = ((rgb.G - rgb.B) / C) % 6.0f;
            } else if (M == rgb.G) {
                h = ((rgb.B - rgb.R) / C) + 2.0f;
            } else if (M == rgb.B) {
                h = ((rgb.R - rgb.G) / C) + 4.0f;
            }

            var hue = (h * 60.0f) / 360.0f;

            var saturation = 0.0f;
            if (0.0f != M) {
                saturation = C / M;
            }

            return new Vector4(hue, saturation, M, rgb.A);
        }

        public static Color FromXyz(Vector4 xyz) {
            var r = 0.41847f * xyz.X + -0.15866f * xyz.Y + -0.082835f * xyz.Z;
            var g = -0.091169f * xyz.X + 0.25243f * xyz.Y + 0.015708f * xyz.Z;
            var b = 0.00092090f * xyz.X + -0.0025498f * xyz.Y + 0.17860f * xyz.Z;
            return new Color(r, g, b, xyz.W);
        }

        public static Vector4 ToXyz(Color rgb) {
            var x = (0.49f * rgb.R + 0.31f * rgb.G + 0.20f * rgb.B) / 0.17697f;
            var y = (0.17697f * rgb.R + 0.81240f * rgb.G + 0.01063f * rgb.B) / 0.17697f;
            var z = (0.00f * rgb.R + 0.01f * rgb.G + 0.99f * rgb.B) / 0.17697f;
            return new Vector4(x, y, z, rgb.A);
        }

        public static Color FromYcbcr(Vector4 ycbcr) {
            var r = 1.0f * ycbcr.X + 0.0f * ycbcr.Y + 1.402f * ycbcr.Z;
            var g = 1.0f * ycbcr.X + -0.344136f * ycbcr.Y + -0.714136f * ycbcr.Z;
            var b = 1.0f * ycbcr.X + 1.772f * ycbcr.Y + 0.0f * ycbcr.Z;
            return new Color(r, g, b, ycbcr.W);
        }

        public static Vector4 ToYcbcr(Color rgb) {
            var y = 0.299f * rgb.R + 0.587f * rgb.G + 0.114f * rgb.B;
            var u = -0.168736f * rgb.R + -0.331264f * rgb.G + 0.5f * rgb.B;
            var v = 0.5f * rgb.R + -0.418688f * rgb.G + -0.081312f * rgb.B;
            return new Vector4(y, u, v, rgb.A);
        }

        public static Color FromHcy(Vector4 hcy) {
            var hue = hcy.X * 360.0f;
            var C = hcy.Y;
            var luminance = hcy.Z;

            var h = hue / 60.0f;
            var X = C * (1.0f - System.Math.Abs(h % 2.0f - 1.0f));

            float r, g, b;
            if (0.0f <= h && h < 1.0f) {
                r = C;
                g = X;
                b = 0.0f;
            } else if (1.0f <= h && h < 2.0f) {
                r = X;
                g = C;
                b = 0.0f;
            } else if (2.0f <= h && h < 3.0f) {
                r = 0.0f;
                g = C;
                b = X;
            } else if (3.0f <= h && h < 4.0f) {
                r = 0.0f;
                g = X;
                b = C;
            } else if (4.0f <= h && h < 5.0f) {
                r = X;
                g = 0.0f;
                b = C;
            } else if (5.0f <= h && h < 6.0f) {
                r = C;
                g = 0.0f;
                b = X;
            } else {
                r = 0.0f;
                g = 0.0f;
                b = 0.0f;
            }

            var m = luminance - (0.30f * r + 0.59f * g + 0.11f * b);
            return new Color(r + m, g + m, b + m, hcy.W);
        }

        public static Vector4 ToHcy(Color rgb) {
            var M = System.Math.Max(rgb.R, System.Math.Max(rgb.G, rgb.B));
            var m = System.Math.Min(rgb.R, System.Math.Min(rgb.G, rgb.B));
            var C = M - m;

            float h = 0.0f;
            if (M == rgb.R) {
                h = ((rgb.G - rgb.B) / C) % 6.0f;
            } else if (M == rgb.G) {
                h = ((rgb.B - rgb.R) / C) + 2.0f;
            } else if (M == rgb.B) {
                h = ((rgb.R - rgb.G) / C) + 4.0f;
            }

            var hue = (h * 60.0f) / 360.0f;

            var luminance = 0.30f * rgb.R + 0.59f * rgb.G + 0.11f * rgb.B;

            return new Vector4(hue, C, luminance, rgb.A);
        }

        public bool Equals(Color other) {
            return this.R == other.R && this.G == other.G && this.B == other.B && this.A == other.A;
        }
    }
	/** API BusEngine.Color */
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
		public static bool IsGame { get; private set; }
		public static bool IsShutdown { get; private set; }
		public static string[] Commands;

		private static bool _GameStart = false;
		public static void GameStart() {
			if (IsGame == false && !_GameStart && BusEngine.UI.Canvas.WinForm != null) {
				IsGame = true;
				_GameStart = true;
				BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint);
				new BusEngine.IPlugin("OnGameStart");
				BusEngine.UI.Canvas.WinForm.Invalidate(false);
				_GameStart = false;
			}
		}
		private static bool _GameStop = false;
		public static void GameStop() {
			if (IsGame == true && !_GameStop && BusEngine.UI.Canvas.WinForm != null) {
				_GameStop = true;
				BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint);
				new BusEngine.IPlugin("OnGameStop");
				BusEngine.UI.Canvas.WinForm.Invalidate(false);
				_GameStop = false;
				IsGame = false;
			}
		}

		private static int _timefps = 0;
		private static int FPSSetting = 100;
		public static void GameUpdate() {
			if (BusEngine.Engine._timefps == 0 && BusEngine.UI.Canvas.WinForm != null) {
				new BusEngine.IPlugin("OnGameUpdate");
				BusEngine.UI.Canvas.WinForm.Invalidate(false);
				//BusEngine.UI.Canvas.WinForm.Refresh();

				BusEngine.Engine._timefps = 1400000000 / BusEngine.Engine.FPSSetting;
			} else {
				// заменить на загрузку данных в файл
				int time = BusEngine.Engine._timefps;
				while (time > 0) {
					time -= 1;
				}
				BusEngine.Engine._timefps = time;

				BusEngine.Engine.GameUpdate();
			}
		}

		private static void Paint(object sender, System.Windows.Forms.PaintEventArgs e) {
			/* System.Drawing.StringFormat drawFormat = new System.Drawing.StringFormat();
			drawFormat.FormatFlags = System.Drawing.StringFormatFlags.DirectionVertical;
			e.Graphics.DrawString("Sample Text", new System.Drawing.Font("Arial", 16), new System.Drawing.SolidBrush(System.Drawing.Color.Black), 150.0F, 50.0F, drawFormat);
 */
			//new BusEngine.IPlugin("OnGameUpdate");
			//BusEngine.UI.Canvas.WinForm.Invalidate(false);
			BusEngine.Engine.GameUpdate();
			//BusEngine.UI.Canvas.WinForm.Update();
			//BusEngine.UI.Canvas.WinForm.Refresh();
		}

			/* // зависимость от времени
			System.Timers.Timer aTimer = new System.Timers.Timer(1000F/FPSSetting);
			aTimer.Elapsed += OnTimedEvent;
			aTimer.AutoReset = true;
			aTimer.Enabled = true; */

		/* private static void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e) {
			new BusEngine.IPlugin("OnGameUpdate");
			BusEngine.UI.Canvas.WinForm.Invalidate(true);
			//BusEngine.UI.Canvas.WinForm.Update();
			//BusEngine.UI.Canvas.WinForm.Refresh();
		} */

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

			IsGame = false;
			IsShutdown = false;
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
			}

			dynamic content;

			if (setting.TryGetValue("content", out content) && content.GetType().GetProperty("Count") != null) {
				foreach (dynamic i in content) {
					if (i is object) {
						string n = i.Name;
						string v = i.Value;

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

			dynamic console_commands;

			if (setting.TryGetValue("console_commands", out console_commands) && console_commands.GetType().GetProperty("Count") != null) {
				foreach (dynamic i in console_commands) {
					if (i is object) {
						settingDefault["console_commands"][i.Name] = i.Value.ToString();
					}
				}
			}

			dynamic console_variables;

			if (setting.TryGetValue("console_variables", out console_variables) && console_variables.GetType().GetProperty("Count") != null) {
				foreach (dynamic i in console_variables) {
					if (i is object) {
						settingDefault["console_variables"][i.Name] = i.Value.ToString();
					}
				}
			}

			dynamic info;

			if (setting.TryGetValue("info", out info) && info.GetType().GetProperty("Count") != null) {
				foreach (dynamic i in info) {
					if (i is object) {
						string n = i.Name;
						string v = i.Value;

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
									{"guid", (require["plugins"][i].ContainsKey("guid") && require["plugins"][i]["guid"] is string ? System.Convert.ToString(require["plugins"][i]["guid"]) : "")},
									{"type", (require["plugins"][i].ContainsKey("type") && require["plugins"][i]["type"] is string ? require["plugins"][i]["type"] : "")},
									{"platforms", (require["plugins"][i].ContainsKey("platforms") && require["plugins"][i]["platforms"].GetType().GetProperty("Count") != null ? require["plugins"][i]["platforms"] : settingDefault["require"]["plugins"][i]["platforms"])}
								});
							}
						}
					}
				}
			}

			if (settingDefault["require"]["engine"] == "") {
				settingDefault["require"]["engine"] = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			}

			/* project_files = null;
			content = null;
			console_commands = null;
			console_variables = null;
			info = null;
			require = null;
			setting.Clear();
			setting = null; */

			// события и синхронизация (увеличиваем потребление RAM)
			System.Threading.Tasks.Task[] tasks = new System.Threading.Tasks.Task[3];

			tasks[0] = System.Threading.Tasks.Task.Factory.StartNew(() => {
			}, System.Threading.Tasks.TaskCreationOptions.AttachedToParent);
			tasks[1] = System.Threading.Tasks.Task.Factory.StartNew(() => {
			}, System.Threading.Tasks.TaskCreationOptions.AttachedToParent);
			tasks[2] = System.Threading.Tasks.Task.Factory.StartNew(() => {
			}, System.Threading.Tasks.TaskCreationOptions.AttachedToParent);

			System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Factory.ContinueWhenAll(tasks, wordCountTasks => {
				foreach (System.Threading.Tasks.Task tt in wordCountTasks) {
					tt.Dispose();
				}
				System.Array.Clear(wordCountTasks, 0, wordCountTasks.Length);
				wordCountTasks = null;
				foreach (System.Threading.Tasks.Task tt in tasks) {
					tt.Dispose();
				}
				System.Array.Clear(tasks, 0, tasks.Length);
				tasks = null;
			});
			task.Wait();
			task.Dispose();

			//BusEngine.Engine.SettingEngine = settingDefault;
			BusEngine.Engine.SettingProject = settingDefault;

			//settingDefault = null;

			// инициализируем язык
			new BusEngine.Localization().Load();

			// включаем консоль
			int r_DisplayInfo;
			int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["r_DisplayInfo"], out r_DisplayInfo);

			if (r_DisplayInfo > 0) {
				BusEngine.Log.ConsoleShow();

				if (r_DisplayInfo > 1) {
					BusEngine.Log.Info("Device UserAgent: {0}", BusEngine.Engine.Device.UserAgent);
					BusEngine.Log.Info("Device OS: {0}", BusEngine.Engine.Device.Name);
					BusEngine.Log.Info("Device Version OS: {0}", BusEngine.Engine.Device.Version);
					BusEngine.Log.Info("Device Processor: {0}", BusEngine.Engine.Device.Processor);
					BusEngine.Log.Info("Device ProcessorCount: {0}", BusEngine.Engine.Device.ProcessorCount);
					BusEngine.Log.Info("Language file: {0}", BusEngine.Localization.LanguageStatic);
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
				BusEngine.Engine.Timer = new System.Timers.Timer(sys_MemoryClearTime * 1000);
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

			int FPSSetting;
			if (!int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["sys_FPS"], out FPSSetting)) {
				FPSSetting = 100;
			}
			BusEngine.Engine.FPSSetting = FPSSetting;

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

			BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint);

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
			//BusEngine.Log.Info("Engine Stop ~");
			//BusEngine.Experemental.Log.File("Engine Stop ~");
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

		// функция очистки консоли
		public static void Clear() {
			System.Console.Clear();
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
BusEngine.Shader
*/
	/** API BusEngine.Model */
	public class Model : System.IDisposable {
		public float[] VertexData;
		public float[] TexCoord;
		public float[] NormCoord;
		public int[] IndexData;
		public int[] TexIndex;
		public int[] NormIndex;

		public string Url = "";
		public string Name = "";
		public BusEngine.Shader Shader;
		public string Animation = "";
		public int Program;
		public float X = 0.0F;
		public float Y = 0.0F;
		public float Z = 0.0F;
		public float Height;
		public float Width;
		public float Length;
		public BusEngine.Material Material;
		public static OpenTK.Matrix4 vp, a, p = OpenTK.Matrix4.CreateTranslation(0.0F, 0.0F, 0.0F);
		public int ColorBuffer, VAO, VBO, EBO, progA, progP, progVP, progtime, progmouse, progscroll, progresolution;

		// цвет точек квадратных полигонов
		private static readonly BusEngine.Color[] ColorData = new BusEngine.Color[] { };

		public Model() {
			
		}

		public Model(string url = "", string name = "", BusEngine.Shader shader = null, string animation = "") {
			Url = url;
			Name = name;
			Shader = shader;

			if (Shader != null) {
				this.Program = Shader.Program;
			}
		}

		public bool Load() {
			return Load(Url);
		}

		public bool Load(string url = "") {
			return import(url);
		}

		private bool export(string url) {
			if (!System.IO.File.Exists(url)) {
				System.ConsoleColor cc = System.Console.ForegroundColor;
				System.Console.ForegroundColor = System.ConsoleColor.Red;
				BusEngine.Log.Info("Failed to open the file: " + url);
				System.Console.ForegroundColor = cc;

				return false;
			} else {
				return false;
			}
		}

		private bool import(string url) {
			if (!System.IO.File.Exists(url)) {
				System.ConsoleColor cc = System.Console.ForegroundColor;
				System.Console.ForegroundColor = System.ConsoleColor.Red;
				BusEngine.Log.Info("Failed to open the file: " + url);
				System.Console.ForegroundColor = cc;

				return false;
			}
 
			using (System.IO.StreamReader sr = new System.IO.StreamReader(url)) {
				int i, ii, l, ll;
				float x, y, z;
				string[] lines = sr.ReadToEnd().Split(new string[] {"\r\n", "\n\r", "\n"}, System.StringSplitOptions.RemoveEmptyEntries);

				//24
				VertexData = new float[0];
				TexCoord = new float[0];
				NormCoord = new float[0];

				//36 - если полигон треугольник, 24 - если квадрат
				IndexData = new int[0];
				TexIndex = new int[0];
				NormIndex = new int[0];
 
				for (i = 0; i < lines.Length; i++) {
					if (lines[i].StartsWith("#")) {
						continue;
					}

					string line = lines[i].Trim();
					System.Collections.Generic.List<string> values = new System.Collections.Generic.List<string>(System.Text.RegularExpressions.Regex.Split(line, @"\s+"));

					l = values.Count;

					if (l == 0) {
						continue;
					}
 
					if (values[0] == "v") {
						System.Array.Resize(ref VertexData, VertexData.Length + 3);

						float.TryParse(values[1], out x);
						VertexData[VertexData.Length - 3] = x;
						float.TryParse(values[2], out y);
						VertexData[VertexData.Length - 2] = y;
						float.TryParse(values[3], out z);
						VertexData[VertexData.Length - 1] = z;
					} else if (values[0] == "vt") {
						System.Array.Resize(ref TexCoord, TexCoord.Length + 2);

						float.TryParse(values[1], out x);
						TexCoord[TexCoord.Length - 2] = x;
						float.TryParse(values[2], out y);
						TexCoord[TexCoord.Length - 1] = y;
					} else if (values[0] == "vn") {
						System.Array.Resize(ref NormCoord, NormCoord.Length + 3);

						float.TryParse(values[1], out x);
						NormCoord[NormCoord.Length - 3] = x;
						float.TryParse(values[2], out y);
						NormCoord[NormCoord.Length - 2] = y;
						float.TryParse(values[3], out z);
						NormCoord[NormCoord.Length - 1] = z;
					} else if (values[0] == "f") {
						for (ii = 1; ii < l; ii++) {
							string[] w = values[ii].Split(new char[] {'/'});

							System.Array.Resize(ref IndexData, IndexData.Length + 1);
							int.TryParse(w[0], out ll);
							IndexData[IndexData.Length - 1] = ll - 1;

							System.Array.Resize(ref TexIndex, TexIndex.Length + 1);
							int.TryParse(w[1], out ll);
							TexIndex[TexIndex.Length - 1] = ll - 1;

							System.Array.Resize(ref NormIndex, NormIndex.Length + 1);
							int.TryParse(w[2], out ll);
							NormIndex[NormIndex.Length - 1] = ll - 1;
						}
					}
				}

				foreach (int index in IndexData) {
					IndexData[index] = index;
				}



				//texture1 = texture(BusEngine.Engine.DataDirectory + "Textures/RayMarchingOpenGL/test0.png");
				//texture1 = texture(BusEngine.Engine.DataDirectory + "Textures/Cloud/1.png");
				/* texture1 = texture(BusEngine.Engine.DataDirectory + "Textures/Vulcan/1.jpg");
				OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture0);
				OpenTK.Graphics.OpenGL.GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, texture1);

				//texture2 = texture(BusEngine.Engine.DataDirectory + "Textures/RayMarchingOpenGL/hex.png");
				texture2 = texture(BusEngine.Engine.DataDirectory + "Textures/Vulcan/2.jpg");
				OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture1);
				OpenTK.Graphics.OpenGL.GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, texture2); */

				/* texture3 = texture(BusEngine.Engine.DataDirectory + "Textures/RayMarchingOpenGL/white_marble1.png");
				OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture2);
				OpenTK.Graphics.OpenGL.GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, texture3);

				texture4 = texture(BusEngine.Engine.DataDirectory + "Textures/RayMarchingOpenGL/roof/texture3.jpg");
				OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture3);
				OpenTK.Graphics.OpenGL.GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, texture4);

				texture5 = texture(BusEngine.Engine.DataDirectory + "Textures/RayMarchingOpenGL/black_marble1.png");
				OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture4);
				OpenTK.Graphics.OpenGL.GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, texture5);

				texture6 = texture(BusEngine.Engine.DataDirectory + "Textures/RayMarchingOpenGL/green_marble1.png");
				OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture5);
				OpenTK.Graphics.OpenGL.GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, texture6);

				texture7 = texture(BusEngine.Engine.DataDirectory + "Textures/RayMarchingOpenGL/roof/height3.png");
				OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture6);
				OpenTK.Graphics.OpenGL.GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, texture7); */

				// создаём шейдеры

				OpenTK.Graphics.OpenGL.GL.UseProgram(Program);

				progA = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(Program, "A");
				progP = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(Program, "P");
				progVP = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(Program, "VP");

				progtime = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(Program, "u_time");
				progmouse = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(Program, "u_mouse");
				progscroll = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(Program, "u_scroll");
				progresolution = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(Program, "u_resolution");

				OpenTK.Graphics.OpenGL.GL.Uniform2(progresolution, new OpenTK.Vector2(BusEngine.UI.Canvas.WinForm.Width, BusEngine.UI.Canvas.WinForm.Height));

				OpenTK.Graphics.OpenGL.GL.Uniform1(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(Program, "u_texture1"), 0);
				OpenTK.Graphics.OpenGL.GL.Uniform1(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(Program, "u_texture2"), 1);
				OpenTK.Graphics.OpenGL.GL.Uniform1(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(Program, "u_texture3"), 2);
				OpenTK.Graphics.OpenGL.GL.Uniform1(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(Program, "u_texture4"), 3);
				OpenTK.Graphics.OpenGL.GL.Uniform1(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(Program, "u_texture5"), 4);
				OpenTK.Graphics.OpenGL.GL.Uniform1(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(Program, "u_texture6"), 5);
				OpenTK.Graphics.OpenGL.GL.Uniform1(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(Program, "u_texture7"), 6);

				//OpenTK.Graphics.OpenGL.GL.Arb.CompileShaderInclude(Program, 0, new string[3] {BusEngine.Engine.EngineDirectory + "Engine/Shader/Test/hg_sdf.glsl", BusEngine.Engine.EngineDirectory + "Engine/Shader/Test/", "hg_sdf.glsl"}, new int[0] {});



				BusEngine.Log.Info("Program 5555555555: {0}", Program);
				BusEngine.Log.Info("IndexData 5555555555: {0} {1}", IndexData.Length, IndexData);
				BusEngine.Log.Info("VertexData 5555555555: {0}", VertexData.Length);

				// Vertex Arrays Object (VAO)
				//VAO = OpenTK.Graphics.OpenGL.GL.GenBuffer();
				VAO = OpenTK.Graphics.OpenGL.GL.GenVertexArray();
				//OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, VAO);
				OpenTK.Graphics.OpenGL.GL.BindVertexArray(VAO);
				//OpenTK.Graphics.OpenGL.GL.DeleteBuffer(VAO);
				//OpenTK.Graphics.OpenGL.GL.DeleteVertexArray(VAO);

				// 24 Vertex Buffer Object (VBO)
				VBO = OpenTK.Graphics.OpenGL.GL.GenBuffer();
				//VBO = OpenTK.Graphics.OpenGL.GL.GenVertexArray();
				OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, VBO);
				OpenTK.Graphics.OpenGL.GL.BufferData(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, VertexData.Length * sizeof(float) * 3, VertexData, OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
				OpenTK.Graphics.OpenGL.GL.VertexAttribPointer(0, 3, OpenTK.Graphics.OpenGL.VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);
				OpenTK.Graphics.OpenGL.GL.EnableVertexAttribArray(0);

				// 24
				ColorBuffer = OpenTK.Graphics.OpenGL.GL.GenBuffer();
				OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, ColorBuffer);
				OpenTK.Graphics.OpenGL.GL.BufferData(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, ColorData.Length * sizeof(float) * 4, ColorData, OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
				OpenTK.Graphics.OpenGL.GL.VertexAttribPointer(1, 4, OpenTK.Graphics.OpenGL.VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);
				OpenTK.Graphics.OpenGL.GL.EnableVertexAttribArray(1);

				// 36
				EBO = OpenTK.Graphics.OpenGL.GL.GenBuffer();
				OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ElementArrayBuffer, EBO);
				OpenTK.Graphics.OpenGL.GL.BufferData(OpenTK.Graphics.OpenGL.BufferTarget.ElementArrayBuffer, IndexData.Length * sizeof(int), IndexData, OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);

				BusEngine.Log.Info("VAO: {0}", VAO);
				BusEngine.Log.Info("VBO: {0}", VBO);
				BusEngine.Log.Info("EBO: {0}", EBO);
				BusEngine.Log.Info("ColorBuffer: {0}", ColorBuffer);

				/* BusEngine.Log.Info("IndexData h: {0}", 36 * sizeof(int));
				BusEngine.Log.Info("VertexData h: {0}", 24 * sizeof(float) * 3);

				BusEngine.Log.Info("IndexData h: {0}", 24 * sizeof(int));
				BusEngine.Log.Info("VertexData h: {0}", 8 * sizeof(float) * 3 * 2); */
			}

			return true;
		}

		private int texture(string path) {
			int id;
			//OpenTK.Graphics.OpenGL.GL.GenTextures(1, out id);
			id = OpenTK.Graphics.OpenGL.GL.GenTexture();

			OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture1);
			OpenTK.Graphics.OpenGL.GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, id);

			/* OpenTK.Graphics.ES10.GL.TexParameter(OpenTK.Graphics.ES10.All.Texture2D, OpenTK.Graphics.ES10.All.TextureMinFilter, (int)OpenTK.Graphics.ES10.All.Linear);
			OpenTK.Graphics.ES10.GL.TexParameter(OpenTK.Graphics.ES10.All.Texture2D, OpenTK.Graphics.ES10.All.TextureMagFilter, (int)OpenTK.Graphics.ES10.All.Linear); */

			//OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapS, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.ClampToBorder);
			//OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapT, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.ClampToBorder);
			//OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMinFilter, (int)OpenTK.Graphics.OpenGL.TextureMinFilter.Nearest);
			//OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMagFilter, (int)OpenTK.Graphics.OpenGL.TextureMagFilter.Linear);

			//BusEngine.Log.Info("sssssssssss: {0}", id);

			if (System.IO.File.Exists(path)) {
				using (System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(path, false)) {
					System.Drawing.Imaging.BitmapData data = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);

					OpenTK.Graphics.OpenGL.GL.TexImage2D(
						OpenTK.Graphics.OpenGL.TextureTarget.Texture2D,
						0,
						OpenTK.Graphics.OpenGL.PixelInternalFormat.Rgba,
						bitmap.Width,
						bitmap.Height,
						0,
						OpenTK.Graphics.OpenGL.PixelFormat.Bgra,
						OpenTK.Graphics.OpenGL.PixelType.UnsignedByte,
						data.Scan0
					);

					OpenTK.Graphics.OpenGL.GL.GenerateMipmap(OpenTK.Graphics.OpenGL.GenerateMipmapTarget.Texture2D);

					bitmap.UnlockBits(data);
				}
			}

			return id;
		}

		public void SwapBuffers() {
			// отправляем настройки расположения модели
			OpenTK.Graphics.OpenGL.GL.UniformMatrix4(progP, true, ref p);

			// рисуем модель
			//OpenTK.Graphics.OpenGL.BeginMode.Points
			//OpenTK.Graphics.OpenGL.BeginMode.Lines
			//OpenTK.Graphics.OpenGL.BeginMode.LineLoop
			//OpenTK.Graphics.OpenGL.BeginMode.LineStrip
			//OpenTK.Graphics.OpenGL.BeginMode.Triangles
			//OpenTK.Graphics.OpenGL.BeginMode.TriangleStrip
			//OpenTK.Graphics.OpenGL.BeginMode.TriangleFan
			//OpenTK.Graphics.OpenGL.BeginMode.Quads
			//OpenTK.Graphics.OpenGL.BeginMode.QuadStrip
			//OpenTK.Graphics.OpenGL.BeginMode.Polygon
			//OpenTK.Graphics.OpenGL.BeginMode.Patches
			//OpenTK.Graphics.OpenGL.BeginMode.LinesAdjacency
			//OpenTK.Graphics.OpenGL.BeginMode.LineStripAdjacency
			//OpenTK.Graphics.OpenGL.BeginMode.TrianglesAdjacency
			//OpenTK.Graphics.OpenGL.BeginMode.TriangleStripAdjacency
			//OpenTK.Graphics.OpenGL.GL.DrawElements(OpenTK.Graphics.OpenGL.BeginMode.Triangles, IndexData.Length, OpenTK.Graphics.OpenGL.DrawElementsType.UnsignedInt, 0);
			//OpenTK.Graphics.OpenGL.GL.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.Quads, 0, VertexData.Length);
			OpenTK.Graphics.OpenGL.GL.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.Quads, 0, 72);
			//OpenTK.Graphics.OpenGL.GL.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.Triangles, 0, VertexData.Length);
			//OpenTK.Graphics.OpenGL.GL.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.Quads, 0, 12);
			//OpenTK.Graphics.OpenGL.GL.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.Quads, 24, 12);
			//OpenTK.Graphics.OpenGL.GL.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.Quads, 48, 12);
		}

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
    /** API BusEngine.Object */
	public class Object : System.IDisposable {
		public Object() {
			
		}

		public void Dispose() {

		}

		~Object() {
			BusEngine.Log.Info("Object ~");
		}
	}
    /** API BusEngine.Object */
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
		private static BusEngine.Benchmark Benchmark;

		// при запуске BusEngine до создания формы
		public IPlugin(string stage = "Initialize") {
			if (BusEngine.Engine.IsShutdown && stage != "Shutdown") {
				return;
			}
			stage = stage.ToLower();
			Stage = stage;
			if (stage != "ongameupdate") {
				BusEngine.Log.Info( "============================ System Plugins Start ============================" );
			}

			int i, i2, i3, l = BusEngine.Engine.SettingProject["require"]["plugins"].Count;
			string m, path;
			//object[] x1 = new object[0];
			object[] x2 = new object[1];
			object[] x3 = new object[2];
			//System.Type[] t1 = new System.Type[] {};
			System.Type[] t2 = new System.Type[] { typeof(string) };
			System.Type[] t3 = new System.Type[] { typeof(string), typeof(string) };
			int typ = 3;

			if (typ == 1 && Modules == null) {
				#if BUSENGINE_BENCHMARK
				using (new BusEngine.Benchmark("Modules " + stage + " " + System.Threading.Thread.CurrentThread.ManagedThreadId)) {
				#endif

					Modules = new System.Collections.Concurrent.ConcurrentDictionary<string, System.Type[]>(System.Environment.ProcessorCount, l);

					System.Threading.Tasks.Task[] tasks = new System.Threading.Tasks.Task[l];

					for (i = 0; i < l; ++i) {
						string ap = BusEngine.Engine.SettingProject["require"]["plugins"][i]["path"];
						if (!Modules.ContainsKey(ap)) {
							tasks[i] = System.Threading.Tasks.Task.Factory.StartNew(() => {
								Modules[ap] = System.Reflection.Assembly.LoadFile(ap).GetTypes();
							}, System.Threading.Tasks.TaskCreationOptions.AttachedToParent);
						}
					}

					System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Factory.ContinueWhenAll(tasks, wordCountTasks => {
						BusEngine.Log.Info("Modules {0}", Modules.Count);
						BusEngine.Log.Info("Modules {0}", wordCountTasks.Length);
						BusEngine.Log.Info("Modules {0}", tasks.Length);
						foreach (System.Threading.Tasks.Task tt in wordCountTasks) {
							tt.Dispose();
						}
						System.Array.Clear(wordCountTasks, 0, wordCountTasks.Length);
						wordCountTasks = null;
						foreach (System.Threading.Tasks.Task tt in tasks) {
							tt.Dispose();
						}
						System.Array.Clear(tasks, 0, tasks.Length);
						tasks = null;
					});
					task.Wait();
					task.Dispose();

				#if BUSENGINE_BENCHMARK
				}
				#endif
			} else if (typ == 2 && Modules == null) {
				#if BUSENGINE_BENCHMARK
				using (new BusEngine.Benchmark("Modules " + stage + " " + System.Threading.Thread.CurrentThread.ManagedThreadId)) {
				#endif

					Modules = new System.Collections.Concurrent.ConcurrentDictionary<string, System.Type[]>(System.Environment.ProcessorCount, l);

					System.Threading.Tasks.Task[] tasks = new System.Threading.Tasks.Task[l];

					for (i = 0; i < l; ++i) {
						string ap = BusEngine.Engine.SettingProject["require"]["plugins"][i]["path"];
						if (!Modules.ContainsKey(ap)) {
							tasks[i] = System.Threading.Tasks.Task.Run(() => {
								Modules[ap] = System.Reflection.Assembly.LoadFile(ap).GetTypes();
							});
						}
					}

					System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Factory.ContinueWhenAll(tasks, (wordCountTasks) => {
						BusEngine.Log.Info("Modules {0}", Modules.Count);
						BusEngine.Log.Info("Modules {0}", wordCountTasks.Length);
						BusEngine.Log.Info("Modules {0}", tasks.Length);
						foreach (System.Threading.Tasks.Task tt in wordCountTasks) {
							tt.Dispose();
						}
						System.Array.Clear(wordCountTasks, 0, wordCountTasks.Length);
						wordCountTasks = null;
						foreach (System.Threading.Tasks.Task tt in tasks) {
							tt.Dispose();
						}
						System.Array.Clear(tasks, 0, tasks.Length);
						tasks = null;
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

					string ap;
					Modules = new System.Collections.Concurrent.ConcurrentDictionary<string, System.Type[]>(System.Environment.ProcessorCount, l);

					for (i = 0; i < l; ++i) {
						ap = BusEngine.Engine.SettingProject["require"]["plugins"][i]["path"];
						if (!Modules.ContainsKey(ap)) {
							Modules[ap] = System.Reflection.Assembly.LoadFile(ap).GetTypes();
						}
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
				int sys_Benchmark;
				if (int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["sys_Benchmark"], out sys_Benchmark) && sys_Benchmark > 0) {
					Benchmark = new BusEngine.Benchmark(path + " " + stage);
				}
				#endif
					// https://learn.microsoft.com/ru-ru/dotnet/framework/deployment/best-practices-for-assembly-loading
					foreach (System.Type type in Modules[path]) {
						if (type.IsSubclassOf(typeof(BusEngine.Plugin))) {
							foreach (System.Reflection.MethodInfo method in type.GetMethods(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.DeclaredOnly)) {
								m = method.Name.ToLower();
								if (m == stage || m == stage + "async") {
									if (BusEngine.Log.ConsoleStatus == true && stage != "ongameupdate") {
										BusEngine.Log.Info(path);
										BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("text_name_class") + ": {0}", type.FullName);
										BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("text_name_method") + ": {0}", method.Name);
									}

									if (m == stage + "async" || IsAsync(method)) {
										if (BusEngine.Log.ConsoleStatus == true && stage != "ongameupdate") {
											BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("text_name_method_start") + ": {0}", "Async");
										}
										// https://learn.microsoft.com/ru-ru/dotnet/api/system.threading.thread?view=net-7.0
										// https://www.youtube.com/watch?v=D9qcKV4j75U&list=PLWCoo5SF-qAMDIAqikhB2hvIytrMiR5TC
										// https://dzen.ru/video/watch/628d09872d486972bd96589d
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
											} else if (i2 == 1) {
												x2[0] = path;
												method.Invoke(System.Activator.CreateInstance(type), x2);
											} else if (i2 == 2) {
												x3[0] = path;
												x3[1] = stage;
												method.Invoke(System.Activator.CreateInstance(type), x3);
											}
											if (stage != "initialize" && stage != "ongameupdate") {
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
										if (BusEngine.Log.ConsoleStatus == true && stage != "ongameupdate") {
											BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("text_name_method_start") + ": {0}", "Sync");
										}
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
										} else if (i2 == 1) {
											x2[0] = path;
											method.Invoke(System.Activator.CreateInstance(type), x2);
										} else if (i2 == 2) {
											x3[0] = path;
											x3[1] = stage;
											method.Invoke(System.Activator.CreateInstance(type), x3);
										}
										if (stage != "initialize" && stage != "ongameupdate") {
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
				if (sys_Benchmark > 0) {
					Benchmark.Dispose();
				}
				#endif
			}

			/* x1 = null;
			x2 = null;
			x3 = null; */

			if (stage != "ongameupdate") {
				BusEngine.Log.Info( "============================ System Plugins Stop  ============================" );
			}

			//System.GC.SuppressFinalize(this);
		}

		public void Dispose() {
			BusEngine.Log.Info("IPlugin Dispose {0}", Stage);
		}

		~IPlugin() {
			//BusEngine.Log.Info("IPlugin ~ {0}", Stage);
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

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.Log
*/
    /** API BusEngine.Shader */
	public class Shader : System.IDisposable {
		public string Vert = "";
		public string Vertex = "";
		public string Tesc = "";
		public string Tescontrol = "";
		public string Tess = "";
		public string Tessellation = "";
		public string Geom = "";
		public string Geometry = "";
		public string Frag = "";
		public string Fragment = "";
		public string Comp = "";
		public string Compute = "";
		public string Incl = "";
		public string Include = "";
		public int Program;

		public Shader(string vert = "", string vertex = "", string tesc = "", string tescontrol = "", string tess = "", string tessellation = "", string geom = "", string geometry = "", string frag = "", string fragment = "", string comp = "", string compute = "", string incl = "", string include = "") {
			if (!string.IsNullOrWhiteSpace(vert)) {
				Vert = vert;
				Vertex = vert;
			}
			if (!string.IsNullOrWhiteSpace(vertex)) {
				Vert = vertex;
				Vertex = vertex;
			}

			if (!string.IsNullOrWhiteSpace(tesc)) {
				Tesc = tesc;
				Tescontrol = tesc;
			}
			if (!string.IsNullOrWhiteSpace(tescontrol)) {
				Tesc = tescontrol;
				Tescontrol = tescontrol;
			}

			if (!string.IsNullOrWhiteSpace(tess)) {
				Tess = tess;
				Tessellation = tess;
			}
			if (!string.IsNullOrWhiteSpace(tessellation)) {
				Tess = tessellation;
				Tessellation = tessellation;
			}

			if (!string.IsNullOrWhiteSpace(geom)) {
				Geom = geom;
				Geometry = geom;
			}
			if (!string.IsNullOrWhiteSpace(geometry)) {
				Geom = geometry;
				Geometry = geometry;
			}

			if (!string.IsNullOrWhiteSpace(frag)) {
				Frag = frag;
				Fragment = frag;
			}
			if (!string.IsNullOrWhiteSpace(fragment)) {
				Frag = fragment;
				Fragment = fragment;
			}

			if (!string.IsNullOrWhiteSpace(comp)) {
				Comp = comp;
				Compute = comp;
			}
			if (!string.IsNullOrWhiteSpace(compute)) {
				Comp = compute;
				Compute = compute;
			}

			if (!string.IsNullOrWhiteSpace(incl)) {
				Incl = incl;
				Include = incl;
			}
			if (!string.IsNullOrWhiteSpace(include)) {
				Incl = include;
				Include = include;
			}

			Program = this.GenProgram(
				vertex: Vertex,
				tescontrol: Tescontrol,
				tessellation: Tessellation,
				geometry: Geometry,
				fragment: Fragment,
				compute: Compute, 
				include: Include
			);
		}

		private int CompileShader(OpenTK.Graphics.OpenGL.ShaderType type, string source = "") {
			int success, shader = OpenTK.Graphics.OpenGL.GL.CreateShader(type);

			if (System.IO.File.Exists(source)) {
				OpenTK.Graphics.OpenGL.GL.ShaderSource(shader, System.IO.File.ReadAllText(source));
			} else {
				OpenTK.Graphics.OpenGL.GL.ShaderSource(shader, source);
			}

			OpenTK.Graphics.OpenGL.GL.CompileShader(shader);

			OpenTK.Graphics.OpenGL.GL.GetShader(shader, OpenTK.Graphics.OpenGL.ShaderParameter.CompileStatus, out success);
			if (success == 0) {
				throw new System.Exception("Failed to compile {type}: " + OpenTK.Graphics.OpenGL.GL.GetShaderInfoLog(shader));
			}

			return shader;
		}

		private int GenProgram(string vertex = "", string tescontrol = "", string tessellation = "", string geometry = "", string fragment = "", string compute = "", string include = "") {
			int vert = 0, tesc = 0, tess = 0, geom = 0, frag = 0, comp = 0, incl = 0, success = 0, program = OpenTK.Graphics.OpenGL.GL.CreateProgram();

			// для управления вершинами полигона
			if (!string.IsNullOrWhiteSpace(vertex)) {
				BusEngine.Log.Info("vertex");
				vert = CompileShader(OpenTK.Graphics.OpenGL.ShaderType.VertexShader, vertex);
				OpenTK.Graphics.OpenGL.GL.AttachShader(program, vert);
			} else {
				vertex = "";
			}

			if (!string.IsNullOrWhiteSpace(tescontrol)) {
				BusEngine.Log.Info("tescontrol");
				tesc = CompileShader(OpenTK.Graphics.OpenGL.ShaderType.TessControlShader, tescontrol);
				OpenTK.Graphics.OpenGL.GL.AttachShader(program, tesc);
			} else {
				tescontrol = "";
			}

			// для создания дополнительных полигонов в целях сглаживания краёв
			if (!string.IsNullOrWhiteSpace(tessellation)) {
				BusEngine.Log.Info("tessellation");
				tess = CompileShader(OpenTK.Graphics.OpenGL.ShaderType.TessEvaluationShader, tessellation);
				OpenTK.Graphics.OpenGL.GL.AttachShader(program, tess);
			} else {
				tessellation = "";
			}

			// для управления группой треугольных полигонов (в зависимости от видеокарты до 128 вершин = 42 полигона), увеличение полигонов через этот шейдер - повышает производительность
			if (!string.IsNullOrWhiteSpace(geometry)) {
				BusEngine.Log.Info("geometry");
				geom = CompileShader(OpenTK.Graphics.OpenGL.ShaderType.GeometryShader, geometry);
				OpenTK.Graphics.OpenGL.GL.AttachShader(program, geom);
			} else {
				geometry = "";
			}

			// для управления каждым пикселем экрана (управление тенями, освещением, отражением, цветом, текстурами и т.д.)
			if (!string.IsNullOrWhiteSpace(fragment)) {
				BusEngine.Log.Info("fragment");
				frag = CompileShader(OpenTK.Graphics.OpenGL.ShaderType.FragmentShader, fragment);
				OpenTK.Graphics.OpenGL.GL.AttachShader(program, frag);
			} else {
				fragment = "";
			}

			// для инных вычеслений (генерация частиц, изображений) https://steps3d.narod.ru/tutorials/compute-shaders-tutorial.html
			if (!string.IsNullOrWhiteSpace(compute)) {
				BusEngine.Log.Info("compute");
				comp = CompileShader(OpenTK.Graphics.OpenGL.ShaderType.ComputeShader, compute);
				OpenTK.Graphics.OpenGL.GL.AttachShader(program, comp);
			} else {
				compute = "";
			}

			// для подключения разлчных функций
			if (!string.IsNullOrWhiteSpace(include)) {
				BusEngine.Log.Info("include");
				incl = CompileShader(OpenTK.Graphics.OpenGL.ShaderType.ComputeShader, include);
				OpenTK.Graphics.OpenGL.GL.AttachShader(program, incl);
			} else {
				include = "";
			}

			OpenTK.Graphics.OpenGL.GL.LinkProgram(program);
			OpenTK.Graphics.OpenGL.GL.GetProgram(program, OpenTK.Graphics.OpenGL.GetProgramParameterName.LinkStatus, out success);

			if (success == 0) {
				throw new System.Exception("Could not link program: " + OpenTK.Graphics.OpenGL.GL.GetProgramInfoLog(program));
			}

			if (vertex != "") {
				OpenTK.Graphics.OpenGL.GL.DetachShader(program, vert);
				OpenTK.Graphics.OpenGL.GL.DeleteShader(vert);
			}

			if (tescontrol != "") {
				OpenTK.Graphics.OpenGL.GL.DetachShader(program, tesc);
				OpenTK.Graphics.OpenGL.GL.DeleteShader(tesc);
			}

			if (tessellation != "") {
				OpenTK.Graphics.OpenGL.GL.DetachShader(program, tess);
				OpenTK.Graphics.OpenGL.GL.DeleteShader(tess);
			}

			if (geometry != "") {
				OpenTK.Graphics.OpenGL.GL.DetachShader(program, geom);
				OpenTK.Graphics.OpenGL.GL.DeleteShader(geom);
			}

			if (fragment != "") {
				OpenTK.Graphics.OpenGL.GL.DetachShader(program, frag);
				OpenTK.Graphics.OpenGL.GL.DeleteShader(frag);
			}

			if (compute != "") {
				OpenTK.Graphics.OpenGL.GL.DetachShader(program, comp);
				OpenTK.Graphics.OpenGL.GL.DeleteShader(comp);
			}

			if (include != "") {
				OpenTK.Graphics.OpenGL.GL.DetachShader(program, incl);
				OpenTK.Graphics.OpenGL.GL.DeleteShader(incl);
			}

			return program;
		}

		public void Dispose() {

		}

		~Shader() {
			BusEngine.Log.Info("Shader ~");
		}
	}
    /** API BusEngine.Shader */
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
	public static class Json {
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
BusEngine.Vector2
BusEngine.Vector3
BusEngine.Vector4
*/

    [System.Serializable]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Quaternion : System.IEquatable<Quaternion> {
        public Vector3 Xyz;
        public float W;

        public Quaternion(Vector3 v, float w) {
            Xyz = v;
            W = w;
        }

        public Quaternion(float x, float y, float z, float w) : this(new Vector3(x, y, z), w) { }

        public Quaternion(float rotationX, float rotationY, float rotationZ) {
            rotationX *= 0.5f;
            rotationY *= 0.5f;
            rotationZ *= 0.5f;

            float c1 = (float)System.Math.Cos(rotationX);
            float c2 = (float)System.Math.Cos(rotationY);
            float c3 = (float)System.Math.Cos(rotationZ);
            float s1 = (float)System.Math.Sin(rotationX);
            float s2 = (float)System.Math.Sin(rotationY);
            float s3 = (float)System.Math.Sin(rotationZ);

            W = c1 * c2 * c3 - s1 * s2 * s3;
            Xyz.X = s1 * c2 * c3 + c1 * s2 * s3;
            Xyz.Y = c1 * s2 * c3 - s1 * c2 * s3;
            Xyz.Z = c1 * c2 * s3 + s1 * s2 * c3;
        }

        public Quaternion(Vector3 eulerAngles) : this(eulerAngles.X, eulerAngles.Y, eulerAngles.Z) { }

        [System.Xml.Serialization.XmlIgnore]
        public float X { get { return Xyz.X; } set { Xyz.X = value; } }

        [System.Xml.Serialization.XmlIgnore]
        public float Y { get { return Xyz.Y; } set { Xyz.Y = value; } }

        [System.Xml.Serialization.XmlIgnore]
        public float Z { get { return Xyz.Z; } set { Xyz.Z = value; } }

        public void ToAxisAngle(out Vector3 axis, out float angle) {
            Vector4 result = ToAxisAngle();
            axis = result.Xyz;
            angle = result.W;
        }

        public Vector4 ToAxisAngle() {
            Quaternion q = this;
            if (System.Math.Abs(q.W) > 1.0f) {
                q.Normalize();
            }

            Vector4 result = new Vector4();

            result.W = 2.0f * (float)System.Math.Acos(q.W); // angle
            float den = (float)System.Math.Sqrt(1.0 - q.W * q.W);
            if (den > 0.0001f) {
                result.Xyz = q.Xyz / den;
            } else {
                // This occurs when the angle is zero.
                // Not a problem: just set an arbitrary normalized axis.
                result.Xyz = Vector3.UnitX;
            }

            return result;
        }

        public float Length {
            get {
                return (float)System.Math.Sqrt(W * W + Xyz.LengthSquared);
            }
        }

        public float LengthSquared {
            get {
                return W * W + Xyz.LengthSquared;
            }
        }

        public Quaternion Normalized() {
            Quaternion q = this;
            q.Normalize();
            return q;
        }

        public void Invert() {
            Invert(ref this, out this);
        }

        public Quaternion Inverted() {
            var q = this;
            q.Invert();
            return q;
        }

        public void Normalize() {
            float scale = 1.0f / this.Length;
            Xyz *= scale;
            W *= scale;
        }

        public void Conjugate() {
            Xyz = -Xyz;
        }

        public static readonly Quaternion Identity = new Quaternion(0, 0, 0, 1);

        public static Quaternion Add(Quaternion left, Quaternion right) {
            return new Quaternion(
                left.Xyz + right.Xyz,
                left.W + right.W);
        }

        public static void Add(ref Quaternion left, ref Quaternion right, out Quaternion result) {
            result = new Quaternion(
                left.Xyz + right.Xyz,
                left.W + right.W);
        }

        public static Quaternion Sub(Quaternion left, Quaternion right) {
            return  new Quaternion(
                left.Xyz - right.Xyz,
                left.W - right.W);
        }

        public static void Sub(ref Quaternion left, ref Quaternion right, out Quaternion result) {
            result = new Quaternion(
                left.Xyz - right.Xyz,
                left.W - right.W);
        }

        public static Quaternion Multiply(Quaternion left, Quaternion right) {
            Quaternion result;
            Multiply(ref left, ref right, out result);
            return result;
        }

        public static void Multiply(ref Quaternion left, ref Quaternion right, out Quaternion result) {
            result = new Quaternion(
                right.W * left.Xyz + left.W * right.Xyz + Vector3.Cross(left.Xyz, right.Xyz),
                left.W * right.W - Vector3.Dot(left.Xyz, right.Xyz));
        }

        public static void Multiply(ref Quaternion quaternion, float scale, out Quaternion result) {
            result = new Quaternion(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
        }

        public static Quaternion Multiply(Quaternion quaternion, float scale) {
            return new Quaternion(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
        }

        public static Quaternion Conjugate(Quaternion q) {
            return new Quaternion(-q.Xyz, q.W);
        }

        public static void Conjugate(ref Quaternion q, out Quaternion result) {
            result = new Quaternion(-q.Xyz, q.W);
        }

        public static Quaternion Invert(Quaternion q) {
            Quaternion result;
            Invert(ref q, out result);
            return result;
        }

        public static void Invert(ref Quaternion q, out Quaternion result) {
            float lengthSq = q.LengthSquared;
            if (lengthSq != 0.0) {
                float i = 1.0f / lengthSq;
                result = new Quaternion(q.Xyz * -i, q.W * i);
            } else {
                result = q;
            }
        }

        public static Quaternion Normalize(Quaternion q) {
            Quaternion result;
            Normalize(ref q, out result);
            return result;
        }

        public static void Normalize(ref Quaternion q, out Quaternion result) {
            float scale = 1.0f / q.Length;
            result = new Quaternion(q.Xyz * scale, q.W * scale);
        }

        public static Quaternion FromAxisAngle(Vector3 axis, float angle) {
            if (axis.LengthSquared == 0.0f) {
                return Identity;
            }

            Quaternion result = Identity;

            angle *= 0.5f;
            axis.Normalize();
            result.Xyz = axis * (float)System.Math.Sin(angle);
            result.W = (float)System.Math.Cos(angle);

            return Normalize(result);
        }

        public static Quaternion FromEulerAngles(float pitch, float yaw, float roll) {
            return new Quaternion(pitch, yaw, roll);
        }

        public static Quaternion FromEulerAngles(Vector3 eulerAngles) {
            return new Quaternion(eulerAngles);
        }

        public static void FromEulerAngles(ref Vector3 eulerAngles, out Quaternion result) {

            float c1 = (float)System.Math.Cos(eulerAngles.X * 0.5f);
            float c2 = (float)System.Math.Cos(eulerAngles.Y * 0.5f);
            float c3 = (float)System.Math.Cos(eulerAngles.Z * 0.5f);
            float s1 = (float)System.Math.Sin(eulerAngles.X * 0.5f);
            float s2 = (float)System.Math.Sin(eulerAngles.Y * 0.5f);
            float s3 = (float)System.Math.Sin(eulerAngles.Z * 0.5f);

            result.W = c1 * c2 * c3 - s1 * s2 * s3;
            result.Xyz.X = s1 * c2 * c3 + c1 * s2 * s3;
            result.Xyz.Y = c1 * s2 * c3 - s1 * c2 * s3;
            result.Xyz.Z = c1 * c2 * s3 + s1 * s2 * c3;
        }

        /* public static Quaternion FromMatrix(Matrix3 matrix) {
            Quaternion result;
            FromMatrix(ref matrix, out result);
            return result;
        }

        public static void FromMatrix(ref Matrix3 matrix, out Quaternion result) {
            float trace = matrix.Trace;

            if (trace > 0) {
                float s = (float)System.Math.Sqrt(trace + 1) * 2;
                float invS = 1f / s;

                result.W = s * 0.25f;
                result.Xyz.X = (matrix.Row2.Y - matrix.Row1.Z) * invS;
                result.Xyz.Y = (matrix.Row0.Z - matrix.Row2.X) * invS;
                result.Xyz.Z = (matrix.Row1.X - matrix.Row0.Y) * invS;
            } else {
                float m00 = matrix.Row0.X, m11 = matrix.Row1.Y, m22 = matrix.Row2.Z;

                if (m00 > m11 && m00 > m22) {
                    float s = (float)System.Math.Sqrt(1 + m00 - m11 - m22) * 2;
                    float invS = 1f / s;

                    result.W = (matrix.Row2.Y - matrix.Row1.Z) * invS;
                    result.Xyz.X = s * 0.25f;
                    result.Xyz.Y = (matrix.Row0.Y + matrix.Row1.X) * invS;
                    result.Xyz.Z = (matrix.Row0.Z + matrix.Row2.X) * invS;
                } else if (m11 > m22) {
                    float s = (float)System.Math.Sqrt(1 + m11 - m00 - m22) * 2;
                    float invS = 1f / s;

                    result.W = (matrix.Row0.Z - matrix.Row2.X) * invS;
                    result.Xyz.X = (matrix.Row0.Y + matrix.Row1.X) * invS;
                    result.Xyz.Y = s * 0.25f;
                    result.Xyz.Z = (matrix.Row1.Z + matrix.Row2.Y) * invS;
                } else {
                    float s = (float)System.Math.Sqrt(1 + m22 - m00 - m11) * 2;
                    float invS = 1f / s;

                    result.W = (matrix.Row1.X - matrix.Row0.Y) * invS;
                    result.Xyz.X = (matrix.Row0.Z + matrix.Row2.X) * invS;
                    result.Xyz.Y = (matrix.Row1.Z + matrix.Row2.Y) * invS;
                    result.Xyz.Z = s * 0.25f;
                }
            }
        } */

        public static Quaternion Slerp(Quaternion q1, Quaternion q2, float blend) {
            // if either input is zero, return the other.
            if (q1.LengthSquared == 0.0f) {
                if (q2.LengthSquared == 0.0f) {
                    return Identity;
                }
                return q2;
            } else if (q2.LengthSquared == 0.0f) {
                return q1;
            }


            float cosHalfAngle = q1.W * q2.W + Vector3.Dot(q1.Xyz, q2.Xyz);

            if (cosHalfAngle >= 1.0f || cosHalfAngle <= -1.0f) {
                // angle = 0.0f, so just return one input.
                return q1;
            } else if (cosHalfAngle < 0.0f) {
                q2.Xyz = -q2.Xyz;
                q2.W = -q2.W;
                cosHalfAngle = -cosHalfAngle;
            }

            float blendA;
            float blendB;
            if (cosHalfAngle < 0.99f) {
                // do proper slerp for big angles
                float halfAngle = (float)System.Math.Acos(cosHalfAngle);
                float sinHalfAngle = (float)System.Math.Sin(halfAngle);
                float oneOverSinHalfAngle = 1.0f / sinHalfAngle;
                blendA = (float)System.Math.Sin(halfAngle * (1.0f - blend)) * oneOverSinHalfAngle;
                blendB = (float)System.Math.Sin(halfAngle * blend) * oneOverSinHalfAngle;
            } else {
                // do lerp if angle is really small.
                blendA = 1.0f - blend;
                blendB = blend;
            }

            Quaternion result = new Quaternion(blendA * q1.Xyz + blendB * q2.Xyz, blendA * q1.W + blendB * q2.W);
            if (result.LengthSquared > 0.0f) {
                return Normalize(result);
            } else {
                return Identity;
            }
        }

        public static Quaternion operator +(Quaternion left, Quaternion right) {
            left.Xyz += right.Xyz;
            left.W += right.W;
            return left;
        }

        public static Quaternion operator -(Quaternion left, Quaternion right) {
            left.Xyz -= right.Xyz;
            left.W -= right.W;
            return left;
        }

        public static Quaternion operator *(Quaternion left, Quaternion right) {
            Multiply(ref left, ref right, out left);
            return left;
        }

        public static Quaternion operator *(Quaternion quaternion, float scale) {
            Multiply(ref quaternion, scale, out quaternion);
            return quaternion;
        }

        public static Quaternion operator *(float scale, Quaternion quaternion) {
            return new Quaternion(quaternion.X * scale, quaternion.Y * scale, quaternion.Z * scale, quaternion.W * scale);
        }

        public static bool operator ==(Quaternion left, Quaternion right) {
            return left.Equals(right);
        }

        public static bool operator !=(Quaternion left, Quaternion right) {
            return !left.Equals(right);
        }

        public override string ToString() {
            return System.String.Format("V: {0}, W: {1}", Xyz, W);
        }

        public override bool Equals(object other) {
            if (other is Quaternion == false) {
                return false;
            }
            return this == (Quaternion)other;
        }

        public override int GetHashCode() {
            unchecked {
                return (this.Xyz.GetHashCode() * 397) ^ this.W.GetHashCode();
            }
        }

        public bool Equals(Quaternion other) {
            return Xyz == other.Xyz && W == other.W;
        }
    }

	/** API BusEngine.Vector */
    [System.Serializable]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Vector2 : System.IEquatable<Vector2> {
        public float X;
        public float Y;
        public Vector2(float value) {
            X = value;
            Y = value;
        }

        public Vector2(float x, float y) {
            X = x;
            Y = y;
        }

        public float this[int index] {
            get {
                if (index == 0) {
                    return X;
                } else if (index == 1) {
                    return Y;
                }
                throw new System.IndexOutOfRangeException("You tried to access this vector at index: " + index);
            } set {
                if (index == 0) {
                    X = value;
                } else if (index == 1) {
                    Y = value;
                } else {
                    throw new System.IndexOutOfRangeException("You tried to set this vector at index: " + index);
                }
            }
        }

        public float Length {
            get {
                return (float)System.Math.Sqrt(X * X + Y * Y);
            }
        }

        public float LengthFast {
            get {
                return 1.0f / BusEngine.Vector3.InverseSqrtFast(X * X + Y * Y);
            }
        }

        public float LengthSquared {
            get {
                return X * X + Y * Y;
            }
        }

        public Vector2 PerpendicularRight {
            get {
                return new Vector2(Y, -X);
            }
        }

        public Vector2 PerpendicularLeft {
            get {
                return new Vector2(-Y, X);
            }
        }

        public Vector2 Normalized() {
            Vector2 v = this;
            v.Normalize();
            return v;
        }

        public void Normalize() {
            float scale = 1.0f / this.Length;
            X *= scale;
            Y *= scale;
        }

        public void NormalizeFast() {
            float scale = BusEngine.Vector3.InverseSqrtFast(X * X + Y * Y);
            X *= scale;
            Y *= scale;
        }

        public static readonly Vector2 UnitX = new Vector2(1, 0);
        public static readonly Vector2 UnitY = new Vector2(0, 1);
        public static readonly Vector2 Zero = new Vector2(0, 0);
        public static readonly Vector2 One = new Vector2(1, 1);
        public static readonly int SizeInBytes = System.Runtime.InteropServices.Marshal.SizeOf(new Vector2());
        public static Vector2 Add(Vector2 a, Vector2 b) {
            Add(ref a, ref b, out a);
            return a;
        }

        public static void Add(ref Vector2 a, ref Vector2 b, out Vector2 result) {
            result.X = a.X + b.X;
            result.Y = a.Y + b.Y;
        }

        public static Vector2 Subtract(Vector2 a, Vector2 b) {
            Subtract(ref a, ref b, out a);
            return a;
        }

        public static void Subtract(ref Vector2 a, ref Vector2 b, out Vector2 result) {
            result.X = a.X - b.X;
            result.Y = a.Y - b.Y;
        }

        public static Vector2 Multiply(Vector2 vector, float scale) {
            Multiply(ref vector, scale, out vector);
            return vector;
        }

        public static void Multiply(ref Vector2 vector, float scale, out Vector2 result) {
            result.X = vector.X * scale;
            result.Y = vector.Y * scale;
        }

        public static Vector2 Multiply(Vector2 vector, Vector2 scale) {
            Multiply(ref vector, ref scale, out vector);
            return vector;
        }

        public static void Multiply(ref Vector2 vector, ref Vector2 scale, out Vector2 result) {
            result.X = vector.X * scale.X;
            result.Y = vector.Y * scale.Y;
        }

        public static Vector2 Divide(Vector2 vector, float scale) {
            Divide(ref vector, scale, out vector);
            return vector;
        }

        public static void Divide(ref Vector2 vector, float scale, out Vector2 result) {
            result.X = vector.X / scale;
            result.Y = vector.Y / scale;
        }

        public static Vector2 Divide(Vector2 vector, Vector2 scale) {
            Divide(ref vector, ref scale, out vector);
            return vector;
        }

        public static void Divide(ref Vector2 vector, ref Vector2 scale, out Vector2 result) {
            result.X = vector.X / scale.X;
            result.Y = vector.Y / scale.Y;
        }

        public static Vector2 ComponentMin(Vector2 a, Vector2 b) {
            a.X = a.X < b.X ? a.X : b.X;
            a.Y = a.Y < b.Y ? a.Y : b.Y;
            return a;
        }

        public static void ComponentMin(ref Vector2 a, ref Vector2 b, out Vector2 result) {
            result.X = a.X < b.X ? a.X : b.X;
            result.Y = a.Y < b.Y ? a.Y : b.Y;
        }

        public static Vector2 ComponentMax(Vector2 a, Vector2 b) {
            a.X = a.X > b.X ? a.X : b.X;
            a.Y = a.Y > b.Y ? a.Y : b.Y;
            return a;
        }

        public static void ComponentMax(ref Vector2 a, ref Vector2 b, out Vector2 result) {
            result.X = a.X > b.X ? a.X : b.X;
            result.Y = a.Y > b.Y ? a.Y : b.Y;
        }

        public static Vector2 MagnitudeMin(Vector2 left, Vector2 right) {
            return left.LengthSquared < right.LengthSquared ? left : right;
        }

        public static void MagnitudeMin(ref Vector2 left, ref Vector2 right, out Vector2 result) {
            result = left.LengthSquared < right.LengthSquared ? left : right;
        }

        public static Vector2 MagnitudeMax(Vector2 left, Vector2 right) {
            return left.LengthSquared >= right.LengthSquared ? left : right;
        }

        public static void MagnitudeMax(ref Vector2 left, ref Vector2 right, out Vector2 result) {
            result = left.LengthSquared >= right.LengthSquared ? left : right;
        }

        [System.Obsolete("Use MagnitudeMin() instead.")]
        public static Vector2 Min(Vector2 left, Vector2 right) {
            return left.LengthSquared < right.LengthSquared ? left : right;
        }

        [System.Obsolete("Use MagnitudeMax() instead.")]
        public static Vector2 Max(Vector2 left, Vector2 right) {
            return left.LengthSquared >= right.LengthSquared ? left : right;
        }

        public static Vector2 Clamp(Vector2 vec, Vector2 min, Vector2 max) {
            vec.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
            vec.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
            return vec;
        }

        public static void Clamp(ref Vector2 vec, ref Vector2 min, ref Vector2 max, out Vector2 result) {
            result.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
            result.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
        }

        public static float Distance(Vector2 vec1, Vector2 vec2) {
            float result;
            Distance(ref vec1, ref vec2, out result);
            return result;
        }

        public static void Distance(ref Vector2 vec1, ref Vector2 vec2, out float result) {
            result = (float)System.Math.Sqrt((vec2.X - vec1.X) * (vec2.X - vec1.X) + (vec2.Y - vec1.Y) * (vec2.Y - vec1.Y));
        }

        public static float DistanceSquared(Vector2 vec1, Vector2 vec2) {
            float result;
            DistanceSquared(ref vec1, ref vec2, out result);
            return result;
        }

        public static void DistanceSquared(ref Vector2 vec1, ref Vector2 vec2, out float result) {
            result = (vec2.X - vec1.X) * (vec2.X - vec1.X) + (vec2.Y - vec1.Y) * (vec2.Y - vec1.Y);
        }

        public static Vector2 Normalize(Vector2 vec) {
            float scale = 1.0f / vec.Length;
            vec.X *= scale;
            vec.Y *= scale;
            return vec;
        }

        public static void Normalize(ref Vector2 vec, out Vector2 result) {
            float scale = 1.0f / vec.Length;
            result.X = vec.X * scale;
            result.Y = vec.Y * scale;
        }

        public static Vector2 NormalizeFast(Vector2 vec) {
            float scale = BusEngine.Vector3.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y);
            vec.X *= scale;
            vec.Y *= scale;
            return vec;
        }

        public static void NormalizeFast(ref Vector2 vec, out Vector2 result) {
            float scale = BusEngine.Vector3.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y);
            result.X = vec.X * scale;
            result.Y = vec.Y * scale;
        }

        public static float Dot(Vector2 left, Vector2 right) {
            return left.X * right.X + left.Y * right.Y;
        }

        public static void Dot(ref Vector2 left, ref Vector2 right, out float result) {
            result = left.X * right.X + left.Y * right.Y;
        }

        public static float PerpDot(Vector2 left, Vector2 right) {
            return left.X * right.Y - left.Y * right.X;
        }

        public static void PerpDot(ref Vector2 left, ref Vector2 right, out float result) {
            result = left.X * right.Y - left.Y * right.X;
        }

        public static Vector2 Lerp(Vector2 a, Vector2 b, float blend) {
            a.X = blend * (b.X - a.X) + a.X;
            a.Y = blend * (b.Y - a.Y) + a.Y;
            return a;
        }

        public static void Lerp(ref Vector2 a, ref Vector2 b, float blend, out Vector2 result) {
            result.X = blend * (b.X - a.X) + a.X;
            result.Y = blend * (b.Y - a.Y) + a.Y;
        }

        public static Vector2 BaryCentric(Vector2 a, Vector2 b, Vector2 c, float u, float v) {
            return a + u * (b - a) + v * (c - a);
        }

        public static void BaryCentric(ref Vector2 a, ref Vector2 b, ref Vector2 c, float u, float v, out Vector2 result) {
            result = a; // copy

            Vector2 temp = b; // copy
            Subtract(ref temp, ref a, out temp);
            Multiply(ref temp, u, out temp);
            Add(ref result, ref temp, out result);

            temp = c; // copy
            Subtract(ref temp, ref a, out temp);
            Multiply(ref temp, v, out temp);
            Add(ref result, ref temp, out result);
        }

        public static Vector2 Transform(Vector2 vec, BusEngine.Quaternion quat) {
            Vector2 result;
            Transform(ref vec, ref quat, out result);
            return result;
        }

        public static void Transform(ref Vector2 vec, ref BusEngine.Quaternion quat, out Vector2 result) {
            BusEngine.Quaternion v = new BusEngine.Quaternion(vec.X, vec.Y, 0, 0), i, t;
            BusEngine.Quaternion.Invert(ref quat, out i);
            BusEngine.Quaternion.Multiply(ref quat, ref v, out t);
            BusEngine.Quaternion.Multiply(ref t, ref i, out v);

            result.X = v.X;
            result.Y = v.Y;
        }

        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Yx { get { return new Vector2(Y, X); } set { Y = value.X; X = value.Y; } }

        public static Vector2 operator +(Vector2 left, Vector2 right) {
            left.X += right.X;
            left.Y += right.Y;
            return left;
        }

        public static Vector2 operator -(Vector2 left, Vector2 right) {
            left.X -= right.X;
            left.Y -= right.Y;
            return left;
        }

        public static Vector2 operator -(Vector2 vec) {
            vec.X = -vec.X;
            vec.Y = -vec.Y;
            return vec;
        }

        public static Vector2 operator *(Vector2 vec, float scale) {
            vec.X *= scale;
            vec.Y *= scale;
            return vec;
        }

        public static Vector2 operator *(float scale, Vector2 vec) {
            vec.X *= scale;
            vec.Y *= scale;
            return vec;
        }

        public static Vector2 operator *(Vector2 vec, Vector2 scale) {
            vec.X *= scale.X;
            vec.Y *= scale.Y;
            return vec;
        }

        public static Vector2 operator /(Vector2 vec, float scale) {
            vec.X /= scale;
            vec.Y /= scale;
            return vec;
        }

        public static bool operator ==(Vector2 left, Vector2 right) {
            return left.Equals(right);
        }

        public static bool operator !=(Vector2 left, Vector2 right) {
            return !left.Equals(right);
        }

        private static string listSeparator = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;

        public override string ToString() {
            return System.String.Format("({0}{2} {1})", X, Y, listSeparator);
        }

        public override int GetHashCode() {
            unchecked {
                return (this.X.GetHashCode() * 397) ^ this.Y.GetHashCode();
            }
        }

        public override bool Equals(object obj) {
            if (!(obj is Vector2)) {
                return false;
            }
            return this.Equals((Vector2)obj);
        }

        public bool Equals(Vector2 other) {
            return X == other.X && Y == other.Y;
        }
    }

    [System.Serializable]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Vector3 : System.IEquatable<Vector3> {
        internal static float InverseSqrtFast(float x) {
            unsafe {
                float xhalf = 0.5f * x;
                int i = *(int*)&x;              // Read bits as integer.
                i = 0x5f375a86 - (i >> 1);      // Make an initial guess for Newton-Raphson approximation
                x = *(float*)&i;                // Convert bits back to float
                x = x * (1.5f - xhalf * x * x); // Perform left single Newton-Raphson step.
                return x;
            }
        }

        public float X;
        public float Y;
        public float Z;

        public Vector3(float value) {
            X = value;
            Y = value;
            Z = value;
        }

        public Vector3(float x, float y, float z) {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3(Vector2 v) {
            X = v.X;
            Y = v.Y;
            Z = 0.0f;
        }

        public Vector3(Vector3 v) {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public Vector3(Vector4 v) {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
        }

        public float this[int index] {
            get {
                if (index == 0) {
                    return X;
                } else if (index == 1) {
                    return Y;
                } else if (index == 2) {
                    return Z;
                }
                throw new System.IndexOutOfRangeException("You tried to access this vector at index: " + index);
            } set {
                if (index == 0) {
                    X = value;
                } else if (index == 1) {
                    Y = value;
                } else if (index == 2) {
                    Z = value;
                } else {
                    throw new System.IndexOutOfRangeException("You tried to set this vector at index: " + index);
                }
            }
        }

        public float Length {
            get {
                return (float)System.Math.Sqrt(X * X + Y * Y + Z * Z);
            }
        }

        public float LengthFast {
            get {
                return 1.0f / BusEngine.Vector3.InverseSqrtFast(X * X + Y * Y + Z * Z);
            }
        }

        public float LengthSquared {
            get {
                return X * X + Y * Y + Z * Z;
            }
        }

        public Vector3 Normalized() {
            Vector3 v = this;
            v.Normalize();
            return v;
        }

        public void Normalize() {
            float scale = 1.0f / this.Length;
            X *= scale;
            Y *= scale;
            Z *= scale;
        }

        public void NormalizeFast() {
            float scale = BusEngine.Vector3.InverseSqrtFast(X * X + Y * Y + Z * Z);
            X *= scale;
            Y *= scale;
            Z *= scale;
        }

        public static readonly Vector3 UnitX = new Vector3(1, 0, 0);
        public static readonly Vector3 UnitY = new Vector3(0, 1, 0);
        public static readonly Vector3 UnitZ = new Vector3(0, 0, 1);
        public static readonly Vector3 Zero = new Vector3(0, 0, 0);
        public static readonly Vector3 One = new Vector3(1, 1, 1);
        public static readonly int SizeInBytes = System.Runtime.InteropServices.Marshal.SizeOf(new Vector3());

        public static Vector3 Add(Vector3 a, Vector3 b) {
            Add(ref a, ref b, out a);
            return a;
        }

        public static void Add(ref Vector3 a, ref Vector3 b, out Vector3 result) {
            result.X = a.X + b.X;
            result.Y = a.Y + b.Y;
            result.Z = a.Z + b.Z;
        }

        public static Vector3 Subtract(Vector3 a, Vector3 b) {
            Subtract(ref a, ref b, out a);
            return a;
        }

        public static void Subtract(ref Vector3 a, ref Vector3 b, out Vector3 result) {
            result.X = a.X - b.X;
            result.Y = a.Y - b.Y;
            result.Z = a.Z - b.Z;
        }

        public static Vector3 Multiply(Vector3 vector, float scale) {
            Multiply(ref vector, scale, out vector);
            return vector;
        }

        public static void Multiply(ref Vector3 vector, float scale, out Vector3 result) {
            result.X = vector.X * scale;
            result.Y = vector.Y * scale;
            result.Z = vector.Z * scale;
        }

        public static Vector3 Multiply(Vector3 vector, Vector3 scale) {
            Multiply(ref vector, ref scale, out vector);
            return vector;
        }

        public static void Multiply(ref Vector3 vector, ref Vector3 scale, out Vector3 result) {
            result.X = vector.X * scale.X;
            result.Y = vector.Y * scale.Y;
            result.Z = vector.Z * scale.Z;
        }

        public static Vector3 Divide(Vector3 vector, float scale) {
            Divide(ref vector, scale, out vector);
            return vector;
        }

        public static void Divide(ref Vector3 vector, float scale, out Vector3 result) {
            result.X = vector.X / scale;
            result.Y = vector.Y / scale;
            result.Z = vector.Z / scale;
        }

        public static Vector3 Divide(Vector3 vector, Vector3 scale) {
            Divide(ref vector, ref scale, out vector);
            return vector;
        }

        public static void Divide(ref Vector3 vector, ref Vector3 scale, out Vector3 result) {
            result.X = vector.X / scale.X;
            result.Y = vector.Y / scale.Y;
            result.Z = vector.Z / scale.Z;
        }

        public static Vector3 ComponentMin(Vector3 a, Vector3 b) {
            a.X = a.X < b.X ? a.X : b.X;
            a.Y = a.Y < b.Y ? a.Y : b.Y;
            a.Z = a.Z < b.Z ? a.Z : b.Z;
            return a;
        }

        public static void ComponentMin(ref Vector3 a, ref Vector3 b, out Vector3 result) {
            result.X = a.X < b.X ? a.X : b.X;
            result.Y = a.Y < b.Y ? a.Y : b.Y;
            result.Z = a.Z < b.Z ? a.Z : b.Z;
        }

        public static Vector3 ComponentMax(Vector3 a, Vector3 b) {
            a.X = a.X > b.X ? a.X : b.X;
            a.Y = a.Y > b.Y ? a.Y : b.Y;
            a.Z = a.Z > b.Z ? a.Z : b.Z;
            return a;
        }

        public static void ComponentMax(ref Vector3 a, ref Vector3 b, out Vector3 result) {
            result.X = a.X > b.X ? a.X : b.X;
            result.Y = a.Y > b.Y ? a.Y : b.Y;
            result.Z = a.Z > b.Z ? a.Z : b.Z;
        }

        public static Vector3 MagnitudeMin(Vector3 left, Vector3 right) {
            return left.LengthSquared < right.LengthSquared ? left : right;
        }

        public static void MagnitudeMin(ref Vector3 left, ref Vector3 right, out Vector3 result) {
            result = left.LengthSquared < right.LengthSquared ? left : right;
        }

        public static Vector3 MagnitudeMax(Vector3 left, Vector3 right) {
            return left.LengthSquared >= right.LengthSquared ? left : right;
        }

        public static void MagnitudeMax(ref Vector3 left, ref Vector3 right, out Vector3 result) {
            result = left.LengthSquared >= right.LengthSquared ? left : right;
        }

        [System.Obsolete("Use MagnitudeMin() instead.")]
        public static Vector3 Min(Vector3 left, Vector3 right) {
            return left.LengthSquared < right.LengthSquared ? left : right;
        }

        [System.Obsolete("Use MagnitudeMax() instead.")]
        public static Vector3 Max(Vector3 left, Vector3 right) {
            return left.LengthSquared >= right.LengthSquared ? left : right;
        }

        public static Vector3 Clamp(Vector3 vec, Vector3 min, Vector3 max) {
            vec.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
            vec.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
            vec.Z = vec.Z < min.Z ? min.Z : vec.Z > max.Z ? max.Z : vec.Z;
            return vec;
        }

        public static void Clamp(ref Vector3 vec, ref Vector3 min, ref Vector3 max, out Vector3 result) {
            result.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
            result.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
            result.Z = vec.Z < min.Z ? min.Z : vec.Z > max.Z ? max.Z : vec.Z;
        }

        public static float Distance(Vector3 vec1, Vector3 vec2) {
            float result;
            Distance(ref vec1, ref vec2, out result);
            return result;
        }

        public static void Distance(ref Vector3 vec1, ref Vector3 vec2, out float result) {
            result = (float)System.Math.Sqrt((vec2.X - vec1.X) * (vec2.X - vec1.X) + (vec2.Y - vec1.Y) * (vec2.Y - vec1.Y) + (vec2.Z - vec1.Z) * (vec2.Z - vec1.Z));
        }

        public static float DistanceSquared(Vector3 vec1, Vector3 vec2) {
            float result;
            DistanceSquared(ref vec1, ref vec2, out result);
            return result;
        }

        public static void DistanceSquared(ref Vector3 vec1, ref Vector3 vec2, out float result) {
            result = (vec2.X - vec1.X) * (vec2.X - vec1.X) + (vec2.Y - vec1.Y) * (vec2.Y - vec1.Y) + (vec2.Z - vec1.Z) * (vec2.Z - vec1.Z);
        }

        public static Vector3 Normalize(Vector3 vec) {
            float scale = 1.0f / vec.Length;
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            return vec;
        }

        public static void Normalize(ref Vector3 vec, out Vector3 result) {
            float scale = 1.0f / vec.Length;
            result.X = vec.X * scale;
            result.Y = vec.Y * scale;
            result.Z = vec.Z * scale;
        }

        public static Vector3 NormalizeFast(Vector3 vec) {
            float scale = BusEngine.Vector3.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            return vec;
        }

        public static void NormalizeFast(ref Vector3 vec, out Vector3 result) {
            float scale = BusEngine.Vector3.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z);
            result.X = vec.X * scale;
            result.Y = vec.Y * scale;
            result.Z = vec.Z * scale;
        }

        public static float Dot(Vector3 left, Vector3 right) {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z;
        }

        public static void Dot(ref Vector3 left, ref Vector3 right, out float result) {
            result = left.X * right.X + left.Y * right.Y + left.Z * right.Z;
        }

        public static Vector3 Cross(Vector3 left, Vector3 right) {
            Vector3 result;
            Cross(ref left, ref right, out result);
            return result;
        }

        public static void Cross(ref Vector3 left, ref Vector3 right, out Vector3 result) {
            result.X = left.Y * right.Z - left.Z * right.Y;
            result.Y = left.Z * right.X - left.X * right.Z;
            result.Z = left.X * right.Y - left.Y * right.X;
        }

        public static Vector3 Lerp(Vector3 a, Vector3 b, float blend) {
            a.X = blend * (b.X - a.X) + a.X;
            a.Y = blend * (b.Y - a.Y) + a.Y;
            a.Z = blend * (b.Z - a.Z) + a.Z;
            return a;
        }

        public static void Lerp(ref Vector3 a, ref Vector3 b, float blend, out Vector3 result) {
            result.X = blend * (b.X - a.X) + a.X;
            result.Y = blend * (b.Y - a.Y) + a.Y;
            result.Z = blend * (b.Z - a.Z) + a.Z;
        }

        public static Vector3 BaryCentric(Vector3 a, Vector3 b, Vector3 c, float u, float v) {
            return a + u * (b - a) + v * (c - a);
        }

        public static void BaryCentric(ref Vector3 a, ref Vector3 b, ref Vector3 c, float u, float v, out Vector3 result) {
            result = a; // copy

            Vector3 temp = b; // copy
            Subtract(ref temp, ref a, out temp);
            Multiply(ref temp, u, out temp);
            Add(ref result, ref temp, out result);

            temp = c; // copy
            Subtract(ref temp, ref a, out temp);
            Multiply(ref temp, v, out temp);
            Add(ref result, ref temp, out result);
        }

        /* public static Vector3 TransformVector(Vector3 vec, Matrix4 mat) {
            Vector3 result;
            TransformVector(ref vec, ref mat, out result);
            return result;
        }

        public static void TransformVector(ref Vector3 vec, ref Matrix4 mat, out Vector3 result) {
            result.X = vec.X * mat.Row0.X + vec.Y * mat.Row1.X + vec.Z * mat.Row2.X;
            result.Y = vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y + vec.Z * mat.Row2.Y;
            result.Z = vec.X * mat.Row0.Z + vec.Y * mat.Row1.Z + vec.Z * mat.Row2.Z;
        }

        public static Vector3 TransformNormal(Vector3 norm, Matrix4 mat) {
            Vector3 result;
            TransformNormal(ref norm, ref mat, out result);
            return result;
        }

        public static void TransformNormal(ref Vector3 norm, ref Matrix4 mat, out Vector3 result) {
            Matrix4 Inverse = Matrix4.Invert(mat);
            Vector3.TransformNormalInverse(ref norm, ref Inverse, out result);
        }

        public static Vector3 TransformNormalInverse(Vector3 norm, Matrix4 invMat) {
            Vector3 result;
            TransformNormalInverse(ref norm, ref invMat, out result);
            return result;
        }

        public static void TransformNormalInverse(ref Vector3 norm, ref Matrix4 invMat, out Vector3 result) {
            result.X = norm.X * invMat.Row0.X + norm.Y * invMat.Row0.Y + norm.Z * invMat.Row0.Z;
            result.Y = norm.X * invMat.Row1.X + norm.Y * invMat.Row1.Y + norm.Z * invMat.Row1.Z;
            result.Z = norm.X * invMat.Row2.X + norm.Y * invMat.Row2.Y + norm.Z * invMat.Row2.Z;
        }

        public static Vector3 TransformPosition(Vector3 pos, Matrix4 mat) {
            Vector3 result;
            TransformPosition(ref pos, ref mat, out result);
            return result;
        }

        public static void TransformPosition(ref Vector3 pos, ref Matrix4 mat, out Vector3 result) {
            result.X = pos.X * mat.Row0.X + pos.Y * mat.Row1.X + pos.Z * mat.Row2.X + mat.Row3.X;
            result.Y = pos.X * mat.Row0.Y + pos.Y * mat.Row1.Y + pos.Z * mat.Row2.Y + mat.Row3.Y;
            result.Z = pos.X * mat.Row0.Z + pos.Y * mat.Row1.Z + pos.Z * mat.Row2.Z + mat.Row3.Z;
        }

        public static Vector3 Transform(Vector3 vec, Matrix3 mat) {
            Vector3 result;
            Transform(ref vec, ref mat, out result);
            return result;
        }

        public static void Transform(ref Vector3 vec, ref Matrix3 mat, out Vector3 result) {
            result.X = vec.X * mat.Row0.X + vec.Y * mat.Row1.X + vec.Z * mat.Row2.X;
            result.Y = vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y + vec.Z * mat.Row2.Y;
            result.Z = vec.X * mat.Row0.Z + vec.Y * mat.Row1.Z + vec.Z * mat.Row2.Z;
        }

        public static Vector3 Transform(Vector3 vec, BusEngine.Quaternion quat) {
            Vector3 result;
            Transform(ref vec, ref quat, out result);
            return result;
        }

        public static void Transform(ref Vector3 vec, ref BusEngine.Quaternion quat, out Vector3 result) {
            // Since vec.W == 0, we can optimize quat * vec * quat^-1 as follows:
            // vec + 2.0 * cross(quat.xyz, cross(quat.xyz, vec) + quat.w * vec)
            Vector3 xyz = quat.Xyz, temp, temp2;
            Cross(ref xyz, ref vec, out temp);
            Multiply(ref vec, quat.W, out temp2);
            Add(ref temp, ref temp2, out temp);
            Cross(ref xyz, ref temp, out temp2);
            Multiply(ref temp2, 2f, out temp2);
            Add(ref vec, ref temp2, out result);
        }

        [System.Obsolete("This function erroneously does a vector * matrix multiplication instead of the intended matrix * vector multiplication. Use TransformColumn() if proper right-handed multiplication is wanted, or TransformRow() for the existing behavior.")]
        public static Vector3 Transform(Matrix3 mat, Vector3 vec) {
			Vector3 result;
            Transform(ref vec, ref mat, out result);
            return result;
        }

        [System.Obsolete("Use TransformColumn() instead.")]
        public static void Transform(ref Matrix3 mat, ref Vector3 vec, out Vector3 result) {
            TransformColumn(ref mat, ref vec, out result);
        }

        public static Vector3 TransformColumn(Matrix3 mat, Vector3 vec) {
			Vector3 result;
            TransformColumn(ref mat, ref vec, out result);
            return result;
        }

        public static void TransformColumn(ref Matrix3 mat, ref Vector3 vec, out Vector3 result) {
            result.X = mat.Row0.X * vec.X + mat.Row0.Y * vec.Y + mat.Row0.Z * vec.Z;
            result.Y = mat.Row1.X * vec.X + mat.Row1.Y * vec.Y + mat.Row1.Z * vec.Z;
            result.Z = mat.Row2.X * vec.X + mat.Row2.Y * vec.Y + mat.Row2.Z * vec.Z;
        }

        public static Vector3 TransformRow(Vector3 vec, Matrix3 mat) {
			Vector3 result;
            TransformColumn(ref mat, ref vec, out result);
            return result;
        }

        public static void TransformRow(ref Vector3 vec, ref Matrix3 mat, out Vector3 result) {
            result.X = vec.X * mat.Row0.X + vec.Y * mat.Row1.X + vec.Z * mat.Row2.X;
            result.Y = vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y + vec.Z * mat.Row2.Y;
            result.Z = vec.X * mat.Row0.Z + vec.Y * mat.Row1.Z + vec.Z * mat.Row2.Z;
        }

        public static Vector3 TransformPerspective(Vector3 vec, Matrix4 mat) {
            Vector3 result;
            TransformPerspective(ref vec, ref mat, out result);
            return result;
        }

        public static void TransformPerspective(ref Vector3 vec, ref Matrix4 mat, out Vector3 result) {
            Vector4 v = new Vector4(vec.X, vec.Y, vec.Z, 1);
            Vector4.Transform(ref v, ref mat, out v);
            result.X = v.X / v.W;
            result.Y = v.Y / v.W;
            result.Z = v.Z / v.W;
        }

        public static float CalculateAngle(Vector3 first, Vector3 second) {
            float result;
            CalculateAngle(ref first, ref second, out result);
            return result;
        }

        public static void CalculateAngle(ref Vector3 first, ref Vector3 second, out float result) {
            float temp;
            Vector3.Dot(ref first, ref second, out temp);
            result = (float)System.Math.Acos(MathHelper.Clamp(temp / (first.Length * second.Length), -1.0, 1.0));
        }

        public static Vector3 Project(Vector3 vector, float x, float y, float width, float height, float minZ, float maxZ, Matrix4 worldViewProjection) {
            Vector4 result;
            result.X = vector.X * worldViewProjection.M11 + vector.Y * worldViewProjection.M21 + vector.Z * worldViewProjection.M31 + worldViewProjection.M41;
            result.Y = vector.X * worldViewProjection.M12 + vector.Y * worldViewProjection.M22 + vector.Z * worldViewProjection.M32 + worldViewProjection.M42;
            result.Z = vector.X * worldViewProjection.M13 + vector.Y * worldViewProjection.M23 + vector.Z * worldViewProjection.M33 + worldViewProjection.M43;
            result.W = vector.X * worldViewProjection.M14 + vector.Y * worldViewProjection.M24 + vector.Z * worldViewProjection.M34 + worldViewProjection.M44;

            result /= result.W;

            result.X = x + (width * ((result.X + 1.0f) / 2.0f));
            result.Y = y + (height * ((result.Y + 1.0f) / 2.0f));
            result.Z = minZ + ((maxZ - minZ) * ((result.Z + 1.0f) / 2.0f));

            return new Vector3(result.X, result.Y, result.Z);
        }

        public static Vector3 Unproject(Vector3 vector, float x, float y, float width, float height, float minZ, float maxZ, Matrix4 inverseWorldViewProjection) {
            float X = (vector.X - x) / width * 2.0f - 1.0f;
            float Y = (vector.Y - y) / height * 2.0f - 1.0f;
            float Z = (vector.Z / (maxZ - minZ)) * 2.0f - 1.0f;

            Vector3 result;
            result.X = X * inverseWorldViewProjection.M11 + Y * inverseWorldViewProjection.M21 + Z * inverseWorldViewProjection.M31 + inverseWorldViewProjection.M41;
            result.Y = X * inverseWorldViewProjection.M12 + Y * inverseWorldViewProjection.M22 + Z * inverseWorldViewProjection.M32 + inverseWorldViewProjection.M42;
            result.Z = X * inverseWorldViewProjection.M13 + Y * inverseWorldViewProjection.M23 + Z * inverseWorldViewProjection.M33 + inverseWorldViewProjection.M43;
            float W = X * inverseWorldViewProjection.M14 + Y * inverseWorldViewProjection.M24 + Z * inverseWorldViewProjection.M34 + inverseWorldViewProjection.M44;

            result /= W;

            return result;
        } */

        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Xy { get { return new Vector2(X, Y); } set { X = value.X; Y = value.Y; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Xz { get { return new Vector2(X, Z); } set { X = value.X; Z = value.Y; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Yx { get { return new Vector2(Y, X); } set { Y = value.X; X = value.Y; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Yz { get { return new Vector2(Y, Z); } set { Y = value.X; Z = value.Y; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Zx { get { return new Vector2(Z, X); } set { Z = value.X; X = value.Y; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Zy { get { return new Vector2(Z, Y); } set { Z = value.X; Y = value.Y; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Xzy { get { return new Vector3(X, Z, Y); } set { X = value.X; Z = value.Y; Y = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Yxz { get { return new Vector3(Y, X, Z); } set { Y = value.X; X = value.Y; Z = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Yzx { get { return new Vector3(Y, Z, X); } set { Y = value.X; Z = value.Y; X = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Zxy { get { return new Vector3(Z, X, Y); } set { Z = value.X; X = value.Y; Y = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Zyx { get { return new Vector3(Z, Y, X); } set { Z = value.X; Y = value.Y; X = value.Z; } }

        public static Vector3 operator +(Vector3 left, Vector3 right) {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            return left;
        }

        public static Vector3 operator -(Vector3 left, Vector3 right) {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            return left;
        }

        public static Vector3 operator -(Vector3 vec) {
            vec.X = -vec.X;
            vec.Y = -vec.Y;
            vec.Z = -vec.Z;
            return vec;
        }

        public static Vector3 operator *(Vector3 vec, float scale) {
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            return vec;
        }

        public static Vector3 operator *(float scale, Vector3 vec) {
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            return vec;
        }

        public static Vector3 operator *(Vector3 vec, Vector3 scale) {
            vec.X *= scale.X;
            vec.Y *= scale.Y;
            vec.Z *= scale.Z;
            return vec;
        }

        /* public static Vector3 operator *(Vector3 vec, Matrix3 mat) {
            Vector3 result;
            Vector3.TransformRow(ref vec, ref mat, out result);
            return result;
        }

        public static Vector3 operator *(Matrix3 mat, Vector3 vec) {
            Vector3 result;
            Vector3.TransformColumn(ref mat, ref vec, out result);
            return result;
        }

        public static Vector3 operator *(BusEngine.Quaternion quat, Vector3 vec) {
            Vector3 result;
            Vector3.Transform(ref vec, ref quat, out result);
            return result;
        } */

        public static Vector3 operator /(Vector3 vec, float scale) {
            vec.X /= scale;
            vec.Y /= scale;
            vec.Z /= scale;
            return vec;
        }

        public static bool operator ==(Vector3 left, Vector3 right) {
            return left.Equals(right);
        }

        public static bool operator !=(Vector3 left, Vector3 right) {
            return !left.Equals(right);
        }

        private static string listSeparator = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;

        public override string ToString() {
            return System.String.Format("({0}{3} {1}{3} {2})", X, Y, Z, listSeparator);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = this.X.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Y.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Z.GetHashCode();
                return hashCode;
            }
        }

        public override bool Equals(object obj) {
            if (!(obj is Vector3)) {
                return false;
            }
            return this.Equals((Vector3)obj);
        }

        public bool Equals(Vector3 other) {
            return X == other.X && Y == other.Y && Z == other.Z;
        }
	}

    [System.Serializable]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
    public struct Vector4 : System.IEquatable<Vector4> {
        public float X;
        public float Y;
        public float Z;
        public float W;
        public static readonly Vector4 UnitX = new Vector4(1, 0, 0, 0);
        public static readonly Vector4 UnitY = new Vector4(0, 1, 0, 0);
        public static readonly Vector4 UnitZ = new Vector4(0, 0, 1, 0);
        public static readonly Vector4 UnitW = new Vector4(0, 0, 0, 1);
        public static readonly Vector4 Zero = new Vector4(0, 0, 0, 0);
        public static readonly Vector4 One = new Vector4(1, 1, 1, 1);
        public static readonly int SizeInBytes = System.Runtime.InteropServices.Marshal.SizeOf(new Vector4());

        public Vector4(float value) {
            X = value;
            Y = value;
            Z = value;
            W = value;
        }

        public Vector4(float x, float y, float z, float w) {
            X = x;
            Y = y;
            Z = z;
            W = w;
        }

        public Vector4(Vector2 v) {
            X = v.X;
            Y = v.Y;
            Z = 0.0f;
            W = 0.0f;
        }

        public Vector4(Vector3 v) {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = 0.0f;
        }

        public Vector4(Vector3 v, float w) {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = w;
        }

        public Vector4(Vector4 v) {
            X = v.X;
            Y = v.Y;
            Z = v.Z;
            W = v.W;
        }

        public float this[int index] {
            get {
                if (index == 0) {
                    return X;
                } else if (index == 1) {
                    return Y;
                } else if (index == 2) {
                    return Z;
                } else if (index == 3) {
                    return W;
                }
                throw new System.IndexOutOfRangeException("You tried to access this vector at index: " + index);
            } set {
                if (index == 0) {
                    X = value;
                } else if (index == 1) {
                    Y = value;
                } else if (index == 2) {
                    Z = value;
                } else if (index == 3) {
                    W = value;
                } else {
                    throw new System.IndexOutOfRangeException("You tried to set this vector at index: " + index);
                }
            }
        }

        public float Length {
            get {
                return (float)System.Math.Sqrt(X * X + Y * Y + Z * Z + W * W);
            }
        }

        public float LengthFast {
            get {
                return 1.0f / BusEngine.Vector3.InverseSqrtFast(X * X + Y * Y + Z * Z + W * W);
            }
        }

        public float LengthSquared {
            get {
                return X * X + Y * Y + Z * Z + W * W;
            }
        }

        public Vector4 Normalized() {
            Vector4 v = this;
            v.Normalize();
            return v;
        }

        public void Normalize() {
            float scale = 1.0f / this.Length;
            X *= scale;
            Y *= scale;
            Z *= scale;
            W *= scale;
        }

        public void NormalizeFast() {
            float scale = BusEngine.Vector3.InverseSqrtFast(X * X + Y * Y + Z * Z + W * W);
            X *= scale;
            Y *= scale;
            Z *= scale;
            W *= scale;
        }

        public static Vector4 Add(Vector4 a, Vector4 b) {
            Add(ref a, ref b, out a);
            return a;
        }

        public static void Add(ref Vector4 a, ref Vector4 b, out Vector4 result) {
            result.X = a.X + b.X;
            result.Y = a.Y + b.Y;
            result.Z = a.Z + b.Z;
            result.W = a.W + b.W;
        }

        public static Vector4 Subtract(Vector4 a, Vector4 b) {
            Subtract(ref a, ref b, out a);
            return a;
        }

        public static void Subtract(ref Vector4 a, ref Vector4 b, out Vector4 result) {
            result.X = a.X - b.X;
            result.Y = a.Y - b.Y;
            result.Z = a.Z - b.Z;
            result.W = a.W - b.W;
        }

        public static Vector4 Multiply(Vector4 vector, float scale) {
            Multiply(ref vector, scale, out vector);
            return vector;
        }

        public static void Multiply(ref Vector4 vector, float scale, out Vector4 result) {
            result.X = vector.X * scale;
            result.Y = vector.Y * scale;
            result.Z = vector.Z * scale;
            result.W = vector.W * scale;
        }

        public static Vector4 Multiply(Vector4 vector, Vector4 scale) {
            Multiply(ref vector, ref scale, out vector);
            return vector;
        }

        public static void Multiply(ref Vector4 vector, ref Vector4 scale, out Vector4 result) {
            result.X = vector.X * scale.X;
            result.Y = vector.Y * scale.Y;
            result.Z = vector.Z * scale.Z;
            result.W = vector.W * scale.W;
        }

        public static Vector4 Divide(Vector4 vector, float scale) {
            Divide(ref vector, scale, out vector);
            return vector;
        }

        public static void Divide(ref Vector4 vector, float scale, out Vector4 result) {
            result.X = vector.X / scale;
            result.Y = vector.Y / scale;
            result.Z = vector.Z / scale;
            result.W = vector.W / scale;
        }

        public static Vector4 Divide(Vector4 vector, Vector4 scale) {
            Divide(ref vector, ref scale, out vector);
            return vector;
        }

        public static void Divide(ref Vector4 vector, ref Vector4 scale, out Vector4 result) {
            result.X = vector.X / scale.X;
            result.Y = vector.Y / scale.Y;
            result.Z = vector.Z / scale.Z;
            result.W = vector.W / scale.W;
        }

        [System.Obsolete("Use ComponentMin() instead.")]
        public static Vector4 Min(Vector4 a, Vector4 b) {
            a.X = a.X < b.X ? a.X : b.X;
            a.Y = a.Y < b.Y ? a.Y : b.Y;
            a.Z = a.Z < b.Z ? a.Z : b.Z;
            a.W = a.W < b.W ? a.W : b.W;
            return a;
        }

        [System.Obsolete("Use ComponentMin() instead.")]
        public static void Min(ref Vector4 a, ref Vector4 b, out Vector4 result) {
            result.X = a.X < b.X ? a.X : b.X;
            result.Y = a.Y < b.Y ? a.Y : b.Y;
            result.Z = a.Z < b.Z ? a.Z : b.Z;
            result.W = a.W < b.W ? a.W : b.W;
        }

        [System.Obsolete("Use ComponentMax() instead.")]
        public static Vector4 Max(Vector4 a, Vector4 b) {
            a.X = a.X > b.X ? a.X : b.X;
            a.Y = a.Y > b.Y ? a.Y : b.Y;
            a.Z = a.Z > b.Z ? a.Z : b.Z;
            a.W = a.W > b.W ? a.W : b.W;
            return a;
        }

        [System.Obsolete("Use ComponentMax() instead.")]
        public static void Max(ref Vector4 a, ref Vector4 b, out Vector4 result) {
            result.X = a.X > b.X ? a.X : b.X;
            result.Y = a.Y > b.Y ? a.Y : b.Y;
            result.Z = a.Z > b.Z ? a.Z : b.Z;
            result.W = a.W > b.W ? a.W : b.W;
        }

        public static Vector4 ComponentMin(Vector4 a, Vector4 b) {
            a.X = a.X < b.X ? a.X : b.X;
            a.Y = a.Y < b.Y ? a.Y : b.Y;
            a.Z = a.Z < b.Z ? a.Z : b.Z;
            a.W = a.W < b.W ? a.W : b.W;
            return a;
        }

        public static void ComponentMin(ref Vector4 a, ref Vector4 b, out Vector4 result) {
            result.X = a.X < b.X ? a.X : b.X;
            result.Y = a.Y < b.Y ? a.Y : b.Y;
            result.Z = a.Z < b.Z ? a.Z : b.Z;
            result.W = a.W < b.W ? a.W : b.W;
        }

        public static Vector4 ComponentMax(Vector4 a, Vector4 b) {
            a.X = a.X > b.X ? a.X : b.X;
            a.Y = a.Y > b.Y ? a.Y : b.Y;
            a.Z = a.Z > b.Z ? a.Z : b.Z;
            a.W = a.W > b.W ? a.W : b.W;
            return a;
        }

        public static void ComponentMax(ref Vector4 a, ref Vector4 b, out Vector4 result) {
            result.X = a.X > b.X ? a.X : b.X;
            result.Y = a.Y > b.Y ? a.Y : b.Y;
            result.Z = a.Z > b.Z ? a.Z : b.Z;
            result.W = a.W > b.W ? a.W : b.W;
        }

        public static Vector4 MagnitudeMin(Vector4 left, Vector4 right) {
            return left.LengthSquared < right.LengthSquared ? left : right;
        }

        public static void MagnitudeMin(ref Vector4 left, ref Vector4 right, out Vector4 result) {
            result = left.LengthSquared < right.LengthSquared ? left : right;
        }

        public static Vector4 MagnitudeMax(Vector4 left, Vector4 right) {
            return left.LengthSquared >= right.LengthSquared ? left : right;
        }

        public static void MagnitudeMax(ref Vector4 left, ref Vector4 right, out Vector4 result) {
            result = left.LengthSquared >= right.LengthSquared ? left : right;
        }

        public static Vector4 Clamp(Vector4 vec, Vector4 min, Vector4 max) {
            vec.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
            vec.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
            vec.Z = vec.Z < min.Z ? min.Z : vec.Z > max.Z ? max.Z : vec.Z;
            vec.W = vec.W < min.W ? min.W : vec.W > max.W ? max.W : vec.W;
            return vec;
        }

        public static void Clamp(ref Vector4 vec, ref Vector4 min, ref Vector4 max, out Vector4 result) {
            result.X = vec.X < min.X ? min.X : vec.X > max.X ? max.X : vec.X;
            result.Y = vec.Y < min.Y ? min.Y : vec.Y > max.Y ? max.Y : vec.Y;
            result.Z = vec.Z < min.Z ? min.Z : vec.Z > max.Z ? max.Z : vec.Z;
            result.W = vec.W < min.W ? min.W : vec.W > max.W ? max.W : vec.W;
        }

        public static Vector4 Normalize(Vector4 vec) {
            float scale = 1.0f / vec.Length;
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            vec.W *= scale;
            return vec;
        }

        public static void Normalize(ref Vector4 vec, out Vector4 result) {
            float scale = 1.0f / vec.Length;
            result.X = vec.X * scale;
            result.Y = vec.Y * scale;
            result.Z = vec.Z * scale;
            result.W = vec.W * scale;
        }

        public static Vector4 NormalizeFast(Vector4 vec) {
            float scale = BusEngine.Vector3.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z + vec.W * vec.W);
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            vec.W *= scale;
            return vec;
        }

        public static void NormalizeFast(ref Vector4 vec, out Vector4 result) {
            float scale = BusEngine.Vector3.InverseSqrtFast(vec.X * vec.X + vec.Y * vec.Y + vec.Z * vec.Z + vec.W * vec.W);
            result.X = vec.X * scale;
            result.Y = vec.Y * scale;
            result.Z = vec.Z * scale;
            result.W = vec.W * scale;
        }

        public static float Dot(Vector4 left, Vector4 right) {
            return left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
        }

        public static void Dot(ref Vector4 left, ref Vector4 right, out float result) {
            result = left.X * right.X + left.Y * right.Y + left.Z * right.Z + left.W * right.W;
        }

        public static Vector4 Lerp(Vector4 a, Vector4 b, float blend) {
            a.X = blend * (b.X - a.X) + a.X;
            a.Y = blend * (b.Y - a.Y) + a.Y;
            a.Z = blend * (b.Z - a.Z) + a.Z;
            a.W = blend * (b.W - a.W) + a.W;
            return a;
        }

        public static void Lerp(ref Vector4 a, ref Vector4 b, float blend, out Vector4 result) {
            result.X = blend * (b.X - a.X) + a.X;
            result.Y = blend * (b.Y - a.Y) + a.Y;
            result.Z = blend * (b.Z - a.Z) + a.Z;
            result.W = blend * (b.W - a.W) + a.W;
        }

        public static Vector4 BaryCentric(Vector4 a, Vector4 b, Vector4 c, float u, float v) {
            return a + u * (b - a) + v * (c - a);
        }

        public static void BaryCentric(ref Vector4 a, ref Vector4 b, ref Vector4 c, float u, float v, out Vector4 result) {
            result = a; // copy

            Vector4 temp = b; // copy
            Subtract(ref temp, ref a, out temp);
            Multiply(ref temp, u, out temp);
            Add(ref result, ref temp, out result);

            temp = c; // copy
            Subtract(ref temp, ref a, out temp);
            Multiply(ref temp, v, out temp);
            Add(ref result, ref temp, out result);
        }

        /* public static Vector4 Transform(Vector4 vec, Matrix4 mat) {
            Vector4 result;
            Transform(ref vec, ref mat, out result);
            return result;
        }

        public static void Transform(ref Vector4 vec, ref Matrix4 mat, out Vector4 result) {
            result = new Vector4(
                vec.X * mat.Row0.X + vec.Y * mat.Row1.X + vec.Z * mat.Row2.X + vec.W * mat.Row3.X,
                vec.X * mat.Row0.Y + vec.Y * mat.Row1.Y + vec.Z * mat.Row2.Y + vec.W * mat.Row3.Y,
                vec.X * mat.Row0.Z + vec.Y * mat.Row1.Z + vec.Z * mat.Row2.Z + vec.W * mat.Row3.Z,
                vec.X * mat.Row0.W + vec.Y * mat.Row1.W + vec.Z * mat.Row2.W + vec.W * mat.Row3.W
			);
        } */

        public static Vector4 Transform(Vector4 vec, BusEngine.Quaternion quat) {
            Vector4 result;
            Transform(ref vec, ref quat, out result);
            return result;
        }

        public static void Transform(ref Vector4 vec, ref BusEngine.Quaternion quat, out Vector4 result) {
            BusEngine.Quaternion v = new BusEngine.Quaternion(vec.X, vec.Y, vec.Z, vec.W), i, t;
            BusEngine.Quaternion.Invert(ref quat, out i);
            BusEngine.Quaternion.Multiply(ref quat, ref v, out t);
            BusEngine.Quaternion.Multiply(ref t, ref i, out v);

            result.X = v.X;
            result.Y = v.Y;
            result.Z = v.Z;
            result.W = v.W;
        }

        /* public static Vector4 Transform(Matrix4 mat, Vector4 vec) {
            Vector4 result;
            Transform(ref mat, ref vec, out result);
            return result;
        }

        public static void Transform(ref Matrix4 mat, ref Vector4 vec, out Vector4 result) {
            result = new Vector4(
                mat.Row0.X * vec.X + mat.Row0.Y * vec.Y + mat.Row0.Z * vec.Z + mat.Row0.W * vec.W,
                mat.Row1.X * vec.X + mat.Row1.Y * vec.Y + mat.Row1.Z * vec.Z + mat.Row1.W * vec.W,
                mat.Row2.X * vec.X + mat.Row2.Y * vec.Y + mat.Row2.Z * vec.Z + mat.Row2.W * vec.W,
                mat.Row3.X * vec.X + mat.Row3.Y * vec.Y + mat.Row3.Z * vec.Z + mat.Row3.W * vec.W
			);
        } */

        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Xy { get { return new Vector2(X, Y); } set { X = value.X; Y = value.Y; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Xz { get { return new Vector2(X, Z); } set { X = value.X; Z = value.Y; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Xw { get { return new Vector2(X, W); } set { X = value.X; W = value.Y; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Yx { get { return new Vector2(Y, X); } set { Y = value.X; X = value.Y; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Yz { get { return new Vector2(Y, Z); } set { Y = value.X; Z = value.Y; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Yw { get { return new Vector2(Y, W); } set { Y = value.X; W = value.Y; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Zx { get { return new Vector2(Z, X); } set { Z = value.X; X = value.Y; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Zy { get { return new Vector2(Z, Y); } set { Z = value.X; Y = value.Y; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Zw { get { return new Vector2(Z, W); } set { Z = value.X; W = value.Y; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Wx { get { return new Vector2(W, X); } set { W = value.X; X = value.Y; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Wy { get { return new Vector2(W, Y); } set { W = value.X; Y = value.Y; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector2 Wz { get { return new Vector2(W, Z); } set { W = value.X; Z = value.Y; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Xyz { get { return new Vector3(X, Y, Z); } set { X = value.X; Y = value.Y; Z = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Xyw { get { return new Vector3(X, Y, W); } set { X = value.X; Y = value.Y; W = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Xzy { get { return new Vector3(X, Z, Y); } set { X = value.X; Z = value.Y; Y = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Xzw { get { return new Vector3(X, Z, W); } set { X = value.X; Z = value.Y; W = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Xwy { get { return new Vector3(X, W, Y); } set { X = value.X; W = value.Y; Y = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Xwz { get { return new Vector3(X, W, Z); } set { X = value.X; W = value.Y; Z = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Yxz { get { return new Vector3(Y, X, Z); } set { Y = value.X; X = value.Y; Z = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Yxw { get { return new Vector3(Y, X, W); } set { Y = value.X; X = value.Y; W = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Yzx { get { return new Vector3(Y, Z, X); } set { Y = value.X; Z = value.Y; X = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Yzw { get { return new Vector3(Y, Z, W); } set { Y = value.X; Z = value.Y; W = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Ywx { get { return new Vector3(Y, W, X); } set { Y = value.X; W = value.Y; X = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Ywz { get { return new Vector3(Y, W, Z); } set { Y = value.X; W = value.Y; Z = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Zxy { get { return new Vector3(Z, X, Y); } set { Z = value.X; X = value.Y; Y = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Zxw { get { return new Vector3(Z, X, W); } set { Z = value.X; X = value.Y; W = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Zyx { get { return new Vector3(Z, Y, X); } set { Z = value.X; Y = value.Y; X = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Zyw { get { return new Vector3(Z, Y, W); } set { Z = value.X; Y = value.Y; W = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Zwx { get { return new Vector3(Z, W, X); } set { Z = value.X; W = value.Y; X = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Zwy { get { return new Vector3(Z, W, Y); } set { Z = value.X; W = value.Y; Y = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Wxy { get { return new Vector3(W, X, Y); } set { W = value.X; X = value.Y; Y = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Wxz { get { return new Vector3(W, X, Z); } set { W = value.X; X = value.Y; Z = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Wyx { get { return new Vector3(W, Y, X); } set { W = value.X; Y = value.Y; X = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Wyz { get { return new Vector3(W, Y, Z); } set { W = value.X; Y = value.Y; Z = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Wzx { get { return new Vector3(W, Z, X); } set { W = value.X; Z = value.Y; X = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector3 Wzy { get { return new Vector3(W, Z, Y); } set { W = value.X; Z = value.Y; Y = value.Z; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Xywz { get { return new Vector4(X, Y, W, Z); } set { X = value.X; Y = value.Y; W = value.Z; Z = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Xzyw { get { return new Vector4(X, Z, Y, W); } set { X = value.X; Z = value.Y; Y = value.Z; W = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Xzwy { get { return new Vector4(X, Z, W, Y); } set { X = value.X; Z = value.Y; W = value.Z; Y = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Xwyz { get { return new Vector4(X, W, Y, Z); } set { X = value.X; W = value.Y; Y = value.Z; Z = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Xwzy { get { return new Vector4(X, W, Z, Y); } set { X = value.X; W = value.Y; Z = value.Z; Y = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Yxzw { get { return new Vector4(Y, X, Z, W); } set { Y = value.X; X = value.Y; Z = value.Z; W = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Yxwz { get { return new Vector4(Y, X, W, Z); } set { Y = value.X; X = value.Y; W = value.Z; Z = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Yyzw { get { return new Vector4(Y, Y, Z, W); } set { X = value.X; Y = value.Y; Z = value.Z; W = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Yywz { get { return new Vector4(Y, Y, W, Z); } set { X = value.X; Y = value.Y; W = value.Z; Z = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Yzxw { get { return new Vector4(Y, Z, X, W); } set { Y = value.X; Z = value.Y; X = value.Z; W = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Yzwx { get { return new Vector4(Y, Z, W, X); } set { Y = value.X; Z = value.Y; W = value.Z; X = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Ywxz { get { return new Vector4(Y, W, X, Z); } set { Y = value.X; W = value.Y; X = value.Z; Z = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Ywzx { get { return new Vector4(Y, W, Z, X); } set { Y = value.X; W = value.Y; Z = value.Z; X = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Zxyw { get { return new Vector4(Z, X, Y, W); } set { Z = value.X; X = value.Y; Y = value.Z; W = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Zxwy { get { return new Vector4(Z, X, W, Y); } set { Z = value.X; X = value.Y; W = value.Z; Y = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Zyxw { get { return new Vector4(Z, Y, X, W); } set { Z = value.X; Y = value.Y; X = value.Z; W = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Zywx { get { return new Vector4(Z, Y, W, X); } set { Z = value.X; Y = value.Y; W = value.Z; X = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Zwxy { get { return new Vector4(Z, W, X, Y); } set { Z = value.X; W = value.Y; X = value.Z; Y = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Zwyx { get { return new Vector4(Z, W, Y, X); } set { Z = value.X; W = value.Y; Y = value.Z; X = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Zwzy { get { return new Vector4(Z, W, Z, Y); } set { X = value.X; W = value.Y; Z = value.Z; Y = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Wxyz { get { return new Vector4(W, X, Y, Z); } set { W = value.X; X = value.Y; Y = value.Z; Z = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Wxzy { get { return new Vector4(W, X, Z, Y); } set { W = value.X; X = value.Y; Z = value.Z; Y = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Wyxz { get { return new Vector4(W, Y, X, Z); } set { W = value.X; Y = value.Y; X = value.Z; Z = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Wyzx { get { return new Vector4(W, Y, Z, X); } set { W = value.X; Y = value.Y; Z = value.Z; X = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Wzxy { get { return new Vector4(W, Z, X, Y); } set { W = value.X; Z = value.Y; X = value.Z; Y = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Wzyx { get { return new Vector4(W, Z, Y, X); } set { W = value.X; Z = value.Y; Y = value.Z; X = value.W; } }

        [System.Xml.Serialization.XmlIgnore]
        public Vector4 Wzyw { get { return new Vector4(W, Z, Y, W); } set { X = value.X; Z = value.Y; Y = value.Z; W = value.W; } }

        public static Vector4 operator +(Vector4 left, Vector4 right) {
            left.X += right.X;
            left.Y += right.Y;
            left.Z += right.Z;
            left.W += right.W;
            return left;
        }

        public static Vector4 operator -(Vector4 left, Vector4 right) {
            left.X -= right.X;
            left.Y -= right.Y;
            left.Z -= right.Z;
            left.W -= right.W;
            return left;
        }

        public static Vector4 operator -(Vector4 vec) {
            vec.X = -vec.X;
            vec.Y = -vec.Y;
            vec.Z = -vec.Z;
            vec.W = -vec.W;
            return vec;
        }

        public static Vector4 operator *(Vector4 vec, float scale) {
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            vec.W *= scale;
            return vec;
        }

        public static Vector4 operator *(float scale, Vector4 vec) {
            vec.X *= scale;
            vec.Y *= scale;
            vec.Z *= scale;
            vec.W *= scale;
            return vec;
        }

        public static Vector4 operator *(Vector4 vec, Vector4 scale) {
            vec.X *= scale.X;
            vec.Y *= scale.Y;
            vec.Z *= scale.Z;
            vec.W *= scale.W;
            return vec;
        }

        /* public static Vector4 operator *(Vector4 vec, Matrix4 mat) {
            Vector4 result;
            Vector4.Transform(ref vec, ref mat, out result);
            return result;
        }

        public static Vector4 operator *(Matrix4 mat, Vector4 vec) {
            Vector4 result;
            Vector4.Transform(ref mat, ref vec, out result);
            return result;
        } */

        public static Vector4 operator *(BusEngine.Quaternion quat, Vector4 vec) {
            Vector4 result;
            Vector4.Transform(ref vec, ref quat, out result);
            return result;
        }

        public static Vector4 operator /(Vector4 vec, float scale) {
            vec.X /= scale;
            vec.Y /= scale;
            vec.Z /= scale;
            vec.W /= scale;
            return vec;
        }

        public static bool operator ==(Vector4 left, Vector4 right) {
            return left.Equals(right);
        }

        public static bool operator !=(Vector4 left, Vector4 right) {
            return !left.Equals(right);
        }

        //[System.CLSCompliant(false)]
        unsafe public static explicit operator float*(Vector4 v) {
            return &v.X;
        }

        public static explicit operator System.IntPtr(Vector4 v) {
            unsafe {
                return (System.IntPtr)(&v.X);
            }
        }

        private static string listSeparator = System.Globalization.CultureInfo.CurrentCulture.TextInfo.ListSeparator;

        public override string ToString() {
            return System.String.Format("({0}{4} {1}{4} {2}{4} {3})", X, Y, Z, W, listSeparator);
        }

        public override int GetHashCode() {
            unchecked {
                var hashCode = this.X.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Y.GetHashCode();
                hashCode = (hashCode * 397) ^ this.Z.GetHashCode();
                hashCode = (hashCode * 397) ^ this.W.GetHashCode();
                return hashCode;
            }
        }

        public override bool Equals(object obj) {
            if (!(obj is Vector4)) {
                return false;
            }
            return this.Equals((Vector4)obj);
        }

        public bool Equals(Vector4 other) {
            return X == other.X && Y == other.Y && Z == other.Z && W == other.W;
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
		public double DisposeAuto = 1000;

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
				//System.Threading.Tasks.Task.Run(() => {
					this.Dispose(true);
				//});
			}

			System.GC.SuppressFinalize(this);
		}

		protected virtual void Dispose(bool disposing) {
			if (!this.IsPlay && (this.IsStop || this.IsEnd) && this.IsDispose) {
				((System.ComponentModel.ISupportInitialize)(_winForm)).BeginInit();
				BusEngine.UI.Canvas.WinForm.SuspendLayout();
				BusEngine.UI.Canvas.WinForm.Controls.Remove(_winForm);
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
					//this.OnDispose.Invoke(this, this.Url);
					// https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.forms.control.invoke?view=windowsdesktop-7.0
					BusEngine.UI.Canvas.WinForm.Invoke(this.OnDispose, new object[2] {this, this.Url});
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