﻿using System.Runtime.InteropServices;

namespace Trans2QuikNet
{
    public class Trans2QuikAPI : IDisposable
    {
        public string QuikPath { get; }

        public const string Lib = "TRANS2QUIK.DLL";

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern nint LoadLibrary(string lib);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern void FreeLibrary(nint module);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern nint GetProcAddress(nint module, string proc);

        private nint _libraryHandle = nint.Zero;
        private bool disposedValue;

        public IntPtr LibraryHandle => _libraryHandle;
        public Trans2QuikAPI(string path)
        {
            ArgumentException.ThrowIfNullOrWhiteSpace(path, nameof(path));

            QuikPath = path;

            _libraryHandle = LoadLibrary(Path.Combine(path, Lib));

            if (_libraryHandle == IntPtr.Zero)
            {
                throw new Exception("Could not load the dynamic library.");
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects)
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null

                if (_libraryHandle != nint.Zero)
                {
                    FreeLibrary(_libraryHandle);
                    _libraryHandle = nint.Zero;
                }

                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        ~Trans2QuikAPI()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: false);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public nint GetProcAddress(string name)
        {
            return GetProcAddress(_libraryHandle, name);
        }
    }
}
