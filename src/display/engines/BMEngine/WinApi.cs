using Microsoft.Win32.SafeHandles;
using System.Runtime.InteropServices;
namespace SCE
{
    // Made with help from from:
    // https://stackoverflow.com/questions/2754518/how-can-i-write-fast-colored-output-to-console

    #region FileEnums

    internal enum AccessGeneric : uint
    {
        All = 0x10000000,
        Execute = 0x20000000,
        Write = 0x40000000,
        Read = 0x80000000,
    }

    internal enum ShareMode : uint
    {
        None = 0x00000000,
        Delete = 0x00000004,
        Read = 0x00000001,
        Write = 0x00000002,
    }

    internal enum CreateMode : uint
    {
        CreateAlways = 2,
        CreateNew = 1,
        OpenAlways = 4,
        OpenExisting = 3,
        TruncateExisting = 5,
    }

    internal enum FileFlag : uint
    {
        Archive = 32,
        Encrypted = 16384,
        Hidden = 2,
        Normal = 128,
        Offline = 4096,
        Readonly = 1,
        System = 4,
        Temporary = 256,
    }

    internal enum FileAttribute : uint
    {
        BackupSemantics = 0x02000000,
        DeleteOnClose = 0x04000000,
        NoBuffering = 0x20000000,
        OpenNoRecall = 0x00100000,
        OpenReparsePoint = 0x00200000,
        Overlapped = 0x40000000,
        PosixSemantics = 0x01000000,
        RandomAccess = 0x10000000,
        SessionAware = 0x00800000,
        SequentialScan = 0x08000000,
        WriteThrough = 0x80000000,
    }

    #endregion

    #region Structs

    [StructLayout(LayoutKind.Explicit)]
    public struct CharUnion : IEquatable<CharUnion>
    {
        [FieldOffset(0)] public ushort UnicodeChar;
        [FieldOffset(0)] public byte ASCIIChar;

        public bool Equals(CharUnion other)
        {
            return UnicodeChar == other.UnicodeChar && ASCIIChar == other.ASCIIChar;
        }
    }

    [StructLayout(LayoutKind.Explicit)]
    public struct CharInfo : IEquatable<CharInfo>
    {
        [FieldOffset(0)] public CharUnion Char;
        [FieldOffset(2)] public short Attributes;

        public bool Equals(CharInfo other)
        {
            return Char.Equals(other.Char) && Attributes == other.Attributes;
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Coord
    {
        public short X;
        public short Y;

        public Coord(short x, short y)
        {
            X = x;
            Y = y;
        }

        public static Coord Zero { get; } = new(0, 0);
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct SmallRect
    {
        public short Left;
        public short Top;
        public short Right;
        public short Bottom;

        public SmallRect(short left, short top, short right, short bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }
    }

    #endregion

    internal static class WinApi
    {
        [DllImport("kernel32", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern SafeFileHandle CreateFileW(
            string lpFileName,
            AccessGeneric dwDesiredAccess,
            ShareMode dwShareMode,
            IntPtr securityAttributes,
            CreateMode dwCreationDisposition,
            uint dwFlagsAndAttributes,
            IntPtr hTemplateFile);

        [DllImport("kernel32", ExactSpelling = true, SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern bool WriteConsoleOutputW(
            SafeFileHandle hConsoleOutput,
            CharInfo[] lpBuffer,
            Coord dwBufferSize,
            Coord dwBufferCoord,
            ref SmallRect lpWriteRegion);
    }
}
