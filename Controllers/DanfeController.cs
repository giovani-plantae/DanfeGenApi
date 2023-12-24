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
        IFormFile? logoFile = form.Files["logo"];

        if (invoiceFile == null || invoiceFile.Length == 0)
        {
            HandleMissingInvoiceFile(context);
            return;
        }

        Stream invoice = GetFormFileAsStream(invoiceFile);

        Stream? companyLogo = null;
        if (logoFile != null && logoFile.Length > 0)
            companyLogo = new CompanyLogo(logoFile).GetStream();

        Danfe danfe = GenerateDanfeFromInvoice(invoice, companyLogo);
        MemoryStream pdf = GeneratePdfFromDanfe(danfe);

        await SendPdfResponse(context, pdf);
    }

    private static void HandleMissingInvoiceFile(HttpContext context)
    {
        context.Response.StatusCode = 422;
        context.Response.WriteAsync("O arquivo 'invoice' é obrigatório na requisição.");
    }

    private static Stream GetFormFileAsStream(IFormFile invoiceFile)
    {
        var reader = new StreamReader(invoiceFile.OpenReadStream());
        return reader.BaseStream;
    }

    private static Danfe GenerateDanfeFromInvoice(Stream invoice, Stream? companyLogo)
    {
        DanfeViewModel modelo = DanfeViewModelCreator.CriarDeArquivoXml(invoice);
        var danfe = new Danfe(modelo);

        if (companyLogo != null)
            danfe.AdicionarLogoImagem(companyLogo);

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
