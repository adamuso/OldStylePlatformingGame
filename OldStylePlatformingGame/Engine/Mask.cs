using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using OldStylePlatformingGame.GFX;

namespace OldStylePlatformingGame.Engine
{
    public abstract class Mask
    {
        protected byte[] mask;
        protected int width, height;

        public Mask(int width, int height)
        {
            mask = new byte[width * height];
            this.width = width;
            this.height = height;
        }

        public byte[] GetData()
        {
            return mask;
        }

        public virtual byte getByte(int index, bool flipped = false)
        {
            if (!flipped)
                return mask[index];

            // flip hotizontaly

            int y = index / width;
            int x = width - (index - y * width) - 1;

            return mask[y * width + x];
        }

        public static bool Intersects(ICollisionable collisionAble1, ICollisionable collisionAble2, bool coll1flipped = false, bool coll2flipped = false)
        {
            return IntersectPixels(collisionAble1.CollisionArea, collisionAble1.Mask, collisionAble2.CollisionArea, collisionAble2.Mask, coll1flipped, coll2flipped);
        }

        public static bool Intersects(ICollisionable collisionAble1, ICollisionable collisionAble2, bool coll1flipped, bool coll2flipped, out int highY, out int lowY)
        {
            return IntersectPixels(collisionAble1.CollisionArea, collisionAble1.Mask, collisionAble2.CollisionArea, collisionAble2.Mask, coll1flipped, coll2flipped, out highY, out lowY);
        }

        public static bool Intersects(Vector2 position1, Mask mask1, Vector2 position2, Mask mask2, bool mask1flipped, bool mask2flipepd)
        {
            return IntersectPixels(new Rectangle((int)position1.X, (int)position1.Y, mask1.width, mask1.height), mask1, new Rectangle((int)position2.X, (int)position2.Y, mask2.width, mask2.height), mask2, mask1flipped, mask2flipepd);
        }

        public static bool Intersects(Vector2 position1, Mask mask1, Vector2 position2, Mask mask2, bool mask1flipped, bool mask2flipped, out int highY, out int lowY)
        {
            return IntersectPixels(new Rectangle((int)position1.X, (int)position1.Y, mask1.width, mask1.height), mask1, new Rectangle((int)position2.X, (int)position2.Y, mask2.width, mask2.height), mask2, mask1flipped, mask2flipped, out highY, out lowY);
        }

        /// <summary>
        /// Check the intersection of two masks in specified rectangles
        /// (gets addition information about y position)
        /// </summary>
        /// <param name="rectangleA"></param>
        /// <param name="dataA"></param>
        /// <param name="rectangleB"></param>
        /// <param name="dataB"></param>
        /// <param name="dataAflipped"></param>
        /// <param name="dataBflipped"></param>
        /// <returns></returns>
        public static bool IntersectPixels(Rectangle rectangleA, Mask dataA, Rectangle rectangleB, Mask dataB, bool dataAflipped, bool dataBflipped, out int highY, out int lowY)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);
            bool result = false;
            int lastY = int.MinValue;

            highY = 0;
            lowY = 0;

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    byte byteA = dataA.getByte((x - rectangleA.Left) +
                                 (y - rectangleA.Top) * rectangleA.Width, dataAflipped);
                    byte byteB = dataB.getByte((x - rectangleB.Left) +
                                 (y - rectangleB.Top) * rectangleB.Width, dataBflipped);

                    if (byteA != 0 && byteB != 0)
                    {
                        //TODO: better height division calculation, this might not work in all cases bacause it assumes that mask is filled from the top to the bottom of image,
                        //      which in our case isn't true (maybe craete a unique point for every mask?)
                        if (y < rectangleA.Top + (int)(rectangleA.Height * (2f/3f)))
                        {
                            if (y != lastY)
                            {
                                highY++;
                                lastY = y;
                            }
                        }
                        else
                        {
                            if (y != lastY)
                            {
                                lowY++;
                                lastY = y;
                            }
                        }

                        result = true;
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Check the intersection of two masks in specified rectangles
        /// (use this for checks without additional information about y position, because it's faster)
        /// </summary>
        /// <param name="rectangleA"></param>
        /// <param name="dataA"></param>
        /// <param name="rectangleB"></param>
        /// <param name="dataB"></param>
        /// <param name="dataAflipped"></param>
        /// <param name="dataBflipped"></param>
        /// <returns></returns>
        public static bool IntersectPixels(Rectangle rectangleA, Mask dataA, Rectangle rectangleB, Mask dataB, bool dataAflipped, bool dataBflipped)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    byte byteA = dataA.getByte((x - rectangleA.Left) +
                     (y - rectangleA.Top) * rectangleA.Width, dataAflipped);
                    byte byteB = dataB.getByte((x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width, dataBflipped);

                    // If both pixels are not completely transparent,
                    if (byteA != 0 && byteB != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            return false;
        }
        public static bool IntersectsWithTile(Sprites.Sprite collisionAble1, Level level, int tileX, int tileY, out int highY, out int lowY)
        {
            Engine.TileData data = level.GetTile(tileX, tileY);
            Tile tile = level.TileSet[data.Data];
            Rectangle rect = new Rectangle(0, 0, TileSet.TileSize, TileSet.TileSize), coll = collisionAble1.CollisionArea;
            rect.Offset(tileX * TileSet.TileSize, tileY * TileSet.TileSize);

            return IntersectsWithTile(coll, collisionAble1.Mask, rect, tile.Mask, false, false, out highY, out lowY);
        }

        public static bool IntersectsWithTile(Rectangle spriteRect, Mask spriteMask, Rectangle tileRect, Mask tileMask, bool spriteFlipped, bool tileFlipped, out int highY, out int lowY)
        {
            return IntersectPixels(spriteRect, spriteMask, tileRect, tileMask, spriteFlipped, tileFlipped, out highY, out lowY);

            /*int top = Math.Max(spriteRect.Top, tileRect.Top);
            int bottom = Math.Min(spriteRect.Bottom, tileRect.Bottom);
            int left = Math.Max(spriteRect.Left, tileRect.Left);
            int right = Math.Min(spriteRect.Right, tileRect.Right);
            bool result = false;
            int lastY = int.MinValue;

            highY = 0;
            lowY = 0;

            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    byte byteA = spriteMask.getByte((x - spriteRect.Left) +
                         (y - spriteRect.Top) * spriteRect.Width, spriteFlipped);
                    byte byteB = tileMask.getByte((x - tileRect.Left) + 
                         (y - tileRect.Top) * tileRect.Width, tileFlipped);

                    if (byteA != 0 && byteB != 0)
                    {

                        //TODO: better height division calculation, this might not work in all cases bacause it assumes that mask is filled from the top to the bottom of image,
                        //      which in our case isn't true (maybe craete a unique point for every mask?)
                        if (y < spriteRect.Top + spriteRect.Height / 2)
                        {
                            if (y != lastY)
                            {
                                highY++;
                                lastY = y;
                            }
                        }
                        else
                        {
                            if (y != lastY)
                            {
                                lowY++;
                                lastY = y;
                            }
                        }

                        result = true;
                    }
                }
            }

            return result;*/
        }
    }
}
