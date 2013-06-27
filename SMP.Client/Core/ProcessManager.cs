using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using SMP.Client.Models;

namespace SMP.Client.Core
{
    public class ProcessManager
    {
        public string BaseUrl { get; set; }

        private List<Processo> processos = new List<Processo>();
        private List<Registro> registros = new List<Registro>();

        public event Action<string> ErroCapturado;
        public event Action<Registro> ProcessoEncerrado;
        
        public void BaixarListaProcessos()
        {
            try
            {
                WebClient client = new WebClient();

                client.DownloadStringCompleted += (sender, e) =>
                {
                    if (e.Error != null)
                    {
                        if (ErroCapturado != null)
                        {
                            ErroCapturado("Falha ao baixar lista de processos: " + e.Error.Message);
                        }

                        return;
                    }

                    processos = JsonConvert.DeserializeObject<List<Processo>>(e.Result);
                };

                client.DownloadStringAsync(new Uri(BaseUrl + "processo"));
            }
            catch (Exception ex)
            {
                if (ErroCapturado != null)
                {
                    ErroCapturado("Falha ao baixar lista de processos: " + ex.Message);
                }
            }
        }
        
        public void EncerrarProcessos()
        {
            foreach (var processo in processos)
            {
                foreach (var item in Process.GetProcessesByName(processo.Nome))
                {
                    try
                    {
                        item.Kill();
                        AdicionarRegistro(processo.Nome);
                    }
                    catch (Exception ex)
                    {
                        if (ErroCapturado != null)
                        {
                            ErroCapturado("Falha ao encerrar processo: " + ex.Message);
                        }
                    }
                }
            }
        }

        public void RegistroVerificacao()
        {
            AdicionarRegistro("Verificacao_ok");
        }

        private void AdicionarRegistro(string nome)
        {
            Registro registro = new Registro();
            registro.Processo = nome;
            registro.Data = DateTime.Now;
            registro.Ip = Utils.GetIP();
            registros.Add(registro);

            if (ProcessoEncerrado != null)
            {
                ProcessoEncerrado(registro);
            }

            WebClient client = new WebClient();
            client.BaseAddress = BaseUrl;
            client.Headers[HttpRequestHeader.ContentType] = "application/json";

            string data = JsonConvert.SerializeObject(registro);

            client.UploadString("registro", data);

        }

        public List<Registro> GetRegistros()
        {
            return registros;
        }
    }
}

