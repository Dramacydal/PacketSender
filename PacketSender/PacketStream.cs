using System;
using System.IO;
using System.Text;

namespace PacketSender
{
    public class PacketStream : IDisposable
    {
        private BinaryWriter w = null;
        private MemoryStream m = null;

        private byte curbitval = 0;
        private byte bitPos = 8;

        public PacketStream()
        {
            m = new MemoryStream();
            w = new BinaryWriter(m);
        }

        public void Dispose()
        {
            m.Dispose();
            w.Dispose();
        }

        public byte[] ToArray()
        {
            return m.ToArray();
        }

        public void FlushBits()
        {
            if (bitPos == 8)
                return;

            WriteUInt8(curbitval);
            curbitval = 0;
            bitPos = 8;
        }

        public void WriteBit(bool set)
        {
            --bitPos;
            if (set)
                curbitval |= (byte)(1 << bitPos);

            if (bitPos == 0)
                FlushBits();
        }

        public void WriteBits(dynamic val, int count)
        {
            for (var i = count - 1; i >= 0; --i)
                WriteBit(((val >> i) & 1) != 0);
        }

        public void WriteInt64(long val)
        {
            w.Write(val);
        }

        public void WriteUInt64(ulong val)
        {
            w.Write(val);
        }

        public void WriteInt32(int val)
        {
            w.Write(val);
        }

        public void WriteUInt32(uint val)
        {
            w.Write(val);
        }

        public void WriteInt16(short val)
        {
            w.Write(val);
        }

        public void WriteUInt16(ushort val)
        {
            w.Write(val);
        }

        public void WriteUInt8(byte val)
        {
            w.Write(val);
        }

        public void WriteInt8(sbyte val)
        {
            w.Write(val);
        }

        public void WritePackedUInt128(ulong hiPart, ulong loPart)
        {
            var loMask = GetPackedUInt64Mask(loPart);
            var hiMask = GetPackedUInt64Mask(hiPart);

            WriteUInt8(loMask);
            WriteUInt8(hiMask);
            WritePackedUInt64(loPart, loMask);
            WritePackedUInt64(hiPart, hiMask);
        }

        private byte GetPackedUInt64Mask(ulong val)
        {
            byte mask = 0;
            for (var i = 0; i < 8; ++i)
                if (val >> i * 8 != 0)
                    mask |= (byte)(1 << i);

            return mask;
        }

        public void WritePackedUInt64(ulong val)
        {
            var mask = GetPackedUInt64Mask(val);
            WriteUInt8(mask);
            WritePackedUInt64(val, mask);
        }

        private void WritePackedUInt64(ulong val, byte mask)
        {
            for (var i = 0; i < 8; ++i)
                if ((mask & 1 << i) != 0)
                    WriteUInt8((byte)((val >> i * 8) & 0xFF));
        }

        public void WriteBytes(byte[] bytes)
        {
            w.Write(bytes);
        }

        public void WriteCString(string val, bool nullTerminated = true)
        {
            if (nullTerminated)
                val += '\0';

            w.Write(Encoding.ASCII.GetBytes(val));
        }

        public void WriteUTF8String(string val, bool nullTerminated = true)
        {
            if (nullTerminated)
                val += '\0';

            w.Write(Encoding.UTF8.GetBytes(val));
        }
    }
}
