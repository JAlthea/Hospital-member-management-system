using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetroFramework.Forms;

namespace WindowsFormsApplication1
{
    public partial class LoginForm : MetroForm
    {
        string connectionString;
        OleDbConnection conn;
        string memberType;  //회원종류
        string ID;

        public LoginForm()
        {
            InitializeComponent();
        }

        private bool typeCheck(string memeberType)
        {
            if (memberType == "의사")
            {
                ID = "do";
                return true;
            }
            else if (memberType == "간호사")
            {
                ID = "nu";
                return true;
            }
            else if (memberType == "환자")
            {
                ID = "pa";
                return true;
            }
            else if (memberType == "관리자")
            {
                ID = "";
                return true;
            }
            else
            {
                MessageBox.Show("회원 종류를 선택해주세요.");
                return false;
            }
        }

        //Login Button (Mouse Click)
        private void button1_Click(object sender, EventArgs e)
        {
            //회원종류 체크
            memberType = metroComboBox1.Text;
            if (!typeCheck(memberType))
                return;

            ID += txtboxID.Text;
            //연결 스트링에 대한 정보 : Oracle - MSDAORA
            connectionString += "Provider=MSDAORA;"
                + "Password=" + txtboxPW.Text
                + ";User ID=" + ID + ";";

            try
            {
                conn = new OleDbConnection(connectionString);
                conn.Open();
            }
            catch
            {
                //회원정보 체크
                MessageBox.Show("유효하지 않은 ID나 PW입니다.");
                return;
            }
            conn.Close();

            //LoadingForm 호출
            new LoadingForm(this, connectionString, ID).ShowDialog();
            this.Close();
        }

        //Login Button (Key Press Enter)
        private void LoginForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1_Click(sender, e);
        }

        //Login Button (Key Press Enter)
        private void txtboxPW_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
                button1_Click(sender, e);
        }
    }
}
