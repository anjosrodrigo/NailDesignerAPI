using System.Text.Json;
using System.Text;

namespace NailDesignerAPI.Services {
    public class WhatsAppService {

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public WhatsAppService( HttpClient httpClient, IConfiguration configuration ) {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<bool> SendMessageAsync( string phone, string message ) {
            try {
                var instance = _configuration[ "WhatsApp:Instance" ];
                var apikey = _configuration[ "WhatsApp:ApiKey" ];
                var baseUrl = _configuration[ "WhatsApp:BaseUrl" ];

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Add( "apikey", apikey );

                var payload = new {
                    number = phone,
                    text = message
                };

                var json = JsonSerializer.Serialize( payload );
                var content = new StringContent( json, Encoding.UTF8, "application/json" );

                var response = await _httpClient.PostAsync(
                    $"{baseUrl}/message/sendText/{instance}",
                    content
                );

                return response.IsSuccessStatusCode;
            }
            catch( Exception ex ) {
                Console.WriteLine( $"WhatsApp error: {ex.Message}" );
                return false;
            }
        }

        public async Task SendAppointmentConfirmationAsync( string phone, string clientName, DateTime startTime, string serviceName ) {

            var message = $"Olá {clientName}! 😊 \n" +
                          $"Seu agendamento foi confirmado!\n\n" +
                          $"📅 Data: {startTime:dd/MM/yyyy}\n" +
                          $"⏰ Horário: {startTime:HH:mm}\n" +
                          $"💅 Serviço: {serviceName}\n\n" +
                          $"Aguardamos você!";

            await SendMessageAsync( phone, message );
        }

        public async Task SendAppointmentReminderAsync( string phone, string clientName, DateTime startTime, string serviceName ) {

            var message = $"Olá {clientName}! 😊\n" +
                          $"Lembrando seu agendamento amanhã:\n\n" +
                          $"⏰ Horário: {startTime:HH:mm}\n" +
                          $"💅 Serviço: {serviceName} \n\n" +
                          $"Por gentileza, confirme sua presença respondendo SIM ou NÃO.\n" +
                          $"Caso não confirme, seu agendamento será cancelado automaticamente.";

            await SendMessageAsync( phone, message );
        }

        public async Task SendBirthdayMessageAsync( string phone, string clientName ) {

            var message = $"🎉 Feliz Aniversário, {clientName}! 🎂\n\n" +
                          $"Que seu dia seja incrível!\n\n" +
                          $"Com carinho, sua Nail Designer 💅";

            await SendMessageAsync( phone, message );
        }
    }
}
