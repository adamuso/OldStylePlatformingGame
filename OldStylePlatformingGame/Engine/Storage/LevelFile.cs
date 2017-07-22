using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OldStylePlatformingGame.Engine.Storage
{
    public class LevelFile
    {
        public static void SaveLevel(Level level, string output)
        {
            System.IO.FileStream file = new System.IO.FileStream(output, System.IO.FileMode.Create);
            System.IO.Compression.GZipStream zips = new System.IO.Compression.GZipStream(file, System.IO.Compression.CompressionMode.Compress);

            level.Serialize(zips);

            zips.Close();
        }

        public static Level LoadLevel(string input, Game1 game, GFX.TileSet set)
        {
            System.IO.FileStream file = new System.IO.FileStream(input, System.IO.FileMode.Open);
            System.IO.Compression.GZipStream zips = new System.IO.Compression.GZipStream(file, System.IO.Compression.CompressionMode.Decompress);

            Level level = Level.Deserialize(zips, game, set);

            zips.Close();

            return level;
        }
    }
}
