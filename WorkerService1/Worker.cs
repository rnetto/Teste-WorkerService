using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService1
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private bool paraWile = true;

        public Worker(ILogger<Worker> logger)
        {
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            string diretorioLog = "D:\\logWorkerService.txt";
            string txtOld;
            string txtStatus;
            string txtNew = "Servi�o esta rodando:" + DateTimeOffset.Now;

            while (paraWile)
            {
                _logger.LogInformation("Servi�o esta rodando :{time}", DateTimeOffset.Now);

                if ((File.Exists(diretorioLog)))
                    txtOld = File.ReadAllText(diretorioLog);
                else
                {
                    //criando o arquivo de log e '.Close()' pra poder ler o arquivo;
                    File.Create(diretorioLog).Close();
                    txtOld = File.ReadAllText(diretorioLog);
                }

                //TimeSpan(dia, hora, min, seg)
                //var timeout = new TimeSpan(1,0,0,0);
                //_logger.LogInformation(timeout.ToString());
                //Thread.Sleep(timeout);

                var url = "https://viacep.com.br/ws/36090040/json/";
                var request = new HttpRequestMessage(HttpMethod.Get, url);

                using (var client = new HttpClient())
                {
                    var response = await client.SendAsync(request);
                    if (response.IsSuccessStatusCode)
                    {
                        paraWile = true;
                        _logger.LogInformation("Serci�o realizado: {time}", DateTimeOffset.Now);
                        txtStatus = "Servi�o realizado com sucesso: " + DateTimeOffset.Now;
                    }
                    else
                    {
                        paraWile = false;
                        _logger.LogInformation("Serci�o n�o realizado: {time}", DateTimeOffset.Now);
                        txtStatus = "Servi�o com erro: " + DateTimeOffset.Now;
                    }
                }

                using (var logTxt = new StreamWriter(diretorioLog))
                {
                    logTxt.WriteLine(txtOld + Environment.NewLine +
                                        txtNew + Environment.NewLine +
                                        txtStatus + Environment.NewLine);
                }

                await Task.Delay(10000, stoppingToken);

            }
            using (var logTxt = new StreamWriter(diretorioLog))
            {
                logTxt.WriteLine("<<<< ERROR >>>>" + Environment.NewLine +
                                    "Servi�o parado - Hor�rio: {time}", DateTime.Now
                                    + Environment.NewLine + "<<<< ERROR >>>>" + Environment.NewLine);
            }
            _logger.LogInformation("Serci�o parou: {time}", DateTimeOffset.Now);
        }
    }
}
