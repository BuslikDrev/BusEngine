/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

/* C# 6.0+              https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history */
/* NET.Framework 4.7.1+ https://learn.microsoft.com/ru-ru/dotnet/framework/migration-guide/versions-and-dependencies */
/* MSBuild 15.0+        https://en.wikipedia.org/wiki/MSBuild#Versions */

/** дорожная карта
- написать запуск игры BusEngine
*/

/** важные ссылки примеров
https://www.cyberforum.ru/blogs/529033/blog5215.html
*/

#define BUSENGINE_WINFORMS
#define BUSENGINE_WINDOWS
/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.ProjectSettingDefault
BusEngine.Engine
BusEngine.Log
BusEngine.UI
*/
	// https://learn.microsoft.com/ru-ru/dotnet/csharp/language-reference/preprocessor-directives
	// https://learn.microsoft.com/ru-ru/dotnet/csharp/programming-guide/classes-and-structs/constants
	public class Global {
		//public const bool BUSENGINE_WINFORM = true;
	}

	internal class Initialize {
        //private static System.Threading.Mutex Mutex;
		private static void Run() {
			// инициализируем API BusEngine
			BusEngine.Engine.Platform = "Windows";
			BusEngine.Engine.Initialize();

			// допускаем только один запуск
			/* bool createdNew;
			Mutex = new System.Threading.Mutex(true, "2b3001ad-2d9b-43a9-82cd-8a6465e1cc5d", out createdNew);
			if (!createdNew) {
				string title;
				string desc;

				if (BusEngine.Localization.GetLanguage("error_warning") != "error_warning") {
					title = BusEngine.Localization.GetLanguage("error_warning");
				} else {
					title = "Увага!";
				}
				if (BusEngine.Localization.GetLanguage("error_is_already_running") != "error_is_already_running") {
					desc = BusEngine.Localization.GetLanguage("error_is_already_running");
				} else {
					desc = "Праграма ўжо запушчана.";
				}

				System.Windows.Forms.MessageBox.Show(desc, title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);

				System.Windows.Forms.Application.Exit();

				return;
			} */

			// создаём форму System.Windows.Forms
			BusEngine.Form form = new BusEngine.Form();

			// устанавливаем нашу иконку
			if (System.IO.File.Exists(BusEngine.Engine.DataDirectory + "Icons/BusEngine.ico")) {
				form.Icon = new System.Drawing.Icon(System.IO.Path.Combine(BusEngine.Engine.DataDirectory, "Icons/BusEngine.ico"), 128, 128);
			}

			// устанавливаем размеры окна
			if (BusEngine.Engine.SettingEngine["console_commands"]["r_Width"] != null) {
				form.Width = System.Convert.ToInt32(BusEngine.Engine.SettingEngine["console_commands"]["r_Width"]);
			}
			if (BusEngine.Engine.SettingEngine["console_commands"]["r_Height"] != null) {
				form.Height = System.Convert.ToInt32(BusEngine.Engine.SettingEngine["console_commands"]["r_Height"]);
			}

			// открываем окно на весь экран
			if (System.Convert.ToInt32(BusEngine.Engine.SettingEngine["console_commands"]["r_Fullscreen"]) > 0) {
				form.WindowState = System.Windows.Forms.FormWindowState.Maximized;
			}

			// убираем линии, чтобы окно было полностью на весь экран
			if (System.Convert.ToInt32(BusEngine.Engine.SettingEngine["console_commands"]["r_Fullscreen"]) == -1 || System.Convert.ToInt32(BusEngine.Engine.SettingEngine["console_commands"]["r_Fullscreen"]) == 1) {
				form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			} else if (System.Convert.ToInt32(BusEngine.Engine.SettingEngine["console_commands"]["r_Fullscreen"]) == -2) {
				form.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Sizable;
				form.MaximizeBox = true;
			}

			// подключаем API BusEngine.UI.Canvas
			BusEngine.UI.Canvas.WinForm = form;

			// инициализируем API BusEngine.UI.Canvas
			BusEngine.UI.Canvas.Initialize();

			/* System.Windows.Forms.Application.EnableVisualStyles();
			System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false); */

			// тест графики
			// https://rsdn.org/article/gdi/gdiplus2mag.xml
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint2);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint3);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint4);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint5);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint6);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint7);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint8);

			/* BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint2);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint3);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint4);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint5);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint6);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint7);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint8);

			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint2);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint3);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint4);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint5);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint6);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint7);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint8);

			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint2);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint3);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint4);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint5);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint6);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint7);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint8);

			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint2);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint3);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint4);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint5);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint6);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint7);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint8); */
			BusEngine.UI.Canvas.WinForm.MouseMove += new System.Windows.Forms.MouseEventHandler(MouseMove);

			// зависимость от времени
			System.Timers.Timer aTimer = new System.Timers.Timer(1000/FPSSetting);
			// Hook up the Elapsed event for the timer. 
			aTimer.Elapsed += OnTimedEvent;
			aTimer.AutoReset = true;
			aTimer.Enabled = true;

			// FPS
			System.Timers.Timer fpsTimer = new System.Timers.Timer(1000);
			// Hook up the Elapsed event for the timer. 
			fpsTimer.Elapsed += OnFPSTimer;
			fpsTimer.AutoReset = true;
			fpsTimer.Enabled = true;

			// запускаем приложение System.Windows.Forms
			System.Windows.Forms.Application.Run(form);
		}

		/** функция запуска приложения */
		//[System.STAThread] // если однопоточное приложение
		private static void Main(string[] args) {
			/** моя мечта
			if (WINXP) {
				System.Windows.Forms.Form form = new System.Windows.Forms.Form();
				BusEngine.UI.Canvas(form);
				Android.App.LoadApplication(form);
			} else if (ANDROID) {
				Xamarin.Forms.Application form = new Xamarin.Forms.Application();
				BusEngine.UI.Canvas(form);
				Xamarin.Forms.LoadApplication(form);
			} else {
				System.Windows.Application form = new System.Windows.Application();
				BusEngine.UI.Canvas(form);
				System.Windows.Application.Run(form);
			}
			*/

			// проверяем целостность библиотек движка
			if (!System.IO.File.Exists(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location) + "\\BusEngine.dll")) {
				string title;
				string desc;

				if (System.Globalization.CultureInfo.CurrentCulture.EnglishName == "English") {
					title = "Memory Manager";
					desc = "Memory Manager: Unable to bind memory management functions. Cloud not access BusEngine.dll (check working directory)";
				} else if (System.Globalization.CultureInfo.CurrentCulture.EnglishName == "Russian") {
					title = "Диспетчер памяти";
					desc = "Диспетчер памяти: невозможно связать функции управления памятью. Облако не имеет доступа к BusEngine.dll (проверьте рабочий каталог)";
				} else if (System.Globalization.CultureInfo.CurrentCulture.EnglishName == "Ukrainian") {
					title = "Менеджер пам'яті";
					desc = "Менеджер пам'яті: не можна зв'язати функції керування пам'яттю. Хмара не має доступу до BusEngine.dll (перевірте робочий каталог)";
				} else {
					title = "Дыспетчар памяці";
					desc = "Дыспетчар памяці: немагчыма звязаць функцыі кіравання памяццю. Воблака не мае доступу да BusEngine.dll (праверце працоўны каталог)";
				}

				System.Windows.Forms.MessageBox.Show(desc, title, System.Windows.Forms.MessageBoxButtons.OK, System.Windows.Forms.MessageBoxIcon.Exclamation);

				System.Windows.Forms.Application.Exit();

				return;
			} else {
				Run();
			}
		}
		/** функция запуска приложения */

		/* private static void OnPostMessage(object sender, CefSharp.JavascriptMessageReceivedEventArgs e) {
			BusEngine.Log.Info("браузер клик");
			string windowSelection = (string)e.Message;
			if (windowSelection == "Log") {
				BusEngine.Log.Info("============== Log");
			}
		} */

		// Настройки
		private static float count = 0;
		private static float speed = 5;
		private static bool nap = true;
		private static int count2 = 0;
		private static int FPS = 0;
		private static int FPSSetting = 70;
		private static int FPSInfo = 0;

		// Создаем объекты-кисти для закрашивания фигур
		private static System.Drawing.SolidBrush myTrub = new System.Drawing.SolidBrush(System.Drawing.Color.DeepPink);
		private static System.Drawing.SolidBrush myCorp = new System.Drawing.SolidBrush(System.Drawing.Color.DarkMagenta);
		private static System.Drawing.SolidBrush myTrum = new System.Drawing.SolidBrush(System.Drawing.Color.DarkOrchid);
		private static System.Drawing.SolidBrush mySeа = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
		//Выбираем перо myPen желтого цвета толщиной в 2 пикселя:
		private static System.Drawing.Pen myWind = new System.Drawing.Pen(System.Drawing.Color.Yellow, 1);

		private static void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
			//BusEngine.UI.Canvas.WinForm.Refresh();
			BusEngine.UI.Canvas.WinForm.Invalidate();
		}

		private static void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e) {
			//BusEngine.Log.Info("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
			BusEngine.Log.Info("FPS ============== FPS Setting " + FPSSetting);
			BusEngine.Log.Info("FPS ============== FPS " + FPSInfo);
			BusEngine.UI.Canvas.WinForm.Invalidate();
		}

		private static void OnFPSTimer(object source, System.Timers.ElapsedEventArgs e) {
			FPSInfo = FPS;
			FPS = 0;
		}

		private static void Paint(object sender, System.Windows.Forms.PaintEventArgs e) {
			FPS++;

			if (count < 10 || count < 300 && nap == true) {
				nap = true;
			} else {
				nap = false;
			}

			if (nap == true) {
				count += speed;
			} else {
				count -= speed;
			}

			if (count/3 == System.Convert.ToInt32(count/3)) {
				count2++;
			}

			BusEngine.Log.Info("Paint ============== Paint " + count + " " + count2);

			// фон
			//e.Graphics.Clear(System.Drawing.Color.Turquoise);
		}

		private static void Paint2(object sender, System.Windows.Forms.PaintEventArgs e) {
			// https://learn.microsoft.com/ru-ru/dotnet/desktop/winforms/advanced/antialiasing-with-lines-and-curves?view=netframeworkdesktop-4.8
			e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
			// труба (прямоугольник)
			e.Graphics.FillRectangle(myTrub, 300 + count, 125, 75, 75);
		}

		private static void Paint3(object sender, System.Windows.Forms.PaintEventArgs e) {
			// труба (прямоугольник)
			e.Graphics.FillRectangle(myTrub, 480 + count, 125, 75, 75);
		}

		private static void Paint4(object sender, System.Windows.Forms.PaintEventArgs e) {
			// палуба (прямоугольник)
			e.Graphics.FillRectangle(myTrum, 250 + count, 200, 350, 100);
		}

		private static void Paint5(object sender, System.Windows.Forms.PaintEventArgs e) {
			// Иллюминаторы
			// 6 окружностей
			e.Graphics.DrawEllipse(myWind, 300 + count, 240, 20, 20);
			e.Graphics.DrawEllipse(myWind, 350 + count, 240, 20, 20);
			e.Graphics.DrawEllipse(myWind, 400 + count, 240, 20, 20);
			e.Graphics.DrawEllipse(myWind, 450 + count, 240, 20, 20);
			e.Graphics.DrawEllipse(myWind, 500 + count, 240, 20, 20);
			e.Graphics.DrawEllipse(myWind, 550 + count, 240, 20, 20);
		}

		private static void Paint6(object sender, System.Windows.Forms.PaintEventArgs e) {
			// корпус (трапеция)
			e.Graphics.FillPolygon(
				myCorp, 
				new System.Drawing.Point[] {
					new System.Drawing.Point(100 + (int)count, 300),
					new System.Drawing.Point(700 + (int)count, 300),
					new System.Drawing.Point(700 + (int)count, 300),
					new System.Drawing.Point(600 + (int)count, 400),
					new System.Drawing.Point(600 + (int)count, 400),
					new System.Drawing.Point(200 + (int)count, 400),
					new System.Drawing.Point(200 + (int)count, 400),
					new System.Drawing.Point(100 + (int)count, 300)
				}
			);
		}

		private static void Paint7(object sender, System.Windows.Forms.PaintEventArgs e) {
			// Море - 12 секторов-полуокружностей
			int xx = 50;
			int Radius = 50;
			while (xx <= BusEngine.UI.Canvas.WinForm.Width - Radius) {
				e.Graphics.FillPie(mySeа, 0 + xx, 375, 50, 50, 0, -180); 
				xx += 50;
			}
		}

		private static void Paint8(object sender, System.Windows.Forms.PaintEventArgs e) {
			// Translate transformation matrix.
			e.Graphics.TranslateTransform(0, 0);

			// Save translated graphics state.
			//System.Drawing.Drawing2D.GraphicsState transState = e.Graphics.Save();

			// Reset transformation matrix to identity and fill rectangle.
			e.Graphics.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Red), 0, 0, 100, 100);

			// Restore graphics state to translated state and fill second
			//e.Graphics.Restore(transState);
			e.Graphics.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Green), 100, 0, 100, 100);
			e.Graphics.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Blue), 200, 0, 100, 100);

			// rectangle.
			//if (count/3 == System.Convert.ToInt32(count/3)) {
				//ScaleTransformFloat(e);
			//}
		}

		private static void ScaleTransformFloat(System.Windows.Forms.PaintEventArgs e) {
			// Set world transform of graphics object to rotate.
			e.Graphics.RotateTransform(30.0F);

			// Then to scale, prepending to world transform.
			e.Graphics.ScaleTransform(3.0F, 1.0F);

			// Draw scaled, rotated rectangle to screen.
			e.Graphics.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Blue, 3), 50, 0, 100, 40);
		}
	}

	// https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.forms.form?view=netframework-4.8
	internal class Form : System.Windows.Forms.Form {
		/** функция запуска окна приложения */
		public Form() {
			// название окна
			this.Text = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name + " BusEngine v" + System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();

			// системная иконка
			this.Icon = new System.Drawing.Icon(System.Drawing.SystemIcons.Exclamation, 128, 128);

			// устанавливаем размеры окна
			this.Width = 900;
			this.Height = 540;

			// центрируем окно
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;

			// открываем окно на весь экран
			//this.WindowState = System.Windows.Forms.FormWindowState.Maximized;

			// устанавливаем стиль границ окна
			//this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;

			// убираем кнопку развернуть
			//this.MaximizeBox = false;

			// убираем кнопку свернуть
			//this.MinimizeBox = false;

			// устанавливаем чёрный цвет фона окна
			this.BackColor = System.Drawing.Color.Black;

			// устанавливаем событие нажатий клавиш
			this.KeyPreview = true;
			//this.KeyDown += OnKeyDown;

			// устанавливаем событие закрытия окна
			//this.FormClosed += OnClosed;
			//this.Disposed += new System.EventHandler(OnDisposed);
			//ClientSize = this.ClientSize;

			// показываем форму\включаем\запускаем\стартуем показ окна
			//this.ShowDialog();
		}

		//public static void Run(BusEngine.UI.WinForm winform = null) {
			//if (winform != null) {
				//_winform = winform;
			//}
		//}

		//private const int WM_ACTIVATEAPP = 0x001C;
		//private bool appActive = true;

		/* protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
			if (appActive) {
				e.Graphics.FillRectangle(System.Drawing.SystemBrushes.ActiveCaption, 20, 20, 260, 50);
				e.Graphics.DrawString("Application is active", Font, System.Drawing.SystemBrushes.ActiveCaptionText, 20, 20);
			} else {
				e.Graphics.FillRectangle(System.Drawing.SystemBrushes.InactiveCaption, 20, 20, 260, 50);
				e.Graphics.DrawString("Application is Inactive", Font, System.Drawing.SystemBrushes.ActiveCaptionText, 20, 20);
			}
			
			e.Graphics.Clear(System.Drawing.Color.Turquoise);
			// Создаем объекты-кисти для закрашивания фигур
			System.Drawing.SolidBrush myCorp = new System.Drawing.SolidBrush(System.Drawing.Color.DarkMagenta);
			System.Drawing.SolidBrush myTrum = new System.Drawing.SolidBrush(System.Drawing.Color.DarkOrchid);
			System.Drawing.SolidBrush myTrub = new System.Drawing.SolidBrush(System.Drawing.Color.DeepPink);
			System.Drawing.SolidBrush mySeа = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
			//Выбираем перо myPen желтого цвета толщиной в 2 пикселя:
			System.Drawing.Pen myWind = new System.Drawing.Pen(System.Drawing.Color.Yellow, 2);
			// Закрашиваем фигуры
			e.Graphics.FillRectangle(myTrub,300,125,75,75); // 1 труба (прямоугольник)
			e.Graphics.FillRectangle(myTrub,480,125,75,75); // 2 труба (прямоугольник)
			e.Graphics.FillPolygon(
				myCorp, 
				new System.Drawing.Point[] {
					new System.Drawing.Point(100,300),new System.Drawing.Point(700,300),
					new System.Drawing.Point(700,300),new System.Drawing.Point(600,400),
					new System.Drawing.Point(600,400),new System.Drawing.Point(200,400),
					new System.Drawing.Point(200,400),new System.Drawing.Point(100,300)
				}
			); // корпус (трапеция)
			e.Graphics.FillRectangle(myTrum, 250, 200, 350, 100); // палуба (прямоугольник)
			// Море - 12 секторов-полуокружностей
			int xx = 50;
			int Radius = 50;
			while (xx <= BusEngine.UI.Canvas.WinForm.Width - Radius) {
				e.Graphics.FillPie(mySeа, 0 + xx, 375, 50, 50, 0, -180); 
				xx += 50;
			}
			// Иллюминаторы 
			for (int yy = 300; yy <= 550; yy += 50) {
				e.Graphics.DrawEllipse(myWind, yy, 240, 20, 20); // 6 окружностей
			}

			e.Graphics.Dispose();
		} */

		/* protected override void WndProc(ref System.Windows.Forms.Message m) {
			switch (m.Msg) {
				case WM_ACTIVATEAPP:
					appActive = (((int)m.WParam != 0));
					Invalidate();

					break;
			}
			base.WndProc(ref m);
		} */

		/* protected override void Callback(System.IntPtr hWnd, System.Int32 msg, System.IntPtr wparam, System.IntPtr lparam) {
			switch (m.Msg) {
				case WM_ACTIVATEAPP:
					appActive = (((int)m.WParam != 0));
					Invalidate();

					break;
			}
			base.WndProc(ref m);
		} */
		/** функция запуска окна приложения */
	}
}
/** API BusEngine */