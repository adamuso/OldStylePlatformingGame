using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using OldStylePlatformingGame.Engine.Sprites;

namespace OldStylePlatformingGame.Engine
{
    public class Level : Storage.ISerializeable
    {
        private List<Sprite> sprites;
        private CollisionManager collisionManager;
        private GFX.TileSet tileSet;
        private Game1 game;
        private Player player;
        private List<GFX.MapLayer> mapLayers;
        private List<Engine.Events.TileEvent> events;
        private Events.EventHandler eventHandler;

        public Level(Game1 game, GFX.TileSet set)
        {
            this.game = game;
            this.tileSet = set;
            game.Camera.Limits = new Rectangle(0, 0, 30 * 32, 200 * 32);
            sprites = new List<Sprite>();
            events = new List<Events.TileEvent>();
            eventHandler = new Events.EventHandler(this);
            mapLayers = new List<GFX.MapLayer>();

            player = new Player(this);
            sprites.Add(player);

            collisionManager = new CollisionManager();
        }

        public void DEBUG()
        {
            TileMap map = new TileMap(tileSet, 30, 200);

            mapLayers.Add(new GFX.MapLayer(this, map));

            for (int i = 0; i < 30; i++)
                map.SetTile(i, 15, new TileData(1));

            AddTileEvent(10, 14, Engine.Events.EventInfo.Warp, 0).TileData.Data = 10;
            AddTileEvent(10, 0, Engine.Events.EventInfo.WarpTarget, 0);

            for (int i = 9; i < 14; i++)
                map.SetTile(10, i, new TileData(2));

            map.SetTile(15, 15, new TileData(3));
            map.SetTile(16, 16, new TileData(3));
            map.SetTile(17, 17, new TileData(3));
            map.SetTile(18, 18, new TileData(3));
            map.SetTile(19, 19, new TileData(3));
            map.SetTile(20, 20, new TileData(3));
            map.SetTile(26, 15, new TileData(4));
            map.SetTile(25, 16, new TileData(4));
            map.SetTile(24, 17, new TileData(4));
            map.SetTile(23, 18, new TileData(4));
            map.SetTile(22, 19, new TileData(4));
            map.SetTile(21, 20, new TileData(4));

            TileMap snow = new TileMap(tileSet, 10, 10);
            snow[0, 1] = 9;
            snow[3, 5] = 9;
            snow[5, 2] = 9;
            snow[8, 7] = 9;
            snow[4, 9] = 9;
            snow[6, 4] = 9;

            mapLayers.Add(new GFX.MapLayer(this, snow) { XSpeed = 2f, YSpeed = 2f, WidthWrap = true, HeightWrap = true, XAutoSpeed = 3f, YAutoSpeed = 2f });
        }

        public Engine.Events.TileEvent AddTileEvent(int x, int y, Engine.Events.EventInfo eventInfo)
        {
            TileData data = GetTile(x, y);
            ExtendedTileData exdata = data.IsExtended ? data.Extended : data.Extend(x, y);
            Engine.Events.TileEvent ev = new Events.TileEvent(eventInfo, exdata);

            exdata.AddEvent(ev);
            events.Add(ev);

            SetTile(x, y, exdata);

            return ev;
        }

        public Engine.Events.TileEvent AddTileEvent(int x, int y, Engine.Events.EventInfo eventInfo, params object[] parameters)
        {
            Engine.Events.TileEvent ev = AddTileEvent(x, y, eventInfo);
            ev.SetParameters(parameters);
            return ev;
        }

        public void SetTile(int x, int y, TileData data)
        {
            if (GetTile(x, y).IsExtended)
                for (int i = 0; i < GetTile(x, y).Extended.Events.Count; i++)
                    events.Remove(GetTile(x, y).Extended.Events[i]);

            mapLayers[0].Map.SetTile(x, y, data);
        }

        public TileData GetTile(int x, int y)
        {
            return mapLayers[0].Map.GetTile(x, y);
        }

        public TileData GetTile(Vector2 position)
        {
            return mapLayers[0].Map.GetTile((int)(position.X / GFX.TileSet.TileSize), (int)(position.Y / GFX.TileSet.TileSize));
        }

        public void Draw(SpriteBatch sb)
        {
            for (int i = 0; i < mapLayers.Count; i++)
            {
                mapLayers[i].Draw(sb);

                if(i == (mapLayers.Count - 1) / 2)
                {
                    sb.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullCounterClockwise, null, game.Camera.Projection);
                    for (int j = 0; j < sprites.Count; j++)
                    {
                        sprites[j].Draw(sb);
                    }
                    sb.End();
                }
            }
        }

        public void Update(GameTime gt)
        {
            game.Camera.Follow(player);

            if(Microsoft.Xna.Framework.Input.Mouse.GetState().LeftButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                Vector2 pos = new Vector2(Microsoft.Xna.Framework.Input.Mouse.GetState().X, Microsoft.Xna.Framework.Input.Mouse.GetState().Y);
                pos = Vector2.Transform(pos, Matrix.Invert(game.Resolution.Projection) * Matrix.Invert(game.Camera.Projection));

                if (GetTile((int)pos.X / 32, (int)pos.Y / 32).Data != 1)
                    // AddTileEvent((int)pos.X / 32, (int)pos.Y / 32, Engine.Events.EventInfo.Warp, 0).TileData.Data = 10;
                    SetTile((int)pos.X / 32, (int)pos.Y / 32, new TileData(1));
            }

            if (Microsoft.Xna.Framework.Input.Mouse.GetState().RightButton == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
            {
                Vector2 pos = new Vector2(Microsoft.Xna.Framework.Input.Mouse.GetState().X, Microsoft.Xna.Framework.Input.Mouse.GetState().Y);
                pos = Vector2.Transform(pos, Matrix.Invert(game.Resolution.Projection) * Matrix.Invert(game.Camera.Projection));

                if (GetTile((int)pos.X / 32, (int)pos.Y / 32).Data != 0)
                    SetTile((int)pos.X / 32, (int)pos.Y / 32, TileData.Empty);
            }

            for (int i = 0; i < mapLayers.Count;i++)
            {
                mapLayers[i].Update(gt);
            }

            for (int i = 0; i < sprites.Count; i++)
            {
                sprites[i].Update(gt);
            }
        }

        public void Serialize(System.IO.Stream stream)
        {
            System.IO.BinaryWriter writer = new System.IO.BinaryWriter(stream);

            writer.Write(mapLayers.Count);

            for (int i = 0; i < mapLayers.Count; i++)
                mapLayers[i].Serialize(stream);
        }

        public static Level Deserialize(System.IO.Stream stream, Game1 game, GFX.TileSet set)
        {
            System.IO.BinaryReader reader = new System.IO.BinaryReader(stream);

            int count = reader.ReadInt32();

            Level level = new Level(game, set);

            for (int i = 0; i < count; i++)
                level.mapLayers.Add(GFX.MapLayer.Deserialize(stream, level, set));

            for (int y = 0; y < level.mapLayers[0].Map.Height; y++)
                for (int x = 0; x < level.mapLayers[0].Map.Width; x++)
                    if (level.GetTile(x, y).IsExtended)
                        if (level.GetTile(x, y).Extended.Events.Count > 0)
                            for (int i = 0; i < level.GetTile(x, y).Extended.Events.Count; i++)
                                level.events.Add(level.GetTile(x, y).Extended.Events[i]);

            return level;
        }

        public Events.EventHandler EventHandler { get { return eventHandler; } }
        public List<Engine.Events.TileEvent> Events { get { return events; } }
        public Game1 Game { get { return game; } }
        public CollisionManager CollisionManager { get { return collisionManager; } }
        public GFX.TileSet TileSet { get { return tileSet; } }
        public Sprite P { get { return sprites[0]; } }
    }
}
