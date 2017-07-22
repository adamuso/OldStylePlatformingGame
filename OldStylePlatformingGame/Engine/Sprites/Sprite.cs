using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using OldStylePlatformingGame.GFX;
using Microsoft.Xna.Framework.Graphics;

namespace OldStylePlatformingGame.Engine.Sprites
{
    public class Sprite
    {
        protected Vector2 position;
        protected TextureSource source;
        protected Rectangle collisionArea;
        protected Rectangle drawingArea;
        protected Level level;
        protected SpriteMask mask;
        protected SpriteEffects spriteEffect;

        public Sprite(Level level)
        {
            position = Vector2.Zero;
            source = null;
            this.level = level;
        }

        public virtual void Draw(SpriteBatch sb)
        {
            if(source != null)
                sb.Draw(source.Parent, DrawingArea, source.Source, Color.White, 0f, Vector2.Zero, spriteEffect, 0f);
        }

        public virtual void Update(GameTime gt)
        {

        }

        public virtual void OnCollisionWith(Sprite sprite)
        {

        }

        public virtual void OnEventEffect(Events.TileEvent ev)
        {
            level.EventHandler.HandleEvent(this, ev);
        }

        public virtual Vector2 Position { get { return position; } set { position = value; } }
        public virtual Rectangle CollisionArea { get { return new Rectangle((int)Position.X, (int)Position.Y, collisionArea.Width, collisionArea.Height); } }
        public Vector2 Center { get { return Position + new Vector2(drawingArea.Width / 2, drawingArea.Height / 2); } }
        public Rectangle DrawingArea { get { return new Rectangle((int)position.X, (int)position.Y, drawingArea.Width, drawingArea.Height); } }
        public Level Level { get { return level; } }
        public Mask Mask { get { return mask; } }
    }
}
