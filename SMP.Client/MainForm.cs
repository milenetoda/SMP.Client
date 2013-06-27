using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SMP.Client.Core;
using SMP.Client.Models;


namespace SMP.Client
{
    public partial class MainForm : Form
    {
        private ProcessManager _processManager;

        public MainForm(ProcessManager processManager)
        {
            InitializeComponent();

            processManager.ProcessoEncerrado += processManager_ProcessoEncerrado;
            processManager.ErroCapturado += processManager_ErroCapturado;
            
            foreach (var item in processManager.GetRegistros())
            {
                AdicionarRegistro(item);   
            }

            _processManager = processManager;
        }

        private void processManager_ErroCapturado(string message)
        {
            if (chkErros.Checked)
            {
                listBox1.Items.Add(message);
            }
        }

        private void processManager_ProcessoEncerrado(Registro registro)
        {
            AdicionarRegistro(registro);
        }

        private void AdicionarRegistro(Registro registro)
        {
            listBox1.Items.Add(registro.Processo + " " + registro.Data);
        }
                

        private void Form2_Load(object sender, EventArgs e)
        {
            ShowInTaskbar = false;
        }
        
        private void Form2_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.Close();
            }
        }

        private void txtServidor_TextChanged(object sender, EventArgs e)
        {
            UpdateServerUrl();
        }

        private void txtServidor_KeyUp(object sender, KeyEventArgs e)
        {
            UpdateServerUrl();
        }

        private void UpdateServerUrl()
        {
            Properties.Settings.Default.Save();
            _processManager.BaseUrl = txtServidor.Text;
        }

    }
}

