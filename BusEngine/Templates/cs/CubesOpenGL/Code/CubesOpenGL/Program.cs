/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2024; BuslikDrev - Усе правы захаваны. */

// benchmark https://cc.davelozinski.com/c-sharp/for-vs-foreach-vs-while
// OpenGL http://pm.samgtu.ru/sites/pm.samgtu.ru/files/materials/comp_graph/RedBook_OpenGL.pdf
// Crysis
// https://habr.com/ru/articles/350782/
// https://habr.com/ru/articles/338998/

/** API BusEngine.Game - пользовательский код */
namespace BusEngine.Game {
	/** API BusEngine.Plugin */
	public class MyPlugin : BusEngine.Plugin {
		// настройки
		private static int FPS = 60, cubes = 0;
		private static float FPSSetting, FPSDelta = 1F, FPSInfo = 60F;

		private static OpenTK.GLControl glControl;
		private static OpenTK.Vector3 position = new OpenTK.Vector3(0.0F, 5.0F, 10.0F);
		private static OpenTK.Vector3 front = new OpenTK.Vector3(0.0F, 0.0F, 1.0F);
		private static OpenTK.Vector3 up = OpenTK.Vector3.UnitY; // new OpenTK.Vector3(0.0f, 1.0f, 0.0f);
		private static OpenTK.Vector3 Orientation = new OpenTK.Vector3((float)System.Math.PI, 0F, 0F);
		private static float speed = 0.1F;
		private static float mousespeed = 0.0025f;
		private static System.Collections.Generic.HashSet<System.Windows.Forms.Keys> IsKeys = new System.Collections.Generic.HashSet<System.Windows.Forms.Keys>();
		private static OpenTK.Vector2 lastPos;
		private static int lastWheel;
		private static bool F1, F2, F3, F4, Pause;

		private static float _angle = 0.0f;
		private static int CubeShader;
		private static int VAO;
		private static int EBO;
		private static int PositionBuffer;
		private static int ColorBuffer;
		private static OpenTK.Matrix4 projection;

		private static int prog;
		private static int progVP;
		private static int progP;
		private static int progA;

		private static readonly OpenTK.Vector3[] VertexData = new OpenTK.Vector3[24] {
			new OpenTK.Vector3(-1.0f, -1.0f, -1.0f),
			new OpenTK.Vector3(-1.0f, 1.0f, -1.0f),
			new OpenTK.Vector3(1.0f, 1.0f, -1.0f),
			new OpenTK.Vector3(1.0f, -1.0f, -1.0f),

			new OpenTK.Vector3(-1.0f, -1.0f, -1.0f),
			new OpenTK.Vector3(1.0f, -1.0f, -1.0f),
			new OpenTK.Vector3(1.0f, -1.0f, 1.0f),
			new OpenTK.Vector3(-1.0f, -1.0f, 1.0f),

			new OpenTK.Vector3(-1.0f, -1.0f, -1.0f),
			new OpenTK.Vector3(-1.0f, -1.0f, 1.0f),
			new OpenTK.Vector3(-1.0f, 1.0f, 1.0f),
			new OpenTK.Vector3(-1.0f, 1.0f, -1.0f),

			new OpenTK.Vector3(-1.0f, -1.0f, 1.0f),
			new OpenTK.Vector3(1.0f, -1.0f, 1.0f),
			new OpenTK.Vector3(1.0f, 1.0f, 1.0f),
			new OpenTK.Vector3(-1.0f, 1.0f, 1.0f),

			new OpenTK.Vector3(-1.0f, 1.0f, -1.0f),
			new OpenTK.Vector3(-1.0f, 1.0f, 1.0f),
			new OpenTK.Vector3(1.0f, 1.0f, 1.0f),
			new OpenTK.Vector3(1.0f, 1.0f, -1.0f),

			new OpenTK.Vector3(1.0f, -1.0f, -1.0f),
			new OpenTK.Vector3(1.0f, 1.0f, -1.0f),
			new OpenTK.Vector3(1.0f, 1.0f, 1.0f),
			new OpenTK.Vector3(1.0f, -1.0f, 1.0f),
		};

		private static int[] IndexData = new int[] {
			0, 1, 2, 2, 3, 0,

			4, 5, 6, 6, 7, 4,

			8,  9, 10, 10, 11, 8,

			12, 13, 14,	14, 15, 12,

			16, 17, 18,	18, 19, 16,

			20, 21, 22,	22, 23, 20,
		};

		private static readonly OpenTK.Graphics.Color4[] ColorData = new OpenTK.Graphics.Color4[] {
			OpenTK.Graphics.Color4.Silver, OpenTK.Graphics.Color4.Silver, OpenTK.Graphics.Color4.Silver, OpenTK.Graphics.Color4.Silver,
			OpenTK.Graphics.Color4.Honeydew, OpenTK.Graphics.Color4.Honeydew, OpenTK.Graphics.Color4.Honeydew, OpenTK.Graphics.Color4.Honeydew,
			OpenTK.Graphics.Color4.Moccasin, OpenTK.Graphics.Color4.Moccasin, OpenTK.Graphics.Color4.Moccasin, OpenTK.Graphics.Color4.Moccasin,
			OpenTK.Graphics.Color4.IndianRed, OpenTK.Graphics.Color4.IndianRed, OpenTK.Graphics.Color4.IndianRed, OpenTK.Graphics.Color4.IndianRed,
			OpenTK.Graphics.Color4.PaleVioletRed, OpenTK.Graphics.Color4.PaleVioletRed, OpenTK.Graphics.Color4.PaleVioletRed, OpenTK.Graphics.Color4.PaleVioletRed,
			OpenTK.Graphics.Color4.ForestGreen, OpenTK.Graphics.Color4.ForestGreen, OpenTK.Graphics.Color4.ForestGreen, OpenTK.Graphics.Color4.ForestGreen,
		};

		/* private static ushort[][] ColorData = new ushort[24][] {
			new ushort[4] {192, 192, 192, 1}, new ushort[4] {192, 192, 192, 1}, new ushort[4] {192, 192, 192, 1}, new ushort[4] {192, 192, 192, 1},
			new ushort[4] {240, 255, 240, 1}, new ushort[4] {240, 255, 240, 1}, new ushort[4] {240, 255, 240, 1}, new ushort[4] {240, 255, 240, 1},
			new ushort[4] {255, 228, 181, 1}, new ushort[4] {255, 228, 181, 1}, new ushort[4] {255, 228, 181, 1}, new ushort[4] {255, 228, 181, 1},
			new ushort[4] {205, 92, 92, 1}, new ushort[4] {205, 92, 92, 1}, new ushort[4] {205, 92, 92, 1}, new ushort[4] {205, 92, 92, 1},
			new ushort[4] {219, 112, 147, 1}, new ushort[4] {219, 112, 147, 1}, new ushort[4] {219, 112, 147, 1}, new ushort[4] {219, 112, 147, 1},
			new ushort[4] {34, 139, 34, 1}, new ushort[4] {34, 139, 34, 1}, new ushort[4] {34, 139, 34, 1}, new ushort[4] {34, 139, 34, 1},
		}; */

		// при запуске BusEngine после создания формы Canvas
		public override void InitializeСanvas() {
			// деламем окно не поверх других окон
			BusEngine.UI.Canvas.WinForm.TopMost = false;
			BusEngine.UI.Canvas.WinForm.KeyPreview = false;
			//BusEngine.UI.Canvas.WinForm.IsMdiContainer = true;
			//BusEngine.UI.Canvas.WinForm.Paint += Paint;

			// FPS
			System.Timers.Timer fpsTimer = new System.Timers.Timer(1000);
			fpsTimer.Elapsed += OnFPS;
			//fpsTimer.Interval = 1000;
			fpsTimer.AutoReset = true;
			fpsTimer.Enabled = true;

			if (!float.TryParse(BusEngine.Engine.SettingProject["console_commands"]["sys_FPS"], out FPSSetting)) {
				FPSSetting = 60F;
			}

			glControl = new OpenTK.GLControl(new OpenTK.Graphics.GraphicsMode(
				color: new OpenTK.Graphics.ColorFormat(0, 0, 0, 1),
				depth: 32,
				stencil: 32,
				samples: 1000,
				accum: new OpenTK.Graphics.ColorFormat(0, 0, 0, 1),
				buffers: 2,
				stereo: true
			));

			//((System.ComponentModel.ISupportInitialize)glControl).BeginInit();
			//BusEngine.UI.Canvas.WinForm.SuspendLayout();

			glControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;

			int r_Width;
			int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["r_Width"], out r_Width);
			if (r_Width < BusEngine.UI.Canvas.WinForm.Width) {
				r_Width = BusEngine.UI.Canvas.WinForm.Width;
			}

			int r_Height;
			int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["r_Height"], out r_Height);
			if (r_Height < BusEngine.UI.Canvas.WinForm.Height) {
				r_Height = BusEngine.UI.Canvas.WinForm.Height;
			}

			glControl.Size = new System.Drawing.Size(r_Width, r_Height);
			glControl.VSync = BusEngine.Engine.SettingProject["console_commands"]["sys_VSync"] == "1";
			glControl.Location = new System.Drawing.Point((r_Width - BusEngine.UI.Canvas.WinForm.Width) / -2, 1);
            glControl.Visible  = true;
            glControl.Enabled  = true;
			// подключаем событие клавиатуры
			/* glControl.KeyDown += new System.Windows.Forms.KeyEventHandler(KeyDown);
			glControl.KeyUp += new System.Windows.Forms.KeyEventHandler(KeyUp); */
			glControl.KeyDown += new System.Windows.Forms.KeyEventHandler(KeyDown);
			glControl.KeyUp += new System.Windows.Forms.KeyEventHandler(KeyUp);

			// подключаем событие мыши
			//glControl.MouseMove += new System.Windows.Forms.MouseEventHandler(MouseMove);
			glControl.MouseLeave += new System.EventHandler(MouseLeave);
			glControl.MouseWheel += new System.Windows.Forms.MouseEventHandler(MouseWheel);
			//BusEngine.UI.Canvas.WinForm.MouseMove += new System.Windows.Forms.MouseEventHandler(MouseMove);
			//BusEngine.UI.Canvas.WinForm.MouseLeave += new System.EventHandler(MouseLeave);
			//BusEngine.UI.Canvas.WinForm.MouseWheel += new System.Windows.Forms.MouseEventHandler(MouseWheel);
			// подключаем событие загрузки окна OpenTK
			glControl.Load += new System.EventHandler(Load);
			// подключаем событие изменение размера окна
			glControl.Resize += new System.EventHandler(Resize);
			// подключаем событие рисования
			//glControl.Paint += new System.Windows.Forms.PaintEventHandler(Paint);
			//BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint);

			//glControl.TabIndex = 0;
			glControl.TabStop = false;
			BusEngine.UI.Canvas.WinForm.Controls.Add(glControl);
			//BusEngine.UI.Canvas.WinForm.Focus();
			//glControl.Focus();
			
			//BusEngine.UI.Canvas.WinForm.TabIndex = 1;
			/* BusEngine.UI.Canvas.WinForm.Focus(); */

			//((System.ComponentModel.ISupportInitialize)(glControl)).EndInit();
			//BusEngine.UI.Canvas.WinForm.ResumeLayout(false);
			//BusEngine.UI.Canvas.WinForm.PerformLayout();

			//glControl.PerformContextUpdate();

			//BusEngine.Log.Info(OpenTK.Input.KeyboardDevice());
			//OpenTK.Input.KeyboardDevice.Item(OpenTK.Input.Key.Ctrl);

			BusEngine.Engine.GameStart();
		}

		public static void KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			if (e != null) {
				BusEngine.Log.Info("ss {0}", e.KeyCode);
				IsKeys.Add(e.KeyCode);

				if (IsKeys.Contains(System.Windows.Forms.Keys.Pause)) {
					if (Pause) {
						BusEngine.Engine.GameStart();
						Pause = false;
						IsKeys.Remove(System.Windows.Forms.Keys.Pause);
					}
				}
			} else {
				// фикс некоторых кнопок
				OpenTK.Input.KeyboardState keyboard = OpenTK.Input.Keyboard.GetState();

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
				}
			}

			if (IsKeys.Contains(System.Windows.Forms.Keys.Escape)) {
				BusEngine.Engine.Shutdown();
			}

			if (e == null) {
				if (IsKeys.Contains(System.Windows.Forms.Keys.W)) {
					if (IsKeys.Contains(System.Windows.Forms.Keys.ShiftKey) || IsKeys.Contains(System.Windows.Forms.Keys.LShiftKey)) {
						position += front * speed * 2;
					} else {
						position += front * speed;
					}
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.S)) {
					if (IsKeys.Contains(System.Windows.Forms.Keys.ShiftKey) || IsKeys.Contains(System.Windows.Forms.Keys.LShiftKey)) {
						position -= front * speed * 2;
					} else {
						position -= front * speed;
					}
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.A)) {
					if (IsKeys.Contains(System.Windows.Forms.Keys.ShiftKey) || IsKeys.Contains(System.Windows.Forms.Keys.LShiftKey)) {
						position -= OpenTK.Vector3.Normalize(OpenTK.Vector3.Cross(front, up)) * speed * 2;
					} else {
						position -= OpenTK.Vector3.Normalize(OpenTK.Vector3.Cross(front, up)) * speed;
					}
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.D)) {
					if (IsKeys.Contains(System.Windows.Forms.Keys.ShiftKey) || IsKeys.Contains(System.Windows.Forms.Keys.LShiftKey)) {
						position += OpenTK.Vector3.Normalize(OpenTK.Vector3.Cross(front, up)) * speed * 2;
					} else {
						position += OpenTK.Vector3.Normalize(OpenTK.Vector3.Cross(front, up)) * speed;
					}
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.Up)) {
					position += up * speed;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.Down)) {
					position -= up * speed;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.Left)) {
					lastPos.X = OpenTK.Input.Mouse.GetState().X + mousespeed * 2000;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.Right)) {
					lastPos.X = OpenTK.Input.Mouse.GetState().X - mousespeed * 2000;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.Space)) {
					position = new OpenTK.Vector3(0.0F, 5.0F, 10.0F);
					//front = new OpenTK.Vector3(0.0F, 0.0F, 1.0F);
					//up = OpenTK.Vector3.UnitY;
					//vp = OpenTK.Matrix4.CreateRotationX(OpenTK.MathHelper.DegreesToRadians(90)) * OpenTK.Matrix4.LookAt(position, position + front, up) * projection;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.Pause)) {
					IsKeys.Remove(System.Windows.Forms.Keys.Pause);
					if (!Pause) {
						Pause = true;
						BusEngine.Engine.GameStop();
					}
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.T)) {
					q += 10;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.G)) {
					q -= 10;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.Y)) {
					line++;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.H)) {
					line--;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.NumPad4) && p.M11 >= 0.09F) {
					p.M11 = (float)System.Math.Round(p.M11 + 0.1F, 1);
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.NumPad1) && p.M11 > 0.1F) {
					p.M11 = (float)System.Math.Round(p.M11 - 0.1F, 1);
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.NumPad5) && p.M43 >= 0.09F) {
					p.M43 = (float)System.Math.Round(p.M43 + 0.1F, 1);
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.NumPad2) && p.M43 > 0.1F) {
					p.M43 = (float)System.Math.Round(p.M43 - 0.1F, 1);
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.NumPad6) && p.M44 >= 0.09F) {
					p.M44 = (float)System.Math.Round(p.M44 + 0.1F, 1);
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.NumPad3) && p.M44 > 0.1F) {
					p.M44 = (float)System.Math.Round(p.M44 - 0.1F, 1);
				}

				// из-за Radeon - убираем возможность показа стены с двух сторон.
				// https://ravesli.com/urok-22-otsechenie-granej-v-opengl/
				if (IsKeys.Contains(System.Windows.Forms.Keys.F1)) {
					IsKeys.Remove(System.Windows.Forms.Keys.F1);
					if (!F1) {
						F1 = true;
						OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.CullFace);
					} else {
						OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.CullFace);
						F1 = false;
					}
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.F2)) {
					IsKeys.Remove(System.Windows.Forms.Keys.F2);
					if (!F2) {
						F2 = true;
						OpenTK.Graphics.OpenGL4.GL.CullFace(OpenTK.Graphics.OpenGL4.CullFaceMode.Front);
					} else {
						OpenTK.Graphics.OpenGL4.GL.CullFace(OpenTK.Graphics.OpenGL4.CullFaceMode.Back);
						F2 = false;
					}
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.F3)) {
					IsKeys.Remove(System.Windows.Forms.Keys.F3);
					if (!F3) {
						F3 = true;
						OpenTK.Graphics.OpenGL4.GL.PolygonMode(OpenTK.Graphics.OpenGL4.MaterialFace.FrontAndBack, OpenTK.Graphics.OpenGL4.PolygonMode.Line);
					} else {
						OpenTK.Graphics.OpenGL4.GL.PolygonMode(OpenTK.Graphics.OpenGL4.MaterialFace.FrontAndBack, OpenTK.Graphics.OpenGL4.PolygonMode.Fill);
						F3 = false;
					}
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.F4)) {
					IsKeys.Remove(System.Windows.Forms.Keys.F4);
					if (!F4) {
						F4 = true;
						OpenTK.Graphics.OpenGL4.GL.FrontFace(OpenTK.Graphics.OpenGL4.FrontFaceDirection.Ccw);
					} else {
						OpenTK.Graphics.OpenGL4.GL.FrontFace(OpenTK.Graphics.OpenGL4.FrontFaceDirection.Cw);
						F4 = false;
					}
				}
			}
		}

		private static void KeyUp(object sender, System.Windows.Forms.KeyEventArgs e) {
			IsKeys.Remove(e.KeyCode);
		}

		private static void MouseLeave(object sender, System.EventArgs e) {
			// если очень долго крутить в одну сторону, то будет ошибка (для теста использовать мышьку и болгарку).
			OpenTK.Input.MouseState mouse = OpenTK.Input.Mouse.GetState();

			OpenTK.Input.Mouse.SetPosition(BusEngine.UI.Canvas.WinForm.DesktopLocation.X + BusEngine.UI.Canvas.WinForm.Width/2.0f, BusEngine.UI.Canvas.WinForm.DesktopLocation.Y + BusEngine.UI.Canvas.WinForm.Height/2.0f);

			lastPos = new OpenTK.Vector2(mouse.X, mouse.Y);
		}

		private void MouseWheel(object sender, System.EventArgs e) {
			// если очень долго крутить в одну сторону, то будет ошибка (для теста использовать мышьку и болгарку).
			OpenTK.Input.MouseState mouse = OpenTK.Input.Mouse.GetState();

			if (lastWheel < mouse.Wheel) {
				speed += 0.1F;
			} else {
				speed -= 0.1F;
			}

			if (System.Math.Round(speed, 1) < 0.1F) {
				speed = 0.1F;
			}

			lastWheel = mouse.Wheel;
		}

		private static void MouseMove(object sender, System.EventArgs e) {
			OpenTK.Input.MouseState mouse = OpenTK.Input.Mouse.GetState();

			OpenTK.Vector2 delta = lastPos - new OpenTK.Vector2(mouse.X, mouse.Y);

			/* if (lastWheel < mouse.Scroll.Y) {
				speed += 0.1F;
			} else {
				speed -= 0.1F;
			}

			if (speed < 0.1F) {
				speed = 0.1F;
			}

			lastWheel = (int)mouse.Scroll.Y; */

			AddRotation(delta.X, delta.Y);

			lastPos = new OpenTK.Vector2(mouse.X, mouse.Y);
			//BusEngine.Log.Info("MouseState: {0}", mouse);
			double y = System.Math.Cos(Orientation.Y);

			front.X = (float)(System.Math.Sin(Orientation.X) * y);
			front.Y = (float)System.Math.Sin(Orientation.Y);
			front.Z = (float)(System.Math.Cos(Orientation.X) * y);

			//front = OpenTK.Vector3.Normalize(front);
		}

		private static void AddRotation(float mx, float my) {
			mx = mx * mousespeed;
			my = my * mousespeed;

			Orientation.X = (Orientation.X + mx) % ((float)System.Math.PI * 2.0f);
			Orientation.Y = System.Math.Max(System.Math.Min(Orientation.Y + my, (float)System.Math.PI / 2.0f - 0.001f), (float)-System.Math.PI / 2.0f + 0.001f);
		}

		private int CompileShader(OpenTK.Graphics.OpenGL4.ShaderType type, string source) {
			int success, shader = OpenTK.Graphics.OpenGL4.GL.CreateShader(type);

			if (System.IO.File.Exists(source)) {
				OpenTK.Graphics.OpenGL4.GL.ShaderSource(shader, System.IO.File.ReadAllText(source));
			} else {
				OpenTK.Graphics.OpenGL4.GL.ShaderSource(shader, source);
			}

			OpenTK.Graphics.OpenGL4.GL.CompileShader(shader);

			OpenTK.Graphics.OpenGL4.GL.GetShader(shader, OpenTK.Graphics.OpenGL4.ShaderParameter.CompileStatus, out success);
			if (success == 0) {
				throw new System.Exception("Failed to compile {type}: " + OpenTK.Graphics.OpenGL4.GL.GetShaderInfoLog(shader));
			}

			return shader;
		}

		private int CompileProgram(string vertShader, string fragShader, string geomShader) {
			int geom, frag, vert, success, program = OpenTK.Graphics.OpenGL4.GL.CreateProgram();

			vert = CompileShader(OpenTK.Graphics.OpenGL4.ShaderType.VertexShaderArb, vertShader);
			frag = CompileShader(OpenTK.Graphics.OpenGL4.ShaderType.FragmentShaderArb, fragShader);
			geom = CompileShader(OpenTK.Graphics.OpenGL4.ShaderType.GeometryShader, geomShader);

			OpenTK.Graphics.OpenGL4.GL.AttachShader(program, vert);
			OpenTK.Graphics.OpenGL4.GL.AttachShader(program, frag);
			OpenTK.Graphics.OpenGL4.GL.AttachShader(program, geom);

			OpenTK.Graphics.OpenGL4.GL.LinkProgram(program);

			OpenTK.Graphics.OpenGL4.GL.GetProgram(program, OpenTK.Graphics.OpenGL4.GetProgramParameterName.LinkStatus, out success);
			if (success == 0) {
				throw new System.Exception("Could not link program: " + OpenTK.Graphics.OpenGL4.GL.GetProgramInfoLog(program));
			}

			OpenTK.Graphics.OpenGL4.GL.DetachShader(program, vert);
			OpenTK.Graphics.OpenGL4.GL.DetachShader(program, frag);
			OpenTK.Graphics.OpenGL4.GL.DetachShader(program, geom);

			OpenTK.Graphics.OpenGL4.GL.DeleteShader(vert);
			OpenTK.Graphics.OpenGL4.GL.DeleteShader(frag);
			OpenTK.Graphics.OpenGL4.GL.DeleteShader(geom);

			return program;
		}

		private void Load(object sender, System.EventArgs e) {
			// устанавливаем контекст GL
			//glControl.MakeCurrent();
			//glControl.Focus();

			OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
			OpenTK.Graphics.OpenGL.GL.LoadIdentity();
			//OpenTK.Graphics.OpenGL.GL.Ext.BeginVertexShader();

			//OpenTK.Graphics.OpenGL4.GL.BeginConditionalRender(1, OpenTK.Graphics.OpenGL4.ConditionalRenderType.QueryWait);

			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.LineSmooth);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PolygonSmooth);
			// из-за Radeon - убираем возможность показа стены с двух сторон.
			// https://ravesli.com/urok-22-otsechenie-granej-v-opengl/
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.CullFace);
			//OpenTK.Graphics.OpenGL4.GL.CullFace(OpenTK.Graphics.OpenGL4.CullFaceMode.Front);
			//OpenTK.Graphics.OpenGL4.GL.FrontFace(OpenTK.Graphics.OpenGL4.FrontFaceDirection.Cw);

			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.DepthTest);

			// режим каркаса включён
			OpenTK.Graphics.OpenGL4.GL.PolygonMode(OpenTK.Graphics.OpenGL4.MaterialFace.FrontAndBack, OpenTK.Graphics.OpenGL4.PolygonMode.Line);
			// режим каркаса отключён
			OpenTK.Graphics.OpenGL4.GL.PolygonMode(OpenTK.Graphics.OpenGL4.MaterialFace.FrontAndBack, OpenTK.Graphics.OpenGL4.PolygonMode.Fill);
			//OpenTK.Graphics.OpenGL4.GL.PolygonMode(OpenTK.Graphics.OpenGL4.MaterialFace.FrontAndBack, OpenTK.Graphics.OpenGL4.PolygonMode.Line);
			
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.StencilTest);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Dither);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Blend);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ColorLogicOp);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ScissorTest);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Texture1D);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Texture2D);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PolygonOffsetPoint);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PolygonOffsetLine);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance0);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipPlane0);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance1);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipPlane1);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance2);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipPlane2);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance3);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipPlane3);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance4);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipPlane4);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance5);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipPlane5);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance6);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance7);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Convolution1D);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Convolution1DExt);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Convolution2D);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Convolution2DExt);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Separable2D);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Separable2DExt);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Histogram);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.HistogramExt);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.MinmaxExt);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PolygonOffsetFill);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.RescaleNormal);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.RescaleNormalExt);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Texture3DExt);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.InterlaceSgix);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Multisample);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.MultisampleSgis);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SampleAlphaToCoverage);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SampleAlphaToMaskSgis);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SampleAlphaToOne);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SampleAlphaToOneSgis);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SampleCoverage);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SampleMaskSgis);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.TextureColorTableSgi);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ColorTable);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ColorTableSgi);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PostConvolutionColorTable);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PostConvolutionColorTableSgi);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PostColorMatrixColorTable);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PostColorMatrixColorTableSgi);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Texture4DSgis);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PixelTexGenSgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SpriteSgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ReferencePlaneSgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.IrInstrument1Sgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.CalligraphicFragmentSgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FramezoomSgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FogOffsetSgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SharedTexturePaletteExt);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.DebugOutputSynchronous);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.AsyncHistogramSgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PixelTextureSgis);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.AsyncTexImageSgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.AsyncDrawPixelsSgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.AsyncReadPixelsSgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLightingSgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentColorMaterialSgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight0Sgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight1Sgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight2Sgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight3Sgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight4Sgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight5Sgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight6Sgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight7Sgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FogCoordArray);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ColorSum);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SecondaryColorArray);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.TextureRectangle);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.TextureCubeMap);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ProgramPointSize);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.VertexProgramPointSize);
			// вершинные шейдеры будут работать в двустороннем цветовом режиме - уменьшит fps
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.VertexProgramTwoSide);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.DepthClamp);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.TextureCubeMapSeamless);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PointSprite);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SampleShading);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.RasterizerDiscard);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PrimitiveRestartFixedIndex);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FramebufferSrgb);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SampleMask);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PrimitiveRestart);
			// включение отладки OpenGL - ловить сообщения через спец. события - уменьшит fps
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.DebugOutput);

			/* OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.LineSmooth);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.PolygonSmooth);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.CullFace);
			//OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.DepthTest);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.StencilTest);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.Dither);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.Blend);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ColorLogicOp);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ScissorTest);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.Texture1D);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.Texture2D);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.PolygonOffsetPoint);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.PolygonOffsetLine);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance0);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ClipPlane0);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance1);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ClipPlane1);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance2);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ClipPlane2);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance3);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ClipPlane3);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance4);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ClipPlane4);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance5);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ClipPlane5);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance6);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance7);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.Convolution1D);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.Convolution1DExt);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.Convolution2D);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.Convolution2DExt);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.Separable2D);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.Separable2DExt);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.Histogram);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.HistogramExt);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.MinmaxExt);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.PolygonOffsetFill);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.RescaleNormal);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.RescaleNormalExt);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.Texture3DExt);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.InterlaceSgix);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.Multisample);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.MultisampleSgis);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.SampleAlphaToCoverage);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.SampleAlphaToMaskSgis);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.SampleAlphaToOne);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.SampleAlphaToOneSgis);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.SampleCoverage);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.SampleMaskSgis);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.TextureColorTableSgi);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ColorTable);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ColorTableSgi);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.PostConvolutionColorTable);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.PostConvolutionColorTableSgi);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.PostColorMatrixColorTable);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.PostColorMatrixColorTableSgi);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.Texture4DSgis);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.PixelTexGenSgix);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.SpriteSgix);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ReferencePlaneSgix);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.IrInstrument1Sgix);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.CalligraphicFragmentSgix);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.FramezoomSgix);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.FogOffsetSgix);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.SharedTexturePaletteExt);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.DebugOutputSynchronous);
			//OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.AsyncHistogramSgix);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.PixelTextureSgis);
			//OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.AsyncTexImageSgix);
			//OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.AsyncDrawPixelsSgix);
			//OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.AsyncReadPixelsSgix);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLightingSgix);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentColorMaterialSgix);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight0Sgix);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight1Sgix);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight2Sgix);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight3Sgix);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight4Sgix);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight5Sgix);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight6Sgix);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight7Sgix);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.FogCoordArray);
			//OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ColorSum);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.SecondaryColorArray);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.TextureRectangle);
			//OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.TextureCubeMap);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.ProgramPointSize);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.VertexProgramPointSize);
			// вершинные шейдеры будут работать в двустороннем цветовом режиме - уменьшит fps
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.VertexProgramTwoSide);
			//OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.DepthClamp);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.TextureCubeMapSeamless);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.PointSprite);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.SampleShading);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.RasterizerDiscard);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.PrimitiveRestartFixedIndex);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.FramebufferSrgb);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.SampleMask);
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.PrimitiveRestart);
			// вкоючение отладки OpenGL - ловить сообщения через спец. события - уменьшит fps
			OpenTK.Graphics.OpenGL4.GL.Disable(OpenTK.Graphics.OpenGL4.EnableCap.DebugOutput); */

			//Resize(glControl, System.EventArgs.Empty);

			OpenTK.Matrix4.LookAt(position, position + front, up);

			/* BusEngine.Log.Clear();
			OpenTK.Matrix4 ggg = OpenTK.Matrix4.LookAt(position, position + front, up);
OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
OpenTK.Graphics.OpenGL.GL.LoadIdentity();
OpenTK.Graphics.OpenGL.GL.LoadMatrix(ref ggg);
			ggg.M41 = position.X;
			ggg.M42 = position.Y;
			ggg.M43 = position.Z;
			ggg.M2 = position + front;
			ggg.M3 = up;
			BusEngine.Log.Info("<{0} {1} {2} {3}>", ggg.M11, ggg.M12, ggg.M13, ggg.M14);
			BusEngine.Log.Info("<{0} {1} {2} {3}>", ggg.M21, ggg.M22, ggg.M23, ggg.M24);
			BusEngine.Log.Info("<{0} {1} {2} {3}>", ggg.M31, ggg.M32, ggg.M33, ggg.M34);
			BusEngine.Log.Info("<{0} {1} {2} {3}>", ggg.M41, ggg.M42, ggg.M43, ggg.M44);
			BusEngine.Log.Info("{0}", "============");
			BusEngine.Log.Info("{0}", OpenTK.Matrix4.LookAt(position, position + front, up));
			BusEngine.Log.Info("{0}", "============");
			BusEngine.Log.Info("{0}", position);
			BusEngine.Log.Info("{0}", front);
			BusEngine.Log.Info("{0}", position + front);
			BusEngine.Log.Info("{0}", up); */

			v = new OpenTK.Vector3(0.0F, 1.0F, 0.0F);
			p = OpenTK.Matrix4.CreateTranslation(0.0F, 0.0F, 0.0F);
			z = 0.0F;

			VAO = OpenTK.Graphics.OpenGL4.GL.GenVertexArray();
			OpenTK.Graphics.OpenGL4.GL.BindVertexArray(VAO);

			EBO = OpenTK.Graphics.OpenGL4.GL.GenBuffer();
			OpenTK.Graphics.OpenGL4.GL.BindBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ElementArrayBuffer, EBO);
			OpenTK.Graphics.OpenGL4.GL.BufferData(OpenTK.Graphics.OpenGL4.BufferTarget.ElementArrayBuffer, IndexData.Length * sizeof(int), IndexData, OpenTK.Graphics.OpenGL4.BufferUsageHint.DynamicRead);

			PositionBuffer = OpenTK.Graphics.OpenGL4.GL.GenBuffer();
			OpenTK.Graphics.OpenGL4.GL.BindBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer, PositionBuffer);
			OpenTK.Graphics.OpenGL4.GL.BufferData(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer, VertexData.Length * sizeof(float) * 3, VertexData, OpenTK.Graphics.OpenGL4.BufferUsageHint.DynamicRead);

			OpenTK.Graphics.OpenGL4.GL.EnableVertexAttribArray(0);
			OpenTK.Graphics.OpenGL4.GL.VertexAttribPointer(0, 3, OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);

			ColorBuffer = OpenTK.Graphics.OpenGL4.GL.GenBuffer();
			OpenTK.Graphics.OpenGL4.GL.BindBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer, ColorBuffer);
			OpenTK.Graphics.OpenGL4.GL.BufferData(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer, ColorData.Length * sizeof(float) * 4, ColorData, OpenTK.Graphics.OpenGL4.BufferUsageHint.DynamicRead);

			OpenTK.Graphics.OpenGL4.GL.EnableVertexAttribArray(1);
			OpenTK.Graphics.OpenGL4.GL.VertexAttribPointer(1, 4, OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);

			OpenTK.Graphics.OpenGL4.GL.BindVertexArray(0);

			// создаём шейдеры
			CubeShader = CompileProgram(
				vertShader: BusEngine.Engine.DataDirectory + "Shader/cube_vert.glsl",
				fragShader: BusEngine.Engine.DataDirectory + "Shader/cube_frag.glsl",
				geomShader: BusEngine.Engine.DataDirectory + "Shader/cube_geom.glsl"
			);
			OpenTK.Graphics.OpenGL4.GL.UseProgram(CubeShader);
			prog = OpenTK.Graphics.OpenGL4.GL.GetUniformLocation(CubeShader, "MVP");
			progP = OpenTK.Graphics.OpenGL4.GL.GetUniformLocation(CubeShader, "P");
			progVP = OpenTK.Graphics.OpenGL4.GL.GetUniformLocation(CubeShader, "VP");
			progA = OpenTK.Graphics.OpenGL4.GL.GetUniformLocation(CubeShader, "A");
		}

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

			OpenTK.Graphics.OpenGL4.GL.Viewport(0, 0, Width, Height);
			//glControl.MaximumSize = new System.Drawing.Size(Width, Height);

			float sys_FOV;
			float.TryParse(BusEngine.Engine.SettingProject["console_commands"]["sys_FOV"], out sys_FOV);
			projection = OpenTK.Matrix4.CreatePerspectiveFieldOfView(OpenTK.MathHelper.DegreesToRadians(sys_FOV), (float)Width / (float)Height, 0.01F, 10000.0F);

			/* BusEngine.UI.Canvas.WinForm.ResumeLayout(false);
			BusEngine.UI.Canvas.WinForm.PerformLayout(); */
		}

		// событие FPS
		private static void OnFPS(object source, System.Timers.ElapsedEventArgs e) {
			if (FPS > 0) {
				FPSInfo = FPS;
			} else {
				FPSInfo = FPSSetting;
			}

			BusEngine.Log.Clear();
			//BusEngine.Log.Info("FPS Setting: {0}", FPSSetting);
			BusEngine.Log.Info("FPS: {0}", FPSInfo);
			BusEngine.Log.Info("mytime: {0}", mytime);
			BusEngine.Log.Info("Cubes Count: {0}", cubes);
			BusEngine.Log.Info("VSync: {0}", BusEngine.Engine.SettingProject["console_commands"]["sys_VSync"] == "1");
			BusEngine.Log.Info("Resolution Display: {0} X {1}", System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Height);
			BusEngine.Log.Info("Resolution Window: {0} X {1}", BusEngine.UI.Canvas.WinForm.Width, BusEngine.UI.Canvas.WinForm.Height);
			BusEngine.Log.Info("Resolution Setting: {0} X {1}", BusEngine.Engine.SettingProject["console_commands"]["r_Width"], BusEngine.Engine.SettingProject["console_commands"]["r_Height"]);
			//BusEngine.Log.Info("GPU: {0}", "NVidea GeForce GT 1030 2 GB");
			//BusEngine.Log.Info("CPU: {0}", "AMD Athlon II x4 645");
			//BusEngine.Log.Info("RAM: {0}", "4 GB");

			FPS = 0;
			mytime = 0;
		}

		private static int cube, q = 8000, line = 75;
		private static float x, y, z, left = -12.0F, right = 12.0F, top = 12.0F, bottom = -12.0F;
		private static OpenTK.Vector3 v;
		private static OpenTK.Matrix4 vp, a, p = OpenTK.Matrix4.CreateTranslation(0.0F, 0.0F, 0.0F);

		/* private float[] Matrix4ToArray(OpenTK.Matrix4 matrix) {
			float[] data = new float[16];

			for (int i = 0; i < 4; i++) {
				for (int j = 0; j < 4; j++) {
					data[i*4+j] = matrix[i, j];

				}
			}

			return data;
		} */

		private static long timeNow;
		private static long mytime;

		// вызывается при отрисовки каждого кадра
		public override void OnGameStart() {
			// скрываем иконку
			BusEngine.UI.Canvas.WinForm.Cursor = new System.Windows.Forms.Cursor(new System.Drawing.Bitmap(16, 16).GetHicon());

			FPS = (int)FPSSetting;

			BusEngine.Game.MyPlugin.MouseLeave(null, null);
		}

		// вызывается при отрисовки каждого кадра
		public override void OnGameStop() {
			BusEngine.UI.Canvas.WinForm.Cursor = null;
		}

		// вызывается при отрисовки каждого кадра
		// 500000 кубов
		public override void OnGameUpdate() {
			FPS++;
			FPSDelta = FPSSetting / FPSInfo;
			//glControl.MakeCurrent();
			//glControl.Focus();
			//BusEngine.Log.Info("mc: {0}", System.DateTime.Now.Millisecond);
			//BusEngine.Log.Info("Ticks: {0}", System.DateTime.Now.Ticks / 10000);

			// fix работы мыши и клавиатуры - вызов каждые 20 мс
			if (System.DateTime.Now.Ticks - timeNow > 10000) {
				mytime++;
				timeNow = System.DateTime.Now.Ticks;

				//BusEngine.Log.Info("mytime: {0}", FPSDelta);
				_angle += 0.3F * FPSDelta;
				BusEngine.Game.MyPlugin.MouseMove(null, null);
				BusEngine.Game.MyPlugin.KeyDown(null, null);
			}

			//using (new BusEngine.Benchmark("OpenTK")) {
				OpenTK.Graphics.OpenGL4.GL.Clear(OpenTK.Graphics.OpenGL4.ClearBufferMask.ColorBufferBit | OpenTK.Graphics.OpenGL4.ClearBufferMask.DepthBufferBit);
				//OpenTK.Graphics.OpenGL4.GL.CullFace(OpenTK.Graphics.OpenGL4.CullFaceMode.Front);
				//OpenTK.Graphics.OpenGL4.GL.FrontFace(OpenTK.Graphics.OpenGL4.FrontFaceDirection.Cw);

				vp = OpenTK.Matrix4.CreateRotationX(OpenTK.MathHelper.DegreesToRadians(90)) * OpenTK.Matrix4.LookAt(position, position + front, up) * projection;
				OpenTK.Graphics.OpenGL4.GL.UniformMatrix4(progVP, true, ref vp);

				a = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(_angle));
				OpenTK.Graphics.OpenGL4.GL.UniformMatrix4(progA, true, ref a);

				x = 1;
				y = 1;

				OpenTK.Graphics.OpenGL4.GL.BindVertexArray(VAO);

				for (cube = 0; cube < q; cube++)  {
					// левый-нижний
					p.M41 = left * x;
					p.M42 = bottom * y;
					p.M43 = z;

					OpenTK.Graphics.OpenGL4.GL.UniformMatrix4(progP, true, ref p);
					OpenTK.Graphics.OpenGL4.GL.DrawElements(OpenTK.Graphics.OpenGL4.BeginMode.Triangles, IndexData.Length, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt, 0);

					// правый-нижний
					p.M41 = right * x - right;
					p.M42 = bottom * y;
					p.M43 = z;

					OpenTK.Graphics.OpenGL4.GL.UniformMatrix4(progP, true, ref p);
					OpenTK.Graphics.OpenGL4.GL.DrawElements(OpenTK.Graphics.OpenGL4.BeginMode.Triangles, IndexData.Length, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt, 0);

					// левый-верхний
					p.M41 = left * x;
					p.M42 = top * y - top;
					p.M43 = z;

					OpenTK.Graphics.OpenGL4.GL.UniformMatrix4(progP, true, ref p);
					OpenTK.Graphics.OpenGL4.GL.DrawElements(OpenTK.Graphics.OpenGL4.BeginMode.Triangles, IndexData.Length, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt, 0);

					// правый-верхний
					p.M41 = right * x - right;
					p.M42 = top * y - top;
					p.M43 = z;

					OpenTK.Graphics.OpenGL4.GL.UniformMatrix4(progP, true, ref p);
					OpenTK.Graphics.OpenGL4.GL.DrawElements(OpenTK.Graphics.OpenGL4.BeginMode.Triangles, IndexData.Length, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt, 0);

					// убираем второй массив
					if (y >= line) {
						x++;
						y = 1;
					} else {
						y++;
					}
				}

				OpenTK.Graphics.OpenGL4.GL.BindVertexArray(0);

				//if (cube > cubes) {
					cubes = cube * 4 * 16;
				//}

				glControl.SwapBuffers();
			//}
		}

		// перед закрытием BusEngine
		public override void Shutdown() {
			BusEngine.Engine.GameStop();
		}
	}
	/** API BusEngine.Plugin */
}
/** API BusEngine.Game - пользовательский код */