using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;
using System.IO;
using System.Reflection;

namespace WindowsFormsApp1
{
    public partial class Form1 : Form
    {
        private Dictionary<string, Bitmap> imageCache = new Dictionary<string, Bitmap>();

        public Form1()
        {
            InitializeComponent();

            // DataGridView의 DoubleBuffered 속성 활성화
            typeof(DataGridView).InvokeMember("DoubleBuffered", BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.SetProperty, null, dataGridView1, new object[] { true });

            dataGridView1.CellFormatting += DataGridView1_CellFormatting; // 이벤트 핸들러 추가

            LoadData("data");
        }

        private void DataGridView1_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (dataGridView1.Columns[e.ColumnIndex].Name == "path" && e.Value != null && e.Value is string)
            {
                string imagePath = e.Value.ToString();

                // "0ea"나 "input"이 포함되어 있으면 이미지를 표시하지 않음
                if (imagePath.Contains("0ea") || imagePath.Contains("input"))
                {
                    e.Value = null;
                    e.FormattingApplied = true;
                    return;
                }

                if (File.Exists(imagePath))
                {
                    if (!imageCache.ContainsKey(imagePath))
                    {
                        Bitmap originalBitmap = new Bitmap(imagePath);
                        Bitmap resizedBitmap = new Bitmap(originalBitmap, new Size(600, 600));
                        imageCache[imagePath] = resizedBitmap;
                    }
                    e.Value = imageCache[imagePath];
                    e.FormattingApplied = true;
                }
                else
                {
                    e.Value = null;
                    e.FormattingApplied = true;
                }
            }
        }

        private void LoadData(string tableName)
        {
            string connectionString = "Server=localhost;Port=3306;Database=testdb;Uid=root;Pwd=1234;";
            MySqlConnection connection = new MySqlConnection(connectionString);

            try
            {
                connection.Open();
                Console.WriteLine("MySQL 데이터베이스에 연결되었습니다.");

                string query = $"SELECT * FROM {tableName}";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                dataGridView1.Columns.Clear();
                dataGridView1.DataSource = dataTable;

                dataGridView1.RowTemplate.Height = 200;
                dataGridView1.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;

                DataGridViewImageColumn imgCol = new DataGridViewImageColumn
                {
                    Name = "path",
                    HeaderText = "Image",
                    ImageLayout = DataGridViewImageCellLayout.Zoom,
                    DataPropertyName = "path",
                    Width = 300,
                    DefaultCellStyle = { NullValue = null } // NullValue를 null로 설정
                };

                dataGridView1.Columns.Insert(dataTable.Columns["path"].Ordinal, imgCol);
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

        private void dataGridView1_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            // 데이터 오류 처리 (필요한 경우 여기에 코드를 추가)
        }
    }
}
