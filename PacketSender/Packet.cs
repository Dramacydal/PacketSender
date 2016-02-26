
namespace PacketSender
{
    public class Packet : PacketStream
    {
        public Packet(ushort opcode)
            : base()
        {
            WriteInt32(0);
            WriteUInt16(opcode);
        }

        public Packet(Opcodes opcode)
            : base()
        {
            WriteInt32(0);
            WriteUInt16((ushort)opcode);
        }
    }
}
