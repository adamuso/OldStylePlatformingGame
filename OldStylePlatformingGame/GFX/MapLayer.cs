using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OldStylePlatformingGame.Engine;

namespace OldStylePlatformingGame.GFX
{
    public class MapLayer : Engine.Storage.ISerializeable
    {
        private Level level;
        private float xspeed, yspeed;
        private bool widthWrap, heightWrap;
        private Matrix extraTransform;
        private float autoX, autoY;
        private TileMap map;
        private float scrollX, scrollY;

        public MapLayer(Level level, TileMap map)
        {
            this.level = level;
            this.xspeed = 1f;
            this.yspeed = 1f;
            this.widthWrap = false;
            this.heightWrap = false;
            this.extraTransform = Matrix.Identity;
            this.autoX = 0f;
            this.autoY = 0f;
            this.map = map;
            this.scrollX = 0f;
            this.scrollY = 0f;
        }

        public void Draw(SpriteBatch sb)
        {
            sb.Begin(SpriteSortMode.Immediate, BlendState.NonPremultiplied, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, level.Game.Camera.Project(new Vector2(xspeed, yspeed)) * Matrix.CreateTranslation(scrollX, scrollY, 0));

            float mpixW = map.Width * TileSet.TileSize,
                  mpixH = map.Height * TileSet.TileSize,
                  projX = level.Game.Camera.Position.X * xspeed - scrollX,
                  projY = level.Game.Camera.Position.Y * yspeed - scrollY;

            int xs = (int)(projX / GFX.TileSet.TileSize),
                ys = (int)(projY / GFX.TileSet.TileSize), 
                xe = (int)((level.Game.Camera.Position.X * xspeed + level.Game.Camera.Bounds.Width) / GFX.TileSet.TileSize) + 1, 
                ye =  (int)((level.Game.Camera.Position.Y * yspeed + level.Game.Camera.Bounds.Height) / GFX.TileSet.TileSize) + 1;

            int startX = (int)(projX / mpixW) - 1, 
                startY = (int)(projY / mpixH) - 1,
                endX = (int)((projX + level.Game.Camera.Bounds.Width) / mpixW) + 1, 
                endY = (int)((projY + level.Game.Camera.Bounds.Height) / mpixH) + 1;

            if (widthWrap && heightWrap)
            {
                for (int x = startX; x < endX; x++)
                    for (int y = startY; y < endY; y++)
                        map.Draw(sb, 0, 0, map.Width, map.Height, x, y);
            }
            else if (widthWrap)
            {
                for (int x = startX; x < endX; x++)
                    map.Draw(sb, 0, 0, map.Width, map.Height, x, 0);
            }
            else if (heightWrap)
            {
                for (int y = startY; y < endY; y++)
                    map.Draw(sb, 0, 0, map.Width, map.Height, 0, y);
            }
            else
                map.Draw(sb, xs, ys, xe, ye, 0, 0);


            sb.End();
        }

        public void Update(GameTime gt)
        {
            if(AutoScroll)
            {
                scrollX += autoX;

                if (Math.Abs(scrollX) > map.Width * TileSet.TileSize)
                    scrollX -= (scrollX / Math.Abs(scrollX)) * map.Width * TileSet.TileSize;

                scrollY += autoY;

                if (Math.Abs(scrollY) > map.Height * TileSet.TileSize)
                    scrollX -= (scrollX / Math.Abs(scrollX)) * map.Width * TileSet.TileSize;
            }
        }

        public void Serialize(System.IO.Stream stream)
        {
            System.IO.BinaryWriter writer = new System.IO.BinaryWriter(stream);

            writer.Write(xspeed);
            writer.Write(yspeed);
            writer.Write(autoX);
            writer.Write(autoY);
            writer.Write(widthWrap);
            writer.Write(heightWrap);

            map.Serialize(stream);
        }

        public static MapLayer Deserialize(System.IO.Stream stream, Level level, TileSet set)
        {
            System.IO.BinaryReader reader = new System.IO.BinaryReader(stream);

            float xs = reader.ReadSingle(),
                  ys = reader.ReadSingle(),
                  ax = reader.ReadSingle(),
                  ay = reader.ReadSingle();

            bool ww = reader.ReadBoolean(),
                 hw = reader.ReadBoolean();

            TileMap map = TileMap.Deserialize(stream, set);

            return new MapLayer(level, map) { xspeed = xs, yspeed = ys, autoX = ax, autoY = ay, widthWrap = ww, heightWrap = hw };
        }

        private bool AutoScroll { get { return autoX != 0f || autoY != 0f; } }

        public float XSpeed { get { return xspeed; } set { xspeed = value; } }
        public float YSpeed { get { return yspeed; } set { yspeed = value; } }
        public float XAutoSpeed { get { return autoX; } set { autoX = value; } }
        public float YAutoSpeed { get { return autoY; } set { autoY = value; } }
        public bool WidthWrap { get { return widthWrap; } set { widthWrap = value; } }
        public bool HeightWrap { get { return heightWrap; } set { heightWrap = value; } }
        public TileMap Map { get { return map; } }
    }
}
