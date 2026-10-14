namespace ClipboardImageConverter;

public partial class Form1 : Form
{
    private readonly ImageConversionService conversionService = new();
    private readonly CancellationTokenSource lifetime = new();
    private ClipboardMonitor? clipboardMonitor;
    private bool processing;

    public Form1()
    {
        InitializeComponent();
    }

    protected override void OnShown(EventArgs e)
    {
        base.OnShown(e);
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
