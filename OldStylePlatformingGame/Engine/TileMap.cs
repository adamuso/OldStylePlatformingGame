using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace OldStylePlatformingGame.Engine
{
    public class TileMap : Storage.ISerializeable
    {
        private TileData[,] tiles;
        private int width, height;
        private GFX.TileSet set;

        public TileMap(GFX.TileSet set, int width, int height)
        {
            tiles = new TileData[width, height];

            this.set = set;
            this.width = width;
            this.height = height;

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    tiles[x, y] = new TileData(0);
        }

        public TileData GetTile(int x, int y)
        {
            if (x >= width || y >= height || x < 0 || y < 0)
                return TileData.Empty;

            return tiles[x, y];
        }

        public void SetTile(int x, int y, TileData data)
        {
            tiles[x, y] = data;
        }

        public void Draw(SpriteBatch sb, int xs, int ys, int xe, int ye, int mapX, int mapY)
        {
            for (int y = ys; y < ye; y++)
                for (int x = xs; x < xe; x++)
                {
                    int posx = mapX * width * GFX.TileSet.TileSize + x * GFX.TileSet.TileSize,
                        posy = mapY * height * GFX.TileSet.TileSize + y * GFX.TileSet.TileSize;

                    if (!GetTile(x, y).IsEmpty)
                        sb.Draw(set[tiles[x, y].Data].Source.Parent,
                                new Rectangle(posx, posy, GFX.TileSet.TileSize, GFX.TileSet.TileSize),
                                set[tiles[x, y].Data].Source.Source, Color.White);
                }

        }

        public void Serialize(System.IO.Stream stream)
        {
            System.IO.BinaryWriter writer = new System.IO.BinaryWriter(stream);

            writer.Write(width);
            writer.Write(height);

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    tiles[x, y].Serialize(stream);
        }

        public static TileMap Deserialize(System.IO.Stream stream, GFX.TileSet set)
        {
            System.IO.BinaryReader reader = new System.IO.BinaryReader(stream);

            int width = reader.ReadInt32(),
                height = reader.ReadInt32();

            TileMap map = new TileMap(set, width, height);

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                    map.tiles[x, y] = TileData.Deserialize(stream, x, y);

            return map;
        }

        public TileData this[int x, int y] { set { SetTile(x, y, value); } get { return GetTile(x, y); } }

        public int Width { get { return width; } }
        public int Height { get { return height; } }
    }
}
