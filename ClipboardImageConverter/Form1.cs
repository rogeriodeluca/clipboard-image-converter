namespace ClipboardImageConverter;

public partial class Form1 : Form
{
    private readonly SettingsService settingsService = new();
    private readonly ImageConversionService conversionService;
    private readonly CancellationTokenSource lifetime = new();
    private ClipboardMonitor? clipboardMonitor;
    private bool processing;

    public Form1()
    {
        conversionService = new ImageConversionService(settingsService);
        InitializeComponent();
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
        try
        {
            settingsService.Load();
            destinationFolderValue.Text = settingsService.DestinationFolder;
        }
        catch (Exception ex)
        {
            destinationFolderValue.Text = "Pasta indisponível. Escolha outra pasta.";
            MessageBox.Show(this, $"Não foi possível acessar a pasta de destino. {ex.Message}",
                "Pasta de destino", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
        try
        {
            clipboardMonitor = new ClipboardMonitor();
            clipboardMonitor.TextChanged += OnClipboardTextChanged;
            statusValue.Text = "Monitorando área de transferência...";
        }
        catch (System.ComponentModel.Win32Exception ex)
        {
            statusValue.Text = "Erro ao iniciar monitoramento.";
            resultValue.Text = ex.Message;
        }
    }

    private void OnChangeDestinationFolder(object? sender, EventArgs e)
    {
        using var dialog = new FolderBrowserDialog
        {
            Description = "Escolha a pasta para salvar as imagens",
            UseDescriptionForTitle = true,
            InitialDirectory = Directory.Exists(destinationFolderValue.Text)
                ? destinationFolderValue.Text : string.Empty,
            ShowNewFolderButton = true
        };
        if (dialog.ShowDialog(this) != DialogResult.OK)
            return;

        try
        {
            settingsService.ChangeDestinationFolder(dialog.SelectedPath);
            destinationFolderValue.Text = settingsService.DestinationFolder;
        }
        catch (Exception ex)
        {
            MessageBox.Show(this,
                $"Não foi possível alterar ou salvar a pasta de destino. A pasta anterior foi mantida. {ex.Message}",
                "Pasta de destino", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }

    private async void OnClipboardTextChanged(string text)
    {
        if (processing || !ImageConversionService.TryGetHttpUrl(text, out Uri? url))
            return;
        processing = true;
        urlValue.Text = url!.AbsoluteUri;
        statusValue.Text = "URL detectada...";
        resultValue.Text = "Processando...";
        var progress = new Progress<string>(status =>
        {
            if (!IsDisposed && !lifetime.IsCancellationRequested && processing)
                statusValue.Text = status;
        });
        try
        {
            string? path = await conversionService.ConvertAsync(url, progress, lifetime.Token);
            if (lifetime.IsCancellationRequested) return;
            resultValue.Text = path is null
                ? "URL não corresponde a uma imagem."
                : "Imagem salva com sucesso.";
            if (path is not null) pathValue.Text = path;
        }
        catch (OperationCanceledException) when (lifetime.IsCancellationRequested)
        {
            // O formulário foi fechado.
        }
        catch (Exception ex)
        {
            if (!lifetime.IsCancellationRequested)
            {
                string message = ex is HttpRequestException or OperationCanceledException
                    ? "Erro ao baixar imagem."
                    : "Erro ao converter ou salvar imagem.";
                resultValue.Text = $"{message} {ex.Message}";
            }
        }
        finally
        {
            processing = false;
            if (!lifetime.IsCancellationRequested)
                statusValue.Text = "Monitorando área de transferência...";
        }
    }

    private void StopMonitoring()
    {
        lifetime.Cancel();
        if (clipboardMonitor is not null)
        {
            clipboardMonitor.TextChanged -= OnClipboardTextChanged;
            clipboardMonitor.Dispose();
            clipboardMonitor = null;
        }
    }
}
