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
		// при запуске BusEngine после создания формы Canvas
		public override void Initialize() {

		}

		// настройки
		private static int FPS = 0;
		private static int FPSSetting;
		private static int FPSInfo = 0;
		private static int cubes = 0;

		private static OpenTK.GLControl glControl;
		private static OpenTK.Vector3 position = new OpenTK.Vector3(0.0F, 0.0F, 10.0F);
		private static OpenTK.Vector3 front = new OpenTK.Vector3(0.0F, 0.0F, 1.0F);
		private static OpenTK.Vector3 up = OpenTK.Vector3.UnitY; // new OpenTK.Vector3(0.0f, 1.0f, 0.0f);
		private static OpenTK.Vector3 Orientation = new OpenTK.Vector3((float)System.Math.PI, 0F, 0F);
		private static float speed = 0.5F;
		private static float mousespeed = 0.0025f;
		private static bool IsKeysUpdateGL = false;
		private static System.Collections.Generic.HashSet<System.Windows.Forms.Keys> IsKeys = new System.Collections.Generic.HashSet<System.Windows.Forms.Keys>();
		private static OpenTK.Vector2 lastPos;

		private System.Timers.Timer _timer = null;
		private float _angle = 0.0f;
		private int CubeShader;
		private int VAO;
		private int EBO;
		private int PositionBuffer;
		private int ColorBuffer;
		private OpenTK.Matrix4 projection;

		private int prog;
		private int progVP;
		private int progP;
		private int progA;

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

		private static readonly int[] IndexData = new int[36] {
			 0,  1,  2,  2,  3,  0,
			 4,  5,  6,  6,  7,  4,
			 8,  9, 10, 10, 11,  8,
			12, 13, 14, 14, 15, 12,
			16, 17, 18, 18, 19, 16,
			20, 21, 22, 22, 23, 20,
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
		public override unsafe void InitializeСanvas() {
			// деламем окно не поверх других окон
			BusEngine.UI.Canvas.WinForm.TopMost = false;

			// подключаем событие рисования
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint);

			// скрываем иконку
			//BusEngine.UI.Canvas.WinForm.Cursor = new System.Windows.Forms.Cursor(new System.Drawing.Bitmap(16, 16).GetHicon());

			// FPS
			System.Timers.Timer fpsTimer = new System.Timers.Timer(1000);
			fpsTimer.Elapsed += OnFPS;
			//fpsTimer.Interval = 1000;
			fpsTimer.AutoReset = true;
			fpsTimer.Enabled = true;

			if (!int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["sys_FPS"], out FPSSetting)) {
				FPSSetting = 100;
			}
			//BusEngine.UI.Canvas.WinForm.Location = new System.Drawing.Point(-100, -100);
			BusEngine.UI.Canvas.WinForm.SuspendLayout();

			glControl = new OpenTK.GLControl();
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

			// подключаем событие клавиатуры
			glControl.KeyDown += KeyDownGL;
			glControl.KeyUp += KeyUpGL;
			// подключаем событие мыши
			glControl.MouseMove += MouseMoveGL;
			glControl.MouseLeave += MouseLeaveGL;
			// подключаем событие загрузки окна OpenTK
			glControl.Load += glControl_Load;

			BusEngine.UI.Canvas.WinForm.Controls.Add(glControl);

			BusEngine.UI.Canvas.WinForm.ResumeLayout(false);
			BusEngine.UI.Canvas.WinForm.PerformLayout();

			BusEngine.Engine.GameStart();
		}

		public static void KeyDownGL(object sender, System.Windows.Forms.KeyEventArgs e) {
			if (e != null) {
				IsKeys.Add(e.KeyCode);
			}

			if (e == null || !IsKeysUpdateGL) {
				IsKeysUpdateGL = true;

				if (IsKeys.Contains(System.Windows.Forms.Keys.Escape)) {
					BusEngine.Engine.Shutdown();
				}

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

				if (IsKeys.Contains(System.Windows.Forms.Keys.Down)) {
					position += up * speed;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.Up)) {
					position -= up * speed;
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.Space)) {
					position = new OpenTK.Vector3(0.0F, 0.0F, 10.0F);
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
			}
		}

		private static void KeyUpGL(object sender, System.Windows.Forms.KeyEventArgs e) {
			IsKeys.Remove(e.KeyCode);
			IsKeysUpdateGL = false;
		}

		/* private void Move(float x, float y, float z) {
			OpenTK.Vector3 offset = new OpenTK.Vector3();
			OpenTK.Vector3 forward = new OpenTK.Vector3((float)System.Math.Sin((float)Orientation.X), 0, (float)System.Math.Cos((float)Orientation.X));

			offset += x * new OpenTK.Vector3(-forward.Z, 0, forward.X);
			offset += y * forward;
			offset.Y += z;

			offset.NormalizeFast();
			offset = OpenTK.Vector3.Multiply(offset, mousespeed);

			position += offset;
		} */

		private void MouseLeaveGL(object sender, System.EventArgs e) {
			// если очень долго крутить в одну сторону, то будет ошибка (для теста использовать мышьку и болгарку).
			var mouse = OpenTK.Input.Mouse.GetState();

			OpenTK.Input.Mouse.SetPosition(BusEngine.UI.Canvas.WinForm.DesktopLocation.X + BusEngine.UI.Canvas.WinForm.Width/2.0f, BusEngine.UI.Canvas.WinForm.DesktopLocation.Y + BusEngine.UI.Canvas.WinForm.Height/2.0f);

			lastPos = new OpenTK.Vector2(mouse.X, mouse.Y);
		}

		private void MouseMoveGL(object sender, System.EventArgs e) {
			var mouse = OpenTK.Input.Mouse.GetState();

			OpenTK.Vector2 delta = lastPos - new OpenTK.Vector2(mouse.X, mouse.Y);

			AddRotation(delta.X, delta.Y);

			lastPos = new OpenTK.Vector2(mouse.X, mouse.Y);

			double y = System.Math.Cos(Orientation.Y);

			front.X = (float)(System.Math.Sin(Orientation.X) * y);
			front.Y = (float)System.Math.Sin(Orientation.Y);
			front.Z = (float)(System.Math.Cos(Orientation.X) * y);

			//front = OpenTK.Vector3.Normalize(front);
		}

		private void AddRotation(float mx, float my) {
			mx = mx * mousespeed;
			my = my * mousespeed;

			Orientation.X = (Orientation.X + mx) % ((float)System.Math.PI * 2.0f);
			Orientation.Y = System.Math.Max(System.Math.Min(Orientation.Y + my, (float)System.Math.PI / 2.0f - 0.1f), (float)-System.Math.PI / 2.0f + 0.1f);
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

			vert = CompileShader(OpenTK.Graphics.OpenGL4.ShaderType.VertexShader, vertShader);
			frag = CompileShader(OpenTK.Graphics.OpenGL4.ShaderType.FragmentShader, fragShader);
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

		private void glControl_Load(object sender, System.EventArgs e) {
			// устанавливаем контекст GL
			//glControl.MakeCurrent();

			glControl.Resize += glControl_Resize;

			OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
			OpenTK.Graphics.OpenGL.GL.LoadIdentity();
			//OpenTK.Graphics.OpenGL.GL.Ext.BeginVertexShader();

			//OpenTK.Graphics.OpenGL4.GL.BeginConditionalRender(1, OpenTK.Graphics.OpenGL4.ConditionalRenderType.QueryWait);

			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.LineSmooth);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PolygonSmooth);
			// из-за Radeon - убираем возможность показа стены с двух сторон.
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.CullFace);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.DepthTest);
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
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Multisample);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.MultisampleSgis);
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

			glControl_Resize(glControl, System.EventArgs.Empty);

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

			// создаём таймер для работы клавиатуры и вращению блоков независимо от частоты кадров и возможности измерять скорость.
			_timer = new System.Timers.Timer();
			_timer.Elapsed += (ts, te) => {
				BusEngine.Game.MyPlugin.KeyDownGL(null, null);
				_angle += 0.3F;
			};
			_timer.Interval = 10;
			_timer.Start();

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
			OpenTK.Graphics.OpenGL4.GL.BufferData(OpenTK.Graphics.OpenGL4.BufferTarget.ElementArrayBuffer, IndexData.Length * sizeof(int), IndexData, OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);

			PositionBuffer = OpenTK.Graphics.OpenGL4.GL.GenBuffer();
			OpenTK.Graphics.OpenGL4.GL.BindBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer, PositionBuffer);
			OpenTK.Graphics.OpenGL4.GL.BufferData(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer, VertexData.Length * sizeof(float) * 3, VertexData, OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);

			OpenTK.Graphics.OpenGL4.GL.EnableVertexAttribArray(0);
			OpenTK.Graphics.OpenGL4.GL.VertexAttribPointer(0, 3, OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Float, false, sizeof(float) * 3, 0);

			ColorBuffer = OpenTK.Graphics.OpenGL4.GL.GenBuffer();
			OpenTK.Graphics.OpenGL4.GL.BindBuffer(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer, ColorBuffer);
			OpenTK.Graphics.OpenGL4.GL.BufferData(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer, ColorData.Length * sizeof(float) * 4, ColorData, OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);

			OpenTK.Graphics.OpenGL4.GL.EnableVertexAttribArray(1);
			OpenTK.Graphics.OpenGL4.GL.VertexAttribPointer(1, 4, OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);
		}

		private void glControl_Resize(object sender, System.EventArgs e) {
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
			glControl.MaximumSize = new System.Drawing.Size(Width, Height);

			float sys_FOV;
			float.TryParse(BusEngine.Engine.SettingProject["console_commands"]["sys_FOV"], out sys_FOV);
			projection = OpenTK.Matrix4.CreatePerspectiveFieldOfView(OpenTK.MathHelper.DegreesToRadians(sys_FOV), (float)Width / (float)Height, 0.01F, 10000.0F);

			/* BusEngine.UI.Canvas.WinForm.ResumeLayout(false);
			BusEngine.UI.Canvas.WinForm.PerformLayout(); */
		}

		// событие FPS
		private static void OnFPS(object source, System.Timers.ElapsedEventArgs e) {
			FPSInfo = FPS;
			FPS = 0;

			BusEngine.Log.Clear();
			//BusEngine.Log.Info("FPS Setting: {0}", FPSSetting);
			BusEngine.Log.Info("FPS: {0}", FPSInfo);
			BusEngine.Log.Info("Cubes Count: {0}", cubes);
			BusEngine.Log.Info("VSync: {0}", BusEngine.Engine.SettingProject["console_commands"]["sys_VSync"] == "1");
			BusEngine.Log.Info("Resolution Display: {0} X {1}", System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Width, System.Windows.Forms.Screen.PrimaryScreen.Bounds.Size.Height);
			BusEngine.Log.Info("Resolution Window: {0} X {1}", BusEngine.UI.Canvas.WinForm.Width, BusEngine.UI.Canvas.WinForm.Height);
			BusEngine.Log.Info("Resolution Setting: {0} X {1}", BusEngine.Engine.SettingProject["console_commands"]["r_Width"], BusEngine.Engine.SettingProject["console_commands"]["r_Height"]);
			//BusEngine.Log.Info("GPU: {0}", "NVidea GeForce GT 1030 2 GB");
			//BusEngine.Log.Info("CPU: {0}", "AMD Athlon II x4 645");
			//BusEngine.Log.Info("RAM: {0}", "4 GB");
		}

		private static int cube, q = 1, line = 1;
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

		// вызывается при отрисовки каждого кадра
		// 50000 кубов
		private void Paint(object sender, System.Windows.Forms.PaintEventArgs e) {
			//using (new BusEngine.Benchmark("OpenTK")) {
				OpenTK.Graphics.OpenGL4.GL.Clear(OpenTK.Graphics.OpenGL4.ClearBufferMask.ColorBufferBit | OpenTK.Graphics.OpenGL4.ClearBufferMask.DepthBufferBit);

				vp = OpenTK.Matrix4.LookAt(position, position + front, up) * projection;
				OpenTK.Graphics.OpenGL4.GL.UniformMatrix4(progVP, true, ref vp);

				a = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(_angle));
				OpenTK.Graphics.OpenGL4.GL.UniformMatrix4(progA, true, ref a);

				x = 1;
				y = 1;

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

				//if (cube > cubes) {
					cubes = cube * 4 * 16;
				//}

				glControl.SwapBuffers();
			//}
		}

		// вызывается при отрисовки каждого кадра
		public override void OnGameUpdate() {
			FPS++;
		}

		// перед закрытием BusEngine
		public override void Shutdown() {
			BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint);
		}
	}
	/** API BusEngine.Plugin */
}
/** API BusEngine.Game - пользовательский код */