using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.IO;

namespace Fund
{
    public partial class Form2 : Form
    {
        string ID;
        public Form2(string id)
        {
            InitializeComponent();
            ID = id;
            //获取图片内容
            pictureBox1.ImageLocation = @"http://j4.dfcfw.com/charts/pic6/" + id + ".png";

            //通过string的substring和IndexOf方法处理字符串，获取文字内容，并放入文字框
            string result = GetContent(@"http://fund.eastmoney.com/" + id + ".html");
            try
            {
                result = result.Substring(result.IndexOf("id=\"gz_gsz\">"));
                string tmp = result.Substring(result.IndexOf("\">") + 2);
                tmp = tmp.Substring(0, tmp.IndexOf("<"));
                textBox1.Text = tmp;

                result = result.Substring(result.IndexOf("ui-font-large") + 1);
                tmp = result.Substring(result.IndexOf("\">") + 2);
                tmp = tmp.Substring(0, tmp.IndexOf("<"));
                textBox2.Text = tmp;

                result = result.Substring(result.IndexOf("ui-font-large"));
                tmp = result.Substring(result.IndexOf("\">") + 2);
                tmp = tmp.Substring(0, tmp.IndexOf("<"));
                textBox3.Text = tmp;

                result = result.Substring(result.IndexOf("<table>"));
                result = result.Substring(result.IndexOf("<td>") + 4);
                result = result.Substring(result.IndexOf(">") + 1);
                tmp = result.Substring(0, result.IndexOf("</a>"));
                result = result.Substring(result.IndexOf("|&nbsp;&nbsp;") + 13);
                tmp += " | " + result.Substring(0, result.IndexOf("<"));
                label6.Text = tmp;

                result = result.Substring(result.IndexOf("基金规模"));
                result = result.Substring(result.IndexOf("：") + 1);
                tmp = result.Substring(0, result.IndexOf("<"));
                label8.Text = tmp;

                result = result.Substring(result.IndexOf("基金经理"));
                result = result.Substring(result.IndexOf(">") + 1);
                tmp = result.Substring(0, result.IndexOf("<"));
                label10.Text = tmp;

                result = result.Substring(result.IndexOf("日"));
                result = result.Substring(result.IndexOf("：") + 1);
                tmp = result.Substring(0, result.IndexOf("<"));
                label12.Text = tmp;

                result = result.Substring(result.IndexOf("人"));
                result = result.Substring(result.IndexOf("：") + 1);
                result = result.Substring(result.IndexOf(">") + 1);
                tmp = result.Substring(0, result.IndexOf("<"));
                label14.Text = tmp;
            }
            catch (Exception)
            {
                MessageBox.Show("此基金处于认购期不存在信息", "提示");
            }
        }



        //获取网页内容
        string GetContent(string url)
        {
            string html = "";
            // 发送查询请求
            WebRequest request = WebRequest.Create(url);
            WebResponse response = null;
            try
            {
                response = request.GetResponse();
                // 获得流
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.Default);
                html = sr.ReadToEnd();
                response.Close();
            }
            catch (Exception ex)
            {
                // 本机没有联网
                if (ex.GetType().ToString().Equals("System.Net.WebException"))
                {
                    MessageBox.Show("请检查你的计算机是否已连接上互联网。", "提示");
                }
            }
            return html;
        }

        private void label13_Click(object sender, EventArgs e)
        {

        }
        //打开基金持股
        private void button1_Click(object sender, EventArgs e)
        {
            Form3 f = new Form3(ID);
            f.Show();
        }
    }
}
