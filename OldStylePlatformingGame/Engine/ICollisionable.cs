using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OldStylePlatformingGame.Engine
{
    public interface ICollisionable
    {
        Rectangle CollisionArea { get; }
        Mask Mask { get; }
        bool IsFlipped { get; }
    }
}
