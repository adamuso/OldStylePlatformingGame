using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OldStylePlatformingGame.Engine.Storage
{
    public interface ISerializeable
    {
        void Serialize(System.IO.Stream stream);
    }
}
