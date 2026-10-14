using System.ComponentModel;
using System.Runtime.InteropServices;

namespace ClipboardImageConverter;

internal sealed class ClipboardMonitor : NativeWindow, IDisposable
{
    private const int ClipboardUpdate = 0x031D;
    private string? lastText;

    public event Action<string>? TextChanged;

    public ClipboardMonitor()
    {
        // Janela invisível que recebe mensagens na thread da interface.
        CreateHandle(new CreateParams { Parent = new IntPtr(-3) });
        if (!AddClipboardFormatListener(Handle))
        {
            int error = Marshal.GetLastWin32Error();
            DestroyHandle();
            throw new Win32Exception(error);
        }
    }

    protected override void WndProc(ref Message m)
    {
        if (m.Msg == ClipboardUpdate)
        {
            string? text;
            try
            {
                text = Clipboard.ContainsText() ? Clipboard.GetText().Trim() : null;
            }
            catch (ExternalException)
            {
                // Outro aplicativo pode estar usando o Clipboard neste instante.
                base.WndProc(ref m);
                return;
            }
            if (!string.Equals(text, lastText, StringComparison.Ordinal))
            {
                lastText = text;
                if (!string.IsNullOrEmpty(text))
                    TextChanged?.Invoke(text);
            }
        }
        base.WndProc(ref m);
    }

    public void Dispose()
    {
        if (Handle == IntPtr.Zero)
            return;
        RemoveClipboardFormatListener(Handle);
        DestroyHandle();
    }

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool AddClipboardFormatListener(IntPtr hwnd);

    [DllImport("user32.dll", SetLastError = true)]
    [return: MarshalAs(UnmanagedType.Bool)]
    private static extern bool RemoveClipboardFormatListener(IntPtr hwnd);
}
