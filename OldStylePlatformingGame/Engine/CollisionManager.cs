using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OldStylePlatformingGame.Engine
{
    public class CollisionManager
    {
        public CollisionManager()
        {

        }

        public bool CheckHorizontalMove(Sprites.MovingSprite sprite, out int highY, out int lowY)
        {
            Rectangle rect = sprite.CollisionArea;

            int left = rect.Left / GFX.TileSet.TileSize,
                right = rect.Right / GFX.TileSet.TileSize,
                top = rect.Top / GFX.TileSet.TileSize,
                bottom = rect.Bottom / GFX.TileSet.TileSize;

            highY = 0;
            lowY = 0;

            bool result = false;

            for (int y = top; y <= bottom; y++)
                for (int x = left; x <= right; x++)
                {
                    if (!sprite.Level.GetTile(x, y).IsEmpty)
                    {
                        int hY, lY;

                        if (Mask.IntersectsWithTile(sprite, sprite.Level, x, y, out hY, out lY))
                        {
                            highY += hY;
                            lowY += lY;

                            result = true;
                        }
                    }
                }

            return result;
        }

        public bool CheckVerticalMove(Sprites.MovingSprite sprite, out int highY, out int lowY)
        {
            Rectangle rect = sprite.CollisionArea;

            int left = rect.Left / GFX.TileSet.TileSize,
                right = rect.Right / GFX.TileSet.TileSize,
                top = rect.Top / GFX.TileSet.TileSize,
                bottom = rect.Bottom / GFX.TileSet.TileSize;

            bool result = false;

            highY = 0;
            lowY = 0;

            for (int x = left; x <= right; x++)
            {

                if (!sprite.Level.GetTile(x, top).IsEmpty)
                {
                    int hb = 0, lb = 0;

                    if (Mask.IntersectsWithTile(sprite, sprite.Level, x, top, out hb, out lb))
                    {
                        highY = Math.Max(highY, hb);
                        lowY = lb;
                        result = true;
                    }
                }

                if (!sprite.Level.GetTile(x, bottom).IsEmpty)
                {
                    int hb = 0, lb = 0;

                    if (Mask.IntersectsWithTile(sprite, sprite.Level, x, bottom, out hb, out lb))
                    {
                        highY = Math.Max(highY, hb);
                        lowY = lb;
                        result = true;
                    }
                }
            }

            return result;
        }
    }
}
