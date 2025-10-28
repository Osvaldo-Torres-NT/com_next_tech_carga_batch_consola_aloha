using Newtonsoft.Json;
using System.Reflection;

namespace com_next_tech_carga_batch_consola_aloha.Services
{
    public static class ConfigService
    {
        public static Config getConfig()
        {

            string strWorkPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            // Construir la ruta completa del archivo de configuración.
            string configFilePath = Path.Combine(strWorkPath, "config.json");

            // Leer el contenido del archivo de configuración.
            string jsonContent;
            using (StreamReader reader = new StreamReader(configFilePath))
            {
                jsonContent = reader.ReadToEnd();
            }

            // Deserializar el contenido JSON en un objeto Config.
            Config config = JsonConvert.DeserializeObject<Config>(jsonContent);

            return config;
        }
    }
}

