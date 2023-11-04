using System;
using System.Collections;
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

namespace CRUD
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=localhost;Port=3306;Database=testdb;Uid=root;Pwd=1234;";
        private MySqlDataAdapter dataAdapter;
        private DataTable dataTable;
        private Timer closeTimer = new Timer();
        private Form2 secondForm;

        public Form1()
        {
            InitializeComponent();
            this.KeyPreview = true;
            comboBox1.Items.AddRange(new string[] { "분임조 선택", "S1DPN", "S3DPN", "S4DPN", "S5DPN" });
            /// 저장된 값을 로드
            if (!string.IsNullOrEmpty(Properties.Settings.Default.ComboBoxValue))
            {
                comboBox1.SelectedItem = Properties.Settings.Default.ComboBoxValue;
            }
            else
            {
                comboBox1.SelectedIndex = 0; // 기본 선택값 설정
            }

            // comboBox1에서 선택된 값을 LoadData의 filter로 전달
            if (comboBox1.SelectedItem != null)
            {
                LoadData(comboBox1.SelectedItem.ToString());
            }
            this.KeyDown += Form1_KeyDown; // KeyDown 이벤트 핸들러 연결
                                           // Timer 설정
            closeTimer.Interval = 1000; // 3초
            closeTimer.Tick += CloseTimer_Tick; // Tick 이벤트 핸들러
        }
        private void CloseTimer_Tick(object sender, EventArgs e)
        {
            if (secondForm != null && !secondForm.IsDisposed)
            {
                secondForm.Close();
            }
            closeTimer.Stop(); // Timer를 중지합니다.
        }
        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                try
                {
                    dataAdapter.Update(dataTable);
                    // Form2를 열고 Timer를 시작합니다.
                    secondForm = new Form2();
                    secondForm.Show();
                    closeTimer.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }

                e.Handled = true; // 이벤트 처리 완료 표시
            }
            if (e.KeyCode == Keys.F5)
            {
                btnRead_Click(sender, e);
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (comboBox1.SelectedItem != null)
            {
                string selectedValue = comboBox1.SelectedItem.ToString();

                // comboBox1에서 선택된 값을 LoadData의 filter로 전달
                LoadData(selectedValue);

                // ComboBox의 선택된 값을 저장
                Properties.Settings.Default.ComboBoxValue = selectedValue;
                Properties.Settings.Default.Save();
            }

        }

        private void LoadData(string filter)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {

                string query = "SELECT * FROM racb";
                // filter 값에 따라 적절한 쿼리 선택
                
                // 체크박스가 체크되어 있으면 모든 값을, 체크 해제되어 있으면 view = 'show' 값을 보여줍니다.
                if (checkBox1.Checked)
                {
                    query = "SELECT * FROM racb";
                }
                else
                {
                    query = "SELECT * FROM racb WHERE view = 'show'";
                }

                // filter 값을 추가하기 전에 WHERE 절이 이미 있는지 확인
                if (filter != "분임조 선택")
                {
                    if (query.Contains("WHERE"))
                    {
                        query += $" AND sdwt = '{filter}'";
                    }
                    else
                    {
                        query += $" WHERE sdwt = '{filter}'";
                    }
                }
                query += " ORDER BY date DESC";

                //Console.WriteLine(query); // 여기서 SQL 문을 출력합니다.

                dataAdapter = new MySqlDataAdapter(query, connection);
                // Explicitly set the UpdateCommand
                dataAdapter.UpdateCommand = new MySqlCommand("UPDATE racb SET date=@date, model=@model, eqpid=@eqpid, cate=@cate, status=@status, vq=@vq, id=@id, view=@view, comment=@comment WHERE id=@id", connection);
                dataAdapter.UpdateCommand.Parameters.Add("@date", MySqlDbType.VarChar, 100, "date");
                dataAdapter.UpdateCommand.Parameters.Add("@model", MySqlDbType.VarChar, 100, "model");
                dataAdapter.UpdateCommand.Parameters.Add("@eqpid", MySqlDbType.VarChar, 100, "eqpid");
                dataAdapter.UpdateCommand.Parameters.Add("@cate", MySqlDbType.VarChar, 100, "cate");
                dataAdapter.UpdateCommand.Parameters.Add("@status", MySqlDbType.VarChar, 100, "status");
                dataAdapter.UpdateCommand.Parameters.Add("@vq", MySqlDbType.VarChar, 100, "vq");
                dataAdapter.UpdateCommand.Parameters.Add("@id", MySqlDbType.VarChar, 100, "id");
                dataAdapter.UpdateCommand.Parameters.Add("@view", MySqlDbType.VarChar, 100, "view");
                dataAdapter.UpdateCommand.Parameters.Add("@comment", MySqlDbType.VarChar, 100, "comment");

                dataTable = new DataTable();
                dataTable.Clear();
                dataAdapter.Fill(dataTable);
                dataGridView1.DataSource = dataTable;
                //dataGridView1.Columns["sdwt"].Visible = false;
               

            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            try
            {
                dataAdapter.Update(dataTable);
                // Form2를 열고 Timer를 시작합니다.
                secondForm = new Form2();
                secondForm.Show();
                closeTimer.Start();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}");
            }
        }

        private void btnRead_Click(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                LoadData(comboBox1.SelectedItem.ToString());
            }
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S)
            {
                try
                {
                    dataAdapter.Update(dataTable);
                    // Form2를 열고 Timer를 시작합니다.
                    secondForm = new Form2();
                    secondForm.Show();
                    closeTimer.Start();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error: {ex.Message}");
                }

                e.Handled = true; // 이벤트 처리 완료 표시
            }
            if (e.KeyCode == Keys.F5)
            {
                btnRead_Click(sender, e);
            }
        }

        private void btnInclude_Click(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // 현재 선택된 행을 가져옴
                DataGridViewRow currentRow = dataGridView1.CurrentRow;

                if (currentRow != null)
                {
                    // id 값을 가져옴
                    var id = currentRow.Cells["id"].Value;

                    // SQL UPDATE 문을 사용하여 view 컬럼 값을 변경
                    string query = $"UPDATE racb SET view='show' WHERE id=@id";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }

            // Form2를 열고 Timer를 시작합니다.
            secondForm = new Form2();
            secondForm.Show();
            closeTimer.Start();

            // LoadData 함수를 호출하여 최신 데이터 로드
            if (comboBox1.SelectedItem != null)
            {
                LoadData(comboBox1.SelectedItem.ToString());
            }
        }

        private void btnExcept_Click(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // 현재 선택된 행을 가져옴
                DataGridViewRow currentRow = dataGridView1.CurrentRow;

                if (currentRow != null)
                {
                    // id 값을 가져옴
                    var id = currentRow.Cells["id"].Value;

                    // SQL UPDATE 문을 사용하여 view 컬럼 값을 변경
                    string query = $"UPDATE racb SET view='hide' WHERE id=@id";
                    using (MySqlCommand cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@id", id);
                        cmd.ExecuteNonQuery();
                    }
                }

                connection.Close();
            }

            // Form2를 열고 Timer를 시작합니다.
            secondForm = new Form2();
            secondForm.Show();
            closeTimer.Start();

            // LoadData 함수를 호출하여 최신 데이터 로드
            if (comboBox1.SelectedItem != null)
            {
                LoadData(comboBox1.SelectedItem.ToString());
            }
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (comboBox1.SelectedItem != null)
            {
                LoadData(comboBox1.SelectedItem.ToString());
            }
        }
    }
}