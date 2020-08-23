using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json.Linq;
using System.Windows.Forms.DataVisualization.Charting;
using GMap.NET;

namespace Atlas_of_innovation
{
    public partial class InputParams : UserControl
    {
        double  authorized_capital;
        string date;
        public InputParams(string inn, string dolg, string patent, double authorized_capital, string date)
        {
            InitializeComponent();
            chart1.ChartAreas[0].AxisX.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisY.MajorGrid.Enabled = false;
            chart1.ChartAreas[0].AxisX.Enabled = AxisEnabled.False;
            this.dolg.Text = dolg;
            this.date = date;
            this.authorized_capital = authorized_capital;
            this.patent.Text = patent;
            var checkINN = new CheckINN(inn);
            var items = checkINN.Check();
            label4.Text = items["map"].ToString();
            label6.Text = items["site"].ToString();
            label8.Text = (items.Value<double>("pro") * 100).ToString() + "%";
            SetRoundedShape(panel1, 10);

            label9.Text = label9.Text.ToUpper();
            label9.Font = new Font("Resources/SFUIText-RegularItalic.woff", 12);
            foreach (Control x in Controls)
            {
                foreach (Control y in x.Controls)
                    foreach (Control z in y.Controls)
                    {
                        z.Text = z.Text.ToUpper();
                        z.Font = new Font("Resources/SFUIText-RegularItalic.woff", 12);
                    }

                this.SizeChanged += (a, b) => { panel1.Width = Width; };
                MapClass mapClass = new MapClass(gMapControl1);
                mapClass.LoadMap();
                gMapControl1.MinZoom = 5;
                gMapControl1.MaxZoom = 15;
                double Lat = double.Parse(label4.Text.Replace(" ", "").Split(',')[0].Replace(".", ","));
                double Lng = double.Parse(label4.Text.Replace(" ", "").Split(',')[1].Replace(".", ","));
                mapClass.Add_Marker(Lat, Lng);
                gMapControl1.Position = new PointLatLng(Lat, Lng);


            }
            Calculete(items);
        }

        private void Calculete(JObject items )
        {
            double patent = double.Parse(this.patent.Text);
            double nalog = double.Parse(dolg.Text) / authorized_capital;
            string reg = date;
            double yandex = items.Value<double>("yandex");
            double pro = items.Value<double>("pro");
            double platform = (items.Value<double>("count_otv") / yandex)*10;

            double _date = DateTime.Now.Year-  DateTime.Parse(date).Year;

            if (patent == 0)
                patent = 0;
            else if (patent == 1)
                patent = 0.2;
            else if (patent == 2)
                patent = 0.4;
            else if (patent == 3)
                patent = 0.6;
            else if (patent == 4)
                patent = 0.8;
            else patent = 1;

            if (_date == 0)
                _date = 0;
            else if (_date == 2)
                _date = 0.2;
            else if (_date == 4)
                _date = 0.4;
            else if (_date == 6)
                _date = 0.6;
            else if (_date == 8)
                _date = 0.8;
            else _date = 1;

            JObject data = new JObject();
            data["patent"] = patent;
            data["reg"] = _date;
            data["platform"] = platform;
            data["nalog"] = 1-nalog;
            data["events"] = pro;
            API calculete = new API();
            var response = calculete.Method("predict", data);
            double result = response["response"][0].Value<double>("result");
            chart1.Series[0].Points.AddY(result);
            
            label9.Text += result+"%";
            chart1.Legends[0].Enabled = false;

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
    }
}
