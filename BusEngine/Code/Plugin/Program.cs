/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2026; BuslikDrev - Усе правы захаваны. */

/** API BusEngine.Game - пользовательский код */
namespace BusEngine.Game {
	/** API BusEngine.Plugin */
	public class MyPlugin : BusEngine.Plugin {
		//private static System.Windows.Forms.Form SplashScreen;
		//private System.ComponentModel.IContainer notifyIconComponents;

		// при запуске BusEngine до создания формы
		public override void Initialize() {
			//BusEngine.Log.Info("Commands: {0}", BusEngine.Tools.Json.Encode(BusEngine.Engine.Commands));

			// устанавливаем настройки
			//BusEngine.Engine.SettingProject["console_commands"]["r_Width"] = "1280";
			//BusEngine.Engine.SettingProject["console_commands"]["r_Height"] = "720";
			BusEngine.Engine.SettingProject["console_commands"]["r_FullScreen"] = "-1";
			string launcher_msbuild;
			if (!BusEngine.Engine.SettingProject["console_commands"].TryGetValue("launcher_msbuild", out launcher_msbuild)) {
				if (BusEngine.Engine.Device.Data["CPU"][0]["Architecture"] == "x64") {
					launcher_msbuild = BusEngine.Engine.SettingProject["console_commands"]["launcher_msbuild"] = "C:/Windows/Microsoft.NET/Framework64/v4.0.30319/MSBuild.exe";
				} else {
					launcher_msbuild = BusEngine.Engine.SettingProject["console_commands"]["launcher_msbuild"] = "C:/Windows/Microsoft.NET/Framework/v4.0.30319/MSBuild.exe";
				}
			}

			BusEngine.Log.Info(launcher_msbuild);

			// загружаем свой язык
			BusEngine.Localization.OnLoadStatic += (BusEngine.Localization l, string language) => {
				#if LOCALIZATION_LOG
				//BusEngine.Log.Info("Язык изменился: {0}", language);
				//BusEngine.Log.Info("Язык изменился: {0}", l.GetLanguage("error"));
				#endif
			};
			new BusEngine.Localization().Load("Ukrainian");
			new BusEngine.Localization().Load("English");
			new BusEngine.Localization().Load("Russian");
			// устанавливаем язык системы в противном случае будет язык по умолчанию.
			new BusEngine.Localization().Load(System.Globalization.CultureInfo.CurrentCulture.EnglishName.Split(' ')[0]);

			//https://learn.microsoft.com/ru-ru/dotnet/api/system.globalization.cultureinfo?view=netframework-4.8
			/* BusEngine.Log.Info(System.Globalization.CultureInfo.CurrentCulture.EnglishName.Split(' ')[0]);
			System.Globalization.CultureInfo[] specificCultures = System.Globalization.CultureInfo.GetCultures(System.Globalization.CultureTypes.SpecificCultures);
            foreach (System.Globalization.CultureInfo ci in specificCultures) {
                BusEngine.Log.Info(ci.EnglishName.Split(' ')[0] + " | " + ci.Name);
			} */

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

			//System.Threading.Tasks.Task.Delay(2000).Wait();
		}

		// при запуске BusEngine после создания формы Canvas
		public override void InitializeСanvas() {
			// убираем стартовую обложку
			/* SplashScreen.Close();
			SplashScreen.Dispose(); */

			//BusEngine.UI.Canvas.WinForms.TransparencyKey = System.Drawing.Color.Black;
			//BusEngine.UI.Canvas.WinForms.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			// деламем окно не поверх других окон
			BusEngine.UI.Canvas.WinForms.TopMost = false;

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
			notifyIcon.Icon = BusEngine.UI.Canvas.WinForms.Icon;
			notifyIcon.ContextMenu = contextMenu;
			notifyIcon.Text = BusEngine.UI.Canvas.WinForms.Text;
			notifyIcon.Visible = true;
			BusEngine.UI.Canvas.WinForms.FormClosing += (o, e) => {
				if (notifyIconComponents != null) {
					notifyIconComponents.Dispose();
				}
			}; */

			// запускаем браузер
			Browser("index.html");
		}

		// запускаем браузер
		private static BusEngine.Browser _browser;
		private void Browser(string url) {
			int w = BusEngine.UI.Canvas.WinForms.Width,
			h = BusEngine.UI.Canvas.WinForms.Height,
			x = BusEngine.UI.Canvas.WinForms.DesktopLocation.X,
			y = BusEngine.UI.Canvas.WinForms.DesktopLocation.Y;

			// Google Speech API
			System.Environment.SetEnvironmentVariable("google_api_key", "123");
			System.Environment.SetEnvironmentVariable("google_default_client_id", "123.apps.googleusercontent.com");
			System.Environment.SetEnvironmentVariable("google_default_client_secret", "123-123");

			_browser = new BusEngine.Browser(url);

			//https://github.com/cefsharp/CefSharp/discussions/4404
			//https://github.com/cefsharp/CefSharp/discussions/4477
			_browser.OnPostMessage += (BusEngine.Browser browser, string message) => {
				string t = message.ToLower();

				if (t == "collapse") {
					BusEngine.UI.Canvas.WinForms.WindowState = System.Windows.Forms.FormWindowState.Minimized;
				} else if (t == "expand") {
					if (BusEngine.UI.Canvas.WinForms.WindowState == System.Windows.Forms.FormWindowState.Maximized) {
						BusEngine.UI.Canvas.WinForms.WindowState = System.Windows.Forms.FormWindowState.Normal;
					} else {
						BusEngine.UI.Canvas.WinForms.WindowState = System.Windows.Forms.FormWindowState.Maximized;
					}
				} else if (t == "exit") {
					BusEngine.Engine.Shutdown();
				} else if (t == "debug") {
					//BusEngine.Log.Info("JavaScript: Привет CSharp!");
					//BusEngine.Log.Info("На команду: " + message);
					_browser.ExecuteJS("document.dispatchEvent(new CustomEvent('BusEngineMessage', {bubbles: true, detail: {hi: 'CSharp: Прювэт JavaScript!', data: 'Получил твою команду! Вось яна: " + message + "'}}));");
				} else {
					t = t.Substring(0, 8);

					if (t == "console|") {
						//BusEngine.Log.Info("Console: {0}", message.Substring(8));
					} else if (t == "_linkgo|") {
						//BusEngine.Log.Info("_linkGo: {0}", message.Substring(8));
						System.Diagnostics.Process.Start(message.Substring(8));
					} else if (t == "_resize|") {
						string[] xy = message.Substring(8).Split(' ');
						if (xy.Length > 0) {
							int nx, ny, cursor, jl, jx, jt, jy;

							nx = System.Convert.ToInt32(xy[0]);
							ny = 0;
							cursor = 0;
							jl = BusEngine.UI.Canvas.WinForms.Left;
							jx = BusEngine.UI.Canvas.WinForms.Width;

							jt = BusEngine.UI.Canvas.WinForms.Top;
							jy = BusEngine.UI.Canvas.WinForms.Height;

							if (xy.Length > 1) {
								ny = System.Convert.ToInt32(xy[1]);
							}

							if (xy.Length > 2) {
								cursor = System.Convert.ToInt32(xy[2]);
							}

							// влево
							if (cursor == 1) {
								if (w <= (BusEngine.UI.Canvas.WinForms.Width + (nx * -1))) {
									jl = BusEngine.UI.Canvas.WinForms.Left + nx;
									jx = BusEngine.UI.Canvas.WinForms.Width + (nx * -1);
								}
							// левый-верхний
							} else if (cursor == 2) {
								if (w <= (BusEngine.UI.Canvas.WinForms.Width + (nx * -1))) {
									jl = BusEngine.UI.Canvas.WinForms.Left + nx;
									jx = BusEngine.UI.Canvas.WinForms.Width + (nx * -1);
								}

								if (h <= (BusEngine.UI.Canvas.WinForms.Height + (ny * -1))) {
									jt = BusEngine.UI.Canvas.WinForms.Top + ny;
									jy = BusEngine.UI.Canvas.WinForms.Height + (ny * -1);
								}
							// вверх
							} else if (cursor == 3) {
								if (h <= (BusEngine.UI.Canvas.WinForms.Height + (ny * -1))) {
									jt = BusEngine.UI.Canvas.WinForms.Top + ny;
									jy = BusEngine.UI.Canvas.WinForms.Height + (ny * -1);
								}
							// правый-верхний
							} else if (cursor == 4) {
								if (w <= (BusEngine.UI.Canvas.WinForms.Width + nx)) {
									jx = BusEngine.UI.Canvas.WinForms.Width + nx;
								}

								if (h <= (BusEngine.UI.Canvas.WinForms.Height + (ny * -1))) {
									jt = BusEngine.UI.Canvas.WinForms.Top + ny;
									jy = BusEngine.UI.Canvas.WinForms.Height + (ny * -1);
								}
							// вправо
							} else if (cursor == 5) {
								if (w <= (BusEngine.UI.Canvas.WinForms.Width + nx)) {
									jx = BusEngine.UI.Canvas.WinForms.Width + nx;
								}
							// правый-нижний
							} else if (cursor == 6) {
								if (w <= (BusEngine.UI.Canvas.WinForms.Width + nx)) {
									jx = BusEngine.UI.Canvas.WinForms.Width + nx;
								}

								if (h <= (BusEngine.UI.Canvas.WinForms.Height + ny)) {
									jy = BusEngine.UI.Canvas.WinForms.Height + ny;
								}
							// вниз
							} else if (cursor == 7) {
								if (h <= (BusEngine.UI.Canvas.WinForms.Height + ny)) {
									jy = BusEngine.UI.Canvas.WinForms.Height + ny;
								}
							// левый-нижний
							} else if (cursor == 8) {
								if (w <= (BusEngine.UI.Canvas.WinForms.Width + (nx * -1))) {
									jl = BusEngine.UI.Canvas.WinForms.Left + nx;
									jx = BusEngine.UI.Canvas.WinForms.Width + (nx * -1);
								}

								if (h <= (BusEngine.UI.Canvas.WinForms.Height + ny)) {
									jy = BusEngine.UI.Canvas.WinForms.Height + ny;
								}
							}

							if (cursor > 0) {
								BusEngine.UI.Canvas.WinForms.Bounds = new System.Drawing.Rectangle(jl, jt, jx, jy);
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

							if (BusEngine.UI.Canvas.WinForms.WindowState == System.Windows.Forms.FormWindowState.Maximized) {
								BusEngine.UI.Canvas.WinForms.WindowState = System.Windows.Forms.FormWindowState.Normal;
							}

							BusEngine.UI.Canvas.WinForms.Bounds = new System.Drawing.Rectangle(BusEngine.UI.Canvas.WinForms.Location.X + nx, BusEngine.UI.Canvas.WinForms.Location.Y + ny, BusEngine.UI.Canvas.WinForms.Width, BusEngine.UI.Canvas.WinForms.Height);
						}
					} else if (t == "___down|") {
						//BusEngine.Log.Info("___down");
						string path = message.Substring(8);

						_browser.Download(path);
					} else if (t == "__dfile|") {
						//BusEngine.Log.Info("__dfile");
						t = message.Substring(8);

						using (System.Windows.Forms.OpenFileDialog openFileDialog = new System.Windows.Forms.OpenFileDialog()) {
							openFileDialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyComputer);
							openFileDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
							openFileDialog.FilterIndex = 2;
							openFileDialog.RestoreDirectory = true;

							if (openFileDialog.ShowDialog(BusEngine.UI.Canvas.WinForms.TopLevelControl) == System.Windows.Forms.DialogResult.OK) {
								_browser.ExecuteJS("document.querySelector('" + t + "').value = '" + openFileDialog.FileName.Replace("\\", "\\\\") + "';");
							}
						}
					} else if (t == "dfolder|") {
						//BusEngine.Log.Info("dfolder");
						t = message.Substring(8);

						using (BusEngine.Tools.FileFolderDialog folderBrowserDialog = new BusEngine.Tools.FileFolderDialog()) {
							folderBrowserDialog.Dialog.InitialDirectory = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyComputer);

							if (folderBrowserDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
								_browser.ExecuteJS("document.querySelector('" + t + "').value = '" + folderBrowserDialog.SelectedPath.Replace("\\", "\\\\") + "';");
							}
						}
					} else if (t == "___test|") {
						//BusEngine.Log.Info("2 Owner: {0}", BusEngine.UI.Canvas.WinForms.Owner);
						//BusEngine.Log.Info("2 Owner Controls: {0}", BusEngine.UI.Canvas.WinForms.Controls.Owner);
						//BusEngine.Log.Info("2 TopLevelControl: {0}", BusEngine.UI.Canvas.WinForms.TopLevelControl);
						//BusEngine.Log.Info("2 ActiveControl: {0}", BusEngine.UI.Canvas.WinForms.ActiveControl);
						//BusEngine.Log.Info("2 TopLevel: {0}", BusEngine.UI.Canvas.WinForms.TopLevel);

						// https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.forms.panel?view=netframework-4.7.1
						System.Windows.Forms.Panel panel = new System.Windows.Forms.Panel();
						panel.Location = BusEngine.UI.Canvas.WinForms.Location;
						panel.Size = new System.Drawing.Size(100, 100);
						panel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
						BusEngine.UI.Canvas.WinForms.Controls.Add(panel);

						// .NET 5+ = null
						//BusEngine.Log.Info("Control1: {0}", BusEngine.UI.Canvas.WinForms.Controls.Owner);

						// in other code
						// https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.forms.control?view=netframework-4.7.1
						// https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.forms.control.controlcollection?view=netframework-4.7.1
						foreach (System.Windows.Forms.Control g in BusEngine.UI.Canvas.WinForms.Controls) {
							// https://cefsharp.github.io/api/109.1.x/html/T_CefSharp_WinForms_ChromiumWebBrowser.htm
							if (g.ToString() == "CefSharp.WinForms.ChromiumWebBrowser") {
								//BusEngine.Log.Info("2 browser: {0}", g.ToString());
								//BusEngine.Log.Info("2 browser: {0}", g.GetType());
								//BusEngine.Log.Info("2 browser: {0}", g.ClientSize);
								//BusEngine.Log.Info("2 browser TopLevelControl: {0}", g.TopLevelControl);
								//BusEngine.Log.Info("2 browser ActiveControl: {0}", g.GetContainerControl().ActiveControl);
								//BusEngine.Log.Info("2 WinForm TopLevelControl: {0}", BusEngine.UI.Canvas.WinForms.TopLevelControl);
								//BusEngine.Log.Info("2 WinForm: {0}", BusEngine.UI.Canvas.WinForms.ActiveControl);
								//BusEngine.Log.Info("2 WinForm ActiveControl: {0}", BusEngine.UI.Canvas.WinForms.GetContainerControl().ActiveControl);

								BusEngine.UI.Canvas.WinForms.ActiveControl = null;
								g.GetContainerControl().ActiveControl = null;

								//BusEngine.Log.Info("3 browser TopLevelControl: {0}", g.TopLevelControl);
								//BusEngine.Log.Info("3 browser ActiveControl: {0}", g.GetContainerControl().ActiveControl);
								//BusEngine.Log.Info("3 WinForm TopLevelControl: {0}", BusEngine.UI.Canvas.WinForms.TopLevelControl);
								//BusEngine.Log.Info("3 WinForm ActiveControl: {0}", BusEngine.UI.Canvas.WinForms.GetContainerControl().ActiveControl);

								// WORK! =)
								//g.Controls.Add(panel);
								//BusEngine.UI.Canvas.WinForms.Controls.Remove(g);
							} else {
								//BusEngine.Log.Info("other: {0}", g.ToString());
								//BusEngine.Log.Info("other: {0}", g.GetType());
								//BusEngine.Log.Info("other: {0}", g.ClientSize);
							}
						}

						//BusEngine.Log.Info("3 Owner: {0}", BusEngine.UI.Canvas.WinForms.Owner);
						//BusEngine.Log.Info("3 Owner Controls: {0}", BusEngine.UI.Canvas.WinForms.Controls.Owner);
						//BusEngine.Log.Info("3 TopLevelControl: {0}", BusEngine.UI.Canvas.WinForms.TopLevelControl);
						//BusEngine.Log.Info("3 ActiveControl: {0}", BusEngine.UI.Canvas.WinForms.ActiveControl);
						//BusEngine.Log.Info("3 TopLevel: {0}", BusEngine.UI.Canvas.WinForms.TopLevel);
					}
				}
			};

			_browser.OnDownload += (e) => {
				//BusEngine.Log.Info("Скачивание программ: {0}", e.FullPath);
				//BusEngine.Log.Info("Скачивание: {0}", e.GetType().GetProperty("FullPath").GetValue(e, null));
				return e;
			};

			_browser.OnLoad += () => {
				//BusEngine.UI.Canvas.WinForms.Controls.Add(_browser.Context);
				//BusEngine.UI.Canvas.WinForms.Context = _browser.Context;
				//System.Windows.Forms.Application.Run(_browser.Context);
				//BusEngine.Engine.Shutdown();
			};
			_browser.Load();
		}

		// перед закрытием BusEngine
		public override void Shutdown() {
			_browser.Shutdown();
			//BusEngine.UI.Canvas.ShutdownStatic();
		}
	}
	/** API BusEngine.Plugin */
}
/** API BusEngine.Game - пользовательский код */
