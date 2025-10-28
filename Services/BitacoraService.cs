using System.Text;


namespace com_next_tech_carga_batch_consola_aloha.Services
{
    public static class BitacoraService
    {
        public static string initLog(string rutaLog, string fileNameTmp)
        {
            String dateProcess = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
            string fileName = $"{rutaLog}RESP[{dateProcess}]_{fileNameTmp}.txt";
            using (FileStream fs = File.Create(fileName))
            {    
                Byte[] title = new UTF8Encoding(true).GetBytes($"{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}|INFO|Inicio de proceso \n");
                fs.Write(title, 0, title.Length);
            }
            return fileName;
        }
        public static string initLogError(string rutaLog, string fileNameTmp)
        {
            String dateProcess = DateTime.Now.ToString("dd_MM_yyyy_HH_mm_ss");
            string fileName = $"{rutaLog}ERROR[{dateProcess}]_{fileNameTmp}.txt";
            using (FileStream fs = File.Create(fileName))
            {   
                Byte[] title = new UTF8Encoding(true).GetBytes($"{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}|INFO|Inicio de proceso de errores\n");
                fs.Write(title, 0, title.Length);
            }
            return fileName;
        }

        public static void writeLog(string filename, string tipo, string evento)
        {
            using (StreamWriter sw = File.AppendText(filename))
            {
                sw.WriteLine($"{DateTime.Now.ToString("dd-MM-yyyy HH:mm:ss")}|{tipo}|{evento}");
            }
        }
    }
}
