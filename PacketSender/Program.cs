using System;
using System.Threading;
using WhiteMagic;

namespace PacketSender
{
    class Program
    {
        static void Main(string[] args)
        {
            var proc = Helpers.FindProcessByInternalName("world of warcraft");
            if (proc == null)
            {
                Console.WriteLine("Can't find process");
                return;
            }

            using (var m = new MemoryHandler(proc))
            {
                var sender = new PacketSender(m);

                for (var i = 0; i < 50000; ++i)
                {
                    using (var packet = new Packet(Opcodes.CMSG_QUEST_QUERY))
                    {
                        packet.WriteInt32(i);
                        packet.WritePackedUInt128(0, 0);

                        using (var suspender = m.Suspend())
                        {
                            sender.Send(packet);
                        }
                    }

                    Thread.Sleep(50);
                }
            }

            Console.ReadKey();
        }
    }
}
