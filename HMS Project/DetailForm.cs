using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace WindowsFormsApplication1
{
    public partial class DetailForm : MetroForm
    {
        string lblContext;
        string txtContext;
        int MaxLength;
        TextBox preTextBox;

        public DetailForm(TextBox preTextBox, string lblContext, string txtContext, int MaxLength)
        {
            InitializeComponent();
            this.preTextBox = preTextBox;
            this.lblContext = lblContext;
            this.txtContext = txtContext;
            this.MaxLength = MaxLength;

            this.Text = lblContext;
            metroTextBox1.MaxLength = MaxLength;
            metroTextBox1.Text += txtContext;
        }

        //저장하기
        private void metroButton1_Click(object sender, EventArgs e)
        {
            preTextBox.Text = metroTextBox1.Text;
            this.Close();
        }

        //취소하기
        private void metroButton2_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
