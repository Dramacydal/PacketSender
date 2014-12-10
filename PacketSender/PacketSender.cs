using System;
using WhiteMagic;

namespace PacketSender
{
    public class PacketSender
    {
        private MemoryHandler m;

        // 17538
        //private static int vTable = 0x910720;
        //private static int send2 = 0x8F1E;
        // 19103
        //private static int vTable = 0xD61170 - 0x400000;
        //private static int send2 = 0x4069AD - 0x400000;
        // 19116
        //private static int vTable = 0xD62168 - 0x400000;
        //private static int send2 = 0x4069BD - 0x400000;
        // 19243
        private static int vTable = 0xD62178 - 0x400000;
        private static int send2 = 0x4069B1 - 0x400000;

        public PacketSender(MemoryHandler m)
        {
            this.m = m;
        }

        public void Send(Packet packet)
        {
            var bytes = packet.ToArray();
            var pPacket = m.AllocateBytes(bytes);

            var dataStore = new CDataStore(IntPtr.Add(m.Process.MainModule.BaseAddress, vTable), (IntPtr)pPacket, bytes.Length);
            var pDataStore = m.Allocate<CDataStore>(dataStore);

            m.Call((uint)(IntPtr.Add(m.Process.MainModule.BaseAddress, send2)),
                CallingConventionEx.StdCall,
                pDataStore);

            m.FreeMemory(pDataStore);
            m.FreeMemory(pPacket);
        }
    }
}
