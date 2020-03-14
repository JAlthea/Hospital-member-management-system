using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Threading;
using System.Collections;
using System.Data.OleDb;

namespace WindowsFormsApplication1
{
    public partial class LoadingForm : MetroForm
    {
        string connectionString;
        OleDbConnection conn;
        string ID;
        ArrayList userTables = new ArrayList(); //해당 유저의 테이블과 뷰를 저장한다.

        public LoadingForm(Form preForm, string connectionString, string ID)
        {
            InitializeComponent();
            metroProgressSpinner1.Value = 0;
            this.connectionString = connectionString;
            this.ID = ID.ToUpper();
            preForm.Visible = false;

            timer1.Start();
            Thread t1 = new Thread(new ThreadStart(Run));
            t1.Start();
        }

        //해당 사용자의 모든 테이블과 뷰를 불러온다.
        private void tableSelecting()
        {
            try
            {
                conn = new OleDbConnection(connectionString);
                conn.Open();
            }
            catch
            {
                MessageBox.Show("Error : DB Connecting");
            }

            //해당 ID에 맞는 Table, View 를 가져온다.
            DataTable dt = conn.GetSchema("Tables");
            foreach (DataRow dataRow in dt.Rows)
            {
                if (dataRow[1].Equals("MANAGER"))
                {
                    //관리자가 아닐 때 
                    if (!ID.Contains("MANAGER") && dataRow["TABLE_NAME"].ToString().Trim().Contains(ID))
                    {
                        userTables.Add(dataRow["TABLE_NAME"].ToString().Trim());
                    }
                    else
                        userTables.Add(dataRow["TABLE_NAME"].ToString().Trim());
                }

            }
            conn.Close();

            /*
            //해당 ID에 맞는 Table만 가져온다.
            DataTable tables = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, new object[] { null, null, null, "TABLE" });
            foreach (DataRow row in tables.Rows)
            {
               if (row[1].Equals(ID))
                    cb1.Items.Add(row[2]);
            }
            */

            //MainForm 호출
            new MainForm(this, userTables, ID, connectionString).ShowDialog();
            this.Close();
        }

        private void Run()
        {
            tableSelecting();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            try
            {
                if (metroProgressSpinner1.Value >= 90)
                    metroProgressSpinner1.Value = 0;
                else
                    metroProgressSpinner1.Value += 10;
            }
            catch
            {

            }
        }
    }
}
