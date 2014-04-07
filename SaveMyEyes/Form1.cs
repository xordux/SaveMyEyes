using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace SaveMyEyes
{
    public partial class Form1 : Form
    {
        public int WM_SYSCOMMAND = 0x0112;
        public int SC_MONITORPOWER = 0xF170;
        Timer timer = new Timer() ;
       
        public Form1()
        {
            InitializeComponent();
        }
        public static class MonitorHelper
        {
            [DllImport("user32.dll")]
            public static extern int SendMessage(int hWnd, int Msg, int wParam, int lParam);

            public static void TurnOn()
            {
                SendMessage(-1, WM_SYSCOMMAND, SC_MONITORPOWER, MONITOR_ON);
            }

            public static void TurnOff()
            {
                SendMessage(-1, WM_SYSCOMMAND, SC_MONITORPOWER, MONITOR_OFF);
            }

            const int SC_MONITORPOWER = 0xF170;
            const int WM_SYSCOMMAND = 0x0112;
            const int MONITOR_ON = -1;
            const int MONITOR_OFF = 2;
        }

        int interval;
        public void FormWithTimer()
        {
            timer = new Timer();
            timer.Tick += new EventHandler(turnOff); // Everytime timer ticks, turnOff will be called
            
            timer.Interval = (1000) * interval * 60;     // Timer will tick evert 'interval' minutes
            timer.Enabled = true;                       // Enable the timer
            timer.Start();                              // Start the timer
        }

        private void turnOff(object sender, EventArgs e)
        {
            timer.Stop();
            MonitorHelper.TurnOff();
            int result; //result is duration of break
            result = (int.TryParse(textBox2.Text, out result)) ? result : 20;
            textBox2.Text = result.ToString();

            while (result > 0)
            {
                MonitorHelper.TurnOff();
                System.Threading.Thread.Sleep(1000);
                result--;
            }
            MonitorHelper.TurnOn();
            timer.Start(); 
        }
        private void button1_Click(object sender, EventArgs e)
        {
            int result;
            interval = 20;
            interval = (int.TryParse(textBox1.Text, out result)) ? result : 20;
            textBox1.Text = interval.ToString();
            
            if (timer.Enabled)
            {
                timer.Stop();
                button1.Text = "Start";
            }
            else
            {
                button1.Text = "Stop";
                FormWithTimer();
            }
        }

        // This code is for starting application in tray , (see Program.cs to fully understand this code)
        
    }
}
