using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OldStylePlatformingGame.Engine.Events
{
    public class EventInfo
    {
        private int id;
        private bool multiple;
        private bool global;
        private List<Type> parameters;

        private EventInfo(int id)
        {
            this.id = id;
            this.multiple = false;
            this.global = false;
            this.parameters = new List<Type>();
        }

        public EventInfo AddParameter(Type parameter)
        {
            if (!parameter.IsValueType)
                throw new Exception("Parameter has to be a value type.");

            parameters.Add(parameter);
            return this;
        }

        public EventInfo SetMultiple(bool value)
        {
            this.multiple = value;
            return this;
        }

        public EventInfo SetGlobal(bool value)
        {
            this.multiple = value;
            return this;
        }

        public bool CanBeMulitple { get { return multiple; } }
        public bool IsGlobal { get { return global; } }
        public int ID { get { return id; } }

        public System.Collections.ObjectModel.ReadOnlyCollection<Type> Parameters { get { return parameters.AsReadOnly(); } }

        public static readonly EventInfo Warp                  = Manager.RegisterNewEvent().AddParameter(typeof(int)); // warp target id
        public static readonly EventInfo WarpTarget            = Manager.RegisterNewEvent().AddParameter(typeof(int)); // warp target id

        public class Manager
        {
            private static List<EventInfo> events;
            private static int lastid;

            public static EventInfo RegisterNewEvent()
            {
                if (events == null)
                    events = new List<EventInfo>();

                for (int i = 0; i < events.Count; i++)
                    while (events[i].id == lastid)
                        lastid++;

                EventInfo ev = new EventInfo(lastid);
                lastid++;

                events.Add(ev);

                return ev;
            }

            public static EventInfo RegisterNewEvent(int id)
            {
                if (events == null)
                    events = new List<EventInfo>();

                for (int i = 0; i < events.Count; i++)
                    if (events[i].id == id)
                        throw new Exception("Specified ID is already registered!");

                EventInfo ev = new EventInfo(id);

                events.Add(ev);

                return ev;
            }

            public static EventInfo GetByID(int id)
            {
                for (int i = 0; i < events.Count; i++)
                    if (events[i].id == id)
                        return events[i];

                return null;
            }

            public static System.Collections.ObjectModel.ReadOnlyCollection<EventInfo> Events { get { return events.AsReadOnly(); } }
        }
    }
}
