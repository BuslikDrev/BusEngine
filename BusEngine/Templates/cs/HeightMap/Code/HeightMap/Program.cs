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

			BusEngine.UI.Canvas.WinForm.DoubleClick += new System.EventHandler(DoubleClick);

			// подключаем событие мыши
			BusEngine.UI.Canvas.WinForm.MouseMove += new System.Windows.Forms.MouseEventHandler(MouseMove);

			// FPS
			System.Timers.Timer fpsTimer = new System.Timers.Timer(1000);
			fpsTimer.Elapsed += OnFPS;
			fpsTimer.AutoReset = true;
			fpsTimer.Enabled = true;

			//BusEngine.Engine.GameStart();

            //источник света
            lamp = System.Numerics.Vector3.Normalize(new System.Numerics.Vector3(-1, 1, -1));

			voxels = new System.Collections.Generic.List<Voxel>();

            //загружаем карту высот
            using (System.Drawing.Bitmap heightMap = (System.Drawing.Bitmap)System.Drawing.Bitmap.FromFile(BusEngine.Engine.DataDirectory + "Textures/heightmap.png")) {
                //создаем обертку для быстрого доступа к пикселам
                using (System.Drawing.ImageWrapper wr = new System.Drawing.ImageWrapper(heightMap)) {
                    //читаем карту высот, формируем воксели
                    foreach (System.Drawing.Point p in wr) {
						if (p.X > 0 && p.Y > 0) {
							//высота
							float height = wr[p].G;

							//высота в соседних точках
							float h1 = wr[p.X - 1, p.Y].G;
							float h2 = wr[p.X, p.Y - 1].G;

							//считаем градиент
							float dx = height - h1;
							float dy = height - h2;

							//считаем нормаль
							System.Numerics.Vector3 n = new System.Numerics.Vector3(dx, NORMAL_Y, dy);
							n = System.Numerics.Vector3.Normalize(n);

							//считаем свет
							int light = (int)(System.Numerics.Vector3.Dot(n, lamp) * 255);

							if (light < 0) {
								light = 0;
							}
							if (light > 255) {
								light = 255;
							}

							//создаем воксель
							voxels.Add(new Voxel{
								Pos = new System.Numerics.Vector3(p.X, height * SCALE_HEIGHT, p.Y),
								Normal = n,
								Light = light
							});
						}
					}
                }

                //создаем результирующее изображение
                result = new System.Drawing.Bitmap(heightMap.Width, heightMap.Height);
            }

            //задаем размер формы
            /* BusEngine.UI.Canvas.WinForm.Size = new System.Drawing.Size(result.Width, 4 * result.Height / 5 + 60);
            BusEngine.UI.Canvas.WinForm.BackColor = System.Drawing.Color.Black; */

            //создаем трекбары
            tbRoll = new System.Windows.Forms.TrackBar{
				Parent = BusEngine.UI.Canvas.WinForm,
				Maximum = 180,
				Minimum = -180,
				Value = 0,
				Left = 10,
				Width = 200 
			};
            tbPitch = new System.Windows.Forms.TrackBar{
				Parent = BusEngine.UI.Canvas.WinForm,
				Maximum = 240,
				Minimum = 180,
				Value = 200,
				Left = 220,
				Width = 200
			};

            tbRoll.ValueChanged += new System.EventHandler(tb_ValueChangedtbRoll);
            tbPitch.ValueChanged += new System.EventHandler(tb_ValueChangedtbPitch);

            tb_ValueChangedtbRoll(null, System.EventArgs.Empty);
			tb_ValueChangedtbPitch(null, System.EventArgs.Empty);
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
            tbRoll.ValueChanged -= new System.EventHandler(tb_ValueChangedtbRoll);
            tbPitch.ValueChanged -= new System.EventHandler(tb_ValueChangedtbPitch);
		}

		// событие мыши
		private static void MouseMove(object sender, System.Windows.Forms.MouseEventArgs e) {
			/* if (BusEngine.Engine.IsGame == false) {
				BusEngine.Engine.GameStart();
			} else {
				BusEngine.Engine.GameStop();
			} */
		}

        private static void DoubleClick(object sender, System.EventArgs e) {
            System.Windows.Forms.SaveFileDialog sfd = new System.Windows.Forms.SaveFileDialog() {
				Title = "Сохранение 3D изображения",
				Filter = "Image|*.png"
			};
            if (sfd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                result.Save(sfd.FileName);
        }

		// событие FPS
		private static void OnFPS(object source, System.Timers.ElapsedEventArgs e) {
			FPSInfo = FPS;
			FPS = 0;
		}







        private static System.Drawing.Bitmap result;
        private static System.Collections.Generic.List<Voxel> voxels;
        private static System.Numerics.Vector3 lamp;//источник света

        private static System.Windows.Forms.TrackBar tbRoll;
        private static System.Windows.Forms.TrackBar tbPitch;

        private static float pitch = 0F;
        private static float roll = 0F;

        private const float SCALE_HEIGHT = 1F / 7F;
        private const float NORMAL_Y = 10F;
		
		

        private void tb_ValueChangedtbRoll(object sender, System.EventArgs e) {
				double x = tbRoll.Value;
				roll = (float)(x * System.Math.PI / 180D);
			//System.Threading.Tasks.Task.Run(() => {
				BusEngine.Engine.GameUpdate();
			//});
        }

        private void tb_ValueChangedtbPitch(object sender, System.EventArgs e) {
				double x = tbPitch.Value;
				pitch = (float)(x * System.Math.PI / 180D);
			//System.Threading.Tasks.Task.Run(() => {
				BusEngine.Engine.GameUpdate();
			//});
        }

		private static void Paint1(object sender, System.Windows.Forms.PaintEventArgs e) {
			System.Drawing.Graphics g = e.Graphics;

            //матрицы вращения
            System.Numerics.Matrix4x4 rotateM0 = System.Numerics.Matrix4x4.CreateRotationY(roll);
            System.Numerics.Matrix4x4 rotateM = System.Numerics.Matrix4x4.CreateFromYawPitchRoll(0F, pitch, 0F);

            //матрица переноса
            System.Numerics.Vector3 position = new System.Numerics.Vector3(result.Width / 2F, 0F, result.Height / 2F);
            System.Numerics.Matrix4x4 translateM = System.Numerics.Matrix4x4.CreateTranslation(position);
            System.Numerics.Matrix4x4 translateM0 = System.Numerics.Matrix4x4.CreateTranslation(-1F * position);

            //матрица смещения относительно экрана
            System.Numerics.Matrix4x4 screenM = System.Numerics.Matrix4x4.CreateTranslation(new System.Numerics.Vector3(0, result.Height / 2f, 0F));

            //рендерим модель
			using (new BusEngine.Benchmark("ImageWrapper")) {
            using (System.Drawing.ImageWrapper wr = new System.Drawing.ImageWrapper(result)) {
				System.Numerics.Matrix4x4 rotate = translateM0 * rotateM0 * rotateM * translateM * screenM;
				foreach (Voxel v in voxels) {
					//BusEngine.Log.Info("FPS Setting: {0}", v);
					//переводим в мировые координаты
					System.Numerics.Vector3 p = System.Numerics.Vector3.Transform(v.Pos, rotate);
					int x = (int)p.X;
					int y = (int)p.Y;
					//цвет
					System.Drawing.Color color = System.Drawing.Color.FromArgb(v.Light, v.Light, v.Light);
					//заносим в изображение
					wr[x, y] = wr[x, (y + 1)] = color;
					
				}
			}
			}

            //отрисовываем
            g.DrawImage(result, new System.Drawing.Point(0, 60));
		}
	}

    struct Voxel {
        public System.Numerics.Vector3 Pos;
        public System.Numerics.Vector3 Normal;
        public int Light;
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
    public class ImageWrapper : System.IDisposable, System.Collections.Generic.IEnumerable<System.Drawing.Point> {
        /// <summary>
        /// Ширина изображения
        /// </summary>
        public static int Width { get; private set; }
        /// <summary>
        /// Высота изображения
        /// </summary>
        public static int Height { get; private set; }
        /// <summary>
        /// Цвет по-умолачнию (используется при выходе координат за пределы изображения)
        /// </summary>
        public Color DefaultColor { get; set; }

        private byte[] data;//буфер исходного изображения
        private byte[] outData;//выходной буфер
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

            bmpData = BMP.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), System.Drawing.Imaging.ImageLockMode.ReadWrite, bmp.PixelFormat);
            stride = bmpData.Stride;

            data = new byte[stride * Height];
            System.Runtime.InteropServices.Marshal.Copy(bmpData.Scan0, data, 0, data.Length);

            outData = copySourceToOutput ? (byte[])data.Clone() : new byte[stride * Height];
        }

        /// <summary>
        /// Возвращает пиксел из исходнго изображения.
        /// Либо заносит пиксел в выходной буфер.
        /// </summary>
        public Color this[int x, int y] {
            get {
                int i = GetIndex(x, y);

                return (i < 0 ? DefaultColor : System.Drawing.Color.FromArgb(data[(i + 3)], data[(i + 2)], data[(i + 1)], data[i]));
            } set {
                int i = GetIndex(x, y);

                if (i >= 0) {
                    outData[i] = value.B;
                    outData[(i + 1)] = value.G;
                    outData[(i + 2)] = value.R;
                    outData[(i + 3)] = value.A;
                };
            }
        }

        public Color GetOutputPixel(int x, int y) {
            int i = GetIndex(x, y);
            return i < 0 ? DefaultColor : System.Drawing.Color.FromArgb(outData[i + 3], outData[i + 2], outData[i + 1], outData[i]);
        }

        /// <summary>
        /// Возвращает пиксел из исходнго изображения.
        /// Либо заносит пиксел в выходной буфер.
        /// </summary>
        public Color this[System.Drawing.Point p] {
            get { return this[p.X, p.Y]; }
            set { this[p.X, p.Y] = value; }
        }

        /// <summary>
        /// Заносит в выходной буфер значение цвета, заданные в double.
        /// Допускает выход double за пределы 0-255.
        /// </summary>
        public void SetPixel(System.Drawing.Point p, double r, double g, double b, double a = 255) {
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
        }

        /* public void SetPixelUnsafe(int x, int y, int r, int g, int b, int a = 255) {
            var i = (x << 2) + y * stride;
            outData[i] = (byte)b;
            outData[i + 1] = (byte)g;
            outData[i + 2] = (byte)r;
            outData[i + 3] = (byte)a;
        } */

        public int GetIndex(int x, int y) {
            return (x < 0 || x >= Width || y < 0 || y >= Height ? -1 : x * 4 + y * stride);
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
        public System.Collections.Generic.IEnumerator<System.Drawing.Point> GetEnumerator() {
			int x, y;

            for (y = 0; y < Height; ++y) {
                for (x = 0; x < Width; ++x) {
                    yield return new System.Drawing.Point(x, y);
				}
			}
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() {
            return GetEnumerator();
        }

        /// <summary>
        /// Меняет местами входной и выходной буферы
        /// </summary>
        public void SwapBuffers() {
            var temp = data;
            data = outData;
            outData = temp;
        }
    }
}