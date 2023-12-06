/* Аўтар: "БуслікДрэў" ( https://buslikdrev.by/ ) */
/* © 2016-2024; BuslikDrev - Усе правы захаваны. */

/** API BusEngine.Game - пользовательский код */
namespace BusEngine.Game {
	/** API BusEngine.Plugin */
	public class MyPlugin : BusEngine.Plugin {
		// настройки
		private static int FPS = 0;
		private static int FPSSetting;
		private static int FPSInfo = 0;

		private static System.Drawing.Bitmap result;
		private static System.Numerics.Vector3 lamp;
		private static Voxel[][] voxels;

		private static System.Windows.Forms.TrackBar tbHorizontal;
		private static System.Windows.Forms.TrackBar tbHorizontalCache;
		private static System.Windows.Forms.TrackBar tbVerticale;

		private static float horizontal = 0F;
		private static float horizontalCache = 0F;
		private static float verticale = 0F;

		private const float SCALE_HEIGHT = 1F / 7F;
		private const float NORMAL_Y = 5F;

		// при запуске BusEngine после создания формы Canvas
		public override void InitializeСanvas() {
			// деламем окно не поверх других окон
			BusEngine.UI.Canvas.WinForm.TopMost = false;
			//BusEngine.UI.Canvas.WinForm.AutoSize = true;
			//BusEngine.UI.Canvas.WinForm.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowOnly;

			if (!int.TryParse(BusEngine.Engine.SettingProject["console_commands"]["sys_FPS"], out FPSSetting)) {
				FPSSetting = 100;
			}

			// тест графики
			// https://rsdn.org/article/gdi/gdiplus2mag.xml
			BusEngine.UI.Canvas.WinForm.Paint += new System.Windows.Forms.PaintEventHandler(Paint);

			// подключаем событие мыши
			BusEngine.UI.Canvas.WinForm.MouseMove += new System.Windows.Forms.MouseEventHandler(MouseMove);

			// подключаем событие клавиатуры
			BusEngine.UI.Canvas.WinForm.KeyDown += new System.Windows.Forms.KeyEventHandler(KeyDown);
			BusEngine.UI.Canvas.WinForm.KeyUp += new System.Windows.Forms.KeyEventHandler(KeyUp);

			// подключаем событие изменение размера окна
			BusEngine.UI.Canvas.WinForm.SizeChanged += new System.EventHandler(SizeChanged);

			// подключаем событие двойного щелчка
			BusEngine.UI.Canvas.WinForm.DoubleClick += new System.EventHandler(DoubleClick);

			// FPS
			System.Timers.Timer fpsTimer = new System.Timers.Timer(1000);
			fpsTimer.Elapsed += OnFPS;
			fpsTimer.AutoReset = true;
			fpsTimer.Enabled = true;

			// источник света
			lamp = System.Numerics.Vector3.Normalize(new System.Numerics.Vector3(-1, 1, -1));

			// загружаем карту высот
			using (System.Drawing.Bitmap heightMap = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(BusEngine.Engine.DataDirectory + "Textures/heightmap.png")) {
				// создаем обертку для быстрого доступа к пикселам
				using (System.Drawing.ImageWrapper wr = new System.Drawing.ImageWrapper(heightMap)) {
					int height, h1, h2, dx, dy, light, l = 0, ll = 0, lll = 0, limit = 0;
					/* System.Collections.Generic.IEnumerator<System.Drawing.Point> e = wr.GetEnumerator();
					while (e.MoveNext()) {
						l++;
					} */
					foreach (int[] p in wr) {
						l++;
					}

					voxels = new Voxel[BusEngine.Engine.Device.ProcessorCount][];
					limit = l / BusEngine.Engine.Device.ProcessorCount + 1;

					foreach (Voxel[] voxel in voxels) {
						lll++;
						if (lll == voxels.Length) {
							if (l < 0) {
								l = 0;
							}
							voxels[lll-1] = new Voxel[l];
							l = 0;
						} else {
							if (l > limit) {
								voxels[lll-1] = new Voxel[limit];
								l = l - limit;
							} else {
								if (l < 0) {
									l = 0;
								}
								voxels[lll-1] = new Voxel[l];
								l = 0;
							}
						}
					}

					lll = 0;

					// читаем карту высот, формируем воксели
					foreach (int[] p in wr) {
						if (p[0] > 0 && p[1] > 0) {
							// высота
							height = wr[p[0], p[1]].G;

							// высота в соседних точках
							h1 = wr[p[0] - 1, p[1]].G;
							h2 = wr[p[0], p[1] - 1].G;

							// считаем градиент
							dx = height - h1;
							dy = height - h2;

							// считаем нормаль
							System.Numerics.Vector3 n = System.Numerics.Vector3.Normalize(new System.Numerics.Vector3(dx, NORMAL_Y, dy));

							// считаем свет
							light = (int)(System.Numerics.Vector3.Dot(n, lamp) * 255);

							if (light < 0) {
								light = 0;
							} else if (light > 255) {
								light = 255;
							}

							// создаем воксель
							if (ll == limit) {
								ll = 0;
								lll++;
							}

							voxels[lll][ll] = new Voxel{
								Pos = new System.Numerics.Vector3(p[0], height * SCALE_HEIGHT, p[1]),
								Normal = n,
								Light = System.Drawing.Color.FromArgb(light, light, light)
							};

							ll++;
							l++;
						}
					}
				}

				// создаем результирующее изображение
				//result = new System.Drawing.Bitmap(heightMap.Width, heightMap.Height);
				result = new System.Drawing.Bitmap(BusEngine.UI.Canvas.WinForm.Width, BusEngine.UI.Canvas.WinForm.Height);
				//BusEngine.Log.Info("FPS Settiппngresult: {0} {1}", result.Width, result.Height);
				//result.SetResolution(48.0F, 48.0F);
			}

			// задаем размер формы
			//BusEngine.UI.Canvas.WinForm.Size = new System.Drawing.Size(result.Width, 4 * result.Height / 5 + 60);
			//BusEngine.UI.Canvas.WinForm.BackColor = System.Drawing.Color.White;

			// создаем трекбары
			tbHorizontal = new System.Windows.Forms.TrackBar{
				Parent = BusEngine.UI.Canvas.WinForm,
				Text = "1111111111",
				TabIndex = 0,
				TabStop = false,
				TickStyle = System.Windows.Forms.TickStyle.TopLeft,
				AutoSize = true,
				Maximum = 360,
				Minimum = -360,
				Value = 180,
				Left = 0,
				Width = 200
			};
			tbHorizontalCache = new System.Windows.Forms.TrackBar{
				Parent = BusEngine.UI.Canvas.WinForm,
				Text = "1111111111",
				TabIndex = 0,
				TabStop = false,
				TickStyle = System.Windows.Forms.TickStyle.TopLeft,
				AutoSize = true,
				Maximum = 360,
				Minimum = -360,
				Value = 0,
				Left = 250,
				Width = 200
			};
			tbVerticale = new System.Windows.Forms.TrackBar{
				Parent = BusEngine.UI.Canvas.WinForm,
				Text = "1111111111",
				TabIndex = 0,
				TabStop = false,
				TickStyle = System.Windows.Forms.TickStyle.BottomRight,
				Orientation = System.Windows.Forms.Orientation.Vertical,
				AutoSize = true,
				Maximum = 180,
				Minimum = 90,
				Value = 170,
				Left = BusEngine.UI.Canvas.WinForm.Width - 60,
				Height = 200
			};

			tbHorizontal.ValueChanged += new System.EventHandler(tb_ValueChangedtbHorizontal);
			tbHorizontalCache.ValueChanged += new System.EventHandler(tb_ValueChangedtbHorizontalCache);
			tbVerticale.ValueChanged += new System.EventHandler(tb_ValueChangedtbVerticale);

			tb_ValueChangedtbHorizontal(null, System.EventArgs.Empty);
			tb_ValueChangedtbHorizontalCache(null, System.EventArgs.Empty);
			tb_ValueChangedtbVerticale(null, System.EventArgs.Empty);

			BusEngine.Engine.GameStart();
		}

		// вызывается при отрисовки каждого кадра
		public /* async */ override void OnGameUpdate() {
			FPS++;

			BusEngine.Log.Clear();
			BusEngine.Log.Info("FPS Setting: {0}", FPSSetting);
			BusEngine.Log.Info("FPS: {0}", FPSInfo);
		}

		// перед закрытием BusEngine
		public override void Shutdown() {
			BusEngine.UI.Canvas.WinForm.Paint -= new System.Windows.Forms.PaintEventHandler(Paint);
			tbHorizontal.ValueChanged -= new System.EventHandler(tb_ValueChangedtbHorizontal);
			tbVerticale.ValueChanged -= new System.EventHandler(tb_ValueChangedtbVerticale);
		}

		// событие мыши
		private static void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
			/* if (BusEngine.Engine.IsGame == false) {
				BusEngine.Engine.GameStart();
			} else {
				BusEngine.Engine.GameStop();
			} */
		}

		// событие клавиатуры
		private static System.Collections.Generic.HashSet<System.Windows.Forms.Keys> IsKeys = new System.Collections.Generic.HashSet<System.Windows.Forms.Keys>();
		private static bool IsKeysUpdate = false;
		private static void KeyDown(object sender, System.Windows.Forms.KeyEventArgs e) {
			/* BusEngine.Log.Info("{0} = {1}", "Alt", e.Alt);
			BusEngine.Log.Info("{0} = {1}", "Control", e.Control);
			BusEngine.Log.Info("{0} = {1}", "Handled", e.Handled);
			BusEngine.Log.Info("{0} = {1}", "KeyCode", e.KeyCode);
			BusEngine.Log.Info("{0} = {1}", "KeyValue", e.KeyValue);
			BusEngine.Log.Info("{0} = {1}", "KeyData", e.KeyData);
			BusEngine.Log.Info("{0} = {1}", "Modifiers", e.Modifiers);
			BusEngine.Log.Info("{0} = {1}", "Shift", e.Shift);
			BusEngine.Log.Info("{0} = {1}", "SuppressKeyPress", e.SuppressKeyPress); */

			IsKeys.Add(e.KeyCode);

			if (!IsKeysUpdate) {
				IsKeysUpdate = true;

				if (IsKeys.Contains(System.Windows.Forms.Keys.W) && tbVerticale.Value < tbVerticale.Maximum) {
					tbVerticale.Value = tbVerticale.Value+1;
				}
				if (IsKeys.Contains(System.Windows.Forms.Keys.S) && tbVerticale.Value > tbVerticale.Minimum) {
					tbVerticale.Value = tbVerticale.Value-1;
				}
				if (IsKeys.Contains(System.Windows.Forms.Keys.D) && tbHorizontal.Value+1 < tbHorizontal.Maximum) {
					tbHorizontal.Value = tbHorizontal.Value+2;
				}
				if (IsKeys.Contains(System.Windows.Forms.Keys.A) && tbHorizontal.Value-1 > tbHorizontal.Minimum) {
					tbHorizontal.Value = tbHorizontal.Value-2;
				}
			}
		}
		private static void KeyUp(object sender, System.Windows.Forms.KeyEventArgs e) {
			IsKeys.Remove(e.KeyCode);
			IsKeysUpdate = false;
		}

		private static void SizeChanged(object sender, System.EventArgs e) {
			tbVerticale.Left = BusEngine.UI.Canvas.WinForm.Width - 60;
			result = new System.Drawing.Bitmap(BusEngine.UI.Canvas.WinForm.Width, BusEngine.UI.Canvas.WinForm.Height);
			IsScroll = true;
			IsDrawImage = false;
			//BusEngine.Engine.GameUpdate();
		}

		private static void DoubleClick(object sender, System.EventArgs e) {
			System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog() {
				Title = "Сохранение 3D изображения",
				Filter = "Image|*.png"
			};
			if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
				result.Save(sfd.FileName);
			}
		}

		// событие FPS
		private static void OnFPS(object source, System.Timers.ElapsedEventArgs e) {
			FPSInfo = FPS;
			FPS = 0;
		}

		private static bool IsScroll = false;
		private void tb_ValueChangedtbHorizontal(object sender, System.EventArgs e) {
			horizontal = (float)(tbHorizontal.Value * System.Math.PI / 180D);
			IsDrawImage = false;
			if (!IsScroll) {
				IsScroll = true;
				//BusEngine.Engine.GameUpdate();
			}
		}
		private void tb_ValueChangedtbHorizontalCache(object sender, System.EventArgs e) {
			horizontalCache = tbHorizontalCache.Value;
			if (!IsScroll) {
				IsScroll = true;
				//BusEngine.Engine.GameUpdate();
			}
		}
		private void tb_ValueChangedtbVerticale(object sender, System.EventArgs e) {
			verticale = (float)(tbVerticale.Value * System.Math.PI / -180D);
			IsDrawImage = false;
			if (!IsScroll) {
				IsScroll = true;
				//BusEngine.Engine.GameUpdate();
			}
		}

		private static bool IsDrawImage = false;
		private static void Paint(object sender, System.Windows.Forms.PaintEventArgs e) {
			using (new BusEngine.Benchmark("ImageWrapper")) {
				System.Drawing.Graphics g = e.Graphics;

				// нужно найти реализацию обработки текстуры в 3D и изменять положение в пространстве непопиксельно, что ускорит в 250000 раз
				if (!IsDrawImage && IsScroll) {
					IsDrawImage = true;

					// рендерим модель
					using (System.Drawing.ImageWrapper wr = new System.Drawing.ImageWrapper(result)) {
						//System.Drawing.ImageWrapper wr = new System.Drawing.ImageWrapper(result);
						/* System.Numerics.Matrix4x4.CreateBillboard(
							new System.Numerics.Vector3(0, 0, 0),
							new System.Numerics.Vector3(0, 0, 0),
							new System.Numerics.Vector3(0, 0, 0),
							new System.Numerics.Vector3(100, 0, 0)
						); */
						System.Numerics.Matrix4x4 rotate =
						/* System.Numerics.Matrix4x4.CreateTranslation(0, 0, 0) * */
						System.Numerics.Matrix4x4.CreateRotationY(horizontal, new System.Numerics.Vector3(result.Width / 2F, 0, result.Height / 2F)) *
						System.Numerics.Matrix4x4.CreateRotationX(verticale, new System.Numerics.Vector3(result.Width / 2F, 0, result.Height / 2F)) *
						//System.Numerics.Matrix4x4.CreateFromYawPitchRoll(0, -90, 0) *
						//System.Numerics.Matrix4x4.CreateTranslation(result.Width / 2F, 0, result.Height / 2F) *
						//System.Numerics.Matrix4x4.CreateTranslation(result.Width / 2F, 0, result.Height / 2F) *
						System.Numerics.Matrix4x4.CreateTranslation(0, result.Height / 2F, 0);

						foreach (Voxel[] voxel in voxels) {
							/* foreach (Voxel v in voxel) {
								// переводим в мировые координаты
								System.Numerics.Vector3 p = System.Numerics.Vector3.Transform(v.Pos, (System.Numerics.Matrix4x4)rotate);
								int x = (int)p.X;
								int y = (int)p.Y;
								// заносим в изображение
								wr[x, y] = wr[x, y + 1] = v.Light;
							} */
							System.Threading.Tasks.Parallel.For(0, voxel.Length, (int i) => {
								// переводим в мировые координаты
								System.Numerics.Vector3 p = System.Numerics.Vector3.Transform(voxel[i].Pos, rotate);
								int x = (int)p.X;
								int y = (int)p.Y;
								// заносим в изображение
								wr[x, y] = wr[x, y + 1] = voxel[i].Light;
							});
						}
					}

					IsScroll = false;

					if (IsKeys.Contains(System.Windows.Forms.Keys.W) && tbVerticale.Value < tbVerticale.Maximum) {
						tbVerticale.Value = tbVerticale.Value+1;
					}
					if (IsKeys.Contains(System.Windows.Forms.Keys.S) && tbVerticale.Value > tbVerticale.Minimum) {
						tbVerticale.Value = tbVerticale.Value-1;
					}
					if (IsKeys.Contains(System.Windows.Forms.Keys.D) && tbHorizontal.Value+1 < tbHorizontal.Maximum) {
						tbHorizontal.Value = tbHorizontal.Value+2;
					}
					if (IsKeys.Contains(System.Windows.Forms.Keys.A) && tbHorizontal.Value-1 > tbHorizontal.Minimum) {
						tbHorizontal.Value = tbHorizontal.Value-2;
					}
				}

				g.TranslateTransform(result.Width / 2F, result.Height / 2F/* , System.Drawing.Drawing2D.MatrixOrder.Prepend */);
				g.RotateTransform(horizontalCache);
				g.TranslateTransform(result.Width / -2F, result.Height / -2F/* , System.Drawing.Drawing2D.MatrixOrder.Append */);

				// отрисовываем
				g.DrawImage(result, 0, 0);
			}
		}
	}

	internal struct Voxel {
		public System.Numerics.Vector3 Pos;
		public System.Numerics.Vector3 Normal;
		public System.Drawing.Color Light;
	}
	/** API BusEngine.Plugin */
}
/** API BusEngine.Game - пользовательский код */

/* using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging; */

namespace System.Drawing {
	/// <summary>
	/// Обертка над Bitmap для быстрого чтения и изменения пикселов.
	/// Также, класс контролирует выход за пределы изображения: при чтении за границей изображения - возвращает DefaultColor, при записи за границей изображения - игнорирует присвоение.
	/// </summary>
	public class ImageWrapper : System.IDisposable, System.Collections.Generic.IEnumerable<int[]> {
		/// <summary>
		/// Ширина изображения
		/// </summary>
		public int Width { get; private set; }
		/// <summary>
		/// Высота изображения
		/// </summary>
		public int Height { get; private set; }
		/// <summary>
		/// Цвет по-умолачнию (используется при выходе координат за пределы изображения)
		/// </summary>
		public Color DefaultColor { get; set; }

		private static byte[] data; // буфер исходного изображения
		private byte[] outData; // выходной буфер
		private int stride;
		private System.Drawing.Imaging.BitmapData bmpData;
		private System.Drawing.Bitmap BMP;

		/// <summary>
		/// Создание обертки поверх bitmap.
		/// </summary>
		/// <param name="copySourceToOutput">Копирует исходное изображение в выходной буфер</param>
		public ImageWrapper(System.Drawing.Bitmap bmp, bool copySourceToOutput = false) {
			Width = bmp.Width;
			Height = bmp.Height;
			BMP = bmp;

			// кросс-платформ (но в 2 раза медленее) https://habr.com/ru/articles/686578/ https://github.com/StbSharp/StbImageSharp
			bmpData = BMP.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);
			stride = bmpData.Stride;

			if (data == null) {
				data = new byte[stride * Height];
				System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, data, 0, data.Length);
			}

			outData = copySourceToOutput ? (byte[])data.Clone() : new byte[stride * Height];
		}

		/// <summary>
		/// Возвращает пиксел из исходнго изображения.
		/// Либо заносит пиксел в выходной буфер.
		/// </summary>
		public Color this[int x, int y] {
			get {
				int i = GetIndex(x, y);

				return (i < 0 ? DefaultColor : System.Drawing.Color.FromArgb(data[i + 3], data[i + 2], data[i + 1], data[i]));
			} set {
				int i = GetIndex(x, y);

				if (i >= 0) {
					outData[i] = value.B;
					outData[i + 1] = value.G;
					outData[i + 2] = value.R;
					outData[i + 3] = value.A;
				};
			}
		}

		/* public Color GetOutputPixel(int x, int y) {
			int i = GetIndex(x, y);
			return i < 0 ? DefaultColor : System.Drawing.Color.FromArgb(outData[i + 3], outData[i + 2], outData[i + 1], outData[i]);
		} */

		/// <summary>
		/// Возвращает пиксел из исходнго изображения.
		/// Либо заносит пиксел в выходной буфер.
		/// </summary>
		/* public Color this[int[] p] {
			get { return this[p[0], p[1]]; }
			set { this[p[0], p[1]] = value; }
		} */

		/// <summary>
		/// Заносит в выходной буфер значение цвета, заданные в double.
		/// Допускает выход double за пределы 0-255.
		/// </summary>
		/* public void SetPixel(System.Drawing.Point p, double r, double g, double b, double a = 255D) {
			if (r < 0) {
				r = 0;
			} else if (r > 255) {
				r = 255;
			}
			if (g < 0) {
				g = 0;
			} else if (g > 255) {
				g = 255;
			}
			if (b < 0) {
				b = 0;
			} else if (b > 255) {
				b = 255;
			}
			if (a < 0) {
				a = 0;
			} else if (a > 255) {
				a = 255;
			}

			this[p.X, p.Y] = System.Drawing.Color.FromArgb((int)a, (int)r, (int)g, (int)b);
		} */

		/* public void SetPixelUnsafe(int x, int y, int r, int g, int b, int a = 255) {
			var i = (x << 2) + y * stride;
			outData[i] = (byte)b;
			outData[i + 1] = (byte)g;
			outData[i + 2] = (byte)r;
			outData[i + 3] = (byte)a;
		} */

		public int GetIndex(int x, int y) {
			return (x >= 0 && x < Width && y >= 0 && y < Height ? x * 4 + y * stride : -1);
		}

		/// <summary>
		/// Заносит в bitmap выходной буфер и снимает лок.
		/// Этот метод обязателен к исполнению (либо явно, лмбо через using)
		/// </summary>
		public void Dispose() {
			System.Runtime.InteropServices.Marshal.Copy(outData, 0, bmpData.Scan0, outData.Length);
			BMP.UnlockBits(bmpData);
		}

		/// <summary>
		/// Перечисление всех точек изображения
		/// </summary>
		public System.Collections.Generic.IEnumerator<int[]> GetEnumerator() {
			int x, y;
			for (y = 0; y < Height; y++) {
				for (x = 0; x < Width; x++) {
					yield return new int[2] {x, y};
				}
			}
		}

		System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
			return GetEnumerator();
		}

		/// <summary>
		/// Меняет местами входной и выходной буферы
		/// </summary>
		/* public void SwapBuffers() {
			var temp = data;
			data = outData;
			outData = temp;
		} */
	}
}