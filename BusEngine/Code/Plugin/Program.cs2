/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

//#define AUDIO_LOG
//#define BROWSER_LOG
//#define LOCALIZATION_LOG
#define VIDEO_LOG
/** API BusEngine.Game - пользовательский код */
[assembly: BusEngine.Tooltip("Это описание сборки для BusEngine.Editor")]
namespace BusEngine.Game {
	/** API BusEngine.Plugin */
	[BusEngine.Tooltip("Это описание класса для BusEngine.Editor")]
	public class MyPlugin : BusEngine.Plugin {
		[BusEngine.Tooltip("Это описание поля для BusEngine.Editor")]
		private static System.Windows.Forms.Form SplashScreen;

		// при запуске BusEngine до создания формы
		public override void Initialize() {
			// загружаем свой язык
			BusEngine.Localization.OnLoadStatic += OnLoadLanguage;
			#if LOCALIZATION_LOG
			BusEngine.Log.Info("2 {0}", BusEngine.Localization.GetLanguageStatic("error"));
			#endif
			BusEngine.Localization localization = new BusEngine.Localization();
			localization.Load("Ukrainian");
			#if LOCALIZATION_LOG
			BusEngine.Log.Info("2 {0}", localization.GetLanguage("error"));
			BusEngine.Log.Info("2 {0}", BusEngine.Localization.GetLanguageStatic("error"));
			#endif

			// добавляем стартовую обложку
			SplashScreen = new System.Windows.Forms.Form();
			SplashScreen.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			SplashScreen.Width = 640;
			SplashScreen.Height = 360;
			SplashScreen.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			if (System.IO.File.Exists(System.IO.Path.GetFullPath(BusEngine.Engine.DataDirectory + "Textures/UI/splashscreen.png"))) {
				SplashScreen.BackgroundImage = System.Drawing.Image.FromFile(BusEngine.Engine.DataDirectory + "Textures/UI/splashscreen.png");
			}
			SplashScreen.Show();
			// запускаем аудио
			if (BusEngine.Engine.Platform == "Windows" || BusEngine.Engine.Platform == "WindowsLauncher") {
				BusEngine.Audio x = new BusEngine.Audio("Audios/BusEngine.mp3").Play();
				x.OnEnd += (BusEngine.Audio a, string url) => {
					a.Dispose();
				};
				//System.Threading.Thread.Sleep(3000);
				System.Threading.Tasks.Task.Delay(3000).Wait();
				x.Stop();
			} else {
				//System.Threading.Thread.Sleep(1000);
				System.Threading.Tasks.Task.Delay(1000).Wait();
			}
		}

		public override void Initialize(string plugin) {
			BusEngine.Log.Info("Plugin.dll Initialize {0}", System.IO.Path.GetFileName(plugin));
		}

		public override void Initialize(string plugin, string state) {
			BusEngine.Log.Info("Plugin.dll Initialize state {0} {1}", plugin, state);
		}

		// при запуске BusEngine после создания формы Canvas
		public override void InitializeСanvas() {
			// убираем стартовую обложку
			SplashScreen.Close();
			SplashScreen.Dispose();
			BusEngine.UI.Canvas.WinForm.KeyDown += new System.Windows.Forms.KeyEventHandler(OnKeyDown);

			// запускаем аудио
			if (BusEngine.Engine.Platform == "Windows" || BusEngine.Engine.Platform == "WindowsLauncher") {
				string[] audios = {"Audios/BusEngine.mp3"};
				BusEngine.Audio audio = new BusEngine.Audio();
				/** событие запуска аудио */
				audio.OnPlay += (BusEngine.Audio a, string url) => {
					#if AUDIO_LOG
					BusEngine.Log.Info("Аудио OnPlayAudio: {0}", url);
					BusEngine.Log.Info("Аудио OnPlayAudio: {0}", a.Url);
					#endif
				};
				/** событие запуска аудио */
				/** событие повтора аудио */
				audio.OnLoop += (BusEngine.Audio a, string url) => {
					#if AUDIO_LOG
					BusEngine.Log.Info("Аудио OnLoopAudio: {0}", url);
					BusEngine.Log.Info("Аудио OnLoopAudio: {0}", a.Url);
					#endif
				};
				/** событие повтора аудио */
				/** событие временной остановки аудио */
				audio.OnPause += (BusEngine.Audio a, string url) => {
					#if AUDIO_LOG
					BusEngine.Log.Info("Аудио OnPauseAudio: {0}", url);
					BusEngine.Log.Info("Аудио OnPauseAudio: {0}", a.Url);
					#endif
				};
				/** событие временной остановки аудио */
				/** событие ручной остановки аудио */
				audio.OnStop += (BusEngine.Audio a, string url) => {
					#if AUDIO_LOG
					BusEngine.Log.Info("Аудио OnStopAudio: {0}", url);
					BusEngine.Log.Info("Аудио OnStopAudio: {0}", a.Url);
					#endif
					a.Dispose();

					if (audios.Length > 0) {
						System.Array.Reverse(audios);
						System.Array.Resize(ref audios, audios.Length - 1);
						System.Array.Reverse(audios);
					}

					if (audios.Length > 0) {
						a.Play(audios[0]);
					}
				};
				/** событие ручной остановки аудио */
				/** событие автоматической остановки аудио */
				audio.OnEnd += (BusEngine.Audio a, string url) => {
					#if AUDIO_LOG
					BusEngine.Log.Info("Аудио OnEndAudio: {0}", url);
					BusEngine.Log.Info("Аудио OnEndAudio: {0}", a.Url);
					#endif
					a.Dispose();

					if (audios.Length > 0) {
						System.Array.Reverse(audios);
						System.Array.Resize(ref audios, audios.Length - 1);
						System.Array.Reverse(audios);
					}

					if (audios.Length > 0) {
						a.Play(audios[0]);
					}
				};
				/** событие автоматической остановки аудио */
				/** событие удаления аудио */
				audio.OnDispose += (BusEngine.Audio a, string url) => {
					#if AUDIO_LOG
					BusEngine.Log.Info("Аудио OnDisposeAudio: {0}", url);
					BusEngine.Log.Info("Аудио OnDisposeAudio: {0}", a.Url);
					#endif
				};
				/** событие удаления аудио */
				/** событие отсутствия аудио */
				audio.OnNotFound += (BusEngine.Audio a, string url) => {
					#if AUDIO_LOG
					BusEngine.Log.Info("Аудио OnNotFoundAudio: {0}", url);
					BusEngine.Log.Info("Аудио OnNotFoundAudio: {0}", a.Url);
					#endif
				};
				/** событие отсутствия аудио */
				audio.Position = 0;
				audio.Play(audios[0]);
				//audio.Stop();
				BusEngine.UI.Canvas.WinForm.KeyDown += (o, e) => {
					// выкл аудио
					if (e.KeyCode == System.Windows.Forms.Keys.Enter) {
						if (audio != null) {
							if (audios.Length == 0) {
								//audio.Dispose();
								audio = null;
							} else {
								audio.Stop();
								//audio.Dispose();
							}
						}
					}
				};
			}

			// запускаем видео
			if (BusEngine.Engine.Platform == "Windows" || BusEngine.Engine.Platform == "WindowsLauncher") {
				//string[] videos = {"https://buslikdrev.by/video/Unity.mp4", "https://buslikdrev.by/video/Unity.mp4", "https://buslikdrev.by/video/Unity.mp4"};
				string[] videos = {"https://buslikdrev.by/video/Unity.mp4", "Videos/BusEngine.mp4"};
				//new BusEngine.Video("https://buslikdrev.by/video/Unity.mp4");
				/* new BusEngine.Video("Videos/BusEngine.mp4").Play();
				new BusEngine.Video("Videos/BusEngine.mp4").Play();
				new BusEngine.Video("Videos/BusEngine.mp4").Play();
				new BusEngine.Video("Videos/BusEngine.mp4").Play();
				new BusEngine.Video("Videos/BusEngine.mp4").Play();
				new BusEngine.Video("Videos/BusEngine.mp4").Play(); */
				BusEngine.Video video = new BusEngine.Video();
				/** событие запуска видео */
				video.OnPlay += (BusEngine.Video v, string url) => {
					#if VIDEO_LOG
					BusEngine.Log.Info("Видео OnPlayVideo: {0}", url);
					BusEngine.Log.Info("Видео OnPlayVideo: {0}", v.Url);
					#endif
				};
				/** событие запуска видео */
				/** событие повтора видео */
				video.OnLoop += (BusEngine.Video v, string url) => {
					#if VIDEO_LOG
					BusEngine.Log.Info("Видео OnLoopVideo: {0}", url);
					BusEngine.Log.Info("Видео OnLoopVideo: {0}", v.Url);
					#endif
				};
				/** событие повтора видео */
				/** событие временной остановки видео */
				video.OnPause += (BusEngine.Video v, string url) => {
					#if VIDEO_LOG
					BusEngine.Log.Info("Видео OnPauseVideo: {0}", url);
					BusEngine.Log.Info("Видео OnPauseVideo: {0}", v.Url);
					#endif
				};
				/** событие временной остановки видео */
				/** событие ручной остановки видео */
				video.OnStop += (BusEngine.Video v, string url) => {
					#if VIDEO_LOG
					BusEngine.Log.Info("Видео OnStopVideo: {0}", url);
					BusEngine.Log.Info("Видео OnStopVideo: {0}", v.Url);
					#endif

					if (videos.Length > 0) {
						System.Array.Reverse(videos);
						System.Array.Resize(ref videos, videos.Length - 1);
						System.Array.Reverse(videos);
					}

					if (videos.Length > 0) {
						v.Play(videos[0]);
					} else {
						//Browser();
					}
				};
				/** событие ручной остановки видео */
				/** событие автоматической остановки видео */
				video.OnEnd += (BusEngine.Video v, string url) => {
					#if VIDEO_LOG
					BusEngine.Log.Info("Видео OnEndVideo: {0}", url);
					BusEngine.Log.Info("Видео OnEndVideo: {0}", v.Url);
					#endif

					if (videos.Length > 0) {
						System.Array.Reverse(videos);
						System.Array.Resize(ref videos, videos.Length - 1);
						System.Array.Reverse(videos);
					}

					if (videos.Length > 0) {
						v.Play(videos[0]);
					} else {
						//Browser();
					}
				};
				/** событие автоматической остановки видео */
				/** событие удаления видео */
				video.OnDispose += (BusEngine.Video v, string url) => {
					#if VIDEO_LOG
					BusEngine.Log.Info("Видео OnDisposeVideo: {0}", url);
					BusEngine.Log.Info("Видео OnDisposeVideo: {0}", v.Url);
					#endif

					Browser();
				};
				/** событие удаления видео */
				/** событие отсутствия видео */
				video.OnNotFound += (BusEngine.Video v, string url) => {
					#if VIDEO_LOG
					BusEngine.Log.Info("Видео OnNotFoundVideo: {0}", url);
					BusEngine.Log.Info("Видео OnNotFoundVideo: {0}", v.Url);
					#endif

					if (videos.Length > 0) {
						System.Array.Reverse(videos);
						System.Array.Resize(ref videos, videos.Length - 1);
						System.Array.Reverse(videos);
					}

					if (videos.Length > 0) {
						v.Play(videos[0]);
					} else {
						//Browser();
					}
				};
				/** событие отсутствия видео */
				video.Position = 0;
				video.Play(videos[0]);
				//video.Stop();
				BusEngine.UI.Canvas.WinForm.KeyDown += (o, e) => {
					BusEngine.Log.Info("клавиатура клик video {0}", o.GetType());
					// выкл видео
					if (e.KeyCode == System.Windows.Forms.Keys.Space) {
						if (video != null) {
							if (videos.Length == 0) {
								//video.Dispose();
								video = null;
							} else {
								video.Stop();
								//video.Dispose();
							}
						}
					}
				};
			}

			// запускаем браузер WinForm только в одном потоке =(
			if (BusEngine.Engine.Platform == "WindowsEditor") {
				Browser();
			}
		}

		// запускаем браузер
		private void Browser() {
			if (BusEngine.Engine.Platform == "WindowsEditor") {
				BusEngine.Browser.Initialize("https://threejs.org/editor/");
			} else if (BusEngine.Engine.Platform == "WindowsLauncher") {
				BusEngine.Browser.Initialize("index.html");
				BusEngine.Browser.OnPostMessageStatic += OnPostMessage;
			}
		}

		/** событие нажатия любой кнопки */
		// https://learn.microsoft.com/en-us/dotnet/api/system.consolekey?view=netframework-4.8
		private void OnKeyDown(object o, System.Windows.Forms.KeyEventArgs e) {
			BusEngine.Log.Info("клавиатура клик {0}", o.GetType());
			BusEngine.Log.Info();
			// выключаем движок по нажатию на Esc
			if (e.KeyCode == System.Windows.Forms.Keys.Escape) {
				BusEngine.UI.Canvas.WinForm.KeyDown -= new System.Windows.Forms.KeyEventHandler(OnKeyDown);
				//Dispose();
				BusEngine.Engine.Shutdown();
			}
			// вкл\выкл консоль движка по нажатию на ~
			if (e.KeyCode == System.Windows.Forms.Keys.Oem3) {
				BusEngine.Log.ConsoleToggle();
			}
		}
		/** событие нажатия любой кнопки */

		/** событие загрузки языка */
		private void OnLoadLanguage(BusEngine.Localization l, string language) {
			#if LOCALIZATION_LOG
			BusEngine.Log.Info("Язык изменился: {0}", language);
			BusEngine.Log.Info("Язык изменился: {0}", l.GetLanguage("error"));
			#endif
		}
		/** событие загрузки языка */

		/** событие запуска аудио */
		private void OnPlayAudio(BusEngine.Audio a, string url) {
			BusEngine.Log.Info("Аудио OnPlayAudio: {0}", url);
			BusEngine.Log.Info("Аудио OnPlayAudio: {0}", a.Url);
		}
		/** событие запуска аудио */

		/** событие повтора аудио */
		private void OnLoopAudio(BusEngine.Audio a, string url) {
			BusEngine.Log.Info("Аудио OnLoopAudio: {0}", url);
			BusEngine.Log.Info("Аудио OnLoopAudio: {0}", a.Url);
		}
		/** событие повтора аудио */

		/** событие временной остановки аудио */
		private void OnPauseAudio(BusEngine.Audio a, string url) {
			BusEngine.Log.Info("Аудио OnPauseAudio: {0}", url);
			BusEngine.Log.Info("Аудио OnPauseAudio: {0}", a.Url);
		}
		/** событие временной остановки аудио */

		/** событие ручной остановки аудио */
		private void OnStopAudio(BusEngine.Audio a, string url) {
			BusEngine.Log.Info("Аудио OnStopAudio: {0}", url);
			BusEngine.Log.Info("Аудио OnStopAudio: {0}", a.Url);
		}
		/** событие ручной остановки аудио */

		/** событие автоматической остановки аудио */
		private void OnEndAudio(BusEngine.Audio a, string url) {
			BusEngine.Log.Info("Аудио OnEndAudio: {0}", url);
			BusEngine.Log.Info("Аудио OnEndAudio: {0}", a.Url);
		}
		/** событие автоматической остановки аудио */

		/** событие удаления аудио */
		private void OnDisposeAudio(BusEngine.Audio a, string url) {
			BusEngine.Log.Info("Аудио OnDisposeAudio: {0}", url);
			BusEngine.Log.Info("Аудио OnDisposeAudio: {0}", a.Url);
		}
		/** событие удаления аудио */

		/** событие отсутствия аудио */
		private void OnNotFoundAudio(BusEngine.Audio a, string url) {
			BusEngine.Log.Info("Аудио OnNotFoundAudio: {0}", url);
			BusEngine.Log.Info("Аудио OnNotFoundAudio: {0}", a.Url);
		}
		/** событие отсутствия аудио */

		/** событие запуска видео */
		private void OnPlayAudio(BusEngine.Video v, string url) {
			BusEngine.Log.Info("Видео OnPlayVideo: {0}", url);
			BusEngine.Log.Info("Видео OnPlayVideo: {0}", v.Url);
		}
		/** событие запуска видео */

		/** событие повтора видео */
		private void OnLoopVideo(BusEngine.Video v, string url) {
			BusEngine.Log.Info("Видео OnLoopVideo: {0}", url);
			BusEngine.Log.Info("Видео OnLoopVideo: {0}", v.Url);
		}
		/** событие повтора видео */

		/** событие временной остановки видео */
		private void OnPauseVideo(BusEngine.Video v, string url) {
			BusEngine.Log.Info("Видео OnPauseVideo: {0}", url);
			BusEngine.Log.Info("Видео OnPauseVideo: {0}", v.Url);
		}
		/** событие временной остановки видео */

		/** событие ручной остановки видео */
		private void OnStopVideo(BusEngine.Video v, string url) {
			BusEngine.Log.Info("Видео OnStopVideo: {0}", url);
			BusEngine.Log.Info("Видео OnStopVideo: {0}", v.Url);
		}
		/** событие ручной остановки видео */

		/** событие автоматической остановки видео */
		private void OnEndVideo(BusEngine.Video v, string url) {
			BusEngine.Log.Info("Видео OnEndVideo: {0}", url);
			BusEngine.Log.Info("Видео OnEndVideo: {0}", v.Url);
		}
		/** событие автоматической остановки видео */

		/** событие удаления видео */
		private void OnDisposeVideo(BusEngine.Video v, string url) {
			BusEngine.Log.Info("Видео OnDisposeVideo: {0}", url);
			BusEngine.Log.Info("Видео OnDisposeVideo: {0}", v.Url);
		}
		/** событие удаления видео */

		/** событие отсутствия видео */
		private void OnNotFoundVideo(BusEngine.Video v, string url) {
			BusEngine.Log.Info("Видео OnNotFoundVideo: {0}", url);
			BusEngine.Log.Info("Видео OnNotFoundVideo: {0}", v.Url);
		}
		/** событие отсутствия видео */

		/** событие получения сообщения из браузера */
		private void OnPostMessage(string message) {
			if (message == "Exit") {
				BusEngine.Engine.Shutdown();
			} else if (message == "Debug") {
				BusEngine.Log.Info("JavaScript: Привет CSharp!");
				BusEngine.Log.Info("На команду: " + message);
				BusEngine.Browser.ExecuteJSStatic("document.dispatchEvent(new CustomEvent('BusEngineMessage', {bubbles: true, detail: {hi: 'CSharp: Прювэт JavaScript!', data: 'Получил твою команду! Вось яна: " + message + "'}}));");
			} else {
				if (message.Substring(0, 8) == "console|") {
					System.Console.ForegroundColor = System.ConsoleColor.Cyan;
					BusEngine.Log.Info("Console: {0}", message.Substring(8));
					System.Console.ResetColor();
				}
			}
		}
		/** событие получения сообщения из браузера */
	}
	/** API BusEngine.Plugin */
}
/** API BusEngine.Game - пользовательский код */