
namespace com_next_tech_carga_batch_consola_aloha.Services
{
    public static class ExportReport
    {
        public static void ExportPDF(string report, string uuid, string fileName, Config filePath, string transaccion)
        {
            byte[] pdfBytes = Convert.FromBase64String(report);
            File.WriteAllBytes($"{filePath.reports}_{fileName}_{uuid}_{transaccion}.pdf", pdfBytes);
        }

        public static void ExportXML(string report, string uuid, string fileName, Config filePath, string transaccion)
        {
            byte[] xmlBytes = Convert.FromBase64String(report);
            File.WriteAllBytes($"{filePath.reports}_{fileName}_{uuid}_{transaccion}.xml", xmlBytes);
        }
    }
}