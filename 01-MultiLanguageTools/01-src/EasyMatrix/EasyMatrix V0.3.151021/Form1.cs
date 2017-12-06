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

#region PublicDataStruct

public struct AddrBytes
{
    public byte addr3;
    public byte addr2;
    public byte addr1;
    public byte addr0;
}

public struct LanInfo
{
    public byte height;
    public AddrBytes indexAreaAddr;
}

public struct CharFormat
{
    public int width;
    public int height;
}

public struct UnicodeBytes
{
    public byte h;
    public byte l;
}

#endregion

#region RefactoringDataStructOfMatrixSheet

public struct MatrixDatas
{
    public UnicodeBytes unicode;
    public int length;
    public int height;
    public int startZeroSum;
    public int stopZeroSum;
    public byte[] datas;
}

public struct BoolMatrix
{
    public int width;
    public int height;
    public bool[][] datas;
}

public struct ComplexMatrix
{
    public String str;
    public MatrixDatas md;
    public BoolMatrix bm;
}

public enum FilterMode
{
    WIDTH_FILTER,
    HEIGHT_FILTER,
    TOTAL_FILTER,
    SMART_HEIGHT_FILTER,
    NA
}

public struct MatrixSheet
{
    // 头
    public byte[] tag;
    public byte[] crc;
    public AddrBytes thisAddr;

    // 保留/配置
    public UnicodeBytes startUnicode;
    public UnicodeBytes stopUnicode;
    public byte height;
    public Font ft;
    public byte[] rev;

    // 索引区
    public UInt16 unicodeSum;//裁剪之后[掐头去尾]
    public AddrBytes[] index;

    // 字模数据区
    public MatrixDatas[] md;

    // 实际长度[存到FLASH中的，补齐之前的]
    public int realLength;
    public int mdLength;
}

#endregion

#region RefactoringDataStructOfGeneralCharacterMatrixSheet

public struct GeneralCharacterMatrixDatasIndex
{
    public byte scaleNumber;
    public byte idSum;
    public AddrBytes[] indexOfID;
}

public struct GeneralCharacterMatrixDatas
{
    public byte id;
    public byte length;
    public byte[] datasOfID;
}

public struct GeneralCharacterMatrixSheet
{
    // 头
    public byte[] tag;
    public byte[] crc;
    public AddrBytes thisAddr;

    // 保留
    public byte[] rev;

    // 字号信息
    public byte scaleSum;
    public LanInfo[] infoOfScale;

    // 索引区
    public GeneralCharacterMatrixDatasIndex[] gcmdiOfScale;

    // 字模数据区
    public GeneralCharacterMatrixDatas[] gcmdOfScale;

    // 实际长度[存到FLASH中的，补齐之前的]
    public int realLength;
}

#endregion

#region RefactoringDataStructOfStringSheet

public struct UnicodeSpliter
{
    public byte sum;
    public UnicodeBytes[] starter;
    public UnicodeBytes[] stoper;
}
public struct UnicodeStream
{
    public int length;
    public UnicodeBytes startUnicode;
    public UnicodeSpliter spliter;
    public UnicodeBytes stopUnicode;
}

// 字符编码串索引表
public struct StringIndexSheet
{
    public byte lanNum;
    public UInt16 idSum;

    public AddrBytes[] datas;
}

public struct SimplyUnicodeStream
{
    public UInt32 length;
    public UnicodeBytes[] u;
}

// 单个字符编码串
public struct StringDatas
{
    public UInt16 idNum;
    public SimplyUnicodeStream sus;
}

// 字符编码串数据表
public struct StringDatasSheet
{
    public byte lanNum;       // 第几个语种的编码串表
    public UInt16 idSum;
    public StringDatas[] sd;
}

// 字符编码串表语种信息
public struct StringLanInfo
{
    public byte lanNum;
    public byte height;       // 字高，联合字模数据长度计算字宽
    public AddrBytes datas;
}

// 字符编码串表
public struct StringSheet
{
    public byte[] tag;
    public byte[] crc;
    public AddrBytes ssAddr;

    public byte lanSum;             // 语种信息区域，值由读入Excel表格的列数决定，默认初始化为2
    public StringLanInfo[] sli;

    public StringIndexSheet[] sis;  // 大小由lanSum决定

    public StringDatasSheet[] sds;  // 大小由lanSum决定
    
    public UInt32 realLen;
}

// 文件参数
public struct FileParameters
{
    public string path;
    public string rowDatas;
    //public string fileName;

    // 左上角位置
    public UInt16 startLinNum;
    public UInt16 startColNum;
	
    // 从左上角开始，有效行列总数
    public UInt16 linSum;
    public UInt16 colSum;
	
    // 最大行列数，目前来看没什么作用
    public UInt16 maxLinNum;
    public UInt16 maxColNum;
}

public struct LanguagePack
{
    public byte[] tag;
    public byte[] crc;
    public AddrBytes thisAddr;

    public AddrBytes ssAddr;
    public AddrBytes msAddr;
    public AddrBytes dlAddr;
    public AddrBytes rev;

    public StringSheet ss;
    public MatrixSheet ms;
}

#endregion

#region 测试用数据结构

public struct PicMatrix {
    public UInt32 width;
    public UInt32 height;

    public UInt16[][] C565;
    public Color[][] color;
    public UInt32[][] C888;

}

#endregion


namespace EasyMatrix_V0._3._151021._1
{
    public partial class FormEasyMatrix : Form
    {
        FileParameters filePara;
        ComplexMatrix gCM;
        String gStr;
        Font gFont;
        int strPos;

        bool gFlag;
        AddrBytes nextAddr;       // 存储下一个字模的首地址(最终地址)
        //AddrBytes addrDefault;
        //string gStr = "搜索";
        


        public FormEasyMatrix()
        {
            InitializeComponent();

            gFont = labelCurrentChar.Font;
            strPos = textBoxInput.TextLength - 1;
            gStr = textBoxInput.Text;
            gCM = getRandomStringMatrixDatas(gStr, gFont);
            drawStringMatrix(gCM);
            drawBackground();
            displayCurrentPix(gCM.md);
            richTextBoxRandomHexDatas.Text = getHexFormatMatrixDatas(killZero(matrixFilter(getRandomStringMatrixDatas(gStr, getFontFromInputBox()), FilterMode.WIDTH_FILTER)));
        }


        #region 基础函数集

        private byte[] toBytes(UInt16 a)
        {
            byte[] bt = new byte[2];
            bt[0] = (byte)(a >> 8);
            bt[1] = (byte)a;
            return bt;
        }
        private byte[] toBytes(UInt32 a)
        {
            byte[] bt = new byte[4];
            bt[0] = (byte)(a >> 24);
            bt[1] = (byte)(a >> 16);
            bt[2] = (byte)(a >> 8);
            bt[3] = (byte)(a);
            return bt;
        }
        private byte[] toBytes(AddrBytes addr)
        {
            byte[] bt = new byte[4];
            bt[0] = addr.addr3;
            bt[1] = addr.addr2;
            bt[2] = addr.addr1;
            bt[3] = addr.addr0;

            return bt;
        }
        private UInt32 toUInt32(AddrBytes addr)
        {
            return (UInt32)((UInt32)addr.addr3 * 256 * 256 * 256 + (UInt32)addr.addr2 * 256 * 256 + (UInt32)addr.addr1 * 256 + (UInt32)addr.addr0);
        }
        private UInt16 toUInt16(UnicodeBytes ub)
        {
            return (UInt16)((UInt16)ub.h * 256 + ub.l);
        }
        private UnicodeBytes toUnicode(UInt16 a)
        {
            UnicodeBytes ub;
            ub.h = (byte)(a >> 8);
            ub.l = (byte)a;
            return ub;
        }
        // 以大端方式结合两个byte位unicode2Bytes
        private UnicodeBytes toUnicode(byte bt1, byte bt2)
        {
            UnicodeBytes u;
            u.h = bt1;
            u.l = bt2;
            return u;
        }
             
        // 以大端方式获取SimplyUnicodeStream
        private SimplyUnicodeStream toUnicodeStream(byte[] bt)
        {
            SimplyUnicodeStream us = new SimplyUnicodeStream();

            us.length = (UInt32)(bt.GetLength(0)/2); // 获取数组第一维的长度
            us.u = new UnicodeBytes[us.length]; 

            for (UInt32 i = 0; i < us.length; i++)
            {
                us.u[i] = toUnicode(bt[i * 2 + 0], bt[i * 2 + 1]);
            }

            return us;
        }

        private AddrBytes toAddrBytes(UInt32 a)
        {
            AddrBytes addr;
            addr.addr3 = toBytes(a)[0];
            addr.addr2 = toBytes(a)[1];
            addr.addr1 = toBytes(a)[2];
            addr.addr0 = toBytes(a)[3];

            return addr;
        }
        private AddrBytes add(AddrBytes addr1, AddrBytes addr2)
        {
            return (toAddrBytes(toUInt32(addr1) + toUInt32(addr2)));
        }
        private AddrBytes min(AddrBytes addr1, AddrBytes addr2)
        {
            return (toAddrBytes(toUInt32(addr1) - toUInt32(addr2)));
        }
        private void eraseFile(String filePath)
        {
            File.WriteAllText(filePath, "");
        }
        private int getMaxLength(MatrixSheet ms)
        {
            int len = 0;
            for (int i = 0; i < ms.mdLength; i++)
            {
                len = len > ms.md[i].length ? len : ms.md[i].length;
            }
            return len;
        }

        #endregion

        #region 字模提取及自适应宽度函数

        #region 构图函数

        private void displayCurrentPix(MatrixDatas md)
        {
            String strBold = gFont.Bold ? ", 粗体, " : ", 常规, ";
            labelFontSizePx.Text = gFont.Name + strBold
                    + gFont.Size + "pt, "
                    + (md.length * 8 / md.height).ToString() + "px × " + (md.height).ToString() + "px";
        }
        private String getHexFormatMatrixDatas(MatrixDatas md)
        {
            //MatrixDatas md = matrixFilter(getRandomStringMatrixDatas(textBoxInput.Text, getFontFromInputBox()), FilterMode.WIDTH_FILTER);
            displayCurrentPix(md);
            StringBuilder sb = new StringBuilder();
            sb.Append(md.height.ToString() + ",\r\n");
            sb.Append(md.length.ToString() + ",");
            for (int i = 0; i < md.length; i++)
            {
                if (i % 8 == 0)
                {
                    sb.Append("\r\n");
                }

                sb.Append("0x");
                if (md.datas[i] < 16)
                {
                    sb.Append("0");
                }
                sb.Append(md.datas[i].ToString("X"));

                if (i < md.length - 1)
                {
                    sb.Append(", ");
                }
            }

            return sb.ToString();
        }
        private float getDiv()
        {
            // 1 inch = 72 pt
            // dpi < > px/inch
            // px = (pt/72)*dpi = (dpi/72)* pt = div * pt;
            // 获取换算参数
            float div;

            if (getScreenDpi()[0] != getScreenDpi()[1])
            {
                MessageBox.Show("Warning", "X轴方向与Y轴方向分辨率不同，点击确定以X轴分辨率为准");
            }

            div = 72.0f / (float)getScreenDpi()[0];

            return div;
        }
        private UInt32[] getScreenDpi()
        {
            UInt32[] dpi = new UInt32[2];
            using (Graphics graphics = Graphics.FromHwnd(IntPtr.Zero))
            {
                dpi[0] = (UInt32)graphics.DpiX;
                dpi[1] = (UInt32)graphics.DpiY;
            }

            return dpi;
        }
        private Rectangle getScreenPx()
        {
            Rectangle rect = Screen.PrimaryScreen.Bounds;
            return rect;
        }
        private UInt32 ptToPx(UInt32 pt)
        {
            return (UInt32)(pt / getDiv());
        }
        private UInt32 pxToPt(UInt32 px)
        {
            return (UInt32)(px * getDiv());
        }
        private char getCharFromInputBox()
        {
            char ch = textBoxInput.TextLength >= 1 ? textBoxInput.Text[textBoxInput.TextLength - 1] : '\0';
            return ch;
        }
        private Font getFontFromInputBox()
        {
            return gFont;
        }
        private CharFormat getCharFormat(char ch, Font ft)
        {
            CharFormat cf = new CharFormat();

            Graphics g = this.CreateGraphics();
            SizeF sf = g.MeasureString(ch.ToString(), ft);
            cf.width = (int)sf.Width;
            cf.height = (int)sf.Height;

            return cf;
        }
        private CharFormat getStringFormat(String str, Font ft)
        {
            CharFormat cf = new CharFormat();

            Graphics g = this.CreateGraphics();
            SizeF sf = g.MeasureString(str, ft);
            cf.width = (int)sf.Width;
            cf.height = (int)sf.Height;

            return cf;
        }
        private CharFormat getTxTFormat(String str, Font ft)
        {
            CharFormat cf = new CharFormat();

            SizeF sf = TextRenderer.MeasureText(str, ft);
            cf.width = (int)(sf.Width);
            cf.height = (int)(sf.Height);

            return cf;
        }
        private void drawCharacter(char ch, Font ft)
        {
            CharFormat cf = getCharFormat(ch, ft);
            Graphics g = this.CreateGraphics();
            Bitmap bmp = new Bitmap(cf.width, cf.height);
            this.pictureBoxDrawingPicture.Image = bmp;
            g = Graphics.FromImage(bmp);
            g.FillRectangle(Brushes.White, 0, 0, cf.width, cf.height);

            g.DrawString(ch.ToString(), ft, Brushes.Black, new PointF(0, 0));
            //g.DrawString(str, ft, Brushes.Blue, new PointF(0, 0));

            g.Dispose();
        }
        private void drawBackground()
        {
            Graphics g;
            Bitmap bmp = new Bitmap(pictureBoxBKG.Width, pictureBoxBKG.Height);
            this.pictureBoxBKG.Image = bmp;
            g = Graphics.FromImage(bmp);
            g.FillRectangle(Brushes.White, 0, 0,
                    (float)bmp.Width, (float)bmp.Height);
            g.DrawRectangle(Pens.MidnightBlue, 0, 0,
                    (float)bmp.Width - 1, (float)bmp.Height - 1);
            g.Dispose();
        }
        private void drawStringMatrix(ComplexMatrix cm)
        {
            Graphics g;
            Bitmap bmp = new Bitmap(pictureBoxViewer.Width, pictureBoxViewer.Height);
            this.pictureBoxViewer.Image = bmp;
            g = Graphics.FromImage(bmp);
            g.FillRectangle(Brushes.White, 0, 0,
                    (float)bmp.Width, (float)bmp.Height);

            float pixRectWidth = 4f;
            float pixRectHeight = 4f;
            float pixRectXPos = 1f;
            float pixRectYPos = 1f;
            float offSet = pixRectHeight + 1f;

            int x = 0;
            int y = 0;
            for (; x < cm.bm.width; x++, pixRectXPos += offSet)
            {
                y = 0;
                pixRectYPos = 1;
                for (; y < cm.bm.height; y++, pixRectYPos += offSet)
                {
                    //Brush br = cm.bm.datas[y][x] ? Brushes.MidnightBlue : Brushes.LightGray;
                    Brush br = cm.bm.datas[y][x] ? Brushes.Black : Brushes.White;

                    g.FillRectangle(br, pixRectXPos, pixRectYPos,
                            pixRectWidth, pixRectHeight);

                }
                for (; pixRectYPos < bmp.Height; pixRectYPos += offSet)
                {
                    //g.FillRectangle(Brushes.LightGray, pixRectXPos, pixRectYPos,
                    //    pixRectWidth, pixRectHeight);
                    g.FillRectangle(Brushes.White, pixRectXPos, pixRectYPos,
                        pixRectWidth, pixRectHeight);
                }
            }

            for (; pixRectXPos < bmp.Width; pixRectXPos += offSet)
            {
                pixRectYPos = 1;
                for (; pixRectYPos < bmp.Height; pixRectYPos += offSet)
                {
                    //g.FillRectangle(Brushes.LightGray, pixRectXPos, pixRectYPos,
                    //        pixRectWidth, pixRectHeight);
                    g.FillRectangle(Brushes.White, pixRectXPos, pixRectYPos,
                            pixRectWidth, pixRectHeight);
                }
            }



            g.DrawRectangle(Pens.MidnightBlue, 0, 0,
                    (float)bmp.Width - 1, (float)bmp.Height - 1);
            g.Dispose();
        }

        #endregion

        private ComplexMatrix getRandomStringMatrixDatas(String str, Font ft)
        {
            // 画字符
            if (str.Length < 1)
            {
                str = " ";
            }
            CharFormat cf = getStringFormat(str, ft);
            Graphics g = this.CreateGraphics();
            Bitmap bmp = new Bitmap(cf.width, cf.height);
            this.pictureBoxDrawingPicture.Image = bmp;
            g = Graphics.FromImage(bmp);
            g.FillRectangle(Brushes.White, 0, 0, cf.width, cf.height);
            g.DrawString(str, ft, Brushes.Black, new PointF((int)numericUpDownXPos.Value, (int)numericUpDownYPos.Value));

            ComplexMatrix cm = new ComplexMatrix();
            cm.str = str;
            cm.md = new MatrixDatas();
            cm.md.length = (cf.width * cf.height) % 8 == 0 ? (cf.width * cf.height) / 8 : (cf.width * cf.height) / 8 + 1;
            cm.md.height = cf.height;
            cm.md.datas = new byte[cm.md.length];

            cm.bm = new BoolMatrix();
            cm.bm.width = cf.width;
            cm.bm.height = cf.height;
            cm.bm.datas = new bool[cm.bm.height][];

            byte tmp = 0x00;
            byte directControl = 0x01;          // 左移为左低右高
            for (int y = 0, i = 0; y < cf.height; y++)
            {
                cm.bm.datas[y] = new bool[cm.bm.width];
                for (int x = 0; x < cf.width; x++, i++)
                {
                    if (i % 8 == 0)
                    {
                        cm.md.datas[i / 8] = 0x00;
                        directControl = 0x01;
                        tmp = 0x00;
                    }

                    //if (bmp.GetPixel(x, y).GetBrightness() > 0.5f)
                    if (bmp.GetPixel(x, y).GetBrightness() >= 0.8f)
                    {
                        tmp = 0x00;
                        cm.bm.datas[y][x] = false;
                    }
                    else
                    {
                        tmp = 0xFF;
                        tmp &= directControl;
                        cm.bm.datas[y][x] = true;
                    }

                    directControl <<= 1;
                    cm.md.datas[i / 8] |= tmp;
                }
            }

            return cm;
        }
        private ComplexMatrix getSingleCharMatrixDatas(char ch, Font ft)
        {
            return getRandomStringMatrixDatas(ch.ToString(), ft);
        }
        private MatrixDatas matrixFilter(ComplexMatrix cm, FilterMode fm)
        {
            int widthStart = 0, widthStop = cm.bm.width - 1;
            int heightStart = 0, heightStop = cm.bm.height - 1;

            if (cm.str == " ")
            {
                widthStop = cm.bm.width / 2;
                goto FilterCompleted;
            }
            if (fm == FilterMode.NA)
            {
                return cm.md;
            }

            #region 限宽
            if (fm != FilterMode.HEIGHT_FILTER)
            {
                for (int i = 0; i < cm.bm.width; i++)
                {
                    for (int j = 0; j < cm.bm.height; j++)
                    {
                        if (cm.bm.datas[j][i])
                        {
                            if (i > 0)
                            {
                                widthStart = i - 1;
                            }
                            else
                            {
                                widthStart = i;
                            }

                            i = cm.bm.width;
                            break;
                        }
                    }
                }

                for (int i = cm.bm.width - 1; i >= 0; i--)
                {
                    for (int j = 0; j < cm.bm.height; j++)
                    {
                        if (cm.bm.datas[j][i])
                        {
                            if (i < cm.bm.width - 1)
                            {
                                widthStop = i + 1;
                            }
                            else
                            {
                                widthStop = i;
                            }

                            i = -1;
                            break;
                        }
                    }
                }
            }
            #endregion

            #region 限高
            if (fm != FilterMode.WIDTH_FILTER)
            {
                for (int i = 0; i < cm.bm.height; i++)
                {
                    for (int j = 0; j < cm.bm.width; j++)
                    {
                        if (cm.bm.datas[i][j])
                        {
                            if (i > 0)
                            {
                                heightStart = i - 1;
                            }
                            else
                            {
                                heightStart = i;
                            }

                            i = cm.bm.height;
                            break;
                        }
                    }
                }

                for (int i = cm.bm.height - 1; i >= 0; i--)
                {
                    for (int j = 0; j < cm.bm.width; j++)
                    {
                        if (cm.bm.datas[i][j])
                        {
                            if (i < cm.bm.height - 1)
                            {
                                heightStop = i + 1;
                            }
                            else
                            {
                                heightStop = i;
                            }

                            i = -1;
                            break;
                        }
                    }
                }
            }

            if (fm == FilterMode.SMART_HEIGHT_FILTER)
            {
                if ((cm.bm.height - 1 - heightStop) < heightStart)
                {
                    heightStart = (cm.bm.height - 1 - heightStop);
                }
                else
                {
                    heightStop = (cm.bm.height - 1 - heightStart);
                }
            }

            #endregion

        FilterCompleted:

            MatrixDatas md = new MatrixDatas();
            //md.height = dm.bm.height;
            md.height = (heightStop - heightStart + 1);
            md.length = (widthStop - widthStart + 1) * md.height % 8 == 0
                      ? (widthStop - widthStart + 1) * md.height / 8
                      : (widthStop - widthStart + 1) * md.height / 8 + 1;

            md.datas = new byte[md.length];

            byte tmp = 0x01, tmp2 = 0x00;
            for (int j = 0, k = 0; j < md.height; j++)
            {
                for (int i = 0; i < widthStop - widthStart + 1; i++, k++)
                {
                    if (k % 8 == 0)
                    {
                        md.datas[k / 8] = 0x00;
                        tmp = 0x01;
                        tmp2 = 0x00;
                    }

                    if (!cm.bm.datas[j + heightStart][i + widthStart])
                    {
                        tmp2 = 0x00;
                    }
                    else
                    {
                        tmp2 = 0xFF;
                        tmp2 &= tmp;
                    }

                    tmp <<= 1;
                    md.datas[k / 8] |= tmp2;
                }
            }

            return md;
        }

        #endregion

        #region 字模表相关函数

        private MatrixSheet creatMatrixSheet(AddrBytes thisAddr, UnicodeStream us)
        {
            MatrixSheet ms;

            // 头
            ms.tag = Encoding.ASCII.GetBytes("Matrix Tag");
            ms.crc = Encoding.ASCII.GetBytes("**");
            ms.thisAddr = thisAddr;

            // 保留/配置
            ms.startUnicode = us.startUnicode;
            ms.stopUnicode = us.stopUnicode;
            ms.height = 0;
            ms.ft = gFont;
            ms.rev = Encoding.ASCII.GetBytes("Reserved...");

            // 索引区
            ms.unicodeSum = (UInt16)(toUInt16(ms.stopUnicode) - toUInt16(ms.startUnicode) + 1);
            ms.index = new AddrBytes[ms.unicodeSum];

            // 实际长度
            ms.realLength = ms.unicodeSum * 4 + 0x10;
            ms.mdLength = 0;
            AddrBytes tmpAddr = toAddrBytes((UInt32)ms.realLength);

            // 字模数据区
            UInt16 tmpLen = ms.unicodeSum;
            UInt16 tmpUnicode = toUInt16(ms.startUnicode);
            for (int i = 0; i < us.spliter.sum; i++)
            {
                tmpLen -= (UInt16)(toUInt16(us.spliter.stoper[i]) - toUInt16(us.spliter.starter[i]) + 1);
            }
            ms.md = new MatrixDatas[tmpLen];
            for (int i = 0; i < tmpLen; i++, tmpUnicode++)
            {
                for (int j = 0; j < us.spliter.sum; j++)
                {
                    if ((tmpUnicode >= toUInt16(us.spliter.starter[j])) && (tmpUnicode <= toUInt16(us.spliter.stoper[j])))
                    {
                        tmpUnicode = (UInt16)(toUInt16(us.spliter.stoper[j]) + 1);
                        break;
                    }
                }
                //ms.md[i] = matrixFilter(getRandomStringMatrixDatas(Encoding.BigEndianUnicode.GetString(toBytes(tmpUnicode)), ms.ft), FilterMode.WIDTH_FILTER);
                ms.md[i] = killZero(matrixFilter(getRandomStringMatrixDatas(Encoding.BigEndianUnicode.GetString(toBytes(tmpUnicode)), ms.ft), FilterMode.WIDTH_FILTER));
                ms.index[tmpUnicode - toUInt16(ms.startUnicode)] = tmpAddr;
                tmpAddr = add(tmpAddr, toAddrBytes((UInt32)(ms.md[i].length + 2)));
                ms.md[i].unicode = toUnicode(tmpUnicode);
                ms.realLength += (ms.md[i].length + 2);
                ms.height = (byte)ms.md[i].height;
                ms.mdLength++;
            }

            //ms.height = 0x00;

            return ms;
        }
        private byte[] toBytes(MatrixSheet ms)
        {

            // checkSum
            //UInt32 checkSum = 0;

            // 补齐
            while ((ms.realLength - 0x10) % 256 != 0)
            {
                ms.realLength++;
            }

            byte[] bt = new byte[ms.realLength - 0x10];
            int i = 0, j;

            /*
            // 头
            for (j = 0; j < 10; j++)
            {
                bt[i++] = ms.tag[j];
            }
            bt[i++] = ms.crc[0];
            bt[i++] = ms.crc[1];
            bt[i++] = ms.thisAddr.addr3;
            bt[i++] = ms.thisAddr.addr2;
            bt[i++] = ms.thisAddr.addr1;
            bt[i++] = ms.thisAddr.addr0;
            */

            // 配置/保留
            bt[i++] = toBytes(toUInt16(ms.startUnicode))[0];
            bt[i++] = toBytes(toUInt16(ms.startUnicode))[1];
            bt[i++] = toBytes(toUInt16(ms.stopUnicode))[0];
            bt[i++] = toBytes(toUInt16(ms.stopUnicode))[1];
            bt[i++] = ms.height;
            for (j = 0; j < 11; j++)
            {
                bt[i++] = ms.rev[j];
            }

            // 索引区
            for (j = 0; j < ms.unicodeSum; j++)
            {
                bt[i++] = ms.index[j].addr3;
                bt[i++] = ms.index[j].addr2;
                bt[i++] = ms.index[j].addr1;
                bt[i++] = ms.index[j].addr0;
            }

            // 字模数据区
            for (j = 0; j < ms.mdLength; j++)
            {
                bt[i++] = (byte)(ms.md[j].length/256);
                bt[i++] = (byte)ms.md[j].length;
                for (int k = 0; k < ms.md[j].length; k++)
                {
                    bt[i++] = ms.md[j].datas[k];
                }
            }
            for (; i < ms.realLength - 0x10; )
            {
                bt[i++] = 0xFF;
            }
            // realLength
            /*
            bt[0x1C] = toAddrBytes((UInt32)ms.realLength).addr3;
            bt[0x1D] = toAddrBytes((UInt32)ms.realLength).addr2;
            bt[0x1E] = toAddrBytes((UInt32)ms.realLength).addr1;
            bt[0x1F] = toAddrBytes((UInt32)ms.realLength).addr0;
            
            
            // 表头checkSum
            checkSum = 0;
            for(i=0; i<0x20; i++)
            { 
                if((i != 0x1A) && (i != 0x1B))
                {
                    checkSum += (UInt32)bt[i];
                }
            }
            bt[0x1A] = toAddrBytes((UInt32)checkSum).addr1;
            bt[0x1B] = toAddrBytes((UInt32)checkSum).addr0;
            */
            return bt;
        }
        private void outputMatrixSheet(MatrixSheet ms, String fileName)
        {
            //ms.thisAddr = headAddr;
            if (ms.realLength > (0x400000 - 0x10))
            { 
                MessageBox.Show("文件大小超过4M，请重新设置字体参数");
                return;
            }
            try
            {
                BinaryWriter bw;
                String filePath = fileName + ".msb";
                eraseFile(filePath);
                FileInfo fi = new FileInfo(filePath);
                using (bw = new BinaryWriter(fi.OpenWrite()))
                {
                    bw.Write(toBytes(ms));
                    bw.Flush();
                    bw.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Warning!");
            }
            MessageBox.Show("文件输出完成！");
        }
        private void outputMatrixSheetCFormat(MatrixSheet ms, String fileName)
        {
            StringBuilder sb1 = new StringBuilder();
            string str;
            byte[] bt = new byte[2];

            //sb1.Append("");
            for (UInt32 i = 0; i < ms.mdLength; i++)
            {
                bt[0] = ms.md[i].unicode.h;
                bt[1] = ms.md[i].unicode.l;

                sb1.Append(
                        "\n\t/*-- U+" +
                        (bt[0].ToString("X2")) +
                        (bt[1].ToString("X2")) +
                        "[BigEndian], \"" +
                        (Encoding.BigEndianUnicode.GetString(bt)) +
                        "\", " +
                        ms.ft.Name.ToString() +
                        ", " +
                        (ms.md[i].length * 8 / ms.md[i].height) +
                        "px × " +
                        ms.md[i].height +
                        "px -----------------*/\n\t{0x" +
                        (ms.md[i].length.ToString("X4")) +
                        ", ");

                sb1.Append(
                        "\n\t 0x" +
                        (ms.md[i].datas[0].ToString("X2")) +
                        (ms.md[i].datas[1].ToString("X2")) +
                        ", ");

                sb1.Append(
                        "0x" +
                        (ms.md[i].datas[2].ToString("X2")) +
                        (ms.md[i].datas[3].ToString("X2")) +
                        ", ");

                for (UInt32 j = 0; j < ms.md[i].length - 4; j++)
                {
                    if (j % 8 == 0)
                    {
                        sb1.Append("\n\t ");
                    }

                    sb1.Append(
                            "0x" +
                            (ms.md[i].datas[j + 4].ToString("X2")));

                    if (j != (ms.md[i].length - 5))
                    {
                        sb1.Append(", ");
                    }
                }
                sb1.Append("\n\t }");
                if (i != (ms.mdLength - 1))
                {
                    sb1.Append(",");
                }
                sb1.Append("\n");
            }

            str = sb1.ToString();
            StringBuilder sb = new StringBuilder();
            sb.Append(
                    "#define MAX_LEN_OF_MATRIX_DATAS " +
                    (getMaxLength(ms) + 1) +
                    "\t//\t单个字模的最大长度\n\n" +
                    "typedef struct tagMatrixDatas{" +
                    "\t//\t单个字模的信息" +
                    "\n\tUINT16\tlength;\n\tUINT16\tpreZero;\n\tUINT16\tpostZero;\n\tUINT08\tdatas[MAX_LEN_OF_MATRIX_DATAS];\n}MatrixDatas;" +
                    "\n\n" +
                    "MatrixDatas md[] = {\n\t" +
                    (str) +
                    "\n\t};");

            StreamWriter sw = new System.IO.StreamWriter(fileName + ".c");
            sw.Write(sb.ToString());
            sw.Close();
        }

        #endregion

        #region 字符编码串相关函数

        private UnicodeStream getUnicodeStream()
        {
            UnicodeStream us = new UnicodeStream();

            return us;
        }
        private String randomFilter()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(textBoxInput.Text[0]);
            int realLength = 1;
            bool flag = true;

            for (int i = 1; i < textBoxInput.Text.Length; i++)
            {
                for (int j = i; j > 0; j--)
                {
                    if (textBoxInput.Text[i] == textBoxInput.Text[j - 1])
                    {
                        flag = false;
                        break;
                    }
                    flag = true;
                }
                if (flag)
                {
                    sb.Append(textBoxInput.Text[i]);
                    realLength++;
                }
            }

            return sb.ToString();
        }
        private UnicodeStream unicodeStreamFilter()
        {
            UnicodeStream us = new UnicodeStream();

            switch (comboBoxCharSet.Text)
            {
                case "基本ASCII字符集":
                    {
                        us.startUnicode = toUnicode(0x0020);
                        us.stopUnicode = toUnicode(0x007F);

                        us.spliter = new UnicodeSpliter();
                        us.spliter.sum = 0;

                        break;
                    }
                case "扩展ASCII字符集":
                    {
                        us.startUnicode = toUnicode(0x0020);
                        us.stopUnicode = toUnicode(0x007F);

                        us.spliter = new UnicodeSpliter();
                        us.spliter.sum = 0;

                        break;
                    }
                case "中英文通用字符集":
                    {
                       us.startUnicode = toUnicode(0x0020);
                        us.stopUnicode = toUnicode(0x9FBF);

                        us.spliter = new UnicodeSpliter();
                        us.spliter.sum = 1;
                        us.spliter.starter = new UnicodeBytes[us.spliter.sum];
                        us.spliter.stoper = new UnicodeBytes[us.spliter.sum];
                        us.spliter.starter[0] = toUnicode(0x0080);
                        us.spliter.stoper[0] = toUnicode(0x4DFF);

                        break;
                    }
                case "中日韩(CJK)基本字符集":
                    {
                        us.startUnicode = toUnicode(0x4E00);
                        us.stopUnicode = toUnicode(0x9FBF);

                        us.spliter = new UnicodeSpliter();
                        us.spliter.sum = 0;

                        break;
                    }
                case "中日韩(CJK)扩展字符集":
                    {
                        us.startUnicode = toUnicode(0x3400);
                        us.stopUnicode = toUnicode(0x9FBF);

                        us.spliter = new UnicodeSpliter();
                        us.spliter.sum = 1;
                        us.spliter.starter = new UnicodeBytes[us.spliter.sum];
                        us.spliter.stoper = new UnicodeBytes[us.spliter.sum];
                        us.spliter.starter[0] = toUnicode(0x4DC0);
                        us.spliter.stoper[0] = toUnicode(0x4DFF);

                        break;
                    }
                case "日文平假名与片假名":
                    {
                        us.startUnicode = toUnicode(0x3040);
                        us.stopUnicode = toUnicode(0x30FF);

                        us.spliter = new UnicodeSpliter();
                        us.spliter.sum = 0;

                        break;
                    }
                case "全Unicode字符集":
                    {
                        us.startUnicode = toUnicode(0x0020);
                        us.stopUnicode = toUnicode(0xFFFF);

                        us.spliter = new UnicodeSpliter();
                        us.spliter.sum = 0;

                        break;
                    }
                default:
                    {
                        us.startUnicode = toUnicode(0x0020);
                        us.stopUnicode = toUnicode(0x007F);

                        us.spliter = new UnicodeSpliter();
                        us.spliter.sum = 0;

                        break;
                    }
            }

            us.length = toUInt16(us.stopUnicode) - toUInt16(us.startUnicode);

            return us;
        }

        #region 150205 addition@Excel

               
        // 将Excel中的数据读入缓存
        public DataSet ExcelToDS(string path)
        {
            // 建立联结
            string strConn = "Provider=Microsoft.Jet.OLEDB.4.0;"
                             + "Data Source=" + path + ";"
                             + "Extended Properties='Excel 8.0;HDR=Yes;IMEX=1;'";   // 从冻结单元格左上角相邻的单元格开始计数(第0行的第0列)

            OleDbConnection conn = new OleDbConnection(strConn);
            conn.Open();

            string strExcel = "";
            OleDbDataAdapter myCommand = null;
            DataSet ds = null;
            strExcel = "select * from [StringLib$]";	                            // Sheet1$更名为StringLib$
            myCommand = new OleDbDataAdapter(strExcel, strConn);
            ds = new DataSet();
            myCommand.Fill(ds, "table1");
            return ds; 
        }

        // 读EXCLE
        private StringSheet loadDataSet(DataSet ds, AddrBytes ab)
        {
            filePara.maxColNum = (UInt16)ds.Tables[0].Columns.Count;
            filePara.maxLinNum = (UInt16)ds.Tables[0].Rows.Count;

            //AddrBytes ab = toAddrBytes(0x00000040);
                	
            StringSheet ss = creatStringSheet((byte)(filePara.maxColNum - 2),(UInt16)(filePara.maxLinNum - 1),ab,0xF0);
            UInt32 tmpIDSUM = (UInt16)(filePara.maxLinNum - 1); // 待修正
                        
            //(0x10+ss.lanSum*4+0x01)索引区首地址
            //nextAddr第一个语种ID0字串的首地址
            nextAddr = toAddrBytes((UInt32)(0x10 + ss.lanSum * 4 + 0x01) + (UInt32)(ss.lanSum * tmpIDSUM * 4));
                	
            // 
            for(byte i=0; i<ss.lanSum; i++)
            {
                ss.sli[i].lanNum = i;

                for(UInt16 j=0; j<ss.sds[0].idSum; j++)
                {
                    ss.sis[i].lanNum = i;
                    ss.sis[i].idSum = ss.sds[0].idSum;  // 有待完善
                    ss.sis[i].datas[j] = add(nextAddr, ss.ssAddr);

                    ss.sds[i].idSum = ss.sis[i].idSum;
                    ss.sds[i].lanNum = ss.sis[i].lanNum;
                    ss.sds[i].sd[j].idNum = j;
                                        
                    //ss.sds[i].sd[j].us.length = (UInt32)(Encoding.BigEndianUnicode.GetByteCount(ds.Tables[0].Rows[j + 1][i + 3].ToString()) / 2);
                    ss.sds[i].sd[j].sus = toUnicodeStream(Encoding.BigEndianUnicode.GetBytes(ds.Tables[0].Rows[j + 1][i + 2].ToString()));
                    nextAddr = add(
                            nextAddr,
                            toAddrBytes((UInt32)(ss.sds[i].sd[j].sus.length * 2 + 1)));
                }
            }
            
            //
            StringBuilder sb = new StringBuilder();
            for (UInt16 i = 0; i < ss.sds[0].idSum; i++)
            {
                if (ds.Tables[0].Rows[i + 1][2].ToString() == null)
                {
                    break;
                }
                sb.Append(ds.Tables[0].Rows[i + 1][2]);
            } 

            //DataColumn dc in ds.Tables[0].Columns;
            filePara.rowDatas = sb.ToString();

            // 待修改
            ss.realLen = 0;
            ss.realLen += (UInt32)(0x10 + 0x05 * ss.lanSum + 0x01);
            for (UInt32 i = 0; i < ss.lanSum; i++)
            {
                for (UInt32 j = 0; j < ss.sis[i].idSum; j++)
                {
                    ss.realLen += 0x04;
                }
            }
            for (UInt32 i = 0; i < ss.lanSum; i++)
            {
                for (UInt32 j = 0; j < ss.sds[i].idSum; j++)
                {
                    ss.realLen++;
                    for (UInt32 k = 0; k < ss.sds[i].sd[j].sus.length; k++)
                    {
                        ss.realLen+=2;
                    }
                }
            }

            return ss;
        }

        private StringSheet creatStringSheet(byte lanSum, UInt16 idSum, AddrBytes ssAddr, byte sdLength)
        {
            StringSheet ss = new StringSheet();
            byte DefaultHeight = 32;    // 与Font有关系

            // tag
            ss.tag = new byte[10];
            ss.tag = Encoding.ASCII.GetBytes("StringArea");

            // crc
            ss.crc = new byte[2];
            ss.crc[0] = 0xFF;
            ss.crc[1] = 0xFF;

            // ssAddr
            ss.ssAddr = ssAddr;         // 由用户指定，需要添加设定框，默认0x00000040

            // lanSum
            ss.lanSum = lanSum;         // 由Excel中读取到的列数决定，默认为2

            // sli;
            ss.sli = new StringLanInfo[ss.lanSum];
            for (byte i = 0; i < ss.lanSum; i++)
            {
                ss.sli[i].lanNum = i;
                ss.sli[i].height = DefaultHeight;
                ss.sli[i].datas =
                        add(
                        add(
                        add(
                        ss.ssAddr, toAddrBytes(0x10 + 0x01)),
                        toAddrBytes((UInt32)ss.lanSum * 0x04)),
                        toAddrBytes((UInt32)idSum * 4 * i));
            }

            // sis
            ss.sis = new StringIndexSheet[ss.lanSum];
            for (byte i = 0; i < ss.lanSum; i++)
            {
                ss.sis[i].lanNum = i;
                ss.sis[i].idSum = idSum;                            // 此处为用户设定，即，从EXCEL表中列数获取

                ss.sis[i].datas = new AddrBytes[ss.sis[i].idSum];
                for (UInt16 j = 0; j < ss.sis[i].idSum; j++)
                {
                    ss.sis[i].datas[j] = toAddrBytes(0xF0000000);   // 4字节地址格式
                }
            }

            // sds
            ss.sds = new StringDatasSheet[ss.lanSum];
            for (byte i = 0; i < ss.lanSum; i++)
            {
                ss.sds[i].lanNum = i;
                ss.sds[i].idSum = idSum;

                ss.sds[i].sd = new StringDatas[ss.sds[i].idSum];
                for (UInt16 j = 0; j < ss.sds[i].idSum; j++)
                {
                    ss.sds[i].sd[j].idNum = i;
                    ss.sds[i].sd[j].sus.length = sdLength;
                                        
                    ss.sds[i].sd[j].sus.u = new UnicodeBytes[ss.sds[i].sd[j].sus.length];

                    for (byte k = 0; k < ss.sds[i].sd[j].sus.length; k++)
                    {
                        ss.sds[i].sd[j].sus.u[k] = toUnicode(0x00, 0x31);// '0'
                    }
                }
            }

            return ss;
        }

         // 将字符编码串表转换为字节流
        private byte[] toBytes(StringSheet ss)
         {
             UInt32 i = ss.realLen;
             while ((i % 0x10) != 0)
             {
                 i++;
             }

             byte[] bt = new byte[i];
             i = 0;

             for (; i < 10; i++)
             {
                 bt[i] = ss.tag[i];
             }

             bt[i++] = ss.crc[0];
             bt[i++] = ss.crc[1];
             bt[i++] = ss.ssAddr.addr3;
             bt[i++] = ss.ssAddr.addr2;
             bt[i++] = ss.ssAddr.addr1;
             bt[i++] = ss.ssAddr.addr0;

             bt[i++] = ss.lanSum;

             for (byte j = 0; j < ss.lanSum; j++)
             {
                 //bt[i++] = ss.sli[j].height;
                 bt[i++] = ss.sli[j].datas.addr3;
                 bt[i++] = ss.sli[j].datas.addr2;
                 bt[i++] = ss.sli[j].datas.addr1;
                 bt[i++] = ss.sli[j].datas.addr0;
             }

             for (byte j = 0; j < ss.lanSum; j++)
             {
                 for (UInt32 k = 0; k < ss.sis[j].idSum; k++)
                 {
                     bt[i++] = ss.sis[j].datas[k].addr3;
                     bt[i++] = ss.sis[j].datas[k].addr2;
                     bt[i++] = ss.sis[j].datas[k].addr1;
                     bt[i++] = ss.sis[j].datas[k].addr0;
                 }
             }

             for (byte j = 0; j < ss.lanSum; j++)
             {
                 for (UInt32 k = 0; k < ss.sds[j].idSum; k++)
                 {
                     bt[i++] = (byte)ss.sds[j].sd[k].sus.length;
                     for (byte l = 0; l < ss.sds[j].sd[k].sus.length; l++)
                     {
                         bt[i++] = ss.sds[j].sd[k].sus.u[l].h;

                         if (i >= ss.realLen - 1)
                         {
                             bt[i] = ss.sds[j].sd[k].sus.u[l].l;
                             break;
                         }

                         bt[i++] = ss.sds[j].sd[k].sus.u[l].l;
                     }
                 }
             }

             // 加校验
             byte[] crc = new byte[2];
             //crc = calcCRC16(bt, 0x10, (UInt32)(bt.Length - 1));
             bt[10] = crc[0];
             bt[11] = crc[1];

             return bt;
         }

        private void outputStringSheetBinaryFile(StringSheet ss, string fileName)
        {
            try
            {
                BinaryWriter bw;
                string sFilePath = fileName + ".ssb";
                eraseFile(sFilePath);
                FileInfo fi = new FileInfo(sFilePath);
                using (bw = new BinaryWriter(fi.OpenWrite()))
                {
                    bw.Write(toBytes(ss));
                    bw.Flush();
                    bw.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Warning!");
            }
        }

        private void outputLanguagePackBinaryFile(string fileName)
        {
            LanguagePack lph = new LanguagePack();
            lph.tag = Encoding.ASCII.GetBytes("LanPackTag");
            lph.crc = new byte[2];

            lph.thisAddr = toAddrBytes((UInt32)numericUpDownFlashAddr.Value);
            lph.ssAddr = toAddrBytes(0x20);

            lph.ss = loadDataSet(ExcelToDS(filePara.path), add(lph.thisAddr, lph.ssAddr));

            lph.msAddr = add(lph.ssAddr, toAddrBytes((UInt16)toBytes(lph.ss).Length));
            lph.ms = creatMatrixSheet(add(lph.thisAddr,lph.msAddr), unicodeStreamFilter());

            lph.dlAddr = new AddrBytes();
            lph.rev = new AddrBytes();

            try
            {
                BinaryWriter bw;
                string sFilePath = fileName + ".lp";
                eraseFile(sFilePath);
                FileInfo fi = new FileInfo(sFilePath);
                using (bw = new BinaryWriter(fi.OpenWrite()))
                {
                    
                    bw.Write(lph.tag);
                    bw.Write(lph.crc);
                    bw.Write(toBytes(lph.thisAddr));
                    bw.Write(toBytes(lph.ssAddr));
                    bw.Write(toBytes(lph.msAddr));
                    bw.Write(toBytes(lph.dlAddr));
                    bw.Write(toBytes(lph.rev));

                    bw.Write(toBytes(lph.ss));
                    
                    bw.Write(toBytes(lph.ms));
                    bw.Flush();
                    bw.Close();
                }
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message.ToString(), "Warning!");
            }
        }

        #endregion


        #endregion

        #region 压缩

        private MatrixDatas killZero(MatrixDatas md)
        {
            MatrixDatas tmpMd = new MatrixDatas();
            tmpMd.unicode = md.unicode;
            tmpMd.height = md.height;

            tmpMd.startZeroSum = tmpMd.stopZeroSum = 0;
            for (int i = 0; i < md.length; i++)
            {
                if (md.datas[i] != 0x00)
                {
                    break;
                }
                tmpMd.startZeroSum++;
            }
            for (int i = md.length - 1; i > 0; i--)
            {
                if (md.datas[i] != 0x00)
                {
                    break;
                }
                tmpMd.stopZeroSum++;
            }

            if (md.length == tmpMd.startZeroSum)
            {
                tmpMd.stopZeroSum = 0;
            }
            tmpMd.length = md.length + 4 - tmpMd.startZeroSum - tmpMd.stopZeroSum;
            tmpMd.datas = new byte[tmpMd.length];
            tmpMd.datas[0] = (byte)(tmpMd.startZeroSum / 256);
            tmpMd.datas[1] = (byte)tmpMd.startZeroSum;
            tmpMd.datas[2] = (byte)(tmpMd.stopZeroSum / 256);
            tmpMd.datas[3] = (byte)tmpMd.stopZeroSum;
            for (int i = 4; i < tmpMd.length; i++)
            {
                tmpMd.datas[i] = md.datas[i - 4 + tmpMd.startZeroSum];
            }

            return tmpMd;
        }

        #endregion

        #region 控制组件

        private void FormEasyMatrix_Load(object sender, EventArgs e)
        {

        }

        private void richTextBoxRandomHexDatas_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBoxViewer_Click(object sender, EventArgs e)
        {

        }

        private void progressBarSaving_Click(object sender, EventArgs e)
        {

        }

        private void buttonSwitchTab_Click(object sender, EventArgs e)
        {
            if (buttonSwitchTab.Text == "数据窗口")
            {
                buttonSwitchTab.Text = "看图窗口";
                pictureBoxViewer.Visible = true;

                if (checkBoxRandomMode.Checked == true)
                { 
                    buttonUp.Enabled = true;
                    buttonDown.Enabled = true;
                    buttonLeft.Enabled = true;
                    buttonRight.Enabled = true; 
                    numericUpDownXPos.Enabled = true;
                    numericUpDownYPos.Enabled = true;
                    labelXPos.Enabled = true;
                    labelYPos.Enabled = true;
                }
            }
            else
            {
                buttonSwitchTab.Text = "数据窗口";
                pictureBoxViewer.Visible = false;

                buttonUp.Enabled = false;
                buttonDown.Enabled = false;
                buttonLeft.Enabled = false;
                buttonRight.Enabled = false;
                //buttonLastChar.Enabled = false;
                //buttonNextChar.Enabled = false;
                numericUpDownXPos.Enabled = false;
                numericUpDownYPos.Enabled = false;
                labelXPos.Enabled = false;
                labelYPos.Enabled = false;
            }

            if ((buttonSwitchTab.Text == "数据窗口") && (checkBoxSingleCharOrString.Checked == false))
            {
                labelCurrentChar.Visible = true;
            }
            else
            {
                labelCurrentChar.Visible = false;
            }
        }

        private void buttonUp_Click(object sender, EventArgs e)
        {
            numericUpDownYPos.Value--;

            gCM = getRandomStringMatrixDatas(gStr, gFont);
            drawStringMatrix(gCM);
            displayCurrentPix(gCM.md);
        }

        private void buttonDown_Click(object sender, EventArgs e)
        {
            numericUpDownYPos.Value++;

            gCM = getRandomStringMatrixDatas(gStr, gFont);
            drawStringMatrix(gCM);
            displayCurrentPix(gCM.md);
        }

        private void buttonLeft_Click(object sender, EventArgs e)
        {
            numericUpDownXPos.Value--;

            gCM = getRandomStringMatrixDatas(gStr, gFont);
            drawStringMatrix(gCM);
            displayCurrentPix(gCM.md);
        }

        private void buttonRight_Click(object sender, EventArgs e)
        {
            numericUpDownXPos.Value++;

            gCM = getRandomStringMatrixDatas(gStr, gFont);
            drawStringMatrix(gCM);
            displayCurrentPix(gCM.md);
        }

        private void textBoxInput_TextChanged(object sender, EventArgs e)
        {
            if (textBoxInput.TextLength <= 1)
            {
                buttonLastChar.Enabled = false;
                buttonNextChar.Enabled = false;
                strPos = 0;
            }
            else
            {
                buttonLastChar.Enabled = true ;
                buttonNextChar.Enabled = false;
                strPos = textBoxInput.TextLength - 1;
            }

            if(textBoxInput.TextLength<1)
            {
                gStr = " ";
            }
            else
            {
                gStr = checkBoxSingleCharOrString.Checked ? textBoxInput.Text
                     : textBoxInput.Text[strPos].ToString();
            }
            

            labelCurrentChar.Text = gStr;
            gCM = getRandomStringMatrixDatas(gStr, gFont);
            drawStringMatrix(gCM);
            displayCurrentPix(gCM.md);
            richTextBoxRandomHexDatas.Text = getHexFormatMatrixDatas(killZero(matrixFilter(getRandomStringMatrixDatas(gStr, getFontFromInputBox()), FilterMode.WIDTH_FILTER)));
            //showCharacterOnPictureBox(0x64);
            //redispCharacterOnPictureBox(scanPictureBox((Bitmap)catchScreen((UInt16)(pictureBox_DrawChar.Location.X), (UInt16)(pictureBox_DrawChar.Location.Y), (UInt16)pictureBox_DrawChar.Width, (UInt16)pictureBox_DrawChar.Height, pictureBoxRedisp.Size)));
        }

        private void buttonOutputFile_Click(object sender, EventArgs e)
        {
            String fileName = "EasyMatrix";
            if (saveFileDialogSaving.ShowDialog() == DialogResult.OK)
            {
                fileName = saveFileDialogSaving.FileName;

                /*
                if (!gFlag)
                {
                    MessageBox.Show("未导入有效文件，请检查！", "Warning!");
                    return;
                }
                else if (checkBoxRandomMode.Checked == true)
                { 
                    
                }
                */
            }
            else
            {
                return;
            }
            MatrixSheet ms = creatMatrixSheet(toAddrBytes((UInt32)numericUpDownFlashAddr.Value), unicodeStreamFilter());
            outputMatrixSheet(ms, fileName);
            
            /*
            outputMatrixSheetCFormat(ms, fileName);

            DataSet ds = ExcelToDS(filePara.path);
            outputStringSheetBinaryFile(loadDataSet(ds, toAddrBytes((UInt32)numericUpDownFlashAddr.Value)), fileName);
            outputLanguagePackBinaryFile(fileName);
            */
        }

        private void buttonInsertFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                filePara.path = dialog.FileName;
            }
            else
            {
                return;
            }

            gFlag = true;
            //textBoxFilePath.Text = filePara.path;
            //textBoxFilePath.Visible = true;
        }

        private void buttonFontSetting_Click(object sender, EventArgs e)
        {
            retry:
            FontDialog fd = new FontDialog()
            {
                Font = gFont,
                FontMustExist = true
            };
            try
            {
                if (fd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    gFont = fd.Font;
                }
            }
            catch (System.ArgumentException)
            {
                DialogResult tmpResult = MessageBox.Show(
                                            "该字体不是TrueType字体，请更换字体",
                                            "Warning!",
                                            MessageBoxButtons.OKCancel,
                                            MessageBoxIcon.Warning);

                if (tmpResult == DialogResult.OK)
                {
                    goto retry;
                }
            }

            labelCurrentChar.Font = gFont;
            gCM = getRandomStringMatrixDatas(gStr, gFont);
            drawStringMatrix(gCM);
            displayCurrentPix(gCM.md);
            richTextBoxRandomHexDatas.Text = getHexFormatMatrixDatas(killZero(matrixFilter(getRandomStringMatrixDatas(gStr, getFontFromInputBox()), FilterMode.WIDTH_FILTER)));
        }

        private void labelFontSizePx_Click(object sender, EventArgs e)
        {

        }

        private void comboBoxCharSet_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBoxRandomMode_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxRandomMode.Checked == true)
            {
                if (buttonSwitchTab.Text != "数据窗口")
                { 
                    numericUpDownXPos.Enabled = true;
                    numericUpDownYPos.Enabled = true;

                    //checkBoxSingleCharOrString.Enabled = true;
                    labelXPos.Enabled = true;
                    labelYPos.Enabled = true;
                    //buttonLastChar.Enabled = true;
                    //buttonNextChar.Enabled = true;

                }
                
                comboBoxCharSet.Enabled = false;
                labelCharSet.Enabled = false;
                buttonInsertFile.Enabled = false;
                labelFlashAddr.Enabled = false;
                numericUpDownFlashAddr.Enabled = false;
                buttonOutputFile.Enabled = false;
                
            }
            else
            {
                numericUpDownXPos.Value = 0;
                numericUpDownYPos.Value = 0;
                numericUpDownXPos.Enabled = false;
                numericUpDownYPos.Enabled = false;
                numericUpDownXPos_ValueChanged(1, EventArgs.Empty);

                //checkBoxSingleCharOrString.Enabled = false;
                labelXPos.Enabled = false;
                labelYPos.Enabled = false;
                //buttonLastChar.Enabled = false;
                //buttonNextChar.Enabled = false;

                comboBoxCharSet.Enabled = true;
                labelCharSet.Enabled = true;
                buttonInsertFile.Enabled = true;
                labelFlashAddr.Enabled = true;
                numericUpDownFlashAddr.Enabled = true;
                buttonOutputFile.Enabled = true;
            }
        }

        private void checkBoxSingleCharOrString_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxSingleCharOrString.Checked == true)
            {
                buttonLastChar.Visible = false;
                buttonNextChar.Visible = false;
            }
            else
            {
                buttonLastChar.Visible = true;
                buttonNextChar.Visible = true;
            }

            if (textBoxInput.TextLength < 1)
            {
                gStr = " ";
            }
            else
            {
                gStr = checkBoxSingleCharOrString.Checked ? textBoxInput.Text
                     : textBoxInput.Text[strPos].ToString();
            }

            labelCurrentChar.Text = gStr;
            gCM = getRandomStringMatrixDatas(gStr, gFont);
            drawStringMatrix(gCM);
            displayCurrentPix(gCM.md);
            richTextBoxRandomHexDatas.Text = getHexFormatMatrixDatas(killZero(matrixFilter(getRandomStringMatrixDatas(gStr, getFontFromInputBox()), FilterMode.WIDTH_FILTER)));


            if ((buttonSwitchTab.Text == "数据窗口") && (checkBoxSingleCharOrString.Checked == false))
            {
                labelCurrentChar.Visible = true;
            }
            else
            {
                labelCurrentChar.Visible = false;
            }
        }

        private void buttonLastChar_Click(object sender, EventArgs e)
        {
            if (strPos < 2) 
            {
                buttonLastChar.Enabled = false;
                buttonNextChar.Enabled = true;
                //return;
            }
            else
            {
                buttonLastChar.Enabled = true;
            }

            strPos--;
            buttonNextChar.Enabled = true;

            gStr = textBoxInput.Text[strPos].ToString();
            labelCurrentChar.Text = gStr;
            gCM = getRandomStringMatrixDatas(gStr, gFont);
            drawStringMatrix(gCM);
            displayCurrentPix(gCM.md);
            richTextBoxRandomHexDatas.Text = getHexFormatMatrixDatas(killZero(matrixFilter(getRandomStringMatrixDatas(gStr, getFontFromInputBox()), FilterMode.WIDTH_FILTER)));
        }

        private void labelCharSet_Click(object sender, EventArgs e)
        {

        }

        private void buttonNextChar_Click(object sender, EventArgs e)
        {
            if (strPos >= textBoxInput.TextLength - 2)
            {
                buttonNextChar.Enabled = false;
                buttonLastChar.Enabled = true;
                //return;
            }
            else
            {
                buttonNextChar.Enabled = true;
            }

            strPos++;
            buttonLastChar.Enabled = true;
            gStr = textBoxInput.Text[strPos].ToString();
            labelCurrentChar.Text = gStr;
            gCM = getRandomStringMatrixDatas(gStr, gFont);
            drawStringMatrix(gCM);
            displayCurrentPix(gCM.md);
            richTextBoxRandomHexDatas.Text = getHexFormatMatrixDatas(killZero(matrixFilter(getRandomStringMatrixDatas(gStr, getFontFromInputBox()), FilterMode.WIDTH_FILTER)));
        }

        private void numericUpDownFlashAddr_ValueChanged(object sender, EventArgs e)
        {

        }

        private void pictureBoxDrawingPicture_Click(object sender, EventArgs e)
        {

        }

        private void numericUpDownYPos_ValueChanged(object sender, EventArgs e)
        {
            gCM = getRandomStringMatrixDatas(gStr, gFont);
            drawStringMatrix(gCM);
            displayCurrentPix(gCM.md);
            richTextBoxRandomHexDatas.Text = getHexFormatMatrixDatas(killZero(matrixFilter(getRandomStringMatrixDatas(gStr, getFontFromInputBox()), FilterMode.WIDTH_FILTER)));
        }

        private void numericUpDownXPos_ValueChanged(object sender, EventArgs e)
        {
            gCM = getRandomStringMatrixDatas(gStr, gFont);
            drawStringMatrix(gCM);
            displayCurrentPix(gCM.md);
            richTextBoxRandomHexDatas.Text = getHexFormatMatrixDatas(killZero(matrixFilter(getRandomStringMatrixDatas(gStr, getFontFromInputBox()), FilterMode.WIDTH_FILTER)));
        }

        private void pictureBoxBKG_Click(object sender, EventArgs e)
        {

        }

        private void labelXPos_Click(object sender, EventArgs e)
        {

        }

        private void labelYPos_Click(object sender, EventArgs e)
        {

        }

        private void labelCurrentChar_Click(object sender, EventArgs e)
        {

        }

        private void saveFileDialogSaving_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void pictureBoxQLT_Click(object sender, EventArgs e)
        {

        }

        #endregion


        #region 测试代码

        #region 网络抄录
        [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetDC(int hwnd);
        [DllImport("gdi32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int GetPixel(int hdc, int X, int y);
        private struct POINTAPI //确定坐标
        {
            private int X;
            private int y;
        }
        [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)] //确定坐标
        private static extern int ReleaseDC(int hwnd, int hdc);
        POINTAPI P; //确定坐标


        [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int ScreenToClient(int hwnd, ref POINTAPI lpPoint);
        [DllImport("user32", ExactSpelling = true, CharSet = CharSet.Ansi, SetLastError = true)]
        private static extern int WindowFromPoint(int xPoint, int yPoint);

        #endregion

        private void redispPicMatrix(PicMatrix pm)
        {
            UInt16 i, j;
            //Bitmap bmpOrg = new Bitmap(pictureBoxRedisp.Width, pictureBoxRedisp.Height);
            //Bitmap bmp = new Bitmap(pictureBoxRedisp.Image, pictureBoxRedisp.Size);
            Bitmap bmp = new Bitmap(pictureBoxRedisp.Width, pictureBoxRedisp.Height);

            Bitmap bitmap = new Bitmap(pictureBox_DrawChar.Width, pictureBox_DrawChar.Height);
            //pictureBox_DrawChar.DrawToBitmap(bitmap, pictureBox_DrawChar.ClientRectangle);

            Bitmap image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics imgGraphics = Graphics.FromImage(image);
            imgGraphics.CopyFromScreen(0, 0, 0, 0, new Size(pictureBox_DrawChar.Width, pictureBox_DrawChar.Height));

            bitmap = new Bitmap(image);


            for (i = 0; i < pm.height; i++) {
                for (j = 0; j < pm.width; j++) 
                {
                    //pictureBox_DrawChar.
                    //bmp.SetPixel(j, i, bitmap.GetPixel(j, i));
                    //bmp.SetPixel(pictureBox_DrawChar.Location.X + j, pictureBox_DrawChar.Location.Y + i, Color.Beige);
                    bmp.SetPixel(j, i, Color.Beige);
                }
            }
        }

        private String toBytes(PicMatrix pm)
        { 
            StringBuilder sb = new StringBuilder();
            UInt16 x, y, temp=0;
            for(x=0; x<pm.width; x++)
            {
                for(y=0; y<pm.height; y++)
                {
                    sb.Append("0x");
                    sb.Append(pm.C565[x][y].ToString("x2"));
                    sb.Append(", ");

                    //if (pm.C565[x][y] != 0xEF5D)
                    //{
                    //    sb.Append("**");
                    //}
                    //else
                    //{
                    //    sb.Append("  ");
                    //}

                    temp ++;
                    if (temp % pm.width == 0 && temp != 0)
                    {
                        sb.Append("\r\n");
                    }
                }
            }

            return sb.ToString();
        }
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

        private PicMatrix SavePixels()
        {
            Graphics g = pictureBox_DrawChar.CreateGraphics();
            string str = "这段文字右对齐";
            Font font = getFontFromInputBox();
            SizeF size = g.MeasureString(str, font);
            g.DrawString(str, font, new SolidBrush(Color.White), pictureBox_DrawChar.Width - size.Width, 10);

            Rectangle rect = new Rectangle();
            rect = Screen.GetWorkingArea(this);

            //int h;
            //int hD;
            //int temp;
            PicMatrix pm = creatPicMatrix();
            Bitmap bitmap = new Bitmap(pictureBox_DrawChar.Width, pictureBox_DrawChar.Height);
            //pictureBox_DrawChar.DrawToBitmap(bitmap, pictureBox_DrawChar.ClientRectangle);
            
            Bitmap image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics imgGraphics = Graphics.FromImage(image);
            imgGraphics.CopyFromScreen(0, 0, 0, 0, new Size(pictureBox_DrawChar.Width, pictureBox_DrawChar.Height));
        
            bitmap = new Bitmap(image);


            for (int y = 0; y < pm.height; y++)
            {
                for (int x = 0; x < pm.width; x++)
                {
                    //h = WindowFromPoint(rect.Width/2, rect.Height/2);
                    //hD = FormEasyMatrix.GetDC(h);//GetDC(h)
                    //ScreenToClient(h, ref P);
                    //temp = GetPixel(hD, x, y);

                    //c.R = (byte)(temp % 256);

                    //pm.C565[x][y] = (UInt16)GetPixel(hD, x, y);
                    pm.C565[x][y] = CalPixel(bitmap.GetPixel(x, y));
                    //pm.C565[x][y] = pm.C565[x][y] != (UInt16)0 ? (UInt16)1 : (UInt16)0;
                }
            }

            redispPicMatrix(pm);

            //bitmap.Dispose();
            g.Dispose();
            return pm;
        }
        
        private PicMatrix creatPicMatrix()
        {
            UInt32 i;
            
            PicMatrix pm = new PicMatrix();

            pm.width = (UInt32)pictureBox_DrawChar.Width;
            pm.height = (UInt32)pictureBox_DrawChar.Height;

            pm.C565 = new UInt16[pm.width][];
            for(i=0; i<pm.width; i++)
            {
                pm.C565[i] = new UInt16[pm.height];
            }

            return pm;
        }

        private PicMatrix creatPicMatrix(Bitmap image)
        {
            UInt32 i;
            
            PicMatrix pm = new PicMatrix();

            pm.width = (UInt32)image.Width;
            pm.height = (UInt32)image.Height;

            pm.C565 = new UInt16[pm.width][];
            pm.C888 = new UInt32[pm.width][];
            pm.color = new Color[pm.width][];
            for(i=0; i<pm.width; i++)
            {
                pm.color[i] = new Color[pm.height];
                pm.C565[i] = new UInt16[pm.height];
                pm.C888[i] = new UInt32[pm.height];
            }

            return pm;
        }

        private void pictureBox_DrawChar_Click(object sender, EventArgs e)
        {

        }

        private void buttonDrawCharOnPic_Click(object sender, EventArgs e)
        {
            //redispPicMatrix(creatPicMatrix());
            // 扫描画图后的取模(图片)结果
            //richTextBoxDrawCharOnPic.Text = toBytes(SavePixels());

            // 过滤黑色像素后的数据结果

            showCharacterOnPictureBox(0x64);
            //redispCharacterOnPictureBox(scanPictureBox((Bitmap)catchScreen(0, 0, 39, 74, pictureBoxRedisp.Size)));
            redispCharacterOnPictureBox(scanPictureBox((Bitmap)catchScreen((UInt16)(pictureBox_DrawChar.Location.X), (UInt16)(pictureBox_DrawChar.Location.Y), (UInt16)pictureBox_DrawChar.Width, (UInt16)pictureBox_DrawChar.Height, pictureBoxRedisp.Size)));
            //redispCharacterOnPictureBox(scanPictureBox((Bitmap)catchScreen((UInt16)(this.Location.X), (UInt16)(this.Location.Y), 39, 74, pictureBoxRedisp.Size)));
        }

        private void richTextBoxDrawCharOnPic_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBoxRedisp_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region 测试代码II

        // 显示文字在纯黑背景的pictrueBox上
        private void showCharacterOnPictureBox(UInt16 unicodes)
        {
            Graphics g = pictureBox_DrawChar.CreateGraphics();
            //string str = Encoding.BigEndianUnicode.GetString(toBytes(unicodes)); 
            string str = textBoxInput.Text.ToString();
            Font font = getFontFromInputBox();
            SizeF size = g.MeasureString(str, font);
            g.FillRectangle(Brushes.White, 0, 0, pictureBox_DrawChar.Width, pictureBox_DrawChar.Height);
            g.DrawString(str, font, new SolidBrush(Color.Black), pictureBox_DrawChar.Width - size.Width, 10);
            g.Dispose();
        }

        // 指定位置指定区域截图
        private Image catchScreen(UInt16 xpos, UInt16 ypos, UInt16 width, UInt16 height, Size size)
        {
            pictureBoxRedisp.Image = new Bitmap(width, height);

            //Bitmap image = new Bitmap(Screen.PrimaryScreen.Bounds.Width, Screen.PrimaryScreen.Bounds.Height);
            Graphics imgGraphics = Graphics.FromImage(pictureBoxRedisp.Image);
            //imgGraphics.CopyFromScreen(xpos, ypos, xpos + width, ypos + height, size);
            //imgGraphics.CopyFromScreen(xpos, ypos, (UInt16)(pictureBoxRedisp.Location.X), (UInt16)(pictureBoxRedisp.Location.Y), size);
            //imgGraphics.CopyFromScreen(this.Location.X + xpos, this.Location.Y + ypos, 0, 0, size);
            Point p = pictureBox_DrawChar.PointToScreen(this.FindForm().Location);
            imgGraphics.CopyFromScreen(p.X - this.Location.X, p.Y - this.Location.Y, 0, 0, size);
            //imgGraphics.CopyFromScreen(this.Location.X, this.Location.Y, 0, 0, size);
            //imgGraphics.CopyFromScreen(this.Location.X, this.Location.Y, (UInt16)(pictureBox_DrawChar.Location.X), (UInt16)(pictureBox_DrawChar.Location.Y), size);

            return pictureBoxRedisp.Image;
        }
        
        // 扫描截图IMAGE，生成pictureBox
        private PicMatrix scanPictureBox(Bitmap image)
        {
            PicMatrix pm = creatPicMatrix(image);
            int x, y;

            for (x = 0; x < image.Width; x++)
            {
                for (y = 0; y < pm.height; y++)
                {
                    pm.color[x][y] = image.GetPixel(x, y);
                    pm.C888[x][y] = (UInt32)((pm.color[x][y].R << 16) & 0xFF0000 + (pm.color[x][y].G << 8) & 0x00FF00 + (pm.color[x][y].B) & 0x0000FF);
                    pm.C565[x][y] = CalPixel(pm.color[x][y]);
                }
            }

            return pm;
        }

        // 验证显示
        // R 0b11111000 00000000 = 0xF800 = 0xFF0000
        // G 0b00000111 11100000 = 0x07E0 = 0x00FF00
        // B 0b00000000 00011111 = 0x001F = 0x0000FF
        private void redispCharacterOnPictureBox(PicMatrix pm)
        {
            UInt16 x, y;
            Color color = new Color();
            Bitmap image = new Bitmap((int)pm.width, (int)pm.height);
            pictureBoxRedisp.Image = image;

            for (x = 0; x < pm.width; x++)
            {
                for (y = 0; y < pm.height; y++)
                {
                    // 0b 0001 0000 0100 0010 = 0x0152 
                    if ((pm.C565[x][y] & 0xF800) + (pm.C565[x][y] & 0x07E0) + (pm.C565[x][y] & 0x001F) > 0x1052<<3)
                    {
                        pm.C565[x][y] = 0xFFFF;
                    }
                    //if(pm.color[x][y].A)
                    color = Color.FromArgb(0xFF, (int)(pm.C565[x][y] & 0xF800) >> 11, (int)(pm.C565[x][y] & 0x07E0) >> 5, (int)(pm.C565[x][y] & 0x001F));
                    //color = Color.FromArgb(0xFF, (int)(pm.C888[x][y] & 0xFF0000) >> 16, (int)(pm.C888[x][y] & 0x00FF00) >> 8, (int)(pm.C888[x][y] & 0x00FF));
                    ((Bitmap)pictureBoxRedisp.Image).SetPixel(x, y, color);
                    //((Bitmap)pictureBoxRedisp.Image).SetPixel(x, y, pm.color[x][y]);
                }
            }
        }

        // 过滤
        private PicMatrix picMatrixFilter(PicMatrix pm)
        {
            return pm;
        }

        #endregion
    }
}
