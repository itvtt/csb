using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace 그리드뷰_배경에_이미지_넣기
{
    public partial class Form1 : Form
    {
        private Image backgroundImage;
        public Form1()
        {
            InitializeComponent();
            // 지정한 경로에서 이미지 로드
            backgroundImage = Image.FromFile(@"C:\image1.jpg");

            // DataGridView의 Paint 이벤트 핸들러를 추가
            dataGridView1.Paint += DataGridView1_Paint;
        }

        private void DataGridView1_Paint(object sender, PaintEventArgs e)
        {
            if (backgroundImage != null)
            {
                // 이미지를 DataGridView의 배경에 그립니다.
                e.Graphics.DrawImage(backgroundImage, new Rectangle(0, 0, dataGridView1.Width, dataGridView1.Height));
            }
        }
    }
}