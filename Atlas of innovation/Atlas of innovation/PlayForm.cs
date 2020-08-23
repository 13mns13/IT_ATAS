using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using Newtonsoft.Json.Linq;
using System.Drawing.Drawing2D;

namespace Atlas_of_innovation
{
    public partial class PlayForm : UserControl
    {
        public PlayForm()
        {
            InitializeComponent();

            SetRoundedShape(panel3, 10);
            SetRoundedShape(Enter, 10);
            textBox1.Click += new EventHandler(this.TextGotFocus);
            textBox1.LostFocus += new EventHandler(this.TextLostFocus);
            ScrollBar vScrollBar1 = new VScrollBar();
            Items.Controls.Add(vScrollBar1);
            SetRoundedShape(vScrollBar1,10);
            SetRoundedShape(panel5, 10);
            SetRoundedShape(panel6, 10);
            label1.Font = new Font("Resources/SFUIText-RegularItalic.woff",12);
            textBox1.Font = new Font("Resources/SFUIText-RegularItalic.woff", 12);
            label2.Font = new Font("Resources/SFUIText-RegularItalic.woff", 12);
            label3.Font = new Font("Resources/SFUIText-RegularItalic.woff", 12);
            label4.Font = new Font("Resources/SFUIText-RegularItalic.woff", 20);
            label5.Font = new Font("Resources/SFUIText-RegularItalic.woff", 30, FontStyle.Bold, GraphicsUnit.Point, ((byte)(0)));

        }


        public void TextGotFocus(object sender, EventArgs e)
        { 
            TextBox tb = (TextBox)sender;
            if (tb.Text == "Искать по назаванию, адресу, руководителю, учереждителям, ОГРН, ИНН")
            {
                tb.Text = "";
                tb.ForeColor = Color.Black;
                Items.Visible = false;
                
            }

        }
        public void TextLostFocus(object sender, EventArgs e)
        {
            TextBox tb = (TextBox)sender;
            if (tb.Text == "")
            {
                tb.Text = "Искать по назаванию, адресу, руководителю, учереждителям, ОГРН, ИНН";
                tb.ForeColor = Color.Gray;
                Items.Visible = false;
            }
        }
        public delegate void CallFormMethod(object obj, string e);
        public event CallFormMethod onButtonClick;

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


        private  void Play_SizeChanged(object sender, EventArgs e)
        {
            SetRoundedShape(panel3, 10);
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (!textBox1.Text.Contains("\n"))
                return;
            textBox1.Text = textBox1.Text.Replace("\n", "");
            AddFindItems();
        }

        private void AddFindItems()
        {
            Items.Controls.Clear();
            flow.Controls.Clear();
            Items.Visible = false;
            var items = new ParseSite(textBox1.Text).GetSearch();
            if (!items.Value<bool>("success"))
            {
                MessageBox.Show("Появилась капча!","Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
                
                return;
            }
            if (items.Value<int>("ul_count") == 0 && items.Value<int>("ip_count") == 0)
            {
                MessageBox.Show("Нет информации о данном предприятии", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            int k = 0;
            string inn = "";
            Items.Visible = true;
            foreach (var key in items) if (key.Key.ToString() == "ul" | key.Key.ToString() == "ip") 
            foreach (var item in items[key.Key.ToString()]) {

                        inn = item.Value<string>("inn").Replace("!", "").Replace("~", "");//"!~~6163133311~~!"
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
                    mainview.onButtonClick += (a, b) => {
                        if (this.onButtonClick == null)
                            return;
                        onButtonClick(this,b) ; };
                        mainview.Width = Items.Width - 50;
                        this.SizeChanged += (a, b) => {  mainview.Width = Items.Width-50 ; };
                Items.Controls.Add(mainview);
                k += 1;
            }
            

        }


        private void Enter_Click(object sender, EventArgs e)
        {
            AddFindItems();
        }

        private void panel1_Click(object sender, EventArgs e)
        {
            Items.Visible = false;
            TextLostFocus(textBox1, new EventArgs());
        }
        FlowLayoutPanel flow = new FlowLayoutPanel();
        private void panel5_Click(object sender, EventArgs e)
        {
            panel7.Controls.Clear();
            Items.Controls.Clear();
            Items.Visible = false;
            
            string[] names = { "Нейронет", "Технет", "Сэйфнет" };
            FlowLayoutPanel flowLayout = new FlowLayoutPanel();
            flowLayout.Dock = DockStyle.Fill;
            flow = new FlowLayoutPanel();
            flow.Click += panel1_Click;
            flowLayout.Click += panel1_Click;
            var map = new MapPanel();
            map.onButtonClick += (p, d) => {
                textBox1.Text = new OKVD_CODE().inn.Replace(",","").Split(' ')[int.Parse(d)]; AddFindItems(); flow.Controls.Clear();
            };
            foreach (var x in names)
            {
                var @okvd = new okvd(x);
                @okvd.onButtonClick += (a, b) => {
                    var  data = new OKVD_CODE().Get(b);
                    flow.Controls.Clear();
                    
                    foreach (var t in data)
                    {
                        var Search_OKVD = new Search_OKVD(t.Value["name"].ToString(), t.Key);
                        flow.Controls.Add(Search_OKVD);
                        flow.Size = new Size(flowLayout.Width-50, flowLayout.Height / 2);
                        flow.AutoScrollMinSize = new Size(flowLayout.Width-300, 0);
                        flow.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                        Search_OKVD.onButtonClick += (p, f) => {  textBox1.Text = f; AddFindItems(); flow.Controls.Clear(); };
                        var index = int.Parse(t.Value["index"].ToString());
                        var map_ = t.Value["map"].ToString().Replace(", "," ").Replace(".", ",").Split(' ');

                        var xx = double.Parse(map_[0]);
                        var yy = double.Parse(map_[1]);
                        map.AddMarker(index, xx,yy);

                    }
                    flowLayout.Controls.Add(flow);

                };

                
                flowLayout.Controls.Add(@okvd);


            }
            
            flowLayout.Controls.Add(map);
            flowLayout.AutoScrollMinSize = new Size(flowLayout.Width, 300);
            panel7.Controls.Add(flowLayout);
            flowLayout.Height = 1000;


        }

        private void panel6_Click(object sender, EventArgs e)
        {
            onButtonClick(this,"close");
        }
    }
}
