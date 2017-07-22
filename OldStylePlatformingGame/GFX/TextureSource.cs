using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace OldStylePlatformingGame.GFX
{
    public class TextureSource
    {
        private Texture2D parent;
        private Rectangle source;

        public TextureSource(Texture2D parent, Rectangle source)
        {
            this.parent = parent;
            this.source = source;
        }

        public Texture2D Parent { get { return parent; } }
        public virtual Rectangle Source { get { return source; } }
    }
}
