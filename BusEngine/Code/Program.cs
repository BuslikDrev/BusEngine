/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */
/* C# 6.0+ NET.Framework 4.5.2 */

namespace BusEngine {
	/** Открытое API Form - нужно как-то закрыть или переделать в BusEngine.UI */
	public class UI : System.Windows.Forms.Form {
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
		private static void ConsoleMain(string[] args) {
			System.Console.WriteLine("Command line = {0}", System.Environment.CommandLine);

			for (int i = 0; i < args.Length; ++i) {
				System.Console.WriteLine("Argument{0} = {1}", i + 1, args[i]);
				System.Console.ReadLine();
			}
		}
		/** консоль */

		/** видео */
		private LibVLCSharp.Shared.LibVLC _VLC;
		private LibVLCSharp.Shared.MediaPlayer _VLC_MP;
		private LibVLCSharp.WinForms.VideoView _VLC_VideoView;
		/** видео */

		/** событие нажатия любой кнопки */
		// https://learn.microsoft.com/en-us/dotnet/api/system.consolekey?view=netframework-4.8
		private void OnKeyDown(object o, System.Windows.Forms.KeyEventArgs e) {
			// Выключаем движок по нажатию на Esc
			if (e.KeyCode == System.Windows.Forms.Keys.Escape) {
				this.KeyDown -= new System.Windows.Forms.KeyEventHandler(OnKeyDown);
				this.Dispose();
				BusEngine.Shutdown();
			}
			// Вкл\Выкл консоль движка по нажатию на ~
			if (e.KeyCode == System.Windows.Forms.Keys.Oem3) {
				if (StatusConsole == false && !AttachConsole(-1)) {
					AllocConsole();
					StatusConsole = true;
				} else {
					FreeConsole();
					StatusConsole = false;
				}
			}
		}
		/** событие нажатия любой кнопки */

		private void OnKeyDown2(object o, System.Windows.Forms.KeyEventArgs e) {
			this.KeyDown -= new System.Windows.Forms.KeyEventHandler(OnKeyDown2);
			this.Dispose();
			BusEngine.Shutdown();
		}

		/** событие остановки видео */
		private void onVideoStop(object o, object e) {
			this.Controls.Remove(_VLC_VideoView);
			this._VLC_MP.Stop();
            this._VLC_MP.Dispose();
            this._VLC.Dispose();
			this.KeyPreview = true;
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(OnKeyDown);
		}
		/** событие остановки видео */

		/** событие клика из браузера */
		private void onBrowserClick(object o, object e) {

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
			settings.UserAgent = settings.UserAgent.Replace(@"Chrome", @"BusEngine");
			settings.LogSeverity = CefSharp.LogSeverity.Disable;
			CefSharp.Cef.Initialize(settings);
			CefSharp.WinForms.ChromiumWebBrowser _browser = new CefSharp.WinForms.ChromiumWebBrowser(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "/../../Data/index.html");
			_browser.KeyDown += new System.Windows.Forms.KeyEventHandler(OnKeyDown2);
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

			/* Microsoft.Web.WebView2.Core.CoreWebView2 _browser = new Microsoft.Web.WebView2.Core.CoreWebView2();
			_browser.Navigate(new System.Uri("https://www.cryengine.com/")); */
		}
		/** функция запуска браузера */

		/** функция запуска окна приложения */
		public UI() {
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
			this.KeyDown += new System.Windows.Forms.KeyEventHandler(OnKeyDown);

			// запускаем видео
			Video();
			// запускаем браузер
			Browser();

			// показываем форму\включаем\запускаем\стартуем показ окна
			//this.ShowDialog();
		}
		/** функция запуска окна приложения */
	}

	/** Открытое API BusEngine */
	public class BusEngine {
		private static UI _ui;

		/** функция запуска приложения */
		//[System.STAThread] // если однопоточное приложение
		private static void Main(string[] args) {
			_ui = new UI();

			System.Windows.Forms.Application.Run(_ui);
		}
		/** функция запуска приложения */

		/** функция остановки приложения */
		public static void Shutdown() {
			System.Windows.Forms.Application.Exit();
		}
		/** функция остановки приложения */
	}
}