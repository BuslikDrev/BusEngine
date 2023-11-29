/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2024; BuslikDrev - Усе правы захаваны. */

/** API BusEngine.Game - пользовательский код */
namespace BusEngine.Game {
	/** API BusEngine.Plugin */
	public class MyPlugin : BusEngine.Plugin {
		// настройки
		private static bool Nap = true;
		private static int Count = 0;
		private static int Count2 = 0;
		private static int FPS = 0;
		private static int FPSSetting;
		private static int FPSInfo = 0;

		private static System.Drawing.SolidBrush myTrub;
		private static System.Drawing.SolidBrush myCorp;
		private static System.Drawing.SolidBrush myTrum;
		private static System.Drawing.SolidBrush mySeа;
		private static System.Drawing.SolidBrush myRed;
		private static System.Drawing.SolidBrush myGreen;
		private static System.Drawing.SolidBrush myBlue;
		private static System.Drawing.Pen myWind;

		// при запуске BusEngine после создания формы Canvas
		public override void InitializeСanvas() {
			// деламем окно не поверх других окон
			BusEngine.UI.Canvas.WinForm.TopMost = false;

			if (!int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["sys_FPS"], out FPSSetting)) {
				FPSSetting = 100;
			}

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

			// тест графики
			// https://rsdn.org/article/gdi/gdiplus2mag.xml
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint1);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint2);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint3);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint4);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint5);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint6);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint7);
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint8);

			// подключаем событие мыши
			BusEngine.UI.Canvas.WinForm.MouseMove += new System.Windows.Forms.MouseEventHandler(MouseMove);

			// FPS
			System.Timers.Timer fpsTimer = new System.Timers.Timer(1000);
			fpsTimer.Elapsed += OnFPS;
			fpsTimer.AutoReset = true;
			fpsTimer.Enabled = true;

			BusEngine.Engine.GameStart();
		}

		// вызывается при отрисовки каждого кадра
		public /* async */ override void OnGameUpdate() {
			FPS++;

			if (Count < 1 || Count < 300 && Nap == true) {
				Nap = true;
			} else {
				Nap = false;
			}

			if (Nap == true) {
				Count++;
			} else {
				Count--;
			}

			Count2++;

			BusEngine.Log.Clear();
			BusEngine.Log.Info("FPS Setting: {0}", FPSSetting);
			BusEngine.Log.Info("FPS: {0}", FPSInfo);
			BusEngine.Log.Info("Paint: {0} {1}", Count, Count2);
		}

		// перед закрытием BusEngine
		public override void Shutdown() {
			BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint1);
			BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint2);
			BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint3);
			BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint4);
			BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint5);
			BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint6);
			BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint7);
			BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint8);
		}

		// событие мыши
		private static void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
			if (BusEngine.Engine.IsGame == false) {
				BusEngine.Engine.GameStart();
			} else {
				BusEngine.Engine.GameStop();
			}
		}

		// событие FPS
		private static void OnFPS(object source, System.Timers.ElapsedEventArgs e) {
			FPSInfo = FPS;
			FPS = 0;
		}

		// событие отрисовки - модель 1
		private static void Paint1(object sender, System.Windows.Forms.PaintEventArgs e) {
			System.Drawing.Graphics g = e.Graphics;
			// фон
			g.Clear(System.Drawing.Color.Turquoise);
		}

		// событие отрисовки - модель 2
		private static void Paint2(object sender, System.Windows.Forms.PaintEventArgs e) {
			System.Drawing.Graphics g = e.Graphics;
			// труба (прямоугольник)
			g.FillRectangle(myTrub, 300 + Count, 125, 75, 75);
		}

		// событие отрисовки - модель 3
		private static void Paint3(object sender, System.Windows.Forms.PaintEventArgs e) {
			System.Drawing.Graphics g = e.Graphics;
			// труба (прямоугольник)
			g.FillRectangle(myTrub, 480 + Count, 125, 75, 75);
		}

		// событие отрисовки - модель 4
		private static void Paint4(object sender, System.Windows.Forms.PaintEventArgs e) {
			System.Drawing.Graphics g = e.Graphics;
			// палуба (прямоугольник)
			g.FillRectangle(myTrum, 250 + Count, 200, 350, 100);
		}

		// событие отрисовки - модель 5
		private static void Paint5(object sender, System.Windows.Forms.PaintEventArgs e) {
			System.Drawing.Graphics g = e.Graphics;
			// Иллюминаторы
			// 6 окружностей
			g.DrawEllipse(myWind, 300 + Count, 240, 20, 20);
			g.DrawEllipse(myWind, 350 + Count, 240, 20, 20);
			g.DrawEllipse(myWind, 400 + Count, 240, 20, 20);
			g.DrawEllipse(myWind, 450 + Count, 240, 20, 20);
			g.DrawEllipse(myWind, 500 + Count, 240, 20, 20);
			g.DrawEllipse(myWind, 550 + Count, 240, 20, 20);
		}

		// событие отрисовки - модель 6
		private static void Paint6(object sender, System.Windows.Forms.PaintEventArgs e) {
			System.Drawing.Graphics g = e.Graphics;
			// корпус (трапеция)
			g.FillPolygon(myCorp, new System.Drawing.Point[] {
				new System.Drawing.Point(100 + Count, 300),
				new System.Drawing.Point(700 + Count, 300),
				new System.Drawing.Point(700 + Count, 300),
				new System.Drawing.Point(600 + Count, 400),
				new System.Drawing.Point(600 + Count, 400),
				new System.Drawing.Point(200 + Count, 400),
				new System.Drawing.Point(200 + Count, 400),
				new System.Drawing.Point(100 + Count, 300)
			});
		}

		// событие отрисовки - модель 7
		private static void Paint7(object sender, System.Windows.Forms.PaintEventArgs e) {
			System.Drawing.Graphics g = e.Graphics;
			// Море - 12 секторов-полуокружностей
			int xx = 50;
			while (xx <= BusEngine.UI.Canvas.WinForm.Width - 50) {
				g.FillPie(mySeа, 0 + xx, 375, 50, 50, 0, -180); 
				xx += 50;
			}
		}

		// событие отрисовки - модель 8
		private static void Paint8(object sender, System.Windows.Forms.PaintEventArgs e) {
			System.Drawing.Graphics g = e.Graphics;
			// Translate transformation matrix.
			//g.TranslateTransform(0, 0);

			// Save translated graphics state.
			//System.Drawing.Drawing2D.GraphicsState transState = g.Save();

			// Reset transformation matrix to identity and fill rectangle.
			g.FillRectangle(myRed, 0, 0, 100, 100);
			g.FillRectangle(myGreen, 100, 0, 100, 100);
			g.FillRectangle(myBlue, 200, 0, 100, 100);

			// rectangle.
			//ScaleTransformFloat(e);

			// Restore graphics state to translated state and fill second
			//g.Restore(transState);
		}

		private static void ScaleTransformFloat(System.Windows.Forms.PaintEventArgs e) {
			System.Drawing.Graphics g = e.Graphics;
			// Set world transform of graphics object to rotate.
			g.RotateTransform(30.0F);

			// Then to scale, prepending to world transform.
			g.ScaleTransform(3.0F, 1.0F);

			// Draw scaled, rotated rectangle to screen.
			g.DrawRectangle(new System.Drawing.Pen(System.Drawing.Color.Blue, 3), 50, 0, 100, 40);
		}
	}
	/** API BusEngine.Plugin */
}
/** API BusEngine.Game - пользовательский код */