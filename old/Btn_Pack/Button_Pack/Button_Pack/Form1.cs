using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using Org.BouncyCastle.Asn1.X509;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace Button_Pack
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            LoadData("data", "date", "DESC", dataGridView1);
            Account();
        }

        private void Account()
        {
            try
            {
                // 파일에서 문자열 읽기
                string content = File.ReadAllText("C:/user.txt");

                // 쉼표를 기준으로 문자열 분리
                string[] parts = content.Split(',');

                // 분리된 문자열을 각각의 TextBox에 할당
                if (parts.Length == 2)
                {
                    IDBox.Text = parts[0].Trim();
                    PWBox.Text = parts[1].Trim();
                }
            }
            catch (Exception ex)
            {
                if (string.IsNullOrEmpty(IDBox.Text))
                {
                    IDBox.Text = "Knox ID";
                }
                if (string.IsNullOrEmpty(PWBox.Text))
                {
                    PWBox.Text = "Windows PW";
                }
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            TabControl tabControl = sender as TabControl;

            if (tabControl != null)
            {
                switch (tabControl.SelectedIndex)
                {
                    case 0: // 첫 번째 탭이 선택되었을 때
                        LoadData("data", "date", "DESC", dataGridView1);
                        break;
                    case 1: // 두 번째 탭이 선택되었을 때
                        LoadData("data0", "date", "DESC", dataGridView2);
                        break;
                    case 2: // 세 번째 탭이 선택되었을 때
                        LoadData("data1", "date", "DESC", dataGridView3);
                        break;
                    // 추가적인 탭에 대한 처리를 여기에 추가
                    default:
                        break;
                }
            }
        }
        private void LoadData(string tableName, string columnName, string order, DataGridView dataGridView)
        {
            string connectionString = "Server=localhost;Port=3306;Database=testdb;Uid=root;Pwd=1234;";
            MySqlConnection connection = new MySqlConnection(connectionString);
            
            try
            {
                connection.Open();
                Console.WriteLine("MySQL 데이터베이스에 연결되었습니다.");
                
                // 데이터 조회 쿼리
                string query = $"SELECT * FROM {tableName} ORDER BY {columnName} {order}";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);
                // DataGridView에 데이터를 설정
                dataGridView.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                Console.WriteLine("에러: " + ex.Message);
            }
            finally
            {
                connection.Close();
                Console.WriteLine("MySQL 데이터베이스 연결을 닫았습니다.");
            }
        }


        private void button1_Click(object sender, EventArgs e)
        {
            string filePath = @"C:/user.txt";
            string content = $"{IDBox.Text},{PWBox.Text}";

            File.WriteAllText(filePath, content);

       
            MessageBox.Show("C:user.txt 에 저장되었습니다.");
        }

        private void dataGridView1_DoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // 더블 클릭된 행의 "link" 컬럼의 값을 가져옴
            string url = dataGridView1.Rows[e.RowIndex].Cells["link"].Value.ToString();

            // URL이 유효한지 간단하게 확인 (더 정교한 검증을 원하면 정규식 등을 사용)
            if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
            {
                System.Diagnostics.Process.Start(url); // 기본 웹 브라우저로 URL 열기
            }
            else
            {
                MessageBox.Show("유효하지 않은 URL입니다.");
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(@"C:/user.txt");
            }
            catch (Exception ex)
            {
                MessageBox.Show("파일을 실행하는 중 오류가 발생했습니다: " + ex.Message);
            }
        }

        private void button24_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("https://www.google.com");
            }
            catch (Exception ex)
            {
                MessageBox.Show("웹사이트를 열 수 없습니다: " + ex.Message);
            }
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            UpdateDatabase(dataGridView1.Rows[e.RowIndex]);
        }
        private void UpdateDatabase(DataGridViewRow row)
        {
            string connectionString = "Server=localhost;Port=3306;Database=testdb;Uid=root;Pwd=1234;";
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // 예를 들어, ID 컬럼을 기준으로 데이터를 업데이트한다고 가정합니다.
                // 실제 구현에서는 적절한 컬럼명과 값을 사용해야 합니다.
                string query = $"UPDATE data SET column1 = @value1, column2 = @value2 WHERE ID = @id";

                using (MySqlCommand cmd = new MySqlCommand(query, connection))
                {
                    // 파라미터 값을 설정합니다.
                    cmd.Parameters.AddWithValue("@value1", row.Cells["column1"].Value);
                    cmd.Parameters.AddWithValue("@value2", row.Cells["column2"].Value);
                    cmd.Parameters.AddWithValue("@id", row.Cells["ID"].Value);

                    cmd.ExecuteNonQuery();
                }
            }
        }

    }
}

