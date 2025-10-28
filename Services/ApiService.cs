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

                // Simular respuesta exitosa (para pruebas)
                //string mockResponseTrue = "{\"exito\":true,\"message\":\"Factura generada correctamente\",\"response_facturacion\":{\"xml\":\"PD94bWwgdmVyc2lvbj0iMS4wIj8+CjxmYWN0dXJhPgogIDxkYXRvPgogICAgPGVtaXNvcj5BQkMwMTAxMDFBQUE8L2VtaXNvcj4KICAgIDxyZWNlcHRvcj5CRUIwMjAyMDJCQkI8L3JlY2VwdG9yPgogICAgPGZlY2hhPjIwMjUtMTAtMTBUMTU6MzA6MDA8L2ZlY2hhPgogICAgPGNvbmNlcHRvPkZBQ1RVUkEgREUgUFJVRUJBPC9jb25jZXB0bz4KICAgIDx0b3RhbD4xMDAuMDA8L3RvdGFsPgogICAgPG1lbnNhamU+SG9sYSBNdW5kbzwvbWVuc2FqZT4KICA8L2RhdG8+CjwvZmFjdHVyYT4=\",\"pdf\":\"JVBERi0xLjQKJcTl8uXrp/Og0MTGCjEgMCBvYmoKPDwKL1R5cGUgL0NhdGFsb2cKL1BhZ2VzIDIgMCBSCj4+CmVuZG9iagoKMiAwIG9iago8PAovVHlwZSAvUGFnZXMKL0tpZHNbMyAwIFJdCj4+CmVuZG9iagoKMyAwIG9iago8PAovVHlwZSAvUGFnZQovUGFyZW50IDIgMCBSCi9SZXNvdXJjZXMgPDwvRm9udCA8PC9GMSA0IDAgUj4+PgovTWVkaWFCb3ggWzAgMCA1OTUuMjggODQxLjg5XQovQ29udGVudHMgNSAwIFIKPj4KZW5kb2JqCgo0IDAgb2JqCjw8Ci9UeXBlIC9Gb250Ci9TdWJ0eXBlIC9UeXBlMQovTmFtZSAvRjEKL0Jhc2VGb250IC9IZWx2ZXRpY2EKL0VuY29kaW5nIC9XaW5BbnNpRW5jb2RpbmcKPj4KZW5kb2JqCgo1IDAgb2JqCjw8Ci9MZW5ndGggNDYKPj4Kc3RyZWFtCkJUCi9GMSAxMiBUZgowIDAgVGYKKChIb2xhIE11bmRvKSAwIDUwIFRkCkVUCmVuZHN0cmVhbQplbmRvYmoKCnhyZWYKMCA2CjAwMDAwMDAwMDAgNjU1MzUgZiAKMDAwMDAwMDExNyAwMDAwMCBuIAowMDAwMDAwMDY3IDAwMDAwIG4gCjAwMDAwMDAxNjIgMDAwMDAgbiAKMDAwMDAwMDI0MCAwMDAwMCBuIAowMDAwMDAwMzAwIDAwMDAwIG4gCnRyYWlsZXIKPDwKL1NpemUgNgovUm9vdCAxIDAgUgovSW5mbyA8PC9Qcm9kdWNlciAoR1BULTUpL1RpdGxlIChIb2xhIE11bmRvKT4+PgpzdGFydHhyZWYKNDA0CiUlRU9G\",\"uuid\":\"A12345B6-C789-4D01-9EF2-ABCDE1234567\"},\"detail\":\"\"}";
                //string mockResponseTrueError = "{\"exito\":false,\"message\":\"Factura generada correctamente\",\"response_facturacion\":{\"xml\":\"PD94bWwgdmVyc2lvbj0iMS4wIj8+CjxmYWN0dXJhPgogIDxkYXRvPgogICAgPGVtaXNvcj5BQkMwMTAxMDFBQUE8L2VtaXNvcj4KICAgIDxyZWNlcHRvcj5CRUIwMjAyMDJCQkI8L3JlY2VwdG9yPgogICAgPGZlY2hhPjIwMjUtMTAtMTBUMTU6MzA6MDA8L2ZlY2hhPgogICAgPGNvbmNlcHRvPkZBQ1RVUkEgREUgUFJVRUJBPC9jb25jZXB0bz4KICAgIDx0b3RhbD4xMDAuMDA8L3RvdGFsPgogICAgPG1lbnNhamU+SG9sYSBNdW5kbzwvbWVuc2FqZT4KICA8L2RhdG8+CjwvZmFjdHVyYT4=\",\"pdf\":\"JVBERi0xLjQKJcTl8uXrp/Og0MTGCjEgMCBvYmoKPDwKL1R5cGUgL0NhdGFsb2cKL1BhZ2VzIDIgMCBSCj4+CmVuZG9iagoKMiAwIG9iago8PAovVHlwZSAvUGFnZXMKL0tpZHNbMyAwIFJdCj4+CmVuZG9iagoKMyAwIG9iago8PAovVHlwZSAvUGFnZQovUGFyZW50IDIgMCBSCi9SZXNvdXJjZXMgPDwvRm9udCA8PC9GMSA0IDAgUj4+PgovTWVkaWFCb3ggWzAgMCA1OTUuMjggODQxLjg5XQovQ29udGVudHMgNSAwIFIKPj4KZW5kb2JqCgo0IDAgb2JqCjw8Ci9UeXBlIC9Gb250Ci9TdWJ0eXBlIC9UeXBlMQovTmFtZSAvRjEKL0Jhc2VGb250IC9IZWx2ZXRpY2EKL0VuY29kaW5nIC9XaW5BbnNpRW5jb2RpbmcKPj4KZW5kb2JqCgo1IDAgb2JqCjw8Ci9MZW5ndGggNDYKPj4Kc3RyZWFtCkJUCi9GMSAxMiBUZgowIDAgVGYKKChIb2xhIE11bmRvKSAwIDUwIFRkCkVUCmVuZHN0cmVhbQplbmRvYmoKCnhyZWYKMCA2CjAwMDAwMDAwMDAgNjU1MzUgZiAKMDAwMDAwMDExNyAwMDAwMCBuIAowMDAwMDAwMDY3IDAwMDAwIG4gCjAwMDAwMDAxNjIgMDAwMDAgbiAKMDAwMDAwMDI0MCAwMDAwMCBuIAowMDAwMDAwMzAwIDAwMDAwIG4gCnRyYWlsZXIKPDwKL1NpemUgNgovUm9vdCAxIDAgUgovSW5mbyA8PC9Qcm9kdWNlciAoR1BULTUpL1RpdGxlIChIb2xhIE11bmRvKT4+PgpzdGFydHhyZWYKNDA0CiUlRU9G\",\"uuid\":\"A12345B6-C789-4D01-9EF2-ABCDE1234567\"},\"detail\":\"\"}";
                //string mockResponseError = "{\"exito\":false,\"message\":\"Error al generar la factura\",\"response_facturacion\":{\"xml\":\"\",\"pdf\":\"\",\"uuid\":\"\"},\"detail\":\"El RFC del receptor no es válido o no coincide con el régimen fiscal.\"}";
                //responsePago.responseXML = mockResponseError;

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