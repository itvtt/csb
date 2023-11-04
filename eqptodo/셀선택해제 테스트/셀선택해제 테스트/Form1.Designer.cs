namespace 셀선택해제_테스트
{
    partial class Form1
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.ISPNF01gd = new System.Windows.Forms.DataGridView();
            this.ISPNF02gd = new System.Windows.Forms.DataGridView();
            ((System.ComponentModel.ISupportInitialize)(this.ISPNF01gd)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ISPNF02gd)).BeginInit();
            this.SuspendLayout();
            // 
            // ISPNF01gd
            // 
            this.ISPNF01gd.AllowUserToAddRows = false;
            this.ISPNF01gd.AllowUserToDeleteRows = false;
            this.ISPNF01gd.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ISPNF01gd.ColumnHeadersVisible = false;
            this.ISPNF01gd.Location = new System.Drawing.Point(12, 12);
            this.ISPNF01gd.Name = "ISPNF01gd";
            this.ISPNF01gd.ReadOnly = true;
            this.ISPNF01gd.RowHeadersVisible = false;
            this.ISPNF01gd.RowTemplate.Height = 23;
            this.ISPNF01gd.Size = new System.Drawing.Size(598, 324);
            this.ISPNF01gd.TabIndex = 0;
            // 
            // ISPNF02gd
            // 
            this.ISPNF02gd.AllowUserToAddRows = false;
            this.ISPNF02gd.AllowUserToDeleteRows = false;
            this.ISPNF02gd.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.ISPNF02gd.ColumnHeadersVisible = false;
            this.ISPNF02gd.Location = new System.Drawing.Point(616, 259);
            this.ISPNF02gd.Name = "ISPNF02gd";
            this.ISPNF02gd.ReadOnly = true;
            this.ISPNF02gd.RowHeadersVisible = false;
            this.ISPNF02gd.RowTemplate.Height = 23;
            this.ISPNF02gd.Size = new System.Drawing.Size(598, 324);
            this.ISPNF02gd.TabIndex = 1;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1226, 595);
            this.Controls.Add(this.ISPNF02gd);
            this.Controls.Add(this.ISPNF01gd);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.ISPNF01gd)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ISPNF02gd)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView ISPNF01gd;
        private System.Windows.Forms.DataGridView ISPNF02gd;
    }
}

