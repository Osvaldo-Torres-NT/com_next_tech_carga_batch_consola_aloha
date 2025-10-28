

using System.Text;
using System.Xml.Schema;
using Microsoft.VisualBasic;
using Newtonsoft.Json;

namespace com_next_tech_carga_batch_consola_aloha.Services
{
    public class ProcessService
    {
        private const string XML_FILE_PATTERN = "*.xml";

        public async Task<string> Process()
        {
            try
            {
                Config config = ConfigService.getConfig();
                var pathFiles = config.files;
                DirectoryInfo di = new DirectoryInfo(pathFiles);

                // Validar solo leer xml 
                var xmlFiles = di.GetFiles(XML_FILE_PATTERN); //constante

                if (xmlFiles.Length == 0)
                {
                    Console.WriteLine($"[{DateTime.Now.ToString("g")}] No se encontraron archivos XML en la ruta: {pathFiles}");
                    return string.Empty;
                }

                await Parallel.ForEachAsync(xmlFiles, async (file, token) =>
                {
                    string transaccion = Guid.NewGuid().ToString();
                    string log = BitacoraService.initLog(config.logs, Path.GetFileNameWithoutExtension(file.Name));
                    string logError = BitacoraService.initLogError(config.logs, Path.GetFileNameWithoutExtension(file.Name));
                    Console.ForegroundColor = ConsoleColor.Gray;
                    Console.WriteLine($"[{DateTime.Now.ToString("g")}] PROCESANDO ARCHIVO : {file.Name}");
                    try
                    {
                        byte[] fileBytes = await File.ReadAllBytesAsync(file.FullName, token);
                        string base64FileName = Convert.ToBase64String(fileBytes);

                        Console.ForegroundColor = ConsoleColor.Blue;
                        Console.WriteLine($"[{DateTime.Now.ToString("g")}] Archivo {file.Name} convertido a Base64 correctamente.");

                        ResponseApi responsePago = await ApiService.SendToTraductor(base64FileName, file.Name, config, transaccion);

                        if (responsePago != null && !string.IsNullOrEmpty(responsePago.responseXML))
                        {
                            var responseXML = JsonConvert.DeserializeObject<XmlReport>(responsePago.responseXML);
                            if (responseXML != null)
                            {
                                // ESCENARIO 1: éxito = true, response_facturacion con datos, detail " "
                                if (responseXML.exito && !string.IsNullOrEmpty(responseXML.response_facturacion.xml) && string.IsNullOrEmpty(responseXML.detail))
                                {
                                    var itemTrans = responseXML.response_facturacion;

                                    ExportReport.ExportPDF(itemTrans.pdf, itemTrans.uuid, Path.GetFileNameWithoutExtension(file.Name), config, transaccion);
                                    ExportReport.ExportXML(itemTrans.xml, itemTrans.uuid, Path.GetFileNameWithoutExtension(file.Name), config, transaccion);
                                    BitacoraService.writeLog(log, "INFO", "ARCHIVO PROCESADO EXITOSAMENTE CON UUID: " + itemTrans.uuid + ", ID_TRANSACCION: " + transaccion);

                                    File.Move($"{config.files}{file.Name}", $"{config.success}_{DateTime.Now.Ticks}_{file.Name}");
                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"[{DateTime.Now:g}] ÉXITO: Archivo procesado correctamente");
                                }
                                // ESCENARIO 2: éxito = false, response_facturacion con datos, detail " "
                                else if (!responseXML.exito && !string.IsNullOrEmpty(responseXML.response_facturacion.xml) && string.IsNullOrEmpty(responseXML.detail))
                                {
                                    var itemTrans = responseXML.response_facturacion;

                                    ExportReport.ExportPDF(itemTrans.pdf, itemTrans.uuid, Path.GetFileNameWithoutExtension(file.Name), config, transaccion);
                                    ExportReport.ExportXML(itemTrans.xml, itemTrans.uuid, Path.GetFileNameWithoutExtension(file.Name), config, transaccion);
                                    BitacoraService.writeLog(log, "INFO", "ARCHIVO PROCESADO CON ERRORES CON UUID: " + itemTrans.uuid + ", ID_TRANSACCION: " + transaccion);

                                    File.Move($"{config.files}{file.Name}", $"{config.success}_{DateTime.Now.Ticks}_{file.Name}");

                                    string errorDetail = $"Procesamiento fallido pero con estructura completa - Message: {responseXML.message}";
                                    BitacoraService.writeLog(logError, "ERROR", errorDetail + " CON ID_TRANSACCION: " + transaccion);
                                    BitacoraService.writeLog(log, "INFO", "ARCHIVO CON ERROR PERO ESTRUCTURA VÁLIDA MOVIDO A SUCCESS CON ID_TRANSACCION: " + transaccion);

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"[{DateTime.Now:g}] ERROR CON DATOS: {errorDetail}");
                                }
                                // ESCENARIO 3: éxito = false, response_facturacion vacío/nulo, detail con datos
                                else if (!responseXML.exito && string.IsNullOrEmpty(responseXML.response_facturacion.xml) && !string.IsNullOrEmpty(responseXML.detail))
                                {
                                    File.Move($"{config.files}{file.Name}", $"{config.open}_{DateTime.Now.Ticks}_{file.Name}");

                                    string errorDetail = $"Error crítico - Detail: {responseXML.detail}";
                                    BitacoraService.writeLog(logError, "ERROR", errorDetail + " CON ID_TRANSACCION: " + transaccion);
                                    BitacoraService.writeLog(log, "INFO", "ARCHIVO CON ERROR MOVIDO A OPEN CON ID_TRANSACCION: " + transaccion);

                                    Console.ForegroundColor = ConsoleColor.Green;
                                    Console.WriteLine($"[{DateTime.Now:g}] ERROR: {errorDetail}");
                                }
                                Console.WriteLine($"[{DateTime.Now:g}] Se procesó exitosamente");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"[{DateTime.Now:g}] ERROR al procesar: {responsePago?.error}");
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"[{DateTime.Now:g}] ERROR : {ex.Message}");
                    }
                });
                return string.Empty;
            }
            catch (Exception ex)
            {
                return $"Error durante el procesamiento: {ex.Message}";
            }
        }
    }
}