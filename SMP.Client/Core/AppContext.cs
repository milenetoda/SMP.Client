using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SMP.Client.Properties;

namespace SMP.Client.Core
{
    public class AppContext : ApplicationContext
    {
        private MainForm form;
        private NotifyIcon tray;
        private Timer timer1;
        private Timer timer2;
        private Timer timer3;
        private ProcessManager processManager;

        public AppContext()
        {
            processManager = new ProcessManager();

            processManager.BaseUrl = Properties.Settings.Default.ServerUrl;

            tray = new NotifyIcon();
            tray.Icon = Resources.logo;
            tray.Visible = true;
            tray.DoubleClick += tray_DoubleClick;
            this.ThreadExit += AppContext_ThreadExit;
            
            timer1 = new Timer();
            timer1.Enabled = true;
            timer1.Interval = 100;
            timer1.Tick += timer1_Tick;

            timer2 = new Timer();
            timer2.Enabled = true;
            timer2.Interval = 1000;
            timer2.Tick += timer2_Tick;

            timer3 = new Timer();
            timer3.Enabled = true;
            timer3.Interval = 1800000; //meia hora
            timer3.Tick += timer3_Tick;
        }

        void timer3_Tick(object sender, EventArgs e)
        {
            processManager.RegistroVerificacao();
        }

        private void timer2_Tick(object sender, EventArgs e)
        {
            processManager.BaixarListaProcessos();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            processManager.EncerrarProcessos();
        }

        private void tray_DoubleClick(object sender, EventArgs e)
        {
            form = new MainForm(processManager);
            form.Show();
            form.Activate();
        }

        private void AppContext_ThreadExit(object sender, EventArgs e)
        {
            tray.Visible = false;
        }
    }
}
