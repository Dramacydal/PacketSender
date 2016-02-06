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
        //private static int vTable = 0xD62178 - 0x400000;
        //private static int send2 = 0x4069B1 - 0x400000;
        // 20994
        private const int vTable = 0xEC1EC0 - 0x400000;
        private const int send2 = 0x4A2C07 - 0x400000;

        public PacketSender(MemoryHandler m)
        {
            this.m = m;
        }

        public void Send(Packet packet)
        {
            var bytes = packet.ToArray();
            var pPacket = m.AllocateBytes(bytes);

            var dataStore = new CDataStore(m.Process.MainModule.BaseAddress.Add(vTable), pPacket, bytes.Length);
            var pDataStore = m.Allocate<CDataStore>(dataStore);

            m.Call(m.Process.MainModule.BaseAddress.Add(send2),
                MagicConvention.StdCall,
                pDataStore);

            m.FreeMemory(pDataStore);
            m.FreeMemory(pPacket);
        }
    }
}
