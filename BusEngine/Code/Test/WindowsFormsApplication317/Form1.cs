using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Windows.Forms;

namespace WindowsFormsApplication317
{
    public partial class Form1 : Form
    {
        Bitmap result;//результирующее 3d изображение
        List<Voxel> voxels = new List<Voxel>();//список вокселей
        Vector3 lamp;//источник света

        TrackBar tbRoll;
        TrackBar tbPitch;

        float pitch = 0;
        float roll = 0;

        private const float SCALE_HEIGHT = 1 / 7f;
        private const float NORMAL_Y = 10;

        public Form1()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);

            //источник света
            lamp = Vector3.Normalize(new Vector3(-1, 1, -1));

            //загружаем карту высот
            using (var heightMap = (Bitmap) Bitmap.FromFile(AppDomain.CurrentDomain.BaseDirectory + "heightmap.png"))
            {
                //создаем обертку для быстрого доступа к пикселам
                using (var wr = new ImageWrapper(heightMap))
                {
                    //читаем карту высот, формируем воксели
                    foreach (var p in wr)
                    if(p.X > 0 && p.Y > 0)
                    {
                        //высота
                        var height = wr[p].G;
                        //высота в соседних точках
                        var h1 = wr[p.X - 1, p.Y].G;
                        var h2 = wr[p.X, p.Y - 1].G;
                        //считаем градиент
                        var dx = height - h1;
                        var dy = height - h2;
                        //считаем нормаль
                        var n = new Vector3(dx, NORMAL_Y, dy);
                        n = Vector3.Normalize(n);
                        //считаем свет
                        var light = (int)(Vector3.Dot(n, lamp) * 255);
                        if (light < 0) light = 0;
                        if (light > 255) light = 255;
                        //создаем воксель
                        var voxel = new Voxel {Pos = new Vector3(p.X, height * SCALE_HEIGHT, p.Y), Normal = n, Light = light};
                        voxels.Add(voxel);
                    }
                }

                //создаем результирующее изображение
                result = new Bitmap(heightMap.Width, heightMap.Height);
            }

            //задаем размер формы
            Size = new Size(result.Width, 4 * result.Height / 5 + 60);
            BackColor = Color.Black;

            //создаем трекбары
            tbRoll = new TrackBar { Parent = this, Maximum = 180, Left = 10, Value = 0, Minimum = -180, Width = 200 };
            tbPitch = new TrackBar { Parent = this, Maximum = 240, Left = 220, Value = 200, Minimum = 180, Width = 200};

            tbRoll.ValueChanged += tb_ValueChanged;
            tbPitch.ValueChanged += tb_ValueChanged;

            tb_ValueChanged(null, EventArgs.Empty);
        }

        void tb_ValueChanged(object sender, EventArgs e)
        {
            pitch = (float)(tbPitch.Value * Math.PI / 180);
            roll = (float)(tbRoll.Value * Math.PI / 180);

            Invalidate();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            //матрицы вращения
            var rotateM0 = Matrix4x4.CreateRotationY(roll);
            var rotateM = Matrix4x4.CreateFromYawPitchRoll(0, pitch, 0);

            //матрица переноса
            var position = new Vector3(result.Width / 2f, 0, result.Height / 2f);
            var translateM = Matrix4x4.CreateTranslation(position);
            var translateM0 = Matrix4x4.CreateTranslation(-1 * position);

            //матрица смещения относительно экрана
            var screenM = Matrix4x4.CreateTranslation(new Vector3(0, result.Height / 2f, 0));

            //результирующая матрица
            var m = translateM0 * rotateM0 * rotateM * translateM * screenM;
            
            //рендерим модель
            Render(m);

            //отрисовываем
            e.Graphics.DrawImage(result, new PointF(0, 60));
        }

        private void Render(Matrix4x4 worldMatrix)
        {
            using (var wr = new ImageWrapper(result))
            foreach (var v in voxels)
            {
                //переводим в мировые координаты
                var p = Vector3.Transform(v.Pos, worldMatrix);
                var intX = (int) p.X;
                var intY = (int) p.Y;
                //цвет
                var color = Color.FromArgb(v.Light, v.Light, v.Light);
                //заносим в изображение
                wr[intX, intY + 1] = wr[intX, intY] = color;
            }
        }

        protected override void OnDoubleClick(EventArgs e)
        {
            var sfd = new SaveFileDialog() {Title = "Сохранение 3D изображения", Filter = "Image|*.png"};
            if (sfd.ShowDialog() == DialogResult.OK)
                result.Save(sfd.FileName);
        }
    }

    struct Voxel
    {
        public Vector3 Pos;
        public Vector3 Normal;
        public int Light;
    }
}
