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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace 셀선택해제_테스트
{
    public partial class Form1 : Form
    {
        private Dictionary<DataGridView, bool> wasSelectedDict = new Dictionary<DataGridView, bool>();
        private const string ConnectionString = "Server=localhost;Port=3306;Database=testdb;Uid=root;Pwd=1234;";
        private bool wasSelected = false;

        public Form1()
        {
            InitializeComponent();
            LoadDataToDataGridView(ISPNF01gd, "AAAAA01");
            LoadDataToDataGridView(ISPNF02gd, "AAAAA02");
            this.Shown += Form1_Shown; // 이벤트 핸들러 추가
            // 모든 DataGridView 컨트롤에 대해 이벤트 핸들러 설정
            foreach (var dgv in this.Controls.OfType<DataGridView>())
            {
                dgv.CellClick += Dgv_CellClick;
                dgv.MouseDown += Dgv_MouseDown;
                wasSelectedDict[dgv] = false; // 초기 상태 설정
            }
        }

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
        private void Form1_Shown(object sender, EventArgs e)
        {
            ISPNF01gd.ClearSelection();
            ISPNF02gd.ClearSelection();

        }
        private void LoadDataToDataGridView(DataGridView dgv, string eqpId)
        {
            using (MySqlConnection connection = new MySqlConnection(ConnectionString))
            {
                connection.Open();
                MySqlDataAdapter adapter = new MySqlDataAdapter($"SELECT * FROM eqptodo WHERE eqpid = '{eqpId}'", connection);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                dgv.DataSource = dt;
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

            }
        }

        private bool HasSelectedCells(DataGridView dgv)
        {
            return dgv.SelectedCells.Count > 0;
        }

    }
}
