using DanfeSharp;
using DanfeSharp.Modelo;
using org.pdfclown.files;

public class DanfeController
{
    public static void MapEndpoints(WebApplication app)
    {
        app.MapPost("/danfe", DanfeHandler);
    }

    private static async Task DanfeHandler(HttpContext context)
    {
        IFormCollection form = await context.Request.ReadFormAsync();
        IFormFile? invoiceFile = form.Files["invoice"];

        if (invoiceFile == null || invoiceFile.Length == 0)
        {
            HandleMissingInvoiceFile(context);
            return;
        }

        string invoice = await ReadInvoiceFileContent(invoiceFile);

        Danfe danfe = GenerateDanfeFromInvoice(invoice);
        MemoryStream pdf = GeneratePdfFromDanfe(danfe);

        await SendPdfResponse(context, pdf);
    }

    private static void HandleMissingInvoiceFile(HttpContext context)
    {
        context.Response.StatusCode = 422;
        context.Response.WriteAsync("O arquivo 'invoice' é obrigatório na requisição.");
    }

    private static async Task<string> ReadInvoiceFileContent(IFormFile invoiceFile)
    {
        var reader = new StreamReader(invoiceFile.OpenReadStream());
        return await reader.ReadToEndAsync();
    }

    private static Danfe GenerateDanfeFromInvoice(string invoice)
    {
        DanfeViewModel modelo = DanfeViewModelCreator.CriarDeStringXml(invoice);
        var danfe = new Danfe(modelo);
        danfe.Gerar();
        return danfe;
    }

    private static MemoryStream GeneratePdfFromDanfe(Danfe danfe)
    {
        MemoryStream stream = new MemoryStream();
        danfe.GetFile().Save(stream, SerializationModeEnum.Incremental);
        return stream;
    }

    private static async Task SendPdfResponse(HttpContext context, MemoryStream pdf)
    {
        var response = context.Response;
        response.Headers.Append("Content-Type", "application/pdf");
        response.Headers.Append("Content-Disposition", "inline; filename=invoice.pdf");

        pdf.Seek(0, SeekOrigin.Begin);
        await pdf.CopyToAsync(response.Body);
    }
}
