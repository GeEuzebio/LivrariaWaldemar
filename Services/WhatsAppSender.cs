using Microsoft.Extensions.Configuration;
using System.Diagnostics;
using System.Text;
namespace LibraryApp.Services
{
    public class WhatsAppSender
    {
        private readonly IConfiguration _config;

        public WhatsAppSender(IConfiguration config)
        {
            _config = config;
        }

        public async Task sendMessage(string phone, string message)
        {
                using HttpClient client = new HttpClient();
                string apiUrl = "https://cluster.apigratis.com/api/v2/whatsapp/sendText";
                string requestBody = $"{{\"number\":\"{phone}\",\"text\":\"{message}\",\"time_typing\":\"1\"}}";
                string? contentType = _config.GetSection("WhatsappSettings").GetSection("Content-Type").Value;
                string? deviceToken = _config.GetSection("WhatsappSettings").GetSection("DeviceToken").Value;
                string? authorization = _config.GetSection("WhatsappSettings").GetSection("Authorization").Value;

                var request = new HttpRequestMessage(HttpMethod.Post, apiUrl);
                request.Headers.Add("DeviceToken", deviceToken);
                request.Headers.Add("Authorization", authorization);
                request.Content = new StringContent(requestBody, Encoding.UTF8, contentType!);
            
                HttpResponseMessage response = await client.SendAsync(request);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Mensagem enviada com sucesso!");
                }
                else
                {
                    Debug.WriteLine($"Erro na solicitação: {response.StatusCode} - {response.ReasonPhrase}");
                }

        }
    }
}
