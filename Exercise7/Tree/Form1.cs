using System;
using System.Drawing;
using System.Windows.Forms;

namespace Tree
{
    public partial class Form1 : Form
    {
        private Graphics _graphics;
        private Color _penColor = Color.Black;
        private double _per1 = 0.6;
        private double _per2 = 0.7;
        private double _th1 = 30 * Math.PI / 180;
        private double _th2 = 20 * Math.PI / 180;

        public Form1()
        {
            InitializeComponent();
        }

        private void DrawCayleyTree(int n, double x0, double y0, double leng, double th)
        {
            if (n == 0) return;

            var x1 = x0 + leng * Math.Cos(th);
            var y1 = y0 + leng * Math.Sin(th);

            DrawLine(x0, y0, x1, y1);

            DrawCayleyTree(n - 1, x1, y1, _per1 * leng, th + _th1);
            DrawCayleyTree(n - 1, x1, y1, _per2 * leng, th - _th2);
        }

        private void DrawLine(double x0, double y0, double x1, double y1)
        {
            _graphics.DrawLine(new Pen(_penColor), (int) x0, (int) y0, (int) x1, (int) y1);
        }

        private void DrawButton_Click(object sender, EventArgs e)
        {
            _per1 = (double) Per1.Value;
            _per2 = (double) Per2.Value;
            _th1 = (double) Th1.Value * Math.PI / 180;
            _th2 = (double) Th2.Value * Math.PI / 180;

            _graphics ??= panel1.CreateGraphics();
            _graphics.Clear(Color.White);
            DrawCayleyTree((int) Depth.Value,
                (panel1.Right - panel1.Left) * 0.5, (panel1.Bottom - panel1.Top) * 0.9,
                (int) Length.Value, -Math.PI / 2);
        }

        private void Pen_Click(object sender, EventArgs e)
        {
            var colorDia = new ColorDialog();
            if (colorDia.ShowDialog() != DialogResult.OK) return;

            _penColor = colorDia.Color;
            Pen.ForeColor = _penColor;
        }
    }
}