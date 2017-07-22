using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OldStylePlatformingGame.GFX
{
    public class Tile
    {
        private TextureSource source;
        private TileMask mask;

        public Tile(TextureSource source, TileMask mask)
        {
            this.source = source;
            this.mask = mask;
        }

        public TextureSource Source { get { return source; } }
        public TileMask Mask { get { return mask; } }
    }
}
