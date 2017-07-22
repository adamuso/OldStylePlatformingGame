using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace OldStylePlatformingGame.GFX
{
    public class TileMask : Engine.Mask
    {
        public TileMask()
            : base(TileSet.TileSize, TileSet.TileSize)
        {

        }

        public static TileMask FromTexture(Texture2D texture, int sx, int sy)
        {
            TileMask mask = new TileMask();
            Color[] colors = new Color[texture.Width * texture.Height];
            texture.GetData<Color>(colors);

            for (int y = sy; y < sy + mask.height; y++)
                for (int x = sx; x < sx + mask.width; x++)
                {
                    if (colors[x + y * texture.Width] == Color.Black)
                    {
                        mask.mask[(x - sx) + (y - sy) * mask.width] = 1;
                    }
                }

            return mask;
        }
    }
}
