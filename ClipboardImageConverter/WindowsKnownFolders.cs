using System.Runtime.InteropServices;

namespace ClipboardImageConverter;

internal static class WindowsKnownFolders
{
    public static string GetDownloadsFolder()
    {
        Guid downloadsFolderId = new("374DE290-123F-4565-9164-39C4925E467B");
        IntPtr pathPointer = IntPtr.Zero;
        try
        {
            // Flags 0 consultam o caminho atual, incluindo redirecionamentos.
            int result = SHGetKnownFolderPath(in downloadsFolderId, 0, IntPtr.Zero, out pathPointer);
            Marshal.ThrowExceptionForHR(result);
            string? path = Marshal.PtrToStringUni(pathPointer);
            if (string.IsNullOrWhiteSpace(path))
                throw new IOException("O Windows não retornou o caminho da pasta Downloads.");
            return path;
        }
        finally
        {
            Marshal.FreeCoTaskMem(pathPointer);
        }
    }

    [DllImport("shell32.dll", ExactSpelling = true)]
    private static extern int SHGetKnownFolderPath(in Guid folderId, uint flags,
        IntPtr token, out IntPtr path);
}
