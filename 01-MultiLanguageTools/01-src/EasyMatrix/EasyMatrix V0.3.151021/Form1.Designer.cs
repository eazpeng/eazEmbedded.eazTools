namespace EasyMatrix_V0._3._151021._1
{
    partial class FormEasyMatrix
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
            this.pictureBoxViewer = new System.Windows.Forms.PictureBox();
            this.progressBarSaving = new System.Windows.Forms.ProgressBar();
            this.richTextBoxRandomHexDatas = new System.Windows.Forms.RichTextBox();
            this.buttonSwitchTab = new System.Windows.Forms.Button();
            this.buttonUp = new System.Windows.Forms.Button();
            this.buttonDown = new System.Windows.Forms.Button();
            this.buttonLeft = new System.Windows.Forms.Button();
            this.buttonRight = new System.Windows.Forms.Button();
            this.textBoxInput = new System.Windows.Forms.TextBox();
            this.buttonFontSetting = new System.Windows.Forms.Button();
            this.buttonOutputFile = new System.Windows.Forms.Button();
            this.checkBoxRandomMode = new System.Windows.Forms.CheckBox();
            this.comboBoxCharSet = new System.Windows.Forms.ComboBox();
            this.labelFontSizePx = new System.Windows.Forms.Label();
            this.labelCharSet = new System.Windows.Forms.Label();
            this.checkBoxSingleCharOrString = new System.Windows.Forms.CheckBox();
            this.buttonLastChar = new System.Windows.Forms.Button();
            this.buttonNextChar = new System.Windows.Forms.Button();
            this.buttonInsertFile = new System.Windows.Forms.Button();
            this.labelFlashAddr = new System.Windows.Forms.Label();
            this.numericUpDownFlashAddr = new System.Windows.Forms.NumericUpDown();
            this.pictureBoxDrawingPicture = new System.Windows.Forms.PictureBox();
            this.numericUpDownXPos = new System.Windows.Forms.NumericUpDown();
            this.numericUpDownYPos = new System.Windows.Forms.NumericUpDown();
            this.labelXPos = new System.Windows.Forms.Label();
            this.labelYPos = new System.Windows.Forms.Label();
            this.pictureBoxBKG = new System.Windows.Forms.PictureBox();
            this.labelCurrentChar = new System.Windows.Forms.Label();
            this.saveFileDialogSaving = new System.Windows.Forms.SaveFileDialog();
            this.pictureBox_DrawChar = new System.Windows.Forms.PictureBox();
            this.pictureBoxRedisp = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxViewer)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFlashAddr)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDrawingPicture)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownXPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYPos)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBKG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_DrawChar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRedisp)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxViewer
            // 
            this.pictureBoxViewer.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.pictureBoxViewer.Location = new System.Drawing.Point(12, 35);
            this.pictureBoxViewer.Name = "pictureBoxViewer";
            this.pictureBoxViewer.Size = new System.Drawing.Size(562, 425);
            this.pictureBoxViewer.TabIndex = 0;
            this.pictureBoxViewer.TabStop = false;
            this.pictureBoxViewer.Click += new System.EventHandler(this.pictureBoxViewer_Click);
            // 
            // progressBarSaving
            // 
            this.progressBarSaving.ForeColor = System.Drawing.Color.LawnGreen;
            this.progressBarSaving.Location = new System.Drawing.Point(-11, 648);
            this.progressBarSaving.Name = "progressBarSaving";
            this.progressBarSaving.Size = new System.Drawing.Size(609, 15);
            this.progressBarSaving.TabIndex = 1;
            this.progressBarSaving.Visible = false;
            this.progressBarSaving.Click += new System.EventHandler(this.progressBarSaving_Click);
            // 
            // richTextBoxRandomHexDatas
            // 
            this.richTextBoxRandomHexDatas.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.richTextBoxRandomHexDatas.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.richTextBoxRandomHexDatas.Font = new System.Drawing.Font("Consolas", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBoxRandomHexDatas.Location = new System.Drawing.Point(13, 36);
            this.richTextBoxRandomHexDatas.Name = "richTextBoxRandomHexDatas";
            this.richTextBoxRandomHexDatas.ReadOnly = true;
            this.richTextBoxRandomHexDatas.Size = new System.Drawing.Size(559, 423);
            this.richTextBoxRandomHexDatas.TabIndex = 2;
            this.richTextBoxRandomHexDatas.Text = "";
            this.richTextBoxRandomHexDatas.TextChanged += new System.EventHandler(this.richTextBoxRandomHexDatas_TextChanged);
            // 
            // buttonSwitchTab
            // 
            this.buttonSwitchTab.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonSwitchTab.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonSwitchTab.Location = new System.Drawing.Point(11, 9);
            this.buttonSwitchTab.Name = "buttonSwitchTab";
            this.buttonSwitchTab.Size = new System.Drawing.Size(91, 23);
            this.buttonSwitchTab.TabIndex = 3;
            this.buttonSwitchTab.Text = "看图窗口";
            this.buttonSwitchTab.UseVisualStyleBackColor = true;
            this.buttonSwitchTab.Click += new System.EventHandler(this.buttonSwitchTab_Click);
            // 
            // buttonUp
            // 
            this.buttonUp.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonUp.Location = new System.Drawing.Point(394, 9);
            this.buttonUp.Name = "buttonUp";
            this.buttonUp.Size = new System.Drawing.Size(46, 23);
            this.buttonUp.TabIndex = 4;
            this.buttonUp.Text = "上";
            this.buttonUp.UseVisualStyleBackColor = false;
            this.buttonUp.Visible = false;
            this.buttonUp.Click += new System.EventHandler(this.buttonUp_Click);
            // 
            // buttonDown
            // 
            this.buttonDown.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonDown.Location = new System.Drawing.Point(439, 9);
            this.buttonDown.Name = "buttonDown";
            this.buttonDown.Size = new System.Drawing.Size(46, 23);
            this.buttonDown.TabIndex = 4;
            this.buttonDown.Text = "下";
            this.buttonDown.UseVisualStyleBackColor = true;
            this.buttonDown.Visible = false;
            this.buttonDown.Click += new System.EventHandler(this.buttonDown_Click);
            // 
            // buttonLeft
            // 
            this.buttonLeft.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonLeft.Location = new System.Drawing.Point(484, 9);
            this.buttonLeft.Name = "buttonLeft";
            this.buttonLeft.Size = new System.Drawing.Size(46, 23);
            this.buttonLeft.TabIndex = 4;
            this.buttonLeft.Text = "左";
            this.buttonLeft.UseVisualStyleBackColor = true;
            this.buttonLeft.Visible = false;
            this.buttonLeft.Click += new System.EventHandler(this.buttonLeft_Click);
            // 
            // buttonRight
            // 
            this.buttonRight.Font = new System.Drawing.Font("黑体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonRight.Location = new System.Drawing.Point(529, 9);
            this.buttonRight.Name = "buttonRight";
            this.buttonRight.Size = new System.Drawing.Size(46, 23);
            this.buttonRight.TabIndex = 4;
            this.buttonRight.Text = "右";
            this.buttonRight.UseVisualStyleBackColor = true;
            this.buttonRight.Visible = false;
            this.buttonRight.Click += new System.EventHandler(this.buttonRight_Click);
            // 
            // textBoxInput
            // 
            this.textBoxInput.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.textBoxInput.ForeColor = System.Drawing.Color.MidnightBlue;
            this.textBoxInput.Location = new System.Drawing.Point(11, 491);
            this.textBoxInput.Multiline = true;
            this.textBoxInput.Name = "textBoxInput";
            this.textBoxInput.Size = new System.Drawing.Size(421, 150);
            this.textBoxInput.TabIndex = 6;
            this.textBoxInput.Text = "HedyMed";
            this.textBoxInput.TextChanged += new System.EventHandler(this.textBoxInput_TextChanged);
            // 
            // buttonFontSetting
            // 
            this.buttonFontSetting.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonFontSetting.Location = new System.Drawing.Point(10, 466);
            this.buttonFontSetting.Name = "buttonFontSetting";
            this.buttonFontSetting.Size = new System.Drawing.Size(91, 23);
            this.buttonFontSetting.TabIndex = 8;
            this.buttonFontSetting.Text = "设置字体";
            this.buttonFontSetting.UseVisualStyleBackColor = true;
            this.buttonFontSetting.Click += new System.EventHandler(this.buttonFontSetting_Click);
            // 
            // buttonOutputFile
            // 
            this.buttonOutputFile.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonOutputFile.Location = new System.Drawing.Point(439, 618);
            this.buttonOutputFile.Name = "buttonOutputFile";
            this.buttonOutputFile.Size = new System.Drawing.Size(136, 23);
            this.buttonOutputFile.TabIndex = 8;
            this.buttonOutputFile.Text = "导出文件";
            this.buttonOutputFile.UseVisualStyleBackColor = true;
            this.buttonOutputFile.Click += new System.EventHandler(this.buttonOutputFile_Click);
            // 
            // checkBoxRandomMode
            // 
            this.checkBoxRandomMode.AutoSize = true;
            this.checkBoxRandomMode.Enabled = false;
            this.checkBoxRandomMode.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBoxRandomMode.Location = new System.Drawing.Point(107, 470);
            this.checkBoxRandomMode.Name = "checkBoxRandomMode";
            this.checkBoxRandomMode.Size = new System.Drawing.Size(82, 18);
            this.checkBoxRandomMode.TabIndex = 9;
            this.checkBoxRandomMode.Text = "随机模式";
            this.checkBoxRandomMode.UseVisualStyleBackColor = true;
            this.checkBoxRandomMode.CheckedChanged += new System.EventHandler(this.checkBoxRandomMode_CheckedChanged);
            // 
            // comboBoxCharSet
            // 
            this.comboBoxCharSet.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.comboBoxCharSet.FormattingEnabled = true;
            this.comboBoxCharSet.Items.AddRange(new object[] {
            "基本ASCII字符集",
            "扩展ASCII字符集",
            "中英文通用字符集",
            "中日韩(CJK)基本字符集",
            "中日韩(CJK)扩展字符集",
            "全Unicode字符集"});
            this.comboBoxCharSet.Location = new System.Drawing.Point(439, 510);
            this.comboBoxCharSet.Name = "comboBoxCharSet";
            this.comboBoxCharSet.Size = new System.Drawing.Size(135, 25);
            this.comboBoxCharSet.TabIndex = 10;
            this.comboBoxCharSet.Text = "全Unicode字符集";
            this.comboBoxCharSet.SelectedIndexChanged += new System.EventHandler(this.comboBoxCharSet_SelectedIndexChanged);
            // 
            // labelFontSizePx
            // 
            this.labelFontSizePx.AutoSize = true;
            this.labelFontSizePx.BackColor = System.Drawing.Color.MidnightBlue;
            this.labelFontSizePx.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelFontSizePx.ForeColor = System.Drawing.Color.White;
            this.labelFontSizePx.Location = new System.Drawing.Point(287, 463);
            this.labelFontSizePx.Name = "labelFontSizePx";
            this.labelFontSizePx.Size = new System.Drawing.Size(234, 17);
            this.labelFontSizePx.TabIndex = 11;
            this.labelFontSizePx.Text = "微软雅黑 , 粗体 , 18pt , 275 px × 31 px ";
            this.labelFontSizePx.Click += new System.EventHandler(this.labelFontSizePx_Click);
            // 
            // labelCharSet
            // 
            this.labelCharSet.AutoSize = true;
            this.labelCharSet.Enabled = false;
            this.labelCharSet.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCharSet.Location = new System.Drawing.Point(437, 493);
            this.labelCharSet.Name = "labelCharSet";
            this.labelCharSet.Size = new System.Drawing.Size(68, 17);
            this.labelCharSet.TabIndex = 12;
            this.labelCharSet.Text = "选择字符集";
            this.labelCharSet.Click += new System.EventHandler(this.labelCharSet_Click);
            // 
            // checkBoxSingleCharOrString
            // 
            this.checkBoxSingleCharOrString.AutoSize = true;
            this.checkBoxSingleCharOrString.Checked = true;
            this.checkBoxSingleCharOrString.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxSingleCharOrString.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.checkBoxSingleCharOrString.Location = new System.Drawing.Point(108, 12);
            this.checkBoxSingleCharOrString.Name = "checkBoxSingleCharOrString";
            this.checkBoxSingleCharOrString.Size = new System.Drawing.Size(87, 21);
            this.checkBoxSingleCharOrString.TabIndex = 13;
            this.checkBoxSingleCharOrString.Text = "字符串模式";
            this.checkBoxSingleCharOrString.UseVisualStyleBackColor = true;
            this.checkBoxSingleCharOrString.CheckedChanged += new System.EventHandler(this.checkBoxSingleCharOrString_CheckedChanged);
            // 
            // buttonLastChar
            // 
            this.buttonLastChar.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonLastChar.Location = new System.Drawing.Point(201, 9);
            this.buttonLastChar.Name = "buttonLastChar";
            this.buttonLastChar.Size = new System.Drawing.Size(58, 23);
            this.buttonLastChar.TabIndex = 14;
            this.buttonLastChar.Text = "<<";
            this.buttonLastChar.UseVisualStyleBackColor = true;
            this.buttonLastChar.Visible = false;
            this.buttonLastChar.Click += new System.EventHandler(this.buttonLastChar_Click);
            // 
            // buttonNextChar
            // 
            this.buttonNextChar.Enabled = false;
            this.buttonNextChar.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonNextChar.Location = new System.Drawing.Point(258, 9);
            this.buttonNextChar.Name = "buttonNextChar";
            this.buttonNextChar.Size = new System.Drawing.Size(58, 23);
            this.buttonNextChar.TabIndex = 15;
            this.buttonNextChar.Text = ">>";
            this.buttonNextChar.UseVisualStyleBackColor = true;
            this.buttonNextChar.Visible = false;
            this.buttonNextChar.Click += new System.EventHandler(this.buttonNextChar_Click);
            // 
            // buttonInsertFile
            // 
            this.buttonInsertFile.Enabled = false;
            this.buttonInsertFile.Font = new System.Drawing.Font("黑体", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.buttonInsertFile.Location = new System.Drawing.Point(439, 591);
            this.buttonInsertFile.Name = "buttonInsertFile";
            this.buttonInsertFile.Size = new System.Drawing.Size(136, 23);
            this.buttonInsertFile.TabIndex = 8;
            this.buttonInsertFile.Text = "导入文件";
            this.buttonInsertFile.UseVisualStyleBackColor = true;
            this.buttonInsertFile.Click += new System.EventHandler(this.buttonInsertFile_Click);
            // 
            // labelFlashAddr
            // 
            this.labelFlashAddr.AutoSize = true;
            this.labelFlashAddr.Enabled = false;
            this.labelFlashAddr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelFlashAddr.Location = new System.Drawing.Point(436, 539);
            this.labelFlashAddr.Name = "labelFlashAddr";
            this.labelFlashAddr.Size = new System.Drawing.Size(85, 17);
            this.labelFlashAddr.TabIndex = 16;
            this.labelFlashAddr.Text = "FlashAddress";
            // 
            // numericUpDownFlashAddr
            // 
            this.numericUpDownFlashAddr.Enabled = false;
            this.numericUpDownFlashAddr.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDownFlashAddr.Hexadecimal = true;
            this.numericUpDownFlashAddr.Location = new System.Drawing.Point(439, 559);
            this.numericUpDownFlashAddr.Maximum = new decimal(new int[] {
            16777216,
            0,
            0,
            0});
            this.numericUpDownFlashAddr.Name = "numericUpDownFlashAddr";
            this.numericUpDownFlashAddr.Size = new System.Drawing.Size(135, 23);
            this.numericUpDownFlashAddr.TabIndex = 17;
            this.numericUpDownFlashAddr.Value = new decimal(new int[] {
            4194304,
            0,
            0,
            0});
            this.numericUpDownFlashAddr.ValueChanged += new System.EventHandler(this.numericUpDownFlashAddr_ValueChanged);
            // 
            // pictureBoxDrawingPicture
            // 
            this.pictureBoxDrawingPicture.Location = new System.Drawing.Point(454, 89);
            this.pictureBoxDrawingPicture.Name = "pictureBoxDrawingPicture";
            this.pictureBoxDrawingPicture.Size = new System.Drawing.Size(121, 50);
            this.pictureBoxDrawingPicture.TabIndex = 18;
            this.pictureBoxDrawingPicture.TabStop = false;
            this.pictureBoxDrawingPicture.Visible = false;
            this.pictureBoxDrawingPicture.Click += new System.EventHandler(this.pictureBoxDrawingPicture_Click);
            // 
            // numericUpDownXPos
            // 
            this.numericUpDownXPos.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDownXPos.Location = new System.Drawing.Point(394, 9);
            this.numericUpDownXPos.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numericUpDownXPos.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            -2147483648});
            this.numericUpDownXPos.Name = "numericUpDownXPos";
            this.numericUpDownXPos.Size = new System.Drawing.Size(60, 23);
            this.numericUpDownXPos.TabIndex = 19;
            this.numericUpDownXPos.ValueChanged += new System.EventHandler(this.numericUpDownXPos_ValueChanged);
            // 
            // numericUpDownYPos
            // 
            this.numericUpDownYPos.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.numericUpDownYPos.Location = new System.Drawing.Point(514, 9);
            this.numericUpDownYPos.Maximum = new decimal(new int[] {
            1024,
            0,
            0,
            0});
            this.numericUpDownYPos.Minimum = new decimal(new int[] {
            1024,
            0,
            0,
            -2147483648});
            this.numericUpDownYPos.Name = "numericUpDownYPos";
            this.numericUpDownYPos.Size = new System.Drawing.Size(60, 23);
            this.numericUpDownYPos.TabIndex = 19;
            this.numericUpDownYPos.ValueChanged += new System.EventHandler(this.numericUpDownYPos_ValueChanged);
            // 
            // labelXPos
            // 
            this.labelXPos.AutoSize = true;
            this.labelXPos.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelXPos.Location = new System.Drawing.Point(347, 12);
            this.labelXPos.Name = "labelXPos";
            this.labelXPos.Size = new System.Drawing.Size(41, 17);
            this.labelXPos.TabIndex = 20;
            this.labelXPos.Text = "X Pos";
            this.labelXPos.Click += new System.EventHandler(this.labelXPos_Click);
            // 
            // labelYPos
            // 
            this.labelYPos.AutoSize = true;
            this.labelYPos.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelYPos.Location = new System.Drawing.Point(467, 12);
            this.labelYPos.Name = "labelYPos";
            this.labelYPos.Size = new System.Drawing.Size(40, 17);
            this.labelYPos.TabIndex = 20;
            this.labelYPos.Text = "Y Pos";
            this.labelYPos.Click += new System.EventHandler(this.labelYPos_Click);
            // 
            // pictureBoxBKG
            // 
            this.pictureBoxBKG.Location = new System.Drawing.Point(12, 35);
            this.pictureBoxBKG.Name = "pictureBoxBKG";
            this.pictureBoxBKG.Size = new System.Drawing.Size(562, 425);
            this.pictureBoxBKG.TabIndex = 21;
            this.pictureBoxBKG.TabStop = false;
            this.pictureBoxBKG.Click += new System.EventHandler(this.pictureBoxBKG_Click);
            // 
            // labelCurrentChar
            // 
            this.labelCurrentChar.AutoSize = true;
            this.labelCurrentChar.BackColor = System.Drawing.Color.MidnightBlue;
            this.labelCurrentChar.Font = new System.Drawing.Font("微软雅黑", 15F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.labelCurrentChar.ForeColor = System.Drawing.Color.White;
            this.labelCurrentChar.Location = new System.Drawing.Point(408, 35);
            this.labelCurrentChar.Name = "labelCurrentChar";
            this.labelCurrentChar.Size = new System.Drawing.Size(23, 27);
            this.labelCurrentChar.TabIndex = 22;
            this.labelCurrentChar.Text = "y";
            this.labelCurrentChar.Visible = false;
            this.labelCurrentChar.Click += new System.EventHandler(this.labelCurrentChar_Click);
            // 
            // saveFileDialogSaving
            // 
            this.saveFileDialogSaving.FileName = "EasyMatrix";
            this.saveFileDialogSaving.FileOk += new System.ComponentModel.CancelEventHandler(this.saveFileDialogSaving_FileOk);
            // 
            // pictureBox_DrawChar
            // 
            this.pictureBox_DrawChar.BackColor = System.Drawing.Color.White;
            this.pictureBox_DrawChar.Enabled = false;
            this.pictureBox_DrawChar.Location = new System.Drawing.Point(13, 96);
            this.pictureBox_DrawChar.Name = "pictureBox_DrawChar";
            this.pictureBox_DrawChar.Size = new System.Drawing.Size(564, 219);
            this.pictureBox_DrawChar.TabIndex = 24;
            this.pictureBox_DrawChar.TabStop = false;
            this.pictureBox_DrawChar.Visible = false;
            this.pictureBox_DrawChar.Click += new System.EventHandler(this.pictureBox_DrawChar_Click);
            // 
            // pictureBoxRedisp
            // 
            this.pictureBoxRedisp.BackColor = System.Drawing.Color.Maroon;
            this.pictureBoxRedisp.Enabled = false;
            this.pictureBoxRedisp.Location = new System.Drawing.Point(11, 237);
            this.pictureBoxRedisp.Name = "pictureBoxRedisp";
            this.pictureBoxRedisp.Size = new System.Drawing.Size(564, 219);
            this.pictureBoxRedisp.TabIndex = 27;
            this.pictureBoxRedisp.TabStop = false;
            this.pictureBoxRedisp.Visible = false;
            this.pictureBoxRedisp.Click += new System.EventHandler(this.pictureBoxRedisp_Click);
            // 
            // FormEasyMatrix
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoValidate = System.Windows.Forms.AutoValidate.EnablePreventFocusChange;
            this.ClientSize = new System.Drawing.Size(586, 652);
            this.Controls.Add(this.labelCurrentChar);
            this.Controls.Add(this.labelYPos);
            this.Controls.Add(this.labelXPos);
            this.Controls.Add(this.numericUpDownYPos);
            this.Controls.Add(this.numericUpDownXPos);
            this.Controls.Add(this.pictureBoxDrawingPicture);
            this.Controls.Add(this.numericUpDownFlashAddr);
            this.Controls.Add(this.labelFlashAddr);
            this.Controls.Add(this.textBoxInput);
            this.Controls.Add(this.buttonNextChar);
            this.Controls.Add(this.buttonLastChar);
            this.Controls.Add(this.labelCharSet);
            this.Controls.Add(this.labelFontSizePx);
            this.Controls.Add(this.comboBoxCharSet);
            this.Controls.Add(this.checkBoxRandomMode);
            this.Controls.Add(this.buttonOutputFile);
            this.Controls.Add(this.buttonInsertFile);
            this.Controls.Add(this.buttonFontSetting);
            this.Controls.Add(this.buttonRight);
            this.Controls.Add(this.buttonLeft);
            this.Controls.Add(this.buttonDown);
            this.Controls.Add(this.buttonUp);
            this.Controls.Add(this.buttonSwitchTab);
            this.Controls.Add(this.progressBarSaving);
            this.Controls.Add(this.checkBoxSingleCharOrString);
            this.Controls.Add(this.pictureBoxRedisp);
            this.Controls.Add(this.pictureBox_DrawChar);
            this.Controls.Add(this.pictureBoxViewer);
            this.Controls.Add(this.richTextBoxRandomHexDatas);
            this.Controls.Add(this.pictureBoxBKG);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.MaximizeBox = false;
            this.Name = "FormEasyMatrix";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "EasyMatrix V0.3";
            this.Load += new System.EventHandler(this.FormEasyMatrix_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxViewer)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownFlashAddr)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxDrawingPicture)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownXPos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDownYPos)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxBKG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox_DrawChar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxRedisp)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxViewer;
        private System.Windows.Forms.ProgressBar progressBarSaving;
        private System.Windows.Forms.RichTextBox richTextBoxRandomHexDatas;
        private System.Windows.Forms.Button buttonSwitchTab;
        private System.Windows.Forms.Button buttonUp;
        private System.Windows.Forms.Button buttonDown;
        private System.Windows.Forms.Button buttonLeft;
        private System.Windows.Forms.Button buttonRight;
        private System.Windows.Forms.TextBox textBoxInput;
        private System.Windows.Forms.Button buttonFontSetting;
        private System.Windows.Forms.Button buttonOutputFile;
        private System.Windows.Forms.CheckBox checkBoxRandomMode;
        private System.Windows.Forms.ComboBox comboBoxCharSet;
        private System.Windows.Forms.Label labelFontSizePx;
        private System.Windows.Forms.Label labelCharSet;
        private System.Windows.Forms.CheckBox checkBoxSingleCharOrString;
        private System.Windows.Forms.Button buttonLastChar;
        private System.Windows.Forms.Button buttonNextChar;
        private System.Windows.Forms.Button buttonInsertFile;
        private System.Windows.Forms.Label labelFlashAddr;
        private System.Windows.Forms.NumericUpDown numericUpDownFlashAddr;
        private System.Windows.Forms.PictureBox pictureBoxDrawingPicture;
        private System.Windows.Forms.NumericUpDown numericUpDownXPos;
        private System.Windows.Forms.NumericUpDown numericUpDownYPos;
        private System.Windows.Forms.Label labelXPos;
        private System.Windows.Forms.Label labelYPos;
        private System.Windows.Forms.PictureBox pictureBoxBKG;
        private System.Windows.Forms.Label labelCurrentChar;
        private System.Windows.Forms.SaveFileDialog saveFileDialogSaving;
        private System.Windows.Forms.PictureBox pictureBox_DrawChar;
        private System.Windows.Forms.PictureBox pictureBoxRedisp;

    }
}

