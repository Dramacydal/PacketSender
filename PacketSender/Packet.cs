
namespace PacketSender
{
    public class Packet : PacketStream
    {
        public Packet(uint opcode)
            : base()
        {
            WriteUInt32(opcode);
        }

        public Packet(Opcodes opcode)
            : base()
        {
            WriteUInt32((uint)opcode);
        }
    }
}
