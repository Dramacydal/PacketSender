using System;
using System.Runtime.InteropServices;

namespace PacketSender
{
    [StructLayout(LayoutKind.Sequential)]
    public struct CDataStore
    {
        public IntPtr vTable;
        public IntPtr Buffer;
        public int mBase;
        public int alloc;
        public int size;
        public int read;

        public CDataStore(IntPtr vTable, IntPtr buffer, int size)
        {
            this.vTable = vTable;
            this.Buffer = buffer;
            this.mBase = 0;
            this.alloc = -1;
            this.size = size;
            this.read = 0;
        }
    }
}
