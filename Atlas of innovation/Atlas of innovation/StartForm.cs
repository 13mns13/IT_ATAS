using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Net;
using Newtonsoft.Json.Linq;
using System.Threading;

namespace Atlas_of_innovation
{
    public partial class StartForm : Form
    {


        public StartForm()
        {
            InitializeComponent();
            
            LoadForm("", "play");

        }

        private PlayForm playForm;
        private ResponseItems responseItems;

        private string type = "start";
        private  void StartForm_SizeChanged(object sender, EventArgs e)
        {
            if(type == "play")
            {
                playForm.Width = ActiveForm.Width;
                playForm.Height = ActiveForm.Height;
                
            }
            else if (type == "response")
            {
                responseItems.Width = ActiveForm.Width;
                responseItems.Height = ActiveForm.Height;

            }
        }
        
        public void LoadForm(object obj, string e)
        {
            if (e == "play")
            {
                type = e;
                playForm = new PlayForm();
                playForm.Width = ActiveForm.Width;
                playForm.Height = ActiveForm.Height;
                
                ActiveForm.Controls.Clear();
                ActiveForm.Controls.Add(playForm);
                playForm.onButtonClick += LoadForm;
            }

            if (e == "close")
            {
                this.Close();
            }

            else if (long.TryParse(e,out long inn)){
                type = "response";
                responseItems = new ResponseItems(inn);
                responseItems.Width = ActiveForm.Width;
                responseItems.Height = ActiveForm.Height;

                ActiveForm.Controls.Clear();
                ActiveForm.Controls.Add(responseItems);
                responseItems.onButtonClick += LoadForm;

            }

            
        }
    }
}
