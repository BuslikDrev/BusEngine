/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

//#define BROWSER_LOG
#define LOCALIZATION_LOG
/** API BusEngine.Game - пользовательский код */
namespace BusEngine.Game {
	/** API BusEngine.Plugin */
	public class MyPlugin : BusEngine.Plugin {
		//private static System.Windows.Forms.Form SplashScreen;
		//private System.ComponentModel.IContainer notifyIconComponents;

		// при запуске BusEngine до создания формы
		public override void Initialize() {
			BusEngine.Log.Info("Commands: {0}", BusEngine.Tools.Json.Encode(BusEngine.Engine.Commands));

			// загружаем свой язык
			BusEngine.Localization.OnLoadStatic += (BusEngine.Localization l, string language) => {
				#if LOCALIZATION_LOG
				BusEngine.Log.Info("Язык изменился: {0}", language);
				BusEngine.Log.Info("Язык изменился: {0}", l.GetLanguage("error"));
				#endif
			};
			new BusEngine.Localization().Load("Ukrainian");

			// добавляем стартовую обложку
			/* SplashScreen = new System.Windows.Forms.Form();
			SplashScreen.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			SplashScreen.Width = 640;
			SplashScreen.Height = 360;
			SplashScreen.TopMost = true;
			SplashScreen.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			if (System.IO.File.Exists(System.IO.Path.GetFullPath(BusEngine.Engine.DataDirectory + "Textures/UI/splashscreen.png"))) {
				SplashScreen.BackgroundImage = System.Drawing.Image.FromFile(BusEngine.Engine.DataDirectory + "Textures/UI/splashscreen.png");
			}
			SplashScreen.Show(); */

			// устанавливаем настройки
			//BusEngine.Engine.SettingEngine["console_commands"]["r_Width"] = "1280";
			//BusEngine.Engine.SettingEngine["console_commands"]["r_Height"] = "720";

			//System.Threading.Tasks.Task.Delay(2000).Wait();
		}

		// при запуске BusEngine после создания формы Canvas
		public override void InitializeСanvas() {
			// убираем стартовую обложку
			/* SplashScreen.Close();
			SplashScreen.Dispose(); */

			//BusEngine.UI.Canvas.WinForm.TransparencyKey = System.Drawing.Color.Black;
			//BusEngine.UI.Canvas.WinForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			// деламем окно не поверх других окон
			BusEngine.UI.Canvas.WinForm.TopMost = false;

			// добавляем иконку в системном трее
			/* notifyIconComponents = new System.ComponentModel.Container();
			System.Windows.Forms.ContextMenu contextMenu = new System.Windows.Forms.ContextMenu();
			System.Windows.Forms.MenuItem menuItem1 = new System.Windows.Forms.MenuItem();
			menuItem1.Index = 0;
			menuItem1.Text = BusEngine.Localization.GetLanguageStatic("button_exit");
			menuItem1.Click += new System.EventHandler((o, e) => {
				BusEngine.Engine.Shutdown();
			});

			System.Windows.Forms.MenuItem menuItem2 = new System.Windows.Forms.MenuItem();
			menuItem2.Index = 1;
			menuItem2.Text = BusEngine.Localization.GetLanguageStatic("button_donate");
			menuItem2.Click += new System.EventHandler((o, e) => {
				System.Diagnostics.Process.Start("Url");
			});

			contextMenu.MenuItems.AddRange(new System.Windows.Forms.MenuItem[] {menuItem1, menuItem2});

			System.Windows.Forms.NotifyIcon notifyIcon = new System.Windows.Forms.NotifyIcon(notifyIconComponents);
			notifyIcon.Icon = BusEngine.UI.Canvas.WinForm.Icon;
			notifyIcon.ContextMenu = contextMenu;
			notifyIcon.Text = BusEngine.UI.Canvas.WinForm.Text;
			notifyIcon.Visible = true;
			BusEngine.UI.Canvas.WinForm.FormClosing += (o, e) => {
				if (notifyIconComponents != null) {
					notifyIconComponents.Dispose();
				}
			}; */

			// запускаем браузер
			Browser("index.html");
		}

		/* private void OnDisposed(object Sender, System.EventArgs e) {
			if (notifyIconComponents != null) {
				notifyIconComponents.Dispose();
			}
		} */

		private static void Paint3(object sender, System.Windows.Forms.PaintEventArgs e) {
			// труба (прямоугольник)
			e.Graphics.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Black), 480, 125, 75, 75);
		}

		// запускаем браузер WinForm только в одном потоке =(
		private void Browser(string url) {
			int w = BusEngine.UI.Canvas.WinForm.Width,
			h = BusEngine.UI.Canvas.WinForm.Height,
			x = BusEngine.UI.Canvas.WinForm.DesktopLocation.X,
			y = BusEngine.UI.Canvas.WinForm.DesktopLocation.Y;

			BusEngine.Browser.Initialize(url);
			BusEngine.Browser.OnPostMessageStatic += (string message) => {
				string t = message.ToLower();

				if (t == "collapse") {
					BusEngine.UI.Canvas.WinForm.WindowState = System.Windows.Forms.FormWindowState.Minimized;
				} else if (t == "expand") {
					if (BusEngine.UI.Canvas.WinForm.WindowState == System.Windows.Forms.FormWindowState.Maximized) {
						BusEngine.UI.Canvas.WinForm.WindowState = System.Windows.Forms.FormWindowState.Normal;
					} else {
						BusEngine.UI.Canvas.WinForm.WindowState = System.Windows.Forms.FormWindowState.Maximized;
					}
				} else if (t == "exit") {
					BusEngine.Engine.Shutdown();
				} else if (t == "debug") {
					BusEngine.Log.Info("JavaScript: Привет CSharp!");
					BusEngine.Log.Info("На команду: " + message);
					BusEngine.Browser.ExecuteJSStatic("document.dispatchEvent(new CustomEvent('BusEngineMessage', {bubbles: true, detail: {hi: 'CSharp: Прювэт JavaScript!', data: 'Получил твою команду! Вось яна: " + message + "'}}));");
				} else {
					t = t.Substring(0, 8);
					if (t == "console|") {
						BusEngine.Log.Info("Console: {0}", message.Substring(8));
					} else if (t == "_linkgo|") {
						BusEngine.Log.Info("_linkGo: {0}", message.Substring(8));
						System.Diagnostics.Process.Start(message.Substring(8));
					} else if (t == "_resize|") {
						string[] xy = message.Substring(8).Split(' ');
						if (xy.Length > 0) {
							int nx, ny, cursor, jl, jx, jt, jy;

							nx = System.Convert.ToInt32(xy[0]);
							ny = 0;
							cursor = 0;
							jl = BusEngine.UI.Canvas.WinForm.Left;
							jx = BusEngine.UI.Canvas.WinForm.Width;

							jt = BusEngine.UI.Canvas.WinForm.Top;
							jy = BusEngine.UI.Canvas.WinForm.Height;

							if (xy.Length > 1) {
								ny = System.Convert.ToInt32(xy[1]);
							}

							if (xy.Length > 2) {
								cursor = System.Convert.ToInt32(xy[2]);
							}

							// влево
							if (cursor == 1) {
								if (w <= (BusEngine.UI.Canvas.WinForm.Width + (nx * -1))) {
									jl = BusEngine.UI.Canvas.WinForm.Left + nx;
									jx = BusEngine.UI.Canvas.WinForm.Width + (nx * -1);
								}
							// левый-верхний
							} else if (cursor == 2) {
								if (w <= (BusEngine.UI.Canvas.WinForm.Width + (nx * -1))) {
									jl = BusEngine.UI.Canvas.WinForm.Left + nx;
									jx = BusEngine.UI.Canvas.WinForm.Width + (nx * -1);
								}

								if (h <= (BusEngine.UI.Canvas.WinForm.Height + (ny * -1))) {
									jt = BusEngine.UI.Canvas.WinForm.Top + ny;
									jy = BusEngine.UI.Canvas.WinForm.Height + (ny * -1);
								}
							// вверх
							} else if (cursor == 3) {
								if (h <= (BusEngine.UI.Canvas.WinForm.Height + (ny * -1))) {
									jt = BusEngine.UI.Canvas.WinForm.Top + ny;
									jy = BusEngine.UI.Canvas.WinForm.Height + (ny * -1);
								}
							// правый-верхний
							} else if (cursor == 4) {
								if (w <= (BusEngine.UI.Canvas.WinForm.Width + nx)) {
									jx = BusEngine.UI.Canvas.WinForm.Width + nx;
								}

								if (h <= (BusEngine.UI.Canvas.WinForm.Height + (ny * -1))) {
									jt = BusEngine.UI.Canvas.WinForm.Top + ny;
									jy = BusEngine.UI.Canvas.WinForm.Height + (ny * -1);
								}
							// вправо
							} else if (cursor == 5) {
								if (w <= (BusEngine.UI.Canvas.WinForm.Width + nx)) {
									jx = BusEngine.UI.Canvas.WinForm.Width + nx;
								}
							// правый-нижний
							} else if (cursor == 6) {
								if (w <= (BusEngine.UI.Canvas.WinForm.Width + nx)) {
									jx = BusEngine.UI.Canvas.WinForm.Width + nx;
								}

								if (h <= (BusEngine.UI.Canvas.WinForm.Height + ny)) {
									jy = BusEngine.UI.Canvas.WinForm.Height + ny;
								}
							// вниз
							} else if (cursor == 7) {
								if (h <= (BusEngine.UI.Canvas.WinForm.Height + ny)) {
									jy = BusEngine.UI.Canvas.WinForm.Height + ny;
								}
							// левый-нижний
							} else if (cursor == 8) {
								if (w <= (BusEngine.UI.Canvas.WinForm.Width + (nx * -1))) {
									jl = BusEngine.UI.Canvas.WinForm.Left + nx;
									jx = BusEngine.UI.Canvas.WinForm.Width + (nx * -1);
								}

								if (h <= (BusEngine.UI.Canvas.WinForm.Height + ny)) {
									jy = BusEngine.UI.Canvas.WinForm.Height + ny;
								}
							}

							if (cursor > 0) {
								BusEngine.UI.Canvas.WinForm.Bounds = new System.Drawing.Rectangle(jl, jt, jx, jy);
							}
						}
					} else if (t == "__point|") {
						string[] xy = message.Substring(8).Split(' ');

						if (xy.Length > 0) {
							int nx, ny;

							nx = System.Convert.ToInt32(xy[0]);
							ny = 0;

							if (xy.Length > 1) {
								ny = System.Convert.ToInt32(xy[1]);
							}

							if (BusEngine.UI.Canvas.WinForm.WindowState == System.Windows.Forms.FormWindowState.Maximized) {
								BusEngine.UI.Canvas.WinForm.WindowState = System.Windows.Forms.FormWindowState.Normal;
							}

							BusEngine.UI.Canvas.WinForm.Bounds = new System.Drawing.Rectangle(BusEngine.UI.Canvas.WinForm.Location.X + nx, BusEngine.UI.Canvas.WinForm.Location.Y + ny, BusEngine.UI.Canvas.WinForm.Width, BusEngine.UI.Canvas.WinForm.Height);
						}
					}
				}
			};
		}
	}
	/** API BusEngine.Plugin */
}
/** API BusEngine.Game - пользовательский код */