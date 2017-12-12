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

namespace Fund
{
    public partial class Form3 : Form
    {
        string ID;
        Thread td;
        //初始化
        public Form3(string id)
        {
            InitializeComponent();
            ID = id;
            ThreadStart ts = new ThreadStart(GetIntroduction);
            td = new Thread(ts);
            td.SetApartmentState(ApartmentState.STA);
            td.Start();
        }

        void GetIntroduction()
        {
            string url = @"http://fund.eastmoney.com/f10/FundArchivesDatas.aspx?type=jjcc&code=" + ID + "&topline=10&year=&month=&rt=0.029766627475606988";

            string data = GetContent(url);
            //如果没有表格内容，则返回
            //string的contain方法是判断字符串是否有一段特定的字符
            if (!data.Contains("<tbody>"))
                return;
            if (!data.Contains("市场"))
            {
                data = data.Substring(data.IndexOf("<tbody>") + 7);
                data = data.Substring(0, data.IndexOf("</tbody>"));
                int index = 0;
                string tmp;
                //处理字符串，获取需要的信息
                while (data.Contains("<tr>"))
                {

                    DataGridViewRow row = new DataGridViewRow();

                    data = data.Substring(data.IndexOf("<td>") + 4);
                    tmp = data.Substring(0, data.IndexOf("<"));
                    this.Invoke((EventHandler)delegate
                    {
                        dataGridView1.Rows.Add(row);
                        dataGridView1.Rows[index].Cells[0].Value = tmp;
                    });

                    data = data.Substring(data.IndexOf("<td>") + 4);
                    tmp = data.Substring(data.IndexOf(">") + 1);
                    //tmp = tmp.Substring(0, tmp.IndexOf("</a"));
                    tmp = tmp.Substring(0, tmp.IndexOf("<"));
                    this.Invoke((EventHandler)delegate
                    {
                        dataGridView1.Rows[index].Cells[1].Value = tmp;
                    });

                    data = data.Substring(data.IndexOf("<td"));
                    data = data.Substring(data.IndexOf(">") + 1);
                    tmp = data.Substring(data.IndexOf(">") + 1);
                    tmp = tmp.Substring(0, tmp.IndexOf("<"));
                    this.Invoke((EventHandler)delegate
                    {
                        dataGridView1.Rows[index].Cells[2].Value = tmp;
                    });

                    //跳过无用数据
                    data = data.Substring(data.IndexOf("<td") + 3);
                    data = data.Substring(data.IndexOf("<td") + 3);
                    data = data.Substring(data.IndexOf("<td") + 3);


                    data = data.Substring(data.IndexOf("<td") + 3);
                    tmp = data.Substring(data.IndexOf(">") + 1);
                    tmp = tmp.Substring(0, tmp.IndexOf("<"));
                    this.Invoke((EventHandler)delegate
                    {
                        dataGridView1.Rows[index].Cells[3].Value = tmp;
                    });

                    data = data.Substring(data.IndexOf("<td") + 3);
                    tmp = data.Substring(data.IndexOf(">") + 1);
                    tmp = tmp.Substring(0, tmp.IndexOf("<"));
                    this.Invoke((EventHandler)delegate
                    {
                        dataGridView1.Rows[index].Cells[4].Value = tmp;
                    });

                    data = data.Substring(data.IndexOf("<td"));
                    tmp = data.Substring(data.IndexOf(">") + 1);
                    tmp = tmp.Substring(0, tmp.IndexOf("<"));
                    this.Invoke((EventHandler)delegate
                    {
                        dataGridView1.Rows[index].Cells[5].Value = tmp;
                    });
                    index++;
                }
            }
            else
            {
                data = data.Substring(data.IndexOf("<tbody>") + 7);
                data = data.Substring(0, data.IndexOf("</tbody>"));
                int index = 0;
                while (data.Contains("<tr>"))
                {

                    DataGridViewRow row = new DataGridViewRow();
                    this.Invoke((EventHandler)delegate
                    {
                        dataGridView1.Rows.Add(row);
                    });
                    for (int i = 0; i < 6; i++)
                    {
                        data = data.Substring(data.IndexOf("<td") + 3);
                        data = data.Substring(data.IndexOf(">") + 1);
                        string tmp = data.Substring(0, data.IndexOf("<"));
                        this.Invoke((EventHandler)delegate
                        {
                            dataGridView1.Rows[index].Cells[i].Value = tmp;
                        });
                    }
                    index++;
                }
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
                StreamReader sr = new StreamReader(response.GetResponseStream(), Encoding.UTF8);
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
    }
}
