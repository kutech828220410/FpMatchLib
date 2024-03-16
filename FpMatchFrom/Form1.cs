using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Basic;
using FpMatchLib;
namespace FpMatchFrom
{
    public partial class Form1 : Form
    {
        public MyThread myThread = new MyThread();
        public bool Abort = false;
        private FpMatchSoket fpMatchSoket = new FpMatchSoket();
        public Form1()
        {
            FpMatchSoket.secLevel = 3;
            FpMatchSoket.ConsoleWrite = true;
            InitializeComponent();
            this.Load += Form1_Load;
            this.button_Init.Click += Button_Init_Click;
            this.button_Enroll.Click += Button_Enroll_Click;
            this.button_Abort.Click += Button_Abort_Click;
            this.button_GetFrature.Click += Button_GetFrature_Click;
            this.button_Match.Click += Button_Match_Click;
            fpMatchSoket.Init($@"127.0.0.1:19002/ws");           
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            myThread.AutoRun(true);
            myThread.SetSleepTime(100);
            myThread.Add_Method(sub_program);
            myThread.Trigger();
        }

        private void sub_program()
        {
            this.Invoke(new Action(delegate 
            {
                if(fpMatchSoket.IsBusy == false)
                {
                    label_狀態.Text = "閒置中";
                    label_狀態.ForeColor = Color.White;
                    label_狀態.BackColor = Color.Green;
                }
                else
                {
                    label_狀態.Text = "忙碌中";
                    label_狀態.ForeColor = Color.White;
                    label_狀態.BackColor = Color.Red;
                }
            }));
        }
        private void Button_Match_Click(object sender, EventArgs e)
        {
            bool flag = fpMatchSoket.Match(textBox_FeatureTemplate.Text, textBox_FeatureValue.Text);
            this.Invoke(new Action(delegate
            {
                label_Score.Text = $"{(flag ? "成功":"失敗")}";
                if(flag)
                {
                    label_Score.BackColor = Color.Green;
                }
                else
                {
                    label_Score.BackColor = Color.Red;
                }
            }));

        }
        private void Button_GetFrature_Click(object sender, EventArgs e)
        {
            Task.Run(new Action(delegate
            {
                FpMatchClass fpMatch = fpMatchSoket.GetFeature();
                if(fpMatch.featureLen == 768)
                {
                    this.Invoke(new Action(delegate
                    {
                        textBox_FeatureValue.Text = fpMatch.feature;
                    }));         
                }
            }));
        }
        private void Button_Abort_Click(object sender, EventArgs e)
        {
            fpMatchSoket.Abort();
        }
        private void Button_Enroll_Click(object sender, EventArgs e)
        {
            Task.Run(new Action(delegate
            {
                FpMatchClass fpMatch =  fpMatchSoket.Enroll();
                if (fpMatch.featureLen == 768)
                {
                    this.Invoke(new Action(delegate
                    {
                        textBox_FeatureValue.Text = fpMatch.feature;
                    }));
                }
            }));
         
        }

        async private void Button_Init_Click(object sender, EventArgs e)
        {
            fpMatchSoket.Open();
        }
    }
}
