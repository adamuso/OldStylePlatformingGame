using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace OldStylePlatformingGame.GFX
{
    public class TextureAnimation
    {
        private int sX, sY, frameCount, frameWidth, frameHeight, frameSwap;
        private Texture2D parent;

        public TextureAnimation(Texture2D parent, int startX, int startY, int fCount, int fw, int fh, int fSwap = -1)
        {
            sX = startX;
            sY = startY;
            frameCount = fCount;
            frameWidth = fw;
            frameHeight = fh;
            frameSwap = fSwap;
            this.parent = parent;
        }

        public static TextureAnimation FromTexture(Texture2D parent, int startX, int startY, int frameWidth, int frameHeight, int frameCount, int frameSwap = -1)
        {
            return new TextureAnimation(parent, startX, startY, frameCount, frameWidth, frameHeight, frameSwap);
        }

        public int FrameCount { get { return frameCount; } }
        public Texture2D Parent { get { return parent; } }

        public Rectangle this[int index]
        {
            get
            {
                if (index < frameCount)
                    if (frameSwap != -1)
                        return new Rectangle(sX + (index % frameSwap) * frameWidth, sY + (index / frameSwap) * frameHeight, frameWidth, frameHeight);
                    else
                        return new Rectangle(sX + index * frameWidth, sY, frameWidth, frameHeight);
                else
                    throw new IndexOutOfRangeException();
            }
        }
    }
}
