using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace 이미지_텟스트
{
    public partial class Form1 : Form
    {
        private PictureBox pictureBox;
        private MySqlConnection connection;
        private string connectionString = "Server=localhost;Port=3306;Database=testdb;Uid=root;Pwd=1234;";

        public Form1()
        {
            InitializeComponent();

            pictureBox = new PictureBox
            {
                Dock = DockStyle.Fill
            };
            Controls.Add(pictureBox);

            LoadImageFromDatabase();
        }

        private void LoadImageFromDatabase()
        {
            try
            {
                connection = new MySqlConnection(connectionString);
                connection.Open();

                string query = "SELECT path FROM data";
                MySqlCommand cmd = new MySqlCommand(query, connection);
                string imagePath = cmd.ExecuteScalar().ToString();

                pictureBox.ImageLocation = imagePath;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                if (connection != null)
                    connection.Close();
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
