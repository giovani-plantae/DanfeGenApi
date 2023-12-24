using System.Drawing;
using System.Drawing.Imaging;

public class CompanyLogo
{
    private Stream resizedLogoStream;

    public CompanyLogo(IFormFile logo)
    {
        resizedLogoStream = ResizeImageToFill(logo.OpenReadStream());
    }

    public Stream GetStream()
    {
        resizedLogoStream.Seek(0, SeekOrigin.Begin);
        return resizedLogoStream;
    }

    public static Stream ResizeImageToFill(Stream image, int width = 900, int height = 180)
    {
        Image originalImage = Image.FromStream(image);

        int newWidth, newHeight;
        float aspectRatio = (float)originalImage.Width / originalImage.Height;

        if (aspectRatio > (float)width / height)
        {
            newWidth = width;
            newHeight = (int)(newWidth / aspectRatio);
        }
        else
        {
            newHeight = height;
            newWidth = (int)(newHeight * aspectRatio);
        }

        using (Bitmap resizedImage = new Bitmap(width, height))
        {
            using (Graphics g = Graphics.FromImage(resizedImage))
            {
                g.FillRectangle(Brushes.White, 0, 0, width, height);

                int xOffset = (width - newWidth) / 2;
                int yOffset = (height - newHeight) / 2;

                g.DrawImage(originalImage, xOffset, yOffset, newWidth, newHeight);

                return ConvertToJPEG(resizedImage);
            }
        }
    }

    private static Stream ConvertToJPEG(Image image)
    {
        const long jpegQuality = 40L;
        
        var encoderParams = new EncoderParameters(1);
        encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, jpegQuality);

        ImageCodecInfo codec = GetEncoderInfo("image/jpeg");

        MemoryStream output = new MemoryStream();
        image.Save(output, codec, encoderParams);
        output.Seek(0, SeekOrigin.Begin);

        return output;
    }

    private static ImageCodecInfo GetEncoderInfo(string mimeType)
    {
        ImageCodecInfo[] codecs = ImageCodecInfo.GetImageEncoders();

        foreach (ImageCodecInfo codec in codecs)
        {
            if (codec.MimeType == mimeType)
            {
                return codec;
            }
        }

        throw new NotSupportedException($"Codec {mimeType} not found.");
    }
}
