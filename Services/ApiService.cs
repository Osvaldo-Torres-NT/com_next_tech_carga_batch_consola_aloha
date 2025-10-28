using System.Text.Json;
using RestSharp;
using System.Threading.Tasks;

namespace com_next_tech_carga_batch_consola_aloha.Services
{
    public static class ApiService
    {
        public static async Task<ResponseApi> SendToTraductor(string fileXML, string fileName, Config config, string transaccion)
        {
            ResponseApi responsePago = new ResponseApi();
            try
            {
                Request requestPago = new Request
                {
                    xml = fileXML,
                    transaccion = transaccion,
                    nombre_archivo = fileName
                };

                RestClientOptions options = new RestClientOptions(config.urlTraductor)
                {
                    ThrowOnAnyError = true,
                    Timeout = System.TimeSpan.FromMinutes(config.maxtTimeout)

                };
                RestClient client = new RestClient(options);
                RestRequest request = new RestRequest { Method = Method.Post };
                request.AddHeader("Content-Type", "application/json");
                request.AddJsonBody(requestPago);
                Console.WriteLine($"[{DateTime.Now:g}] Request: {JsonSerializer.Serialize(requestPago)}");

                RestResponse response = await client.ExecuteAsync(request);
                Console.WriteLine($"[{DateTime.Now:g}] Request para Api: {response.Content}");

                if (response.IsSuccessful)
                    responsePago = JsonSerializer.Deserialize<ResponseApi>(response.Content);
                else
                    responsePago.error = (string.IsNullOrEmpty(response.ErrorMessage)) ? "No se encontro informacion del UUID" : response.ErrorMessage;

                return responsePago;
            }
            catch (System.Exception ex)
            {
                responsePago.error = ex.Message;
                return responsePago;
            }
        }
    }

}