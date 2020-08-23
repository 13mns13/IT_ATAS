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
    public partial class ItemsViewSearch : UserControl
    {
        public ItemsViewSearch(string name, string ceo_type, string ceo_name, string address, string inn, string ogrn, string reg_date, string authorized_capital, string okved_descr, string link)
        {
            InitializeComponent();
            if (name == null) name = "";
            if (ceo_type == null) ceo_type = "";
            if (ceo_name == null) ceo_name = "";
            if (address == null) address = "";
            if (inn == null) inn = "";
            if (ogrn == null) ogrn = "";
            if (reg_date == null) reg_date = "";
            if (authorized_capital == null) authorized_capital = "";
            if (okved_descr == null) okved_descr = "";

            this.name.Text = name.ToUpper(); ;
            this.ceo_type.Text = ceo_type.ToUpper(); ;
            this.ceo_name.Text = ceo_name.ToUpper(); ;
            this.address.Text = address.ToUpper(); ;
            this.inn.Text = inn.ToUpper(); ;
            this.ogrn.Text = ogrn.ToUpper(); ;
            this.reg_date.Text = reg_date.ToUpper(); ;
            this.authorized_capital.Text = authorized_capital.ToUpper(); ;
            this.okved_descr.Text = okved_descr.ToUpper(); ;
            SetRoundedShape(panel1, 10);
            
            panel1.Click+= (a, b) => { onButtonClick("", inn); };
            foreach (Control x in panel1.Controls)
            {
                x.Click += (a, b) => { onButtonClick("", inn); };

                foreach (Control y in x.Controls)
                {
                    y.Click += (a, b) => { onButtonClick("", inn); };
                    foreach (Control z in y.Controls)   
                    {

                        z.Click += (a, b) => { onButtonClick("", inn); };
                        foreach (Control t in z.Controls)
                        {
                            t.Click += (a, b) => { onButtonClick("", inn); };
                            t.Font = new Font("Resources/SFUIText-RegularItalic.woff", 12);
                        }
                        z.Font = new Font("Resources/SFUIText-RegularItalic.woff", 12);
                    }
                    y.Font = new Font("Resources/SFUIText-RegularItalic.woff", 12);
                }
                x.Font = new Font("Resources/SFUIText-RegularItalic.woff", 12);
            }
            foreach (Control x in Controls)
                if (x is Label)
                    x.Text = x.Text.Replace("\n","");

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

        private void ItemsViewSearch_SizeChanged(object sender, EventArgs e)
        {
            panel1.Width = this.Width-50;
            SetRoundedShape(panel1, 10);
        }
    }
}
