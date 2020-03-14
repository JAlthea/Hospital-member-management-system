using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MetroFramework.Forms;
using System.Data.OleDb;
using System.Collections;
using System.Text.RegularExpressions;

namespace WindowsFormsApplication1
{
    public partial class MainForm : MetroForm
    {
        OleDbConnection conn;   //데이터베이스에 연결
        string connectionString;//연결스트링에 대한 정보
        string ID;              //현재 사용자의 ID

        string tableName;       //현재 테이블의 이름
        bool tableStart;        //관련 테이블을 처음 가져왔는지 판단한다.
        TextBox[] textBoxes;    //테이블마다 생성된 열의 개수만큼 텍스트박스를 담는다.
        Label[] labels;         //테이블마다 생성된 열의 개수만큼 속성 이름을 담는다.
        Button[] buttons;       //테이블마다 생성된 세부버튼을 담는다.
        ArrayList dataTypes = new ArrayList();    //현재 테이블 속성의 데이터타입들을 담는다.
        ArrayList PrimaryKeys = new ArrayList();  //현재 테이블의 기본키를 담는다.
        ArrayList[] tableInfo;  //현재 테이블의 속성에 관한 모든 정보를 담는다.
        bool SQLStart;

        public MainForm(Form preForm, ArrayList userTables, string ID, string connectionString)
        {
            InitializeComponent();
            this.ID = ID;
            this.connectionString = connectionString;
            preForm.Visible = false;
            btnCreate.Visible = false;

            //사용자의 유형 확인
            if (ID.StartsWith("MANAGER"))
                this.Text = "System manager";
            else if (ID.StartsWith("DO"))
                this.Text = "Doctor";
            else if (ID.StartsWith("NU"))
                this.Text = "Nurse";
            else if (ID.StartsWith("PA"))
                this.Text = "Patient";

            //사용자의 유형에 맞는 폼 불러오기
            if (this.Text == "System manager")
            {
                lblSQL.Visible = true;
                txtSQL.Visible = true;
                btnCreate.Visible = true;
            }
            else if (this.Text == "Doctor")
            {
                lblSQL.Visible = false;
                txtSQL.Visible = false;
                btnSQL.Visible = false;
                btnSQLReset.Visible = false;

                btnAdd.Visible = false;
                btnDel.Visible = false;
            }
            else if (this.Text == "Nurse")
            {
                lblSQL.Visible = false;
                txtSQL.Visible = false;
                btnSQL.Visible = false;
                btnSQLReset.Visible = false;
            }
            else if (this.Text == "Patient")
            {
                lblSQL.Visible = false;
                txtSQL.Visible = false;
                btnSQL.Visible = false;
                btnSQLReset.Visible = false;

                btnAdd.Visible = false;
                btnDel.Visible = false;
                btnFix.Visible = false;
                btnReset.Visible = false;
            }

            //열람가능한 테이블을 저장
            for (int i = 0; i < userTables.Count; i++)
                cb1.Items.Add(userTables[i]);

            btnAdd.Enabled = false;
            btnDel.Enabled = false;
            btnFix.Enabled = false;
            btnReset.Enabled = false;

            try
            {
                conn = new OleDbConnection(connectionString);
                conn.Open();
            }
            catch
            {
                MessageBox.Show("Error : DB Connecting");
            }
        }

        //Setting tableName
        private void cb1_SelectedValueChanged(object sender, EventArgs e)
        {
            //관리자일 때와 아닐 때를 구분하여 받는다.
            if (ID != "MANAGER")
                tableName = "manager." + cb1.SelectedItem.ToString();
            else
                tableName = cb1.SelectedItem.ToString();

            //All Reset
            dataGridView1.Columns.Clear();
            dataGridView1.Rows.Clear();
            dataTypes.Clear();
            PrimaryKeys.Clear();
            if (textBoxes != null && labels != null)
            {
                for (int i = 0; i < textBoxes.Length; i++)
                {
                    textBoxes[i].Dispose();
                    labels[i].Dispose();
                }
            }
            if (buttons != null)
            {
                for (int i = 0; i < buttons.Length; i++)
                    if (buttons[i] != null)
                        buttons[i].Dispose();
            }

            tableStart = true;
            btnAdd.Enabled = true;
            btnDel.Enabled = true;
            btnFix.Enabled = true;
            btnReset.Enabled = true;
            btnSQL.Enabled = true;
            btnSQLReset.Enabled = true;

            updatedb();
        }

        //세부사항을 입력하는 버튼을 누르면
        private void btnPlus_Click(object sender, EventArgs e)
        {
            Button tmp = sender as Button;
            for (int i = 0; i < buttons.Length; i++)
            {
                if (tmp.Equals(buttons[i]))
                {
                    new DetailForm(textBoxes[i], labels[i].Text, textBoxes[i].Text, textBoxes[i].MaxLength).ShowDialog();
                    return;
                }
            }   
        }

        //Print to DataGridView (select table_name from user_tables)
        private void updatedb()
        {
            try
            {
                OleDbCommand cmd = new OleDbCommand();

                cmd.CommandText = "select * from " + tableName; //예 : select * from table_name;
                cmd.CommandType = CommandType.Text; //검색명령을 쿼리 형태로
                cmd.Connection = conn;

                OleDbDataReader read = cmd.ExecuteReader();
                int Col = dataGridView1.ColumnCount = read.FieldCount;  //테이블의 열 개수를 반환하고 저장

                //해당 테이블을 처음 불러왔을 때 실행
                if (tableStart)
                {
                    //기본키를 갖는 속성을 가져온다.
                    DataTable PKTable = conn.GetOleDbSchemaTable(OleDbSchemaGuid.Primary_Keys, new object[] { null, null, tableName });
                    if (PKTable.Rows.Count >= 1)
                        PrimaryKeys.Add(PKTable.Rows[0][3].ToString());

                    //테이블의 속성 정보를 가져온다.
                    tableInfo = new ArrayList[Col];
                    int index = 0;
                    DataRowCollection tmp = read.GetSchemaTable().Rows;
                    foreach (DataRow attr in tmp)
                    {
                        object[] tmp2 = attr.ItemArray;

                        //테이블에 있는 속성의 모든 정보(nullable, dataType, length, ...)를 담는다.
                        tableInfo[index] = new ArrayList();
                        for (int i = 0; i < 9; i++)
                            tableInfo[index].Add(tmp2[i].ToString());
                        index++;

                        //System.DataType 형태를 "DataType"로 받는다.
                        dataTypes.Add(tmp2[5].ToString().Substring(7));
                    }

                    /*
                    foreach (string var in dataTypes)
                        txtSQL.Text += var;
                    */

                    //레이블, 텍스트박스, 세부버튼 동적생성
                    if (!tableName.StartsWith("manager.개인정보") && !tableName.StartsWith("manager.담당환자"))
                    {
                        textBoxes = new TextBox[Col];
                        labels = new Label[Col];
                        buttons = new Button[Col];
                        for (int i = 0; i < Col; i++)
                        {
                            labels[i] = new Label();
                            labels[i].Name = "lbl" + i;
                            labels[i].Size = new Size(80, 20);
                            labels[i].Location = new Point(720, 132 + (i * 40));
                            labels[i].Text = read.GetName(i);

                            //필수로 작성해야 하는 항목이면, 레이블에 밑줄을 추가한다.
                            if (tableInfo[i][8].ToString() == "False")
                                labels[i].Font = new Font(labels[i].Font.Name, labels[i].Font.SizeInPoints, FontStyle.Underline);
                            this.Controls.Add(labels[i]);

                            textBoxes[i] = new TextBox();
                            textBoxes[i].Name = "tb" + i;
                            textBoxes[i].Location = new Point(810, 130 + (i * 40));

                            //VARCHAR2 가변길이만큼 최대 길이 설정
                            if (tableInfo[i][6].ToString() == "200")
                            {
                                if (Convert.ToInt32(tableInfo[i][2]) > 20)  //최대 길이가 많이 길 경우
                                {
                                    //+ 버튼을 통해서 많은 내용을 입력할 수 있게 새로운 폼을 연다.
                                    buttons[i] = new Button();
                                    buttons[i].Click += new EventHandler(btnPlus_Click);
                                    buttons[i].Text = "+";
                                    buttons[i].Size = new Size(22, 22);
                                    buttons[i].Name = "btnPlus" + i;
                                    buttons[i].Location = new Point(920, 130 + (i * 40));
                                    this.Controls.Add(buttons[i]);
                                }

                                textBoxes[i].MaxLength = Convert.ToInt32(tableInfo[i][2]);
                            }
                            //Decimal 가변길이만큼 최대 길이 설정
                            else if (tableInfo[i][6].ToString() == "131")
                            {
                                if (Convert.ToInt32(tableInfo[i][3]) > 20)  //최대 길이가 많이 길 경우
                                {
                                    //+ 버튼을 통해서 많은 내용을 입력할 수 있게 새로운 폼을 연다.
                                    buttons[i] = new Button();
                                    buttons[i].Click += new EventHandler(btnPlus_Click);
                                    buttons[i].Text = "+";
                                    buttons[i].Size = new Size(22, 22);
                                    buttons[i].Name = "btnPlus" + i;
                                    buttons[i].Location = new Point(920, 130 + (i * 40));
                                    this.Controls.Add(buttons[i]);
                                }

                                textBoxes[i].MaxLength = Convert.ToInt32(tableInfo[i][3]);
                            }
                            else
                                textBoxes[i].MaxLength = 10;

                            this.Controls.Add(textBoxes[i]);
                        }
                    }

                    //권한에 따른 접근 설정
                    for (int i = 0; i < Col; i++)
                    {
                        //의사
                        if (tableName.StartsWith("manager.의사소견"))
                        {
                            if (labels[i].Text != "의사소견")
                                textBoxes[i].Enabled = false;
                        }

                        if (tableName.StartsWith("manager.진료내용"))
                        {
                            if (labels[i].Text != "진료내용")
                                textBoxes[i].Enabled = false;
                        }

                        //환자
                        if (tableName.StartsWith("manager.개인차트"))
                            textBoxes[i].Enabled = false;
                    }

                    tableStart = false;
                }
                
                //필드명 받아오는 반복문
                for (int i = 0; i < Col; i++)
                    dataGridView1.Columns[i].Name = read.GetName(i);

                //행 단위로 반복
                while (read.Read())
                {
                    object[] obj = new object[Col]; // 필드수만큼 오브젝트 배열

                    for (int i = 0; i < Col; i++) // 필드 수만큼 반복
                    {
                        obj[i] = new object();
                        obj[i] = read.GetValue(i); // 오브젝트배열에 데이터 저장
                    }

                    dataGridView1.Rows.Add(obj); //데이터그리드뷰에 오브젝트 배열 추가
                }

                read.Close();
            }
            catch (Exception ex)
            {
                txtSQL.Text += "Error updateDB-part";
                MessageBox.Show("Error: " + ex.Message); //에러 메세지 
            }
        }

        //그리드뷰의 셀을 클릭했을 때
        private void dataGridView1_Click(object sender, DataGridViewCellMouseEventArgs e)
        {
            if (textBoxes == null)
                return;

            //Text Clear
            for (int i = 0; i < textBoxes.Length; i++)
                textBoxes[i].Clear();

            try
            {
                for (int i = 0; i < textBoxes.Length; i++)
                {
                    if (tableInfo[i][6].ToString() == "135")
                        textBoxes[i].Text = dataGridView1.Rows[e.RowIndex].Cells[i].Value.ToString().Substring(0, 10);
                    else
                        textBoxes[i].Text = dataGridView1.Rows[e.RowIndex].Cells[i].Value.ToString();
                }
            }
            catch
            {
                return;
            }
        }

        //Insert
        private void button2_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            SQLStart = true;
            int check = 0;

            OleDbCommand cmd = new OleDbCommand();

            if (textBoxes == null)
                return;

            //모든 입력 오류에 대한 검사
            for (int i = 0; i < textBoxes.Length; i++)
            {
                //텍스트박스가 비어있는지 검사
                if (textBoxes[i].Text == "")
                    check++;

                //DateTime 검사
                if (tableInfo[i][6].ToString() == "135" && textBoxes[i].Text != "")
                {
                    //정규식으로 원하는 포맷인지 검사
                    string pattern = @"\d{4}-\d{2}-\d{2}$";
                    if (!Regex.IsMatch(textBoxes[i].Text, pattern))
                    {
                        MessageBox.Show("날짜 타입이 맞지 않습니다. \n Input example : 2018-12-01");
                        updatedb();
                        return;
                    }
                }

                //NULL 검사
                if (tableInfo[i][8].ToString() == "False" && textBoxes[i].Text == "")
                {
                    MessageBox.Show("해당 값은 null 일 수 없습니다. [" + (i + 1) + "번]");
                    updatedb();
                    return;
                }

                //VARCHAR2 가변길이의 문자열의 길이 검사
                if (tableInfo[i][6].ToString() == "200")
                {
                    if (Convert.ToInt32(tableInfo[i][2]) < textBoxes[i].Text.Length)    //예: VARCHAR2(9)
                    {
                        MessageBox.Show("해당 문자열이 너무 많습니다. [" + (i + 1) + "번]");
                        updatedb();
                        return;
                    }
                }

                //Decimal 가변길이의 길이 검사
                if (tableInfo[i][6].ToString() == "131")
                {
                    if (Convert.ToInt32(tableInfo[i][4]) == 0)  //정수, 예: NUMBER(4)
                    {
                        if (Convert.ToInt32(tableInfo[i][3]) < textBoxes[i].Text.Length)
                        {
                            MessageBox.Show("해당 숫자가 너무 많습니다. [" + (i + 1) + "번]");
                            updatedb();
                            return;
                        }
                    }
                    else    //소숫점을 포함하는 수, 예: NUMBER(7,2)이면, 정수자릿수 5 소수자릿수 2
                    {
                        if (Convert.ToInt32(tableInfo[i][3]) < textBoxes[i].Text.Length)
                        {
                            MessageBox.Show("해당 숫자가 너무 많습니다. [" + (i + 1) + "번]");
                            updatedb();
                            return;
                        }

                        if (textBoxes[i].Text.Contains("."))    //소숫점이 존재, 예: 5000.00
                        {
                            int onIndex = textBoxes[i].Text.IndexOf('.') + 1;   //.의 위치
                            txtSQL.Text += onIndex;

                            int pointLength = textBoxes[i].Text.Length - onIndex;
                            if (pointLength > Convert.ToInt32(tableInfo[i][4]))
                            {
                                MessageBox.Show("해당 소숫점 이하가 너무 많습니다. [" + (i + 1) + "번]");
                                updatedb();
                                return;
                            }
                        }
                        else    //소숫점이 존재하지 않을 때, 예: 55555
                        {
                            if (Convert.ToInt32(tableInfo[i][3]) - Convert.ToInt32(tableInfo[i][4]) < textBoxes[i].Text.Length)
                            {
                                MessageBox.Show("해당 숫자가 너무 많습니다. [" + (i + 1) + "번]");
                                updatedb();
                                return;
                            }
                        }
                    }
                }
            }

            //모든 텍스트박스가 비어있으면
            if (check == textBoxes.Length)
            {
                MessageBox.Show("모든 항목의 값이 null 일 수 없습니다.");
                updatedb();
                return;
            }

            //테이블의 속성 개수만큼 값 삽입
            cmd.CommandText = "INSERT INTO " + tableName + " VALUES(";
            for (int i = 0; i < textBoxes.Length; i++)
            {
                if (SQLStart == true)
                {
                    //각 속성의 NULL 값을 고려한다.
                    if (textBoxes[i].Text == "")
                    {
                        if (i == textBoxes.Length - 1)
                            cmd.CommandText += "'" + textBoxes[i].Text + "')";
                        else
                            cmd.CommandText += "'" + textBoxes[i].Text + "'";
                    }
                    else if (dataTypes[i].ToString().Equals("Decimal") || dataTypes[i].ToString().Equals("Double"))
                    {
                        if (i == textBoxes.Length - 1)
                            cmd.CommandText += textBoxes[i].Text + ")";
                        else
                            cmd.CommandText += textBoxes[i].Text;
                    }
                    else if (dataTypes[i].ToString().Equals("String"))
                    {
                        if (i == textBoxes.Length - 1)
                            cmd.CommandText += "'" + textBoxes[i].Text + "')";
                        else
                            cmd.CommandText += "'" + textBoxes[i].Text + "'";
                    }
                    else if (dataTypes[i].ToString().Equals("DateTime"))
                    {
                        if (i == textBoxes.Length - 1)
                            cmd.CommandText += "'" + textBoxes[i].Text + "')";
                        else
                            cmd.CommandText += "'" + textBoxes[i].Text + "'";
                    }

                    SQLStart = false;
                }
                else
                {
                    //각 속성의 NULL 값을 고려한다.
                    if (textBoxes[i].Text == "")
                    {
                        if (i == textBoxes.Length - 1)
                            cmd.CommandText += ",'" + textBoxes[i].Text + "')";
                        else
                            cmd.CommandText += ",'" + textBoxes[i].Text + "'";
                    }
                    else if (dataTypes[i].ToString().Equals("Decimal") || dataTypes[i].ToString().Equals("Double"))
                    {
                        if (i == textBoxes.Length - 1)
                            cmd.CommandText += "," + textBoxes[i].Text + ")";
                        else
                            cmd.CommandText += "," + textBoxes[i].Text;
                    }
                    else if (dataTypes[i].ToString().Equals("String"))
                    {
                        if (i == textBoxes.Length - 1)
                            cmd.CommandText += ",'" + textBoxes[i].Text + "')";
                        else
                            cmd.CommandText += ",'" + textBoxes[i].Text + "'";
                    }
                    else if (dataTypes[i].ToString().Equals("DateTime"))
                    {
                        if (i == textBoxes.Length - 1)
                            cmd.CommandText += ",'" + textBoxes[i].Text + "')";
                        else
                            cmd.CommandText += ",'" + textBoxes[i].Text + "'";
                    }
                }
            }

            txtSQL.Text = cmd.CommandText;
            cmd.CommandType = CommandType.Text; //검색명령을 쿼리 형태로
            cmd.Connection = conn;

            /*
            cmd.ExecuteNonQuery(); //데이터를 반환하지 않는 쿼리를 실행한다. 예를 들면, insert, delete, update 이다.
            cmd.ExecuteReader(); //데이터를 반환하는 쿼리를 실행한다. 예를 들면, select 이다.
            */
            try
            {
                cmd.ExecuteNonQuery(); //쿼리문을 실행하고 영향받는 행의 수를 반환
            }
            catch
            {
                //PK, FK에 대한 체크
                MessageBox.Show("무결성 제약조건에 위배됩니다. \n값을 확인하세요");
                updatedb();
                return;
            }

            updatedb();
        }

        //Delete
        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            SQLStart = true;
            int check = 0;

            try
            {
                OleDbCommand cmd = new OleDbCommand();

                cmd.CommandText = "delete from " + tableName + " where ";
                //기본키를 갖고 있는 테이블은 NULL일 수가 없다. 만약에 사용자가 입력하지 않으면 입력하라는 메시지를 보낸다.
                //기본키를 갖고 있지 않는 테이블도 입력한 데이터가 하나도 없다면 하나라도 입력하라는 메시지를 보낸다.
                if (PrimaryKeys.Count != 0)
                {
                    for (int i = 0; i < PrimaryKeys.Count; i++)
                    {
                        for (int j = 0; j < textBoxes.Length; j++)
                        {
                            if (PrimaryKeys[i].Equals(labels[j].Text))
                            {
                                //NULL이면
                                if (textBoxes[i].Text == "")
                                {
                                    MessageBox.Show("기본키는 null 일 수 없습니다.");
                                    updatedb();
                                    return;
                                }

                                if (dataTypes[j].ToString().Equals("Decimal") || dataTypes[j].ToString().Equals("Double"))
                                {
                                    if (i == PrimaryKeys.Count - 1)
                                        cmd.CommandText += labels[j].Text + " = " + textBoxes[j].Text;
                                    else
                                        cmd.CommandText += labels[j].Text + " = " + textBoxes[j].Text + " AND ";
                                }
                                else if (dataTypes[j].ToString().Equals("String") || dataTypes[j].ToString().Equals("DateTime"))
                                {
                                    if (i == PrimaryKeys.Count - 1)
                                        cmd.CommandText += labels[j].Text + " = '" + textBoxes[j].Text + "'";
                                    else
                                        cmd.CommandText += labels[j].Text + " = '" + textBoxes[j].Text + "' AND ";
                                }
                            }
                        }
                    }
                }
                else
                {
                    for (int i = 0; i < textBoxes.Length; i++)
                    {
                        //NULL 값 고려
                        if (textBoxes[i].Text == "")
                        {
                            check++;
                            continue;
                        }

                        if (SQLStart == true)
                        {
                            if (dataTypes[i].ToString().Equals("Decimal") || dataTypes[i].ToString().Equals("Double"))
                            {
                                cmd.CommandText += labels[i].Text + " = " + textBoxes[i].Text;
                            }
                            else if (dataTypes[i].ToString().Equals("String") || dataTypes[i].ToString().Equals("DateTime"))
                            {
                                cmd.CommandText += labels[i].Text + " = '" + textBoxes[i].Text + "'";
                            }

                            SQLStart = false;
                        }
                        else
                        {
                            if (dataTypes[i].ToString().Equals("Decimal") || dataTypes[i].ToString().Equals("Double"))
                            {
                                cmd.CommandText += " AND " + labels[i].Text + " = " + textBoxes[i].Text;
                            }
                            else if (dataTypes[i].ToString().Equals("String") || dataTypes[i].ToString().Equals("DateTime"))
                            {
                                cmd.CommandText += " AND " + labels[i].Text + " = '" + textBoxes[i].Text + "'";
                            }
                        }   
                    }
                }

                //모든 텍스트박스가 비어있는지 검사
                if (check == textBoxes.Length)
                {
                    MessageBox.Show("모든 항목의 값이 null 일 수 없습니다.");
                    updatedb();
                    return;
                }

                txtSQL.Text = cmd.CommandText;
                cmd.CommandType = CommandType.Text; //검색명령을 쿼리 형태로
                cmd.Connection = conn;
                cmd.ExecuteNonQuery(); //쿼리문을 실행하고 영향받는 행의 수를 반환

                updatedb();
            }
            catch (Exception ex)
            {
                txtSQL.Text += "Error delete-part";
                MessageBox.Show("Error: " + ex.Message); //에러 메세지 
            }
            finally
            {
                if (conn != null)
                {
                    //conn.Close(); //데이터베이스 연결 해제
                }
            }
        }

        //Update
        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Clear();
            SQLStart = true;
            int check = 0;

            try
            {
                OleDbCommand cmd = new OleDbCommand();

                //모든 입력 오류에 대한 검사
                for (int i = 0; i < textBoxes.Length; i++)
                {
                    //텍스트박스가 비어있는지 검사
                    if (textBoxes[i].Text == "")
                        check++;

                    //DateTime 검사
                    if (tableInfo[i][6].ToString() == "135" && textBoxes[i].Text != "")
                    {
                        //정규식으로 원하는 포맷인지 검사
                        string pattern = @"\d{4}-\d{2}-\d{2}$";
                        if (!Regex.IsMatch(textBoxes[i].Text, pattern))
                        {
                            MessageBox.Show("날짜 타입이 맞지 않습니다. \n Input example : 2018-12-01");
                            updatedb();
                            return;
                        }
                    }

                    //NULL 검사
                    if (tableInfo[i][8].ToString() == "False" && textBoxes[i].Text == "")
                    {
                        MessageBox.Show("해당 값은 null 일 수 없습니다. [" + (i + 1) + "번]");
                        updatedb();
                        return;
                    }

                    //VARCHAR2 가변길이의 문자열의 길이 검사
                    if (tableInfo[i][6].ToString() == "200")
                    {
                        if (Convert.ToInt32(tableInfo[i][2]) < textBoxes[i].Text.Length)    //예: VARCHAR2(9)
                        {
                            MessageBox.Show("해당 문자열이 너무 많습니다. [" + (i + 1) + "번]");
                            updatedb();
                            return;
                        }
                    }

                    //Decimal 가변길이의 길이 검사
                    if (tableInfo[i][6].ToString() == "131")
                    {
                        if (Convert.ToInt32(tableInfo[i][4]) == 0)  //정수, 예: NUMBER(4)
                        {
                            if (Convert.ToInt32(tableInfo[i][3]) < textBoxes[i].Text.Length)
                            {
                                MessageBox.Show("해당 숫자가 너무 많습니다. [" + (i + 1) + "번]");
                                updatedb();
                                return;
                            }
                        }
                        else    //소숫점을 포함하는 수, 예: NUMBER(7,2)이면, 정수자릿수 5 소수자릿수 2
                        {
                            if (Convert.ToInt32(tableInfo[i][3]) < textBoxes[i].Text.Length)
                            {
                                MessageBox.Show("해당 숫자가 너무 많습니다. [" + (i + 1) + "번]");
                                updatedb();
                                return;
                            }

                            if (textBoxes[i].Text.Contains("."))    //소숫점이 존재, 예: 5000.00
                            {
                                int onIndex = textBoxes[i].Text.IndexOf('.') + 1;   //.의 위치
                                txtSQL.Text += onIndex;

                                int pointLength = textBoxes[i].Text.Length - onIndex;
                                if (pointLength > Convert.ToInt32(tableInfo[i][4]))
                                {
                                    MessageBox.Show("해당 소숫점 이하가 너무 많습니다. [" + (i + 1) + "번]");
                                    updatedb();
                                    return;
                                }
                            }
                            else    //소숫점이 존재하지 않을 때, 예: 55555
                            {
                                if (Convert.ToInt32(tableInfo[i][3]) - Convert.ToInt32(tableInfo[i][4]) < textBoxes[i].Text.Length)
                                {
                                    MessageBox.Show("해당 숫자가 너무 많습니다. [" + (i + 1) + "번]");
                                    updatedb();
                                    return;
                                }
                            }
                        }
                    }
                }

                //모든 텍스트박스가 비어있는지 검사
                if (check == textBoxes.Length)
                {
                    MessageBox.Show("모든 항목의 값이 null 일 수 없습니다.");
                    updatedb();
                    return;
                }

                //테이블의 원하는 속성 값 수정
                cmd.CommandText = "update " + tableName + " set ";
                for (int i = 0; i < textBoxes.Length; i++)
                {
                    if (SQLStart == true)
                    {
                        //NULL 값 처리
                        if (textBoxes[i].Text == "")
                        {
                            if (i == textBoxes.Length - 1)
                                cmd.CommandText += labels[i].Text + " = ''";
                            else
                                cmd.CommandText += labels[i].Text + " = ''";

                            continue;
                        }

                        if (dataTypes[i].ToString().Equals("Decimal") || dataTypes[i].ToString().Equals("Double"))
                        {
                            if (i == textBoxes.Length - 1)
                                cmd.CommandText += labels[i].Text + " = " + textBoxes[i].Text + "";
                            else
                                cmd.CommandText += labels[i].Text + " = " + textBoxes[i].Text + "";
                        }
                        else if (dataTypes[i].ToString().Equals("String") || dataTypes[i].ToString().Equals("DateTime"))
                        {
                            if (i == textBoxes.Length - 1)
                                cmd.CommandText += labels[i].Text + " = '" + textBoxes[i].Text + "'";
                            else
                                cmd.CommandText += labels[i].Text + " = '" + textBoxes[i].Text + "'";
                        }

                        SQLStart = false;
                    }
                    else
                    {
                        //NULL 값 처리
                        if (textBoxes[i].Text == "")
                        {
                            if (i == textBoxes.Length - 1)
                                cmd.CommandText += ", " + labels[i].Text + " = ''";
                            else
                                cmd.CommandText += ", " + labels[i].Text + " = ''";

                            continue;
                        }

                        if (dataTypes[i].ToString().Equals("Decimal") || dataTypes[i].ToString().Equals("Double"))
                        {
                            if (i == textBoxes.Length - 1)
                                cmd.CommandText += ", " + labels[i].Text + " = " + textBoxes[i].Text + "";
                            else
                                cmd.CommandText += ", " + labels[i].Text + " = " + textBoxes[i].Text + "";
                        }
                        else if (dataTypes[i].ToString().Equals("String") || dataTypes[i].ToString().Equals("DateTime"))
                        {
                            if (i == textBoxes.Length - 1)
                                cmd.CommandText += ", " + labels[i].Text + " = '" + textBoxes[i].Text + "'";
                            else
                                cmd.CommandText += ", " + labels[i].Text + " = '" + textBoxes[i].Text + "'";
                        }
                    }
                }

                //SQLStart = true;
                cmd.CommandText += " where ";

                //기본키가 존재하는 테이블 수정
                if (PrimaryKeys.Count != 0)
                {
                    //기본키가 2개 이상의 속성으로 이루어진 테이블도 처리하기 위해 반복한다.
                    for (int i = 0; i < PrimaryKeys.Count; i++)
                    {
                        for (int j = 0; j < textBoxes.Length; j++)
                        {
                            if (PrimaryKeys[i].Equals(labels[j].Text))
                            {
                                //NULL이면
                                if (textBoxes[j].Text == "")
                                {
                                    MessageBox.Show("기본키는 null 일 수 없습니다.");
                                    updatedb();
                                    return;
                                }

                                if (dataTypes[j].ToString().Equals("Decimal") || dataTypes[j].ToString().Equals("Double"))
                                {
                                    if (i == PrimaryKeys.Count - 1)
                                        cmd.CommandText += labels[j].Text + " = " + textBoxes[j].Text;
                                    else
                                        cmd.CommandText += labels[j].Text + " = " + textBoxes[j].Text + " AND ";
                                }
                                else if (dataTypes[j].ToString().Equals("String") || dataTypes[j].ToString().Equals("DateTime"))
                                {
                                    if (i == PrimaryKeys.Count - 1)
                                        cmd.CommandText += labels[j].Text + " = '" + textBoxes[j].Text + "'";
                                    else
                                        cmd.CommandText += labels[j].Text + " = '" + textBoxes[j].Text + "' AND ";
                                }
                            }
                        }
                    }
                }
                else    //기본키가 존재하지 않는 테이블 수정
                {
                    for (int i = 0; i < textBoxes.Length; i++)
                    {
                        //NULL 값 고려
                        if (textBoxes[i].Text == "")
                        {
                            continue;
                        }

                        //NULL이 아닌 가장 먼저 만나는 값을 조건으로 사용
                        if (dataTypes[i].ToString().Equals("Decimal") || dataTypes[i].ToString().Equals("Double"))
                            cmd.CommandText += labels[i].Text + " = " + textBoxes[i].Text + "";
                        else if (dataTypes[i].ToString().Equals("String") || dataTypes[i].ToString().Equals("DateTime"))
                            cmd.CommandText += labels[i].Text + " = '" + textBoxes[i].Text + "'";

                        break;
                    }
                }

                txtSQL.Text += cmd.CommandText;
                cmd.CommandType = CommandType.Text; //검색명령을 쿼리 형태로
                cmd.Connection = conn;
                cmd.ExecuteNonQuery(); //쿼리문을 실행하고 영향받는 행의 수를 반환

                updatedb();
            }
            catch (Exception ex)
            {
                txtSQL.Text += "Error update-part";
                MessageBox.Show("Error: " + ex.Message); //에러 메세지 
            }
            finally
            {
                if (conn != null)
                {
                    //conn.Close(); //데이터베이스 연결 해제
                }
            }
        }

        //View & TextBox Clear
        private void button5_Click(object sender, EventArgs e)
        {
            //Clear
            dataGridView1.Rows.Clear();
            if (textBoxes != null)
                for (int i = 0; i < textBoxes.Length; i++)
                    textBoxes[i].Clear();

            conn = new OleDbConnection(connectionString);
            conn.Open();

            updatedb();
        }

        //SQL query sending
        private void btSQL_Click(object sender, EventArgs e)
        {
            OleDbCommand cmd = new OleDbCommand();
            cmd.CommandText = txtSQL.Text;
            cmd.CommandType = CommandType.Text; //검색명령을 쿼리 형태로
            cmd.Connection = conn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                txtSQL.Text += "Error SQL-part";
                MessageBox.Show("Error: " + ex.Message); //에러 메세지 
                return;
            }

            MessageBox.Show("성공적으로 처리하였습니다.");
        }

        //SQL query reset
        private void btnSQLReset_Click(object sender, EventArgs e)
        {
            txtSQL.Text = "";
        }

        //계정관리
        private void btnCreate_Click(object sender, EventArgs e)
        {
            new CreateForm(this, connectionString).ShowDialog();
        }
    }
}