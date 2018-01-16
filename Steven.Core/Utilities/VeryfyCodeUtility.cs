using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Text;
using System.Web;

namespace Steven.Core.Utilities
{
    public class VeryfyCodeUtility
    {
        public const string VerifyCode = "$BeiLin$VerifyCode$";
        /// <summary>
        /// 生成验证码
        /// [GatewangMachine]
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreateVerifyCode(int length)
        {
            var randMembers = new int[length];
            var validateNums = new int[length];
            var strBuilder = new StringBuilder();

            //生成起始序列值
            var seekSeek = unchecked((int)DateTime.Now.Ticks);
            var seekRand = new Random(seekSeek);
            var beginSeek = (int)seekRand.Next(0, Int32.MaxValue - 10000 * length);
            var seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }

            //生成随机数字
            var pownum = (int)Math.Pow(10, length);
            for (int i = 0; i < length; i++)
            {
                var rand = new Random(seeks[i]);
                randMembers[i] = rand.Next(pownum, Int32.MaxValue);
            }

            //抽取随机数字
            for (int i = 0; i < length; i++)
            {
                var numStr = Math.Abs(randMembers[i]).ToString();
                var rand = new Random();
                var numPosition = rand.Next(0, numStr.Length - 1);
                validateNums[i] = Int32.Parse(numStr.Substring(numPosition, 1));
            }

            //生成验证码
            for (int i = 0; i < length; i++)
            {
                strBuilder.Append(validateNums[i]);
            }
            return strBuilder.ToString();
        }

        private static readonly string[] AllChar = new string[] { "0","1","2","3","4","5","6","7","8","9",
            "A","B","C","D","E","F","G","H","I","J","K","L","M","N","O","P","Q","R","S","T","U","W","X","Y","Z"};

        /// <summary>
        /// 生成随机密码，由字符和数字组成
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string CreatePass(int length)
        {
            StringBuilder sbPass = new StringBuilder();
            Random seekRand = new Random(Guid.NewGuid().GetHashCode());
            //生成起始序列值
            int beginSeek = (int)seekRand.Next(0, Int32.MaxValue - 10000 * length);
            int[] seeks = new int[length];
            for (int i = 0; i < length; i++)
            {
                beginSeek += 10000;
                seeks[i] = beginSeek;
            }

            //生成并抽取随机数字
            for (int i = 0; i < length; i++)
            {
                Random rand = new Random(seeks[i]);
                sbPass.Append(AllChar[rand.Next(35)]);
            }
            return sbPass.ToString();
        }

        /// <summary>
        /// 创建验证码的图片
        /// </summary>
        /// <param name="verifyCode"></param>
        /// <returns></returns>
        public static byte[] CreateVerifyGraphic(string verifyCode)
        {
            Bitmap image = new Bitmap((int)Math.Ceiling(verifyCode.Length * 12.0), 22);
            Graphics g = Graphics.FromImage(image);

            try
            {
                //生成随机生成器
                Random rand = new Random();
                //清空图片背景色
                g.Clear(Color.White);
                //画图片的干扰线
                for (int i = 0; i < 25; i++)
                {
                    int x1 = rand.Next(image.Width);
                    int x2 = rand.Next(image.Width);
                    int y1 = rand.Next(image.Height);
                    int y2 = rand.Next(image.Height);
                    g.DrawLine(new Pen(Color.Silver), x1, y1, x2, y2);
                }
                Font font = new Font("Area", 12, (FontStyle.Bold | FontStyle.Italic));
                LinearGradientBrush brush = new LinearGradientBrush(
                    new Rectangle(0, 0, image.Width, image.Height),
                    Color.Blue,
                    Color.DarkRed,
                    1.2f,
                    true);
                g.DrawString(verifyCode, font, brush, 3, 2);

                //画图片的前景干扰点
                for (int i = 0; i < 100; i++)
                {
                    int x = rand.Next(image.Width);
                    int y = rand.Next(image.Height);
                    image.SetPixel(x, y, Color.FromArgb(rand.Next()));
                }
                //画图片的边框
                g.DrawRectangle(new Pen(Color.Silver), 0, 0, image.Width - 1, image.Height - 1);
                //保存图片数据
                MemoryStream stream = new MemoryStream();
                image.Save(stream, ImageFormat.Jpeg);
                return stream.ToArray();
            }
            finally
            {
                g.Dispose();
                image.Dispose();
            }
        }

        /// <summary>
        /// 创建验证证并记录在Session中
        /// </summary>
        /// <param name="session"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        public static byte[] CreateVerifyImage(HttpSessionStateBase session,int length)
        {
            if (length < 2) length = 2;
            if (length > 10) length = 10;
            string code = CreateVerifyCode(length);
            session.Remove(VerifyCode);
            session.Add(VerifyCode, code);
            return CreateVerifyGraphic(code);
        }

        /// <summary>
        /// 检查验证码是否匹配
        /// </summary>
        /// <param name="session"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public static bool IsVerifyCodeMatch(HttpSessionStateBase session,string code)
        {
            object data = session[VerifyCode];
            session.Remove(VerifyCode);
            return (null != data) && (data.ToString().Equals(code, StringComparison.OrdinalIgnoreCase));
        }
        public static bool IsVerifyCodeMatchNotRemove(HttpSessionStateBase session, string code)
        {
            object data = session[VerifyCode];
            return (null != data) && (data.ToString().Equals(code, StringComparison.OrdinalIgnoreCase));
        }
    }
}