using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using System.Windows.Forms;
using System.Diagnostics;
using System.Runtime.InteropServices;

public struct BinFile
{ 
    public String path;
    public UInt32 addr;
    public int len;
}

public struct StitchInfo
{
    public int fileNum;
    public BinFile[] binFiles;
}


namespace hedyBinStitcher
{
    public partial class hedyBinStitcher : Form
    {

        const int DATA_SIZE_KB = 1024;
        const int DATA_SIZE_MB = 1024 * DATA_SIZE_KB;
        const int DATA_SIZE_GB = 1024 * DATA_SIZE_MB;

        StitchInfo stitchInfo;
        BinFile binFileOutput;
        BinFile[] binFileArrow = new BinFile[4];

        public hedyBinStitcher()
        {
            InitializeComponent();
            creatBinFile();
            eraseBinFile();
        }

        private void hedyBinStitcher_Load(object sender, EventArgs e)
        {

        }

        #region General Functions
        private void creatBinFile()
        { 
            stitchInfo.binFiles = new BinFile[binFileArrow.Length];
            for (int i = 0; i < binFileArrow.Length; i++)
            {
                stitchInfo.binFiles[i].path = "";
                stitchInfo.binFiles[i].len = 0;
                stitchInfo.binFiles[i].addr = 0;
            }
        }
        private void eraseBinFile()
        {
            for (int i = 0; i < binFileArrow.Length; i++)
            { 
                binFileArrow[i].path = "";
                binFileArrow[i].len = 0;
                binFileArrow[i].addr = 0;
            }

            for (int i = 0; i < stitchInfo.binFiles.Length; i++)
            {
                stitchInfo.binFiles[i].path = "";
                stitchInfo.binFiles[i].len = 0;
                stitchInfo.binFiles[i].addr = 0;
            }
        }
        private void eraseFile(String fileName)
        {
            File.WriteAllText(fileName, "");
        }
        #endregion

        #region Functions
        // 排序
        private void sortFiles()
        {
            BinFile binFileTemp;

            for (int i = 0; i < stitchInfo.fileNum; i++)
            {
                for (int j = i + 1; j < stitchInfo.fileNum; j++)
                {
                    if (stitchInfo.binFiles[i].addr > stitchInfo.binFiles[j].addr)
                    {
                        binFileTemp = stitchInfo.binFiles[i];
                        stitchInfo.binFiles[i] = stitchInfo.binFiles[j];
                        stitchInfo.binFiles[j] = binFileTemp;
                    }
                }
            }
        }

        // 检查交叉
        private bool checkOverlap()
        { 
            sortFiles();
            for (int i = 0; i < stitchInfo.fileNum - 1; i++)
            { 
                if(stitchInfo.binFiles[i].addr + stitchInfo.binFiles[i].len > stitchInfo.binFiles[i+1].addr)
                { 
                    return false;
                }
            }
            // 检查一共几份文件
            return true;
        }

        // 计算文件大小显示字串及单位
        private String calSizeString(int len)
        {
            String strSize;
            //if (len >= DATA_SIZE_GB)
            //{
            //    len /= DATA_SIZE_GB;
            //    strSize = len.ToString() + " GB |";
            //}
            //else 
            if (len >= DATA_SIZE_MB)
            {
                len /= DATA_SIZE_MB;
                strSize = len.ToString() + " MB |";
            }
            else if (len >= DATA_SIZE_KB)
            {
                len /= DATA_SIZE_KB;
                strSize = len.ToString() + " KB |";
            }
            else
            {
                strSize = len.ToString() + " B |";
            }

            return strSize;
        }

        // 输出拼接文件
        private void outputStitchedBinFile()
        {
            byte ch = 0xFF;
            try
            {
                BinaryWriter bw;
                BinaryReader br;
                String filePath = binFileOutput.path + ".hff";
                eraseFile(filePath);
                FileInfo fi = new FileInfo(filePath);
                using (bw = new BinaryWriter(fi.OpenWrite()))
                {
                    // 开头清空
                    for (int i = 0; i < stitchInfo.binFiles[0].addr; i++)
                    {
                        bw.Write(ch);
                    }

                    for (int i = 0; i < stitchInfo.fileNum; i++)
                    {
                        FileInfo fiBr = new FileInfo(stitchInfo.binFiles[i].path);
                        using (br = new BinaryReader(fiBr.OpenRead()))
                        {
                            bw.Write(br.ReadBytes(stitchInfo.binFiles[i].len));

                            // 插空
                            if(i < stitchInfo.fileNum - 1)
                            {
                                for (int j = 0; j < stitchInfo.binFiles[i + 1].addr - stitchInfo.binFiles[i].addr - stitchInfo.binFiles[i].len; j++)
                                {
                                    bw.Write(ch);
                                }
                            }

                            br.Close();
                        }
                    }

                    bw.Flush();
                    bw.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "警告！");
            }
        }
        #endregion

        #region numericUpDownAddrFile1 event process
        private void numericUpDownAddrFile1_ValueChanged(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                if (numericUpDownAddrFile1.Text.Length > 6)
                {
                    numericUpDownAddrFile1.Text = "FFFFFF";
                }
            }));
            
            binFileArrow[0].addr = (UInt32)numericUpDownAddrFile1.Value;
        }

        private void numericUpDownAddrFile1_KeyDown(object sender, KeyEventArgs e)
        {
            // 字符非十六进制字符、非退格键、非方向键、非删除键，不响应本次按键
            if (!(Char.IsNumber((char)e.KeyValue))
                && (e.KeyValue != 0x08)
                && (e.KeyValue != 0x2E)
                && !(e.KeyValue >= 0x25 && e.KeyValue <= 0x28)
                && !(e.KeyValue >= 'a' && e.KeyValue <= 'f')
                && !(e.KeyValue >= 'A' && e.KeyValue <= 'F'))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void numericUpDownAddrFile1_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 控制字符串在6位以内，且不允许为空
            if (numericUpDownAddrFile1.Text.Length > 6)
            {
                numericUpDownAddrFile1.Text = "FFFFFF";
            }
        }

        private void numericUpDownAddrFile1_KeyUp(object sender, KeyEventArgs e)
        {
            // 控制字符串在6位以内，且不允许为空
            if (numericUpDownAddrFile1.Text == "")
            {
                numericUpDownAddrFile1.Text = "0";
            }
            else if (numericUpDownAddrFile1.Text.Length > 6)
            {
                numericUpDownAddrFile1.Text = "FFFFFF";
            }
        }
        #endregion

        #region numericUpDownAddrFile2 event process
        private void numericUpDownAddrFile2_ValueChanged(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                if (numericUpDownAddrFile2.Text.Length > 6)
                {
                    numericUpDownAddrFile2.Text = "FFFFFF";
                }
            }));

            binFileArrow[1].addr = (UInt32)numericUpDownAddrFile2.Value;
        }

        private void numericUpDownAddrFile2_KeyDown(object sender, KeyEventArgs e)
        {
            // 字符非十六进制字符、非退格键、非方向键、非删除键，不响应本次按键
            if (!(Char.IsNumber((char)e.KeyValue))
                && (e.KeyValue != 0x08)
                && (e.KeyValue != 0x2E)
                && !(e.KeyValue >= 0x25 && e.KeyValue <= 0x28)
                && !(e.KeyValue >= 'a' && e.KeyValue <= 'f')
                && !(e.KeyValue >= 'A' && e.KeyValue <= 'F'))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void numericUpDownAddrFile2_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 控制字符串在6位以内，且不允许为空
            if (numericUpDownAddrFile2.Text.Length > 6)
            {
                numericUpDownAddrFile2.Text = "FFFFFF";
            }
        }

        private void numericUpDownAddrFile2_KeyUp(object sender, KeyEventArgs e)
        {
            // 控制字符串在6位以内，且不允许为空
            if (numericUpDownAddrFile2.Text == "")
            {
                numericUpDownAddrFile2.Text = "0";
            }
            else if (numericUpDownAddrFile2.Text.Length > 6)
            {
                numericUpDownAddrFile2.Text = "FFFFFF";
            }
        }
        #endregion

        #region numericUpDownAddrFile3 event process
        private void numericUpDownAddrFile3_ValueChanged(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                if (numericUpDownAddrFile3.Text.Length > 6)
                {
                    numericUpDownAddrFile3.Text = "FFFFFF";
                }
            }));

            binFileArrow[2].addr = (UInt32)numericUpDownAddrFile3.Value;
        }

        private void numericUpDownAddrFile3_KeyDown(object sender, KeyEventArgs e)
        {
            // 字符非十六进制字符、非退格键、非方向键、非删除键，不响应本次按键
            if (!(Char.IsNumber((char)e.KeyValue))
                && (e.KeyValue != 0x08)
                && (e.KeyValue != 0x2E)
                && !(e.KeyValue >= 0x25 && e.KeyValue <= 0x28)
                && !(e.KeyValue >= 'a' && e.KeyValue <= 'f')
                && !(e.KeyValue >= 'A' && e.KeyValue <= 'F'))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void numericUpDownAddrFile3_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 控制字符串在6位以内，且不允许为空
            if (numericUpDownAddrFile3.Text.Length > 6)
            {
                numericUpDownAddrFile3.Text = "FFFFFF";
            }
        }
        
        private void numericUpDownAddrFile3_KeyUp(object sender, KeyEventArgs e)
        {
            // 控制字符串在6位以内，且不允许为空
            if (numericUpDownAddrFile3.Text == "")
            {
                numericUpDownAddrFile3.Text = "0";
            }
            else if (numericUpDownAddrFile3.Text.Length > 6)
            {
                numericUpDownAddrFile3.Text = "FFFFFF";
            }
        }
        #endregion

        #region numericUpDownAddrFile4 event process
        private void numericUpDownAddrFile4_ValueChanged(object sender, EventArgs e)
        {
            this.BeginInvoke(new MethodInvoker(delegate
            {
                if (numericUpDownAddrFile4.Text.Length > 6)
                {
                    numericUpDownAddrFile4.Text = "FFFFFF";
                }
            }));

            binFileArrow[3].addr = (UInt32)numericUpDownAddrFile4.Value;
        }

        private void numericUpDownAddrFile4_KeyDown(object sender, KeyEventArgs e)
        {
            // 字符非十六进制字符、非退格键、非方向键、非删除键，不响应本次按键
            if(!(Char.IsNumber((char)e.KeyValue))
                && (e.KeyValue != 0x08)
                && (e.KeyValue != 0x2E)
                && !(e.KeyValue >= 0x25 && e.KeyValue <= 0x28)
                && !(e.KeyValue >= 'a' && e.KeyValue <= 'f')
                && !(e.KeyValue >= 'A' && e.KeyValue <= 'F'))
            {
                e.Handled = true;
            }
            else
            {
                e.Handled = false;
            }
        }

        private void numericUpDownAddrFile4_KeyPress(object sender, KeyPressEventArgs e)
        {
            // 控制字符串在6位以内，且不允许为空
            if (numericUpDownAddrFile4.Text.Length > 6)
            {
                numericUpDownAddrFile4.Text = "FFFFFF";
            }
        }

        private void numericUpDownAddrFile4_KeyUp(object sender, KeyEventArgs e)
        {
            // 控制字符串在6位以内，且不允许为空
            if (numericUpDownAddrFile4.Text == "")
            { 
                numericUpDownAddrFile4.Text = "0";
            }
            else if(numericUpDownAddrFile4.Text.Length > 6)
            { 
                numericUpDownAddrFile4.Text = "FFFFFF";
            }
        }
        #endregion


        private void progressBar_Click(object sender, EventArgs e)
        {
            
        }

        private void buttonOpenFile1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(dialog.FileName);

                if (fi.Length >= DATA_SIZE_GB)
                {
                    MessageBox.Show("文件大小超过1GB！", "警告！");
                }
                else
                {
                    binFileArrow[0].len = (int)fi.Length;
                    binFileArrow[0].path = dialog.FileName;
                    textBoxPathFile1.Text = dialog.FileName + "     ";
                    labelSizeFile1.Text = calSizeString(binFileArrow[0].len);
                    buttonDeletFile1.Visible = true;
                } 
            }
            else
            {
                return;
            }
        }
        
        private void buttonOpenFile2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(dialog.FileName);

                if (fi.Length >= DATA_SIZE_GB)
                {
                    MessageBox.Show("文件大小超过1GB！", "警告！");
                }
                else
                {
                    binFileArrow[1].len = (int)fi.Length;
                    binFileArrow[1].path = dialog.FileName;
                    textBoxPathFile2.Text = dialog.FileName + "     ";
                    labelSizeFile2.Text = calSizeString(binFileArrow[1].len);
                    buttonDeletFile2.Visible = true;
                } 
            }
            else
            {
                return;
            }
        }

        private void buttonOpenFile3_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(dialog.FileName);

                if (fi.Length >= DATA_SIZE_GB)
                {
                    MessageBox.Show("文件大小超过1GB！", "警告！");
                }
                else
                {
                    binFileArrow[2].len = (int)fi.Length;
                    binFileArrow[2].path = dialog.FileName;
                    textBoxPathFile3.Text = dialog.FileName + "     ";
                    labelSizeFile3.Text = calSizeString(binFileArrow[2].len);
                    buttonDeletFile3.Visible = true;
                } 
            }
            else
            {
                return;
            }
        }

        private void buttonOpenFile4_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                FileInfo fi = new FileInfo(dialog.FileName);

                if (fi.Length >= DATA_SIZE_GB)
                {
                    MessageBox.Show("文件大小超过1GB！", "警告！");
                }
                else
                {
                    binFileArrow[3].len = (int)fi.Length;
                    binFileArrow[3].path = dialog.FileName;
                    textBoxPathFile4.Text = dialog.FileName + "     ";
                    labelSizeFile4.Text = calSizeString(binFileArrow[3].len);
                    buttonDeletFile4.Visible = true;
                } 
            }
            else
            {
                return;
            }
        }

        // 检查数量
        private bool checkFileNum()
        { 
            stitchInfo.fileNum = 0;

            // 计数+拷贝
            for (int i = 0; i < binFileArrow.Length; i++)
            {
                if (binFileArrow[i].path != "")
                { 
                    stitchInfo.binFiles[stitchInfo.fileNum] = binFileArrow[i];
                    stitchInfo.fileNum ++;
                }
            }

            if (stitchInfo.fileNum < 2)
            { 
                return false;
            }

            return true;
        }

        private void buttonStitch_Click(object sender, EventArgs e)
        {
            // 
            if (!checkFileNum())
            {
                MessageBox.Show("文件数量过少!", "警告！");
                return;
            }

            // 
            if (!checkOverlap())
            {
                MessageBox.Show("文件区域交叉!", "警告！");
                return;
            }

            // 弹窗
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
            {
                binFileOutput.path = saveFileDialog.FileName;
            }
            else
            {
                return;
            }

            // 
            outputStitchedBinFile();

            // 提示完成
            MessageBox.Show("文件输出成功!", "提示！");
        }

        private void buttonDeletFile1_Click(object sender, EventArgs e)
        {
            binFileArrow[0].len = 0;
            binFileArrow[0].path = "";
            textBoxPathFile1.Text = "";
            labelSizeFile1.Text = "- B |";
            buttonDeletFile1.Visible = false;
        }

        private void buttonDeletFile2_Click(object sender, EventArgs e)
        {
            binFileArrow[1].len = 0;
            binFileArrow[1].path = "";
            textBoxPathFile2.Text = "";
            labelSizeFile2.Text = "- B |";
            buttonDeletFile2.Visible = false;
        }

        private void buttonDeletFile3_Click(object sender, EventArgs e)
        {
            binFileArrow[2].len = 0;
            binFileArrow[2].path = "";
            textBoxPathFile3.Text = "";
            labelSizeFile3.Text = "- B |";
            buttonDeletFile3.Visible = false;
        }

        private void buttonDeletFile4_Click(object sender, EventArgs e)
        {
            binFileArrow[3].len = 0;
            binFileArrow[3].path = "";
            textBoxPathFile4.Text = "";
            labelSizeFile4.Text = "- B |";
            buttonDeletFile4.Visible = false;
        }

    }
}
