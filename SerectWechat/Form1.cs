using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SerectWechat
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textBox1.Text))
            {

            }
        }

        /// <summary>
        /// 加密
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            string secrect = textBox1.Text;
            string outStr = "";

            if (!string.IsNullOrEmpty(secrect))
            {
                //for (int i = 0; i < secrect.Length; i++)
                //{
                //    //將中文轉為10進制整數，然後轉為16進制unicode
                //    outStr += "\\u" + ((int)secrect[i]).ToString("x");
                //}

                //string dst = "";
                char[] src = secrect.ToCharArray();


                List<UnicodeUnit> us = new List<UnicodeUnit>();
                for (int i = 0; i < src.Length; i++)
                {
                    byte[] bytes = Encoding.Unicode.GetBytes(src[i].ToString());
                    //string str = "|" + bytes[1].ToString() + "," + bytes[0].ToString();//
                    //string str = @"\u" + bytes[1].ToString("X2") + bytes[0].ToString("X2");
                    //dst += str;

                    var u = new UnicodeUnit();
                    u.a = bytes[1];
                    u.b = bytes[0];
                    Change(u);
                    us.Add(u);
                }


                //us.Add(new UnicodeUnit { a = 78, b = 0 });

                textBox2.Text = UnicodeToString(us);//dst;//Regex.Unescape(outStr);
            }
        }

        public static string UnicodeToString(List<UnicodeUnit> us)
        {
            string dst = "";
            //for (int i = 0; i <= u.Count - 1; i++)
            foreach (var u in us)
            {
                string str = "";
                byte[] bytes = new byte[2];
                bytes[1] = byte.Parse(u.a.ToString());
                bytes[0] = byte.Parse(u.b.ToString());
                dst += Encoding.Unicode.GetString(bytes);
            }
            return dst;
        }


        private void Change(UnicodeUnit u)
        {
            if (u.a == 159 && u.b == 239)
            {
                u.a = 78;
                u.b = 0;
                return;
            }

            u.b++;

            if (u.b == 265)
            {
                u.b = 0;
                u.a++;
            }
        }

        private void BackChange(UnicodeUnit u)
        {
            if (u.a == 78 && u.b == 0)
            {
                u.a = 159;
                u.b = 239;
                return;
            }

            u.b--;

            if (u.b < 0)
            {
                u.b = 255;
                u.a--;
            }
        }
    }

    public class UnicodeUnit
    {
        //bytes[1]
        public int a;
        //bytes[0]
        public int b;
    }
}
