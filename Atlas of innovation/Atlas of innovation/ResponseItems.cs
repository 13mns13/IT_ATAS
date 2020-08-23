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
    public partial class ResponseItems : UserControl
    {
        private long inn;
        private int patent;
        private int date;
        
        public ResponseItems(long inn)
        {
            InitializeComponent();
            this.inn = inn;
            Start();
            SetRoundedShape(panel3,10);
            label1.Font = new Font("Resources/SFUIText-RegularItalic.woff", 12);

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

        private void Start()
        {
            var items = new ParseSite(this.inn).GetSearch();
            if (!items.Value<bool>("success"))
            {
                MessageBox.Show("Ошибка сервера!");
                return;
            }
            if (items.Value<int>("ul_count") == 0 && items.Value<int>("ip_count") == 0)
            {
                MessageBox.Show("Никого не нашел");
                return;
            }
            int k = 0;
            string name = "";
            string date = "";
            string  link = "";
            double authorized_capital = 1;
            string inn = "";

            foreach (var key in items) if (key.Key.ToString() == "ul" | key.Key.ToString() == "ip")
                    foreach (var item in items[key.Key.ToString()])
                    {

                        inn = item.Value<string>("inn").Replace("!","").Replace("~","");
                        ItemsViewSearch mainview = new ItemsViewSearch(item.Value<string>("raw_name"),
                            item.Value<string>("ceo_type"),
                            item.Value<string>("snippet_string"),
                            item.Value<string>("address"),
                            inn,
                            item.Value<string>("ogrn"),
                            item.Value<string>("reg_date"),
                            item.Value<string>("authorized_capital"),
                            item.Value<string>("okved_descr"),
                             item.Value<string>("link")

                            );
                        try
                        {
                            authorized_capital = item.Value<double>("authorized_capital");
                        }
                        catch { authorized_capital = 1; }
                        name = item.Value<string>("raw_name");
                        date = item.Value<string>("reg_date");
                        link = item.Value<string>("link");
                        mainview.onButtonClick += (a, b) => {
                            if (this.onButtonClick == null)
                                return;
                            onButtonClick(this, b);
                        };
                        mainview.Width= panel2.Width+500;
                       
                        panel2.Controls.Add(mainview);
                        k += 1;
                    }

            this.date = (DateTime.Now - DateTime.Parse(date)).Days / 365;

            patent = int.Parse(new ParseSite(inn).GetPatent(name));
            var input = new InputParams(inn, new ParseSite(inn).GetRusprofile(link)["Dolg"].ToString(), patent.ToString(), authorized_capital, date);
            panel1.Controls.Add(input);
            input.Width = panel2.Width + 500;
            //MessageBox.Show((new ParseSite(inn).GetRusprofile(link).ToString()));

        }

        private void label1_Click(object sender, EventArgs e)
        {
            onButtonClick(this,"play");
        }
    }
}
