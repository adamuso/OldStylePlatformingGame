using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OldStylePlatformingGame.Engine
{
    public class Camera2D
    {
        private Vector2 position;
        private Matrix projection;
        private Resolution resolution;
        private Rectangle limits;

        public Camera2D(Resolution resolution)
        {
            this.resolution = resolution;
            this.position = Vector2.Zero;
            this.projection = Matrix.Identity;
        }

        private void ReCalcProjection()
        {
            if (limits != Rectangle.Empty)
            {
                position = Vector2.Clamp(position, new Vector2(limits.X, limits.Y), new Vector2(limits.Width - resolution.SceneWidth, limits.Height - resolution.SceneHeight));
            }

            position = new Vector2((int)position.X, (int)position.Y);
            projection = Matrix.CreateTranslation(new Vector3(-position, 0f));
        }

        public Matrix Project(Vector2 parallax)
        {
            return Matrix.CreateTranslation(new Vector3(-position * parallax, 0f));
        }

        public void Follow(Sprites.Sprite sprite)
        {
            Position = sprite.Position + new Vector2(sprite.DrawingArea.Width, sprite.DrawingArea.Height) / 2 - new Vector2(resolution.SceneWidth, resolution.SceneHeight) / 2;
        }

        public Rectangle Limits { get { return limits; } set { limits = value; ReCalcProjection(); } }
        public Rectangle Bounds { get { return new Rectangle((int)position.X, (int)position.Y, resolution.SceneWidth, resolution.SceneHeight); } }
        public Vector2 Position { get { return position; } set { position = value; ReCalcProjection(); } }
        public Matrix Projection { get { return projection; } }
    }
}
