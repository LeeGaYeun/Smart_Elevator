using System;
using System.Data;
using System.Data.SQLite;
using System.Windows.Forms;

namespace ElePrj
{
    public partial class Database : Form
    {
        public Database()
        {
            InitializeComponent();
            DisplayDatabaseContents();
        }

        private void DisplayDatabaseContents()
        {
            try
            {
                // 데이터베이스 연결 문자열
                string connectionString = "Data Source=C:\\Users\\EMBEDDED\\source\\repos\\Intel\\OpenCV\\eleprj\\faces.db;Version=3;";

                // SQL 쿼리
                string sql = "SELECT name, room FROM faces";

                // 데이터베이스 연결 생성
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    // 데이터베이스 연결 열기
                    connection.Open();

                    // 데이터 어댑터 및 데이터테이블 생성
                    using (SQLiteDataAdapter adapter = new SQLiteDataAdapter(sql, connection))
                    {
                        DataTable dataTable = new DataTable();

                        // 데이터 가져오기
                        adapter.Fill(dataTable);

                        // DataGridView에 데이터 표시
                        faceDb.DataSource = dataTable;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터베이스 연결 중 오류가 발생했습니다: " + ex.Message);
            }


        }

        private void RemoveRowFromDatabase(string name, string room)
        {
            try
            {
                // 데이터베이스 연결 문자열
                string connectionString = "Data Source=C:\\Users\\EMBEDDED\\source\\repos\\Intel\\OpenCV\\eleprj\\faces.db;Version=3;";

                // SQL 쿼리
                string sql = "DELETE FROM faces WHERE name = @name AND room = @room";

                // 데이터베이스 연결 생성
                using (SQLiteConnection connection = new SQLiteConnection(connectionString))
                {
                    // 데이터베이스 연결 열기
                    connection.Open();

                    // SQL 명령 및 매개변수 설정
                    using (SQLiteCommand command = new SQLiteCommand(sql, connection))
                    {
                        command.Parameters.AddWithValue("@name", name);
                        command.Parameters.AddWithValue("@room", room);

                        // 명령 실행
                        int rowsAffected = command.ExecuteNonQuery();

                        // 삭제된 행이 있으면 메시지 출력
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("데이터베이스에서 행이 성공적으로 삭제되었습니다.");
                        }
                        else
                        {
                            MessageBox.Show("해당하는 이름과 층수를 가진 행이 데이터베이스에 없습니다.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("데이터베이스 연결 중 오류가 발생했습니다: " + ex.Message);
            }
        }

        private void RemoveRowFromDatabase()
        {
            // 사용자로부터 이름과 층수 입력 받기
            string[] input = PromptInput("이름을 입력하세요:", "층수를 입력하세요:", "행 삭제");

            if (input != null)
            {
                // 입력된 이름과 층수로 데이터베이스에서 행 삭제
                RemoveRowFromDatabase(input[0], input[1]);
            }
            // 삭제 후 데이터베이스 내용 갱신
            DisplayDatabaseContents();
        }
        private string[] PromptInput(string message1, string message2, string caption)
        {
            string[] result = new string[2];

            using (var inputForm = new Form())
            {
                var lblMessage1 = new Label();
                lblMessage1.Text = message1;
                lblMessage1.Location = new System.Drawing.Point(10, 20);
                lblMessage1.AutoSize = true;

                var txtInput1 = new TextBox();
                txtInput1.Location = new System.Drawing.Point(10, 50);
                txtInput1.Size = new System.Drawing.Size(250, 20);

                var lblMessage2 = new Label();
                lblMessage2.Text = message2;
                lblMessage2.Location = new System.Drawing.Point(10, 80);
                lblMessage2.AutoSize = true;

                var txtInput2 = new TextBox();
                txtInput2.Location = new System.Drawing.Point(10, 110);
                txtInput2.Size = new System.Drawing.Size(250, 20);

                var btnOK = new Button();
                btnOK.Text = "OK";
                btnOK.DialogResult = DialogResult.OK;
                btnOK.Location = new System.Drawing.Point(90, 140);
                btnOK.Size = new System.Drawing.Size(75, 23);

                inputForm.Text = caption;
                inputForm.ClientSize = new System.Drawing.Size(280, 200);
                inputForm.Controls.AddRange(new Control[] { lblMessage1, txtInput1, lblMessage2, txtInput2, btnOK });
                inputForm.FormBorderStyle = FormBorderStyle.FixedDialog;
                inputForm.StartPosition = FormStartPosition.CenterScreen;
                inputForm.AcceptButton = btnOK;

                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    result[0] = txtInput1.Text;
                    result[1] = txtInput2.Text;
                    return result;
                }
                else
                {
                    return null;
                }
            }
        }

        private void del_Btn_Click(object sender, EventArgs e)
        {
            RemoveRowFromDatabase();
        }
    }
}
