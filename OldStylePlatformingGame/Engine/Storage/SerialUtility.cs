using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OldStylePlatformingGame.Engine.Storage
{
    public class SerialUtility
    {
        public static void WriteAs(System.IO.Stream stream, object value, Type type)
        {
            System.IO.BinaryWriter writer = new System.IO.BinaryWriter(stream);

            if (type == typeof(bool))
                writer.Write((bool)value);
            else if (type == typeof(char))
                writer.Write((char)value);
            else if (type == typeof(byte))
                writer.Write((byte)value);
            else if (type == typeof(short))
                writer.Write((short)value);
            else if (type == typeof(int))
                writer.Write((int)value);
            else if (type == typeof(long))
                writer.Write((long)value);
            else if (type == typeof(float))
                writer.Write((float)value);
            else if (type == typeof(double))
                writer.Write((double)value);
            else if (type == typeof(sbyte))
                writer.Write((sbyte)value);
            else if (type == typeof(ushort))
                writer.Write((ushort)value);
            else if (type == typeof(uint))
                writer.Write((uint)value);
            else if (type == typeof(ulong))
                writer.Write((ulong)value);
            else if (type == typeof(string))
                writer.Write((string)value);
            else
                throw new ArgumentException("Usupported type!");
        }

        public static object ReadAs(System.IO.Stream stream, Type type)
        {
            System.IO.BinaryReader reader = new System.IO.BinaryReader(stream);

            if (type == typeof(bool))
                return reader.ReadBoolean();
            else if (type == typeof(char))
                return reader.ReadChar();
            else if (type == typeof(byte))
                return reader.ReadByte();
            else if (type == typeof(short))
                return reader.ReadInt16();
            else if (type == typeof(int))
                return reader.ReadInt32();
            else if (type == typeof(long))
                return reader.ReadInt64();
            else if (type == typeof(float))
                return reader.ReadSingle();
            else if (type == typeof(double))
                return reader.ReadDouble();
            else if (type == typeof(sbyte))
                return reader.ReadSByte();
            else if (type == typeof(ushort))
                return reader.ReadUInt16();
            else if (type == typeof(uint))
                return reader.ReadUInt32();
            else if (type == typeof(ulong))
                return reader.ReadUInt64();
            else if (type == typeof(string))
                return reader.ReadString();

            throw new ArgumentException("Usupported type!");
        }
    }
}
