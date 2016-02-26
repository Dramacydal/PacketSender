using System;
using System.Diagnostics;
using System.Threading;
using WhiteMagic;

namespace PacketSender
{
    class Program
    {
        static void Main(string[] args)
        {
            Process proc;
            try
            {
                proc = MagicHelpers.SelectProcess("world of warcraft");
            }
            catch (Exception e)
            {
                Console.WriteLine("Can't find process: \"{0}\"", e.Message);
                return;
            }

            using (var m = new MemoryHandler(proc))
            {
                var sender = new PacketSender(m);

                using (var packet = new Packet(Opcodes.CMSG_LOGOUT_REQUEST))
                {
                    using (var suspender = m.MakeSuspender())
                    {
                        sender.Send(packet);
                    }
                }

                /*for (var i = 0; i < 50000; ++i)
                {
                    using (var packet = new Packet(Opcodes.CMSG_QUEST_QUERY))
                    {
                        packet.WriteInt32(i);
                        packet.WritePackedUInt128(0, 0);

                        using (var suspender = m.MakeSuspender())
                        {
                            sender.Send(packet);
                        }
                    }

                    Thread.Sleep(50);
                }*/
            }

            //Console.ReadKey();
        }
    }
}
