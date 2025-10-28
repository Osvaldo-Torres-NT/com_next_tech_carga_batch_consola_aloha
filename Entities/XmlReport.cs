public class XmlReport
{
    public bool exito { get; set; } = false;
    public string message { get; set; }
    public ResponseFacturacion response_facturacion { get; set; }
    public string detail { get; set; }
}

public class ResponseFacturacion
{
    public string xml { get; set; }
    public string pdf { get; set; }
    public string uuid { get; set; }
}