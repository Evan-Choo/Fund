using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections;

namespace Fund
{
    public partial class Form1 : Form
    {
        Thread td;
        //初始化页数
        int page = 1;
        //初始化开始界面，并把页数转到第一页

        List<string> fundsID = new List<string>();
        List<Stock> stocks = new List<Stock>();

        public Form1()
        {
            InitializeComponent();
            ThreadStart ts = new ThreadStart(GetIntroduction);
            td = new Thread(ts);
            td.SetApartmentState(ApartmentState.STA);
            td.Start();
        }

        void GetIntroduction()
        {
            turnTo(1);
        }

        //获取网页的内容，代码来自老师给的股票
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
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
                html = sr.ReadToEnd();
                response.Close();
            }
            catch (Exception ex)
            {
                // 本机没有联网
                if (ex.GetType().ToString().Equals("System.Net.WebException"))
                {
                    MessageBox.Show("请检查你的计算机是否已连接上互联网。\n" + url, "提示");
                }
            }
            return html;
        }
        
        //转到特定页
        string turnTo(int pi)
        {
            //清空之前内容
            this.dataGridView1.Rows.Clear();
            string url = "http://fund.eastmoney.com/data/rankhandler.aspx?op=ph&dt=kf&ft=all&rs=&gs=0&sc=zzf&st=desc&sd=2015-10-29&ed=2016-10-29&qdii=&tabSubtype=,,,,,&pi="
                + pi + "&pn=50&dx=1&v=0.10850418109563731";

            string data = GetContent(url);
            //正则表达式，提取每两个引号之间内容
            Regex re = new Regex("(?<=\").*?(?=\")", RegexOptions.None);

            //用正则表达式提取内容
            MatchCollection mc = re.Matches(data);

            int index = 0;

            //跳过单数项
            int pass = 0;

            foreach(Match funds in mc){
                if (pass % 2 == 1)
                {
                    pass++;
                    continue;
                }

                string fund = funds.Value;
                //把逗号之间的内容提取出来放进string数组里
                string[] all = Regex.Split(fund, ",", RegexOptions.IgnoreCase);
                //新建一行
                DataGridViewRow row = new DataGridViewRow();


                //之后的代码都是把string数组的内容放进每一行里
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows.Add(row);
                    dataGridView1.Rows[index].Cells[0].Value = all[0];
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[1].Value = all[1].Substring(0, (all[1].Length > 6 ? 6 : all[1].Length));
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[2].Value = all[3].Length == 0 ? "---" : all[3].Substring(5);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[3].Value = all[4].Length == 0 ? "---" : all[4];
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[4].Value = all[5].Length == 0 ? "---" : all[5];
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[5].Value = getPecent(all[6]);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[6].Value = getPecent(all[7]);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[7].Value = getPecent(all[8]);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[8].Value = getPecent(all[9]);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[9].Value = getPecent(all[10]);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[10].Value = getPecent(all[11]);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[11].Value = getPecent(all[12]);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[12].Value = getPecent(all[13]);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[13].Value = getPecent(all[14]);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[14].Value = getPecent(all[15]);
                });
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows[index].Cells[15].Value = all[20];
                });
                //换行
                index++;
                pass++;
            }
            //改变页码
            page = pi;
            label1.Text = page + "/55";

            //清空文字框内容
            textBox1.Text = "";
            return url;
        }

        //获得两位有效数字的字符串百分数
        string getPecent(string temp)
        {
            if (temp.Length == 0)
            {
                return "---";
            }
            double d = Math.Round(Convert.ToDouble(temp), 2);
            temp = d.ToString();
            if (Convert.ToInt32(d) - d == 0)
                temp += ".00%";
            else if (Convert.ToInt32(d*10) - d*10 == 0)
                temp += "0%";
            else
                temp += "%";
            return temp;
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }
        
        //下一页
        private void button2_Click(object sender, EventArgs e)
        {
            if (page == 55)
                return;
            turnTo(page + 1);
        }
        
        //上一页
        private void button1_Click(object sender, EventArgs e)
        {
            if (page == 1)
                return;
            turnTo(page - 1);
        }
        
        //转到特定页
        private void button3_Click(object sender, EventArgs e)
        {
            //使用try catch通过文字框内容转到特定页，并抛出错误输出
            try
            {
                int a = Convert.ToInt32(textBox1.Text);
                if (!(a > 0 && a < 56))
                    throw new Exception();
                turnTo(a);
            }
            catch(Exception ex)
            {
                MessageBox.Show("输入错误，请检查您输入的数字（1-55）。", "提示");
                textBox1.Text = "";
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button5.Enabled = true;
            }
        }

        //输入基金代码查看某只基金详情
        private void button5_Click(object sender, EventArgs e)
        {
            string str = textBox2.Text;

            //通过try catch把输入的基金代码转到网页，若网页不存在，则抛出错误，
            //同时把输入的错误基金代号的格式抛出
            try
            {
                string x = GetContent(@"http://fund.eastmoney.com/" + str + ".html");
                x = x.Substring(x.IndexOf("<title>") + 7);
                x = x.Substring(0, x.IndexOf("</title>"));
                if (!(x.Contains(str)))
                {
                    throw new Exception();
                }
                Form2 f = new Form2(str);
                f.Show();
            }
            catch (Exception ex)
            {
                MessageBox.Show("输入错误，不存编号为" + str + "的基金。", "提示");
                textBox1.Text = "";
                textBox1.Enabled = true;
                textBox2.Enabled = true;
                button1.Enabled = true;
                button2.Enabled = true;
                button3.Enabled = true;
                button5.Enabled = true;
            }
        }


        //查看某只基金详情
        private void dataGridView1_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            Form2 f = new Form2(dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString());
            f.Show();
        }
        
        //更新股票数据
        private void button4_Click(object sender, EventArgs e)
        {

            //todo:multi-thread
            for (int i = 1; i < 56; i++)
            {
                this.Invoke((EventHandler)delegate
                {
                    string url = "http://fund.eastmoney.com/data/rankhandler.aspx?op=ph&dt=kf&ft=all&rs=&gs=0&sc=zzf&st=desc&sd=2015-10-29&ed=2016-10-29&qdii=&tabSubtype=,,,,,&pi="
                        + i + "&pn=50&dx=1&v=0.10850418109563731";

                    string data = GetContent(url);
                    //正则表达式，提取每两个引号之间内容
                    Regex re = new Regex("(?<=\").*?(?=\")", RegexOptions.None);

                    //用正则表达式提取内容
                    MatchCollection mc = re.Matches(data);
                    foreach (Match funds in mc)
                    {
                        string fund = funds.Value;
                        //把逗号之间的内容提取出来放进string数组里
                        string[] all = Regex.Split(fund, ",", RegexOptions.IgnoreCase);
                        if (all[0].Length == 0)
                            continue;
                        fundsID.Add(all[0]);
                    }
                });
            }
            
            string url2;
            string HTML;
            foreach (string ID in fundsID)
            {
                url2 = "http://fund.eastmoney.com/" + ID + ".html";

                HTML = GetContent(url2);

                inText(HTML, ID);
            }

            List<string> names = GetAll();

            string content;
            string total, percent;

            double sums;
            foreach(string Name in names)
            {
                content = Read(Name);
                content = content.Substring(content.IndexOf("基金规模") + 9);
                total = content.Substring(0, content.IndexOf("亿"));
                if (total == "--")
                    continue;
                sums = Convert.ToDouble(total);


                content = content.Substring(content.IndexOf("股票名称"));
                content = content.Substring(0, content.IndexOf("</table>"));
                while (content.Contains("alignLeft"))
                {
                    content = content.Substring(content.IndexOf("alignLeft"));
                    content = content.Substring(content.IndexOf(">") + 1);
                    string tmp1 = content.Substring(0, 2);
                    if (tmp1 == "<a")
                        content = content.Substring(content.IndexOf(">") + 1);
                    string stockName = content.Substring(0, content.IndexOf("<"));
                    content = content.Substring(content.IndexOf("alignRight"));
                    content = content.Substring(content.IndexOf(">") + 1);
                    percent = content.Substring(0, content.IndexOf("<") - 1);
                    sums *= Convert.ToDouble(percent) / 100.0;
                    bool matchs = false;
                    foreach (Stock sto in stocks)
                    {
                        if (sto.name == stockName)
                        {
                            sto.sum += sums;
                            matchs = true;
                            break;
                        }
                    }
                    if (!matchs)
                    {
                        Stock st = new Stock();
                        st.name = stockName;
                        st.sum = sums;
                        stocks.Add(st);
                    }
                }
            }
            List<Stock> tmp = new List<Stock>();
            bool IN;
            foreach(Stock st in stocks)
            {
                IN = false;
                for (int i = 0; i < tmp.Count; i++)
                {
                    if (tmp.ElementAt(i).sum < st.sum)
                    {
                        tmp.Insert(i, st);
                        IN = true;
                        break;
                    }
                }
                if (!IN)
                {
                    tmp.Add(st);
                }
            }
            string result = "";
            for(int i = 0; i < 10; i++)
            {
                result += tmp.ElementAt(i).name + "," + tmp.ElementAt(i).sum + ",";
            }
            inText(result, "result\\result", "Default");
        }

        void inText(string data, string name,string str="UTF8")
        {
            FileStream fs = new FileStream("..\\..\\stock\\" + name + ".txt", FileMode.Create);
            //获得字节数组
            byte[] datas;
            if (str == "UTF8")
            {
                datas = System.Text.Encoding.UTF8.GetBytes(data);
                fs.Write(datas, 0, data.Length);
            }
            else
            {
                datas = System.Text.Encoding.Default.GetBytes(data);
                fs.Write(datas, 0, datas.Length);
            }
            //开始写入
            //清空缓冲区、关闭流
            fs.Flush();
            fs.Close();
        }

        List<string> GetAll()//搜索文件夹中的文件
        {
            //list数组步用指定长度
            List<string> b = new List<string>();

            //读取路径下的信息
            DirectoryInfo folder = new DirectoryInfo("..\\..\\stock\\");
            
            for (int i = 0; i < folder.GetFiles("*.txt").Count(); i++)
            {
                b.Add(folder.GetFiles("*.txt")[i].Name);
            }
            
            return b;
        }

        private string Read(string name)
        {
            StreamReader sr = new StreamReader("..\\..\\stock\\" + name, Encoding.UTF8);
            String data = sr.ReadToEnd();
            return data;
        }

        private void button6_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4();
            form4.Show();
        }
    }

    public class Stock
    {
        public string name { get; set; }
        public double sum { get; set; }
    }
}
