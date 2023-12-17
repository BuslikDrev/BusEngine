/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2024; BuslikDrev - Усе правы захаваны. */

// benchmark https://cc.davelozinski.com/c-sharp/for-vs-foreach-vs-while
// OpenGL http://pm.samgtu.ru/sites/pm.samgtu.ru/files/materials/comp_graph/RedBook_OpenGL.pdf

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
		private static OpenTK.Vector3 front = new OpenTK.Vector3(0.0F, 0.0F, -1.0F);
		private static OpenTK.Vector3 up = OpenTK.Vector3.UnitY; // new OpenTK.Vector3(0.0f, 1.0f, 0.0f);
		private static OpenTK.Vector3 Orientation = new OpenTK.Vector3((float)System.Math.PI, 0F, 0F);
		private static float speed = 0.1F;
		private static float mousespeed = 0.1F;
		private static bool IsKeysUpdateGL = false;
		private static System.Collections.Generic.HashSet<System.Windows.Forms.Keys> IsKeys = new System.Collections.Generic.HashSet<System.Windows.Forms.Keys>();

		private static OpenTK.Vector2 lastPos;
		public static float MouseSensitivity = 0.0025f;

		private System.Timers.Timer _timer = null;
		private float _angle = 0.0f;
		private int CubeShader;
		private int VAO;
		private int EBO;
		private int PositionBuffer;
		private int ColorBuffer;
		private OpenTK.Matrix4 projection;

		private int prog;
		private float Time;
		private OpenTK.Vector2 Mouse;

		private static readonly OpenTK.Vector3[] VertexData = new OpenTK.Vector3[] {
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
			 0,  1,  2,  2,  3,  0,
			 4,  5,  6,  6,  7,  4,
			 8,  9, 10, 10, 11,  8,
			12, 13, 14, 14, 15, 12,
			16, 17, 18, 18, 19, 16,
			20, 21, 22, 22, 23, 20,
		};

		private static OpenTK.Graphics.Color4[] ColorData = new OpenTK.Graphics.Color4[] {
			OpenTK.Graphics.Color4.Silver, OpenTK.Graphics.Color4.Silver, OpenTK.Graphics.Color4.Silver, OpenTK.Graphics.Color4.Silver,
			OpenTK.Graphics.Color4.Honeydew, OpenTK.Graphics.Color4.Honeydew, OpenTK.Graphics.Color4.Honeydew, OpenTK.Graphics.Color4.Honeydew,
			OpenTK.Graphics.Color4.Moccasin, OpenTK.Graphics.Color4.Moccasin, OpenTK.Graphics.Color4.Moccasin, OpenTK.Graphics.Color4.Moccasin,
			OpenTK.Graphics.Color4.IndianRed, OpenTK.Graphics.Color4.IndianRed, OpenTK.Graphics.Color4.IndianRed, OpenTK.Graphics.Color4.IndianRed,
			OpenTK.Graphics.Color4.PaleVioletRed, OpenTK.Graphics.Color4.PaleVioletRed, OpenTK.Graphics.Color4.PaleVioletRed, OpenTK.Graphics.Color4.PaleVioletRed,
			OpenTK.Graphics.Color4.ForestGreen, OpenTK.Graphics.Color4.ForestGreen, OpenTK.Graphics.Color4.ForestGreen, OpenTK.Graphics.Color4.ForestGreen,
		};

		// при запуске BusEngine после создания формы Canvas
		public override unsafe void InitializeСanvas() {
			// деламем окно не поверх других окон
			BusEngine.UI.Canvas.WinForm.TopMost = false;

			// подключаем событие рисования
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint);

			// скрываем иконку
			BusEngine.UI.Canvas.WinForm.Cursor = new System.Windows.Forms.Cursor(new System.Drawing.Bitmap(16, 16).GetHicon());

			// FPS
			System.Timers.Timer fpsTimer = new System.Timers.Timer(1000);
			fpsTimer.Elapsed += OnFPS;
			fpsTimer.AutoReset = true;
			fpsTimer.Enabled = true;

			if (!int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["sys_FPS"], out FPSSetting)) {
				FPSSetting = 100;
			}

			BusEngine.UI.Canvas.WinForm.SuspendLayout();

			glControl = new OpenTK.GLControl();
			glControl.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
			glControl.Location = new System.Drawing.Point(0, 1);
			int r_Width;
			int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["r_Width"], out r_Width);
			int r_Height;
			int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["r_Height"], out r_Height);
			glControl.Size = new System.Drawing.Size(r_Width, r_Height);
			glControl.MaximumSize = new System.Drawing.Size(r_Width, r_Height);
			glControl.TabIndex = 1;
			glControl.VSync = BusEngine.Engine.SettingProject["console_commands"]["sys_VSync"] == "1";

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

				if (IsKeys.Contains(System.Windows.Forms.Keys.W)) {
					position += front * speed; 

					if (IsKeys.Contains(System.Windows.Forms.Keys.ShiftKey) || IsKeys.Contains(System.Windows.Forms.Keys.LShiftKey)) {
						position += front * 5;
					}
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.S)) {
					position -= front * speed;

					if (IsKeys.Contains(System.Windows.Forms.Keys.ShiftKey) || IsKeys.Contains(System.Windows.Forms.Keys.LShiftKey)) {
						position -= front * 5;
					}
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.A)) {
					position -= OpenTK.Vector3.Normalize(OpenTK.Vector3.Cross(front, up)) * speed;

					if (IsKeys.Contains(System.Windows.Forms.Keys.ShiftKey) || IsKeys.Contains(System.Windows.Forms.Keys.LShiftKey)) {
						position -= OpenTK.Vector3.Normalize(OpenTK.Vector3.Cross(front, up)) * 5;
					}
				}

				if (IsKeys.Contains(System.Windows.Forms.Keys.D)) {
					position += OpenTK.Vector3.Normalize(OpenTK.Vector3.Cross(front, up)) * speed;

					if (IsKeys.Contains(System.Windows.Forms.Keys.ShiftKey) || IsKeys.Contains(System.Windows.Forms.Keys.LShiftKey)) {
						position += OpenTK.Vector3.Normalize(OpenTK.Vector3.Cross(front, up)) * 5;
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
			}
		}

		private static void KeyUpGL(object sender, System.Windows.Forms.KeyEventArgs e) {
			IsKeys.Remove(e.KeyCode);
			IsKeysUpdateGL = false;
		}

		private void Move(float x, float y, float z) {
			OpenTK.Vector3 offset = new OpenTK.Vector3();
			OpenTK.Vector3 forward = new OpenTK.Vector3((float)System.Math.Sin((float)Orientation.X), 0, (float)System.Math.Cos((float)Orientation.X));

			offset += x * new OpenTK.Vector3(-forward.Z, 0, forward.X);;
			offset += y * forward;
			offset.Y += z;

			offset.NormalizeFast();
			offset = OpenTK.Vector3.Multiply(offset, mousespeed);

			position += offset;
		}

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

			Mouse = lastPos;
			double y = System.Math.Cos(Orientation.Y);

			front.X = (float)(System.Math.Sin(Orientation.X) * y);
			front.Y = (float)System.Math.Sin(Orientation.Y);
			front.Z = (float)(System.Math.Cos(Orientation.X) * y);
		}

		private void AddRotation(float x, float y) {
			x *= MouseSensitivity;
			y *= MouseSensitivity;

			Orientation.X = (Orientation.X + x) % ((float)System.Math.PI * 2.0f);
			Orientation.Y = System.Math.Max(System.Math.Min(Orientation.Y + y, (float)System.Math.PI / 2.0f - 0.1f), (float)-System.Math.PI / 2.0f + 0.1f);
		}

		private int CompileProgram(string vertexShader, string fragmentShader) {
			int CompileShader(OpenTK.Graphics.OpenGL4.ShaderType type, string source) {
				int shader = OpenTK.Graphics.OpenGL4.GL.CreateShader(type);

				if (System.IO.File.Exists(source)) {
					OpenTK.Graphics.OpenGL4.GL.ShaderSource(shader, System.IO.File.ReadAllText(source));
				} else {
					OpenTK.Graphics.OpenGL4.GL.ShaderSource(shader, source);
				}

				OpenTK.Graphics.OpenGL4.GL.CompileShader(shader);


				OpenTK.Graphics.OpenGL4.GL.GetShader(shader, OpenTK.Graphics.OpenGL4.ShaderParameter.CompileStatus, out int status);
				if (status == 0) {
					throw new System.Exception("Failed to compile {type}: " + OpenTK.Graphics.OpenGL4.GL.GetShaderInfoLog(shader));
				}

				return shader;
			}

			int program = OpenTK.Graphics.OpenGL4.GL.CreateProgram();

			int vert = CompileShader(OpenTK.Graphics.OpenGL4.ShaderType.VertexShader, vertexShader);
			int frag = CompileShader(OpenTK.Graphics.OpenGL4.ShaderType.FragmentShader, fragmentShader);

			OpenTK.Graphics.OpenGL4.GL.AttachShader(program, vert);
			OpenTK.Graphics.OpenGL4.GL.AttachShader(program, frag);

			OpenTK.Graphics.OpenGL4.GL.LinkProgram(program);

			OpenTK.Graphics.OpenGL4.GL.GetProgram(program, OpenTK.Graphics.OpenGL4.GetProgramParameterName.LinkStatus, out int success);
			if (success == 0) {
				throw new System.Exception("Could not link program: " + OpenTK.Graphics.OpenGL4.GL.GetProgramInfoLog(program));
			}

			OpenTK.Graphics.OpenGL4.GL.DetachShader(program, vert);
			OpenTK.Graphics.OpenGL4.GL.DetachShader(program, frag);

			OpenTK.Graphics.OpenGL4.GL.DeleteShader(vert);
			OpenTK.Graphics.OpenGL4.GL.DeleteShader(frag);

			return program;
		}

        private void glControl_Load(object sender, System.EventArgs e) {
			// устанавливаем контекст GL
			//glControl.MakeCurrent();

			glControl.Resize += glControl_Resize;

			//OpenTK.Graphics.OpenGL.GL.MatrixMode(OpenTK.Graphics.OpenGL.MatrixMode.Modelview);
			//OpenTK.Graphics.OpenGL.GL.LoadIdentity();

			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.DepthTest);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.DepthClamp);

			/* OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.LineSmooth);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PolygonSmooth);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.CullFace);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.DepthTest);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.StencilTest);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Dither);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Blend);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ColorLogicOp);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ScissorTest);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Texture1D);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Texture2D);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PolygonOffsetPoint);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PolygonOffsetLine);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance0);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipPlane0);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance1);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipPlane1);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance2);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipPlane2);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance3);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipPlane3);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance4);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipPlane4);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance5);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipPlane5);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance6);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ClipDistance7);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Convolution1D);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Convolution1DExt);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Convolution2D);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Convolution2DExt);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Separable2D);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Separable2DExt);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Histogram);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.HistogramExt);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.MinmaxExt);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PolygonOffsetFill);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.RescaleNormal);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.RescaleNormalExt);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Texture3DExt);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.InterlaceSgix);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Multisample);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.MultisampleSgis);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SampleAlphaToCoverage);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SampleAlphaToMaskSgis);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SampleAlphaToOne);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SampleAlphaToOneSgis);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SampleCoverage);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SampleMaskSgis);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.TextureColorTableSgi);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ColorTable);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ColorTableSgi);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PostConvolutionColorTable);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PostConvolutionColorTableSgi);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PostColorMatrixColorTable);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PostColorMatrixColorTableSgi);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.Texture4DSgis);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PixelTexGenSgix);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SpriteSgix);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ReferencePlaneSgix);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.IrInstrument1Sgix);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.CalligraphicFragmentSgix);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FramezoomSgix);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FogOffsetSgix);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SharedTexturePaletteExt);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.DebugOutputSynchronous);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.AsyncHistogramSgix);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PixelTextureSgis);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.AsyncTexImageSgix);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.AsyncDrawPixelsSgix);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.AsyncReadPixelsSgix);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLightingSgix);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentColorMaterialSgix);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight0Sgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight1Sgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight2Sgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight3Sgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight4Sgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight5Sgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight6Sgix);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FragmentLight7Sgix);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FogCoordArray);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ColorSum);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SecondaryColorArray);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.TextureRectangle);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.TextureCubeMap);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.ProgramPointSize);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.VertexProgramPointSize);
			// вершинные шейдеры будут работать в двустороннем цветовом режиме - уменьшит fps
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.VertexProgramTwoSide);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.DepthClamp);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.TextureCubeMapSeamless);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PointSprite);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SampleShading);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.RasterizerDiscard);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PrimitiveRestartFixedIndex);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.FramebufferSrgb);
			OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.SampleMask);
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.PrimitiveRestart);
			// вкоючение отладки OpenGL - ловить сообщения через спец. события - уменьшит fps
			//OpenTK.Graphics.OpenGL4.GL.Enable(OpenTK.Graphics.OpenGL4.EnableCap.DebugOutput); */

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
			CubeShader = CompileProgram(BusEngine.Engine.DataDirectory + "Shader/cube_vert.glsl", BusEngine.Engine.DataDirectory + "Shader/cube_frag.glsl");
			OpenTK.Graphics.OpenGL4.GL.UseProgram(CubeShader);
			prog = OpenTK.Graphics.OpenGL4.GL.GetUniformLocation(CubeShader, "MVP");

			// создаём таймер для работы клавиатуры и вращению блоков независимо от частоты кадров и возможности измерять скорость.
			_timer = new System.Timers.Timer();
			_timer.Elapsed += (ts, te) => {
				BusEngine.Game.MyPlugin.KeyDownGL(null, null);
				_angle += 0.3F;
				Time = te.SignalTime.Millisecond;
			};
			_timer.Interval = 10;
			_timer.Start();

			//OpenTK.Matrix4.LookAt(position, position + front, up);

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
			OpenTK.Graphics.OpenGL4.GL.BufferData(OpenTK.Graphics.OpenGL4.BufferTarget.ArrayBuffer, ColorData.Length * sizeof(uint) * 4, ColorData, OpenTK.Graphics.OpenGL4.BufferUsageHint.StaticDraw);

			OpenTK.Graphics.OpenGL4.GL.EnableVertexAttribArray(1);
			OpenTK.Graphics.OpenGL4.GL.VertexAttribPointer(1, 4, OpenTK.Graphics.OpenGL4.VertexAttribPointerType.Float, false, sizeof(float) * 4, 0);

			v = new OpenTK.Vector3(0.0F, 1.0F, 0.0F);
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

			projection = OpenTK.Matrix4.CreatePerspectiveFieldOfView(OpenTK.MathHelper.DegreesToRadians(45.0F), ((float)Width / (float)Height), 0.01F, 10000.0F);

			BusEngine.UI.Canvas.WinForm.ResumeLayout(false);
			BusEngine.UI.Canvas.WinForm.PerformLayout();
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
			//BusEngine.Log.Info("RAM: {0}", 1);
		}

		private static int x;
		private static int y;
		private static float z = 0.0F;
		private static float left = -3.0F;
		private static float right = 3.0F;
		private static float top = 3.0F;
		private static float bottom = -3.0F;
		private static float leftres;
		private static float rightres;
		private static float topres;
		private static float bottomres;
		private static OpenTK.Matrix4 vp;
		private static OpenTK.Matrix4 mvp;
		private static OpenTK.Vector3 v;

		private void Paint(object sender, System.Windows.Forms.PaintEventArgs e) {
			//using (new BusEngine.Benchmark("OpenTK")) {
				OpenTK.Graphics.OpenGL4.GL.Clear(OpenTK.Graphics.OpenGL4.ClearBufferMask.ColorBufferBit | OpenTK.Graphics.OpenGL4.ClearBufferMask.DepthBufferBit);

				vp = OpenTK.Matrix4.LookAt(position, position + front, up) * projection;

				int cube = 1;
				mvp = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(_angle)) * OpenTK.Matrix4.CreateTranslation(0.0F, 0.0F, 0.0F) * vp;
				OpenTK.Graphics.OpenGL4.GL.UniformMatrix4(prog, true, ref mvp);
				OpenTK.Graphics.OpenGL4.GL.DrawElements(OpenTK.Graphics.OpenGL4.BeginMode.Triangles, IndexData.Length, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt, 0);

				for (x = 1; x < 116; x++)  {
					cube++;
					mvp = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(_angle)) * OpenTK.Matrix4.CreateTranslation(right * x, 0.0F, z) * vp;
					OpenTK.Graphics.OpenGL4.GL.UniformMatrix4(prog, true, ref mvp);
					OpenTK.Graphics.OpenGL4.GL.DrawElements(OpenTK.Graphics.OpenGL4.BeginMode.Triangles, IndexData.Length, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt, 0);

					cube++;
					mvp = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(_angle)) * OpenTK.Matrix4.CreateTranslation(left * x, 0.0F, z) * vp;
					OpenTK.Graphics.OpenGL4.GL.UniformMatrix4(prog, true, ref mvp);
					OpenTK.Graphics.OpenGL4.GL.DrawElements(OpenTK.Graphics.OpenGL4.BeginMode.Triangles, IndexData.Length, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt, 0);
				}

				for (y = 1; y < 65; y++)  {
					cube++;
					mvp = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(_angle)) * OpenTK.Matrix4.CreateTranslation(0.0F, bottom * y, z) * vp;
					OpenTK.Graphics.OpenGL4.GL.UniformMatrix4(prog, true, ref mvp);
					OpenTK.Graphics.OpenGL4.GL.DrawElements(OpenTK.Graphics.OpenGL4.BeginMode.Triangles, IndexData.Length, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt, 0);

					cube++;
					mvp = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(_angle)) * OpenTK.Matrix4.CreateTranslation(0.0F, top * y, z) * vp;
					OpenTK.Graphics.OpenGL4.GL.UniformMatrix4(prog, true, ref mvp);
					OpenTK.Graphics.OpenGL4.GL.DrawElements(OpenTK.Graphics.OpenGL4.BeginMode.Triangles, IndexData.Length, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt, 0);
				}

				for (x = 0; x < 116; x++)  {
					for (y = 1; y < 65; y++)  {
						leftres = left * x;
						bottomres = bottom * y;
						rightres = right * x;
						topres = top * y;
						cube++;
						mvp = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(_angle)) * OpenTK.Matrix4.CreateTranslation(leftres, bottomres, z) * vp;
						OpenTK.Graphics.OpenGL4.GL.UniformMatrix4(prog, true, ref mvp);
						OpenTK.Graphics.OpenGL4.GL.DrawElements(OpenTK.Graphics.OpenGL4.BeginMode.Triangles, IndexData.Length, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt, 0);

						cube++;
						mvp = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(_angle)) * OpenTK.Matrix4.CreateTranslation(rightres, bottomres, z) * vp;
						OpenTK.Graphics.OpenGL4.GL.UniformMatrix4(prog, true, ref mvp);
						OpenTK.Graphics.OpenGL4.GL.DrawElements(OpenTK.Graphics.OpenGL4.BeginMode.Triangles, IndexData.Length, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt, 0);

						cube++;
						mvp = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(_angle)) * OpenTK.Matrix4.CreateTranslation(leftres, topres, z) * vp;
						OpenTK.Graphics.OpenGL4.GL.UniformMatrix4(prog, true, ref mvp);
						OpenTK.Graphics.OpenGL4.GL.DrawElements(OpenTK.Graphics.OpenGL4.BeginMode.Triangles, IndexData.Length, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt, 0);

						cube++;
						mvp = OpenTK.Matrix4.CreateFromAxisAngle(v, OpenTK.MathHelper.DegreesToRadians(_angle)) * OpenTK.Matrix4.CreateTranslation(rightres, topres, z) * vp;
						OpenTK.Graphics.OpenGL4.GL.UniformMatrix4(prog, true, ref mvp);
						OpenTK.Graphics.OpenGL4.GL.DrawElements(OpenTK.Graphics.OpenGL4.BeginMode.Triangles, IndexData.Length, OpenTK.Graphics.OpenGL4.DrawElementsType.UnsignedInt, 0);
					}
				}

				if (cube > cubes) {
					cubes = cube;
				}

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