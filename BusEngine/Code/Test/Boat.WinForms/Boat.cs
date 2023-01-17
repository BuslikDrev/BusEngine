/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2023; BuslikDrev - Усе правы захаваны. */

/* C# 6.0+              https://learn.microsoft.com/ru-ru/dotnet/csharp/whats-new/csharp-version-history */
/* NET.Framework 4.7.1+ https://learn.microsoft.com/ru-ru/dotnet/framework/migration-guide/versions-and-dependencies */
/* MSBuild 15.0+        https://en.wikipedia.org/wiki/MSBuild#Versions */

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

	internal class Start {
		/** функция запуска приложения */
		//[System.STAThread] // если однопоточное приложение
		private static void Main(string[] args) {
			// инициализируем API BusEngine
			BusEngine.Engine.Platform = "Windows";
			BusEngine.Engine.Initialize();

			// создаём форму System.Windows.Forms
			Form form = new Form();

			// подключаем API BusEngine.UI.Canvas
			BusEngine.UI.Canvas.WinForm = form;
			BusEngine.UI.Canvas.Initialize();

			// запускаем видео
			//BusEngine.Video.Play("Videos/BusEngine.mp4");

			// запускаем браузер
			//BusEngine.Browser.Start("index.html");

			// тест графики
			// https://rsdn.org/article/gdi/gdiplus2mag.xml
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint);
			//BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint1);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint2);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint3);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint4);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint5);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint6);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint7);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint8);
			// подключаем событие мыши
			//BusEngine.UI.Canvas.WinForm.MouseMove += new System.Windows.Forms.MouseEventHandler(MouseMove);

			// зависимость от времени
			System.Timers.Timer aTimer = new System.Timers.Timer(1000/FPSSetting);
			// Hook up the Elapsed event for the timer.
			// подключаем событие времени
			aTimer.Elapsed += OnTimedEvent;
			aTimer.AutoReset = true;
			aTimer.Enabled = true;

			// FPS
			System.Timers.Timer fpsTimer = new System.Timers.Timer(1000);
			// Hook up the Elapsed event for the timer.
			// подключаем событие FPS
			fpsTimer.Elapsed += OnFPS;
			fpsTimer.AutoReset = true;
			fpsTimer.Enabled = true;

			// запускаем приложение System.Windows.Forms
			System.Windows.Forms.Application.Run(form);
		}
		/** функция запуска приложения */

		// настройки
		private static float Count = 0;
		private static bool Nap = true;
		private static int Count2 = 0;
		private static int FPS = 0;
		private static int FPSSetting = 70;
		private static int FPSInfo = 0;

		// создаем объекты-кисти для закрашивания фигур
		private static System.Drawing.SolidBrush myTrub = new System.Drawing.SolidBrush(System.Drawing.Color.DeepPink);
		private static System.Drawing.SolidBrush myCorp = new System.Drawing.SolidBrush(System.Drawing.Color.DarkMagenta);
		private static System.Drawing.SolidBrush myTrum = new System.Drawing.SolidBrush(System.Drawing.Color.DarkOrchid);
		private static System.Drawing.SolidBrush mySeа = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
		// выбираем перо myPen желтого цвета толщиной в 1 пиксель:
		private static System.Drawing.Pen myWind = new System.Drawing.Pen(System.Drawing.Color.Yellow, 1);

		// событие мыши
		private static void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
			BusEngine.Log.Info("FPS ============== FPS Setting " + FPSSetting);
			BusEngine.Log.Info("FPS ============== FPS " + FPSInfo);
			//BusEngine.UI.Canvas.WinForm.Refresh();
			BusEngine.UI.Canvas.WinForm.Invalidate();
		}

		// событие времени
		private static void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e) {
			//BusEngine.Log.Info("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
			BusEngine.Log.Info("FPS ============== FPS Setting " + FPSSetting);
			BusEngine.Log.Info("FPS ============== FPS " + FPSInfo);
			//BusEngine.UI.Canvas.WinForm.Refresh();
			BusEngine.UI.Canvas.WinForm.Invalidate();
		}

		// событие FPS
		private static void OnFPS(object source, System.Timers.ElapsedEventArgs e) {
			FPSInfo = FPS;
			FPS = 0;
		}

		// событие отрисовки - главный подсчёт кадров
		private static void Paint(object sender, System.Windows.Forms.PaintEventArgs e) {
			FPS++;

			if (Count < 10 || Count < 300 && Nap == true) {
				Nap = true;
			} else {
				Nap = false;
			}

			if (Nap == true) {
				Count++;
			} else {
				Count--;
			}

			if (Count/3 == System.Convert.ToInt32(Count/3)) {
				Count2++;
			}

			BusEngine.Log.Info("Paint ============== Paint " + Count + " " + Count2);
		}

		// событие отрисовки - модель 1
		private static void Paint1(object sender, System.Windows.Forms.PaintEventArgs e) {
			// фон
			e.Graphics.Clear(System.Drawing.Color.Turquoise);
		}

		// событие отрисовки - модель 2
		private static void Paint2(object sender, System.Windows.Forms.PaintEventArgs e) {
			// труба (прямоугольник)
			e.Graphics.FillRectangle(myTrub, 300 + Count, 125, 75, 75);
		}

		// событие отрисовки - модель 3
		private static void Paint3(object sender, System.Windows.Forms.PaintEventArgs e) {
			// труба (прямоугольник)
			e.Graphics.FillRectangle(myTrub, 480 + Count, 125, 75, 75);
		}

		// событие отрисовки - модель 4
		private static void Paint4(object sender, System.Windows.Forms.PaintEventArgs e) {
			// палуба (прямоугольник)
			e.Graphics.FillRectangle(myTrum, 250 + Count, 200, 350, 100);
		}

		// событие отрисовки - модель 5
		private static void Paint5(object sender, System.Windows.Forms.PaintEventArgs e) {
			// Иллюминаторы
			// 6 окружностей
			e.Graphics.DrawEllipse(myWind, 300 + Count, 240, 20, 20);
			e.Graphics.DrawEllipse(myWind, 350 + Count, 240, 20, 20);
			e.Graphics.DrawEllipse(myWind, 400 + Count, 240, 20, 20);
			e.Graphics.DrawEllipse(myWind, 450 + Count, 240, 20, 20);
			e.Graphics.DrawEllipse(myWind, 500 + Count, 240, 20, 20);
			e.Graphics.DrawEllipse(myWind, 550 + Count, 240, 20, 20);
		}

		// событие отрисовки - модель 6
		private static void Paint6(object sender, System.Windows.Forms.PaintEventArgs e) {
			// корпус (трапеция)
			e.Graphics.FillPolygon(
				myCorp, 
				new System.Drawing.Point[] {
					new System.Drawing.Point(100 + (int)Count, 300),
					new System.Drawing.Point(700 + (int)Count, 300),
					new System.Drawing.Point(700 + (int)Count, 300),
					new System.Drawing.Point(600 + (int)Count, 400),
					new System.Drawing.Point(600 + (int)Count, 400),
					new System.Drawing.Point(200 + (int)Count, 400),
					new System.Drawing.Point(200 + (int)Count, 400),
					new System.Drawing.Point(100 + (int)Count, 300)
				}
			);
		}

		// событие отрисовки - модель 7
		private static void Paint7(object sender, System.Windows.Forms.PaintEventArgs e) {
			// Море - 12 секторов-полуокружностей
			int xx = 50;
			int Radius = 50;
			while (xx <= BusEngine.UI.Canvas.WinForm.Width - Radius) {
				e.Graphics.FillPie(mySeа, 0 + xx, 375, 50, 50, 0, -180); 
				xx += 50;
			}
		}

		// событие отрисовки - модель 8
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

			// устанавливаем нашу иконку, есди она есть по пути exe, в противном случае устанавливаем системную
			if (System.IO.File.Exists(BusEngine.Engine.DataDirectory + "Icons/BusEngine.ico")) {
				this.Icon = new System.Drawing.Icon(System.IO.Path.Combine(BusEngine.Engine.DataDirectory, "Icons/BusEngine.ico"), 128, 128);
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

			// устанавливаем событие нажатий клавиш
			this.KeyPreview = true;
			//this.KeyDown += OnKeyDown;

			// https://learn.microsoft.com/ru-ru/dotnet/api/system.windows.forms.controlstyles?view=netframework-4.6.2#system-windows-forms-controlstyles-userpaint
			// убираем мерцание и доступна настройка только в этом месте.
			this.SetStyle(System.Windows.Forms.ControlStyles.AllPaintingInWmPaint, true);
			this.SetStyle(System.Windows.Forms.ControlStyles.OptimizedDoubleBuffer, true);
			this.SetStyle(System.Windows.Forms.ControlStyles.FixedHeight, false);
			this.SetStyle(System.Windows.Forms.ControlStyles.FixedWidth, false);

			// устанавливаем событие закрытия окна
			//this.FormClosed += OnClosed;
			//this.Disposed += new System.EventHandler(OnDisposed);
			//ClientSize = this.ClientSize;

			// показываем форму\включаем\запускаем\стартуем показ окна
			//this.ShowDialog();
		}

		/* private const int WM_ACTIVATEAPP = 0x001C;
		private bool appActive = true;

		protected override void OnPaint(System.Windows.Forms.PaintEventArgs e) {
			if (appActive) {
				e.Graphics.FillRectangle(System.Drawing.SystemBrushes.ActiveCaption, 20, 20, 260, 50);
				e.Graphics.DrawString("Application is active", Font, System.Drawing.SystemBrushes.ActiveCaptionText, 20, 20);
			} else {
				e.Graphics.FillRectangle(System.Drawing.SystemBrushes.InactiveCaption, 20, 20, 260, 50);
				e.Graphics.DrawString("Application is Inactive", Font, System.Drawing.SystemBrushes.ActiveCaptionText, 20, 20);
			}
		}

		protected override void WndProc(ref System.Windows.Forms.Message m) {
			switch (m.Msg) {
				case WM_ACTIVATEAPP:
					appActive = (((int)m.WParam != 0));
					this.Invalidate();

					break;
			}
			base.WndProc(ref m);
		} */

		/* protected void Callback(System.IntPtr hWnd, System.Int32 msg, System.IntPtr wparam, System.IntPtr lparam) {
			switch (m.Msg) {
				case WM_ACTIVATEAPP:
					appActive = (((int)m.WParam != 0));
					this.Invalidate();

					break;
			}
			base.WndProc(ref m);
		} */
		/** функция запуска окна приложения */
	}
}
/** API BusEngine */