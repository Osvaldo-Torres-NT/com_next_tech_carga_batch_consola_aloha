using System.ServiceProcess;
using com_next_tech_carga_batch_consola_aloha;

// var builder = Host.CreateApplicationBuilder(args);
// builder.Services.AddHostedService<Worker>();

// var host = builder.Build();
// host.Run();

public class Program
{
    public static void Main(string[] args)
    {

        var service = new CargaBatchService();
        service.GetType().GetMethod("OnStart", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
            ?.Invoke(service, new object[] { args });

        Thread.Sleep(Timeout.Infinite); 

        ServiceBase.Run(new CargaBatchService());

    }
}
