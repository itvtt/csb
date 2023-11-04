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

namespace DB
{
    public partial class Form1 : Form
    {
        private Form imageForm = null; // 이미지를 보여주는 폼
        private const int ImagePadding = 50; // 여백 크기

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadData("data");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            LoadData("data0");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            List<string> selectedImagePaths = new List<string>();

            // DataGridView에서 선택된 셀이 속한 행의 이미지 파일 경로를 수집
            foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
            {
                int rowIndex = cell.RowIndex;
                string imagePath = dataGridView1.Rows[rowIndex].Cells["image"].Value as string;

                if (!string.IsNullOrEmpty(imagePath) && !selectedImagePaths.Contains(imagePath))
                {
                    selectedImagePaths.Add(imagePath);
                }
            }

            // 선택된 이미지가 있을 경우에만 표시
            if (selectedImagePaths.Count > 0)
            {
                ShowMultipleImages(selectedImagePaths);
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

                // 데이터 조회 쿼리
                string query = $"SELECT * FROM {tableName}";
                MySqlCommand cmd = new MySqlCommand(query, connection);

                MySqlDataAdapter adapter = new MySqlDataAdapter(cmd);
                DataTable dataTable = new DataTable();

                adapter.Fill(dataTable);

                // DataGridView에 데이터를 설정
                dataGridView1.DataSource = dataTable;

                // DataGridView 더블 클릭 이벤트 핸들러 추가
                dataGridView1.DoubleClick += DataGridView1_DoubleClick;
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

        private void DataGridView1_DoubleClick(object sender, EventArgs e)
        {
            if (dataGridView1.SelectedCells.Count > 0)
            {
                int rowIndex = dataGridView1.SelectedCells[0].RowIndex;
                string imagePath = dataGridView1.Rows[rowIndex].Cells["image"].Value as string;

                if (!string.IsNullOrEmpty(imagePath))
                {
                    if (imageForm == null || imageForm.IsDisposed)
                    {
                        imageForm = new Form();
                        imageForm.Text = "이미지 창";
                        imageForm.StartPosition = FormStartPosition.CenterScreen;

                        Panel panel = new Panel();
                        panel.Dock = DockStyle.Fill;
                        panel.AutoScroll = true;

                        PictureBox pictureBox = new PictureBox();
                        pictureBox.Image = Image.FromFile(imagePath);
                        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

                        int desiredWidth = 400;  // 원하는 이미지 너비
                        int desiredHeight = 400; // 원하는 이미지 높이
                        pictureBox.Size = new Size(desiredWidth, desiredHeight);

                        panel.Controls.Add(pictureBox);
                        imageForm.Controls.Add(panel);

                        // 이미지 폼 Shown 이벤트 핸들러 추가
                        imageForm.Shown += (s, args) =>
                        {
                            // 이미지를 패널 중앙에 정렬
                            int posX = (panel.Width - desiredWidth) / 2;
                            int posY = (panel.Height - desiredHeight) / 2;
                            pictureBox.Location = new Point(posX, posY);
                        };

                        imageForm.Size = new System.Drawing.Size(desiredWidth + ImagePadding, desiredHeight + ImagePadding);
                    }
                    else
                    {
                        // ... (기존의 다른 이미지 추가 코드는 그대로 유지)
                    }

                    if (!imageForm.Visible)
                    {
                        imageForm.Show();
                    }
                }
            }
        }



        private void ShowMultipleImages(List<string> imagePaths)
        {
            if (imagePaths.Count == 0)
            {
                return;
            }

            Form multipleImagesForm = new Form();
            multipleImagesForm.Text = "여러 이미지";
            multipleImagesForm.StartPosition = FormStartPosition.CenterScreen;

            Panel panel = new Panel();
            panel.Dock = DockStyle.Fill;
            panel.AutoScroll = true;

            int desiredWidth = 300; // 원하는 이미지 너비
            int desiredHeight = 300; // 원하는 이미지 높이
            int imagesPerRow = 4; // 한 행에 표시될 이미지 수

            int currentX = 0; // 현재 x 위치
            int currentY = 0; // 현재 y 위치
            int count = 0; // 현재 행에 추가된 이미지 수

            foreach (string imagePath in imagePaths)
            {
                PictureBox pictureBox = new PictureBox();
                pictureBox.Image = Image.FromFile(imagePath);
                pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                pictureBox.Size = new Size(desiredWidth, desiredHeight);

                pictureBox.Location = new Point(currentX, currentY);
                panel.Controls.Add(pictureBox);

                count++;
                if (count == imagesPerRow)
                {
                    // 한 행에 이미지가 imagesPerRow만큼 추가되면 위치를 조정
                    currentY += desiredHeight;
                    currentX = 0;
                    count = 0;
                }
                else
                {
                    currentX += desiredWidth;
                }
            }

            multipleImagesForm.Controls.Add(panel);
            multipleImagesForm.Size = new Size(imagesPerRow * desiredWidth + ImagePadding, Math.Min((imagePaths.Count / imagesPerRow + 1) * desiredHeight + ImagePadding, 600));

            multipleImagesForm.ShowDialog();
        }


    }
}