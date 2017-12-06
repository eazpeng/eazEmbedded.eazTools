using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using SharpSvn;
using CalMD5;
using System.Runtime.InteropServices;

public struct PicHead
{
    public byte[] tag;
    public byte[] crc;
    public byte[] thisAddr;

    public byte[] picAddr;
    public byte[] datas;

    public UInt32 len;
    public UInt32 lenDatas;
}
public struct MonoFile{
    public UInt16 rowLen;
    public UInt16 colLen;

    public UInt32 dataLen;
    public byte[] datas;
}
namespace picMatrix
{
    public partial class FormPicMatrix : Form
    {
        String[] openFileNames;
        String[] saveFileNames;
        UInt16 fileSum;
        UInt16 curFileIndex;

        public FormPicMatrix()
        {
            InitializeComponent();
        }

        #region

        private UInt16 CalPixel(Color color)
        {
            int tmpColorValue1;
            int tmpColorValue2;
            int tmpColorValue3;

            tmpColorValue1 = color.R / 8;
            tmpColorValue2 = color.G / 4;
            tmpColorValue3 = color.B / 8;

            /*
            if (color.B < 0x0F)
            {
                tmpColorValue3 = 0;
            }
            if (color.G < 0x0F)
            { 
                tmpColorValue2 = 0;
            }
            if (color.R < 0x0F)
            {
                tmpColorValue1 = 0;
            }
            */

            return (UInt16)(tmpColorValue1 * (Math.Pow(2, 11)) + tmpColorValue2 * (Math.Pow(2, 5)) + tmpColorValue3);
        }

        private UInt16[] SavePixels()
        {
            UInt16 index = 0;
            Bitmap bitmap = new Bitmap(pictureBoxCurrentPic.Image);
            UInt16[] colorValues = new UInt16[pictureBoxCurrentPic.Image.Width * pictureBoxCurrentPic.Image.Height + 2];

            colorValues[index++] = (UInt16)pictureBoxCurrentPic.Image.Width;
            colorValues[index++] = (UInt16)pictureBoxCurrentPic.Image.Height;

            for (int y = 0; y < colorValues[1]; y++)
            {
                for (int x = 0; x < colorValues[0]; x++)
                {
                    colorValues[index++] = CalPixel(bitmap.GetPixel(x, y));
                }
            }
            
            bitmap.Dispose();
            return colorValues;
        }

        private PicHead creatPicHead()
        { 
            PicHead ph = new PicHead();

            ph.tag = Encoding.ASCII.GetBytes("Reserved........");
            ph.crc = Encoding.ASCII.GetBytes("**");
            ph.thisAddr =  toBytes(0x1000);//Encoding.ASCII.GetBytes("1234");

            ph.picAddr = new byte[0x100*4];
            ph.datas = new byte[0x800000-0x410];
            UInt32 tmp = 0x410;

            pictureBoxCurrentPic.Image = Image.FromFile(openFileNames[0]);
            UInt16[] colorsHex;
            int j = 0;
            int tempDataLen = 0;
            for (int i = 0; i < fileSum; i++)
            {
                ph.picAddr[j++] = (byte)(tmp>>24);
                ph.picAddr[j++] = (byte)(tmp>>16) ;
                ph.picAddr[j++] = (byte)(tmp>>8);
                ph.picAddr[j++] = (byte)(tmp);
                colorsHex = SavePixels();
                tmp += (UInt32)colorsHex.Length*2;
                for (int l = 0; l < colorsHex.Length; l++)
                {
                    ph.datas[tempDataLen++] = (byte)(colorsHex[l] >> 8);
                    ph.datas[tempDataLen++] = (byte)(colorsHex[l]);
                }
                if (i < fileSum - 1)
                {
                    pictureBoxCurrentPic.Image = Image.FromFile(openFileNames[i + 1]);
                }
            }

            ph.lenDatas = (UInt32)(tempDataLen);
            ph.len = (UInt32)((UInt32)ph.tag.Length + (UInt32)ph.crc.Length + (UInt32)ph.thisAddr.Length + (UInt32)ph.picAddr.Length + ph.lenDatas);

            return ph;
        }

        private byte[] toBytes(UInt32 num)
        { 
            byte[] bt = new byte[4];

            for(int i = 0; i < 4; i++)
            { 
                bt[i] = (byte)(num / Math.Pow(0x100, 3 - i));
            }

            return bt;
        }

        private byte[] toBytes(PicHead ph)
        { 

            while(ph.len % 256 != 0)
            { 
                ph.len ++;
            }

            byte[] bt = new byte[ph.len];
            int j = 0;
            
            for (int i = 0; i < ph.tag.Length; i++)
            { 
                bt[j++]=ph.tag[i];
            }
            /*
            for (int i = 0; i < ph.crc.Length; i++)
            { 
                bt[j++] = ph.crc[i];
            }

            for (int i = 0; i < ph.thisAddr.Length; i++)
            {
                bt[j++] = ph.thisAddr[i];
            }
            for(int i = 0; i < 0x10; i++)
            {
                bt[j++] = 0xFF;
            }
            */

            for (int i = 0; i < ph.picAddr.Length; i++)
            {
                bt[j++] = ph.picAddr[i];
            }
            for (int i = 0; i < ph.lenDatas; i++)
            {
                bt[j++] = ph.datas[i];
            }
            for (; j < ph.len; j++)
            { 
                bt[j] = 0xFF;
            }

            return bt;
        }

        private void eraseFile(String filePath)
        {
            File.WriteAllText(filePath, "");
        }
        private void OutputBinaryFile(UInt16[] colorsHex, String fileName)
        {
            try
            {
                BinaryWriter bw;
                String filePath = fileName + ".pdx";
                eraseFile(filePath);
                FileInfo fi = new FileInfo(filePath);
                using (bw = new BinaryWriter(fi.OpenWrite()))
                {
                    for(int i =0;i<colorsHex.Length;i++)
                    {
                        bw.Write((byte)(colorsHex[i] / 256));
                        bw.Write((byte)(colorsHex[i] % 256));
                    }
                    bw.Flush();
                    bw.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Warning!");
            }
        }

        private void outputBinaryFile(String fileName)
        {
            try
            {
                BinaryWriter bw;
                PicHead ph = creatPicHead();
                if(ph.len > 0x300000 - 0x400*64)
                { 
                    MessageBox.Show("文件大小超出3M-64K！！");
                    return;
                }
                String filePath = fileName + ".pic";
                eraseFile(filePath);
                FileInfo fi = new FileInfo(filePath);
                using (bw = new BinaryWriter(fi.OpenWrite()))
                {

                    bw.Write(toBytes(ph));
                    
                    bw.Flush();
                    bw.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Warning!");
            }
        }

        private MonoFile toMonoFile(UInt16[] colorsHex)
        { 
            MonoFile monoFile;

            monoFile.rowLen = colorsHex[1];
            monoFile.colLen = colorsHex[0];
            monoFile.dataLen = (UInt32)(monoFile.rowLen * (monoFile.colLen / 8 + (monoFile.colLen % 8 > 0 ? 1 : 0)));
            monoFile.datas = new byte[monoFile.dataLen];

            UInt16 i, j;
            UInt32 index = 0;
            byte tempMask;
            for (i = 0; i < monoFile.rowLen; i++)
            {
                tempMask = 0x80;
                for (j = 0; j < monoFile.colLen; j++)
                {
                    if (colorsHex[i * monoFile.colLen + j + 2] < 0x20)
                    {
                        monoFile.datas[index] |= tempMask;
                    }
                    tempMask >>= 1;
                    if(tempMask == 0)
                    { 
                        tempMask = 0x80;
                    }
                    if (tempMask == 0x80 && j < monoFile.colLen - 1)
                    { 
                        index ++;
                    }
                }
                index ++;
            }

            return monoFile;
        }

        private void OutputCFormatFile(UInt16[] colorsHex, String fileName)//
        {
            if(checkBoxBinFormat.Checked == false)
            { 
                return;    
            }

            UInt16 index = 0;
            String fn = Path.GetFileNameWithoutExtension(fileName);
            StringBuilder sb = new StringBuilder();
            sb.Append("// AhHua@HedyMed\r\n\r\n");
            sb.Append("// ");
            sb.Append(fn);
            sb.Append("\r\n");


            // 转换
            if (colorsHex[1] > 64 || colorsHex[0] > 128)
            { 
                MessageBox.Show("文件超限！");
                return;
            }
            MonoFile monoFile = toMonoFile(colorsHex);
            // 换名字
            sb.Append("const UINT16 monoPicAhHua[" + monoFile.dataLen + "+2" + "] = { 0x");
            sb.Append(monoFile.colLen.ToString("X2") + ", 0x");
            sb.Append(monoFile.rowLen.ToString("X2") + ", // WIDTH & HEIGHT");

            for (int x = 0; x < monoFile.dataLen; x++)
            {
                if (index % 16 == 0)
                {
                    sb.Append("\r\n\t");
                }
                sb.Append("0x" + monoFile.datas[index++].ToString("X2"));
                if (index <= monoFile.dataLen - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.Append(" };\r\n");

            fileName = fileName.Substring(0, fileName.Length - 4);
            StreamWriter sw = new System.IO.StreamWriter(fileName + ".c");
            sw.Write(sb.ToString());
            sw.Close();
            OutputBinaryFile(colorsHex,fileName);
        }

        #endregion

        private void saveFileDialogPicMatrix_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void FormPicMatrix_Load(object sender, EventArgs e)
        {

        }

        private void pictureBoxCurrentPic_Click(object sender, EventArgs e)
        {

        }

        private void buttonOpen_Click(object sender, EventArgs e)
        {
            OpenFileDialog openBMP = new OpenFileDialog();
            openBMP.Multiselect = true;
            openBMP.Filter = "位图|*.bmp";
            if (openBMP.ShowDialog() == DialogResult.OK)
            {
                saveFileNames = openFileNames = openBMP.FileNames;
                fileSum = (UInt16)openFileNames.Length;
                curFileIndex = 0;
                pictureBoxCurrentPic.Image = Image.FromFile(openFileNames[curFileIndex]);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < openFileNames.Length; i++)
                {
                    sb.Append(openFileNames[i] + "\r\n");
                }
                richTextBoxOpenPaths.Text = sb.ToString();
            }
            else
            {
                return;
            }
        }

        private void outputInfoFile(String fileName, String text)
        {
            try
            {
                StreamWriter sw = new System.IO.StreamWriter(fileName + "Info.txt");
                sw.Write(text);
                sw.Close();
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Warning!");
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            if (openFileNames == null)
            { 
                MessageBox.Show("请导入图片");
                return;
            }
            else
            {
                progressBarSaving.Visible = true;
                progressBarSaving.Value = 0;
                for (int i = 0; i < fileSum; i++)
                {
                    ///*
                    OutputCFormatFile(SavePixels(), saveFileNames[i]);
                    //*/
                    if (i < fileSum - 1)
                    {
                        pictureBoxCurrentPic.Image = Image.FromFile(openFileNames[i + 1]);
                    }
                    progressBarSaving.Value = (int)(((float)(i + 1) / (float)fileSum) * 100);
                }
                pictureBoxCurrentPic.Image = Image.FromFile(openFileNames[curFileIndex]);
                outputBinaryFile("PicturePack");

                //
                //OperINI.ReadINI("UserSection", "UserName", "", UserName, STR_LEN, str_ini_path);
                //OperINI.ReadINI("UserSection", "UserPassword", "", UserPassword, STR_LEN, str_ini_path);

                String strMd5, strSvn;
                String uiFilePath = "https://192.168.38.203/svn/Projects/01. Infusion Pump/02. 详细设计阶段/02-Software/06-SupportSoftware/07-图片取模工具/UI切图";
                CalMD5.Class1 md5 = new CalMD5.Class1();
                strMd5 = md5.GetMD5HashFromFile(System.AppDomain.CurrentDomain.BaseDirectory + "PicturePack.pic");
                strSvn = md5.GetSvnFromServe(uiFilePath, "yuanmengjue", "yuanmengjue");
                outputInfoFile(System.AppDomain.CurrentDomain.BaseDirectory + "PicturePack", strMd5 + "\r" + strSvn);
                //

                MessageBox.Show("输出完成，请到原位图文件夹查看");
                progressBarSaving.Visible = false;
                progressBarSaving.Value = 0;
            }
        }

        private void textBoxOpenPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void textBoxSavingPath_TextChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxBatchProcess_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxBinFormat_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxCformat_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void progressBarSaving_Click(object sender, EventArgs e)
        {

        }

        private void richTextBoxOpenPaths_TextChanged(object sender, EventArgs e)
        {

        }

        private void buttonNext_Click(object sender, EventArgs e)
        {
            if (openFileNames == null)
            {
                return;
            }

            if (curFileIndex >= fileSum - 1)
            {
                curFileIndex = 0;
                pictureBoxCurrentPic.Image = Image.FromFile(openFileNames[curFileIndex]);
                return;
            }
            pictureBoxCurrentPic.Image = Image.FromFile(openFileNames[++curFileIndex]);
        }

        private void buttonPrevious_Click(object sender, EventArgs e)
        {
            if (openFileNames == null)
            {
                return;
            }

            if (curFileIndex == 0)
            {
                curFileIndex = fileSum;
            }
            pictureBoxCurrentPic.Image = Image.FromFile(openFileNames[--curFileIndex]);
        }

        private void openFileDialogPicMatrix_FileOk(object sender, CancelEventArgs e)
        {
        }
    }

    public class OperINI
    {

        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string value, string filePath);
        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string defValue, StringBuilder retvalue, int size, string filePath);

        public static int ReadINI(string section, string key, string defValue, StringBuilder retValue, int size, string filepath)
        {
            return GetPrivateProfileString(section, key, defValue, retValue, size, filepath);
        }

        public static long WriteINI(string section, string key, string value, string filePath)
        {
            return WritePrivateProfileString(section, key, value, filePath);
        }
        public static long DeleteSection(string section, string filePath)
        {
            return WritePrivateProfileString(section, null, null, filePath);
        }
        public static long DeleteKey(string section, string key, string filePath)
        {
            return WritePrivateProfileString(section, key, null, filePath);
        }
    }
}
