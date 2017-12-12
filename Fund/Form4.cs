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
using System.IO;

namespace Fund
{
    public partial class Form4 : Form
    {
        Thread td;
        public Form4()
        {
            InitializeComponent();
            ThreadStart ts = new ThreadStart(GetIntroduction);
            td = new Thread(ts);
            td.SetApartmentState(ApartmentState.STA);
            td.Start();
        }

        void GetIntroduction()
        {
            string str = Read();
            string[] all = str.Split(',');
            for(int i = 0; i < 10; i++)
            {
                DataGridViewRow row = new DataGridViewRow();
                
                this.Invoke((EventHandler)delegate
                {
                    dataGridView1.Rows.Add(row);
                    dataGridView1.Rows[i].Cells[0].Value = i + 1;
                    dataGridView1.Rows[i].Cells[1].Value = all[i * 2];

                    double d = Convert.ToDouble(all[i * 2 + 1]);
                    d = Math.Round(d, 2);
                    dataGridView1.Rows[i].Cells[2].Value = d;
                });
            }
        }

        string Read()
        {
            StreamReader sr = new StreamReader("..\\..\\stock\\result\\result.txt", Encoding.Default);
            String data = sr.ReadToEnd();
            return data;
        }
    }
}
