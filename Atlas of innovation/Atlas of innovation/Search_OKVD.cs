using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Atlas_of_innovation
{
    public partial class Search_OKVD : UserControl
    {
        public Search_OKVD(string name, string inn)
        {
            InitializeComponent();
            label1.Text = name;
            label2.Text = inn; panel1.Click += label2_Click;

            label1.Font = new Font("Resources/SFUIText-RegularItalic.woff", 12);
            label2.Font = new Font("Resources/SFUIText-RegularItalic.woff", 12);
            label3.Font = new Font("Resources/SFUIText-RegularItalic.woff", 12);
            SetRoundedShape(panel1, 10);
        }

        private static void SetRoundedShape(Control control, int radius)
        {
            System.Drawing.Drawing2D.GraphicsPath path = new System.Drawing.Drawing2D.GraphicsPath();
            path.AddLine(radius, 0, control.Width - radius, 0);
            path.AddArc(control.Width - radius, 0, radius, radius, 270, 90);
            path.AddLine(control.Width, radius, control.Width, control.Height - radius);
            path.AddArc(control.Width - radius, control.Height - radius, radius, radius, 0, 90);
            path.AddLine(control.Width - radius, control.Height, radius, control.Height);
            path.AddArc(0, control.Height - radius, radius, radius, 90, 90);
            path.AddLine(0, control.Height - radius, 0, radius);
            path.AddArc(0, 0, radius, radius, 180, 90);
            control.Region = new Region(path);
        }
        public delegate void CallFormMethod(object obj, string e);
        public event CallFormMethod onButtonClick;

        private void label2_Click(object sender, EventArgs e)
        {
            if (onButtonClick == null)
                return;
            onButtonClick(this, label2.Text);
        }

        public double[] GetMap() {

            double Lat = new Random().Next(10,100);
            double Lng = new Random().Next(10, 100);
            double [] map = { Lat, Lng };
            return map;
        }
    }
}
