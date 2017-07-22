using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OldStylePlatformingGame.Engine
{
    public class ExtendedTileData : TileData
    {
        private int x, y;
        private List<Events.TileEvent> events;

        public ExtendedTileData(int data, int x, int y)
            : base(data)
        {
            this.x = x;
            this.y = y;
        }

        public void AddEvent(Events.TileEvent ev)
        {
            if (events == null)
                events = new List<Events.TileEvent>();

            if (!ev.Info.CanBeMulitple)
                for (int i = 0; i < events.Count; i++)
                    if (events[i].Info == ev.Info)
                        throw new Exception("This event cannot be added multiple times to one tile!");

            ev.TileData = this;
            events.Add(ev);
        }

        public void AddEvent(Events.TileEvent ev, params object[] parameters)
        {
            AddEvent(ev);
            ev.SetParameters(parameters);
        }

        public void AddEvents(params Events.TileEvent[] ev)
        {
            for (int i = 0; i < ev.Length; i++)
                AddEvent(ev[i]);
        }

        public override void Serialize(System.IO.Stream stream)
        {
            base.Serialize(stream);

            System.IO.BinaryWriter writer = new System.IO.BinaryWriter(stream);
            writer.Write(events.Count);

            for(int i = 0; i < events.Count; i++)
            {
                events[i].Serialize(stream);
            }
        }

        public static ExtendedTileData Deserialize(System.IO.Stream stream, int data, int x, int y)
        {
            System.IO.BinaryReader reader = new System.IO.BinaryReader(stream);
            int count = reader.ReadInt32();
            Events.TileEvent[] events = new Events.TileEvent[count];

            for (int i = 0; i < events.Length; i++)
                events[i] = Engine.Events.TileEvent.Deserialize(stream);

            ExtendedTileData exdata = new ExtendedTileData(data, x, y);
            exdata.AddEvents(events);

            return exdata;
        }

        public void RemoveEvent(int index)
        {
            if (events != null)
            {
                events.RemoveAt(index);

                if (events.Count == 0)
                    events = null;
            }
        }

        public int X { get { return x; } }
        public int Y { get { return y; } }
        public List<Events.TileEvent> Events { get { return events; } }
        public override bool IsExtended { get { return true; } }
    }
}
