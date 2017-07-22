using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace OldStylePlatformingGame.Engine
{
    public class SpriteMask : Mask
    {
        protected Sprites.Sprite sprite;

        protected SpriteMask(Sprites.Sprite sprite)
            : base(sprite.DrawingArea.Width, sprite.DrawingArea.Height)
        {
            this.sprite = sprite;
        }

        public static SpriteMask FromTexture(Sprites.Sprite entity, Texture2D texture)
        {
            SpriteMask mask = new SpriteMask(entity);
            Color[] colors = new Color[mask.width * mask.height];
            texture.GetData<Color>(colors);

            for (int y = 0; y < mask.height; y++)
                for (int x = 0; x < mask.width; x++)
                {
                    if (colors[x + y * mask.width] == Color.Black)
                    {
                        mask.mask[x + y * mask.width] = 1;
                    }
                }

            return mask;
        }
    }


}
