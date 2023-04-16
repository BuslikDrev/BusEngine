/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

#define AUDIO_LOG
//#define BROWSER_LOG
#define LOCALIZATION_LOG
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
			BusEngine.Localization.OnLoadStatic += (BusEngine.Localization l, string language) => {
				#if LOCALIZATION_LOG
				BusEngine.Log.Info("Язык изменился: {0}", language);
				BusEngine.Log.Info("Язык изменился: {0}", l.GetLanguage("error"));
				#endif
			};
			new BusEngine.Localization().Load("Ukrainian");

			// добавляем стартовую обложку
			SplashScreen = new System.Windows.Forms.Form();
			SplashScreen.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			SplashScreen.Width = 640;
			SplashScreen.Height = 360;
			SplashScreen.TopMost = true;
			SplashScreen.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			if (System.IO.File.Exists(System.IO.Path.GetFullPath(BusEngine.Engine.DataDirectory + "Textures/UI/splashscreen.png"))) {
				SplashScreen.BackgroundImage = System.Drawing.Image.FromFile(BusEngine.Engine.DataDirectory + "Textures/UI/splashscreen.png");
			}
			SplashScreen.Show();

			// устанавливаем настройки
			if (BusEngine.Engine.Platform == "WindowsEditor") {
				BusEngine.Engine.SettingEngine["console_commands"]["r_Fullscreen"] = "-2";
			}

			if (BusEngine.Engine.Platform == "Windows") {
				BusEngine.Engine.SettingEngine["console_commands"]["r_Width"] = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Width + "";
				BusEngine.Engine.SettingEngine["console_commands"]["r_Height"] = System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Height + "";
			}

			if (BusEngine.Engine.Platform == "WindowsLauncher") {
				BusEngine.Engine.SettingEngine["console_commands"]["r_Width"] = "960";
				BusEngine.Engine.SettingEngine["console_commands"]["r_Height"] = "540";
			}

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

		// при запуске BusEngine после создания формы Canvas
		public override void InitializeСanvas() {
			// убираем стартовую обложку
			SplashScreen.Close();
			SplashScreen.Dispose();

			// запускаем аудио
			if (BusEngine.Engine.Platform == "Windows" || BusEngine.Engine.Platform == "WindowsLauncher") {
				// создаём массив ссылок или путей
				string[] audios = {"Audios/BusEngine.ogg", "Audios/BusEngine.mp3", "Audios/BusEngine.mp3"};
				// создаём новый объект аудио
				BusEngine.Audio _audio = new BusEngine.Audio(audios).Play();
				// создаём событие клавиатуры внутри метода
				System.Windows.Forms.KeyEventHandler KeyDownAudio = (o, e) => {
					// выбираем клавишу пробела
					if (e.KeyCode == System.Windows.Forms.Keys.Enter) {
						if (!_audio.IsDispose) {
							_audio.Stop();
							#if AUDIO_LOG
							BusEngine.Log.Info("Stop Audio");
							#endif
						}
					}
				};
				_audio.OnDispose += (BusEngine.Audio a, string url) => {
					#if AUDIO_LOG
					BusEngine.Log.Info("Dispose Audio");
					#endif
					// удаляем событие клавиш
					BusEngine.UI.Canvas.WinForm.KeyDown -= KeyDownAudio;
					/* здесь пишем код запуска другого кода */
				};
				// добавляем событие клавиш
				BusEngine.UI.Canvas.WinForm.KeyDown += KeyDownAudio;
			}

			// запускаем видео
			if (BusEngine.Engine.Platform == "Windows") {
				// создаём массив ссылок или путей
				string[] videos = {"Videos/BusEngine.mp4", "Videos/BusEngine.webm", "Videos/BusEngine.ogv"};
				// создаём новый объект видео
				BusEngine.Video _video = new BusEngine.Video(videos).Play();
				// создаём событие клавиатуры внутри метода
				System.Windows.Forms.KeyEventHandler KeyDownVideo = (o, e) => {
					// выбираем клавишу пробела
					if (e.KeyCode == System.Windows.Forms.Keys.Space) {
						if (!_video.IsDispose) {
							_video.Stop();
							#if VIDEO_LOG
							BusEngine.Log.Info("Stop Video");
							#endif
						}
					}
				};
				_video.OnDispose += (BusEngine.Video v, string url) => {
					#if VIDEO_LOG
					BusEngine.Log.Info("Dispose Video");
					#endif
					// удаляем событие клавиш
					BusEngine.UI.Canvas.WinForm.KeyDown -= KeyDownVideo;
					// запускаем браузер
					Browser("index.html");
				};
				// добавляем событие клавиш
				BusEngine.UI.Canvas.WinForm.KeyDown += KeyDownVideo;
			}

			// запускаем браузер
			if (BusEngine.Engine.Platform == "WindowsEditor") {
				Browser("https://threejs.org/editor/"); 
			} else if (BusEngine.Engine.Platform == "WindowsLauncher") {
				Browser("launcher.html");
			}
		}

		// запускаем браузер WinForm только в одном потоке =(
		private void Browser(string url) {
			BusEngine.Browser.Initialize(url);                                                   
			BusEngine.Browser.OnPostMessageStatic += (string message) => {
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
			};
		}
	}
	/** API BusEngine.Plugin */
}
/** API BusEngine.Game - пользовательский код */