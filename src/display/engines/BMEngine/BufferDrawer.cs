using Microsoft.Win32.SafeHandles;
namespace SCE
{
    public class BufferDrawer
    {
        private static readonly Lazy<BufferDrawer> _lazy = new(() => new());

        private readonly SafeFileHandle _handle;

        private BufferDrawer()
        {
            _handle = WinApi.CreateFileW("CONOUT$", AccessGeneric.Write, ShareMode.Write, 
                IntPtr.Zero, CreateMode.OpenAlways, 0, IntPtr.Zero);

            if (_handle.IsInvalid)
            {
                throw new Exception("Failed to resolve console buffer handle.");
            }
        }

        public static BufferDrawer Instance { get => _lazy.Value; }

        public bool WriteBuffer(CharInfo[] buf, Coord size, Coord pos, ref SmallRect rect)
        {
            return WinApi.WriteConsoleOutputW(_handle, buf, size, pos, ref rect);
        }
    }
}
