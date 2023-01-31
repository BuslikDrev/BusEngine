/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

/** API BusEngine.Game - пользовательский код */
namespace BusEngine.Game {
	/** API BusEngine.Plugin */
	public class MyPlugin : BusEngine.Plugin {
		private static System.Windows.Forms.Form SplashScreen;
		private static BusEngine.Audio video;
		private static string[] videos = {"https://buslikdrev.by/video/Unity.mp4", "https://buslikdrev.by/video/Unity.mp4"};

		// при запуске BusEngine до создания формы
		public override void Initialize() {
			// загружаем свой язык
			BusEngine.Localization.SOnLoad += OnLoadLanguage;
			BusEngine.Log.Info("2 {0}", BusEngine.Localization.SGetLanguage("error"));
			BusEngine.Localization localization = new BusEngine.Localization();
			localization.Load("Ukrainian");
			//BusEngine.Log.Info("2 {0}", localization.GetLanguage("error"));
			//BusEngine.Log.Info("2 {0}", BusEngine.Localization.SGetLanguage("error"));

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
				BusEngine.Audio a = new BusEngine.Audio();
				a.Position = 0;
				a.OnStop += (o, e) => {
					o.Dispose();
				};
				a.Play("Audios/BusEngine.mp3");
				System.Threading.Thread.Sleep(3000);
				a.Stop();
			} else {
				System.Threading.Thread.Sleep(1000);
			}
		}

		// при запуске BusEngine после создания формы Canvas
		public override void InitializeСanvas() {
			// убираем стартовую обложку
			SplashScreen.Close();
			SplashScreen.Dispose();
			BusEngine.UI.Canvas.WinForm.KeyDown += new System.Windows.Forms.KeyEventHandler(OnKeyDown);

			// запускаем аудио
			if (BusEngine.Engine.Platform == "Windows" || BusEngine.Engine.Platform == "WindowsLauncher") {
				string[] audios = {"Audios/BusEngine.mp3", "Audios/BusEngine.mp3", "Audios/BusEngine.mp3", "Audios/BusEngine.mp3", "Audios/BusEngine.mp3", "Audios/BusEngine.mp3", "Audios/BusEngine.mp3", "Audios/BusEngine.mp3", "Audios/BusEngine.mp3", "Audios/BusEngine.mp3", "Audios/BusEngine.mp3"};
				BusEngine.Audio audio = new BusEngine.Audio();
				/** событие запуска аудио */
				audio.OnPlay += (BusEngine.Audio a, string url) => {
					BusEngine.Log.Info("Аудио OnPlayAudio: {0}", url);
					BusEngine.Log.Info("Аудио OnPlayAudio: {0}", a.Url);
				};
				/** событие запуска аудио */
				/** событие повтора аудио */
				audio.OnLoop += (BusEngine.Audio a, string url) => {
					BusEngine.Log.Info("Аудио OnLoopAudio: {0}", url);
					BusEngine.Log.Info("Аудио OnLoopAudio: {0}", a.Url);
				};
				/** событие повтора аудио */
				/** событие временной остановки аудио */
				audio.OnPause += (BusEngine.Audio a, string url) => {
					BusEngine.Log.Info("Аудио OnPauseAudio: {0}", url);
					BusEngine.Log.Info("Аудио OnPauseAudio: {0}", a.Url);
				};
				/** событие временной остановки аудио */
				/** событие ручной остановки аудио */
				audio.OnStop += (BusEngine.Audio a, string url) => {
					BusEngine.Log.Info("Аудио OnStopAudio: {0}", url);
					BusEngine.Log.Info("Аудио OnStopAudio: {0}", a.Url);

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
					BusEngine.Log.Info("Аудио OnEndAudio: {0}", url);
					BusEngine.Log.Info("Аудио OnEndAudio: {0}", a.Url);
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
					BusEngine.Log.Info("Аудио OnEndAudio: {0}", url);
					BusEngine.Log.Info("Аудио OnEndAudio: {0}", a.Url);
				};
				/** событие удаления аудио */
				/** событие отсутствия аудио */
				audio.OnNotFound += (BusEngine.Audio a, string url) => {
					BusEngine.Log.Info("Аудио OnNotFoundAudio: {0}", url);
					BusEngine.Log.Info("Аудио OnNotFoundAudio: {0}", a.Url);
				};
				/** событие отсутствия аудио */
				audio.Position = 0;
				audio.Play(audios[0]);
				//audio.Stop();
				BusEngine.UI.Canvas.WinForm.KeyDown += (o, e) => {
					// выкл аудио
					if (e.KeyCode == System.Windows.Forms.Keys.Space) {
						if (audio != null) {
							if (audios.Length == 0) {
								audio = null;
							} else {
								audio.Stop();
								audio.Dispose();
							}
						}
					}
				};
			}

			// запускаем видео
			/* if (BusEngine.Engine.Platform == "Windows" || BusEngine.Engine.Platform == "WindowsLauncher") {
				BusEngine.Video video = new BusEngine.Video();
				//video.OnPlay += OnPlayVideo;
				video.OnStop += OnStopVideo;
				video.OnDispose += OnDisposeVideo;
				//video.Play("https://buslikdrev.by/video/Unity.mp4");
				video.Play("Videos/BusEngine.mp4");
			} */

			// запускаем браузер
			if (BusEngine.Engine.Platform == "WindowsEditor") {
				BusEngine.Browser.Initialize("https://threejs.org/editor/");
				BusEngine.Browser.Initialize("https://buslikdrev.by/");
			} else if (1 == 0 && BusEngine.Engine.Platform == "WindowsLauncher") {
				BusEngine.Browser.Initialize("index.html");
				BusEngine.Browser.SOnPostMessage += OnPostMessage;
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
			BusEngine.Log.Info("Язык именился: {0}", language);
			BusEngine.Log.Info("Язык именился: {0}", l.GetLanguage("error"));
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
			BusEngine.Log.Info("Аудио OnEndAudio: {0}", url);
			BusEngine.Log.Info("Аудио OnEndAudio: {0}", a.Url);
		}
		/** событие удаления аудио */

		/** событие отсутствия аудио */
		private void OnNotFoundAudio(BusEngine.Audio a, string url) {
			BusEngine.Log.Info("Аудио OnNotFoundAudio: {0}", url);
			BusEngine.Log.Info("Аудио OnNotFoundAudio: {0}", a.Url);
		}
		/** событие отсутствия аудио */

		/** событие запуска видео */
		private void OnPlayVideo(BusEngine.Video v, string url) {
			BusEngine.Log.Info("Видео OnPlayVideo: {0}", url);
			BusEngine.Log.Info("Видео OnPlayVideo: {0}", v.Url);
		}
		/** событие запуска видео */

		/** событие остановки видео */
		private void OnStopVideo(BusEngine.Video v, string url) {
			BusEngine.Log.Info("Видео OnStopVideo: {0}", url);
			BusEngine.Log.Info("Видео OnStopVideo: {0}", v.Url);
			//v.Play("https://buslikdrev.by/video/Unity.mp4");
			//v.Stop();
		}
		/** событие остановки видео */

		/** событие удаления видео */
		private void OnDisposeVideo(BusEngine.Video v, string url) {
			BusEngine.Log.Info("Видео OnDisposeVideo: {0}", url);
			BusEngine.Log.Info("Видео OnDisposeVideo: {0}", v.Url);
			//video.Play("https://buslikdrev.by/video/Unity.mp4");
			//video.OnDispose -= OnDisposeVideo;
			//video.Dispose();
			BusEngine.Log.Info("Stop ===========");
			//v.OnDispose -= OnDisposeVideo;

			var x = new BusEngine.Video();
			//x.OnPlay += OnPlayVideo;
			//x.OnStop += OnStopVideo;
			x.OnDispose += OnDisposeVideo;
			x.Play("https://buslikdrev.by/video/Unity.mp4");

			BusEngine.Log.Info("Stop ===========");
		}
		/** событие удаления видео */

		/** событие получения сообщения из браузера */
		private void OnPostMessage(string message) {
			if (message == "Exit") {
				BusEngine.Engine.Shutdown();
			} else if (message == "Debug") {
				BusEngine.Log.Info("JavaScript: Привет CSharp!");
				BusEngine.Log.Info("На команду: " + message);
				BusEngine.Browser.SExecuteJS("document.dispatchEvent(new CustomEvent('BusEngineMessage', {bubbles: true, detail: {hi: 'CSharp: Прювэт JavaScript!', data: 'Получил твою команду! Вось яна: " + message + "'}}));");
			} else {
				if (message.Substring(0, 8) == "console|") {
					BusEngine.Log.Info(message.Substring(8));
				}
			}
		}
		/** событие получения сообщения из браузера */
	}
	/** API BusEngine.Plugin */
}
/** API BusEngine.Game - пользовательский код */