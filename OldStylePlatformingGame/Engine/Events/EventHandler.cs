using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace OldStylePlatformingGame.Engine.Events
{
    public class EventHandler
    {
        private Level level;

        public EventHandler(Level level)
        {
            this.level = level;    
        }

        public void HandleEvent(Sprites.Sprite sprite, TileEvent ev)
        {
            OnHandleEvent(sprite, ev);
        }

        protected void OnHandleEvent(Sprites.Sprite sprite, TileEvent ev, bool handled = false)
        {
            if (handled)
                return;

            if (ev.Info.IsGlobal)
                return;

            if (ev.Info == EventInfo.Warp)
            {
                for (int i = 0; i < level.Events.Count; i++)
                    if (level.Events[i].Info == EventInfo.WarpTarget)
                        sprite.Position = new Vector2(level.Events[i].TileData.X * GFX.TileSet.TileSize, level.Events[i].TileData.Y * GFX.TileSet.TileSize);
            }
        }
    }
}
