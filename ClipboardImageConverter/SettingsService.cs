using System.Security;
using System.Text.Json;

namespace ClipboardImageConverter;

internal sealed class SettingsService
{
    private string? destinationFolder;
    private readonly Func<string> getDownloadsFolder;

    public SettingsService() : this(Path.Combine(
        Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "ClipboardImageConverter", "settings.json"), WindowsKnownFolders.GetDownloadsFolder)
    {
    }

    internal SettingsService(string settingsFilePath, Func<string> getDownloadsFolder)
    {
        SettingsFilePath = settingsFilePath;
        this.getDownloadsFolder = getDownloadsFolder;
    }

    public string SettingsFilePath { get; }

    public string DestinationFolder => Volatile.Read(ref destinationFolder)
        ?? throw new IOException("Nenhuma pasta de destino está disponível. Escolha outra pasta.");

    public void Load()
    {
        string? configuredFolder = null;
        try
        {
            var settings = JsonSerializer.Deserialize<AppSettings>(File.ReadAllText(SettingsFilePath));
            configuredFolder = PrepareFolder(settings?.DestinationFolder);
        }
        catch (Exception ex) when (IsFolderOrFileError(ex) || ex is JsonException)
        {
            // Configuração ausente, inválida ou inacessível: usar Downloads.
        }

        Volatile.Write(ref destinationFolder,
            configuredFolder ?? PrepareFolder(getDownloadsFolder()));
    }

    public void ChangeDestinationFolder(string folder)
    {
        string preparedFolder = PrepareFolder(folder);
        string settingsPath = SettingsFilePath;
        Directory.CreateDirectory(Path.GetDirectoryName(settingsPath)!);
        string temporaryPath = settingsPath + "." + Guid.NewGuid().ToString("N") + ".tmp";
        try
        {
            string json = JsonSerializer.Serialize(new AppSettings { DestinationFolder = preparedFolder },
                new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(temporaryPath, json);
            File.Move(temporaryPath, settingsPath, overwrite: true);
        }
        finally
        {
            try { File.Delete(temporaryPath); }
            catch (Exception ex) when (IsFolderOrFileError(ex)) { }
        }

        // Só aplicar a nova pasta depois de persistir a configuração com sucesso.
        Volatile.Write(ref destinationFolder, preparedFolder);
    }

    private static string PrepareFolder(string? folder)
    {
        if (string.IsNullOrWhiteSpace(folder) || !Path.IsPathFullyQualified(folder))
            throw new ArgumentException("A pasta de destino deve ser um caminho absoluto válido.");

        string fullPath = Path.GetFullPath(folder);
        Directory.CreateDirectory(fullPath);
        string probePath = Path.Combine(fullPath, ".clipboard-image-converter-" + Guid.NewGuid().ToString("N") + ".tmp");
        // Verifica gravação sem deixar arquivos na pasta selecionada.
        using (var probe = new FileStream(probePath, FileMode.CreateNew, FileAccess.Write,
                   FileShare.None, 1, FileOptions.DeleteOnClose))
        {
            probe.WriteByte(0);
        }
        return fullPath;
    }

    private static bool IsFolderOrFileError(Exception ex) =>
        ex is IOException or UnauthorizedAccessException or ArgumentException
            or NotSupportedException or SecurityException;
}
