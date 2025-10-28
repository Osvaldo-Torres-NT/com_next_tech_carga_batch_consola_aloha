public class Request
{
    public string xml { get; set; }
    public string transaccion { get; set; } = Guid.NewGuid().ToString();
    public string nombre_archivo { get; set; }
}