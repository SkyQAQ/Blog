using System;
using System.Web;
using System.Drawing;
using System.Security.Cryptography;
using System.Buffers.Text;
using System.IO;
using System.Drawing.Imaging;
using System.Data;

namespace Blog.Core.Common
{
    /// <summary>
    /// 验证码帮助类
    /// </summary>
    public class VerifyCodeHelper
    {
        #region 私有字段
        private int letterWidth = 16;  //单个字体的宽度范围
        private int letterHeight = 24; //单个字体的高度范围
        private static byte[] randb = new byte[4];
        private static RNGCryptoServiceProvider rand = new RNGCryptoServiceProvider();
        private Font[] fonts =
        {
           new Font(new FontFamily("Times New Roman"),14, FontStyle.Bold),
           new Font(new FontFamily("Georgia"), 14, FontStyle.Bold),
           new Font(new FontFamily("Arial"), 14, FontStyle.Bold),
           new Font(new FontFamily("Comic Sans MS"), 14, FontStyle.Bold)
        };
        #endregion

        #region 公有属性
        /// <summary>
        /// 验证码
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// 验证码图片
        /// </summary>
        public Bitmap Image { get; private set; }

        /// <summary>
        /// 验证码图片string
        /// </summary>
        public string ImageString { get; private set; }
        #endregion

        #region 私有方法
        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="max">最大值</param>
        private static int Next(int max)
        {
            rand.GetBytes(randb);
            int value = BitConverter.ToInt32(randb, 0);
            value = value % (max + 1);
            if (value < 0) value = -value;
            return value;
        }

        /// <summary>
        /// 获得下一个随机数
        /// </summary>
        /// <param name="min">最小值</param>
        /// <param name="max">最大值</param>
        private static int Next(int min, int max)
        {
            int value = Next(max - min) + min;
            return value;
        }

        /// <summary>
        /// 字体随机颜色
        /// </summary>
        private Color GetRandomColor()
        {
            Random RandomNum_First = new Random((int)DateTimeUtils.NowBeijing().Ticks);
            System.Threading.Thread.Sleep(RandomNum_First.Next(50));
            Random RandomNum_Sencond = new Random((int)DateTimeUtils.NowBeijing().Ticks);
            int int_Red = RandomNum_First.Next(180);
            int int_Green = RandomNum_Sencond.Next(180);
            int int_Blue = (int_Red + int_Green > 300) ? 0 : 400 - int_Red - int_Green;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;
            return Color.FromArgb(int_Red, int_Green, int_Blue);
        }

        /// <summary>
        /// 正弦曲线Wave扭曲图片
        /// </summary>
        /// <param name="srcBmp">图片路径</param>
        /// <param name="bXDir">如果扭曲则选择为True</param>
        /// <param name="nMultValue">波形的幅度倍数，越大扭曲的程度越高,一般为3</param>
        /// <param name="dPhase">波形的起始相位,取值区间[0-2*PI)</param>
        private Bitmap TwistImage(Bitmap srcBmp, bool bXDir, double dMultValue, double dPhase)
        {
            double PI = 6.283185307179586476925286766559;
            Bitmap destBmp = new Bitmap(srcBmp.Width, srcBmp.Height);
            Graphics graph = Graphics.FromImage(destBmp);
            graph.FillRectangle(new SolidBrush(Color.White), 0, 0, destBmp.Width, destBmp.Height);
            graph.Dispose();
            double dBaseAxisLen = bXDir ? (double)destBmp.Height : (double)destBmp.Width;
            for (int i = 0; i < destBmp.Width; i++)
            {
                for (int j = 0; j < destBmp.Height; j++)
                {
                    double dx = 0;
                    dx = bXDir ? (PI * (double)j) / dBaseAxisLen : (PI * (double)i) / dBaseAxisLen;
                    dx += dPhase;
                    double dy = Math.Sin(dx);
                    int nOldX = 0, nOldY = 0;
                    nOldX = bXDir ? i + (int)(dy * dMultValue) : i;
                    nOldY = bXDir ? j : j + (int)(dy * dMultValue);

                    Color color = srcBmp.GetPixel(i, j);
                    if (nOldX >= 0 && nOldX < destBmp.Width
                     && nOldY >= 0 && nOldY < destBmp.Height)
                    {
                        destBmp.SetPixel(nOldX, nOldY, color);
                    }
                }
            }
            srcBmp.Dispose();
            return destBmp;
        }

        /// <summary>
        /// 图片转Base64String
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        private static string BitmapToString(Bitmap bitmap)
        {
            MemoryStream ms = new MemoryStream();
            try
            {
                bitmap.Save(ms, ImageFormat.Gif);
                byte[] buffer = new byte[ms.Length];
                ms.Position = 0L;
                ms.Read(buffer, 0, (int)ms.Length);
                ms.Close();
                return Convert.ToBase64String(buffer);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                ms.Dispose();
            }

        }

        /// <summary>
        /// 加密验证码
        /// </summary>
        /// <param name="text">验证码</param>
        /// <returns></returns>
        private static string EncryptVcCode(string text)
        {
            string clientId = Guid.NewGuid().ToString();
            string code = WuYao.GetMd5(text.ToUpper());
            string plainText = clientId + "$" + code + "$" + Rand.Str_char(6);
            SqlHelper _sql = new SqlHelper();
            _sql.OpenDb();
            _sql.Execute(string.Format("insert into tbl_loginverifycode values('{0}','{1}',{2})", clientId, text, DateTime.UtcNow.AddMinutes(3).Ticks));
            _sql.CloseDb();
            return WuYao.AesEncrypt(plainText);
        }
        #endregion

        #region 公有方法
        /// <summary>
        /// 绘制验证码
        /// </summary>
        public void CreateVerifyCode()
        {
            string text = Rand.Str(4);
            int int_ImageWidth = text.Length * letterWidth;
            Bitmap image = new Bitmap(int_ImageWidth, letterHeight);
            Graphics g = Graphics.FromImage(image);
            g.Clear(Color.White);
            for (int i = 0; i < 2; i++)
            {
                int x1 = Next(image.Width - 1);
                int x2 = Next(image.Width - 1);
                int y1 = Next(image.Height - 1);
                int y2 = Next(image.Height - 1);
                g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
            }
            int _x = -12, _y = 0;
            for (int int_index = 0; int_index < text.Length; int_index++)
            {
                _x += Next(12, 16);
                _y = Next(-2, 2);
                string str_char = text.Substring(int_index, 1);
                str_char = Next(1) == 1 ? str_char.ToLower() : str_char.ToUpper();
                Brush newBrush = new SolidBrush(GetRandomColor());
                Point thePos = new Point(_x, _y);
                g.DrawString(str_char, fonts[Next(fonts.Length - 1)], newBrush, thePos);
            }
            for (int i = 0; i < 10; i++)
            {
                int x = Next(image.Width - 1);
                int y = Next(image.Height - 1);
                image.SetPixel(x, y, Color.FromArgb(Next(0, 255), Next(0, 255), Next(0, 255)));
            }
            image = TwistImage(image, true, Next(1, 3), Next(4, 6));
            g.DrawRectangle(new Pen(Color.LightGray, 1), 0, 0, int_ImageWidth - 1, (letterHeight - 1));
            this.Image = image;
            this.ImageString = BitmapToString(image);
            this.Text = EncryptVcCode(text);
        }

        /// <summary>
        /// 检测验证码，返回ClientId
        /// </summary>
        /// <param name="inputCode"></param>
        /// <param name="encryptCode"></param>
        /// <returns></returns>
        public string CheckVerifyCode(string inputCode, string encryptCode)
        {
            string str = WuYao.AesDecrypt(encryptCode);
            if (string.IsNullOrEmpty(str))
            {
                throw new Exception("你还想不想登录了！");
            }
            string[] stra = str.Split('$');
            if (stra == null || stra.Length == 0)
            {
                throw new Exception("系统有误！");
            }
            if (WuYao.GetMd5(inputCode.ToUpper()) != stra[1])
            {
                throw new Exception("验证码有误！");
            }
            SqlHelper _sql = new SqlHelper();
            DataTable dt = _sql.Query("SELECT * FROM tbl_loginverifycode WITH(nolock) WHERE ClientId = @id", 
                new System.Collections.Generic.Dictionary<string, object> { { "@id", stra[0] } });
            if (dt == null || dt.Rows.Count == 0)
            {
                throw new Exception("验证码已失效！");
            }
            if (DateTime.UtcNow.Ticks > long.Parse(Cast.ConToString(dt.Rows[0]["Ticks"])))
            {
                throw new Exception("验证码已失效！");
            }
            return stra[0];
        }
        #endregion
    }

    /// <summary>
    /// 验证码类
    /// </summary>
    public class Rand
    {
        #region 生成随机数字
        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="length">生成长度</param>
        public static string Number(int Length)
        {
            return Number(Length, false);
        }

        /// <summary>
        /// 生成随机数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string Number(int Length, bool Sleep)
        {
            if (Sleep) System.Threading.Thread.Sleep(3);
            string result = "";
            System.Random random = new Random();
            for (int i = 0; i < Length; i++)
            {
                result += random.Next(10).ToString();
            }
            return result;
        }
        #endregion

        #region 生成随机字母与数字
        /// <summary>
        /// 生成随机字母与数字
        /// </summary>
        /// <param name="IntStr">生成长度</param>
        public static string Str(int Length)
        {
            return Str(Length, false);
        }

        /// <summary>
        /// 生成随机字母与数字
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string Str(int Length, bool Sleep)
        {
            if (Sleep) System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTimeUtils.NowBeijing().Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }
        #endregion

        #region 生成随机纯字母随机数
        /// <summary>
        /// 生成随机纯字母随机数
        /// </summary>
        /// <param name="IntStr">生成长度</param>
        public static string Str_char(int Length)
        {
            return Str_char(Length, false);
        }

        /// <summary>
        /// 生成随机纯字母随机数
        /// </summary>
        /// <param name="Length">生成长度</param>
        /// <param name="Sleep">是否要在生成前将当前线程阻止以避免重复</param>
        public static string Str_char(int Length, bool Sleep)
        {
            if (Sleep) System.Threading.Thread.Sleep(3);
            char[] Pattern = new char[] { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
            string result = "";
            int n = Pattern.Length;
            System.Random random = new Random(~unchecked((int)DateTimeUtils.NowBeijing().Ticks));
            for (int i = 0; i < Length; i++)
            {
                int rnd = random.Next(0, n);
                result += Pattern[rnd];
            }
            return result;
        }
        #endregion
    }
}
