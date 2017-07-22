using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace OldStylePlatformingGame.GFX
{
    public class TileSet
    {
        private Tile[] tiles;
        private int sw, sh, gTileSize;
        private Texture2D texture;

        public TileSet(Texture2D texture, Texture2D maskTexture, int graphicsTileSize)
        {
            this.sw = texture.Width / graphicsTileSize;
            this.sh = texture.Height / graphicsTileSize;

            tiles = new Tile[sw * sh];

            this.gTileSize = graphicsTileSize;
            this.texture = texture;

            for (int y = 0; y < sh; y++)
                for (int x = 0; x < sw; x++)
                    tiles[x + y * sw] = new Tile(new TextureSource(texture, new Rectangle(x * gTileSize, y * gTileSize, gTileSize, gTileSize)),
                                                 TileMask.FromTexture(maskTexture, x * TileSize, y * TileSize));
        }

        public Tile this[int index] { get { return tiles[index]; } }

        public int Width { get { return sw; } }
        public int Height { get { return sh; } }
        public int GraphicsTileSize { get { return gTileSize; } }

        public static int TileSize { get { return 32; } }
    }
}
