using SkiaSharp;

namespace App.Graphics;

public static class ImageFormatHelper
{
    public static string GetImageFormatFromStream(Stream stream, bool returnExtension = false)
    {
        using var skStream = new SKManagedStream(stream);

        using var codec = SKCodec.Create(skStream);

        var format = codec.EncodedFormat.ToString();

        return returnExtension ? format switch
        {
            "Jpeg" => ".jpg",
            "Gif" => ".gif",
            "Bmp" => ".bmp",
            "Wbmp" => ".wbmp",
            "Ico" => ".ico",
            "Adng" => ".dng",
            "Heif" => ".heif",
            "Tiff" => ".tiff",
            "Png" => ".png",
            "Webp" => ".webp",
            "Avif" => ".avif",
            _ => ".jpg"
        } : format;
    }
}