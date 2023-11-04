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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace WindowsFormsApp1
{
    public partial class todo : Form
    {
        private Image backgroundImage;
        private Dictionary<DataGridView, bool> wasSelectedDict = new Dictionary<DataGridView, bool>();
        private const string ConnectionString = "Server=localhost;Port=3306;Database=testdb;Uid=root;Pwd=1234;";
        private bool wasSelected = false;
        private IEnumerable<Control> GetAllControls(Control container)
        {
            foreach (Control control in container.Controls)
            {
                yield return control;

                foreach (Control nestedControl in GetAllControls(control))
                {
                    yield return nestedControl;
                }
            }
        }

        public todo()
        {
            InitializeComponent();
           
            LoadDataToDataGridView(ISPNF01gd, "ISPNF01");
            LoadDataToDataGridView(ISPNF02gd, "ISPNF02");
            LoadDataToDataGridView(ISPNF03gd, "ISPNF03");
            LoadDataToDataGridView(ISPNF04gd, "ISPNF04");
            LoadDataToDataGridView(ISPNF05gd, "ISPNF05");
            LoadDataToDataGridView(ISPNF06gd, "ISPNF06");

            LoadDataToDataGridView(ISPNV01gd, "ISPNV01");
            LoadDataToDataGridView(ISPNV02gd, "ISPNV02");

            LoadDataToDataGridView(ISDMF01gd, "ISDMF01");
            LoadDataToDataGridView(ISDMF02gd, "ISDMF02");

            LoadDataToDataGridView(ISPNH01gd, "ISPNH01");
            LoadDataToDataGridView(ISPNH02gd, "ISPNH02");
            LoadDataToDataGridView(ISPNH03gd, "ISPNH03");
            LoadDataToDataGridView(ISPNH04gd, "ISPNH04");
            LoadDataToDataGridView(ISPNH05gd, "ISPNH05");
            LoadDataToDataGridView(ISPNH06gd, "ISPNH06");

            LoadDataToDataGridView(ISPXH01gd, "ISPXH01");
            LoadDataToDataGridView(ISPXH02gd, "ISPXH02");

            LoadDataToDataGridView(ISDMH01gd, "ISDMH01");


            this.Shown += Form1_Shown; // 이벤트 핸들러 추가

            
            foreach (var dgv in GetAllControls(this).OfType<DataGridView>())
            {
                dgv.CellClick += Dgv_CellClick;
                dgv.MouseDown += Dgv_MouseDown;
                wasSelectedDict[dgv] = false; // 초기 상태 설정
            }
            backgroundImage = Image.FromFile(@"C:\image.jpg");


            // DataGridView의 CellPainting 이벤트 핸들러를 추가
            //ISPNF01gd.CellPainting += ISPNF01gd_CellPainting;



            // 각 DataGridView의 CellPainting 이벤트 핸들러를 추가
            ISPNF01gd.CellPainting += DataGridView_CellPainting;
            ISPNF02gd.CellPainting += DataGridView_CellPainting;
            // ...
            ISPNF03gd.CellPainting += DataGridView_CellPainting;

        }


        private void DataGridView_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        {
            DataGridView dgv = sender as DataGridView;
            if (dgv == null) return;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                // 셀의 위치와 크기에 해당하는 이미지 부분을 추출
                Rectangle cellBounds = e.CellBounds;
                Rectangle imagePart = new Rectangle(cellBounds.X + dgv.HorizontalScrollingOffset,
                                                    cellBounds.Y + dgv.VerticalScrollingOffset,
                                                    cellBounds.Width,
                                                    cellBounds.Height);

                // 해당 부분을 셀에 그림
                e.Graphics.DrawImage(backgroundImage, cellBounds, imagePart, GraphicsUnit.Pixel);

                // 기본 셀 내용을 그림
                e.PaintContent(e.ClipBounds);
                e.Handled = true;
            }
        }




        //private void ISPNF01gd_CellPainting(object sender, DataGridViewCellPaintingEventArgs e)
        //{
        //    if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
        //    {
        //        // 셀의 위치와 크기에 해당하는 이미지 부분을 추출
        //        Rectangle cellBounds = e.CellBounds;
        //        Rectangle imagePart = new Rectangle(cellBounds.X + ISPNF01gd.HorizontalScrollingOffset,
        //                                            cellBounds.Y + ISPNF01gd.VerticalScrollingOffset,
        //                                            cellBounds.Width,
        //                                            cellBounds.Height);

        //        // 해당 부분을 셀에 그림
        //        e.Graphics.DrawImage(backgroundImage, cellBounds, imagePart, GraphicsUnit.Pixel);

        //        // 기본 셀 내용을 그림
        //        e.PaintContent(e.ClipBounds);
        //        e.Handled = true;
        //    }
        //}





        private void Dgv_MouseDown(object sender, MouseEventArgs e)
        {
            if (sender is DataGridView dgv)
            {
                var hitTest = dgv.HitTest(e.X, e.Y);
                if (hitTest.RowIndex >= 0 && hitTest.ColumnIndex >= 0)
                {
                    wasSelectedDict[dgv] = dgv[hitTest.ColumnIndex, hitTest.RowIndex].Selected;
                }
            }
        }

        private void Dgv_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0 || e.ColumnIndex < 0) // 헤더를 클릭한 경우는 무시
                return;

            if (sender is DataGridView dgv)
            {
                if (wasSelectedDict[dgv]) // 마우스 버튼이 눌렸을 때 셀이 선택된 상태였다면
                {
                    dgv.ClearSelection(); // 선택 해제
                }
                else
                {
                    dgv.ClearSelection(); // 기존 선택된 셀들을 해제
                    dgv[e.ColumnIndex, e.RowIndex].Selected = true; // 현재 셀 선택
                }
            }
        }
        private void Form1_reload(object sender, EventArgs e)
        {
            LoadDataToDataGridView(ISPNF01gd, "ISPNF01");
            LoadDataToDataGridView(ISPNF02gd, "ISPNF02");
            LoadDataToDataGridView(ISPNF03gd, "ISPNF03");
            LoadDataToDataGridView(ISPNF04gd, "ISPNF04");
            LoadDataToDataGridView(ISPNF05gd, "ISPNF05");
            LoadDataToDataGridView(ISPNF06gd, "ISPNF06");

            LoadDataToDataGridView(ISPNV01gd, "ISPNV01");
            LoadDataToDataGridView(ISPNV02gd, "ISPNV02");

            LoadDataToDataGridView(ISDMF01gd, "ISDMF01");
            LoadDataToDataGridView(ISDMF02gd, "ISDMF02");

            LoadDataToDataGridView(ISPNH01gd, "ISPNH01");
            LoadDataToDataGridView(ISPNH02gd, "ISPNH02");
            LoadDataToDataGridView(ISPNH03gd, "ISPNH03");
            LoadDataToDataGridView(ISPNH04gd, "ISPNH04");
            LoadDataToDataGridView(ISPNH05gd, "ISPNH05");
            LoadDataToDataGridView(ISPNH06gd, "ISPNH06");

            LoadDataToDataGridView(ISPXH01gd, "ISPXH01");
            LoadDataToDataGridView(ISPXH02gd, "ISPXH02");

            LoadDataToDataGridView(ISDMH01gd, "ISDMH01");

        }
        private void Form1_Shown(object sender, EventArgs e)
        {
            ISPNF01gd.ClearSelection();
            ISPNF02gd.ClearSelection();
            ISPNF03gd.ClearSelection();
            ISPNF04gd.ClearSelection();
            ISPNF05gd.ClearSelection();
            ISPNF06gd.ClearSelection();

            ISPNV01gd.ClearSelection();
            ISPNV02gd.ClearSelection();

            ISDMF01gd.ClearSelection();
            ISDMF02gd.ClearSelection();

            ISPNH01gd.ClearSelection();
            ISPNH02gd.ClearSelection();
            ISPNH03gd.ClearSelection();
            ISPNH04gd.ClearSelection();
            ISPNH05gd.ClearSelection();
            ISPNH06gd.ClearSelection();

            ISPXH01gd.ClearSelection();
            ISPXH02gd.ClearSelection();

            ISDMH01gd.ClearSelection();

            

        }
        private void LoadDataToDataGridView(DataGridView dgv, string eqpId)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter($"SELECT * FROM eqptodo WHERE eqpid = '{eqpId}' AND view = 'show'", connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgv.DataSource = dt;
                if (dgv.Columns.Contains("id"))
                {
                    dgv.Columns["id"].Visible = false;
                }
                    if (dgv.Columns.Contains("date"))
                {
                    dgv.Columns["date"].Visible = false;
                }
                if (dgv.Columns.Contains("view"))
                {
                    dgv.Columns["view"].Visible = false;
                }
                if (dgv.Columns.Contains("eqpid"))
                {
                    dgv.Columns["eqpid"].Visible = false;
                }
                if (dgv.Columns.Contains("todo"))
                {
                    dgv.Columns["todo"].Width = 295;
                }
                ISPNF01gd.DefaultCellStyle.BackColor = this.BackColor;

                ISPNF01gd.BackgroundImage = Image.FromFile("c:/image1.jpg");
                ISPNF01gd.BackgroundImageLayout = ImageLayout.Stretch; // 이미지를 DataGridView 크기에 맞게 늘립니다.
                
            }
        }

        private void InsertIntoDatabase(MySqlConnection connection, string eqpId, string textValue)
        {
            using (MySqlCommand command = new MySqlCommand())
            {
                command.Connection = connection;
                command.CommandText = "INSERT INTO eqptodo (eqpid, todo, view) VALUES (@eqpid, @todo, @view)";

                command.Parameters.AddWithValue("@eqpid", eqpId);
                command.Parameters.AddWithValue("@todo", textValue);
                command.Parameters.AddWithValue("@view", "show");

                command.ExecuteNonQuery();

            }
        }


        private void button2_Click(object sender, EventArgs e)
        {
            if (HasSelectedCells(ISPNF01gd)) UpdateDatabase(ISPNF01gd, "ISPNF01");
            if (HasSelectedCells(ISPNF02gd)) UpdateDatabase(ISPNF02gd, "ISPNF02");
            if (HasSelectedCells(ISPNF03gd)) UpdateDatabase(ISPNF03gd, "ISPNF03");
            if (HasSelectedCells(ISPNF04gd)) UpdateDatabase(ISPNF04gd, "ISPNF04");
            if (HasSelectedCells(ISPNF05gd)) UpdateDatabase(ISPNF05gd, "ISPNF05");
            if (HasSelectedCells(ISPNF06gd)) UpdateDatabase(ISPNF06gd, "ISPNF06");

            if (HasSelectedCells(ISPNV01gd)) UpdateDatabase(ISPNV01gd, "ISPNV01");
            if (HasSelectedCells(ISPNV02gd)) UpdateDatabase(ISPNV02gd, "ISPNV02");

            if (HasSelectedCells(ISDMF01gd)) UpdateDatabase(ISDMF01gd, "ISDMF01");
            if (HasSelectedCells(ISDMF02gd)) UpdateDatabase(ISDMF02gd, "ISDMF02");

            if (HasSelectedCells(ISPNH01gd)) UpdateDatabase(ISPNH01gd, "ISPNH01");
            if (HasSelectedCells(ISPNH02gd)) UpdateDatabase(ISPNH02gd, "ISPNH02");
            if (HasSelectedCells(ISPNH03gd)) UpdateDatabase(ISPNH03gd, "ISPNH03");
            if (HasSelectedCells(ISPNH04gd)) UpdateDatabase(ISPNH04gd, "ISPNH04");
            if (HasSelectedCells(ISPNH05gd)) UpdateDatabase(ISPNH05gd, "ISPNH05");
            if (HasSelectedCells(ISPNH06gd)) UpdateDatabase(ISPNH06gd, "ISPNH06");

            if (HasSelectedCells(ISPXH01gd)) UpdateDatabase(ISPXH01gd, "ISPXH01");
            if (HasSelectedCells(ISPXH02gd)) UpdateDatabase(ISPXH02gd, "ISPXH02");

            if (HasSelectedCells(ISDMH01gd)) UpdateDatabase(ISDMH01gd, "ISDMH01");

            Form1_reload(sender, e);
            Form1_Shown(sender, e);
        }
        private bool HasSelectedCells(DataGridView dgv)
        {
            return dgv.SelectedCells.Count > 0;
        }

        private void UpdateDatabase(DataGridView dgv, string eqpId)
        {
            foreach (DataGridViewCell cell in dgv.SelectedCells)
            {
                DataGridViewRow row = cell.OwningRow;
                string updateValue = Convert.ToString(row.Cells["view"].Value);
                int idValue = Convert.ToInt32(row.Cells["id"].Value); // id 컬럼의 값을 가져옵니다. "id"는 해당 컬럼의 이름이어야 합니다.
                if (!string.IsNullOrEmpty(updateValue))
                {
                    using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                    {
                        connection.Open();
                        MySqlCommand command = new MySqlCommand("UPDATE eqptodo SET view = 'comp' WHERE eqpid = @eqpid AND id = @id", connection);
                        command.Parameters.AddWithValue("@eqpid", eqpId);
                        command.Parameters.AddWithValue("@id", idValue);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
   
        private void DeleteDatabase(DataGridView dgv, string eqpId)
        {
            foreach (DataGridViewCell cell in dgv.SelectedCells)
            {
                DataGridViewRow row = cell.OwningRow;
                int idValue = Convert.ToInt32(row.Cells["id"].Value); // id 컬럼의 값을 가져옵니다. "id"는 해당 컬럼의 이름이어야 합니다.

                using (MySqlConnection connection = new MySqlConnection(ConnectionString))
                {
                    connection.Open();
                    MySqlCommand command = new MySqlCommand("UPDATE eqptodo SET view = 'delete' WHERE eqpid = @eqpid AND id = @id", connection);
                    command.Parameters.AddWithValue("@eqpid", eqpId);
                    command.Parameters.AddWithValue("@id", idValue);
                    command.ExecuteNonQuery();
                }
            }
        }


        private void button10_Click(object sender, EventArgs e)
        {
            ISPNF01chk.Checked = false;
            ISPNF02chk.Checked = false;
            ISPNF03chk.Checked = false;
            ISPNF04chk.Checked = false;
            ISPNF05chk.Checked = false;
            ISPNF06chk.Checked = false;

            ISPNV01chk.Checked = false;
            ISPNV02chk.Checked = false;

            ISDMF01chk.Checked = false;
            ISDMF02chk.Checked = false;

            ISPNH01chk.Checked = false;
            ISPNH02chk.Checked = false;
            ISPNH03chk.Checked = false;
            ISPNH04chk.Checked = false;
            ISPNH05chk.Checked = false;
            ISPNH06chk.Checked = false;

            ISPXH01chk.Checked = false;
            ISPXH02chk.Checked = false;

            ISDMH01chk.Checked = false;


        }

        private void button4_Click(object sender, EventArgs e)
        {
            ISPNF01chk.Checked = true;
            ISPNF02chk.Checked = true;
            ISPNF03chk.Checked = true;
            ISPNF04chk.Checked = true;
            ISPNF05chk.Checked = true;
            ISPNF06chk.Checked = true;

            ISPNV01chk.Checked = true;
            ISPNV02chk.Checked = true;

            ISDMF01chk.Checked = true;
            ISDMF02chk.Checked = true;

            ISPNH01chk.Checked = true;
            ISPNH02chk.Checked = true;
            ISPNH03chk.Checked = true;
            ISPNH04chk.Checked = true;
            ISPNH05chk.Checked = true;
            ISPNH06chk.Checked = true;

            ISPXH01chk.Checked = true;
            ISPXH02chk.Checked = true;

            ISDMH01chk.Checked = true;
        }

        private void button5_Click(object sender, EventArgs e)
        {
            ISPNF01chk.Checked = true;
            ISPNF02chk.Checked = true;
            ISPNF03chk.Checked = true;
            ISPNF04chk.Checked = true;
            ISPNF05chk.Checked = true;
            ISPNF06chk.Checked = true;

            ISDMF01chk.Checked = true;
            ISDMF02chk.Checked = true;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            ISPNV01chk.Checked = true;
            ISPNV02chk.Checked = true;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            ISPNH01chk.Checked = true;
            ISPNH02chk.Checked = true;
            ISPNH03chk.Checked = true;
            ISPNH04chk.Checked = true;
            ISPNH05chk.Checked = true;
            ISPNH06chk.Checked = true;

            ISPXH01chk.Checked = true;
            ISPXH02chk.Checked = true;

            ISDMH01chk.Checked = true;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            ISPNF01chk.Checked = true;
            ISPNF02chk.Checked = true;
            ISPNF03chk.Checked = true;
            ISPNF04chk.Checked = true;
            ISPNF05chk.Checked = true;
            ISPNF06chk.Checked = true;

            ISPNV01chk.Checked = true;
            ISPNV02chk.Checked = true;

            ISPNH01chk.Checked = true;
            ISPNH02chk.Checked = true;
            ISPNH03chk.Checked = true;
            ISPNH04chk.Checked = true;
            ISPNH05chk.Checked = true;
            ISPNH06chk.Checked = true;

        }

        private void button7_Click(object sender, EventArgs e)
        {
            ISDMF01chk.Checked = true;
            ISDMF02chk.Checked = true;

            ISDMH01chk.Checked = true;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            ISPXH01chk.Checked = true;
            ISPXH02chk.Checked = true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();

                string textBoxValue = textBox1.Text;

                if (ISPNF01chk.Checked)
                {
                    InsertIntoDatabase(connection, "ISPNF01", textBoxValue);
                    LoadDataToDataGridView(ISPNF01gd, "ISPNF01"); // 데이터 다시 로드
                    ISPNF01gd.ClearSelection();
                }
                if (ISPNF02chk.Checked)
                {
                    InsertIntoDatabase(connection, "ISPNF02", textBoxValue);
                    LoadDataToDataGridView(ISPNF02gd, "ISPNF02"); // 데이터 다시 로드
                    ISPNF02gd.ClearSelection();
                }
                if (ISPNF03chk.Checked)
                {
                    InsertIntoDatabase(connection, "ISPNF03", textBoxValue);
                    LoadDataToDataGridView(ISPNF03gd, "ISPNF03"); // 데이터 다시 로드
                    ISPNF03gd.ClearSelection();
                }
                if (ISPNF04chk.Checked)
                {
                    InsertIntoDatabase(connection, "ISPNF04", textBoxValue);
                    LoadDataToDataGridView(ISPNF04gd, "ISPNF04"); // 데이터 다시 로드
                    ISPNF04gd.ClearSelection();
                }
                if (ISPNF05chk.Checked)
                {
                    InsertIntoDatabase(connection, "ISPNF05", textBoxValue);
                    LoadDataToDataGridView(ISPNF05gd, "ISPNF05"); // 데이터 다시 로드
                    ISPNF05gd.ClearSelection();
                }
                if (ISPNF06chk.Checked)
                {
                    InsertIntoDatabase(connection, "ISPNF06", textBoxValue);
                    LoadDataToDataGridView(ISPNF06gd, "ISPNF06"); // 데이터 다시 로드
                    ISPNF06gd.ClearSelection();
                }


                if (ISPNV01chk.Checked)
                {
                    InsertIntoDatabase(connection, "ISPNV01", textBoxValue);
                    LoadDataToDataGridView(ISPNV01gd, "ISPNV01"); // 데이터 다시 로드
                    ISPNV01gd.ClearSelection();
                }
                if (ISPNV02chk.Checked)
                {
                    InsertIntoDatabase(connection, "ISPNV02", textBoxValue);
                    LoadDataToDataGridView(ISPNV02gd, "ISPNV02"); // 데이터 다시 로드
                    ISPNV02gd.ClearSelection();
                }



                if (ISDMF01chk.Checked)
                {
                    InsertIntoDatabase(connection, "ISDMF01", textBoxValue);
                    LoadDataToDataGridView(ISDMF01gd, "ISDMF01"); // 데이터 다시 로드
                    ISDMF01gd.ClearSelection();
                }
                if (ISDMF02chk.Checked)
                {
                    InsertIntoDatabase(connection, "ISDMF02", textBoxValue);
                    LoadDataToDataGridView(ISDMF02gd, "ISDMF02"); // 데이터 다시 로드
                    ISDMF02gd.ClearSelection();
                }



                if (ISPNH01chk.Checked)
                {
                    InsertIntoDatabase(connection, "ISPNH01", textBoxValue);
                    LoadDataToDataGridView(ISPNH01gd, "ISPNH01"); // 데이터 다시 로드
                    ISPNH01gd.ClearSelection();
                }
                if (ISPNH02chk.Checked)
                {
                    InsertIntoDatabase(connection, "ISPNH02", textBoxValue);
                    LoadDataToDataGridView(ISPNH02gd, "ISPNH02"); // 데이터 다시 로드
                    ISPNH02gd.ClearSelection();
                }
                if (ISPNH03chk.Checked)
                {
                    InsertIntoDatabase(connection, "ISPNH03", textBoxValue);
                    LoadDataToDataGridView(ISPNH03gd, "ISPNH03"); // 데이터 다시 로드
                    ISPNH03gd.ClearSelection();
                }
                if (ISPNH04chk.Checked)
                {
                    InsertIntoDatabase(connection, "ISPNH04", textBoxValue);
                    LoadDataToDataGridView(ISPNH04gd, "ISPNH04"); // 데이터 다시 로드
                    ISPNH04gd.ClearSelection();
                }
                if (ISPNH05chk.Checked)
                {
                    InsertIntoDatabase(connection, "ISPNH05", textBoxValue);
                    LoadDataToDataGridView(ISPNH05gd, "ISPNH05"); // 데이터 다시 로드
                    ISPNH05gd.ClearSelection();
                }
                if (ISPNH06chk.Checked)
                {
                    InsertIntoDatabase(connection, "ISPNH06", textBoxValue);
                    LoadDataToDataGridView(ISPNH06gd, "ISPNH06"); // 데이터 다시 로드
                    ISPNH06gd.ClearSelection();
                }

                if (ISPXH01chk.Checked)
                {
                    InsertIntoDatabase(connection, "ISPXH01", textBoxValue);
                    LoadDataToDataGridView(ISPXH01gd, "ISPXH01"); // 데이터 다시 로드
                    ISPXH01gd.ClearSelection();
                }
                if (ISPXH02chk.Checked)
                {
                    InsertIntoDatabase(connection, "ISPXH02", textBoxValue);
                    LoadDataToDataGridView(ISPXH02gd, "ISPXH02"); // 데이터 다시 로드
                    ISPXH02gd.ClearSelection();
                }
                if (ISDMH01chk.Checked)
                {
                    InsertIntoDatabase(connection, "ISDMH01", textBoxValue);
                    LoadDataToDataGridView(ISDMH01gd, "ISDMH01"); // 데이터 다시 로드
                    ISDMH01gd.ClearSelection();
                }
            }
            //MessageBox.Show("데이터가 DB에 저장되었습니다.");
        }

        private void button3_Click(object sender, EventArgs e)
        {

            if (HasSelectedCells(ISPNF01gd)) DeleteDatabase(ISPNF01gd, "ISPNF01");
            if (HasSelectedCells(ISPNF02gd)) DeleteDatabase(ISPNF02gd, "ISPNF02");
            if (HasSelectedCells(ISPNF03gd)) DeleteDatabase(ISPNF03gd, "ISPNF03");
            if (HasSelectedCells(ISPNF04gd)) DeleteDatabase(ISPNF04gd, "ISPNF04");
            if (HasSelectedCells(ISPNF05gd)) DeleteDatabase(ISPNF05gd, "ISPNF05");
            if (HasSelectedCells(ISPNF06gd)) DeleteDatabase(ISPNF06gd, "ISPNF06");

            if (HasSelectedCells(ISPNV01gd)) DeleteDatabase(ISPNV01gd, "ISPNV01");
            if (HasSelectedCells(ISPNV02gd)) DeleteDatabase(ISPNV02gd, "ISPNV02");

            if (HasSelectedCells(ISDMF01gd)) DeleteDatabase(ISDMF01gd, "ISDMF01");
            if (HasSelectedCells(ISDMF02gd)) DeleteDatabase(ISDMF02gd, "ISDMF02");

            if (HasSelectedCells(ISPNH01gd)) DeleteDatabase(ISPNH01gd, "ISPNH01");
            if (HasSelectedCells(ISPNH02gd)) DeleteDatabase(ISPNH02gd, "ISPNH02");
            if (HasSelectedCells(ISPNH03gd)) DeleteDatabase(ISPNH03gd, "ISPNH03");
            if (HasSelectedCells(ISPNH04gd)) DeleteDatabase(ISPNH04gd, "ISPNH04");
            if (HasSelectedCells(ISPNH05gd)) DeleteDatabase(ISPNH05gd, "ISPNH05");
            if (HasSelectedCells(ISPNH06gd)) DeleteDatabase(ISPNH06gd, "ISPNH06");

            if (HasSelectedCells(ISPXH01gd)) DeleteDatabase(ISPXH01gd, "ISPXH01");
            if (HasSelectedCells(ISPXH02gd)) DeleteDatabase(ISPXH02gd, "ISPXH02");

            if (HasSelectedCells(ISDMH01gd)) DeleteDatabase(ISDMH01gd, "ISDMH01");

            Form1_reload(sender, e);
            Form1_Shown(sender, e);

        }

    }
    
}
