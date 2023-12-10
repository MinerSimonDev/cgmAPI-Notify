using System;
using System.Drawing;
using System.Net;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace cgmAPI_Notify
{
    public partial class Form1 : Form
    {
        private NotifyIcon notifyIcon;
        private Timer timer;

        public Form1()
        {
            InitializeComponent();
            notifyIcon = new NotifyIcon();
            timer = new Timer();
            timer.Interval = 60000;
            timer.Tick += Timer_Tick;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            checkBs();
        }


        public void checkBs()
        {
            using (WebClient webclient = new WebClient())
            {
                string json = webclient.DownloadString("http://192.168.1.36:3000/api/v1/currentBloodSugar/raw");
                BloodSugarData data = JsonConvert.DeserializeObject<BloodSugarData>(json);

                if (data.sgv >= 170) {
                    ShowNotification("Blutzucker", $"HOCH | BZ: {data.sgv} Δ {data.delta}");
                } else if (data.sgv < 80)
                {
                    ShowNotification("Blutzucker", $"HOCH | BZ: {data.sgv} Δ {data.delta}");
                }
            }
        }

        public void ShowNotification(string title, string text)
        {
            notifyIcon.Icon = SystemIcons.Warning;
            notifyIcon.BalloonTipTitle = title;
            notifyIcon.BalloonTipText = text;
            notifyIcon.BalloonTipIcon = ToolTipIcon.Info;
            notifyIcon.Visible = true;

            notifyIcon.ShowBalloonTip(5000);
        }
    }

    public class BloodSugarData
    {
        public int currentBloodSugar { get; set; }
        public double delta { get; set; }
        public int sgv { get; set; }
        public string dateString { get; set; }
    }
}
