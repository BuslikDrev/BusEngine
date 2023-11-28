/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2024; BuslikDrev - Усе правы захаваны. */

//https://youtu.be/K8eW4cPIlx0?t=2133

//#define BROWSER_LOG
#define LOCALIZATION_LOG

using System;

/** API BusEngine.Game - пользовательский код */
namespace BusEngine.Game {
	/** API BusEngine.Plugin */
	public class MyPlugin : BusEngine.Plugin {
		// настройки
		private static int Count = 0;
		private static bool Nap = true;
		private static int Count2 = 0;
		private static int FPS = 0;
		private static int FPSSetting = 100;
		private static int FPSInfo = 0;

		private static System.Drawing.SolidBrush myTrub;
		private static System.Drawing.SolidBrush myCorp;
		private static System.Drawing.SolidBrush myTrum;
		private static System.Drawing.SolidBrush mySeа;
		private static System.Drawing.SolidBrush myRed;
		private static System.Drawing.SolidBrush myGreen;
		private static System.Drawing.SolidBrush myBlue;
		private static System.Drawing.Pen myWind;

		// при запуске BusEngine до создания формы
		public /* async */ override void Initialize() {
			// создаем объекты-кисти для закрашивания фигур
			myTrub = new System.Drawing.SolidBrush(System.Drawing.Color.DeepPink);
			myCorp = new System.Drawing.SolidBrush(System.Drawing.Color.DarkMagenta);
			myTrum = new System.Drawing.SolidBrush(System.Drawing.Color.DarkOrchid);
			mySeа = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
			myRed = new System.Drawing.SolidBrush(System.Drawing.Color.Red);
			myGreen = new System.Drawing.SolidBrush(System.Drawing.Color.Green);
			myBlue = new System.Drawing.SolidBrush(System.Drawing.Color.Blue);
			// выбираем перо myPen желтого цвета толщиной в 1 пиксель:
			myWind = new System.Drawing.Pen(System.Drawing.Color.Yellow, 1);
		}

		// при запуске BusEngine после создания формы Canvas
		public /* async */ override void InitializeСanvas() {
			//BusEngine.UI.Canvas.WinForm.TransparencyKey = System.Drawing.Color.Black;
			//BusEngine.UI.Canvas.WinForm.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			// деламем окно не поверх других окон
			BusEngine.UI.Canvas.WinForm.TopMost = false;

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
			BusEngine.UI.Canvas.WinForm.MouseMove += new System.Windows.Forms.MouseEventHandler(MouseMove);

			/* // зависимость от времени
			System.Timers.Timer aTimer = new System.Timers.Timer(1000F/FPSSetting);
			// Hook up the Elapsed event for the timer.
			// подключаем событие времени
			aTimer.Elapsed += OnTimedEvent;
			aTimer.AutoReset = true;
			aTimer.Enabled = true; */

			// FPS
			System.Timers.Timer fpsTimer = new System.Timers.Timer(1000);
			// Hook up the Elapsed event for the timer.
			// подключаем событие FPS
			fpsTimer.Elapsed += OnFPS;
			fpsTimer.AutoReset = true;
			fpsTimer.Enabled = true;
		}

		// событие мыши
		private static void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
			//BusEngine.Log.Clear();
			BusEngine.Log.Info("FPS ============== FPS Setting " + FPSSetting);
			BusEngine.Log.Info("FPS ============== FPS " + FPSInfo);
			//BusEngine.UI.Canvas.WinForm.Refresh();
			//BusEngine.UI.Canvas.WinForm.Invalidate();
		}

		// событие времени
		private static void OnTimedEvent(object source, System.Timers.ElapsedEventArgs e) {
			BusEngine.Log.Clear();
			//BusEngine.Log.Info("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
			//FPS++;
			BusEngine.Log.Info("FPS ============== FPS Setting " + FPSSetting);
			BusEngine.Log.Info("FPS ============== FPS " + FPSInfo);
			/* foreach(System.Windows.Forms.Control c in BusEngine.UI.Canvas.WinForm.Controls) {
				BusEngine.Log.Info("GGG " + c.Name + " GGG");
			} */
			//BusEngine.UI.Canvas.WinForm.Refresh();
			BusEngine.UI.Canvas.WinForm.Invalidate();
			//BusEngine.UI.Canvas.WinForm.Update();
			//BusEngine.UI.Canvas.WinForm.Controls.Find().InvokePaint();
		}

		// событие FPS
		private static void OnFPS(object source, System.Timers.ElapsedEventArgs e) {
			FPSInfo = FPS;
			FPS = 0;
		}

		// событие отрисовки - главный подсчёт кадров
		private static void Paint(object sender, System.Windows.Forms.PaintEventArgs e) {
			FPS++;
			//e.Graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;

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

			//if (Count/3 == System.Convert.ToInt32(Count/3)) {
				Count2++;
			//}
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
			//e.Graphics.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.Color.Black), 480, 125, 75, 75);
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
					new System.Drawing.Point(100 + Count, 300),
					new System.Drawing.Point(700 + Count, 300),
					new System.Drawing.Point(700 + Count, 300),
					new System.Drawing.Point(600 + Count, 400),
					new System.Drawing.Point(600 + Count, 400),
					new System.Drawing.Point(200 + Count, 400),
					new System.Drawing.Point(200 + Count, 400),
					new System.Drawing.Point(100 + Count, 300)
				}
			);
		}

		// событие отрисовки - модель 7
		private static void Paint7(object sender, System.Windows.Forms.PaintEventArgs e) {
			// Море - 12 секторов-полуокружностей
			int xx = 50;
			while (xx <= BusEngine.UI.Canvas.WinForm.Width - 50) {
				e.Graphics.FillPie(mySeа, 0 + xx, 375, 50, 50, 0, -180); 
				xx += 50;
			}
		}

		// событие отрисовки - модель 8
		private static void Paint8(object sender, System.Windows.Forms.PaintEventArgs e) {
			// Translate transformation matrix.
			//e.Graphics.TranslateTransform(0, 0);

			// Save translated graphics state.
			System.Drawing.Drawing2D.GraphicsState transState = e.Graphics.Save();

			// Reset transformation matrix to identity and fill rectangle.
			e.Graphics.FillRectangle(myRed, 0, 0, 100, 100);
			e.Graphics.FillRectangle(myGreen, 100, 0, 100, 100);
			e.Graphics.FillRectangle(myBlue, 200, 0, 100, 100);

			// rectangle.
			//if (count/3 == System.Convert.ToInt32(count/3)) {
				//ScaleTransformFloat(e);
			//}

			// Restore graphics state to translated state and fill second
			e.Graphics.Restore(transState);
			//FPS++;
/* 			BusEngine.Log.Clear();
			BusEngine.Log.Info("The Elapsed event was raised at {0}", e.Graphics.CompositingQuality);
			//BusEngine.Log.Info("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
			BusEngine.Log.Info(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width);
			BusEngine.Log.Info("FPS ============== FPS Setting " + FPSSetting);
			BusEngine.Log.Info("FPS ============== FPS " + FPSInfo);
			BusEngine.Log.Info("Paint ============== Paint " + Count + " " + Count2);
			BusEngine.UI.Canvas.WinForm.Invalidate(); */
		}

		private static void ScaleTransformFloat(System.Windows.Forms.PaintEventArgs e) {
			// Set world transform of graphics object to rotate.
			e.Graphics.RotateTransform(30.0F);

			// Then to scale, prepending to world transform.
			e.Graphics.ScaleTransform(3.0F, 1.0F);

			// Draw scaled, rotated rectangle to screen.
			e.Graphics.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Blue, 3), 50, 0, 100, 40);
		}

		// вызывается при отрисовки каждого кадра
		public override void OnGameUpdate() {
			BusEngine.Log.Clear();
			//BusEngine.Log.Info("The Elapsed event was raised at {0}", e.Graphics.CompositingQuality);
			//BusEngine.Log.Info("The Elapsed event was raised at {0:HH:mm:ss.fff}", e.SignalTime);
			BusEngine.Log.Info(System.Windows.Forms.Screen.PrimaryScreen.WorkingArea.Width);
			BusEngine.Log.Info("FPS ============== FPS Setting " + FPSSetting);
			BusEngine.Log.Info("FPS ============== FPS " + FPSInfo);
			BusEngine.Log.Info("Paint ============== Paint " + Count + " " + Count2);
			BusEngine.UI.Canvas.WinForm.Invalidate();
		}

		// перед закрытием BusEngine
		public override void Shutdown() {
			//BusEngine.UI.Canvas.ShutdownStatic();
			BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint);
			//BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint1);
			BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint2);
			BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint3);
			BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint4);
			BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint5);
			BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint6);
			BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint7);
			BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint8);
		}
	}
	/** API BusEngine.Plugin */
}
/** API BusEngine.Game - пользовательский код */