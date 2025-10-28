using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;
using com_next_tech_carga_batch_consola_aloha.Services;
public class CargaBatchService : ServiceBase
{
    private Thread workerThread;
    private ManualResetEvent eventThread = new ManualResetEvent(false);
    public CargaBatchService()
    {
        ServiceName = "CargaBatchService";
    }

    protected override void OnStart(string[] args)
    {
        workerThread = new Thread(() => WorkerThreadFunction());
        workerThread.IsBackground = true;
        workerThread.Start();
    }

    protected override void OnStop()
    {
        eventThread.Set();
        if (workerThread.Join(5000))
        {
            workerThread.Abort();
        }
    }

    private async Task WorkerThreadFunction()
    {
        while (!eventThread.WaitOne(0))
        {
            try
            {
                ProcessService process = new ProcessService();
                string result = await process.Process();
                if (!string.IsNullOrEmpty(result))
                    Console.WriteLine(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
            
            Thread.Sleep(5000); 
        }
    }
}