using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OldStylePlatformingGame.Engine
{
    public class TileData : Storage.ISerializeable
    {
        private int data;

        public TileData(int data)
        {
            this.data = data;
        }

        public ExtendedTileData Extend(int x, int y)
        {
            return new ExtendedTileData(data, x, y);
        }

        public virtual void Serialize(System.IO.Stream stream)
        {
            System.IO.BinaryWriter writer = new System.IO.BinaryWriter(stream);

            int serialData = IsExtended ? (int)((uint)data | 0x80000000) : (int)(data & (~0x80000000));

            writer.Write(serialData);
        }

        public static TileData Deserialize(System.IO.Stream stream, int x, int y)
        {
            System.IO.BinaryReader reader = new System.IO.BinaryReader(stream);

            int serialData = reader.ReadInt32();

            if ((serialData & 0x80000000) == 0x80000000)
                return ExtendedTileData.Deserialize(stream, (int)(serialData & (~0x80000000)), x, y);

            return new TileData(serialData);
        }

        public static implicit operator TileData(int value)
        {
            return new TileData(value);
        }

        public virtual bool IsExtended { get { return false; } }
        public ExtendedTileData Extended { get { return IsExtended ? (ExtendedTileData)this : null; } }
        public int Data { get { return data; } set { data = value; } }
        public bool IsEmpty { get { return data == 0; } }
 
        public static TileData Empty { get { return new TileData(0); } }
    }
}
