namespace picMatrix
{
    partial class FormPicMatrix
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormPicMatrix));
            this.saveFileDialogPicMatrix = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialogPicMatrix = new System.Windows.Forms.OpenFileDialog();
            this.pictureBoxCurrentPic = new System.Windows.Forms.PictureBox();
            this.buttonOpen = new System.Windows.Forms.Button();
            this.checkBoxBinFormat = new System.Windows.Forms.CheckBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.progressBarSaving = new System.Windows.Forms.ProgressBar();
            this.pictureBoxBKG = new System.Windows.Forms.PictureBox();
            this.richTextBoxOpenPaths = new System.Windows.Forms.RichTextBox();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.buttonPrevious = new System.Windows.Forms.Button();
            this.buttonNext = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCurrentPic)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBKG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // saveFileDialogPicMatrix
            // 
            this.saveFileDialogPicMatrix.FileName = "PicturePack";
            this.saveFileDialogPicMatrix.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialogPicMatrix_FileOk);
            // 
            // openFileDialogPicMatrix
            // 
            this.openFileDialogPicMatrix.FileOk += new System.ComponentModel.CancelEventHandler(this.openFileDialogPicMatrix_FileOk);
            // 
            // pictureBoxCurrentPic
            // 
            this.pictureBoxCurrentPic.BackColor = System.Drawing.Color.Black;
            this.pictureBoxCurrentPic.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxCurrentPic.Image")));
            this.pictureBoxCurrentPic.Location = new System.Drawing.Point(12, 12);
            this.pictureBoxCurrentPic.Name = "pictureBoxCurrentPic";
            this.pictureBoxCurrentPic.Size = new System.Drawing.Size(480, 272);
            this.pictureBoxCurrentPic.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.pictureBoxCurrentPic.TabIndex = 0;
            this.pictureBoxCurrentPic.TabStop = false;
            this.pictureBoxCurrentPic.Click += new System.EventHandler(this.pictureBoxCurrentPic_Click);
            // 
            // buttonOpen
            // 
            this.buttonOpen.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonOpen.Location = new System.Drawing.Point(11, 288);
            this.buttonOpen.Name = "buttonOpen";
            this.buttonOpen.Size = new System.Drawing.Size(63, 27);
            this.buttonOpen.TabIndex = 2;
            this.buttonOpen.Text = "FILES";
            this.buttonOpen.UseVisualStyleBackColor = true;
            this.buttonOpen.Click += new System.EventHandler(this.buttonOpen_Click);
            // 
            // checkBoxBinFormat
            // 
            this.checkBoxBinFormat.AutoSize = true;
            this.checkBoxBinFormat.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.checkBoxBinFormat.Location = new System.Drawing.Point(150, 296);
            this.checkBoxBinFormat.Name = "checkBoxBinFormat";
            this.checkBoxBinFormat.Size = new System.Drawing.Size(86, 21);
            this.checkBoxBinFormat.TabIndex = 4;
            this.checkBoxBinFormat.Text = "MonoCFile";
            this.checkBoxBinFormat.UseVisualStyleBackColor = true;
            this.checkBoxBinFormat.CheckedChanged += new System.EventHandler(this.checkBoxBinFormat_CheckedChanged);
            // 
            // buttonStart
            // 
            this.buttonStart.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonStart.Location = new System.Drawing.Point(78, 288);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(63, 27);
            this.buttonStart.TabIndex = 2;
            this.buttonStart.Text = "START";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // progressBarSaving
            // 
            this.progressBarSaving.Location = new System.Drawing.Point(-6, 427);
            this.progressBarSaving.Name = "progressBarSaving";
            this.progressBarSaving.Size = new System.Drawing.Size(516, 23);
            this.progressBarSaving.TabIndex = 5;
            this.progressBarSaving.Visible = false;
            this.progressBarSaving.Click += new System.EventHandler(this.progressBarSaving_Click);
            // 
            // pictureBoxBKG
            // 
            this.pictureBoxBKG.BackColor = System.Drawing.Color.Green;
            this.pictureBoxBKG.Location = new System.Drawing.Point(11, 11);
            this.pictureBoxBKG.Name = "pictureBoxBKG";
            this.pictureBoxBKG.Size = new System.Drawing.Size(482, 274);
            this.pictureBoxBKG.TabIndex = 7;
            this.pictureBoxBKG.TabStop = false;
            // 
            // richTextBoxOpenPaths
            // 
            this.richTextBoxOpenPaths.Location = new System.Drawing.Point(12, 319);
            this.richTextBoxOpenPaths.Name = "richTextBoxOpenPaths";
            this.richTextBoxOpenPaths.ReadOnly = true;
            this.richTextBoxOpenPaths.Size = new System.Drawing.Size(480, 99);
            this.richTextBoxOpenPaths.TabIndex = 8;
            this.richTextBoxOpenPaths.Text = "";
            this.richTextBoxOpenPaths.TextChanged += new System.EventHandler(this.richTextBoxOpenPaths_TextChanged);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.Green;
            this.pictureBox1.Location = new System.Drawing.Point(11, 318);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(482, 101);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // buttonPrevious
            // 
            this.buttonPrevious.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonPrevious.Location = new System.Drawing.Point(422, 288);
            this.buttonPrevious.Name = "buttonPrevious";
            this.buttonPrevious.Size = new System.Drawing.Size(36, 27);
            this.buttonPrevious.TabIndex = 10;
            this.buttonPrevious.Text = "<<";
            this.buttonPrevious.UseVisualStyleBackColor = true;
            this.buttonPrevious.Click += new System.EventHandler(this.buttonPrevious_Click);
            // 
            // buttonNext
            // 
            this.buttonNext.Font = new System.Drawing.Font("Calibri", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.buttonNext.Location = new System.Drawing.Point(458, 288);
            this.buttonNext.Name = "buttonNext";
            this.buttonNext.Size = new System.Drawing.Size(36, 27);
            this.buttonNext.TabIndex = 10;
            this.buttonNext.Text = ">>";
            this.buttonNext.UseVisualStyleBackColor = true;
            this.buttonNext.Click += new System.EventHandler(this.buttonNext_Click);
            // 
            // FormPicMatrix
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(504, 430);
            this.Controls.Add(this.buttonNext);
            this.Controls.Add(this.buttonPrevious);
            this.Controls.Add(this.progressBarSaving);
            this.Controls.Add(this.checkBoxBinFormat);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.buttonOpen);
            this.Controls.Add(this.pictureBoxCurrentPic);
            this.Controls.Add(this.pictureBoxBKG);
            this.Controls.Add(this.richTextBoxOpenPaths);
            this.Controls.Add(this.pictureBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "FormPicMatrix";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "PicMatrix V0.1";
            this.Load += new System.EventHandler(this.FormPicMatrix_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxCurrentPic)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBKG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.SaveFileDialog saveFileDialogPicMatrix;
        private System.Windows.Forms.OpenFileDialog openFileDialogPicMatrix;
        private System.Windows.Forms.PictureBox pictureBoxCurrentPic;
        private System.Windows.Forms.Button buttonOpen;
        private System.Windows.Forms.CheckBox checkBoxBinFormat;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.ProgressBar progressBarSaving;
        private System.Windows.Forms.PictureBox pictureBoxBKG;
        private System.Windows.Forms.RichTextBox richTextBoxOpenPaths;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button buttonPrevious;
        private System.Windows.Forms.Button buttonNext;
    }
}

