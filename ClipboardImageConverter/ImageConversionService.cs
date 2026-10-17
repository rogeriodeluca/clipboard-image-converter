using ImageMagick;

namespace ClipboardImageConverter;

internal sealed class ImageConversionService(SettingsService settingsService)
{
    private static readonly HttpClient Client = new() { Timeout = TimeSpan.FromSeconds(60) };

    public static bool TryGetHttpUrl(string text, out Uri? url) =>
        Uri.TryCreate(text, UriKind.Absolute, out url)
        && (url.Scheme == Uri.UriSchemeHttp || url.Scheme == Uri.UriSchemeHttps)
        && !string.IsNullOrWhiteSpace(url.Host);

    // null indica que a resposta não contém uma imagem decodificável.
    public async Task<string?> ConvertAsync(Uri url, IProgress<string> progress,
        CancellationToken cancellationToken)
    {
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(60));
        cancellationToken = timeout.Token;
        progress.Report("Baixando imagem...");
        using var response = await Client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead,
            cancellationToken);
        response.EnsureSuccessStatusCode();
        string? mediaType = response.Content.Headers.ContentType?.MediaType;
        // Sem tipo ou com octet-stream, a decodificação dos bytes decide.
        if (mediaType is not null
            && !mediaType.StartsWith("image/", StringComparison.OrdinalIgnoreCase)
            && !mediaType.Equals("application/octet-stream", StringComparison.OrdinalIgnoreCase))
            return null;

        byte[] data = await response.Content.ReadAsByteArrayAsync(cancellationToken);
        progress.Report("Convertendo para JPG...");
        return await Task.Run(() => ConvertAndSave(data, url, cancellationToken), cancellationToken);
    }

    private string? ConvertAndSave(byte[] data, Uri url, CancellationToken cancellationToken)
    {
        using var image = new MagickImage();
        try { image.Read(data); }
        catch (MagickException) { return null; }

        cancellationToken.ThrowIfCancellationRequested();
        image.AutoOrient();
        image.BackgroundColor = MagickColors.White;
        image.Alpha(AlphaAction.Remove);
        image.ColorSpace = ColorSpace.sRGB;
        image.Format = MagickFormat.Jpeg;
        image.Quality = 90;
        // Converte antes de criar o arquivo para evitar JPG vazio em caso de erro.
        byte[] jpeg = image.ToByteArray();
        cancellationToken.ThrowIfCancellationRequested();
        string destinationFolder = settingsService.DestinationFolder;
        Directory.CreateDirectory(destinationFolder);
        string name = GetFileName(url);
        for (int suffix = 0; ; suffix++)
        {
            string fileName = suffix == 0 ? $"{name}.jpg" : $"{name} ({suffix}).jpg";
            string path = Path.Combine(destinationFolder, fileName);
            FileStream file;
            try
            {
                // CreateNew impede sobrescrita, inclusive se outro programa criar o arquivo.
                file = new FileStream(path, FileMode.CreateNew, FileAccess.Write);
            }
            catch (IOException) when (File.Exists(path)) { continue; }
            try
            {
                using (file) { file.Write(jpeg); }
                return path;
            }
            catch
            {
                File.Delete(path);
                throw;
            }
        }
    }

    private static string GetFileName(Uri url)
    {
        string name = Uri.UnescapeDataString(url.AbsolutePath.Split('/').Last());
        foreach (char invalid in Path.GetInvalidFileNameChars())
            name = name.Replace(invalid, '_');
        name = Path.GetFileNameWithoutExtension(name).Trim().TrimEnd('.');
        if (string.IsNullOrWhiteSpace(name)) name = "imagem";
        string deviceName = name.Split('.')[0].ToUpperInvariant();
        if (deviceName is "CON" or "PRN" or "AUX" or "NUL"
            || (deviceName.Length == 4 && deviceName[3] is >= '1' and <= '9'
                && (deviceName.StartsWith("COM") || deviceName.StartsWith("LPT"))))
            name = "_" + name;
        // Reserva espaço para pasta, extensão e sufixos nos limites usuais do Windows.
        return name.Length > 150 ? name[..150].TrimEnd(' ', '.') : name;
    }
}
