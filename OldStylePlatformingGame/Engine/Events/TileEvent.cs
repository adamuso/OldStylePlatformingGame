using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OldStylePlatformingGame.Engine.Events
{
    public class TileEvent : Storage.ISerializeable
    {
        private ExtendedTileData data;
        private EventInfo info;
        private List<object> parameters;

        public TileEvent(EventInfo info)
        {
            this.data = null;
            this.info = info;
            parameters = new List<object>(info.Parameters.Count);

            for (int i = 0; i < info.Parameters.Count; i++)
                parameters.Add(null);
        }

        public TileEvent(EventInfo info, ExtendedTileData data)
        {
            this.data = data;
            this.info = info;
            parameters = new List<object>(info.Parameters.Count);

            for (int i = 0; i < info.Parameters.Count; i++)
                parameters.Add(null);
        }

        public void SetParameter(int num, object value)
        {
            if (num < 0 || num >= parameters.Count)
                throw new ArgumentException("Invalid parameter index!");

            if(value.GetType() != info.Parameters[num])
                throw new ArgumentException("Invalid parameter type!");

            parameters[num] = value;
        }

        public void SetParameters(params object[] values)
        {
            for (int i = 0; i < values.Length; i++)
                SetParameter(i, values[i]);
        }

        public void Serialize(System.IO.Stream stream)
        {
            System.IO.BinaryWriter writer = new System.IO.BinaryWriter(stream);

            writer.Write(info.ID);

            for (int i = 0; i < parameters.Count; i++)
                Storage.SerialUtility.WriteAs(stream, parameters[i], info.Parameters[i]);
        }

        public static TileEvent Deserialize(System.IO.Stream stream)
        {
            System.IO.BinaryReader reader = new System.IO.BinaryReader(stream);

            int id = reader.ReadInt32();
            EventInfo info = EventInfo.Manager.GetByID(id);
            object[] parameters = new object[info.Parameters.Count];

            for (int i = 0; i < parameters.Length; i++)
                parameters[i] = Storage.SerialUtility.ReadAs(stream, info.Parameters[i]);

            TileEvent ev = new TileEvent(info);
            ev.SetParameters(parameters);

            return ev;
        }

        public int ParametersCount { get { return parameters.Count; } }
        public EventInfo Info { get { return info; } }
        public ExtendedTileData TileData { get { return data; } set { data = value; } }
    }
}
