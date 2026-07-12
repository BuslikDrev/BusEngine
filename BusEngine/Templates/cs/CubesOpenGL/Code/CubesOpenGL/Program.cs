/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2026; BuslikDrev - Усе правы захаваны. */

// benchmark https://cc.davelozinski.com/c-sharp/for-vs-foreach-vs-while
// OpenGL http://pm.samgtu.ru/sites/pm.samgtu.ru/files/materials/comp_graph/RedBook_OpenGL.pdf
// Crysis
// https://habr.com/ru/articles/350782/
// https://habr.com/ru/articles/338998/
// http://mar.ugatu.su/index.php?id=laboratornye-raboty-1
// https://habr.com/ru/articles/458988/
// https://render.ru/ru/Kaino/post/22718
// https://github.com/8Observer8/TexturedRectangle_OpenTkOpenGL30CSharp/tree/master
// https://github.com/StanislavPetrovV/Advanced_RayMarching/tree/main
// https://github.com/opentk/LearnOpenTK/blob/3.x/Common/Texture.cs

//https://learnopengl.com/Advanced-OpenGL/Framebuffers


// внедрение браузера в рендер
// https://github.com/cefsharp/CefSharp/blob/master/CefSharp.OffScreen.Example/Program.cs#L174
//https://habr.com/ru/articles/351706/
//http://www.opengl-tutorial.org/intermediate-tutorials/billboards-particles/particles-instancing/
// советы по оптимизации
//https://cyberleninka.ru/article/n/metody-optimizatsii-pod-opengl/viewer

/** API BusEngine.Game - пользовательский код */
namespace BusEngine.Game {
	/** API BusEngine.Plugin */
	public class MyPlugin : BusEngine.Plugin {
		// настройки
		private static int CPU, GPU, KEY;
		private static float CPUInfo, GPUInfo, KEYInfo, FPSCPU, FPSGPU, FPSKEY, FPSDelta = 1.0F;
		private static OpenTK.Vector3 position = new OpenTK.Vector3(-10F, -10.0F, 500.0F);
		private static OpenTK.Vector3 front = new OpenTK.Vector3(0.0F, 0.0F, 0.0F);
		private static OpenTK.Vector3 up = OpenTK.Vector3.UnitY; // new OpenTK.Vector3(0.0F, 1.0F, 0.0F);
		//private static OpenTK.Vector3 orientation = new OpenTK.Vector3((float)System.Math.PI, 0.0F, 0.0F);
		private static OpenTK.Vector3 orientation = new OpenTK.Vector3(OpenTK.MathHelper.Pi, 0.0F, 0.0F);
		private static float speed = 0.1F;
		private static float mousespeed = 0.0025F;
		private static System.Collections.Generic.HashSet<System.Windows.Forms.Keys> IsKeys = new System.Collections.Generic.HashSet<System.Windows.Forms.Keys>();
		public static OpenTK.Vector2 lastPos;
		private static int lastWheel;
		private static bool F1, F2, F3, F4, Pause;

		private System.Timers.Timer _timer = null;
		public static float _angle_left = 0.0F;
		public static float _angle_right = 0.0F;
		public static OpenTK.Matrix4 view, projection, frustumd;

		// https://opentk.net/learn/chapter1/3-element-buffer-objects.html
		public static BusEngine.Model[] Models = new BusEngine.Model[0];
		//private static System.Collections.Concurrent.BlockingCollection<BusEngine.Model> Models = new System.Collections.Concurrent.BlockingCollection<BusEngine.Model>();

		public static OpenTK.GLControl glControl;
		//public static OpenTK.GameWindow glControl2;
		private static System.Windows.Forms.Label label;

		// при запуске BusEngine после создания формы Canvas
		public override void InitializeСanvas() {
			// деламем окно не поверх других окон
			BusEngine.UI.Canvas.WinForms.TopMost = false;
			BusEngine.UI.Canvas.WinForms.KeyPreview = false;
			BusEngine.UI.Canvas.WinForms.AllowTransparency = false;
			//BusEngine.UI.Canvas.WinForms.TransparencyKey = System.Drawing.Color.FromArgb(255, 0, 0, 0);
			//BusEngine.UI.Canvas.WinForms.TransparencyKey = System.Drawing.Color.FromArgb(255, 255, 0, 0);
			BusEngine.UI.Canvas.WinForms.BackColor = System.Drawing.Color.FromArgb(255, 255, 255, 255);
			//BusEngine.UI.Canvas.WinForms.BackColor = System.Drawing.Color.Transparent;
			//BusEngine.UI.Canvas.WinForms.IsMdiContainer = true;

			// FPS
			System.Timers.Timer fpsTimer = new System.Timers.Timer(1000);
			fpsTimer.Elapsed += OnFPS;
			//fpsTimer.Interval = 1000;
			fpsTimer.AutoReset = true;
			fpsTimer.Enabled = true;

			// FPS
			/* BusEngine.Tools.Timer timer = new BusEngine.Tools.Timer(1);
			timer.Elapsed += (BusEngine.Tools.Timer o, int time) => {
				BusEngine.Log.Info(time);
			};
			//timer.Interval = 1000;
			timer.AutoReset = true;
			timer.Enabled = true; */

			float.TryParse(BusEngine.Engine.SettingProject["console_commands"]["sys_FPS_CPU"], out FPSCPU);
			float.TryParse(BusEngine.Engine.SettingProject["console_commands"]["sys_FPS"], out FPSCPU);
			if (FPSCPU < 1.0F) {
				FPSCPU = 60.0F;
			}

			float.TryParse(BusEngine.Engine.SettingProject["console_commands"]["sys_FPS_GPU"], out FPSGPU);
			if (FPSGPU < 1.0F) {
				FPSGPU = 60.0F;
			}

			float.TryParse(BusEngine.Engine.SettingProject["console_commands"]["sys_FPS_Key"], out FPSKEY);
			if (FPSKEY < 1.0F) {
				FPSKEY = 100.0F;
			}

			int r_Width;
			int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["r_Width"], out r_Width);
			if (r_Width < BusEngine.UI.Canvas.WinForms.Width) {
				r_Width = BusEngine.UI.Canvas.WinForms.Width;
			}

			int r_Height;
			int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["r_Height"], out r_Height);
			if (r_Height < BusEngine.UI.Canvas.WinForms.Height) {
				r_Height = BusEngine.UI.Canvas.WinForms.Height;
			}

			
			/* browser = new BusEngine.Browser("index.html");
			browser.CaptureScreenshotAsync();
			browser.OnLoad += () => {
				BusEngine.Log.Info("dddddddddd");
				browser.CaptureScreenshotAsync();
				BusEngine.Log.Info(browser.Screenshot.Length);
				BusEngine.Log.Info("dddddddddd");
			};
			browser.Load();
			browser.Control.Location = new System.Drawing.Point(0, 0);
			browser.Control.Size = new System.Drawing.Size(400, 400);
			browser.Control.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			browser.Control.TabIndex = 10;
			browser.Control.TabStop = false;
			browser.Control.ForeColor = System.Drawing.Color.White; */

			//http://jeske.github.io/opentkr-doxygen/html/class_open_t_k_1_1_graphics_1_1_graphics_mode.html
			OpenTK.Graphics.GraphicsMode graphicsMode = new OpenTK.Graphics.GraphicsMode(
				color: new OpenTK.Graphics.ColorFormat(0, 0, 0, 0),
				depth: 32,
				stencil: 8,
				samples: 4, //FSAA
				accum: new OpenTK.Graphics.ColorFormat(0, 0, 0, 0),
				buffers: 3,
				stereo: false
			);
			/* OpenTK.Graphics.GraphicsMode graphicsMode = new OpenTK.Graphics.GraphicsMode(
				color: new OpenTK.Graphics.ColorFormat(0, 0, 0, 0),
				depth: 16,
				stencil: 16,
				samples: 16, //FSAA
				accum: new OpenTK.Graphics.ColorFormat(0, 0, 0, 0),
				stereo: true,
				buffers: 2
			); */
			//graphicsMode.Index = 0x00C00000;
			
			//graphicsMode.SelectResolution(100, 100, 32, 30F);

//OpenTK.DisplayDevice.SelectResolution(100, 100, 32, 30F);
//BusEngine.Log.Info(OpenTK.DisplayDevice.AvailableResolutions);

			glControl = new OpenTK.GLControl(graphicsMode, 1, 0, OpenTK.Graphics.GraphicsContextFlags.ForwardCompatible);

/* foreach (OpenTK.DisplayDevice device in OpenTK.DisplayDevice.AvailableResolutions.Get) {
	BusEngine.Log.Info(device);
	device.SelectResolution(100, 100, 32, 30F);
} */


			//((System.ComponentModel.ISupportInitialize)glControl).BeginInit();
			//BusEngine.UI.Canvas.WinForms.SuspendLayout();
			glControl.Size = new System.Drawing.Size(r_Width, r_Height);
			glControl.VSync = BusEngine.Engine.SettingProject["console_commands"]["sys_VSync"] == "1";
			glControl.Location = new System.Drawing.Point(0, 0);
			glControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            glControl.Visible  = true;
            glControl.Enabled  = true;
			// подключаем событие клавиатуры
			/* glControl.KeyDown += new System.Windows.Forms.KeyEventHandler(KeyDown);
			glControl.KeyUp += new System.Windows.Forms.KeyEventHandler(KeyUp); */
			glControl.KeyDown += new System.Windows.Forms.KeyEventHandler(KeyDown);
			glControl.KeyUp += new System.Windows.Forms.KeyEventHandler(KeyUp);
			// зволяет включить нажатие сразу после запуска игры
			glControl.KeyPress += new System.Windows.Forms.KeyPressEventHandler((object sender, System.Windows.Forms.KeyPressEventArgs e) => {
				//BusEngine.Log.Info("Press {0}", e.KeyChar);
				e.Handled = true;
			});
			// позволяет включить нажатие кнопок стрелок
			glControl.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler((object sender, System.Windows.Forms.PreviewKeyDownEventArgs e) => {
				//BusEngine.Log.Info("Preview {0}", e.KeyCode);
				e.IsInputKey = true;
			});
			// подключаем событие мыши
			glControl.MouseMove += new System.Windows.Forms.MouseEventHandler(MouseMove);
			glControl.MouseLeave += new System.EventHandler(MouseLeave);
			glControl.MouseWheel += new System.Windows.Forms.MouseEventHandler(MouseWheel);
			//BusEngine.UI.Canvas.WinForms.MouseMove += new System.Windows.Forms.MouseEventHandler(MouseMove);
			//BusEngine.UI.Canvas.WinForms.MouseLeave += new System.EventHandler(MouseLeave);
			//BusEngine.UI.Canvas.WinForms.MouseWheel += new System.Windows.Forms.MouseEventHandler(MouseWheel);
			// подключаем событие загрузки окна OpenTK
			glControl.Load += new System.EventHandler(Load);
			// подключаем событие изменение размера окна
			glControl.Resize += new System.EventHandler(Resize);
			// подключаем событие рисования
			//glControl.Paint += new System.Windows.Forms.PaintEventHandler(Paint);
			//BusEngine.UI.Canvas.WinForms.Paint += new System.Windows.Forms.PaintEventHandler(Paint);
			glControl.TabIndex = 0;
			glControl.TabStop = true; // чтобы при старте приложения сразу работали кнопки
			//glControl.BringToFront();
			//glControl.SuspendLayout();
			glControl.BackColor = System.Drawing.Color.FromArgb(255, 1, 1, 1);
			//glControl.BackColor = System.Drawing.Color.Transparent;

			label = new System.Windows.Forms.Label();
			label.Location = new System.Drawing.Point(20, 5);
			label.Size = new System.Drawing.Size(250, 310);
			//label.Size = new System.Drawing.Size(label.PreferredWidth, label.PreferredHeight);
			//label.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			label.TabIndex = 3;
			label.TabStop = true;
			label.ForeColor = System.Drawing.Color.FromArgb(255, 255, 255, 255);
			label.BackColor = System.Drawing.Color.FromArgb(255, 0, 0, 0);
			//label.Opacity = 50;
			//label.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
			//label.FlatStyle = System.Windows.Forms.FlatStyle.Popup;

			//glControl.Controls.Add(label);
			
			BusEngine.UI.Canvas.WinForms.Controls.Add(label);
			BusEngine.UI.Canvas.WinForms.Controls.Add(glControl);

			/* foreach (System.Windows.Forms.Control control in panel.Controls) {
				if (control.GetType().ToString().IndexOf("BusEngine.Game") != -1) {
					control.BringToFront();
				} else {
					foreach (System.Reflection.PropertyInfo xxxx in control.GetType().GetProperties()) {
						BusEngine.Log.Info(xxxx);
					}
					control.SendToBack();
				}
			} */
			

			//panel.Focus();

			

			//browser.Control.Focus();
			//browser.Control.BringToFront();
			//browser.Control.Invalidate(true);
			//browser.Control.Update();

			//label.Focus();
			//label.BringToFront();
			//label.Invalidate(true);
			//label.Update();

			//glControl.Focus();
			//glControl.BringToFront();
			//glControl.Invalidate(true);
			//glControl.Update();

			
			//BusEngine.UI.Canvas.WinForms.Controls.Add(glControl);


			//browser.Control.Owner.Focus();
			//glControl.ResumeLayout(false);
			//glControl.PerformLayout();

			//BusEngine.UI.Canvas.WinForms.Margin = new System.Windows.Forms.Padding(100, 100, 100, 0);
			//BusEngine.UI.Canvas.WinForms.Focus();
			//glControl.Focus();
			//BusEngine.UI.Canvas.WinForms.TabIndex = 1;
			/* BusEngine.UI.Canvas.WinForms.Focus(); */

			//((System.ComponentModel.ISupportInitialize)(glControl)).EndInit();
			//BusEngine.UI.Canvas.WinForms.ResumeLayout(false);
			//BusEngine.UI.Canvas.WinForms.PerformLayout();

			//glControl.PerformContextUpdate();

			//BusEngine.Log.Info(OpenTK.Input.KeyboardDevice());
			//OpenTK.Input.KeyboardDevice.Item(OpenTK.Input.Key.Ctrl);
		}

		public static BusEngine.Browser browser;
		public static void KeyDown(object sender, dynamic e) {
			if (e != null) {
				//BusEngine.Log.Info("ss {0}", e.KeyCode);
				IsKeys.Add(e.KeyCode);

				if (IsKeys.Contains(System.Windows.Forms.Keys.Pause)) {
					if (Pause) {
						BusEngine.Engine.GameStart();
						BusEngine.Game.MyPlugin.MouseLeave(null, null);
						IsKeys.Remove(System.Windows.Forms.Keys.Pause);
						Pause = false;
					}
				}
			} else {
				// фикс некоторых кнопок
				/* OpenTK.Input.KeyboardState keyboard = OpenTK.Input.Keyboard.GetState();

				//BusEngine.Log.Info("KeyboardState {0}", keyboard.IsAnyKeyDown);
				if (keyboard.IsKeyDown(OpenTK.Input.Key.Up)) {
					IsKeys.Add(System.Windows.Forms.Keys.Up);
				}
				if (keyboard.IsKeyDown(OpenTK.Input.Key.Down)) {
					IsKeys.Add(System.Windows.Forms.Keys.Down);
				}
				if (keyboard.IsKeyDown(OpenTK.Input.Key.Left)) {
					IsKeys.Add(System.Windows.Forms.Keys.Left);
				}
				if (keyboard.IsKeyDown(OpenTK.Input.Key.Right)) {
					IsKeys.Add(System.Windows.Forms.Keys.Right);
				} */
			}

			if (IsKeys.Contains(System.Windows.Forms.Keys.Escape)) {
				BusEngine.Engine.Shutdown();
			}

			if (e == null) {
				bool play = false;

				if (IsKeys.Contains(System.Windows.Forms.Keys.W)) {
					if (IsKeys.Contains(System.Windows.Forms.Keys.ShiftKey) || IsKeys.Contains(System.Windows.Forms.Keys.LShiftKey)) {
						position += front * speed * 4.0f;
					} else {
						position += front * speed;
					}
					play = true;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.S)) {
					if (IsKeys.Contains(System.Windows.Forms.Keys.ShiftKey) || IsKeys.Contains(System.Windows.Forms.Keys.LShiftKey)) {
						position -= front * speed * 4.0f;
					} else {
						position -= front * speed;
					}
					play = true;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.A)) {
					if (IsKeys.Contains(System.Windows.Forms.Keys.ShiftKey) || IsKeys.Contains(System.Windows.Forms.Keys.LShiftKey)) {
						position -= OpenTK.Vector3.Normalize(OpenTK.Vector3.Cross(front, up)) * speed * 4.0f;
					} else {
						position -= OpenTK.Vector3.Normalize(OpenTK.Vector3.Cross(front, up)) * speed;
					}
					play = true;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.D)) {
					if (IsKeys.Contains(System.Windows.Forms.Keys.ShiftKey) || IsKeys.Contains(System.Windows.Forms.Keys.LShiftKey)) {
						position += OpenTK.Vector3.Normalize(OpenTK.Vector3.Cross(front, up)) * speed * 4.0f;
					} else {
						position += OpenTK.Vector3.Normalize(OpenTK.Vector3.Cross(front, up)) * speed;
					}
					play = true;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.Up)) {
					//position += up * speed;

					Mouse.Y += 0.1f;
					BusEngine.Model.LightPos.Y += 5.0f * speed;
					play = true;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.Down)) {
					//position -= up * speed;

					Mouse.Y -= 0.1f;
					BusEngine.Model.LightPos.Y -= 5.0f * speed;
					play = true;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.Left)) {
					//lastPos.X = OpenTK.Input.Mouse.GetState().X + mousespeed * 2000;

					Mouse.X -= 0.1f;
					BusEngine.Model.LightPos.X -= 5.0f * speed;
					play = true;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.Right)) {
					//lastPos.X = OpenTK.Input.Mouse.GetState().X - mousespeed * 2000;

					Mouse.X += 0.1f;
					BusEngine.Model.LightPos.X += 5.0f * speed;
					play = true;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.Space)) {
					//position = new OpenTK.Vector3(1600.0F, 850.0F, 1700.0F);
					position = new OpenTK.Vector3(-10F, -10.0F, 0.0F);
					play = true;
					orientation = new OpenTK.Vector3(OpenTK.MathHelper.Pi, 0.0F, 0.0F);
					front = new OpenTK.Vector3(0F, 0F, 0F);

					OpenTK.Input.MouseState mouse = OpenTK.Input.Mouse.GetState();

					OpenTK.Vector2 delta = lastPos - new OpenTK.Vector2(mouse.X, mouse.Y);

					AddRotation(delta.X, delta.Y);

					//orientation.X = (float)System.Math.PI;
					//orientation.X = OpenTK.MathHelper.Pi;

					lastPos = new OpenTK.Vector2(mouse.X, mouse.Y);

					double y = System.Math.Cos(orientation.Y);

					front.X = (float)(System.Math.Sin(orientation.X) * y);
					front.Y = (float)System.Math.Sin(orientation.Y);
					front.Z = (float)(System.Math.Cos(orientation.X) * y);

					//up = OpenTK.Vector3.UnitY;
					//view = OpenTK.Matrix4.CreateRotationX(OpenTK.MathHelper.DegreesToRadians(90)) * OpenTK.Matrix4.LookAt(position, position + front, up) * projection;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.Pause)) {
					IsKeys.Remove(System.Windows.Forms.Keys.Pause);
					if (!Pause) {
						Pause = true;
						BusEngine.Engine.GameStop();
					}
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.T)) {
					cubes += 1;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.G)) {
					cubes -= 1;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.Y)) {
					line++;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.H)) {
					line--;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.NumPad1)) {
					position = new OpenTK.Vector3(0.0F, 0.0F, -42.0F);
					play = true;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.NumPad2)) {
					position = new OpenTK.Vector3(20.0F, 0.0F, -42.0F);
					play = true;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.NumPad3)) {
					position = new OpenTK.Vector3(40.0F, 0.0F, -42.0F);
					play = true;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.NumPad4)) {
					position = new OpenTK.Vector3(60.0F, 0.0F, -42.0F);
					play = true;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.NumPad5)) {
					position = new OpenTK.Vector3(0.0F, -20.0F, -42.0F);
					play = true;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.NumPad6)) {
					position = new OpenTK.Vector3(20.0F, -20.0F, -42.0F);
					play = true;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.NumPad7)) {
					position = new OpenTK.Vector3(40.0F, -20.0F, -42.0F);
					play = true;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.NumPad8)) {
					position = new OpenTK.Vector3(60.0F, -20.0F, -42.0F);
					play = true;
				}

				// из-за Radeon - убираем возможность показа стены с двух сторон.
				// https://ravesli.com/urok-22-otsechenie-granej-v-opengl/
				if (IsKeys.Contains(System.Windows.Forms.Keys.F1)) {
					IsKeys.Remove(System.Windows.Forms.Keys.F1);
					if (!F1) {
						F1 = true;
						OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.CullFace);
					} else {
						OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.CullFace);
						F1 = false;
					}
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.F2)) {
					IsKeys.Remove(System.Windows.Forms.Keys.F2);
					if (!F2) {
						F2 = true;
						OpenTK.Graphics.OpenGL.GL.CullFace(OpenTK.Graphics.OpenGL.CullFaceMode.Front);
					} else {
						OpenTK.Graphics.OpenGL.GL.CullFace(OpenTK.Graphics.OpenGL.CullFaceMode.Back);
						F2 = false;
					}
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.F3)) {
					IsKeys.Remove(System.Windows.Forms.Keys.F3);
					if (!F3) {
						F3 = true;
						OpenTK.Graphics.OpenGL.GL.PolygonMode(OpenTK.Graphics.OpenGL.MaterialFace.FrontAndBack, OpenTK.Graphics.OpenGL.PolygonMode.Line);
					} else {
						OpenTK.Graphics.OpenGL.GL.PolygonMode(OpenTK.Graphics.OpenGL.MaterialFace.FrontAndBack, OpenTK.Graphics.OpenGL.PolygonMode.Fill);
						F3 = false;
					}
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.F4)) {
					IsKeys.Remove(System.Windows.Forms.Keys.F4);
					if (!F4) {
						F4 = true;
						OpenTK.Graphics.OpenGL.GL.FrontFace(OpenTK.Graphics.OpenGL.FrontFaceDirection.Ccw);
					} else {
						OpenTK.Graphics.OpenGL.GL.FrontFace(OpenTK.Graphics.OpenGL.FrontFaceDirection.Cw);
						F4 = false;
					}
				}
				
				//Resize(glControl, System.EventArgs.Empty);

				if (IsKeys.Contains(System.Windows.Forms.Keys.F12)) {
					IsKeys.Remove(System.Windows.Forms.Keys.F12);
					GrabScreenshot();
					GrabScreenshotStatus = false;

/* if (1 == 0) {
			System.Threading.Tasks.Task[] tasks = new System.Threading.Tasks.Task[1];

			tasks[0] = System.Threading.Tasks.Task.Factory.StartNew(() => {
				int r_Width;
				int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["r_Width"], out r_Width);
				if (r_Width < BusEngine.UI.Canvas.WinForms.Width) {
					r_Width = BusEngine.UI.Canvas.WinForms.Width;
				}

				int r_Height;
				int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["r_Height"], out r_Height);
				if (r_Height < BusEngine.UI.Canvas.WinForms.Height) {
					r_Height = BusEngine.UI.Canvas.WinForms.Height;
				}

				for (ScreenshotCol = 0; ScreenshotCol < Screenshots.Length; ScreenshotCol++) {
					ggg = Screenshots[ScreenshotCol];


				int b = 3, l = bytea.Length;
				byte[] news = new byte[] {137, 80, 78, 71, 13, 10, 26, 10}, hach = new byte[4];

				int i = 4;

				string chank = System.Text.Encoding.ASCII.GetString(new byte[] {bytea[1], bytea[2], bytea[3]});

				if (chank == "PNG") {
					int i2, keys = 0, w = 0, h = 0;

					for (; i < l; i++) {
						if (i+4 < l) {
							chank = System.Text.Encoding.ASCII.GetString(new byte[] {bytea[i++], bytea[i++], bytea[i++], bytea[i]});
						} else {
							break;
						}

						if (chank == "IHDR" || chank == "sRGB" || chank == "gAMA" || chank == "PLTE" || chank == "pHYs" || chank == "tEXt" || chank == "IDAT" || chank == "IEND") {
							keys = System.BitConverter.ToChar(new byte[] {bytea[i-4], bytea[i-5], bytea[i-6], bytea[i-7]}, 0);

							// пропускаем блоки
							if (chank == "tEXt" || chank == "gAMA" || chank == "pHYs" || chank == "PLTE") {
								i += keys+4;
								continue;
							}
							i++;
							byte[] token = new byte[keys+4];
							byte[] data = new byte[keys];
							for (; keys > 0; keys--) {
								token[token.Length-keys] = bytea[i];
								data[data.Length-keys] = bytea[i];
								i++;
							}

							if (chank == "IHDR") {
								w = r_Width;//System.BitConverter.ToInt32(new byte[] {data[3], data[2], data[1], data[0]}, 0);
								h = r_Height;//System.BitConverter.ToInt32(new byte[] {data[7], data[6], data[5], data[4]}, 0);
								byte[] width = System.BitConverter.GetBytes(w);
								byte[] height = System.BitConverter.GetBytes(h);
								if (b == 3 || data[8] == 8 && data[9] == 2) {
									//b = 3;
									token = new byte[] {73, 72, 68, 82, width[3], width[2], width[1], width[0], height[3], height[2], height[1], height[0], 8, 2, 0, 0, 0};
								} else if (b == 4 || data[8] == 8 && data[9] == 6) {
									//b = 4;
									token = new byte[] {73, 72, 68, 82, width[3], width[2], width[1], width[0], height[3], height[2], height[1], height[0], 8, 6, 0, 0, 0};
								}
							}

							if (chank == "IDAT") {
								int keys2 = 0, s = 0, lvl = 0;

								for (keys = ggg.Length; keys > 0;) {
									if (keys2 == s) {
										decompressed[keys2++] = 0;
										s += fff;

										keys -= (fff-1) * 2;

										if (keys < 0) {
											break;
										}
									}

									if (lvl == 0) {
										lvl++;
										keys += 2;
									} else if (lvl == 1) {
										lvl++;
										keys--;
									} else if (lvl == 2) {
										lvl = 0;
										keys--;
									}
									decompressed[keys2++] = ggg[keys]; // красный
									if (lvl == 0) {
										keys += 3;
									}
								}

								using (System.IO.MemoryStream memory_stream = new System.IO.MemoryStream()) {
									using (System.IO.Compression.DeflateStream compressed_file = new System.IO.Compression.DeflateStream(memory_stream, System.IO.Compression.CompressionLevel.Optimal)) {
										compressed_file.Write(decompressed, 0, decompressed.Length);
										compressed_file.Close();

										byte[] res = memory_stream.ToArray();
										token = new byte[res.Length+6];

										token[0] = 0;
										token[1] = 0;
										token[2] = 0;
										token[3] = 0;
										token[4] = data[0];
										token[5] = data[1];

										for (keys = 6; keys < token.Length; keys++) {
											token[keys] = res[keys-6];
										}
									}
								}
							}

							i += 3;
							keys = i-data.Length-7;
							token[0] = bytea[keys++];
							token[1] = bytea[keys++];
							token[2] = bytea[keys++];
							token[3] = bytea[keys];

							hach = Crc322(token);

							// создаём изображение
							i2 = news.Length;

							System.Array.Resize(ref news, news.Length + token.Length + 8);

							byte[] count = System.BitConverter.GetBytes(token.Length - 4);

							// количество
							for (keys = count.Length-1; keys > -1; keys--) {
								news[i2++] = count[keys];
							}

							// название
							//for (keys = 0; keys < chank.Length; keys++) {
							//	news[i2++] = chank[keys];
							//}

							// данные
							for (keys = 0; keys < token.Length; keys++) {
								news[i2++] = token[keys];
							}

							// хэш
							for (keys = 0; keys < hach.Length; keys++) {
								news[i2++] = hach[keys];
							}
							
							if (chank == "tEXt") {
								BusEngine.Log.Info("XUETA {0}", i);
								//continue;
							}
						}
					}

					System.IO.File.WriteAllBytes(BusEngine.Engine.LogDirectory + "screenshots/" + ScreenshotCol + ".png", news);
				}
				}
				//System.Array.Clear(Screenshots, 0, Screenshots.Length);
				


			}, System.Threading.Tasks.TaskCreationOptions.LongRunning);

			System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Factory.ContinueWhenAll(tasks, (wordCountTasks) => {
				if (wordCountTasks != null) {
					foreach (System.Threading.Tasks.Task t in wordCountTasks) {
						t.Dispose();
					}
					//System.Array.Clear(wordCountTasks, 0, wordCountTasks.Length);
					//wordCountTasks = null;
				}
				if (tasks != null) {
					foreach (System.Threading.Tasks.Task t in tasks) {
						t.Dispose();
						Screenshots = new byte[100000][];
					}
					//System.Array.Clear(tasks, 0, tasks.Length);
					//tasks = null;
				}
			});
			//task.Wait();
			//task.Dispose();
} */
				}

				if (play) {
					// расположение мира (камеры)
					//a = OpenTK.Matrix4.CreateRotationX(OpenTK.MathHelper.Pi / 180.0F * 1.0F);
					//a = OpenTK.Matrix4.CreateFromAxisAngle(v, 50.0F);
					view = OpenTK.Matrix4.LookAt(position, position + front, up);
					//ExtractFrustum();
				}
			}
		}

		private static void KeyUp(object sender, System.Windows.Forms.KeyEventArgs e) {
			IsKeys.Remove(e.KeyCode);
		}

		private static void MouseLeave(object sender, System.EventArgs e) {
			if (!Pause) {
				// если очень долго крутить в одну сторону, то будет ошибка (для теста использовать мышьку и болгарку).
				OpenTK.Input.MouseState mouse = OpenTK.Input.Mouse.GetState();

				//OpenTK.Input.Mouse.SetPosition(BusEngine.UI.Canvas.WinForms.DesktopLocation.X + BusEngine.UI.Canvas.WinForms.Width/2.0F, BusEngine.UI.Canvas.WinForms.DesktopLocation.Y + BusEngine.UI.Canvas.WinForms.Height/2.0F);
				System.Windows.Forms.Cursor.Position = new System.Drawing.Point((int)(BusEngine.UI.Canvas.WinForms.DesktopLocation.X + BusEngine.UI.Canvas.WinForms.Width/2.0F), (int)(BusEngine.UI.Canvas.WinForms.DesktopLocation.Y + BusEngine.UI.Canvas.WinForms.Height/2.0F));
				//BusEngine.Log.Info(mouse);

				lastPos = new OpenTK.Vector2(mouse.X, mouse.Y);
			}
		}

		private void MouseWheel(object sender, System.EventArgs e) {
			// если очень долго крутить в одну сторону, то будет ошибка (для теста использовать мышьку и болгарку).
			OpenTK.Input.MouseState mouse = OpenTK.Input.Mouse.GetState();

			speed = (float)System.Math.Round(speed, 4);

			if (speed > 0.02f) {
				if (lastWheel < mouse.Wheel) {
					speed += 0.02f;
				} else {
					speed -= 0.02f;
				}
			} else if (speed < 0.0f) {
				speed = 0.0f;
			} else {
				if (lastWheel < mouse.Wheel) {
					speed += 0.001f;
				} else {
					speed -= 0.001f;
				}
			}

			lastWheel = mouse.Wheel;
		}

		private static void MouseMove(object sender, System.EventArgs e) {
			OpenTK.Input.MouseState mouse = OpenTK.Input.Mouse.GetState();

			OpenTK.Vector2 mousepos = new OpenTK.Vector2(mouse.X, mouse.Y);

			//if (mousepos != lastPos) {
				OpenTK.Vector2 delta = lastPos - mousepos;

				AddRotation(delta.X, delta.Y);

				lastPos = mousepos;

				double y = System.Math.Cos(orientation.Y);

				front.X = (float)(System.Math.Sin(orientation.X) * y);
				front.Y = (float)System.Math.Sin(orientation.Y);
				front.Z = (float)(System.Math.Cos(orientation.X) * y);

				//front = OpenTK.Vector3.Normalize(front);

				// расположение мира (камеры)
				//a = OpenTK.Matrix4.CreateRotationX(OpenTK.MathHelper.Pi / 180.0F * 1.0F);
				//a = OpenTK.Matrix4.CreateFromAxisAngle(v, 50.0F);
				view = OpenTK.Matrix4.LookAt(position, position + front, up);
				//ExtractFrustum();
			//}
		}

		private static void AddRotation(float mx, float my) {
			mx *= mousespeed;
			my *= mousespeed;

			//orientation.X = (orientation.X + mx) % ((float)System.Math.PI * 2.0F);
			orientation.X = (orientation.X + mx) % (OpenTK.MathHelper.Pi * 2.0F);
			//orientation.Y = System.Math.Max(System.Math.Min(orientation.Y + my, (float)System.Math.PI / 2.0F - 0.001f), (float)-System.Math.PI / 2.0F + 0.001f);
			orientation.Y = System.Math.Max(System.Math.Min(orientation.Y + my, OpenTK.MathHelper.Pi / 2.0F - 0.001f), -OpenTK.MathHelper.Pi / 2.0F + 0.001f);
		}

		public static OpenTK.Vector4 Mouse;

		private void Resize(object sender, System.EventArgs e) {
			// устанавливаем контекст GL
			//glControl.MakeCurrent();

			int Width = glControl.ClientSize.Width;
			int Height = glControl.ClientSize.Height;

			if (Width < 1) {
				Width = 1;
			}
			if (Height < 1) {
				Height = 1;
			}

			OpenTK.Graphics.OpenGL.GL.Viewport(0, 0, Width, Height);
			//OpenTK.Graphics.OpenGL.GL.Arb.BufferPageCommitment(OpenTK.Graphics.OpenGL.All,System.IntPtr,System.Int32,System.Boolean);
			glControl.MaximumSize = new System.Drawing.Size(15360, 8640);

			float sys_FOV;
			float.TryParse(BusEngine.Engine.SettingProject["console_commands"]["sys_FOV"], out sys_FOV);
			float sys_DistanceMin;
			float.TryParse(BusEngine.Engine.SettingProject["console_commands"]["sys_DistanceMin"], out sys_DistanceMin);
			float sys_DistanceMax;
			float.TryParse(BusEngine.Engine.SettingProject["console_commands"]["sys_DistanceMax"], out sys_DistanceMax);
			//projection = OpenTK.Matrix4.CreatePerspectiveFieldOfView(OpenTK.MathHelper.DegreesToRadians(sys_FOV), (float)Width / (float)Height, sys_DistanceMin, sys_DistanceMax);
			float fovy = OpenTK.MathHelper.DegreesToRadians(sys_FOV);
			float aspect = (float)Width / (float)Height;
			float zNear = sys_DistanceMin;
			float zFar = sys_DistanceMax;

			float top = zNear * (float)System.Math.Tan(0.5f * fovy);
            float bottom = -top;
            float left = bottom * aspect;
            float right = top * aspect;

			projection = OpenTK.Matrix4.CreatePerspectiveOffCenter(left, right, bottom, top, zNear, zFar);
			
			
			//OpenTK.Graphics.OpenGL.GL.Frustum(left, right, bottom, top, zNear, zFar);

			/* BusEngine.UI.Canvas.WinForms.ResumeLayout(false);
			BusEngine.UI.Canvas.WinForms.PerformLayout(); */
		}

		// событие FPS
		public static float Distance = 0.0f;
		public static float fx = 0.0f, fy = 0.0f, fz = 0.0f, fmx = 0.0f, fmy = 0.0f, fmz = 0.0f;
		private static void OnFPS(object source, System.Timers.ElapsedEventArgs e) {
			if (CPU > 0) {
				CPUInfo = (int)CPU;
			} else {
				CPUInfo = (int)FPSCPU;
			}
			if (GPU > 0) {
				GPUInfo = (int)GPU;
			} else {
				GPUInfo = (int)FPSGPU;
			}
			if (KEY > 0) {
				KEYInfo = (int)KEY;
			} else {
				KEYInfo = (int)FPSKEY;
			}
			
			CPU = 0;
			GPU = 0;
			KEY = 0;

			/* BusEngine.Log.Clear(); */
			//BusEngine.Engine.Device.Update();
			text = "";
			text += "Models Count: " + (BusEngine.Model.Count) + "\n";
			text += "Triangles polygons Count: " + (BusEngine.Model.TrianglesCount) + "\n";
			text += "Quads polygons Count: " + (BusEngine.Model.QuadsCount) + "\n";
			text += "Other Polygons Count: " + (BusEngine.Model.PolygonsCount) + "\n";
			text += "All Polygons Count: " + (BusEngine.Model.TrianglesCount + BusEngine.Model.QuadsCount + BusEngine.Model.PolygonsCount) + "\n";
			text += "Textures Count: " + BusEngine.Model.TexturesCount + "\n";
			text += "Shader Count: " + BusEngine.Shader.Count + "\n";
			//text += "Models line: " + line + "\n";
			//text += "FPS CPU Setting: " + FPSCPU + "\n";
			//text += "FPS CPU: " + CPUInfo + "\n";
			text += "FPS GPU Setting: " + FPSGPU + "\n";
			text += "FPS GPU: " + GPUInfo + "\n";
			//text += "FPS KEY Setting: " + FPSKEY + "\n";
			//text += "FPS KEY: " + KEYInfo + "\n";
			//text += "Frustum: Off" + "\n";
			text += "GPU: NVidia GT1030" + "\n";
			text += "Расстояние до объекта: " + Distance +  "\n";
			text += "Min Frustum: " + fx +  " " + fy +  " " + fz +  "\n";
			text += "Max Frustum: " + fmx +  " " + fmy +  " " + fmz +  "\n";
			text += "Расстояние до объекта: " + Distance +  "\n";
			//text += "Mouse speed: " + speed + "\n";
			//text += "VSync: " + (BusEngine.Engine.SettingProject["console_commands"]["sys_VSync"] == "1" ? "true" : "false") + "\n";
			text += "Resolution Display: " + System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Width + " X " + System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Height + "\n";
			text += "Resolution Window: " + BusEngine.UI.Canvas.WinForms.Width + " X " + BusEngine.UI.Canvas.WinForms.Height + "\n";
			text += "Resolution Setting: " + BusEngine.Engine.SettingProject["console_commands"]["r_Width"] + " X " + BusEngine.Engine.SettingProject["console_commands"]["r_Height"] + "\n";
			//text += "CPU: " + BusEngine.Engine.Device.Data["CPU"][0]["Name"] + ": " + BusEngine.Engine.Device.Data["CPU"][0]["Speed"] + " Hz " + BusEngine.Engine.Device.Data["CPU"][0]["Load"] + "/100%\n";
			//text += "GPU: " + BusEngine.Engine.Device.Data["GPU"][0]["Name"] + ": " + BusEngine.Engine.Device.Data["GPU"][0]["FreeSize"] + "/" + BusEngine.Engine.Device.Data["GPU"][0]["Size"] + " GB\n";
			//text += "RAM: " + BusEngine.Engine.Device.Data["RAM"][0]["Name"] + ": " + /* BusEngine.Engine.Device.Data["RAM"][0]["FreeSize"] */0 + "/" + BusEngine.Engine.Device.Data["RAM"][0]["Size"] + " GB\n";
			//text += "DISK: HDD " + BusEngine.Engine.Device.Data["DISK"][0]["Name"] + " " + BusEngine.Engine.Device.Data["DISK"][0]["FreeSize"] + "/" + BusEngine.Engine.Device.Data["DISK"][0]["Size"] + " GB\n";

			//text += position.ToString() + "\n";
			//text += view.ToString() + "\n";;
			//text += projection.ToString() + "\n";;

			label.Text = text;
			label.Update();
		}
		
		private static string text = "";

		private static int progLight, progView, progProjection, progTime, progMouse, progScroll, progResolution, cube, cubes = 7813, line = 75;
		private static float x, y, z = 0.0F, left = -12.0F, right = 12.0F, top = 12.0F, bottom = -12.0F;
		private static OpenTK.Vector3 v = new OpenTK.Vector3(0.0F, 1.0F, 0.0F);
		private static OpenTK.Vector3 myobjectpos = new OpenTK.Vector3(0.0F, 0.0F, 2.0F);
		//private static OpenTK.Matrix4 a, a2;

		/* private float[] Matrix4ToArray(OpenTK.Matrix4 matrix) {
			float[] data = new float[16];

			for (int i = 0; i < 4; i++) {
				for (int j = 0; j < 4; j++) {
					data[i*4+j] = matrix[i, j];

				}
			}

			return data;
		} */

		//private static long TimeNow;

		// вызывается при отрисовки каждого кадра
		public override void OnGameStart() {
			/* if (audio == null) {
				audio = new BusEngine.Audio(BusEngine.Engine.DataDirectory + "Audios/Intro.mp3");
			}
			//await System.Threading.Tasks.Task.Run(() => {
			System.Timers.Timer time = new System.Timers.Timer(1000);
			time.AutoReset = false;
			time.Elapsed += (ts, te) => {
				if (audio != null) {
					if (audio.IsPause) {
						audio.Pause();
					} else {
						audio.Play();
					}
				}
			};
			time.Interval = 1000;
			time.Enabled = true;
			//}); */

			// скрываем иконку
			BusEngine.UI.Canvas.WinForms.Cursor = new System.Windows.Forms.Cursor(new System.Drawing.Bitmap(16, 16).GetHicon());
		}

		// вызывается при отрисовки каждого кадра
		public override void OnGameStop() {
			BusEngine.UI.Canvas.WinForms.Cursor = null;

			/* if (audio != null) {
				audio.Pause();
			} */
		}

		/* public OpenTK.Vector3 GetPickingRay(float mouseX, float mouseY, int screenWidth, int screenHeight, OpenTK.Matrix4 projectionMatrix, OpenTK.Matrix4 viewMatrix) {
			// 1. Экранные координаты -> NDC (-1 to 1)
			float x = (2.0f * mouseX) / screenWidth - 1.0f;
			float y = 1.0f - (2.0f * mouseY) / screenHeight;

			// 2. Координаты в пространстве клипа
			OpenTK.Vector4 rayClip = new OpenTK.Vector4(x, y, -1.0f, 1.0f);

			// 3. Из пространства клипа в пространство глаза (Eye Space)
			OpenTK.Vector4 rayEye = OpenTK.Vector4.TransformRow(rayClip, projectionMatrix.Inverted());
			rayEye.Z = -1.0f; 
			rayEye.W = 0.0f;

			// 4. Из пространства глаза в мировое пространство (World Space)
			OpenTK.Vector3 rayWorld = OpenTK.Vector4.TransformRow(rayEye, viewMatrix.Inverted()).Xyz;
			return OpenTK.Vector3.Normalize(rayWorld);
		} */

		private static int f, k;
		public override void OnGameUpdate() {
			CPU++;

			//glControl.Update();
			//return;
			if (CPUInfo > 0) {
				FPSDelta = FPSGPU / CPUInfo;

				_angle_left -= 0.5f;
				_angle_right = 0.01f;
			}

			// fix работы мыши и клавиатуры
			k = (int)(CPUInfo / FPSKEY);

			if (k == 0) {
				k = 1;
			}

			if (CPU % k == 0) {
				//BusEngine.Game.MyPlugin.MouseMove(null, null);
				KeyDown(null, null);
				KEY++;
			}

			f = (int)(CPUInfo / FPSGPU);

			if (f == 0) {
				f = 1;
			}

			if (CPU % f == 0) {
				// чистим экран от предыдущего кадра
				//if (CPU > 20) {
				OpenTK.Graphics.OpenGL.GL.Clear(
					OpenTK.Graphics.OpenGL.ClearBufferMask.DepthBufferBit|
					//OpenTK.Graphics.OpenGL.ClearBufferMask.AccumBufferBit|
					//OpenTK.Graphics.OpenGL.ClearBufferMask.StencilBufferBit|
					//OpenTK.Graphics.OpenGL.ClearBufferMask.CoverageBufferBitNv|
					OpenTK.Graphics.OpenGL.ClearBufferMask.ColorBufferBit
				);
				//}
				//OpenTK.Graphics.OpenGL.GL.ClearColor(new BusEngine.Color(0, 0, 0, 255));
				/* OpenTK.Graphics.OpenGL.GL.ClearBufferData(
					OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer,
					OpenTK.Graphics.OpenGL.PixelInternalFormat.DepthComponent,
					OpenTK.Graphics.OpenGL.PixelFormat.DepthComponent,
					OpenTK.Graphics.OpenGL.PixelType.Float
				); */

				/* System.Threading.Tasks.Parallel.Invoke(() => {

				}); */

				//view = OpenTK.Matrix4.LookAt(position, position + front, up);
				//ExtractFrustum();

/* int scale = 10;

    // Convert to pixel Co-ordinates
    x = x / (screenWidth/2) - 1;
    y = (screenHeight - y - charHeight) / (screenHeight/2) - 1;
    Vector3 Position = new Vector3 (x, y, 0);

    // Update shader with quad position
    OpenTK.Matrix4 modelMatrix = Matrix4.CreateScale(scale) * Matrix4.CreateTranslation(Position);
    OpenTK.Graphics.OpenGL.GL.UniformMatrix4(shader.modelviewMatrixLocation, false, ref modelMatrix);

    // Draw
    OpenTK.Graphics.OpenGL.GL.BindVertexArray( VAOs[ CharSheet.IndexOf( character ) ] );
    OpenTK.Graphics.OpenGL.GL.DrawArrays(OpenTK.Graphics.OpenGL.PrimitiveType.Triangles, 0, 6); */


				// анимация поворота модели
				//float radian = OpenTK.MathHelper.DegreesToRadians(_angle_left);
				//float radian = _angle_left/OpenTK.MathHelper.Pi;
				//float radian = OpenTK.MathHelper.Pi / 180.0F * _angle_left;
				OpenTK.Matrix4 a = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.Pi / 180.0F * _angle_left);
				//OpenTK.Matrix4 a2 = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.Pi / 180.0F * _angle_right);

				// передача данных в общие шейдеры
				// переделать на буфер униформ
				// использовать юниформ буффер https://habr.com/ru/articles/350156/
				for (int i = 1, l = BusEngine.Shader.Count + 1; i < l; i++) {
					OpenTK.Graphics.OpenGL.GL.UseProgram(i);

					progProjection = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(i, "Projection");
					progView = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(i, "View");
					progTime = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(i, "Time");
					progScroll = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(i, "Scroll");
					progResolution = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(i, "Resolution");
					progMouse = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(i, "Mouse");
					progLight = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(i, "LightPos");

					OpenTK.Graphics.OpenGL.GL.UniformMatrix4(progProjection, false, ref BusEngine.Game.MyPlugin.projection);
					OpenTK.Graphics.OpenGL.GL.UniformMatrix4(progView, false, ref BusEngine.Game.MyPlugin.view);
					OpenTK.Graphics.OpenGL.GL.Uniform3(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(i, "ViewPos"), position);
					OpenTK.Graphics.OpenGL.GL.Uniform1(progTime, BusEngine.Game.MyPlugin._angle_left);
					OpenTK.Graphics.OpenGL.GL.Uniform1(progScroll, BusEngine.Game.MyPlugin.speed);
					OpenTK.Graphics.OpenGL.GL.Uniform2(progResolution, new OpenTK.Vector2(BusEngine.UI.Canvas.WinForms.Width, BusEngine.UI.Canvas.WinForms.Height));
					OpenTK.Graphics.OpenGL.GL.Uniform2(progMouse, BusEngine.Game.MyPlugin.Mouse.X, BusEngine.Game.MyPlugin.Mouse.Y);
					OpenTK.Graphics.OpenGL.GL.Uniform3(progLight, BusEngine.Model.LightPos);
				}
				
				OpenTK.Matrix4 viewProj = BusEngine.Game.MyPlugin.view * BusEngine.Game.MyPlugin.projection;
				viewProj.Transpose();

				Frustum cameraFrustum = Frustum.FromMatrix(viewProj);

				// отрисовка моделей
				foreach (BusEngine.Model model in BusEngine.Game.MyPlugin.Models) {
					if (model != null/*  && cameraFrustum.Contains(model.AABB) *//*  && PointInFrustum(model.Frustum) > 0.0f */) {
						if (model.Light) {
							Distance = OpenTK.Vector3.Distance(myobjectpos, BusEngine.Model.LightPos);

							fx = model.AABB.Min.X;
							fy = model.AABB.Min.Y;
							fz = model.AABB.Min.Z;
							fmx = model.AABB.Max.X;
							fmy = model.AABB.Max.Y;
							fmz = model.AABB.Max.Z;

							/* fx = model.fx;
							fy = model.fy;
							fz = model.fz;
							fmx = model.fmx;
							fmy = model.fmy;
							fmz = model.fmz; */
						}
						//model.View = view;
						//model.projection = projection;
						//model.A = ((m % 2) == 0 ? a : a2);
						//model.A = a;
						//model.Z += _angle_right;
						model.SwapBuffers();
					}
				}

				glControl.SwapBuffers();
				GPU++;

				/* if (BusEngine.Model.Count == 640) {
					BusEngine.Engine.Shutdown();
				} */
				//glControl.MakeCurrent();
				//glControl.Context.Update(glControl2.WindowInfo);
				//glControl2.SwapBuffers();
				//glControl2.MakeCurrent();
				//glControl2.Context.Update(glControl.WindowInfo);

				//glControl.MakeCurrent();
				//glControl.Invalidate();
				//glControl.PerformContextUpdate();
				//glControl.Context.Update(glControl.WindowInfo);
				//glControl.Context.SwapBuffers();
				//glControl.Context.MakeCurrent(null);
				//glControl.Context.MakeNoneCurrent();
				//OpenTK.Graphics.IGraphicsContext.SwapBuffers();
				//OpenTK.Graphics.OpenGL.GL.Flush();
				//OpenTK.Graphics.OpenGL.GL.Finish();
				//GrabScreenshot();
				//BusEngine.Engine.GameUpdate();
				/* System.Threading.Thread thread = new System.Threading.Thread(() => {
					GrabScreenshot();
				});

				thread.Priority = System.Threading.ThreadPriority.Lowest;
				thread.Start(); */
			}
		}

		// 2. Структура AABB (Axis-Aligned Bounding Box)
		public struct AABB {
			public OpenTK.Vector3 Min;
			public OpenTK.Vector3 Max;

			public AABB(OpenTK.Vector3 min, OpenTK.Vector3 max) {
				this.Min = min;
				this.Max = max;
			}

			/// <summary>
			/// Создает мировой AABB из локального, учитывая трансформацию (поворот, масштаб, позицию)
			/// </summary>
			public static AABB Transform(AABB localBox, OpenTK.Matrix4 modelMatrix) {
				// Извлекаем позицию из матрицы (колонна M41, M42, M43)
				OpenTK.Vector3 min = modelMatrix.ExtractTranslation();
				OpenTK.Vector3 max = min;

				// Проходим по осям X, Y, Z и вычисляем вклад каждой оси в итоговый бокс
				// Это эффективный алгоритм Джима Арво (Jim Arvo)
				
				// Ось X
				UpdateAxis(modelMatrix.M11, modelMatrix.M12, modelMatrix.M13, localBox.Min.X, localBox.Max.X, ref min, ref max);
				// Ось Y
				UpdateAxis(modelMatrix.M21, modelMatrix.M22, modelMatrix.M23, localBox.Min.Y, localBox.Max.Y, ref min, ref max);
				// Ось Z
				UpdateAxis(modelMatrix.M31, modelMatrix.M32, modelMatrix.M33, localBox.Min.Z, localBox.Max.Z, ref min, ref max);

				return new AABB(min, max);
			}

			private static void UpdateAxis(float m1, float m2, float m3, float localMin, float localMax, ref OpenTK.Vector3 worldMin, ref OpenTK.Vector3 worldMax) {
				// Считаем вклад компонента X матрицы в новые границы
				float a = m1 * localMin;
				float b = m1 * localMax;
				worldMin.X += System.Math.Min(a, b);
				worldMax.X += System.Math.Max(a, b);

				// Считаем вклад компонента Y
				a = m2 * localMin;
				b = m2 * localMax;
				worldMin.Y += System.Math.Min(a, b);
				worldMax.Y += System.Math.Max(a, b);

				// Считаем вклад компонента Z
				a = m3 * localMin;
				b = m3 * localMax;
				worldMin.Z += System.Math.Min(a, b);
				worldMax.Z += System.Math.Max(a, b);
			}

			public bool Intersect(BuslikDrev.Physics.FastRay ray, out float distance) {
				float t1 = (this.Min.X - ray.Origin.X) * ray.InvDir.X;
				float t2 = (this.Max.X - ray.Origin.X) * ray.InvDir.X;
				float t3 = (this.Min.Y - ray.Origin.Y) * ray.InvDir.Y;
				float t4 = (this.Max.Y - ray.Origin.Y) * ray.InvDir.Y;
				float t5 = (this.Min.Z - ray.Origin.Z) * ray.InvDir.Z;
				float t6 = (this.Max.Z - ray.Origin.Z) * ray.InvDir.Z;

				float tmin = (float)System.Math.Max(System.Math.Max(System.Math.Min((double)t1, (double)t2), System.Math.Min((double)t3, (double)t4)), System.Math.Min((double)t5, (double)t6));
				float tmax = (float)System.Math.Min(System.Math.Min(System.Math.Max((double)t1, (double)t2), System.Math.Max((double)t3, (double)t4)), System.Math.Max((double)t5, (double)t6));

				distance = tmin;

				// Если tmax < 0, объект находится позади луча. 
				// Если tmin > tmax, луч проходит мимо.
				return tmax >= (float)System.Math.Max(0.0, (double)tmin);
			}
		}

		// 5. Структура плоскости для Frustum Culling
		public struct Plane {
			public OpenTK.Vector3 Normal;
			public float Distance;

			public Plane(float a, float b, float c, float d) {
				float length = (float)System.Math.Sqrt((double)(a * a + b * b + c * c));
				this.Normal = new OpenTK.Vector3(a / length, b / length, c / length);
				this.Distance = d / length;
			}

			// Расстояние от точки до плоскости
			public float DotCoordinate(OpenTK.Vector3 point) {
				return OpenTK.Vector3.Dot(this.Normal, point) + this.Distance;
			}
		}

		// 6. Структура пирамиды видимости (Frustum)
		public struct Frustum {
			private Plane[] planes;

			// Создание пирамиды из матрицы View-Projection
			public static Frustum FromMatrix(OpenTK.Matrix4 matrix) {
				Frustum f = new Frustum();
				f.planes = new Plane[6];

				// Правая плоскость
				f.planes[0] = new Plane(matrix.M14 - matrix.M11, matrix.M24 - matrix.M21, matrix.M34 - matrix.M31, matrix.M44 - matrix.M41);
				// Левая плоскость
				f.planes[1] = new Plane(matrix.M14 + matrix.M11, matrix.M24 + matrix.M21, matrix.M34 + matrix.M31, matrix.M44 + matrix.M41);
				// Нижняя плоскость
				f.planes[2] = new Plane(matrix.M14 + matrix.M12, matrix.M24 + matrix.M22, matrix.M34 + matrix.M32, matrix.M44 + matrix.M42);
				// Верхняя плоскость
				f.planes[3] = new Plane(matrix.M14 - matrix.M12, matrix.M24 - matrix.M22, matrix.M34 - matrix.M32, matrix.M44 - matrix.M42);
				// Дальняя плоскость
				f.planes[4] = new Plane(matrix.M14 - matrix.M13, matrix.M24 - matrix.M23, matrix.M34 - matrix.M33, matrix.M44 - matrix.M43);
				// Ближняя плоскость
				f.planes[5] = new Plane(matrix.M14 + matrix.M13, matrix.M24 + matrix.M23, matrix.M34 + matrix.M33, matrix.M44 + matrix.M43);

				return f;
			}

			// Проверка: находится ли AABB внутри или пересекает пирамиду
			public bool Contains(AABB box) {
				for (int i = 0; i < 6; i++) {
					OpenTK.Vector3 p = box.Min;
					if (this.planes[i].Normal.X >= 0) p.X = box.Max.X;
					if (this.planes[i].Normal.Y >= 0) p.Y = box.Max.Y;
					if (this.planes[i].Normal.Z >= 0) p.Z = box.Max.Z;

					if (this.planes[i].DotCoordinate(p) < 0) {
						return false; // Объект полностью за пределами одной из плоскостей
					}
				}

				return true;
			}
		}










		/* float h = charHeight / (screenHeight / 2);
		float w = charWidth / (screenWidth / 2); */

		/* Vector2[] vertices = new Vector2[]{
			new Vector2( 0, h),
			new Vector2( w, h),
			new Vector2( w, 0),

			new Vector2( 0, h),
			new Vector2( w, 0),
			new Vector2( 0, 0)
		};

		float x1 = (charWidth / textureWidth) * i;
		float x2 = (charWidth / textureWidth) * (i + 1);

		Vector2[] textureData = new Vector2[] {
			new Vector2 (x1, 0),
			new Vector2 (x2, 0),
			new Vector2 (x2, 1),

			new Vector2 (x1, 0),
			new Vector2 (x2, 1),
			new Vector2 (x1, 1) 
		}; */

		//private static string CharSheet = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-=_+[]{}\\|;:'\".,<>/?`~ ";








		private static float[] Proj = new float[16];
		private static float[] Modl = new float[16];
		private static float[] Clip = new float[16];
		private static float[][] Frustum2 = new float[6][] {
			new float[5], new float[5], new float[5], new float[5], new float[5], new float[5]
		};
		private static float t;
		private static void ExtractFrustum2() {
		   /* Proj[0] = projection.Row0.X;
		   Proj[1] = projection.Row0.Y;
		   Proj[2] = projection.Row0.Z;
		   Proj[3] = projection.Row0.W;
		   Proj[4] = projection.Row1.X;
		   Proj[5] = projection.Row1.Y;
		   Proj[6] = projection.Row1.Z;
		   Proj[7] = projection.Row1.W;
		   Proj[8] = projection.Row2.X;
		   Proj[9] = projection.Row2.Y;
		   Proj[10] = projection.Row2.Z;
		   Proj[11] = projection.Row2.W;
		   Proj[12] = projection.Row3.X;
		   Proj[13] = projection.Row3.Y;
		   Proj[14] = projection.Row3.Z;
		   Proj[15] = projection.Row3.W;

		   Modl[0] = view.Row0.X;
		   Modl[1] = view.Row0.Y;
		   Modl[2] = view.Row0.Z;
		   Modl[3] = view.Row0.W;
		   Modl[4] = view.Row1.X;
		   Modl[5] = view.Row1.Y;
		   Modl[6] = view.Row1.Z;
		   Modl[7] = view.Row1.W;
		   Modl[8] = view.Row2.X;
		   Modl[9] = view.Row2.Y;
		   Modl[10] = view.Row2.Z;
		   Modl[11] = view.Row2.W;
		   Modl[12] = view.Row3.X;
		   Modl[13] = view.Row3.Y;
		   Modl[14] = view.Row3.Z;
		   Modl[15] = view.Row3.W;

		   //Комбинируем матрицы(перемножаем)
		   Clip[ 0] = Modl[ 0] * Proj[ 0] + Modl[ 1] * Proj[ 4] + Modl[ 2] * Proj[ 8] + Modl[ 3] * Proj[12];
		   Clip[ 1] = Modl[ 0] * Proj[ 1] + Modl[ 1] * Proj[ 5] + Modl[ 2] * Proj[ 9] + Modl[ 3] * Proj[13];
		   Clip[ 2] = Modl[ 0] * Proj[ 2] + Modl[ 1] * Proj[ 6] + Modl[ 2] * Proj[10] + Modl[ 3] * Proj[14];
		   Clip[ 3] = Modl[ 0] * Proj[ 3] + Modl[ 1] * Proj[ 7] + Modl[ 2] * Proj[11] + Modl[ 3] * Proj[15];

		   Clip[ 4] = Modl[ 4] * Proj[ 0] + Modl[ 5] * Proj[ 4] + Modl[ 6] * Proj[ 8] + Modl[ 7] * Proj[12];
		   Clip[ 5] = Modl[ 4] * Proj[ 1] + Modl[ 5] * Proj[ 5] + Modl[ 6] * Proj[ 9] + Modl[ 7] * Proj[13];
		   Clip[ 6] = Modl[ 4] * Proj[ 2] + Modl[ 5] * Proj[ 6] + Modl[ 6] * Proj[10] + Modl[ 7] * Proj[14];
		   Clip[ 7] = Modl[ 4] * Proj[ 3] + Modl[ 5] * Proj[ 7] + Modl[ 6] * Proj[11] + Modl[ 7] * Proj[15];

		   Clip[ 8] = Modl[ 8] * Proj[ 0] + Modl[ 9] * Proj[ 4] + Modl[10] * Proj[ 8] + Modl[11] * Proj[12];
		   Clip[ 9] = Modl[ 8] * Proj[ 1] + Modl[ 9] * Proj[ 5] + Modl[10] * Proj[ 9] + Modl[11] * Proj[13];
		   Clip[10] = Modl[ 8] * Proj[ 2] + Modl[ 9] * Proj[ 6] + Modl[10] * Proj[10] + Modl[11] * Proj[14];
		   Clip[11] = Modl[ 8] * Proj[ 3] + Modl[ 9] * Proj[ 7] + Modl[10] * Proj[11] + Modl[11] * Proj[15];

		   Clip[12] = Modl[12] * Proj[ 0] + Modl[13] * Proj[ 4] + Modl[14] * Proj[ 8] + Modl[15] * Proj[12];
		   Clip[13] = Modl[12] * Proj[ 1] + Modl[13] * Proj[ 5] + Modl[14] * Proj[ 9] + Modl[15] * Proj[13];
		   Clip[14] = Modl[12] * Proj[ 2] + Modl[13] * Proj[ 6] + Modl[14] * Proj[10] + Modl[15] * Proj[14];
		   Clip[15] = Modl[12] * Proj[ 3] + Modl[13] * Proj[ 7] + Modl[14] * Proj[11] + Modl[15] * Proj[15]; */

		   OpenTK.Matrix4 g = view * projection;

		   Clip[ 0] = g.Row0.X;
		   Clip[ 1] = g.Row0.Y;
		   Clip[ 2] = g.Row0.Z;
		   Clip[ 3] = g.Row0.W;

		   Clip[ 4] = g.Row1.X;
		   Clip[ 5] = g.Row1.Y;
		   Clip[ 6] = g.Row1.Z;
		   Clip[ 7] = g.Row1.W;

		   Clip[ 8] = g.Row2.X;
		   Clip[ 9] = g.Row2.Y;
		   Clip[10] = g.Row2.Z;
		   Clip[11] = g.Row2.W;

		   Clip[12] = g.Row3.X;
		   Clip[13] = g.Row3.Y;
		   Clip[14] = g.Row3.Z;
		   Clip[15] = g.Row3.W;

		   /* Находим A, B, C, D для ПРАВОЙ плоскости */
		   Frustum2[0][0] = Clip[ 3] - Clip[ 0];
		   Frustum2[0][1] = Clip[ 7] - Clip[ 4];
		   Frustum2[0][2] = Clip[11] - Clip[ 8];
		   Frustum2[0][3] = Clip[15] - Clip[12];

		   /* Приводим уравнение плоскости к нормальному виду */
		   t = (float)System.Math.Sqrt( Frustum2[0][0] * Frustum2[0][0] + Frustum2[0][1] * Frustum2[0][1] + Frustum2[0][2] * Frustum2[0][2] );
		   Frustum2[0][0] /= t;
		   Frustum2[0][1] /= t;
		   Frustum2[0][2] /= t;
		   Frustum2[0][3] /= t;

		   Frustum2[0][4] = Frustum2[0][0] + Frustum2[0][1] + Frustum2[0][2];

		   /* Находим A, B, C, D для ЛЕВОЙ плоскости */
		   Frustum2[1][0] = Clip[ 3] + Clip[ 0];
		   Frustum2[1][1] = Clip[ 7] + Clip[ 4];
		   Frustum2[1][2] = Clip[11] + Clip[ 8];
		   Frustum2[1][3] = Clip[15] + Clip[12];

		   /* Приводим уравнение плоскости к нормальному виду */
		   t = (float)System.Math.Sqrt( Frustum2[1][0] * Frustum2[1][0] + Frustum2[1][1] * Frustum2[1][1] + Frustum2[1][2] * Frustum2[1][2] );
		   Frustum2[1][0] /= t;
		   Frustum2[1][1] /= t;
		   Frustum2[1][2] /= t;
		   Frustum2[1][3] /= t;

		   Frustum2[1][4] = Frustum2[1][0] + Frustum2[1][1] + Frustum2[1][2];

		   /* Находим A, B, C, D для НИЖНЕЙ плоскости */
		   Frustum2[2][0] = Clip[ 3] + Clip[ 1];
		   Frustum2[2][1] = Clip[ 7] + Clip[ 5];
		   Frustum2[2][2] = Clip[11] + Clip[ 9];
		   Frustum2[2][3] = Clip[15] + Clip[13];

		   /* Приводим уравнение плоскости к нормальному */
		   t = (float)System.Math.Sqrt( Frustum2[2][0] * Frustum2[2][0] + Frustum2[2][1] * Frustum2[2][1] + Frustum2[2][2] * Frustum2[2][2] );
		   Frustum2[2][0] /= t;
		   Frustum2[2][1] /= t;
		   Frustum2[2][2] /= t;
		   Frustum2[2][3] /= t;

		   Frustum2[2][4] = Frustum2[2][0] + Frustum2[2][1] + Frustum2[2][2];

		   /* ВЕРХНЯЯ плоскость */
		   Frustum2[3][0] = Clip[ 3] - Clip[ 1];
		   Frustum2[3][1] = Clip[ 7] - Clip[ 5];
		   Frustum2[3][2] = Clip[11] - Clip[ 9];
		   Frustum2[3][3] = Clip[15] - Clip[13];

		   /* Нормальный вид */
		   t = (float)System.Math.Sqrt( Frustum2[3][0] * Frustum2[3][0] + Frustum2[3][1] * Frustum2[3][1] + Frustum2[3][2] * Frustum2[3][2] );
		   Frustum2[3][0] /= t;
		   Frustum2[3][1] /= t;
		   Frustum2[3][2] /= t;
		   Frustum2[3][3] /= t;

		   Frustum2[3][4] = Frustum2[3][0] + Frustum2[3][1] + Frustum2[3][2];

		   /* ЗАДНЯЯ плоскость */
		   Frustum2[4][0] = Clip[ 3] - Clip[ 2];
		   Frustum2[4][1] = Clip[ 7] - Clip[ 6];
		   Frustum2[4][2] = Clip[11] - Clip[10];
		   Frustum2[4][3] = Clip[15] - Clip[14];

		   /* Нормальный вид */
		   t = (float)System.Math.Sqrt( Frustum2[4][0] * Frustum2[4][0] + Frustum2[4][1] * Frustum2[4][1] + Frustum2[4][2] * Frustum2[4][2] );
		   Frustum2[4][0] /= t;
		   Frustum2[4][1] /= t;
		   Frustum2[4][2] /= t;
		   Frustum2[4][3] /= t;

		   Frustum2[4][4] = Frustum2[4][0] + Frustum2[4][1] + Frustum2[4][2];

		   /* ПЕРЕДНЯЯ плоскость */
		   Frustum2[5][0] = Clip[ 3] + Clip[ 2];
		   Frustum2[5][1] = Clip[ 7] + Clip[ 6];
		   Frustum2[5][2] = Clip[11] + Clip[10];
		   Frustum2[5][3] = Clip[15] + Clip[14];

		   /* Нормальный вид */
		   t = (float)System.Math.Sqrt( Frustum2[5][0] * Frustum2[5][0] + Frustum2[5][1] * Frustum2[5][1] + Frustum2[5][2] * Frustum2[5][2] );
		   Frustum2[5][0] /= t;
		   Frustum2[5][1] /= t;
		   Frustum2[5][2] /= t;
		   Frustum2[5][3] /= t;

		   Frustum2[5][4] = Frustum2[5][0] + Frustum2[5][1] + Frustum2[5][2];

		   //text = BusEngine.Tools.Json.Encode(Frustum2);
		}

		private static float PointInFrustum2(float[] f) {
			float d = 0.0f, pizda, pizda2;
			int p, i = 0, l = f.Length;
			text = BusEngine.Tools.Json.Encode(Frustum2);
			string ort = "";

			// плоскости проекции
			for (p = 0; p < 6; p++) {
				pizda = Frustum2[p][4];
				pizda2 = Frustum2[p][3];
				if (p == 4) {
					d = 10 - pizda2;
				} else {
					d = 10 - pizda2;
				}
				if (p == 0) {
					ort = "право";
				} else if (p == 1) {
					ort = "лево";
				} else if (p == 2) {
					ort = "низ";
				} else if (p == 3) {
					ort = "вверх";
				} else if (p == 4) {
					ort = "зад";
				} else if (p == 5) {
					ort = "перед";
				}
				for (i = 0; i < l; i++) {
					if (pizda * f[i] > d) {
						break;
					}
				}

				if (i == l) {
					text +=  i + " нет " + ort + " " + f[0] + " " + f[1] + " " + f[2] + " " + f[3] + " " + f[4] + " " + f[5] + " " + f[6] + " " + f[7] + " \n";
					return 0.0f;
				} else {
					text +=  i + " да " + ort + " " + f[0] + " " + f[1] + " " + f[2] + " " + f[3] + " " + f[4] + " " + f[5] + " " + f[6] + " " + f[7] + " \n";
				}
			}

			return Frustum2[4][0] * f[7] + Frustum2[4][3];
		}
		
		
		
		
		
		
		
		
		public static uint SwapBytes(System.UInt32 x) {
			// swap adjacent 16-bit blocks
			x = (x >> 16) | (x << 16);
			// swap adjacent 8-bit blocks
			return ((x & 0xFF00FF00) >> 8) | ((x & 0x00FF00FF) << 8);
		}
		//private static byte[] ScreenshotHeader;
		//private static byte[] ScreenshotBytes;
		private static byte[] bytea;
		private static int ScreenshotCol = 0;
		private static bool GrabScreenshotStatus = true;
		public static System.IntPtr bytes;
		public static System.Drawing.Imaging.PixelFormat vvvv = System.Drawing.Imaging.PixelFormat.Format32bppArgb;
		private static byte[][] Screenshots = new byte[100000][];
		private static void GrabScreenshot() {
			if (!GrabScreenshotStatus) {
				return;
			}
			OpenTK.Graphics.OpenGL.GL.Finish();
			BusEngine.Engine.GameStop();

			int r_Width;
			int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["r_Width"], out r_Width);
			if (r_Width < BusEngine.UI.Canvas.WinForms.Width) {
				r_Width = BusEngine.UI.Canvas.WinForms.Width;
			}

			int r_Height;
			int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["r_Height"], out r_Height);
			if (r_Height < BusEngine.UI.Canvas.WinForms.Height) {
				r_Height = BusEngine.UI.Canvas.WinForms.Height;
			}
			
			//OpenTK.Graphics.OpenGL.GL.PixelStore(OpenTK.Graphics.OpenGL.PixelStoreParameter.PackAlignment, 1);
			//OpenTK.Graphics.OpenGL.GL.ReadBuffer(OpenTK.Graphics.OpenGL.ReadBufferMode.Front);
			OpenTK.Graphics.OpenGL.GL.ReadPixels(0, 0, r_Width, r_Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, ggg);
			
			/* if (1 == 0) {
			//Screenshots[ScreenshotCol++] = ggg;
			ScreenshotCol++;
			int col = ScreenshotCol;
			System.Threading.Tasks.Task[] tasks = new System.Threading.Tasks.Task[1];

			tasks[0] = System.Threading.Tasks.Task.Factory.StartNew(() => {
				using (System.IO.StreamWriter sw = new System.IO.StreamWriter(BusEngine.Engine.LogDirectory + "screenshots/video" + col + ".tmp")) {
					sw.BaseStream.Write(ggg, 0, ggg.Length);
					sw.Close();
					System.Array.Clear(ggg, 0, ggg.Length);
				}
			}, System.Threading.Tasks.TaskCreationOptions.LongRunning);

			System.Threading.Tasks.Task task = System.Threading.Tasks.Task.Factory.ContinueWhenAll(tasks, (wordCountTasks) => {
				if (wordCountTasks != null) {
					foreach (System.Threading.Tasks.Task t in wordCountTasks) {
						t.Dispose();
					}
					//System.Array.Clear(wordCountTasks, 0, wordCountTasks.Length);
					//wordCountTasks = null;
				}
				if (tasks != null) {
					foreach (System.Threading.Tasks.Task t in tasks) {
						t.Dispose();
					}
				}
			});
			} */

			
			
			
			//new BusEngine.Experemental.Log(BusEngine.Engine.LogDirectory + "screenshots/video" + ScreenshotCol++ + ".tmp", ggg).Dispose();
			
			// Save JPG, JPEG, 
			/* #if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("bitmap " + System.Threading.Thread.CurrentThread.ManagedThreadId)) {
			#endif
			if (1 == 0) {
			using (System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(r_Width, r_Height)) {
				System.Drawing.Imaging.BitmapData data = bmp.LockBits(BusEngine.UI.Canvas.WinForms.ClientRectangle, System.Drawing.Imaging.ImageLockMode.WriteOnly, vvvv);

				bytes = data.Scan0;
				OpenTK.Graphics.OpenGL.GL.ReadPixels(0, 0, r_Width, r_Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, bytes);

				bmp.UnlockBits(data);
				bmp.RotateFlip(System.Drawing.RotateFlipType.RotateNoneFlipY);
				if (!System.IO.Directory.Exists(BusEngine.Engine.LogDirectory + "screenshots/")) {
					System.IO.Directory.CreateDirectory(BusEngine.Engine.LogDirectory + "screenshots/");
				}
				//new TGASharpLib.TGA(bmp).Save(BusEngine.Engine.LogDirectory + "screenshots/" + System.DateTime.Now.Minute + "-" + System.DateTime.Now.Second + "-" + System.DateTime.Now.Millisecond + ".tga");
				bmp.Save(BusEngine.Engine.LogDirectory + "screenshots/" + System.DateTime.Now.Minute + "-" + System.DateTime.Now.Second + "-" + System.DateTime.Now.Millisecond + ".png", System.Drawing.Imaging.ImageFormat.Png);
			}
			}
			#if BUSENGINE_BENCHMARK
			}
			#endif */

			// Save TGA
			/* #if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("tga " + System.Threading.Thread.CurrentThread.ManagedThreadId)) {
			#endif
			if (1 == 0) {
            using (System.IO.StreamWriter tga = new System.IO.StreamWriter(BusEngine.Engine.LogDirectory + "screenshots/" + System.DateTime.Now.Minute + "-" + System.DateTime.Now.Second + "-" + System.DateTime.Now.Millisecond + ".tga")) {
				OpenTK.Graphics.OpenGL.GL.PixelStore(OpenTK.Graphics.OpenGL.PixelStoreParameter.PackAlignment, 1);
				OpenTK.Graphics.OpenGL.GL.ReadBuffer(OpenTK.Graphics.OpenGL.ReadBufferMode.Front);
				OpenTK.Graphics.OpenGL.GL.ReadPixels(0, 0, r_Width, r_Height, OpenTK.Graphics.OpenGL.PixelFormat.Bgr, OpenTK.Graphics.OpenGL.PixelType.UnsignedByte, ScreenshotBytes);

				tga.BaseStream.Write(ScreenshotHeader, 0, ScreenshotHeader.Length);
                tga.BaseStream.Write(ScreenshotBytes, 0, ScreenshotBytes.Length);
				tga.Close();
            }
			}
			#if BUSENGINE_BENCHMARK
			}
			#endif */

			// Save PNG
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("png " + System.Threading.Thread.CurrentThread.ManagedThreadId)) {
			#endif
			if (1 == 1) {
			//using (System.IO.FileStream fs = System.IO.File.OpenRead(BusEngine.Engine.LogDirectory + "1.png")) {
				int b = 3, l = bytea.Length;
				byte[] news = new byte[] {137, 80, 78, 71, 13, 10, 26, 10}, hach = new byte[4];

				int i = 4;

				string chank = System.Text.Encoding.ASCII.GetString(new byte[] {bytea[1], bytea[2], bytea[3]});

				if (chank == "PNG") {
					string text = "";

					//BusEngine.Log.Info("IMAGE PNG");
					//Crc32 crc32 = new Crc32();
					int i2, keys = 0, w = 0, h = 0;


					for (; i < l; i++) {
						if (i+4 < l) {
							chank = System.Text.Encoding.ASCII.GetString(new byte[] {bytea[i++], bytea[i++], bytea[i++], bytea[i]});
						} else {
							break;
						}

						if (chank == "IHDR" || chank == "sRGB" || chank == "gAMA" || chank == "PLTE" || chank == "pHYs" || chank == "tEXt" || chank == "IDAT" || chank == "IEND") {
							keys = System.BitConverter.ToChar(new byte[] {bytea[i-4], bytea[i-5], bytea[i-6], bytea[i-7]}, 0);
							//BusEngine.Log.Info("CHUNK============");
							//BusEngine.Log.Info("NAME {0}", chank);
							//BusEngine.Log.Info("COUNT {0}", keys);
							// пропускаем блоки
							if (chank == "tEXt" || chank == "gAMA" || chank == "pHYs" || chank == "PLTE") {
								i += keys+4;
								//BusEngine.Log.Info("XUETA {0}", i);
								continue;
							}
							i++;
							byte[] token = new byte[keys+4];
							byte[] data = new byte[keys];
							for (; keys > 0; keys--) {
								token[token.Length-keys] = bytea[i];
								data[data.Length-keys] = bytea[i];
								text += bytea[i++] + " ";
							}
							if (chank == "IHDR") {
								w = r_Width;//System.BitConverter.ToInt32(new byte[] {data[3], data[2], data[1], data[0]}, 0);
								h = r_Height;//System.BitConverter.ToInt32(new byte[] {data[7], data[6], data[5], data[4]}, 0);
								byte[] width = System.BitConverter.GetBytes(w);
								byte[] height = System.BitConverter.GetBytes(h);
								if (b == 3 || data[8] == 8 && data[9] == 2) {
									//b = 3;
									token = new byte[] {73, 72, 68, 82, width[3], width[2], width[1], width[0], height[3], height[2], height[1], height[0], 8, 2, 0, 0, 0};
								} else if (b == 4 || data[8] == 8 && data[9] == 6) {
									//b = 4;
									token = new byte[] {73, 72, 68, 82, width[3], width[2], width[1], width[0], height[3], height[2], height[1], height[0], 8, 6, 0, 0, 0};
								}
							}
							if (chank == "IDAT") {
								//BusEngine.Log.Info("DATA LZIP (Deflate) {0}", text);


								//int outputSize;
			//BusEngine.Log.Info("decompressed.Length {0}", decompressed.Length);
								/* using (System.IO.MemoryStream memory_stream = new System.IO.MemoryStream(data, false)) {
									memory_stream.Read(decompressed, 0, 2); //discard 2 bytes

									using (System.IO.Compression.DeflateStream compressed_file = new System.IO.Compression.DeflateStream(memory_stream, System.IO.Compression.CompressionMode.Decompress)) {
										outputSize = compressed_file.Read(decompressed, 0, decompressed.Length);
										compressed_file.Close();
									}

									memory_stream.Close();
								}
								text = "";
								for (keys = 0; keys < 1000; keys++) {
									text += decompressed[keys] + " ";
								}
			BusEngine.Log.Info("decompressed.Length2 {0}", text); */


								/* text = "";
								for (keys = 0; keys < 1282*3; keys++) {
									text += decompressed[keys] + " ";
								} */
			//BusEngine.Log.Info("decompressed.Length3 {0}", text);
								//keys = ggg.Length -1;
								int keys2 = 0, s = 0, lvl = 0;
								//BusEngine.Log.Info("decompressed.Length3 {0}", ggg.Length);
								for (keys = ggg.Length; keys > 0;) {
									if (keys2 == s) {
										decompressed[keys2++] = 0;
										s += fff;

										keys -= (fff-1) * 2;

										if (keys < 0) {
											break;
										}
									}

									if (lvl == 0) {
										lvl++;
										keys += 2;
									} else if (lvl == 1) {
										lvl++;
										keys--;
									} else if (lvl == 2) {
										lvl = 0;
										keys--;
									}
									decompressed[keys2++] = ggg[keys]; // красный
									if (lvl == 0) {
										keys += 3;
									}
								}

								using (System.IO.MemoryStream memory_stream = new System.IO.MemoryStream()) {
									using (System.IO.Compression.DeflateStream compressed_file = new System.IO.Compression.DeflateStream(memory_stream, System.IO.Compression.CompressionLevel.Optimal)) {
										compressed_file.Write(decompressed, 0, decompressed.Length);
										compressed_file.Close();

										byte[] res = memory_stream.ToArray();
										token = new byte[res.Length+6];

										token[0] = 0;
										token[1] = 0;
										token[2] = 0;
										token[3] = 0;
										token[4] = data[0];
										token[5] = data[1];

										for (keys = 6; keys < token.Length; keys++) {
											token[keys] = res[keys-6];
										}
									}
								}

								/* text = "";
								for (keys = 0; keys < decompressed.Length; keys++) {
									text += decompressed[keys].ToString() + " ";
								} */

								//BusEngine.Log.Info("DATA RGB {0}", text);

								/* text = "";
								for (keys = 0; keys < token.Length; keys++) {
									text += token[keys] + " ";
								} */

								//BusEngine.Log.Info("DATA LZIP (Deflate) {0}", text);
							} else {
								//BusEngine.Log.Info("DATA {0}", text);
							}

							i += 3;
							//BusEngine.Log.Info("TOKEN LAST {0} {1} {2} {3}", bytea[i++], bytea[i++], bytea[i++], bytea[i]);

							keys = i-data.Length-7;
							token[0] = bytea[keys++];
							token[1] = bytea[keys++];
							token[2] = bytea[keys++];
							token[3] = bytea[keys];

							//BusEngine.Log.Info("chank NEW {0} {1} {2} {3}", token[0], token[1], token[2], token[3]);
							//BusEngine.Log.Info("chank NEW2 {0} {1} {2} {3}", chank[0], chank[1], chank[2], chank[3]);
							//hach = crc32.ComputeHash(token);
							hach = Crc322(token);
							//BusEngine.Log.Info("TOKEN NEW {0} {1} {2} {3}", hach[0], hach[1], hach[2], hach[3]);

							// создаём изображение
							i2 = news.Length;

							System.Array.Resize(ref news, news.Length + token.Length + 8);

							byte[] count = System.BitConverter.GetBytes(token.Length - 4);

							// количество
							for (keys = count.Length-1; keys > -1; keys--) {
								news[i2++] = count[keys];
							}

							// название
							/* for (keys = 0; keys < chank.Length; keys++) {
								news[i2++] = chank[keys];
							} */

							// данные
							for (keys = 0; keys < token.Length; keys++) {
								news[i2++] = token[keys];
							}

							// хэш
							for (keys = 0; keys < hach.Length; keys++) {
								news[i2++] = hach[keys];
							}
							
							if (chank == "tEXt") {
								//BusEngine.Log.Info("XUETA {0}", i);
								//continue;
							}
						}
					}

					System.IO.File.WriteAllBytes(BusEngine.Engine.LogDirectory + "screenshots/" + ScreenshotCol++ + ".png", news);
				}
			//}
			}
			#if BUSENGINE_BENCHMARK
			}
			#endif

			/* if (1 == 0) {
				string info = "1111", input = BusEngine.Engine.LogDirectory + "screenshots/2.png", output = BusEngine.Engine.LogDirectory + "screenshots/2.mp4";
					BusEngine.Log.Info(BusEngine.Engine.ToolsDirectory + "ffmpeg/ffmpeg.exe");
				System.Diagnostics.Process process = System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo {
					FileName = BusEngine.Engine.ToolsDirectory + "ffmpeg/ffmpeg.exe",
					Arguments = " -framerate 30 -i " + input + " -codec copy " + output,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = false
				});
				BusEngine.Log.Info(info);
				//while (!process.StandardOutput.EndOfStream) {
				//	info += process.StandardOutput.ReadLine()+ "\n";
				//}
				//BusEngine.Log.Info(info);

				while (!process.StandardError.EndOfStream) {
					info += process.StandardError.ReadLine()+ "\n";
				}

				BusEngine.Log.Info(info);

				info += "чтение завершено";
			} */

			BusEngine.Engine.GameStart();
		}
		private static uint[] crcTable;
		private static byte[] idatCrc = Crc322(new byte[] { (byte)'I', (byte)'D', (byte)'A', (byte)'T' });
		public static byte[] Crc322(byte[] stream, int offset = 0, int length = 0, uint crc = 0) {
			uint c;

			if (crcTable == null) {
				crcTable = new uint[256];

				for (uint n = 0; n <= 255; n++) {
					c = n;

					for (var k = 0; k <= 7; k++){
						if ((c & 1) == 1) {
							c = 0xEDB88320^((c>>1)&0x7FFFFFFF);
						} else {
							c = ((c>>1)&0x7FFFFFFF);
						}
					}
					crcTable[n] = c;
				}
			}

			c = crc^0xffffffff;

			if (length == 0) {
				length = stream.Length;
			}

			for (var endOffset = offset + length; offset < endOffset; offset++) {
				c = crcTable[(c^stream[offset]) & 255]^((c>>8)&0xFFFFFF);
			}

			c = c^0xffffffff;

			return new byte[] {(byte)((c >> 24) & 0xff), (byte)((c >> 16) & 0xff), (byte)((c >> 8) & 0xff), (byte)(c & 0xff)};
		}
		private void decompressFile(string inFile, string outFile) {
			System.IO.FileStream outFileStream = new System.IO.FileStream(outFile, System.IO.FileMode.Create);
			ComponentAce.Compression.Libs.zlib.ZOutputStream outZStream = new ComponentAce.Compression.Libs.zlib.ZOutputStream(outFileStream);
			System.IO.FileStream inFileStream = new System.IO.FileStream(inFile, System.IO.FileMode.Open);          
			try
			{
				//BusEngine.Log.Info(System.BitConverter.ToString(outZStream, 0));
				//CopyStream(inFileStream, outZStream);
			}
			finally
			{
				outZStream.Close();
				outFileStream.Close();
				inFileStream.Close();
			}
		}
		// перед закрытием BusEngine
		public override void Shutdown() {
			BusEngine.Engine.GameStop();
			/* if (audio != null) {
				audio.Dispose();
			} */
		}
		private static int fff;
		private static byte[] ggg;
		private static byte[] decompressed;
		private void Load(object sender, System.EventArgs e) {
			// устанавливаем контекст GL
///glControl.Context.SwapInterval = 0;
			int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["g_cubes"], out cubes);
			int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["g_cubes_line"], out line);


			// Redraw the screen every 1/20 of a second.
			_timer = new System.Timers.Timer();
			_timer.Elapsed += (ts, te) => {
				_angle_left -= 0.01f;
				_angle_right += 0.01f;
				//radian = OpenTK.MathHelper.Pi / 180.0F * _angle_left;
				//Time = te.SignalTime.Millisecond;
				//BusEngine.Log.Info("RAM: {0}", _angle_left);
			};
			_timer.Interval = 1;
			_timer.Start();
			
			System.Timers.Timer _timer6 = new System.Timers.Timer();
			_timer6.Elapsed += (ts, te) => {
				if (browser != null) {
					//browser.CaptureScreenshotAsync();
					//BusEngine.Log.Info(browser.Screenshot.Length);
					/* System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(browser.Control.Width, browser.Control.Height);
					browser.Control.DrawToBitmap(bmp, new System.Drawing.Rectangle(0, 0, browser.Control.Width, browser.Control.Height));
					bmp.Save(BusEngine.Engine.LogDirectory + "screenshots/" + System.DateTime.Now.Minute + "-" + System.DateTime.Now.Second + "-" + System.DateTime.Now.Millisecond + ".png", System.Drawing.Imaging.ImageFormat.Png);
					bmp.Dispose(); */
				}
			};
			_timer6.Interval = 60;
			_timer6.Start();

			//OpenTK.Graphics.OpenGL.All.MaxGeometryOutputVertices = 1024;

			OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
			//OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview0Ext);
			//OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Projection);
			//OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Texture);
			//OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Color);
			//OpenTK.Graphics.OpenGL.GL.LoadIdentity();
			//OpenTK.Graphics.OpenGL.GL.Ext.BeginVertexShader();

			//OpenTK.Graphics.OpenGL.GL.BeginConditionalRender(1, OpenTK.Graphics.OpenGL.ConditionalRenderType.QueryWait);


			//https://stackoverflow.com/questions/44824060/using-async-to-save-a-filestream
			int r_Width;
			int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["r_Width"], out r_Width);
			if (r_Width < BusEngine.UI.Canvas.WinForms.Width) {
				r_Width = BusEngine.UI.Canvas.WinForms.Width;
			}

			int r_Height;
			int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["r_Height"], out r_Height);
			if (r_Height < BusEngine.UI.Canvas.WinForms.Height) {
				r_Height = BusEngine.UI.Canvas.WinForms.Height;
			}


			//ScreenshotBytes = new byte[(r_Width * r_Height) * 4];
            byte[] width = System.BitConverter.GetBytes((short)r_Width);
            byte[] height = System.BitConverter.GetBytes((short)r_Height);
            //ScreenshotHeader = new byte[] {0, 0, 2, 0, 0, 0, 0, 0, 0, 0, 0, 0, width[0], width[1], height[0], height[1], 24, 0010};
			int b = 3;
			fff = (1 + r_Width * b);
			ggg = new byte[r_Width * r_Height * b];
			decompressed = new byte[fff * r_Height];

			System.IO.FileStream fs = System.IO.File.OpenRead(BusEngine.Engine.LogDirectory + "1.png");
			int l = (int)fs.Length;
			bytea = new byte[l];
			fs.Read(bytea, 0, l);
			fs.Dispose();


			/* OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.LineSmooth);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.PolygonSmooth);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.CullFace);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.DepthTest);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.StencilTest);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Dither);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Blend);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ColorLogicOp);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ScissorTest);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Texture1D);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.PolygonOffsetPoint);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.PolygonOffsetLine);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipDistance0);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane0);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipDistance1);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane1);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipDistance2);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane2);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipDistance3);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane3);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipDistance4);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane4);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipDistance5);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane5);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipDistance6);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ClipDistance7);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Convolution1D);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Convolution1DExt);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Convolution2D);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Convolution2DExt);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Separable2D);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Separable2DExt);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Histogram);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.HistogramExt);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.MinmaxExt);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.PolygonOffsetFill);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.RescaleNormal);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.RescaleNormalExt);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Texture3DExt);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.InterlaceSgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Multisample);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.MultisampleSgis);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.SampleAlphaToCoverage);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.SampleAlphaToMaskSgis);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.SampleAlphaToOne);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.SampleAlphaToOneSgis);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.SampleCoverage);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.SampleMaskSgis);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.TextureColorTableSgi);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ColorTable);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ColorTableSgi);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.PostConvolutionColorTable);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.PostConvolutionColorTableSgi);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.PostColorMatrixColorTable);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.PostColorMatrixColorTableSgi);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.Texture4DSgis);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.PixelTexGenSgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.SpriteSgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ReferencePlaneSgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.IrInstrument1Sgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.CalligraphicFragmentSgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.FramezoomSgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.FogOffsetSgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.SharedTexturePaletteExt);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.DebugOutputSynchronous);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.AsyncHistogramSgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.PixelTextureSgis);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.AsyncTexImageSgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.AsyncDrawPixelsSgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.AsyncReadPixelsSgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.FragmentLightingSgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.FragmentColorMaterialSgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.FragmentLight0Sgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.FragmentLight1Sgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.FragmentLight2Sgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.FragmentLight3Sgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.FragmentLight4Sgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.FragmentLight5Sgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.FragmentLight6Sgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.FragmentLight7Sgix);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.FogCoordArray);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.SecondaryColorArray);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ColorSum);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.TextureRectangle);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.TextureCubeMap);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.ProgramPointSize);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.VertexProgramPointSize);
			// вершинные шейдеры будут работать в двустороннем цветовом режиме - уменьшит fps
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.VertexProgramTwoSide);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.DepthClamp);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.TextureCubeMapSeamless);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.PointSprite);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.SampleShading);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.RasterizerDiscard);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.PrimitiveRestartFixedIndex);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.FramebufferSrgb);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.SampleMask);
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.PrimitiveRestart);
			// вкоючение отладки OpenGL - ловить сообщения через спец. события - уменьшит fps
			OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.DebugOutput); */


			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.LineSmooth);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.PolygonSmooth);
			// из-за Radeon - убираем возможность показа стены с двух сторон.
			// https://ravesli.com/urok-22-otsechenie-granej-v-opengl/
			//OpenTK.Graphics.OpenGL.GL.Disable(OpenTK.Graphics.OpenGL.EnableCap.CullFace);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.CullFace);
			//OpenTK.Graphics.OpenGL.GL.CullFace(OpenTK.Graphics.OpenGL.CullFaceMode.Back);
			//OpenTK.Graphics.OpenGL.GL.FrontFace(OpenTK.Graphics.OpenGL.FrontFaceDirection.Cw);

			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.DepthTest);

			// режим каркаса включён
			//OpenTK.Graphics.OpenGL.GL.PolygonMode(OpenTK.Graphics.OpenGL.MaterialFace.FrontAndBack, OpenTK.Graphics.OpenGL.PolygonMode.Point);
			//OpenTK.Graphics.OpenGL.GL.PolygonMode(OpenTK.Graphics.OpenGL.MaterialFace.FrontAndBack, OpenTK.Graphics.OpenGL.PolygonMode.Line);
			// режим каркаса отключён
			//OpenTK.Graphics.OpenGL.GL.PolygonMode(OpenTK.Graphics.OpenGL.MaterialFace.FrontAndBack, OpenTK.Graphics.OpenGL.PolygonMode.Fill);
			//OpenTK.Graphics.OpenGL.GL.PolygonMode(OpenTK.Graphics.OpenGL.MaterialFace.FrontAndBack, OpenTK.Graphics.OpenGL.PolygonMode.Line);
			
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.StencilTest);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Dither);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Blend);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ColorLogicOp);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ScissorTest);

			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.PolygonOffsetPoint);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.PolygonOffsetLine);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipDistance0);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane0);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipDistance1);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane1);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipDistance2);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane2);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipDistance3);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane3);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipDistance4);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane4);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipDistance5);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipPlane5);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipDistance6);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ClipDistance7);
			/* OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Convolution1D);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Convolution1DExt);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Convolution2D);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Convolution2DExt);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Separable2D);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Separable2DExt); */
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.PixelTextureSgis);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.PixelTexGenSgix);
			//Не даёт снимать видео
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.MinmaxExt);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.PolygonOffsetFill);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.RescaleNormal);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.RescaleNormalExt);

			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.InterlaceSgix);
			// сглаживание
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Multisample);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.MultisampleSgis);
			/* OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.SampleAlphaToCoverage);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.SampleAlphaToMaskSgis);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.SampleAlphaToOne);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.SampleAlphaToOneSgis);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.SampleCoverage);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.SampleMaskSgis);
			
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ColorTable);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ColorTableSgi); */
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.PostConvolutionColorTable);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.PostConvolutionColorTableSgi);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.PostColorMatrixColorTable);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.PostColorMatrixColorTableSgi);
			
			
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.SpriteSgix);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ReferencePlaneSgix);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.IrInstrument1Sgix);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.CalligraphicFragmentSgix);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.FramezoomSgix);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.FogOffsetSgix);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.SharedTexturePaletteExt);

			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Histogram);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.HistogramExt);

			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.AsyncHistogramSgix);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.AsyncTexImageSgix);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.AsyncDrawPixelsSgix);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.AsyncReadPixelsSgix);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.FragmentLightingSgix);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.FragmentColorMaterialSgix);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.FragmentLight0Sgix);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.FragmentLight1Sgix);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.FragmentLight2Sgix);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.FragmentLight3Sgix);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.FragmentLight4Sgix);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.FragmentLight5Sgix);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.FragmentLight6Sgix);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.FragmentLight7Sgix);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.FogCoordArray);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ColorSum);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.SecondaryColorArray);

			/* OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.TextureRectangle);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.TextureCubeMap);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.TextureCubeMapSeamless);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.TextureColorTableSgi); */
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Texture4DSgis);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Texture3DExt);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Texture1D);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);

			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.ProgramPointSize);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.VertexProgramPointSize);
			// вершинные шейдеры будут работать в двустороннем цветовом режиме - уменьшит fps
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.VertexProgramTwoSide);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.DepthClamp);
			
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.PointSprite);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.SampleShading);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.RasterizerDiscard);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.PrimitiveRestartFixedIndex);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.FramebufferSrgb);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.SampleMask);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.PrimitiveRestart);
			// включение отладки OpenGL - ловить сообщения через спец. события - уменьшит fps
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.DebugOutput);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.DebugOutputSynchronous);
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.IndexedEnableCap.Blend, 1);
			

			Resize(glControl, System.EventArgs.Empty);

			// https://github.com/8Observer8/TexturedRectangle_OpenTkOpenGL30CSharp/tree/master
			// https://github.com/StanislavPetrovV/Advanced_RayMarching/tree/main
			//OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Texture1D);
			/* OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Lighting);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Light0);
            OpenTK.Graphics.OpenGL.GL.Light(OpenTK.Graphics.OpenGL.LightName.Light0, OpenTK.Graphics.OpenGL.LightParameter.Position, new float[4] { 0, -30, 30, 0 });
            OpenTK.Graphics.OpenGL.GL.Light(OpenTK.Graphics.OpenGL.LightName.Light0, OpenTK.Graphics.OpenGL.LightParameter.Ambient, new float[4] { 1, 1, 1, 0 }); // Рассеивание
			OpenTK.Graphics.OpenGL.GL.Material(OpenTK.Graphics.OpenGL.MaterialFace.Front, OpenTK.Graphics.OpenGL.MaterialParameter.Diffuse, new float[4] { 0, 1, 0, 0 });
 */

			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Texture2D);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Texture3DExt);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Texture4DSgis);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.TextureCubeMap);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.TextureCubeMapSeamless);
			OpenTK.Graphics.OpenGL.GL.Enable(OpenTK.Graphics.OpenGL.EnableCap.Blend);
			OpenTK.Graphics.OpenGL.GL.BlendFunc(OpenTK.Graphics.OpenGL.BlendingFactor.SrcAlpha, OpenTK.Graphics.OpenGL.BlendingFactor.OneMinusSrcAlpha);

			// меняет координаты мира
			//OpenTK.Graphics.OpenGL.GL.ClipControl(OpenTK.Graphics.OpenGL.ClipOrigin.UpperLeft, OpenTK.Graphics.OpenGL.ClipDepthMode.NegativeOneToOne);
			//OpenTK.Graphics.OpenGL.GL.ClipControl(OpenTK.Graphics.OpenGL.ClipOrigin.LowerLeft, OpenTK.Graphics.OpenGL.ClipDepthMode.NegativeOneToOne);
			// настройка уровня тесселяции
			/* OpenTK.Graphics.OpenGL.GL.PatchParameter(OpenTK.Graphics.OpenGL.PatchParameterFloat.PatchDefaultInnerLevel, new float[2] {0.0F, 64.0F});
			OpenTK.Graphics.OpenGL.GL.PatchParameter(OpenTK.Graphics.OpenGL.PatchParameterFloat.PatchDefaultOuterLevel, new float[4] {0.0F, 10.0F, 20.0F, 128.0F});
			OpenTK.Graphics.OpenGL.GL.PatchParameter(OpenTK.Graphics.OpenGL.PatchParameterInt.PatchVertices, 0); */

			//int ddddd = 60;
			//OpenTK.Graphics.OpenGL.GL.QueryCounter(0, OpenTK.Graphics.OpenGL.QueryCounterTarget.Timestamp);
			//OpenTK.Graphics.OpenGL.GL.GetQueryObject(System.UInt32,OpenTK.Graphics.OpenGL.GetQueryObjectParam,System.UInt64*)

			//BusEngine.Log.Info(ddddd);

			BusEngine.Engine.GameStart();

			// ассинхронная загрузка моделей
			// https://ravesli.com/urok-35-parallaks-v-opengl/
			// http://luiscubal.blogspot.com/2013/04/asynchronous-opengl-texture-loading.html

				string dir = BusEngine.Engine.DataDirectory;
				int x16 = 16;
				float[] Pozitions;
				OpenTK.Vector3 v = new OpenTK.Vector3(1.0F, 0.0F, 0.0F);

			System.Threading.Tasks.Task.Factory.StartNew(() => {
				BusEngine.Model model;

				BusEngine.Engine.GameStop();
					/* System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/t90/t90.obj",
						//url: dir + "Models/Center City Sci-Fi/Center City Sci-Fi.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/t90/t90.obj"
					);
					model.X = 36.5f;
					model.Y = 0f;
					model.Z = -100f;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(new OpenTK.Vector3(1.0F, 0.0F, 0.0F), OpenTK.MathHelper.DegreesToRadians(90));
					//model.S = OpenTK.Matrix4.CreateScale(4.0F, 4.0F, 4.0F);
					Models[Models.Length - 1] = model;
					//model.Load(); */

					/* // перед-справа-снизу
					System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/square/square.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/RayMarching_frag.glsl"
						),
						animation: dir + "Models/square/square.obj"
					);
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(90));
					model.S = OpenTK.Matrix4.CreateScale(10.0F, 1.0F, 10.0F);
					Models[Models.Length - 1] = model;
					//model.Load();

					// перед-слева-снизу
					System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/square/square.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/RayMarching_frag.glsl"
						),
						animation: dir + "Models/square/square.obj"
					);
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(90));
					model.S = OpenTK.Matrix4.CreateScale(10.0F, 1.0F, 10.0F);
					Models[Models.Length - 1] = model;
					//model.Load();
					
					System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/square/square.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/RayMarching_frag.glsl"
						),
						animation: dir + "Models/square/square.obj"
					);
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(90));
					model.S = OpenTK.Matrix4.CreateScale(10.0F, 1.0F, 10.0F);
					Models[Models.Length - 1] = model;
					//model.Load();
					
					
					System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/square/square.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/RayMarching_frag.glsl"
						),
						animation: dir + "Models/square/square.obj"
					);
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(90));
					model.S = OpenTK.Matrix4.CreateScale(10.0F, 1.0F, 10.0F);
					Models[Models.Length - 1] = model;
					//model.Load();
					
					System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/square/square.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/RayMarching_frag.glsl"
						),
						animation: dir + "Models/square/square.obj"
					);
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(90));
					model.S = OpenTK.Matrix4.CreateScale(10.0F, 1.0F, 10.0F);
					Models[Models.Length - 1] = model;
					//model.Load();
					
					System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/square/square.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/RayMarching_frag.glsl"
						),
						animation: dir + "Models/square/square.obj"
					);
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(90));
					model.S = OpenTK.Matrix4.CreateScale(10.0F, 1.0F, 10.0F);
					Models[Models.Length - 1] = model;
					//model.Load();
					
					System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/square/square.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/RayMarching_frag.glsl"
						),
						animation: dir + "Models/square/square.obj"
					);
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(90));
					model.S = OpenTK.Matrix4.CreateScale(10.0F, 1.0F, 10.0F);
					Models[Models.Length - 1] = model;
					//model.Load();

					System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/square/square.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/RayMarching_frag.glsl"
						),
						animation: dir + "Models/square/square.obj"
					);
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(90));
					model.S = OpenTK.Matrix4.CreateScale(10.0F, 1.0F, 10.0F);
					Models[Models.Length - 1] = model;
					//model.Load();
					 */
					
					
					
					

					
					

					
					

					
					
					
					
					
					
					
					
					
					
					
					/* System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/t90_2/t90.obj",
						//url: dir + "Models/Center City Sci-Fi/Center City Sci-Fi.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/t90_2/t90.obj"
					);
					model.X = 36.5f;
					model.Y = 0f;
					model.Z = -100f;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(new OpenTK.Vector3(1.0F, 0.0F, 0.0F), OpenTK.MathHelper.DegreesToRadians(90));
					//model.S = OpenTK.Matrix4.CreateScale(4.0F, 4.0F, 4.0F);
					Models[Models.Length - 1] = model;
					//model.Load(); */



					/* System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/square/square.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/ocean1_frag.glsl"
						),
						animation: dir + "Models/square/square.obj"
					);
					model.X = 0.0F;
					model.Y = 0.0F;
					model.Z = -50.0F;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(90));
					model.S = OpenTK.Matrix4.CreateScale(10.0F, 1.0F, 10.0F);
					Models[Models.Length - 1] = model;
					//model.Load();

					System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/square/square.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/ocean2_frag.glsl"
						),
						animation: dir + "Models/square/square.obj"
					);
					model.X = 20.0F;
					model.Y = 0.0F;
					model.Z = -50.0F;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(90));
					model.S = OpenTK.Matrix4.CreateScale(10.0F, 1.0F, 10.0F);
					Models[Models.Length - 1] = model;
					//model.Load();
					
					System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/square/square.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/ocean4_frag.glsl"
						),
						animation: dir + "Models/square/square.obj"
					);
					model.X = 40.0F;
					model.Y = 0.0F;
					model.Z = -50.0F;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(90));
					model.S = OpenTK.Matrix4.CreateScale(10.0F, 1.0F, 10.0F);
					Models[Models.Length - 1] = model;
					//model.Load();


					System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/square/square.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/RayMarching_frag.glsl"
						),
						animation: dir + "Models/square/square.obj"
					);

					model.X = 60.0F;
					model.Y = 0.0F;
					model.Z = -50.0F;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(90));
					model.S = OpenTK.Matrix4.CreateScale(10.0F, 1.0F, 10.0F);
					Models[Models.Length - 1] = model;
					//model.Load();
					
					System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/square/square.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/cloud2_frag.glsl"
						),
						animation: dir + "Models/square/square.obj"
					);
					model.X = 0.0F;
					model.Y = -20.0F;
					model.Z = -50.0F;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(90));
					model.S = OpenTK.Matrix4.CreateScale(10.0F, 1.0F, 10.0F);
					Models[Models.Length - 1] = model;
					//model.Load();

					System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						context: glControl.WindowInfo,
						url: dir + "Models/square/square.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/vulcan_frag.glsl"
						),
						animation: dir + "Models/square/square.obj"
					);
					model.Textures = new string[1][] {new string[] {
						null,
						dir + "Textures/Vulcan/2.jpg"
					}};
					model.X = 20.0F;
					model.Y = -20.0F;
					model.Z = -50.0F;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(90));
					model.S = OpenTK.Matrix4.CreateScale(10.0F, 1.0F, 10.0F);
					Models[Models.Length - 1] = model;
					//model.Load();

					System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/square/square.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/RayMarching_frag.glsl"
						),
						animation: dir + "Models/square/square.obj"
					);
					model.X = 40.0F;
					model.Y = -20.0F;
					model.Z = -50.0F;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(90));
					model.S = OpenTK.Matrix4.CreateScale(10.0F, 1.0F, 10.0F);
					Models[Models.Length - 1] = model;
					//model.Load();

					System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/square/square.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/RayMarching_frag_orig2.glsl"
						),
						animation: dir + "Models/square/square.obj"
					);
					model.Textures = new string[1][] {new string[] {
						//cube
						dir + "Textures/RayMarchingOpenGL/green_marble1_bump.png",
						//floor
						dir + "Textures/RayMarchingOpenGL/hex.png",
						//walls
						dir + "Textures/RayMarchingOpenGL/white_marble1.png",
						//roof
						dir + "Textures/RayMarchingOpenGL/roof.jpg",
						//pedestal
						dir + "Textures/RayMarchingOpenGL/black_marble1.png",
						//sphere
						dir + "Textures/RayMarchingOpenGL/green_marble1.png",
						//roof bump
						dir + "Textures/RayMarchingOpenGL/roof_bump.png"
					}};
					model.X = 60.0F;
					model.Y = -20.0F;
					model.Z = -50.0F;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(90));
					model.S = OpenTK.Matrix4.CreateScale(10.0F, 1.0F, 10.0F);
					Models[Models.Length - 1] = model;
					//model.Load(); */





					/* System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/Dirt Road Forest Scene 2/Dirt Road Forest Scene.obj",
						//url: dir + "Models/Center City Sci-Fi/Center City Sci-Fi.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/Dirt Road Forest Scene/Dirt road scene.obj"
					);
					model.X = 0;
					model.Y = 0;
					model.Z = 0;
					//model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					//model.S = OpenTK.Matrix4.CreateScale(0.012F, 0.012F, 0.012F);
					Models[Models.Length - 1] = model; */


					/* System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/Railway track concrete sleepers/1524mm Rail Track Concrete.obj",
						//url: dir + "Models/Center City Sci-Fi/Center City Sci-Fi.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/Railway track concrete sleepers/1524mm Rail Track Concrete.obj"
					);
					model.X = -200;
					model.Y = 0;
					model.Z = 0;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					model.S = OpenTK.Matrix4.CreateScale(0.012F, 0.012F, 0.012F);
					Models[Models.Length - 1] = model; */
					//model.Load();

					/* System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/E-100 - Cut Mäuschen Turret/Complete Model.obj",
						//url: dir + "Models/Center City Sci-Fi/Center City Sci-Fi.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/E-100 - Cut Mäuschen Turret/Complete Model.obj"
					);
					model.X = 200f;
					model.Y = 0f;
					model.Z = -100f;
					//model.A = OpenTK.Matrix4.CreateFromAxisAngle(new OpenTK.Vector3(0.0F, 1.0F, 0.0F), OpenTK.MathHelper.DegreesToRadians(-90));
					model.S = OpenTK.Matrix4.CreateScale(20.0F, 20.0F, 20.0F);
					Models[Models.Length - 1] = model;
					//model.Load(); */

					/* System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/Meshy_AI_автомат_М16_5_0219105434_texture_obj/Meshy_AI_автомат_М16_5_0219105434_texture.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/Meshy_AI_автомат_М16_5_0219105434_texture_obj/Meshy_AI_автомат_М16_5_0219105434_texture.obj"
					);
					model.X = 36.5f;
					model.Y = 0f;
					model.Z = -20f;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(new OpenTK.Vector3(0.0F, 1.0F, 0.0F), OpenTK.MathHelper.DegreesToRadians(-90));
					model.S = OpenTK.Matrix4.CreateScale(8.0F, 8.0F, 8.0F);
					Models[Models.Length - 1] = model;
					//model.Load(); */
					
					
					
					
					
					
					
					
					
					/* System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/bus_maz_203/bus_maz_203.obj",
						//url: dir + "Models/Center City Sci-Fi/Center City Sci-Fi.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/bus_maz_203/bus_maz_203.obj"
					);
					model.X = 300;
					model.Y = 0;
					model.Z = 100;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					//model.S = OpenTK.Matrix4.CreateScale(4.0F, 4.0F, 4.0F);
					Models[Models.Length - 1] = model;
					//model.Load();

					System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/kv2/kv2.obj",
						//url: dir + "Models/Center City Sci-Fi/Center City Sci-Fi.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/kv2/kv2.obj"
					);
					model.X = -150;
					model.Y = 30;
					model.Z = 0;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					//model.S = OpenTK.Matrix4.CreateScale(4.0F, 4.0F, 4.0F);
					Models[Models.Length - 1] = model;
					//model.Load(); */



					/* System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						//url: dir + "Models/BusGameMap/BusGameMap.obj",
						url: dir + "Models/Center City Sci-Fi/Center City Sci-Fi.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/Center City Sci-Fi/Center City Sci-Fi.obj"
					);
					model.X = 260;
					model.Y = -23;
					model.Z = -120;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					model.S = OpenTK.Matrix4.CreateScale(4.0F, 4.0F, 4.0F);
					Models[Models.Length - 1] = model;
					//model.Load(); */

					/* System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/nanosuit/nanosuit.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/nanosuit/nanosuit.obj"
					);
					model.X = 0;
					model.Y = 0;
					model.Z = 10;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					model.S = OpenTK.Matrix4.CreateScale(2.54F, 2.54F, 2.54F);
					Models[Models.Length - 1] = model;
					//model.Load();
					
					System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/nanosuit2/nanosuit.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/nanosuit2/nanosuit.obj"
					);
					model.X = 0;
					model.Y = 0;
					model.Z = 20;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					model.S = OpenTK.Matrix4.CreateScale(0.211F, 0.211F, 0.211F);
					Models[Models.Length - 1] = model;
					//model.Load(); */


					/* System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/Laurence Barnes (Prophet)/Laurence Barnes.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/Laurence Barnes (Prophet)/Laurence Barnes.obj"
					);
					model.X = -40;
					model.Y = 0;
					model.Z = 20;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					model.S = OpenTK.Matrix4.CreateScale(1.0F, 1.0F, 1.0F);
					Models[Models.Length - 1] = model;
					//model.Load(); */


					/* System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/traffic_lights/t1_200.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/traffic_lights/t1_200.obj"
					);
					model.X = 20;
					model.Y = 0;
					model.Z = 0;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					model.S = OpenTK.Matrix4.CreateScale(2.0F, 2.0F, 2.0F);
					Models[Models.Length - 1] = model;
					//model.Load(); */




					/* System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/bugatti/bugatti.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//geom: dir + "Shaders/cube_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/bugatti/bugatti.obj"
					);
					model.X = 0;
					model.Y = 0;
					model.Z = 0;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					Models[Models.Length - 1] = model;
					//model.Load(); */

					/* System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/afro/african_head.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/afro/african_head.obj"
					);
					model.X = 0;
					model.Y = 0;
					model.Z = 0;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					//model.S = OpenTK.Matrix4.CreateScale(0.25F, 0.25F, 0.25F);
					Models[Models.Length - 1] = model;
					//model.Load(); */




					/* System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/light/light.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/light_vert.glsl",
							//tesc: dir + "Shaders/light_tesc.glsl",
							//tess: dir + "Shaders/light_tess.glsl",
							geom: dir + "Shaders/light_geom.glsl",
							frag: dir + "Shaders/light_frag.glsl",
							//comp: dir + "Shaders/light_comp.glsl",
							incl: dir + "Shaders/hg_sdf.glsl"
						),
						animation: dir + "Models/light/light.obj"
					);
					model.Light = true;
					model.X = 0;
					model.Y = 0;
					model.Z = 0;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					model.S = OpenTK.Matrix4.CreateScale(100000.0f, 100000.0f, 100000.0f);
					
					Models[Models.Length - 1] = model;
					//model.Load(); */

					for (int i = 0; i < 10; i++) {
						for (int ii = 0; ii < 10; ii++) {
							for (int iii = 0; iii < 10; iii++) {
					System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/terrain/Globe2.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/terrain/Globe2.obj"
					);
					model.X = 40000.0F * i;
					model.Y = -40000.0F * ii;
					model.Z = 40000.0F * iii;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(90));
					model.S = OpenTK.Matrix4.CreateScale(400.0F, 400.0F, 400.0F);
					Models[Models.Length - 1] = model;
					//model.Load();
							}
						}
					}


					/* System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/terrain/terrain.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							//geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/terrain/terrain.obj"
					);
					model.X = 0;
					model.Y = -10000;
					model.Z = 0;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					model.S = OpenTK.Matrix4.CreateScale(10000.0f, 10000.0f, 10000.0f);
					
					Models[Models.Length - 1] = model;
					//model.Load();

					BusEngine.Game.MyPlugin.Mouse.X = model.X;
					BusEngine.Game.MyPlugin.Mouse.Y = model.Y; */





					/* System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/nanosuit2/nanosuit.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/example_tess_vert.glsl",
							tesc: dir + "Shaders/example_tess_tesc.glsl",
							tess: dir + "Shaders/example_tess_tess.glsl",
							//geom: dir + "Shaders/example_tess_geom.glsl",
							frag: dir + "Shaders/example_tess_frag.glsl"
						),
						animation: dir + "Models/nanosuit2/nanosuit.obj"
					);
					model.X = 60.0F;
					model.Y = -20.0F;
					model.Z = -50.0F;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					model.S = OpenTK.Matrix4.CreateScale(0.211F, 0.211F, 0.211F);
					Models[Models.Length - 1] = model;
					//model.Load(); */




					System.Array.Resize(ref Models, Models.Length + 1);
					model = new BusEngine.Model(
						url: dir + "Models/light/light.obj",
						name: "",
						/* shader: new BusEngine.Shader(
							vert: dir + "Shaders/light_vert.glsl",
							//tesc: dir + "Shaders/light_tesc.glsl",
							//tess: dir + "Shaders/light_tess.glsl",
							geom: dir + "Shaders/light_geom.glsl",
							frag: dir + "Shaders/light_frag.glsl",
							//comp: dir + "Shaders/light_comp.glsl",
							incl: dir + "Shaders/hg_sdf.glsl"
						), */
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert2.glsl",
							geom: dir + "Shaders/cube_geom2.glsl",
							frag: dir + "Shaders/cube_frag2.glsl"
						),
						animation: dir + "Models/light/light.obj"
					);
					model.Material = dir + "Models/light/light.mtl";
					model.Light = true;
					model.X = 2;
					model.Y = 2;
					model.Z = 2;
					//model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					//model.S = OpenTK.Matrix4.CreateScale(0.5f, 0.5f, 0.5f);
					
					Models[Models.Length - 1] = model;
					//model.Load();

					//BusEngine.Game.MyPlugin.Mouse.X = model.X;
					//BusEngine.Game.MyPlugin.Mouse.Y = model.Y;





				/* System.Array.Resize(ref Models, Models.Length + 1);
				model = new BusEngine.Model(
					url: dir + "Models/light/light.obj",
					name: "",
					shader: new BusEngine.Shader(
						vert: dir + "Shaders/cube_vert.glsl",
						geom: dir + "Shaders/cube_geom.glsl",
						frag: dir + "Shaders/cube_frag.glsl"
					),
					animation: dir + "Models/light/light.obj"
				);
				//model.Name = "100";
				model.X = 0;
				model.Y = 0;
				model.Z = 2;
				//model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
				Models[Models.Length - 1] = model;
				//model.Load(); */






				//cubes = 31252;
				cube = Models.Length;
				cubes = 0 * 4 + cube;
				x = 0;
				y = 0;
				z = 0;
				left = -20.0F;
				right = 20.0F;
				top = 20.0F;
				bottom = -20.0F;
				x16 = 25;

				Pozitions = new float[x16 * 6];

				for (int pi = 0, px = 0, py = 0; pi < Pozitions.Length; pi++) {
					Pozitions[pi++] = 4.0F * px;
					Pozitions[pi++] = 4.0F * py;
					Pozitions[pi++] = 0.0f;

					Pozitions[pi++] = 0.0f;
					Pozitions[pi++] = 0.0f;
					Pozitions[pi] = 0.0f;

					if (px == 4) {
						py++;
						px = 0;
					} else {
						px++;
					}
				}

				System.Array.Resize(ref Models, cubes);

				for (; cube < cubes;) {
				//System.Threading.Tasks.Parallel.For(cube, cubes, async (i) => {
					model = new BusEngine.Model(
						url: dir + "Models/cube/cube.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert.glsl",
							geom: dir + "Shaders/cube_geom.glsl",
							frag: dir + "Shaders/cube_frag.glsl"
						),
						animation: dir + "Models/cube/cube.obj"
					);
					model.Light = true;
					model.Pozitions = Pozitions;
					model.X = 0;
					model.Y = 0;
					model.Z = 0;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					Models[cube++] = model;
					//model.Load();

					model = new BusEngine.Model(
						url: dir + "Models/cube/cube.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert.glsl",
							geom: dir + "Shaders/cube_geom.glsl",
							frag: dir + "Shaders/cube_frag.glsl"
						),
						animation: dir + "Models/cube/cube.obj"
					);
					model.Pozitions = Pozitions;
					model.X = right * x - right;
					model.Y = bottom * y;
					model.Z = z;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					Models[cube++] = model;
					//model.Load();

					model = new BusEngine.Model(
						url: dir + "Models/cube/cube.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert.glsl",
							geom: dir + "Shaders/cube_geom.glsl",
							frag: dir + "Shaders/cube_frag.glsl"
						),
						animation: dir + "Models/cube/cube.obj"
					);
					model.Pozitions = Pozitions;
					model.X = left * x;
					model.Y = top * y - top;
					model.Z = z;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					Models[cube++] = model;
					//model.Load();

					model = new BusEngine.Model(
						url: dir + "Models/cube/cube.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/cube_vert.glsl",
							geom: dir + "Shaders/cube_geom.glsl",
							frag: dir + "Shaders/cube_frag.glsl"
						),
						animation: dir + "Models/cube/cube.obj"
					);
					
					model.Pozitions = Pozitions;
					model.X = right * x - right;
					model.Y = top * y - top;
					model.Z = z;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					Models[cube++] = model;
					//model.Load();

					// убираем второй массив
					if (y == line) {
						x++;
						y = 1;
					} else {
						y++;
					}
					
				//});
				}

				
				cube = Models.Length;
				cubes = 0 * 4 + cube;
				x = 1;
				y = 1;
				z = 0;
				left = -50;
				right = 50;
				top = 100;
				bottom = -100;
				x16 = 1;
				line = 2;

				Pozitions = new float[x16 * 6];

				for (int pi = 0, px = 0, py = 0; pi < Pozitions.Length; pi++) {
					Pozitions[pi++] = 10.0F * px;
					Pozitions[pi++] = 20.0F * py;
					Pozitions[pi++] = 0.0f;

					Pozitions[pi++] = 0.0f;
					Pozitions[pi++] = 0.0f;
					Pozitions[pi] = 0.0f;

					if (px == 3) {
						py++;
						px = 0;
					} else {
						px++;
					}
				}

				System.Array.Resize(ref Models, cubes);

				for (; cube < cubes;) {
				//System.Threading.Tasks.Parallel.For(0, cube, (i) => {
					model = new BusEngine.Model(
						url: dir + "Models/nanosuit2/nanosuit.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/nanosuit2/nanosuit.obj"
					);
					model.Pozitions = Pozitions;
					model.X = left * x;
					model.Y = bottom * y;
					model.Z = z;
					//model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					//model.S = OpenTK.Matrix4.CreateScale(0.211F, 0.211F, 0.211F);
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(new OpenTK.Vector3(0.0F, 1.0F, 0.0F), OpenTK.MathHelper.DegreesToRadians(180));
					model.S = OpenTK.Matrix4.CreateScale(0.09F, 0.09F, 0.09F);
					Models[cube++] = model;
					//model.Load();

					model = new BusEngine.Model(
						url: dir + "Models/nanosuit2/nanosuit.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/nanosuit2/nanosuit.obj"
					);
					model.Pozitions = Pozitions;
					model.X = right * x - right;
					model.Y = bottom * y;
					model.Z = z;
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(new OpenTK.Vector3(0.0F, 1.0F, 0.0F), OpenTK.MathHelper.DegreesToRadians(180));
					model.S = OpenTK.Matrix4.CreateScale(0.09F, 0.09F, 0.09F);
					Models[cube++] = model;
					//model.Load();

					model = new BusEngine.Model(
						url: dir + "Models/nanosuit2/nanosuit.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/nanosuit2/nanosuit.obj"
					);
					model.Pozitions = Pozitions;
					model.X = left * x;
					model.Y = top * y - top;
					model.Z = z;
					//model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					//model.S = OpenTK.Matrix4.CreateScale(0.211F, 0.211F, 0.211F);
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(new OpenTK.Vector3(0.0F, 1.0F, 0.0F), OpenTK.MathHelper.DegreesToRadians(180));
					model.S = OpenTK.Matrix4.CreateScale(0.09F, 0.09F, 0.09F);
					Models[cube++] = model;
					//model.Load();

					model = new BusEngine.Model(
						url: dir + "Models/nanosuit2/nanosuit.obj",
						name: "",
						shader: new BusEngine.Shader(
							vert: dir + "Shaders/crysis_vert.glsl",
							//tesc: dir + "Shaders/crysis_tesc.glsl",
							//tess: dir + "Shaders/crysis_tess.glsl",
							geom: dir + "Shaders/crysis_geom.glsl",
							frag: dir + "Shaders/crysis_frag.glsl"
						),
						animation: dir + "Models/nanosuit2/nanosuit.obj"
					);
					model.Pozitions = Pozitions;
					model.X = right * x - right;
					model.Y = top * y - top;
					model.Z = z;
					//model.A = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(0));
					//model.S = OpenTK.Matrix4.CreateScale(0.211F, 0.211F, 0.211F);
					model.A = OpenTK.Matrix4.CreateFromAxisAngle(new OpenTK.Vector3(0.0F, 1.0F, 0.0F), OpenTK.MathHelper.DegreesToRadians(180));
					model.S = OpenTK.Matrix4.CreateScale(0.09F, 0.09F, 0.09F);
					Models[cube++] = model;
					//model.Load();

					// убираем второй массив
					if (y == line) {
						x++;
						y = 1;
					} else {
						y++;
					}
				//});
				}
position = new OpenTK.Vector3(0.0F, 0.0F, 10.0F);
				BusEngine.Engine.GameStart();
			}).ContinueWith(task => {
				System.Threading.Tasks.Task.Factory.StartNew(async () => {
				int ii = 0;
				foreach (BusEngine.Model model_1 in Models) {
					await model_1.Load();
					/* if (ii > 0 && ii < 9) {
						model_1.X = Models[0].Selection[ii-1].X;
						model_1.Y = Models[0].Selection[ii-1].Y;
						model_1.Z = Models[0].Selection[ii-1].Z;
					} */
					
					ii++;
				}
				
				});
			});
			
			
		}
	}
	/** API BusEngine.Plugin */
}
/** API BusEngine.Game - пользовательский код */

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.Log
BusEngine.Shader
*/
	/** API BusEngine.Level */
	//[System.Serializable]
	public class Level : System.IDisposable {
		//[System.NonSerialized]
		//private bool BufferStatus = false;
		//[System.NonSerialized]
		//private OpenTK.Graphics.OpenGL.BeginMode BeginMode;
		//private OpenTK.Graphics.OpenGL.PrimitiveType PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;

		// цвет точек квадратных полигонов
		//[System.Xml.Serialization.XmlArray("cd")]
		//[System.Xml.Serialization.XmlArrayItem("c")]
		public BusEngine.Color[] ColorData;

		[System.Xml.Serialization.XmlIgnore]
		public static int Count { get; private set; }
		[System.Xml.Serialization.XmlIgnore]
		public static int TrianglesCount { get; private set; }
		[System.Xml.Serialization.XmlIgnore]
		public static int QuadsCount { get; private set; }
		[System.Xml.Serialization.XmlIgnore]
		public static int PolygonsCount { get; private set; }

		//[System.Xml.Serialization.XmlArray("vd")]
		//[System.Xml.Serialization.XmlArrayItem("v")]
		public BusEngine.Vector3[] VertexData;
		//[System.Xml.Serialization.XmlArray("tc")]
		//[System.Xml.Serialization.XmlArrayItem("v")]
		public BusEngine.Vector3[] TexData;
		//[System.Xml.Serialization.XmlArray("nc")]
		//[System.Xml.Serialization.XmlArrayItem("v")]
		public BusEngine.Vector3[] NormData;
		//[System.Xml.Serialization.XmlArray("id")]
		//[System.Xml.Serialization.XmlArrayItem("v")]
		public int[] VertexIndex;
		//[System.Xml.Serialization.XmlArray("ti")]
		//[System.Xml.Serialization.XmlArrayItem("v")]
		public int[] TexIndex;
		//[System.Xml.Serialization.XmlArray("ni")]
		//[System.Xml.Serialization.XmlArrayItem("v")]
		public int[] NormIndex;

		//[System.NonSerialized]
		//[System.Xml.Serialization.XmlIgnore]
		public string Mode { get; set; }
		[System.NonSerialized]
		[System.Xml.Serialization.XmlIgnore]
		public OpenTK.Platform.IWindowInfo Context;
		//[System.NonSerialized]
		//[System.Xml.Serialization.XmlIgnore]
		public string Url = "";
		public string Name = "";
		[System.NonSerialized]
		[System.Xml.Serialization.XmlIgnore]
		public BusEngine.Shader Shader;
		[System.NonSerialized]
		[System.Xml.Serialization.XmlIgnore]
		public string Animation = "";
		[System.NonSerialized]
		[System.Xml.Serialization.XmlIgnore]
		public int Program;
		//[System.NonSerialized]
		[System.Xml.Serialization.XmlIgnore]
		public float X { get { return P.Row3.X; } set { P.Row3.X = value;} }
		//[System.NonSerialized]
		[System.Xml.Serialization.XmlIgnore]
		public float Y { get { return P.Row3.Y; } set { P.Row3.Y = value;} }
		//[System.NonSerialized]
		[System.Xml.Serialization.XmlIgnore]
		public float Z { get { return P.Row3.Z; } set { P.Row3.Z = value;} }
		[System.NonSerialized]
		[System.Xml.Serialization.XmlIgnore]
		public float Height;
		[System.NonSerialized]
		[System.Xml.Serialization.XmlIgnore]
		public float Width;
		[System.NonSerialized]
		[System.Xml.Serialization.XmlIgnore]
		public float Length;
		[System.NonSerialized]
		[System.Xml.Serialization.XmlIgnore]
		public BusEngine.Material Material;
		[System.NonSerialized]
		[System.Xml.Serialization.XmlIgnore]
		public OpenTK.Matrix4 VP, A = OpenTK.Matrix4.CreateFromAxisAngle(new OpenTK.Vector3(1.0F, 0.0F, 0.0F), OpenTK.MathHelper.Pi / 180.0F * -90), P = OpenTK.Matrix4.CreateTranslation(0.0F, 0.0F, 0.0F);

		public Level() {
			
		}

		public Level(OpenTK.Platform.IWindowInfo context = null, string url = "", string name = "", BusEngine.Shader shader = null, string animation = "") {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Model Initialize "+ url + " " + System.Threading.Thread.CurrentThread.ManagedThreadId)) {
			#endif

			Context = context;
			Url = url;
			Name = name;
			Shader = shader;

			if (Shader.GetType().GetField("Program") != null || Shader.GetType().GetProperty("Program") != null) {
				//this.Program = Shader.Program;
			}

			// поворот
			P.Row0.X = A.Row0.X;
			P.Row0.Y = A.Row0.Y;
			P.Row0.Z = A.Row0.Z;
			P.Row1.X = A.Row1.X;
			P.Row1.Y = A.Row1.Y;
			P.Row1.Z = A.Row1.Z;
			P.Row2.X = A.Row2.X;
			P.Row2.Y = A.Row2.Y;
			P.Row2.Z = A.Row2.Z;

			// позиция
			P.Row3.X = X;
			P.Row3.Y = Y;
			P.Row3.Z = Z;

			#if BUSENGINE_BENCHMARK
			}
			#endif
		}

		public bool Load() {
			return Load(Url);
		}

		public bool Load(string url = "") {
			return true;
		}

		System.Runtime.Serialization.Formatters.Binary.BinaryFormatter binarySerializer = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
		System.Xml.Serialization.XmlSerializer xmlSerializer = new System.Xml.Serialization.XmlSerializer(typeof(Model));

		public void Dispose() {

		}

		~Level() {
			BusEngine.Log.Info("Level ~ " + this.Program + " " + this.Url);
		}
	}
	/** API BusEngine.Level */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.Log
*/
    /** API BusEngine.Object */
	public class Object : System.IDisposable {
		public Object() {
			
		}

		public void Dispose() {

		}

		~Object() {
			BusEngine.Log.Info("Object ~");
		}
	}
    /** API BusEngine.Object */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.Log
*/
    /** API BusEngine.Shader */
	public class Shader : System.IDisposable {
		public string Vert, Vertex, Tesc, Tescontrol, Tess, Tessellation, Geom, Geometry, Frag, Fragment, Mesh, MeshShader, Comp, Compute, Incl, Include;
		public string BUSENGINE_URL = "";
		public int BUSENGINE_COPY_INT = 3;
		internal static string[] Data = new string[10000];
		internal static int Count { get; private set; }
		private int program;

		private int CompileShader(OpenTK.Graphics.OpenGL.ShaderType type, string source = "") {
			int success, shader = OpenTK.Graphics.OpenGL.GL.CreateShader(type);

			if (System.IO.File.Exists(source)) {
				source = System.IO.File.ReadAllText(source);
			}

			source = source.Replace("define BUSENGINE_COPY_INT", "define BUSENGINE_COPY_INT " + BUSENGINE_COPY_INT);

			OpenTK.Graphics.OpenGL.GL.ShaderSource(shader, source);

			success = 1;
			// для подключения разлчных функций
			if (Include != "") {
				OpenTK.Graphics.OpenGL.GL.Arb.CompileShaderInclude(shader, 1, new string[1] {Include}, ref success);
			}
			OpenTK.Graphics.OpenGL.GL.CompileShader(shader);

			OpenTK.Graphics.OpenGL.GL.GetShader(shader, OpenTK.Graphics.OpenGL.ShaderParameter.CompileStatus, out success);
			if (success == 0) {
				throw new System.Exception("Failed to compile {type}: " + OpenTK.Graphics.OpenGL.GL.GetShaderInfoLog(shader));
			}

			return shader;
		}

		internal int GenProgram() {
			// проверка на пробел
			if (string.IsNullOrWhiteSpace(Vert)) {
				this.Vert = "";
				this.Vertex = "";
			} else {
				this.Vertex = Vert;
			}
			if (string.IsNullOrWhiteSpace(Vertex)) {
				this.Vert = "";
				this.Vertex = "";
			}

			if (string.IsNullOrWhiteSpace(Tesc)) {
				this.Tesc = "";
				this.Tescontrol = "";
			} else {
				this.Tescontrol = Tesc;
			}
			if (string.IsNullOrWhiteSpace(Tescontrol)) {
				this.Tesc = "";
				this.Tescontrol = "";
			}

			if (string.IsNullOrWhiteSpace(Tess)) {
				this.Tess = "";
				this.Tessellation = "";
			} else {
				this.Tessellation = Tess;
			}
			if (string.IsNullOrWhiteSpace(Tessellation)) {
				this.Tess = "";
				this.Tessellation = "";
			}

			if (string.IsNullOrWhiteSpace(Geom)) {
				this.Geom = "";
				this.Geometry = "";
			} else {
				this.Geometry = Geom;
			}
			if (string.IsNullOrWhiteSpace(Geometry)) {
				this.Geom = "";
				this.Geometry = "";
			}

			if (string.IsNullOrWhiteSpace(Frag)) {
				this.Frag = "";
				this.Fragment = "";
			} else {
				this.Fragment = Frag;
			}
			if (string.IsNullOrWhiteSpace(Fragment)) {
				this.Frag = "";
				this.Fragment = "";
			}

			if (string.IsNullOrWhiteSpace(Mesh)) {
				this.Mesh = "";
				this.MeshShader = "";
			} else {
				this.MeshShader = Mesh;
			}
			if (string.IsNullOrWhiteSpace(MeshShader)) {
				this.Mesh = "";
				this.MeshShader = "";
			}

			if (string.IsNullOrWhiteSpace(Comp)) {
				this.Comp = "";
				this.Compute = "";
			} else {
				this.Compute = Comp;
			}
			if (string.IsNullOrWhiteSpace(Compute)) {
				this.Comp = "";
				this.Compute = "";
			}

			if (string.IsNullOrWhiteSpace(Incl)) {
				this.Incl = "";
				this.Include = "";
			} else {
				this.Include = Incl;
			}
			if (string.IsNullOrWhiteSpace(Include)) {
				this.Incl = "";
				this.Include = "";
			}

			// проверяем повтор шейдера
			string from = Vertex + Tescontrol + Tessellation + Geometry + Fragment + MeshShader + Compute + Include + BUSENGINE_COPY_INT + BUSENGINE_URL;

			program = System.Array.IndexOf(Data, from);

			if (program != -1) {
				return program;
			} else {
				program = OpenTK.Graphics.OpenGL.GL.CreateProgram();
				if (Count >= Data.Length) {
					System.Array.Resize(ref Data, Count + 10000);
				}
				Data[program] = from;
				Count++;
			}

			// создаём программу (шейдер)
			int success = 0, vert = 0, tesc = 0, tess = 0, geom = 0, frag = 0, mesh = 0, comp = 0;

			// для управления вершинами полигона
			if (Vertex != "") {
				//BusEngine.Log.Info("Vertex {0}", OpenTK.Graphics.OpenGL.ShaderType.VertexShader);
				vert = CompileShader(OpenTK.Graphics.OpenGL.ShaderType.VertexShader, Vertex);
				OpenTK.Graphics.OpenGL.GL.AttachShader(program, vert);
			}

			if (Tescontrol != "") {
				//BusEngine.Log.Info("Tescontrol {0}", OpenTK.Graphics.OpenGL.ShaderType.TessControlShader);
				tesc = CompileShader(OpenTK.Graphics.OpenGL.ShaderType.TessControlShader, Tescontrol);
				OpenTK.Graphics.OpenGL.GL.AttachShader(program, tesc);
			}

			// для создания дополнительных полигонов в целях сглаживания краёв или создания копии модели
			if (Tessellation != "") {
				//BusEngine.Log.Info("Tessellation {0}", OpenTK.Graphics.OpenGL.ShaderType.TessEvaluationShader);
				tess = CompileShader(OpenTK.Graphics.OpenGL.ShaderType.TessEvaluationShader, Tessellation);
				OpenTK.Graphics.OpenGL.GL.AttachShader(program, tess);
				//OpenTK.Graphics.OpenGL.GL.GetProgram(program, OpenTK.Graphics.OpenGL.GetProgramParameterName.TessGenMode, out success);
			}

			// для управления группой треугольных полигонов (в зависимости от видеокарты до 128 вершин = 42 полигона), увеличение полигонов через этот шейдер - повышает производительность
			if (Geometry != "") {
				//BusEngine.Log.Info("Geometry {0}", OpenTK.Graphics.OpenGL.ShaderType.GeometryShader);
				geom = CompileShader(OpenTK.Graphics.OpenGL.ShaderType.GeometryShader, Geometry);
				OpenTK.Graphics.OpenGL.GL.AttachShader(program, geom);
			}

			// для управления каждым пикселем экрана (управление тенями, освещением, отражением, цветом, текстурами и т.д.)
			if (Fragment != "") {
				//BusEngine.Log.Info("Fragment {0}", OpenTK.Graphics.OpenGL.ShaderType.FragmentShader);
				frag = CompileShader(OpenTK.Graphics.OpenGL.ShaderType.FragmentShader, Fragment);
				OpenTK.Graphics.OpenGL.GL.AttachShader(program, frag);
			}

			// для управления мешем
			// #extension GL_NV_mesh_shader : require
			// https://zone.dog/braindump/mesh_shaders/
			// https://registry.khronos.org/OpenGL/extensions/NV/NV_mesh_shader.txt
			/* if (MeshShader != "") {
				//BusEngine.Log.Info("MeshShader {0}", OpenTK.Graphics.OpenGL.ShaderType.MeshShader);
				mesh = CompileShader(OpenTK.Graphics.OpenGL.ShaderType.MeshShader, MeshShader);
				OpenTK.Graphics.OpenGL.GL.AttachShader(program, mesh);
			} */

			// для инных вычеслений (генерация частиц, изображений) https://steps3d.narod.ru/tutorials/compute-shaders-tutorial.html
			if (Compute != "") {
				//BusEngine.Log.Info("Compute {0}", OpenTK.Graphics.OpenGL.ShaderType.ComputeShader);
				comp = CompileShader(OpenTK.Graphics.OpenGL.ShaderType.ComputeShader, Compute);
				OpenTK.Graphics.OpenGL.GL.AttachShader(program, comp);
			}

			OpenTK.Graphics.OpenGL.GL.LinkProgram(program);
			OpenTK.Graphics.OpenGL.GL.GetProgram(program, OpenTK.Graphics.OpenGL.GetProgramParameterName.LinkStatus, out success);

			if (success == 0) {
				throw new System.Exception("Could not link program: " + OpenTK.Graphics.OpenGL.GL.GetProgramInfoLog(program));
			}

			if (Vertex != "") {
				OpenTK.Graphics.OpenGL.GL.DetachShader(program, vert);
				OpenTK.Graphics.OpenGL.GL.DeleteShader(vert);
			}

			if (Tescontrol != "") {
				OpenTK.Graphics.OpenGL.GL.DetachShader(program, tesc);
				OpenTK.Graphics.OpenGL.GL.DeleteShader(tesc);
			}

			if (Tessellation != "") {
				OpenTK.Graphics.OpenGL.GL.DetachShader(program, tess);
				OpenTK.Graphics.OpenGL.GL.DeleteShader(tess);
			}

			if (Geometry != "") {
				OpenTK.Graphics.OpenGL.GL.DetachShader(program, geom);
				OpenTK.Graphics.OpenGL.GL.DeleteShader(geom);
			}

			if (Fragment != "") {
				OpenTK.Graphics.OpenGL.GL.DetachShader(program, frag);
				OpenTK.Graphics.OpenGL.GL.DeleteShader(frag);
			}

			if (MeshShader != "") {
				OpenTK.Graphics.OpenGL.GL.DetachShader(program, mesh);
				OpenTK.Graphics.OpenGL.GL.DeleteShader(mesh);
			}

			if (Compute != "") {
				OpenTK.Graphics.OpenGL.GL.DetachShader(program, comp);
				OpenTK.Graphics.OpenGL.GL.DeleteShader(comp);
			}

			return program;
		}

		public Shader() {}

		public Shader(string vert = "", string vertex = "", string tesc = "", string tescontrol = "", string tess = "", string tessellation = "", string geom = "", string geometry = "", string frag = "", string fragment = "", string mesh = "", string meshshader = "", string comp = "", string compute = "", string incl = "", string include = "") : this() {
			this.Vert = vert;
			this.Vertex = vertex;
			this.Tesc = tesc;
			this.Tescontrol = tescontrol;
			this.Tess = tess;
			this.Tessellation = tessellation;
			this.Geom = geom;
			this.Geometry = geometry;
			this.Frag = frag;
			this.Fragment = fragment;
			this.Mesh = mesh;
			this.MeshShader = meshshader;
			this.Comp = comp;
			this.Compute = compute;
			this.Incl = incl;
			this.Include = include;

			//System.GC.SuppressFinalize(this);
		}

		public void Dispose() {
			if (OpenTK.Graphics.OpenGL.GL.IsProgram(program)) {
				OpenTK.Graphics.OpenGL.GL.DeleteProgram(program);

				System.GC.SuppressFinalize(this);
			}
		}

		~Shader() {
			//BusEngine.Log.Info("Shader ~ " + this.program);
		}
	}
    /** API BusEngine.Shader */
}
/** API BusEngine */

/** API BusEngine */
namespace BusEngine {
/*
Зависит от плагинов:
BusEngine.Log
BusEngine.Material
BusEngine.Matrix
BusEngine.Shader
BusEngine.Texture
BusEngine.Vector
OpenTK
*/
	/** API BusEngine.Model */
	public class Model : System.IDisposable {
		public bool Active = true;
		public bool Static = false;
		public bool Light = false;
		private OpenTK.Graphics.OpenGL.PrimitiveType[] PrimitiveType;
		private OpenTK.Graphics.OpenGL.BeginMode[] BeginMode;
		private bool Loaded;
		public int BufferStatus;
		private int CacheStatus;

		public static int Count { get; private set; }
		public static int TrianglesCount { get; private set; }
		public static int QuadsCount { get; private set; }
		public static int PolygonsCount { get; private set; }
		public static int TexturesCount { get; private set; }

		public int[] VAOs;
		public int[][] TIs;
		public float[][][] ColorData;
		public string[][] Textures;

		public BusEngine.Vector3[][] VertexData;
		public BusEngine.Vector2[][] TexData;
		public BusEngine.Vector3[][] NormData;
		// 1, 2, 3, 4 - точки спереди по часовой для отсечения модели.
		// 5, 6, 7, 8 - точки зади по часовой для отсечения модели.
		public BusEngine.Vector3[] Selection = new BusEngine.Vector3[8]; // координаты куба отсечения и выделения модели
		public float[] Frustum = new float[8];
		private float[] frustum = new float[8];
		public BusEngine.Game.MyPlugin.AABB AABB;
		public float fx = float.MaxValue, fy = float.MaxValue, fz = float.MaxValue, fmx = float.MinValue, fmy = float.MinValue, fmz = float.MinValue;

		public string[] Mode;
		public string[] Groups;
		public float[] Pozitions;
		public string Url = "";
		public string Name = "";
		public BusEngine.Shader Shader;
		public string Animation = "";
		public int Program, D = 1;

		public float Height;
		public float Width;
		public float Length;

		//public BusEngine.Material Material;
		public string Material = "";
		public OpenTK.Matrix4 A = OpenTK.Matrix4.CreateFromAxisAngle(new OpenTK.Vector3(0.0F, 1.0F, 0.0F), OpenTK.MathHelper.Pi / 180.0F * 0), P = OpenTK.Matrix4.CreateTranslation(0.0F, 0.0F, 0.0F), S = OpenTK.Matrix4.CreateScale(1.0F, 1.0F, 1.0F);
		public float X { get { return P.Row3.X; } set { P.Row3.X = value; } }
		public float Y { get { return P.Row3.Y; } set { P.Row3.Y = value; } }
		public float Z { get { return P.Row3.Z; } set { P.Row3.Z = value; } }
		private int VBO, TBO, NBO, progTexStatus, progColorDefault, progPozitions, progDistance, progAngle, progModel;
		private int sys_CacheModel;

		public Model() {
			
		}

		public Model(bool light = false, string url = "", string name = "", BusEngine.Shader shader = null, string material = "", string animation = "", float[] pozitions = null) {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Model Initialize "+ url + " " + System.Threading.Thread.CurrentThread.ManagedThreadId)) {
			#endif

			int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["sys_CacheModel"], out sys_CacheModel);

			Url = url;
			Name = name;
			Shader = shader;
			Material = material;
			PrimitiveType = new OpenTK.Graphics.OpenGL.PrimitiveType[0];
			BeginMode = new OpenTK.Graphics.OpenGL.BeginMode[0];
			Mode = new string[0];
			Groups = new string[0];
			Pozitions = pozitions;
			Light = light;

			//System.GC.SuppressFinalize(this);

			#if BUSENGINE_BENCHMARK
			}
			#endif
		}

		public async System.Threading.Tasks.Task Load() {
			await Load(Url);
		}

		private static int DataModelsCount = 0;
		public async System.Threading.Tasks.Task Load(string url = "") {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Model Loaded "+ url + " " + System.Threading.Thread.CurrentThread.ManagedThreadId)) {
			#endif

			Url = url;
			Loaded = false;
			if (Light) {
				BusEngine.Model.LightPos = new OpenTK.Vector3(X, Y, Z);
			}
			string path = System.IO.Path.GetDirectoryName(url) + '/', filename = System.IO.Path.GetFileNameWithoutExtension(url);

			int id = System.Array.IndexOf(DataModels, path + filename);

			if (sys_CacheModel > 1 && id != -1) {
				BusEngine.Model model = BusEngine.Game.MyPlugin.Models[id];

				VertexData = model.VertexData;
				TexData = model.TexData;
				NormData = model.NormData;
				if (Shader == null) {
					Shader = model.Shader;
				}
				if (Material == "") {
					Material = model.Material;
				}

				Program = model.Program;
				Programs = model.Program;

				ColorData = model.ColorData;
				Textures = model.Textures;
				VAOs = model.VAOs;
				TIs = model.TIs;

				Mode = model.Mode;
				Groups = model.Groups;
				Pozitions = model.Pozitions;
				if (Name == "") {
					Name = model.Name;
				}
				Animation = model.Animation;
				Selection = model.Selection;
				Frustum = model.Frustum;
				
				D = model.D;

				fx = model.fx;
				fy = model.fy;
				fz = model.fz;
				fmx = model.fmx;
				fmy = model.fmy;
				fmz = model.fmz;

				if (D > 1) {
					float x = float.MinValue, y = float.MinValue, z = float.MinValue;

					for (id = 0; id < Pozitions.Length; id += 6) {
						if (x < Pozitions[id]) {
							x = Pozitions[id];
						}
						if (y < Pozitions[id+1]) {
							y = Pozitions[id+1];
						}
						if (z < Pozitions[id+2]) {
							z = Pozitions[id+2];
						}
					}

					fmx = x + (fx + fmx) / 2.0f;
					fmy = y + (fy + fmy) / 2.0f;
					fmz = z + (fz + fmz) / 2.0f;
				}

				//AABB = model.AABB;
				//AABB = new BusEngine.Game.MyPlugin.AABB(new OpenTK.Vector3(fx + X, fy + Y, fz + Z), new OpenTK.Vector3(fmx + X, fmy + Y, fmz + Z));



				Count += D;

				System.Array.Resize(ref this.PrimitiveType, Groups.Length);
				System.Array.Resize(ref this.BeginMode, Groups.Length);

				int i = 0;

				foreach (string m in Mode) {
					if (m == "polygon") {
						PrimitiveType[i] = OpenTK.Graphics.OpenGL.PrimitiveType.Polygon;
						BeginMode[i] = OpenTK.Graphics.OpenGL.BeginMode.Polygon;
					} else if (m == "quads") {
						PrimitiveType[i] = OpenTK.Graphics.OpenGL.PrimitiveType.Quads;
						BeginMode[i] = OpenTK.Graphics.OpenGL.BeginMode.Quads;
					} else if (m == "triangles") {
						PrimitiveType[i] = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;
						BeginMode[i] = OpenTK.Graphics.OpenGL.BeginMode.Triangles;
					} else if (m == "lines") {
						PrimitiveType[i] = OpenTK.Graphics.OpenGL.PrimitiveType.Lines;
						BeginMode[i] = OpenTK.Graphics.OpenGL.BeginMode.Lines;
					} else if (m == "points") {
						PrimitiveType[i] = OpenTK.Graphics.OpenGL.PrimitiveType.Points;
						BeginMode[i] = OpenTK.Graphics.OpenGL.BeginMode.Points;
					} else {
						PrimitiveType[i] = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;
						BeginMode[i] = OpenTK.Graphics.OpenGL.BeginMode.Triangles;
					}

					if (m == "quads") {
						QuadsCount += VertexData[i].Length/4 * D;
					} else if (m == "triangles") {
						TrianglesCount += VertexData[i].Length/3 * D;
					} else {
						PolygonsCount += VertexData[i].Length/5 * D;
					}

					i++;
				}

				DataModels[DataModelsCount] = "";

				Loaded = true;
			} else/*  if (sys_CacheModel > 0 && System.IO.File.Exists(path + filename + ".bem")) {
				// загружаем в формате движка .bem
				await GetCache(path + filename + ".bem");

				DataModels[DataModelsCount] = path + filename;
			} else */ {
				await import(url);

				DataModels[DataModelsCount] = path + filename;
			}

			DataModelsCount++;

			if (DataModelsCount >= DataModels.Length) {
				System.Array.Resize(ref DataModels, DataModelsCount + 10000);
			}

			#if BUSENGINE_BENCHMARK
			}
			#endif
		}

		private bool export(string url) {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Model Export "+ url + " " + System.Threading.Thread.CurrentThread.ManagedThreadId)) {
			#endif

			if (!System.IO.File.Exists(url)) {
				System.ConsoleColor c = System.Console.ForegroundColor;
				System.Console.ForegroundColor = System.ConsoleColor.Red;
				BusEngine.Log.Info(BusEngine.Localization.GetLanguageStatic("error_browser_url") + ": " + url);
				System.Console.ForegroundColor = c;

				return false;
			} else {
				return false;
			}

			#if BUSENGINE_BENCHMARK
			}
			#endif
		}
/* 1. По коду и структурам данных
Структуры vs Классы: Вы правильно заметили, что структуры (Vector3) передаются быстрее. В C# структуры — это value types, они лучше ложатся в память (cache locality) и не нагружают Garbage Collector (GC), что критично для стабильного FPS.
Dictionary vs List/Array: В видео вы упоминаете «словарь-лист». Для загрузки данных лучше использовать List<T> с заранее заданным Capacity (если известно количество вершин) или обычные массивы. Словари (Dictionary) имеют накладные расходы на хэширование ключей, что при сотнях тысяч вершин может замедлять процесс.
String Manipulation: В парсере OBJ вы используете Split(' ') и Replace. Это создает огромное количество временных строк в куче, которые потом приходится чистить сборщику мусора. Для ускорения парсинга текстовых файлов лучше использовать ReadOnlySpan<char> — это позволит парсить числа прямо из строки без создания новых объектов.
2. Оптимизация бинарного кэша
Memory-Mapped Files: Вместо того чтобы читать весь бинарный файл в массив (ReadAllBytes), можно использовать Memory-Mapped Files. Это позволит операционной системе «пробросить» файл напрямую в адресное пространство процесса. Для больших сцен это работает быстрее, чем обычное чтение.
SIMD (Single Instruction, Multiple Data): Если вы делаете какие-то преобразования координат при загрузке, можно использовать векторы из System.Numerics. Это задействует инструкции процессора (SSE/AVX), позволяя обрабатывать 4 или 8 чисел за один такт.
3. Работа с GPU (для GT1030)
Instancing: Вы упомянули, что копируете модели в шейдере. Если это 640 одинаковых моделей, то использование DrawIndexedInstanced — это ключ к успеху. Вместо того чтобы посылать 640 команд на отрисовку, вы посылаете одну команду и массив трансформаций (позиции, повороты). Для GT1030 это критично, так как её слабое место — пропускная способность и количество вызовов отрисовки (Draw Calls).
Staging Buffers: При загрузке бинарных данных в видеокарту убедитесь, что вы используете промежуточные буферы (Staging Buffers), если работаете с Vulkan или DX12/11, чтобы передача данных шла через DMA (Direct Memory Access), не блокируя CPU.


1. Использование Mip-map уровней в DDS
Поскольку в DDS уже зашиты слои разного разрешения (Mip-maps), вы можете реализовать стратегию «от мутного к четкому»:

Шаг 1: Сначала загружаете только самый маленький (последний) слой (например, 16x16 или 32x32 пикселя). Он весит копейки и залетает в GPU мгновенно. Объект в кадре уже не прозрачный, а имеет цвет.
Шаг 2: В фоновом потоке подгружаете основные слои и обновляете текстуру.
2. Фоновая загрузка (Async Upload)
Чтобы не было микро-фризов («заиканий» движка) во время загрузки, в OpenGL есть два пути:

PBO (Pixel Buffer Objects): Вы копируете данные из файла в буфер PBO в отдельном потоке. Команда glTexSubImage2D при использовании PBO становится асинхронной — она просто дает команду DMA-контроллеру перекинуть данные в видеопамять, не блокируя основной поток рендеринга.
Shared Contexts: Создание второго контекста OpenGL специально для загрузки ресурсов. Но PBO обычно проще и стабильнее в реализации.
3. Приоритезация (как в WoT)
В танках текстуры объектов, которые ближе к камере, грузятся первыми. Раз вы уже считаете расстояния до объектов и используете Bounding Boxes, вы можете:

Сортировать очередь загрузки текстур по дистанции до камеры.
Если объект далеко — грузить только низкий Mip-level.
Если игрок приближается — догружать детали.
4. Bindless Textures (если позволяет карта)
Если ваша GT1030 поддерживает расширение GL_ARB_bindless_texture, вы можете уйти от классических glBindTexture. Это позволит передавать в шейдер просто массив «ручек» (64-битных указателей) на текстуры. Это невероятно ускоряет отрисовку, когда моделей (и текстур) очень много.

Технический нюанс: Для DDS в OpenGL используйте функцию glCompressedTexImage2D. Это позволит загрузить сжатые данные (в форматах DXT1/DXT5 или BC7) без лишних преобразований.


https://learn.microsoft.com/ru-ru/dotnet/api/system.io.memorymappedfiles.memorymappedfile?view=net-10.0

HR-ы запомните не Пэ-Хэ-Пэ, а Пи Эйч Пи, а потом уже набирайте программистов на работу

*/



		//https://help.nira.app/hc/en-us/articles/4418314558363-OBJ-color-and-MTL-material-settings
		private async System.Threading.Tasks.Task import(string url) {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Model Import "+ url + " " + System.Threading.Thread.CurrentThread.ManagedThreadId)) {
			#endif

			if (!System.IO.File.Exists(url)) {
				System.ConsoleColor c = System.Console.ForegroundColor;
				System.Console.ForegroundColor = System.ConsoleColor.Red;
				BusEngine.Log.Info("Failed to open the import model: " + url);
				System.Console.ForegroundColor = c;
			} else {
				string path = System.IO.Path.GetDirectoryName(Url) + '/', extension = System.IO.Path.GetExtension(url).ToLower();

				if (extension != ".obj") {
					System.ConsoleColor c = System.Console.ForegroundColor;
					System.Console.ForegroundColor = System.ConsoleColor.Red;
					BusEngine.Log.Info("Failed to open the model format: " + url);
					System.Console.ForegroundColor = c;

					return;
				}

				using (System.IO.StreamReader sr = new System.IO.StreamReader(url)) {
					int g = 0, i, ii = 0, l, all = 0, c = 0;
					float x, y, z;
					string line, name = "";
					string[] values, val;
					bool st = false, m = false;

					System.Collections.Generic.List<BusEngine.Vector3> VertexDataNew = new System.Collections.Generic.List<BusEngine.Vector3>();
					System.Collections.Generic.List<BusEngine.Vector2> TexDataNew = new System.Collections.Generic.List<BusEngine.Vector2>();
					System.Collections.Generic.List<BusEngine.Vector3> NormDataNew = new System.Collections.Generic.List<BusEngine.Vector3>();
					System.Collections.Generic.List<string>[] index = new System.Collections.Generic.List<string>[0];

					if (Pozitions == null || Pozitions.Length == 0) {
						Pozitions = new float[0];
						D = 1;
					} else {
						D = Pozitions.Length / 6;
					}

					while (true) {
						line = await sr.ReadLineAsync();

						if (line == null) {
							// конец
							all = index.Length;

							VertexData = new BusEngine.Vector3[all][];
							TexData = new BusEngine.Vector2[all][];
							NormData = new BusEngine.Vector3[all][];
							//System.Collections.Generic.List<BusEngine.Vector3> VertexDataStaticNew = new System.Collections.Generic.List<BusEngine.Vector3>();

							for (g = 0; g < all; g++) {
								l = index[g].Count;

								VertexData[g] = new BusEngine.Vector3[l];
								TexData[g] = new BusEngine.Vector2[l];
								NormData[g] = new BusEngine.Vector3[l];

								for (i = 0; i < l; i++) {
									val = index[g][i].Split('/');

									if (val.Length > 0 && val[0] != "") {
										int.TryParse(val[0], out ii);
										ii--;
										VertexData[g][i] = new BusEngine.Vector3(VertexDataNew[ii].X, VertexDataNew[ii].Y, VertexDataNew[ii].Z);
										//VertexDataStaticNew.Add(VertexData[g][i]);
										// находим минимальный вектор
										if (fx > VertexData[g][i].X) {
											fx = VertexData[g][i].X;
										}
										if (fy > VertexData[g][i].Y) {
											fy = VertexData[g][i].Y;
										}
										if (fz > VertexData[g][i].Z) {
											fz = VertexData[g][i].Z;
										}
										// находим максимальный вектор
										if (fmx < VertexData[g][i].X) {
											fmx = VertexData[g][i].X;
										}
										if (fmy < VertexData[g][i].Y) {
											fmy = VertexData[g][i].Y;
										}
										if (fmz < VertexData[g][i].Z) {
											fmz = VertexData[g][i].Z;
										}
									}

									if (val.Length > 1 && val[1] != "") {
										int.TryParse(val[1], out ii);
										ii--;
										TexData[g][i] = new BusEngine.Vector2(TexDataNew[ii].X, 1.0f - TexDataNew[ii].Y);
									}

									if (val.Length > 2 && val[2] != "") {
										int.TryParse(val[2], out ii);
										ii--;
										NormData[g][i] = new BusEngine.Vector3(NormDataNew[ii].X, NormDataNew[ii].Y, NormDataNew[ii].Z);
									}
								}

								if (Mode[g] == "quads") {
									QuadsCount += l/4 * D;
								} else if (Mode[g] == "triangles") {
									TrianglesCount += l/3 * D;
								} else {
									PolygonsCount += l/5 * D;
								}

								index[g].Clear();
							}

							//VertexDataStatic = VertexDataStaticNew.ToArray();
							//VertexDataStaticNew.Clear();
							VertexDataNew.Clear();
							TexDataNew.Clear();
							NormDataNew.Clear();
							System.Array.Clear(index, 0, index.Length);

							break;
						}

						if (line == "" || line.StartsWith("#")) {
							continue;
						}

						//values = System.Text.RegularExpressions.Regex.Split(line, @"\s+");
						values = line.Replace("\t", "").Replace("  ", " ").Split(' ');

						l = values.Length;

						if (l == 0) {
							continue;
						}

						if (values[0] == "o") {
							name += line.Substring(2) + " ";
						} else if (values[0] == "g") {
							name += line.Substring(2) + " ";
						} else if (values[0] == "usemtl") {
							System.Array.Resize(ref this.Groups, this.Groups.Length + 1);
							Groups[g++] = line.Substring(7);
							st = false;
							m = true;
						} else if (values[0] == "mtllib") {
							if (Material == "" || !System.IO.File.Exists(Material)) {
								Material = line.Substring(7);
							}
						} else if (values[0] == "v") {
							float.TryParse(values[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out x);
							float.TryParse(values[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out y);
							float.TryParse(values[3], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out z);

							VertexDataNew.Add(new BusEngine.Vector3(x, y, z));
						} else if (values[0] == "vt") {
							float.TryParse(values[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out x);
							float.TryParse(values[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out y);

							TexDataNew.Add(new BusEngine.Vector2(x, y));
						} else if (values[0] == "vn") {
							float.TryParse(values[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out x);
							float.TryParse(values[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out y);
							float.TryParse(values[3], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out z);

							NormDataNew.Add(new BusEngine.Vector3(x, y, z));
						} else if (values[0] == "vp") {
							
						} else if (values[0] == "s") { // это индекс - он не нужен - свой автоматом получаем и удаляем
							
						} else if (values[0] == "f") {
							if (!st) {
								st = true;

								if (m) {
									c = g - 1;
								} else {
									c = g;
								}

								System.Array.Resize(ref index, index.Length + 1);
								System.Array.Resize(ref this.Mode, this.Mode.Length + 1);
								System.Array.Resize(ref this.PrimitiveType, this.PrimitiveType.Length + 1);
								System.Array.Resize(ref this.BeginMode, this.BeginMode.Length + 1);

								index[c] = new System.Collections.Generic.List<string>();

								if (l > 5) {
									Mode[c] = "polygon";
									PrimitiveType[c] = OpenTK.Graphics.OpenGL.PrimitiveType.Polygon;
									BeginMode[c] = OpenTK.Graphics.OpenGL.BeginMode.Polygon;
								} else if (l == 5) {
									Mode[c] = "quads";
									PrimitiveType[c] = OpenTK.Graphics.OpenGL.PrimitiveType.Quads;
									BeginMode[c] = OpenTK.Graphics.OpenGL.BeginMode.Quads;
								} else if (l == 4) {
									Mode[c] = "triangles";
									PrimitiveType[c] = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;
									BeginMode[c] = OpenTK.Graphics.OpenGL.BeginMode.Triangles;
								} else if (l == 3) {
									Mode[c] = "lines";
									PrimitiveType[c] = OpenTK.Graphics.OpenGL.PrimitiveType.Lines;
									BeginMode[c] = OpenTK.Graphics.OpenGL.BeginMode.Lines;
								} else if (l == 2) {
									Mode[c] = "points";
									PrimitiveType[c] = OpenTK.Graphics.OpenGL.PrimitiveType.Points;
									BeginMode[c] = OpenTK.Graphics.OpenGL.BeginMode.Points;
								} else {
									Mode[c] = "triangles";
									PrimitiveType[c] = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;
									BeginMode[c] = OpenTK.Graphics.OpenGL.BeginMode.Triangles;
								}
							}

							for (ii = 1; ii < l; ii++) {
								index[c].Add(values[ii]);
							}
						}
					}

					if (Name == "") {
						Name = name;
					}

					ColorData = new float[g][][];
					Textures = new string[g][];
					VAOs = new int[g];
					TIs = new int[g][];

					// доработать сделав через максимальный вектор среди всех векторов Pozitions
					if (D > 1) {
						x = float.MinValue;
						y = float.MinValue;
						z = float.MinValue;

						for (i = 0; i < Pozitions.Length; i += 6) {
							if (x < Pozitions[i]) {
								x = Pozitions[i];
							}
							if (y < Pozitions[i+1]) {
								y = Pozitions[i+1];
							}
							if (z < Pozitions[i+2]) {
								z = Pozitions[i+2];
							}
						}

						fmx = (System.Math.Abs(fx) + System.Math.Abs(fmx)) / 2.0f + x;
						fmy = (System.Math.Abs(fy) + System.Math.Abs(fmy)) / 2.0f + y;
						fmz = (System.Math.Abs(fz) + System.Math.Abs(fmz)) / 2.0f + z;
					}

					AABB = new BusEngine.Game.MyPlugin.AABB(new OpenTK.Vector3(fx, fy, fz), new OpenTK.Vector3(fmx, fmy, fmz));

					Selection = new BusEngine.Vector3[8] {
						new BusEngine.Vector3(0.0f, 0.0f, 0.0f),
						new BusEngine.Vector3(0.0f, fy, 0.0f),
						new BusEngine.Vector3(fx, 0.0f, 0.0f),
						new BusEngine.Vector3(fx, fy, 0.0f),
						new BusEngine.Vector3(0.0f, 0.0f, fz),
						new BusEngine.Vector3(0.0f, fy, fz),
						new BusEngine.Vector3(fx, fy, fz),
						new BusEngine.Vector3(fx, 0.0f, fz)
					};

					frustum = new float[8] {
						0.0f,
						fy/3,
						fx/3,
						(fx + fy)/3,
						fz/3,
						(fy + fz)/3,
						(fx + fy + fz)/3,
						(fx + fz)/3
					};

					Frustum = frustum;

					//AABB = new BusEngine.Game.MyPlugin.AABB(new OpenTK.Vector3(fx + X, fy + Y, fz + Z), new OpenTK.Vector3(fmx + X, fmy + Y, fmz + Z));
					Count += D;

					// сохраняем в формат движка .bem
					if (sys_CacheModel > 0 && CacheStatus == 0) {
						CacheStatus = 1;
						string filename = System.IO.Path.GetFileNameWithoutExtension(Url);

						await SetCache(path + filename + ".bem");
					}

					// Material
					if (!System.IO.File.Exists(Material)) {
						Material = path + Material;
					}

					await importMaterial(Material);
				}
			}

			/* if (Mode == "points") {
				//BeginMode = OpenTK.Graphics.OpenGL.BeginMode.Points;
				PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.Points;
			} else if (Mode == "lines") {
				//BeginMode = OpenTK.Graphics.OpenGL.BeginMode.Lines;
				PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.Lines;
			} else if (Mode == "lineloop") {
				//BeginMode = OpenTK.Graphics.OpenGL.BeginMode.LineLoop;
				PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.LineLoop;
			} else if (Mode == "linestrip") {
				//BeginMode = OpenTK.Graphics.OpenGL.BeginMode.LineStrip;
				PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.LineStrip;
			} else if (Mode == "linesadjacency") {
				//BeginMode = OpenTK.Graphics.OpenGL.BeginMode.LinesAdjacency;
				PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.LinesAdjacency;
			} else if (Mode == "linestripadjacency") {
				//BeginMode = OpenTK.Graphics.OpenGL.BeginMode.LineStripAdjacency;
				PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.LineStripAdjacency;
			} else if (Mode == "triangles") {
				//BeginMode = OpenTK.Graphics.OpenGL.BeginMode.Triangles;
				PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;
			} else if (Mode == "trianglestrip") {
				//BeginMode = OpenTK.Graphics.OpenGL.BeginMode.TriangleStrip;
				PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.TriangleStrip;
			} else if (Mode == "trianglefan") {
				//BeginMode = OpenTK.Graphics.OpenGL.BeginMode.TriangleFan;
				PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.TriangleFan;
			} else if (Mode == "trianglesadjacency") {
				//BeginMode = OpenTK.Graphics.OpenGL.BeginMode.TrianglesAdjacency;
				PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.TrianglesAdjacency;
			} else if (Mode == "trianglestripadjacency") {
				//BeginMode = OpenTK.Graphics.OpenGL.BeginMode.TriangleStripAdjacency;
				PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.TriangleStripAdjacency;
			} else if (Mode == "quads") {
				//BeginMode = OpenTK.Graphics.OpenGL.BeginMode.Quads;
				PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.Quads;
			} else if (Mode == "quadstrip") {
				//BeginMode = OpenTK.Graphics.OpenGL.BeginMode.QuadStrip;
				PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.QuadStrip;
			} else if (Mode == "polygon") {
				//BeginMode = OpenTK.Graphics.OpenGL.BeginMode.Polygon;
				PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.Polygon;
			} else if (Mode == "patches") {
				//BeginMode = OpenTK.Graphics.OpenGL.BeginMode.Patches;
				PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.Patches;
			} else {
				//BeginMode = OpenTK.Graphics.OpenGL.BeginMode.Triangles;
				PrimitiveType = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;
			} */

			#if BUSENGINE_BENCHMARK
			}
			#endif
		}

		private async System.Threading.Tasks.Task importMaterial(string url) {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Model Import "+ url + " " + System.Threading.Thread.CurrentThread.ManagedThreadId)) {
			#endif

			// https://paulbourke.net/dataformats/mtl/
			// http://www.martinreddy.net/gfx/3d/OBJ.spec
			// https://help.nira.app/hc/en-us/articles/4418314558363-OBJ-color-and-MTL-material-settings
			// https://stackoverflow.com/questions/26453143/how-is-normaltexture-represented-in-the-wavefront-resource-material-format
			// http://demofox.org/WebGLPBR/
			// https://learnopengl.com/PBR/Theory
			if (url != "" && System.IO.File.Exists(url)) {
				string extension = System.IO.Path.GetExtension(url).ToLower();

				if (extension != ".mtl") {
					System.ConsoleColor c = System.Console.ForegroundColor;
					System.Console.ForegroundColor = System.ConsoleColor.Red;
					BusEngine.Log.Info("Failed to open the material format: " + url);
					System.Console.ForegroundColor = c;

					return;
				}

				using (System.IO.StreamReader sr = new System.IO.StreamReader(new System.IO.BufferedStream(System.IO.File.OpenRead(url), 1024*1024))) {
					int g = 0, l, ll = 0;
					float x, y, z, w;
					string[] values;
					string line, name = "", dir = BusEngine.Engine.DataDirectory, path = System.IO.Path.GetDirectoryName(url) + '/';
					BusEngine.Color color;
					System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<int, string>> mtl = new System.Collections.Generic.Dictionary<string, System.Collections.Generic.Dictionary<int, string>>(Groups.Length, System.StringComparer.OrdinalIgnoreCase);

					while (true) {
						line = await sr.ReadLineAsync();

						if (line == null) {
							break;
						}

						if (line.StartsWith("#")) {
							continue;
						}

						//values = System.Text.RegularExpressions.Regex.Split(line, @"\s+");
						values = line.Replace("\t", "").Replace("  ", " ").Split(' ');

						l = values.Length;

						if (l == 0) {
							continue;
						}

						if (values[0] == "newmtl") { // название материала
							name = line.Substring(7);
							mtl[name] = new System.Collections.Generic.Dictionary<int, string>(25);
							mtl[name][0] = name;
							mtl[name][1] = "";
							mtl[name][2] = "";
							mtl[name][3] = "";
							mtl[name][4] = "";
							mtl[name][5] = "";
							mtl[name][6] = "";
							mtl[name][7] = "";
							mtl[name][8] = "";
							mtl[name][9] = "";
							mtl[name][10] = "";
							mtl[name][11] = "";
							mtl[name][12] = "";
							mtl[name][13] = "";
							mtl[name][14] = "";
							mtl[name][15] = "";
							mtl[name][16] = "";
							mtl[name][17] = "";
							mtl[name][18] = "";
							mtl[name][19] = "";
							mtl[name][20] = "";
							mtl[name][21] = "";
							mtl[name][22] = "";
							mtl[name][23] = "";
							mtl[name][24] = "";
						} else if (values[0] == "Ka") { // ambient Цвет окружающего освещения (жёлтый)
							mtl[name][1] = line.Substring(3);
						} else if (values[0] == "Kd") { // diffuse Диффузный цвет (белый)
							mtl[name][2] = line.Substring(3);
						} else if (values[0] == "Ks") { // specular Цвет зеркального отражения (0;0;0 - выключен) Specular
							mtl[name][3] = line.Substring(3);
						} else if (values[0] == "Ke") { // emissive Синий цвет излучения Эмиссии
							mtl[name][4] = line.Substring(3);
						} else if (values[0] == "Ni") { // Оптическая плотность поверхности (0.001 до 10)
							mtl[name][5] = values[1];
						} else if (values[0] == "Ns") { // specular highlight Коэффициент зеркального отражения (глянец) (от 0 до 1000) шероховатость
							mtl[name][6] = values[1];
						} else if (values[0] == "Pr") { // Параметр шероховатости
							mtl[name][7] = values[1];
						} else if (values[0] == "Pm") { // Параметр металла
							mtl[name][8] = values[1];
						} else if (values[0] == "Ps") { // Параметр блеска
							mtl[name][9] = values[1];
						} else if (values[0] == "Pc") { // Параметр толщина прозрачного покрытия 
							mtl[name][10] = values[1];
						} else if (values[0] == "Pcr") { // Параметр шероховатость прозрачного покрытия
							mtl[name][11] = values[1];
						} else if (values[0] == "aniso") { // анизотропия
							mtl[name][12] = values[1];
						} else if (values[0] == "anisor") { // норма вращения анизотропии
							mtl[name][13] = values[1];
						} else if (values[0] == "d" || values[0] == "Tr") { // Непрозрачность и прозрачность 0.0 - 1.0
							mtl[name][14] = values[1];
						} else if (values[0] == "illum") { // вид освещения (от 0 до 10)
							/* 0 Цвет включен, а окружающее освещение выключено 
							1 Цвет включен, а окружающее освещение выключено 
							2 Подсветка включена
							3 Отражение включено, а трассировка лучей включена 
							4 Прозрачность: стекло включено Отражение: трассировка лучей включена 
							5 Отражение: Френель включено и трассировка лучей включена 
							6 Прозрачность: преломление включено Отражение: Френель выключено и трассировка лучей включена
							7 Прозрачность: преломление включено Отражение: Френель включено и трассировка лучей включена
							8 Отражение включено и трассировка лучей выключена 
							9 Прозрачность: стекло включено Отражение: трассировка лучей выключена 
							10 Отбрасывает тени на невидимые поверхности  */
							mtl[name][15] = values[1];
						} else if (values[0] == "map_Ka") { // ambient texture Цвет окружающего освещения (жёлтый)
							mtl[name][16] = line.Substring(7);
						} else if (values[0] == "map_Kd") { // diffuse texture оснавная текстура диффузная
							mtl[name][17] = line.Substring(7);
						} else if (values[0] == "map_Ks") { // specular texture Зеркальная текстура
							mtl[name][18] = line.Substring(7);
						} else if (values[0] == "map_Ke") { // emissive texture Эмиссионная текстура
							mtl[name][19] = line.Substring(7);
						} else if (values[0] == "map_Ns") { // specular highlight Текстура шероховатости (должна быть в оттенках серого)
							mtl[name][20] = line.Substring(7);
						} else if (values[0] == "map_Pr") { // Текстура шероховатости (должна быть в оттенках серого)
							mtl[name][21] = line.Substring(7);
						} else if (values[0] == "map_Pm") { // Текстура металла (должна быть в оттенках серого)
							mtl[name][22] = line.Substring(7);
						} else if (values[0] == "map_d") { // Текстура шероховатости (должна быть в оттенках серого)
							mtl[name][23] = line.Substring(6);
						} else if (values[0] == "map_Kn") { // normal texture
							mtl[name][24] = line.Substring(7);
						} else if (values[0] == "map_bump" || values[0] == "map_Bump") { // normal texture
							mtl[name][24] = line.Substring(9);
						} else if (values[0] == "bump" || values[0] == "norm") { // normal texture
							mtl[name][24] = line.Substring(5);
						} 
					}

					// выборка данных
					foreach (string n in Groups) {
						if (mtl.ContainsKey(n)) {
							// перегоняем вершины и цвет/текстуры
							ColorData[g] = new float[4][];

							if (Textures[g] == null) {
								Textures[g] = new string[9];
							} else if (Textures[g].Length < 9) {
								System.Array.Resize(ref Textures[g], 9);
							}

							// цвета
							for (l = 0; l < 4; l++) {
								name = mtl[n][l+1];

								if (name != "") {
									values = name.Split(' ');

									float.TryParse(values[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out x);
									float.TryParse(values[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out y);
									float.TryParse(values[2], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out z);
									float.TryParse(mtl[n][14], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture.NumberFormat, out w);

									// преобразование RGB to sRGB
									color = BusEngine.Color.ToSrgb(new BusEngine.Color(x, y, z, w));

									ColorData[g][l] = new float[] {color.R, color.G, color.B, color.A};
								}
							}

							// значения
							for (l = 0; l < 10; l++) {
								name = mtl[n][l+5];
							}

							// текстуры
							ll = 0;

							for (l = 0; l < 9; l++) {
								name = mtl[n][l+16];

								if (name != "") {
									//BusEngine.Log.Info("Failed to open the material: " + path + name);
									if (System.IO.File.Exists(path + name)) {
										ll++;
										Textures[g][l] = path + name;
									} else if (System.IO.File.Exists(name)) {
										ll++;
										Textures[g][l] = name;
									} else if (System.IO.File.Exists(dir + name)) {
										ll++;
										Textures[g][l] = dir + name;
									}
								}
							}

							if (ll == 0) {
								System.Array.Clear(Textures[g], 0, Textures[g].Length);
								Textures[g] = null;
							}
						}

						g++;
					}

					mtl.Clear();
					Loaded = true;
				}
			} else {
				System.ConsoleColor c = System.Console.ForegroundColor;
				System.Console.ForegroundColor = System.ConsoleColor.Red;
				BusEngine.Log.Info("Failed to open the material: " + url);
				System.Console.ForegroundColor = c;
			}

			#if BUSENGINE_BENCHMARK
			}
			#endif
		}

		// загружаем во формат движка .bem
		private async System.Threading.Tasks.Task SetCache(string url) {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Model SetCache "+ url + " " + System.Threading.Thread.CurrentThread.ManagedThreadId)) {
			#endif

			if (!System.IO.File.Exists(url)) {
				// https://ru.stackoverflow.com/questions/1325622/c-%D1%83%D1%81%D0%BA%D0%BE%D1%80%D0%B8%D1%82%D1%8C-%D0%B7%D0%B0%D0%BF%D0%B8%D1%81%D1%8C-%D0%B2-%D1%84%D0%B0%D0%B9%D0%BB
				using (System.IO.BufferedStream sw = new System.IO.BufferedStream(new System.IO.StreamWriter(url).BaseStream, 2 * 1024 * 1024)) {
					byte[] buffer,
					engine = System.Text.Encoding.UTF8.GetBytes("BusEngine v0.4.0.0"),
					name = System.Text.Encoding.UTF8.GetBytes(Name),
					material = System.Text.Encoding.UTF8.GetBytes(Material),
					groups = System.Text.Encoding.UTF8.GetBytes(string.Join("#", Groups, 0, Groups.Length)),
					mode = System.Text.Encoding.UTF8.GetBytes(string.Join("#", Mode, 0, Mode.Length));

					int i = 0,
					i_engine = engine.Length,
					i_name = name.Length,
					i_material = material.Length,
					i_groups = groups.Length,
					i_mode = mode.Length;
					float g = 0;

					foreach (BusEngine.Vector3[] group in VertexData) {
						g++;
						i += group.Length;
					}

					float[] FrustumNew = new float[6] {fx, fy, fz, fmx, fmy, fmz};
					float[] VertexDataNew = new float[i * 3 + (int)g + 1];
					float[] TexDataNew = new float[i * 2 + (int)g + 1];
					float[] NormDataNew = new float[i * 3 + (int)g + 1];

					i = 0;
					VertexDataNew[i++] = g;
					foreach (BusEngine.Vector3[] group in VertexData) {
						VertexDataNew[i++] = group.Length;
						foreach (BusEngine.Vector3 result in group) {
							VertexDataNew[i++] = result.X;
							VertexDataNew[i++] = result.Y;
							VertexDataNew[i++] = result.Z;
						}
					}

					i = 0;
					TexDataNew[i++] = g;
					foreach (BusEngine.Vector2[] group in TexData) {
						TexDataNew[i++] = group.Length;
						foreach (BusEngine.Vector2 result in group) {
							TexDataNew[i++] = result.X;
							TexDataNew[i++] = result.Y;
						}
					}

					i = 0;
					NormDataNew[i++] = g;
					foreach (BusEngine.Vector3[] group in NormData) {
						NormDataNew[i++] = group.Length;
						foreach (BusEngine.Vector3 result in group) {
							NormDataNew[i++] = result.X;
							NormDataNew[i++] = result.Y;
							NormDataNew[i++] = result.Z;
						}
					}

					buffer = new byte[
						4 + i_engine +
						4 + i_name +
						4 + i_material +
						4 + i_groups +
						4 + i_mode +
						4 + FrustumNew.Length * 4 +
						4 + VertexDataNew.Length * 4 +
						4 + TexDataNew.Length * 4 +
						4 + NormDataNew.Length * 4
					];

					i = 0;

					System.Buffer.BlockCopy(System.BitConverter.GetBytes(i_engine), 0, buffer, i, 4);
					i += 4;
					System.Buffer.BlockCopy(engine, 0, buffer, i, i_engine);
					i += i_engine;

					System.Buffer.BlockCopy(System.BitConverter.GetBytes(i_name), 0, buffer, i, 4);
					i += 4;
					System.Buffer.BlockCopy(name, 0, buffer, i, i_name);
					i += i_name;

					System.Buffer.BlockCopy(System.BitConverter.GetBytes(i_material), 0, buffer, i, 4);
					i += 4;
					System.Buffer.BlockCopy(material, 0, buffer, i, i_material);
					i += i_material;

					System.Buffer.BlockCopy(System.BitConverter.GetBytes(i_groups), 0, buffer, i, 4);
					i += 4;
					System.Buffer.BlockCopy(groups, 0, buffer, i, i_groups);
					i += i_groups;

					System.Buffer.BlockCopy(System.BitConverter.GetBytes(i_mode), 0, buffer, i, 4);
					i += 4;
					System.Buffer.BlockCopy(mode, 0, buffer, i, i_mode);
					i += i_mode;

					System.Buffer.BlockCopy(System.BitConverter.GetBytes(FrustumNew.Length), 0, buffer, i, 4);
					i += 4;
					System.Buffer.BlockCopy(FrustumNew, 0, buffer, i, FrustumNew.Length * 4);
					i += FrustumNew.Length * 4;

					System.Buffer.BlockCopy(System.BitConverter.GetBytes(VertexDataNew.Length), 0, buffer, i, 4);
					i += 4;
					System.Buffer.BlockCopy(VertexDataNew, 0, buffer, i, VertexDataNew.Length * 4);
					i += VertexDataNew.Length * 4;

					System.Buffer.BlockCopy(System.BitConverter.GetBytes(TexDataNew.Length), 0, buffer, i, 4);
					i += 4;
					System.Buffer.BlockCopy(TexDataNew, 0, buffer, i, TexDataNew.Length * 4);
					i += TexDataNew.Length * 4;

					System.Buffer.BlockCopy(System.BitConverter.GetBytes(NormDataNew.Length), 0, buffer, i, 4);
					i += 4;
					System.Buffer.BlockCopy(NormDataNew, 0, buffer, i, NormDataNew.Length * 4);

					await sw.WriteAsync(buffer, 0, buffer.Length);
					sw.Close();

					CacheStatus = 2;

					System.Array.Clear(buffer, 0, buffer.Length);
					System.Array.Clear(FrustumNew, 0, FrustumNew.Length);
					System.Array.Clear(VertexDataNew, 0, VertexDataNew.Length);
					System.Array.Clear(TexDataNew, 0, TexDataNew.Length);
					System.Array.Clear(NormDataNew, 0, NormDataNew.Length);
				}
			}

			#if BUSENGINE_BENCHMARK
			}
			#endif
		}

		// загружаем из формата движка .bem
		private async System.Threading.Tasks.Task GetCache(string url) {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Model GetCache "+ url + " " + System.Threading.Thread.CurrentThread.ManagedThreadId)) {
			#endif

			if (System.IO.File.Exists(url)) {
				using (System.IO.BufferedStream sr = new System.IO.BufferedStream(new System.IO.StreamReader(url).BaseStream, 2 * 1024 * 1024)) {
					byte[] buffer = new byte[sr.Length];

					await sr.ReadAsync(buffer, 0, buffer.Length);
					sr.Close();

					int p = 0, i, ii, l, ll;

					// Engine version
					// получаем число символов текста из первых 4х байт
					l = System.BitConverter.ToInt32(buffer, p);
					p += 4;
					// получаем байты самого текста и конвертируем в текст с поддержкой кириллицы;
					//string Engine = System.Text.Encoding.UTF8.GetString(buffer, p, l);
					p += l;

					// Name
					l = System.BitConverter.ToInt32(buffer, p);
					p += 4;
					Name = System.Text.Encoding.UTF8.GetString(buffer, p, l);
					p += l;

					// Material
					l = System.BitConverter.ToInt32(buffer, p);
					p += 4;
					if (Material == "") {
						Material = System.Text.Encoding.UTF8.GetString(buffer, p, l);
					}
					p += l;

					// Groups
					l = System.BitConverter.ToInt32(buffer, p);
					p += 4;
					Groups = System.Text.Encoding.UTF8.GetString(buffer, p, l).Split('#');
					p += l;

					ll = Groups.Length;
					System.Array.Resize(ref this.PrimitiveType, ll);
					System.Array.Resize(ref this.BeginMode, ll);
					ColorData = new float[ll][][];
					Textures = new string[ll][];
					VAOs = new int[ll];
					TIs = new int[ll][];

					// Mode
					l = System.BitConverter.ToInt32(buffer, p);
					p += 4;
					Mode = System.Text.Encoding.UTF8.GetString(buffer, p, l).Split('#');

					i = 0;

					foreach (string m in Mode) {
						if (m == "polygon") {
							PrimitiveType[i] = OpenTK.Graphics.OpenGL.PrimitiveType.Polygon;
							BeginMode[i] = OpenTK.Graphics.OpenGL.BeginMode.Polygon;
						} else if (m == "quads") {
							PrimitiveType[i] = OpenTK.Graphics.OpenGL.PrimitiveType.Quads;
							BeginMode[i] = OpenTK.Graphics.OpenGL.BeginMode.Quads;
						} else if (m == "triangles") {
							PrimitiveType[i] = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;
							BeginMode[i] = OpenTK.Graphics.OpenGL.BeginMode.Triangles;
						} else if (m == "lines") {
							PrimitiveType[i] = OpenTK.Graphics.OpenGL.PrimitiveType.Lines;
							BeginMode[i] = OpenTK.Graphics.OpenGL.BeginMode.Lines;
						} else if (m == "points") {
							PrimitiveType[i] = OpenTK.Graphics.OpenGL.PrimitiveType.Points;
							BeginMode[i] = OpenTK.Graphics.OpenGL.BeginMode.Points;
						} else {
							PrimitiveType[i] = OpenTK.Graphics.OpenGL.PrimitiveType.Triangles;
							BeginMode[i] = OpenTK.Graphics.OpenGL.BeginMode.Triangles;
						}
						i++;
					}

					p += l;

					// Pozitions
					if (Pozitions != null && Pozitions.Length > 0) {
						D = Pozitions.Length / 6;
					} else {
						D = 1;
					}

					// Frustum
					l = System.BitConverter.ToInt32(buffer, p);
					p += 4;
					fx = System.BitConverter.ToSingle(buffer, p);
					p += 4;
					fy = System.BitConverter.ToSingle(buffer, p);
					p += 4;
					fz = System.BitConverter.ToSingle(buffer, p);
					p += 4;
					fmx = System.BitConverter.ToSingle(buffer, p);
					p += 4;
					fmy = System.BitConverter.ToSingle(buffer, p);
					p += 4;
					fmz = System.BitConverter.ToSingle(buffer, p);
					p += 4;

					if (D > 1) {
						float x = float.MinValue, y = float.MinValue, z = float.MinValue;

						for (i = 0; i < Pozitions.Length; i += 6) {
							if (x < Pozitions[i]) {
								x = Pozitions[i];
							}
							if (y < Pozitions[i+1]) {
								y = Pozitions[i+1];
							}
							if (z < Pozitions[i+2]) {
								z = Pozitions[i+2];
							}
						}

						fmx = x + (fx + fmx) / 2.0f;
						fmy = y + (fy + fmy) / 2.0f;
						fmz = z + (fz + fmz) / 2.0f;
					}

					Selection = new BusEngine.Vector3[8] {
						new BusEngine.Vector3(0.0f, 0.0f, 0.0f),
						new BusEngine.Vector3(0.0f, fy, 0.0f),
						new BusEngine.Vector3(fx, 0.0f, 0.0f),
						new BusEngine.Vector3(fx, fy, 0.0f),
						new BusEngine.Vector3(0.0f, 0.0f, fz),
						new BusEngine.Vector3(0.0f, fy, fz),
						new BusEngine.Vector3(fx, fy, fz),
						new BusEngine.Vector3(fx, 0.0f, fz)
					};

					frustum = new float[8] {
						0.0f,
						fy/3,
						fx/3,
						(fx + fy)/3,
						fz/3,
						(fy + fz)/3,
						(fx + fy + fz)/3,
						(fx + fz)/3
					};

					Frustum = frustum;

					//AABB = new BusEngine.Game.MyPlugin.AABB(new OpenTK.Vector3(fx + X, fy + Y, fz + Z), new OpenTK.Vector3(fmx + X, fmy + Y, fmz + Z));

					Count += D;

					// VertexData
					l = System.BitConverter.ToInt32(buffer, p);
					p += 4;
					l = (int)System.BitConverter.ToSingle(buffer, p);
					p += 4;

					VertexData = new BusEngine.Vector3[l][];

					for (i = 0; i < l; i++) {
						ll = (int)System.BitConverter.ToSingle(buffer, p);

						VertexData[i] = new BusEngine.Vector3[ll];

						if (Mode[i] == "quads") {
							QuadsCount += ll/4 * D;
						} else if (Mode[i] == "triangles") {
							TrianglesCount += ll/3 * D;
						} else {
							PolygonsCount += ll/5 * D;
						}

						for (ii = 0; ii < ll; ii++) {
							VertexData[i][ii] = new BusEngine.Vector3(System.BitConverter.ToSingle(buffer, p += 4), System.BitConverter.ToSingle(buffer, p += 4), System.BitConverter.ToSingle(buffer, p += 4));
						}

						p += 4;
					}

					// TexData
					l = System.BitConverter.ToInt32(buffer, p);
					p += 4;
					l = (int)System.BitConverter.ToSingle(buffer, p);
					p += 4;

					TexData = new BusEngine.Vector2[l][];

					for (i = 0; i < l; i++) {
						ll = (int)System.BitConverter.ToSingle(buffer, p);
						
						TexData[i] = new BusEngine.Vector2[ll];

						for (ii = 0; ii < ll; ii++) {
							TexData[i][ii] = new BusEngine.Vector2(System.BitConverter.ToSingle(buffer, p += 4), System.BitConverter.ToSingle(buffer, p += 4));
						}

						p += 4;
					}

					// NormData
					l = System.BitConverter.ToInt32(buffer, p);
					p += 4;
					l = (int)System.BitConverter.ToSingle(buffer, p);
					p += 4;

					NormData = new BusEngine.Vector3[l][];

					for (i = 0; i < l; i++) {
						ll = (int)System.BitConverter.ToSingle(buffer, p);

						NormData[i] = new BusEngine.Vector3[ll];

						for (ii = 0; ii < ll; ii++) {
							NormData[i][ii] = new BusEngine.Vector3(System.BitConverter.ToSingle(buffer, p += 4), System.BitConverter.ToSingle(buffer, p += 4), System.BitConverter.ToSingle(buffer, p += 4));
						}

						p += 4;
					}

					System.Array.Clear(buffer, 0, buffer.Length);

					string path = System.IO.Path.GetDirectoryName(url) + '/';

					// Material
					if (!System.IO.File.Exists(Material)) {
						Material = path + Material;
					}

					await importMaterial(Material);
				}

			}

			#if BUSENGINE_BENCHMARK
			}
			#endif
		}

		// https://www.cyberforum.ru/opengl/thread3134052.html
		// http://dreamstatecoding.blogspot.com/2017/02/opengl-4-with-opentk-in-c-part-11-mipmap.html
		// https://github.com/7Bpencil/ScreenshotOpenTK/blob/master/Screenshot.cs
		private static int texturescount = 0;
		private static string[] textures = new string[20];
		private static string[] extensions = new string[] {".dds", ".tga", ".png", ".jpg", ".jpeg", ".bmp", ".gif", ".exif", ".tif", ".tiff"};

		private int Texture(string url, int unit) {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Model Texture "+ Url + " " + url + "" + System.Threading.Thread.CurrentThread.ManagedThreadId)) {
			#endif

			int id = 0;

			if (url != null && System.IO.File.Exists(url)) {
				id = System.Array.IndexOf(textures, url);

				if (id != -1) {
					return id;
				}

				//OpenTK.Graphics.OpenGL.GL.GenTextures(unit+1, out id);
				id = OpenTK.Graphics.OpenGL.GL.GenTexture();

				if (id >= texturescount) {
					System.Array.Resize(ref textures, texturescount + 20);
				}

				textures[id] = url;
				texturescount++;

				string extension = System.IO.Path.GetExtension(url).ToLower();

				/* if (extension == ".tga") {
					TGASharpLib.TGA tga = new TGASharpLib.TGA(url);
					System.Drawing.Bitmap bitmap = new TGASharpLib.TGA(url).ToBitmap(true);
				} */
				//new  System.Drawing.Image();

				if (System.Array.IndexOf(extensions, extension) == -1) {
					System.ConsoleColor c = System.Console.ForegroundColor;
					System.Console.ForegroundColor = System.ConsoleColor.Red;
					BusEngine.Log.Info("Failed to open the texture format: " + url);
					System.Console.ForegroundColor = c;

					return id;
				}

				//unchecked {
					if (extension == ".dds") {
						using (System.IO.StreamReader fsSource = new System.IO.StreamReader(url)) {
							int i, size, mipMapCountReal, w, h, offset, width = -1, height = -1;
							bool SRGB = false;
							byte[] buffer, header = new byte[128];
							fsSource.BaseStream.Read(header, 0, 128);
							//await fsSource.ReadAsync(header, 0, 128);

							// first 4 bytes should be 'DDS '
							// 84-87 bytes should be 'DXTn'
							// only DXT1, DXT3, DXT5 formats are supported
							if (!(header[0] == 'D' && header[1] == 'D' && header[2] == 'S' && header[3] == ' ')) {
								System.ConsoleColor c = System.Console.ForegroundColor;
								System.Console.ForegroundColor = System.ConsoleColor.Red;
								BusEngine.Log.Info("Failed to open the texture format: " + url);
								System.Console.ForegroundColor = c;

								return id;
							}

							height = header[12] | (header[13] << 8) | (header[14] << 16) | (header[15] << 24);
							width = header[16] | (header[17] << 8) | (header[18] << 16) | (header[19] << 24);
							int mipMapCount = header[28] | (header[29] << 8) | (header[30] << 16) | (header[31] << 24);

							OpenTK.Graphics.OpenGL.InternalFormat format = OpenTK.Graphics.OpenGL.InternalFormat.CompressedRgbaS3tcDxt5Ext;
							int blockSize = 24;
							switch ((char)header[87]) {
								case '1': // DXT1
									format = SRGB ? OpenTK.Graphics.OpenGL.InternalFormat.CompressedSrgbS3tcDxt1Ext : OpenTK.Graphics.OpenGL.InternalFormat.CompressedRgbS3tcDxt1Ext;
									blockSize = 8;
									break;
								case '3': // DXT3
									format = SRGB ? OpenTK.Graphics.OpenGL.InternalFormat.CompressedSrgbAlphaS3tcDxt3Ext : OpenTK.Graphics.OpenGL.InternalFormat.CompressedRgbaS3tcDxt3Ext;
									blockSize = 16;
									break;
								case '5': // DXT5
									format = SRGB ? OpenTK.Graphics.OpenGL.InternalFormat.CompressedSrgbAlphaS3tcDxt5Ext : OpenTK.Graphics.OpenGL.InternalFormat.CompressedRgbaS3tcDxt5Ext;
									blockSize = 16;
									break;
								default: break;
							}

							// int32 should be enough
							size = (int)fsSource.BaseStream.Length - 128;
							buffer = new byte[size];

							fsSource.BaseStream.Read(buffer, 0, size);
							fsSource.Close();


							//OpenTK.Graphics.OpenGL.GL.BindSampler(id, unit);
							OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture0 + unit);
							OpenTK.Graphics.OpenGL.GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, id);
							//OpenTK.Graphics.OpenGL.GL.HP.ImageTransformParameter(OpenTK.Graphics.OpenGL.HpImageTransform.ImageRotateOriginYHp, OpenTK.Graphics.OpenGL.HpImageTransform.ImageRotateOriginXHp, id);

							mipMapCount = 1;
							mipMapCountReal = mipMapCount;

							for (i = 0, w = width, h = height, offset = 0; i < mipMapCount; i++, w /= 2, h /= 2) {
								// discard any odd mipmaps with 0x1, 0x2 resolutions
								if (w == 0 || h == 0) {
									mipMapCountReal--;
									continue;
								}

								size = ((w + 3) / 4) * ((h + 3) / 4) * blockSize;

								OpenTK.Graphics.OpenGL.GL.CompressedTexImage2D(
									OpenTK.Graphics.OpenGL.TextureTarget.Texture2D,
									i,
									format,
									w,
									h,
									0,
									size,
									buffer/*  + size */
								);
								offset += size;
							}

							System.Array.Clear(buffer, 0, buffer.Length);

							OpenTK.Graphics.OpenGL.TextureMinFilter textureMinFilter = mipMapCountReal != 1 ? OpenTK.Graphics.OpenGL.TextureMinFilter.LinearMipmapLinear : OpenTK.Graphics.OpenGL.TextureMinFilter.Linear;
							if (mipMapCountReal != 1) {
								OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMaxLevel, mipMapCountReal - 1);
							}
							OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMinFilter, (int)textureMinFilter);
							OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMagFilter, (int)OpenTK.Graphics.OpenGL.TextureMagFilter.Linear);
							OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapS, 1);

							

							/* float maxAnisotropy = 1.0f;
							BusEngine.Log.Info("xxxxxx1 {0}", maxAnisotropy);
							OpenTK.Graphics.OpenGL.GL.GetFloat((OpenTK.Graphics.OpenGL.GetPName)OpenTK.Graphics.OpenGL.GetPName.MaxTessControlInputComponents, out maxAnisotropy);
							OpenTK.Graphics.OpenGL.GL.GetFloat((OpenTK.Graphics.OpenGL.GetPName)OpenTK.Graphics.OpenGL.GetPName.MaxTessEvaluationInputComponents, out maxAnisotropy);
							OpenTK.Graphics.OpenGL.GL.GetFloat((OpenTK.Graphics.OpenGL.GetPName)OpenTK.Graphics.OpenGL.GetPName.MaxPatchVertices, out maxAnisotropy); */





							//OpenTK.Graphics.OpenGL.GL.HP.ImageTransformParameter(OpenTK.Graphics.OpenGL.HpImageTransform.ImageRotateOriginYHp, OpenTK.Graphics.OpenGL.HpImageTransform.ImageRotateOriginYHp, 1);
							//OpenTK.Graphics.OpenGL.GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, 0);
							//OpenTK.Graphics.OpenGL.GL.GenerateTextureMipmap(id);
							//OpenTK.Graphics.OpenGL.GL.GenerateMipmap(OpenTK.Graphics.OpenGL.GenerateMipmapTarget.Texture2D);

							/* OpenTK.Graphics.OpenGL.GL.TextureParameter(id, OpenTK.Graphics.OpenGL.TextureParameterName.TextureBaseLevel, 0);
							OpenTK.Graphics.OpenGL.GL.TextureParameter(id, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMaxLevel, 16);
							float[] borderColor = { 1.0f, 1.0f, 0.0f, 1.0f };
							OpenTK.Graphics.OpenGL.GL.TextureParameter(id, OpenTK.Graphics.OpenGL.TextureParameterName.TextureBorderColor, borderColor);
							OpenTK.Graphics.OpenGL.GL.TextureParameter(id, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapS, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.Repeat);
							OpenTK.Graphics.OpenGL.GL.TextureParameter(id, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapT, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.Repeat);
							OpenTK.Graphics.OpenGL.GL.TextureParameter(id, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMinFilter, (int)OpenTK.Graphics.OpenGL.TextureMinFilter.LinearMipmapLinear);
							OpenTK.Graphics.OpenGL.GL.TextureParameter(id, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMagFilter, (int)OpenTK.Graphics.OpenGL.TextureMinFilter.LinearMipmapLinear);
							OpenTK.Graphics.OpenGL.GL.TextureParameter(id, OpenTK.Graphics.OpenGL.TextureParameterName.GenerateMipmap, 1);


							float maxAnisotropy;
							OpenTK.Graphics.OpenGL.GL.GetFloat((OpenTK.Graphics.OpenGL.GetPName)OpenTK.Graphics.OpenGL.ExtTextureFilterAnisotropic.MaxTextureMaxAnisotropyExt, out maxAnisotropy);
							maxAnisotropy = 1.0f;
							OpenTK.Graphics.OpenGL.GL.TextureParameter(id, (OpenTK.Graphics.OpenGL.TextureParameterName)OpenTK.Graphics.OpenGL.ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt, maxAnisotropy);
							float MaxImageUnits;
							OpenTK.Graphics.OpenGL.GL.GetFloat(OpenTK.Graphics.OpenGL.GetPName.MaxCombinedTextureImageUnits, out MaxImageUnits);
 */

							TexturesCount++;
						}
					} else {
						using (System.Drawing.Bitmap bitmap = (extension == ".tga" ? new TGASharpLib.TGA(url).ToBitmap(true) : new System.Drawing.Bitmap(url, false))) {
							byte[] dataByte, outData;
							if (extension != ".tga") {
								//bitmap.RotateFlip(System.Drawing.RotateFlipType.Rotate180FlipX);
							} else {
								//bitmap.RotateFlip(System.Drawing.RotateFlipType.Rotate90FlipX);
							}
							bitmap.SetResolution(16.0F, 16.0F);
							System.Drawing.Rectangle ract = new System.Drawing.Rectangle(0, 0, bitmap.Width, bitmap.Height);
							System.Drawing.Imaging.BitmapData data = bitmap.LockBits(ract, System.Drawing.Imaging.ImageLockMode.ReadOnly, bitmap.PixelFormat);
							System.IntPtr ptr = data.Scan0;
							int bytes  = System.Math.Abs(data.Stride) * bitmap.Height;

							dataByte = new byte[bytes];
							//int index = 100 * 4 + 100 * System.Math.Abs(data.Stride);

							//BusEngine.Log.Info("tttt {0} {1}", index, System.Math.Abs(data.Stride));

							//outData = 1 == 0 ? (byte[])dataByte.Clone() : dataByte;
							outData = dataByte;

							System.Runtime.InteropServices.Marshal.Copy(ptr, outData, 0, bytes);

							
							//OpenTK.Graphics.OpenGL.GL.BindSampler(id, unit);
							OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture0 + unit);
							OpenTK.Graphics.OpenGL.GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, id);

							/* OpenTK.Graphics.OpenGL.GL.CompressedTexImage2D(
								//id,
								OpenTK.Graphics.OpenGL.TextureTarget.Texture2D,
								0,
								//OpenTK.Graphics.OpenGL.ExtDirectStateAccess.TransposeProgramMatrixExt,
								OpenTK.Graphics.OpenGL.InternalFormat.CompressedSrgbAlphaBptcUnorm,
								bitmap.Width,
								bitmap.Height,
								0,
								bytes,
								outData
							); */
							//OpenTK.Graphics.OpenGL.GL.CreateTextures(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, 1, out id);

							OpenTK.Graphics.OpenGL.PixelFormat format;

							if (bitmap.PixelFormat == System.Drawing.Imaging.PixelFormat.Format32bppArgb) {
								format = OpenTK.Graphics.OpenGL.PixelFormat.Bgra;
							} else {
								//BusEngine.Log.Info(bitmap.PixelFormat);
								format = OpenTK.Graphics.OpenGL.PixelFormat.Bgr;
							}

							//OpenTK.Graphics.OpenGL.GL.TextureImage2D(
							OpenTK.Graphics.OpenGL.GL.TexImage2D(
								//id,
								OpenTK.Graphics.OpenGL.TextureTarget.Texture2D,
								0,
								//OpenTK.Graphics.OpenGL.ExtDirectStateAccess.TransposeProgramMatrixExt,
								OpenTK.Graphics.OpenGL.PixelInternalFormat.Rgb,
								bitmap.Width,
								bitmap.Height,
								0,
								format,
								OpenTK.Graphics.OpenGL.PixelType.UnsignedByte,
								outData
							);

							//OpenTK.Graphics.OpenGL.ArbTextureCompressionBptc.CompressedSrgbAlphaBptcUnormArb;

							/* //OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.DetailTextureLevelSgis, 1);
							//OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.DetailTextureModeSgis, 1);
							OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureBaseLevel, 1);
							OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMaxLevel, 8);
							float[] borderColor = { 1.0f, 1.0f, 0.0f, 1.0f };
							OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureBorderColor, borderColor);
							OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapS, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.Repeat);
							OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapT, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.Repeat);
							OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMinFilter, (int)OpenTK.Graphics.OpenGL.TextureMinFilter.LinearMipmapLinear);
							OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMagFilter, (int)OpenTK.Graphics.OpenGL.TextureMinFilter.LinearMipmapLinear);
							OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.GenerateMipmap, 8);

							//OpenTK.Graphics.OpenGL.GL.GenerateTextureMipmap(id);
							OpenTK.Graphics.OpenGL.GL.GenerateMipmap(OpenTK.Graphics.OpenGL.GenerateMipmapTarget.Texture2D);

							//OpenTK.Graphics.OpenGL.GL.GetSamplerParameter(id, OpenTK.Graphics.OpenGL.SamplerParameter.TextureMagFilter, new float[1] {(int)OpenTK.Graphics.OpenGL.TextureMagFilter.Linear});

							float maxAnisotropy;
							OpenTK.Graphics.OpenGL.GL.GetFloat((OpenTK.Graphics.OpenGL.GetPName)OpenTK.Graphics.OpenGL.ExtTextureFilterAnisotropic.MaxTextureMaxAnisotropyExt, out maxAnisotropy);
							maxAnisotropy = 1.0f;
							OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, (OpenTK.Graphics.OpenGL.TextureParameterName)OpenTK.Graphics.OpenGL.ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt, maxAnisotropy); */

							//OpenTK.Graphics.OpenGL.GL.TextureParameter(id, OpenTK.Graphics.OpenGL.TextureParameterName.DetailTextureLevelSgis, 1);
							//OpenTK.Graphics.OpenGL.GL.TextureParameter(id, OpenTK.Graphics.OpenGL.TextureParameterName.DetailTextureModeSgis, 1);
							OpenTK.Graphics.OpenGL.GL.TextureParameter(id, OpenTK.Graphics.OpenGL.TextureParameterName.TextureBaseLevel, 0);
							OpenTK.Graphics.OpenGL.GL.TextureParameter(id, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMaxLevel, 16);
							float[] borderColor = { 1.0f, 1.0f, 0.0f, 1.0f };
							OpenTK.Graphics.OpenGL.GL.TextureParameter(id, OpenTK.Graphics.OpenGL.TextureParameterName.TextureBorderColor, borderColor);
							OpenTK.Graphics.OpenGL.GL.TextureParameter(id, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapS, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.Repeat);
							OpenTK.Graphics.OpenGL.GL.TextureParameter(id, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapT, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.Repeat);
							OpenTK.Graphics.OpenGL.GL.TextureParameter(id, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMinFilter, (int)OpenTK.Graphics.OpenGL.TextureMinFilter.LinearMipmapLinear);
							OpenTK.Graphics.OpenGL.GL.TextureParameter(id, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMagFilter, (int)OpenTK.Graphics.OpenGL.TextureMinFilter.LinearMipmapLinear);
							OpenTK.Graphics.OpenGL.GL.TextureParameter(id, OpenTK.Graphics.OpenGL.TextureParameterName.GenerateMipmap, 1);

							//OpenTK.Graphics.OpenGL.GL.GenerateTextureMipmap(id);
							OpenTK.Graphics.OpenGL.GL.GenerateMipmap(OpenTK.Graphics.OpenGL.GenerateMipmapTarget.Texture2D);

							//OpenTK.Graphics.OpenGL.GL.GetSamplerParameter(id, OpenTK.Graphics.OpenGL.SamplerParameter.TextureMagFilter, new float[1] {(int)OpenTK.Graphics.OpenGL.TextureMagFilter.Linear});

							float maxAnisotropy;
							OpenTK.Graphics.OpenGL.GL.GetFloat((OpenTK.Graphics.OpenGL.GetPName)OpenTK.Graphics.OpenGL.ExtTextureFilterAnisotropic.MaxTextureMaxAnisotropyExt, out maxAnisotropy);
							maxAnisotropy = 1.0f;
							OpenTK.Graphics.OpenGL.GL.TextureParameter(id, (OpenTK.Graphics.OpenGL.TextureParameterName)OpenTK.Graphics.OpenGL.ExtTextureFilterAnisotropic.TextureMaxAnisotropyExt, maxAnisotropy);
							float MaxImageUnits;
							OpenTK.Graphics.OpenGL.GL.GetFloat(OpenTK.Graphics.OpenGL.GetPName.MaxCombinedTextureImageUnits, out MaxImageUnits);
							
							
							//BusEngine.Log.Info("MaxImageUnits {0}", MaxImageUnits);
							OpenTK.Graphics.OpenGL.GL.GetFloat((OpenTK.Graphics.OpenGL.GetPName)OpenTK.Graphics.OpenGL.GetPName.MaxTessControlInputComponents, out maxAnisotropy);
							OpenTK.Graphics.OpenGL.GL.GetFloat((OpenTK.Graphics.OpenGL.GetPName)OpenTK.Graphics.OpenGL.GetPName.MaxTessEvaluationInputComponents, out maxAnisotropy);
							OpenTK.Graphics.OpenGL.GL.GetFloat((OpenTK.Graphics.OpenGL.GetPName)OpenTK.Graphics.OpenGL.GetPName.MaxPatchVertices, out maxAnisotropy);

							/* OpenTK.Graphics.OpenGL.GL.GetFloat((OpenTK.Graphics.OpenGL.GetPName)OpenTK.Graphics.OpenGL.GetPName.PatchDefaultInnerLevel, out maxAnisotropy);
							BusEngine.Log.Info("xxxxxx4 {0}", maxAnisotropy);
							
							OpenTK.Graphics.OpenGL.GL.GetFloat((OpenTK.Graphics.OpenGL.GetPName)OpenTK.Graphics.OpenGL.GetPName.PatchDefaultOuterLevel, out maxAnisotropy);
							BusEngine.Log.Info("xxxxxx5 {0}", maxAnisotropy); */

							TexturesCount++;

							//OpenTK.Graphics.OpenGL.GL.DeleteTexture(id);

							System.Runtime.InteropServices.Marshal.Copy(outData, 0, ptr, bytes);
							bitmap.UnlockBits(data);
						}
					}
				//}
			} else {
				System.ConsoleColor c = System.Console.ForegroundColor;
				System.Console.ForegroundColor = System.ConsoleColor.Red;
				BusEngine.Log.Info("Failed to open the texture: " + url);
				System.Console.ForegroundColor = c;
			}

			return id;

			#if BUSENGINE_BENCHMARK
			}
			#endif
		}

		public void Buffered(int g) {
			Buffered(g, false);
		}

		public void Buffered(int g, bool st) {
			#if BUSENGINE_BENCHMARK
			using (new BusEngine.Benchmark("BusEngine.Model Buffered "+ Url + " " + System.Threading.Thread.CurrentThread.ManagedThreadId)) {
			#endif

			if (Loaded && this.Program != 0) {
				// Vertex Arrays Object (VAO)
				OpenTK.Graphics.OpenGL.GL.BindVertexArray(VAOs[g]);
				//OpenTK.Graphics.OpenGL.GL.DeleteVertexArray(VAOs[g]);

				// получаем id атрибут по названию
				int locationVertexData = OpenTK.Graphics.OpenGL.GL.GetAttribLocation(this.Program, "VertexData");
				int locationTexData = OpenTK.Graphics.OpenGL.GL.GetAttribLocation(this.Program, "TexData");
				int locationNormData = OpenTK.Graphics.OpenGL.GL.GetAttribLocation(this.Program, "NormData");
				//int locationD = OpenTK.Graphics.OpenGL.GL.GetAttribLocation(this.Program, "Pozitions[48]");

				// Vertex Buffer Object (VBO)
				/* if (VertexDataStatic != null && st) {
					VBO = OpenTK.Graphics.OpenGL.GL.GenBuffer();
					OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, VBO);
					OpenTK.Graphics.OpenGL.GL.BufferData(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, VertexDataStatic.Length * sizeof(float) * 3, VertexDataStatic, OpenTK.Graphics.OpenGL.BufferUsageHint.DynamicDraw);
					OpenTK.Graphics.OpenGL.GL.VertexAttribPointer(locationVertexData, 3, OpenTK.Graphics.OpenGL.VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);
					//OpenTK.Graphics.OpenGL.GL.DeleteBuffer(VBO);
					OpenTK.Graphics.OpenGL.GL.EnableVertexAttribArray(locationVertexData);
				} else  */if (VertexData.Length > g && !st) {
					VBO = OpenTK.Graphics.OpenGL.GL.GenBuffer();
					OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, VBO);
					OpenTK.Graphics.OpenGL.GL.BufferData(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, VertexData[g].Length * sizeof(float) * 3, VertexData[g], OpenTK.Graphics.OpenGL.BufferUsageHint.DynamicDraw);
					OpenTK.Graphics.OpenGL.GL.VertexAttribPointer(locationVertexData, 3, OpenTK.Graphics.OpenGL.VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);
					//OpenTK.Graphics.OpenGL.GL.DeleteBuffer(VBO);
					OpenTK.Graphics.OpenGL.GL.EnableVertexAttribArray(locationVertexData);
				}

				// Textures Buffer Object (TBO)
				if (TexData.Length > g && !st) {
					TBO = OpenTK.Graphics.OpenGL.GL.GenBuffer();
					OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, TBO);
					OpenTK.Graphics.OpenGL.GL.BufferData(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, TexData[g].Length * sizeof(float) * 2, TexData[g], OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
					OpenTK.Graphics.OpenGL.GL.VertexAttribPointer(locationTexData, 2, OpenTK.Graphics.OpenGL.VertexAttribPointerType.Float, false, sizeof(float) * 2, 0);
					//OpenTK.Graphics.OpenGL.GL.DeleteBuffer(TBO);
					OpenTK.Graphics.OpenGL.GL.EnableVertexAttribArray(locationTexData);
				}

				// Normal Buffer Object (NBO)
				if (NormData.Length > g && !st) {
					NBO = OpenTK.Graphics.OpenGL.GL.GenBuffer();
					OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, NBO);
					OpenTK.Graphics.OpenGL.GL.BufferData(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, NormData[g].Length * sizeof(float) * 3, NormData[g], OpenTK.Graphics.OpenGL.BufferUsageHint.StaticDraw);
					OpenTK.Graphics.OpenGL.GL.VertexAttribPointer(locationNormData, 3, OpenTK.Graphics.OpenGL.VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);
					//OpenTK.Graphics.OpenGL.GL.DeleteBuffer(NBO);
					OpenTK.Graphics.OpenGL.GL.EnableVertexAttribArray(locationNormData);
				}

				/* if (Pozitions != null && Pozitions.Length > 0) {
					//OpenTK.Graphics.OpenGL.BufferTarget.ParameterBuffer
					//OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer
					//OpenTK.Graphics.OpenGL.BufferTarget.ElementArrayBuffer
					//OpenTK.Graphics.OpenGL.BufferTarget.PixelPackBuffer
					//OpenTK.Graphics.OpenGL.BufferTarget.PixelUnpackBuffer
					//OpenTK.Graphics.OpenGL.BufferTarget.UniformBuffer
					//OpenTK.Graphics.OpenGL.BufferTarget.TextureBuffer
					//OpenTK.Graphics.OpenGL.BufferTarget.TransformFeedbackBuffer
					//OpenTK.Graphics.OpenGL.BufferTarget.CopyReadBuffer
					//OpenTK.Graphics.OpenGL.BufferTarget.CopyWriteBuffer
					//OpenTK.Graphics.OpenGL.BufferTarget.DrawIndirectBuffer
					//OpenTK.Graphics.OpenGL.BufferTarget.ShaderStorageBuffer
					//OpenTK.Graphics.OpenGL.BufferTarget.DispatchIndirectBuffer
					//OpenTK.Graphics.OpenGL.BufferTarget.QueryBuffer
					//OpenTK.Graphics.OpenGL.BufferTarget.AtomicCounterBuffer
				} */

				/* OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ArrayBuffer, 0);
				OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.ElementArrayBuffer, 0);
				OpenTK.Graphics.OpenGL.GL.BindBuffer(OpenTK.Graphics.OpenGL.BufferTarget.UniformBuffer, 0); */

				OpenTK.Graphics.OpenGL.GL.BindVertexArray(0);

				/* OpenTK.Graphics.OpenGL.GL.VertexAttribDivisor(locationVertexData, 1);
				OpenTK.Graphics.OpenGL.GL.VertexAttribDivisor(locationTexData, 1);
				OpenTK.Graphics.OpenGL.GL.VertexAttribDivisor(locationNormData, 1); */

				OpenTK.Graphics.OpenGL.GL.DisableVertexAttribArray(locationVertexData);
				OpenTK.Graphics.OpenGL.GL.DisableVertexAttribArray(locationTexData);
				OpenTK.Graphics.OpenGL.GL.DisableVertexAttribArray(locationNormData);
			}

			#if BUSENGINE_BENCHMARK
			}
			#endif
		}
		private static string[] Data = new string[5];
		public static string[] DataModels = new string[10000];
		private static int count = 0;
		private static int last_vaos = 0;
		public static int Programs;
		public static OpenTK.Vector3 LightPos = new OpenTK.Vector3(0.0f, 0.0f, 0.0f);
		private int g, i, l;
		private float poz;
		private int[] gggg;
		private int[] gggg2;
		//private BusEngine.Vector3[] VertexDataStatic;
		//private static BusEngine.Audio audio;
		// https://github.com/URIS-2022/Tim-10---QSIV2.0/blob/ef7100b432eb8047168582c079b6693c74ddb25f/Ryujinx.Graphics.OpenGL/Pipeline.cs#L280
		
		public void SwapBuffers() {
			if (!Loaded) {
				return;
			}

			// загружаем буфер
			if (BufferStatus == 0) {
				BufferStatus = 1;

				string from;

				// создаём шейдеры
				if (Shader != null) {
					if (Pozitions != null && Pozitions.Length > 6) {
						Shader.BUSENGINE_COPY_INT = Pozitions.Length / 2;
					}
					Shader.BUSENGINE_URL = Material;
					Program = Shader.GenProgram();
				} else {
					
				}

				OpenTK.Graphics.OpenGL.GL.UseProgram(this.Program);

				progPozitions = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Program, "Pozitions");
				progDistance = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Program, "Distance");
				progTexStatus = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Program, "TexturesStatus");
				progColorDefault = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Program, "ColorDefault0");
				OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Program, "ColorDefault1");
				OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Program, "ColorDefault2");
				OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Program, "ColorDefault3");
				progAngle = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Program, "Angle");
				progModel = OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Program, "Model");

				if (Pozitions != null) {
					OpenTK.Graphics.OpenGL.GL.Uniform1(progPozitions, Pozitions.Length, Pozitions);
				}
				OpenTK.Graphics.OpenGL.GL.Uniform1(progDistance, (float)D);
				for (int i = 0; i < 9; i++) {
					OpenTK.Graphics.OpenGL.GL.Uniform1(OpenTK.Graphics.OpenGL.GL.GetUniformLocation(this.Program, "Texture" + i), i);
				}

				g = 0;
				gggg = new int[VertexData.Length];
				gggg[0] = 0;
				gggg2 = new int[VertexData.Length];
				foreach (BusEngine.Vector3[] group in VertexData) {
					from = Url + g;
	
					gggg2[g] = group.Length;
					if (g+1 != VertexData.Length) {
						gggg[g+1] = group.Length;
					}

					if (Textures[g] != null) {
						TIs[g] = new int[9];

						for (i = 0; i < Textures[g].Length; i++) {
							TIs[g][i] = Textures[g][i] != null ? Texture(Textures[g][i], i) : 0;
						}

						if (!OpenTK.Graphics.OpenGL.GL.IsTexture(TIs[g][1])) {
							System.Array.Clear(TexData[g], 0, TexData[g].Length);
						}

						if (!OpenTK.Graphics.OpenGL.GL.IsTexture(TIs[g][8])) {
							System.Array.Clear(NormData[g], 0, NormData[g].Length);
						}
					} else {
						System.Array.Clear(TexData[g], 0, TexData[g].Length);
						System.Array.Clear(NormData[g], 0, NormData[g].Length);
					}

					VAOs[g] = System.Array.IndexOf(Data, from);

					if (VAOs[g] == -1) {
						VAOs[g] = OpenTK.Graphics.OpenGL.GL.GenVertexArray();

						if (VAOs[g] >= count) {
							System.Array.Resize(ref Data, count + 5);
						}
						Data[VAOs[g]] = from;
						count++;

						Buffered(g);

						OpenTK.Graphics.OpenGL.GL.BindVertexArray(VAOs[g]);

						if (ColorData[g] != null) {
							for (i = 0, l = ColorData[g].Length; i < l; i++) {
								if (ColorData[g][i] != null) {
									//OpenTK.Graphics.OpenGL.GL.Uniform1(progColorDefault+i, ColorData[g][i].Length, ColorData[g][i]);
									OpenTK.Graphics.OpenGL.GL.Uniform4(progColorDefault+i, ColorData[g][i][0], ColorData[g][i][1], ColorData[g][i][2], ColorData[g][i][3]);
								}
							}
						}
					}

					g++;
				}

				/* from = Url + g;
				VAOs[g] = System.Array.IndexOf(Data, from);

				if (VAOs[g] == -1) {
					VAOs[g] = OpenTK.Graphics.OpenGL.GL.GenVertexArray();
					if (VAOs[g] >= count) {
						System.Array.Resize(ref Data, count + 10);
					}
					Data[VAOs[g]] = from;
					count++;

					Buffered(g, true);
				} */

				//System.Array.Clear(VertexData, 0, VertexData.Length);
				/* System.Array.Clear(TexData, 0, TexData.Length);
				System.Array.Clear(NormData, 0, NormData.Length);
				System.Array.Clear(ColorData, 0, ColorData.Length); */
				P = OpenTK.Matrix4.CreateTranslation(X, Y, Z);

				BufferStatus = 2;
			} else if (BufferStatus == 2) {
				if (Programs != this.Program) {
					//OpenTK.Graphics.OpenGL.GL.LinkProgram(this.Program);
					//OpenTK.Graphics.OpenGL.GL.UniformMatrix4(progProjection, true, ref BusEngine.Game.MyPlugin.projection);
					OpenTK.Graphics.OpenGL.GL.UseProgram(this.Program);
					//BusEngine.Log.Info(Programs);
				}

				//P = OpenTK.Matrix4.Identity;
				//P = OpenTK.Matrix4.CreateTranslation(X, Y, Z);

				// позиция
				/* P.Row3.X += 1;
				P.Row3.Y += 1;
				P.Row3.Z += 1; */
				//A = OpenTK.Matrix4.CreateFromAxisAngle(new OpenTK.Vector3(0.0f, 1.0f, 0.0f), 1);

				if (Light) {
					P = OpenTK.Matrix4.CreateTranslation(BusEngine.Model.LightPos.X, BusEngine.Model.LightPos.Y, BusEngine.Model.LightPos.Z);
				}

				// поворот
				OpenTK.Matrix4 p = S * A * P;
				/* P.Row0.X = A.Row0.X * P.Row0.X;
				P.Row0.Y = A.Row0.Y * P.Row0.Y;
				P.Row0.Z = A.Row0.Z * P.Row0.Z;
				P.Row0.W = A.Row0.W * P.Row0.W;
				P.Row1.X = A.Row1.X * P.Row1.X;
				P.Row1.Y = A.Row1.Y * P.Row1.Y;
				P.Row1.Z = A.Row1.Z * P.Row1.Z;
				P.Row1.W = A.Row1.W * P.Row1.W;
				P.Row2.X = A.Row2.X * P.Row2.X;
				P.Row2.Y = A.Row2.Y * P.Row2.Y;
				P.Row2.Z = A.Row2.Z * P.Row2.Z;
				P.Row2.W = A.Row2.W * P.Row2.W;
				P.Row3 = A.Row3 * P.Row3; */

				// масштаб
				//P = S * P;
				/* P.Row0.X = S.Row0.X;
				P.Row1.Y = S.Row1.Y;
				P.Row2.Z = S.Row2.Z; */

				OpenTK.Graphics.OpenGL.GL.UniformMatrix4(progModel, false, ref p);

				/* OpenTK.Graphics.OpenGL.GL.PatchParameter(OpenTK.Graphics.OpenGL.PatchParameterFloat.PatchDefaultInnerLevel, new float[2] {1.0F, 1.0F});
				OpenTK.Graphics.OpenGL.GL.PatchParameter(OpenTK.Graphics.OpenGL.PatchParameterFloat.PatchDefaultOuterLevel, new float[4] {1.0F, 1.0F, 1.0F, 1.0F}); */
				//float i;
				//OpenTK.Graphics.OpenGL.GL.GetFloat((OpenTK.Graphics.OpenGL.GetPName)OpenTK.Graphics.OpenGL.GetPName.MaxPatchVertices, out i);
				//OpenTK.Graphics.OpenGL.GL.PatchParameter(OpenTK.Graphics.OpenGL.PatchParameterInt.PatchVertices, (int)i);

				/* for (int pi = 0; pi < Pozitions.Length; pi++) {
					Pozitions[pi++] = X;
					Pozitions[pi++] = Y;
					Pozitions[pi++] = Z;
					pi++;
					pi++;
					//Pozitions[pi++] += 0.001f;
					//Pozitions[pi++] += 0.1f;
					//Pozitions[pi] = 1.0f;
				}
				OpenTK.Graphics.OpenGL.GL.Uniform1(progPozitions, Pozitions.Length, Pozitions); */

				/* Selection = new BusEngine.Vector3[8] {
					new BusEngine.Vector3(p.Row3.X, p.Row3.Y, p.Row3.Z),
					new BusEngine.Vector3(p.Row3.X, p.Row3.Y + fy, p.Row3.Z),
					new BusEngine.Vector3(p.Row3.X + fx, p.Row3.Y, p.Row3.Z),
					new BusEngine.Vector3(p.Row3.X + fx, p.Row3.Y + fy, p.Row3.Z),
					new BusEngine.Vector3(p.Row3.X, p.Row3.Y, p.Row3.Z + fz),
					new BusEngine.Vector3(p.Row3.X, p.Row3.Y + fy, p.Row3.Z + fz),
					new BusEngine.Vector3(p.Row3.X + fx, p.Row3.Y + fy, p.Row3.Z + fz),
					new BusEngine.Vector3(p.Row3.X + fx, p.Row3.Y, p.Row3.Z + fz)
				}; */

				/* poz = (p.Row3.X + p.Row3.Y + p.Row3.Z)/3;

				Frustum[0] = frustum[0] + poz;
				Frustum[1] = frustum[1] + poz;
				Frustum[2] = frustum[2] + poz;
				Frustum[3] = frustum[3] + poz;
				Frustum[4] = frustum[4] + poz;
				Frustum[5] = frustum[5] + poz;
				Frustum[6] = frustum[6] + poz;
				Frustum[7] = frustum[7] + poz; */

				//AABB = new BusEngine.Game.MyPlugin.AABB(new OpenTK.Vector3(fx + X, fy + Y, fz + Z), new OpenTK.Vector3(fmx + X, fmy + Y, fmz + Z));
				AABB = BusEngine.Game.MyPlugin.AABB.Transform(AABB, p);

				g = 0;
				foreach (BusEngine.Vector3[] group in VertexData) {
					// подключаем буфер объекта модели
					if (last_vaos != VAOs[g]) {
						OpenTK.Graphics.OpenGL.GL.BindVertexArray(VAOs[g]);
						last_vaos = VAOs[g];
					}

					if (TIs[g] != null) {
						// т.к. можем использовать один шейдер на разные модели, то проверяем статус каждой текстуры
						OpenTK.Graphics.OpenGL.GL.Uniform1(progTexStatus, TIs[g].Length, TIs[g]);

						for (i = 0, l = TIs[g].Length; i < l; i++) {
							if (TIs[g][i] > 0) {
								OpenTK.Graphics.OpenGL.GL.ActiveTexture(OpenTK.Graphics.OpenGL.TextureUnit.Texture0+i);
								OpenTK.Graphics.OpenGL.GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, TIs[g][i]);
							}
						}
					}

					// рисуем модель
					//OpenTK.Graphics.OpenGL.GL.DrawElements(PrimitiveType[g], group.Length, OpenTK.Graphics.OpenGL.DrawElementsType.UnsignedInt, 0);
					//OpenTK.Graphics.OpenGL.GL.Arb.DrawElementsInstanced(BeginMode[g], group.Length, OpenTK.Graphics.OpenGL.DrawElementsType.UnsignedInt, (System.IntPtr)0, VertexIndex.Length);
					OpenTK.Graphics.OpenGL.GL.DrawArrays(PrimitiveType[g], 0, group.Length);
					//OpenTK.Graphics.OpenGL.GL.DrawArraysInstanced(PrimitiveType[g], 0, group.Length, 1);

					if (TIs[g] == null){
						OpenTK.Graphics.OpenGL.GL.BindTexture(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, -1);
					}

					g++;
				}

				//OpenTK.Graphics.OpenGL.GL.MultiDrawArrays(PrimitiveType[0], gggg, gggg2, gggg2.Length);

				if (Programs != this.Program) {
					Programs = this.Program;
				}
			}
		}

		public void Dispose() {

		}

		~Model() {
			//BusEngine.Log.Info("Model ~ " + this.Program + " " + this.Url);
		}
	}
	/** API BusEngine.Model */
}
/** API BusEngine */