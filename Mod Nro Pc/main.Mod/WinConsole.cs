using System;
using System.Runtime.InteropServices;

namespace ModCak.main.Mod
{
    public static class WinConsole
    {
        [DllImport("kernel32.dll")]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetStdHandle(int nStdHandle);

        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern bool SetConsoleTitle(string lpConsoleTitle);


        [DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
        private static extern bool WriteConsoleW(
            IntPtr hConsoleOutput,
            string lpBuffer,
            uint nNumberOfCharsToWrite,
            out uint lpNumberOfCharsWritten,
            IntPtr lpReserved
        );

        private const int STD_OUTPUT_HANDLE = -11;
        private static IntPtr _handle = IntPtr.Zero;
        private static bool _initialized;

        public static bool IsInitialized => _initialized;

        public static bool Init()
        {
            if (_initialized)
                return true;

            if (!AllocConsole())
                return false;

            SetConsoleTitle("Debug Client Nro - Cuong Le");

            _handle = GetStdHandle(STD_OUTPUT_HANDLE);

            if (_handle == IntPtr.Zero || _handle == new IntPtr(-1))
                return false;

            _initialized = true;
            WriteLine("Debug Client Nro - Cuong Le");
            return true;
        }


        public static void WriteLine(string text)
        {
            if (!_initialized || _handle == IntPtr.Zero)
                return; 

            WriteConsoleW(
                _handle,
                text + Environment.NewLine,
                (uint)(text.Length + Environment.NewLine.Length),
                out _,
                IntPtr.Zero
            );
        }
    }
}
