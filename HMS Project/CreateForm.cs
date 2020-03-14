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
    public partial class CreateForm : MetroForm
    {
        OleDbConnection conn;   //데이터베이스에 연결
        string connectionString;//연결스트링에 대한 정보

        public CreateForm(Form preform, string connectionString)
        {
            InitializeComponent();
            this.connectionString = connectionString;
        }

        private void matchingString()
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
            OleDbCommand cmd = new OleDbCommand();

            if (cbType.SelectedItem.ToString() == "의사")
            {
                //계정 생성
                cmd.CommandText = "create user do" + tbID.Text + " identified by " + tbPW.Text;
                cmd.CommandType = CommandType.Text; 
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 계정 생성 오류");
                    return;
                }

                //뷰 생성1
                cmd.CommandText = "create view 개인정보_do" + tbID.Text + " as select 의사번호, 이름, 진료과목 from 의사 where 의사번호 = " + tbID.Text + " with read only";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 뷰(1) 생성 오류");
                }

                //뷰 생성2
                cmd.CommandText = "create view 진료내용_do" + tbID.Text + " as select 진료번호, 환자번호, 의사번호, 날짜, 진료내용 from 진료 where 의사번호 = " + tbID.Text + " with check option";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 뷰(2) 생성 오류");
                }

                //뷰 생성3
                cmd.CommandText = "create view 의사소견_do" + tbID.Text + " as select 차트번호, 진료번호, 간호사번호, 의사소견 from 차트 where 진료번호 in (select 진료번호 from 진료 where 의사번호 = " + tbID.Text + ") with check option";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 뷰(3) 생성 오류");
                }

                //권한 부여1
                cmd.CommandText = "grant connect to do" + tbID.Text;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 권한(1) 부여 오류");
                }

                //권한 부여2
                cmd.CommandText = "grant select on 개인정보_do" + tbID.Text + " to do" + tbID.Text;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 권한(2) 부여 오류");
                }

                //권한 부여3
                cmd.CommandText = "grant select, insert, delete, update on 진료내용_do" + tbID.Text + " to do" + tbID.Text;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 권한(3) 부여 오류");
                }

                //권한 부여4
                cmd.CommandText = "grant select, insert, delete, update on 의사소견_do" + tbID.Text + " to do" + tbID.Text;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 권한(4) 부여 오류");
                }
            }
            else if (cbType.SelectedItem.ToString() == "간호사")
            {
                //계정 생성
                cmd.CommandText = "create user nu" + tbID.Text + " identified by " + tbPW.Text;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 계정 생성 오류");
                    return;
                }

                //뷰 생성1
                cmd.CommandText = "create view 개인정보_nu" + tbID.Text + " as select 간호사번호, 이름, 담당업무 from 간호사 where 간호사번호 = " + tbID.Text + " with read only";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 뷰(1) 생성 오류");
                }

                //뷰 생성2
                cmd.CommandText = "create view 담당환자_nu" + tbID.Text + " as select 환자번호, 이름, 의사번호, 간호사번호 from 환자 where 간호사번호 = " + tbID.Text + " with read only";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 뷰(2) 생성 오류");
                }

                //뷰 생성3
                cmd.CommandText = "create view 진료등록_nu" + tbID.Text + " as select 진료번호, 환자번호, 의사번호, 날짜 from 진료";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 뷰(3) 생성 오류");
                }

                //뷰 생성4
                cmd.CommandText = "create view 차트등록_nu" + tbID.Text + " as select 차트번호, 진료번호, 간호사번호 from 차트 where 간호사번호 = " + tbID.Text + " with check option";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 뷰(4) 생성 오류");
                }

                //권한 부여1
                cmd.CommandText = "grant connect to nu" + tbID.Text;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 권한(1) 부여 오류");
                }

                //권한 부여2
                cmd.CommandText = "grant select on 개인정보_nu" + tbID.Text + " to nu" + tbID.Text;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 권한(2) 부여 오류");
                }

                //권한 부여3
                cmd.CommandText = "grant select, insert, delete, update on 진료등록_nu" + tbID.Text + " to nu" + tbID.Text;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 권한(3) 부여 오류");
                }

                //권한 부여4
                cmd.CommandText = "grant select, insert, delete, update on 차트등록_nu" + tbID.Text + " to nu" + tbID.Text;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 권한(4) 부여 오류");
                }

                //권한 부여5
                cmd.CommandText = "grant select on 담당환자_nu" + tbID.Text + " to nu" + tbID.Text;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 권한(5) 부여 오류");
                }
            }
            else if (cbType.SelectedItem.ToString() == "환자")
            {
                //계정 생성
                cmd.CommandText = "create user pa" + tbID.Text + " identified by " + tbPW.Text;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 계정 생성 오류");
                    return;
                }

                //뷰 생성1
                cmd.CommandText = "create view 개인정보_pa" + tbID.Text + " as select 환자번호, 이름, 의사번호, 간호사번호 from 환자 where 환자번호 = " + tbID.Text + " with read only";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 뷰(1) 생성 오류");
                }

                //뷰 생성2
                cmd.CommandText = "create view 개인차트_pa" + tbID.Text + " as select 차트번호, 진료번호, 간호사번호, 의사소견 from 차트 where 진료번호 in (select 진료번호 from 진료 where 환자번호 = " + tbID.Text + ") with read only";
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 뷰(1) 생성 오류");
                }

                //권한 부여1
                cmd.CommandText = "grant connect to pa" + tbID.Text;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 권한(1) 부여 오류");
                }

                //권한 부여2
                cmd.CommandText = "grant select on 개인정보_pa" + tbID.Text + " to pa" + tbID.Text;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 권한(2) 부여 오류");
                }

                //권한 부여3
                cmd.CommandText = "grant select on 개인차트_pa" + tbID.Text + " to pa" + tbID.Text;
                cmd.CommandType = CommandType.Text;
                cmd.Connection = conn;

                try
                {
                    cmd.ExecuteNonQuery();
                }
                catch
                {
                    MessageBox.Show("Error : 권한(3) 부여 오류");
                }
            }
        }

        //생성하기
        private void btnCreate_Click(object sender, EventArgs e)
        {
            matchingString();
            MessageBox.Show("생성을 완료했습니다.");
        }

        //취소하기
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
