using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OldStylePlatformingGame.Engine.Sprites
{
    public class MovingSprite : Sprite
    {
        protected Vector2 velocity;
        protected Vector2 bufferedPosition;
        protected TileData lastTile;

        public MovingSprite(Level level)
            : base(level)
        {

        }

        public void ApplyPosition()
        {
            base.position = bufferedPosition;
        }

        public void Move(float x, float y)
        {
            bufferedPosition += new Vector2(x, y);
        }

        public override void Update(Microsoft.Xna.Framework.GameTime gt)
        {
            base.Update(gt);

            bufferedPosition = position;

            velocity += new Vector2(0, 0.32f);

            if (velocity.Y > 10f)
                velocity.Y = 10f;

            MoveHorizontal();
            ApplyPosition();

            bool onground = MoveVertical();
            ApplyPosition();

            //if (onground)
             //   velocity.Y = 0;
            
            velocity.X *= 0.85f;
            velocity.Y *= 0.995f;

            if (Math.Abs(velocity.X) < 0.001f)
                velocity.X = 0;
        }

        private void MoveHorizontal()
        {
            int vel = (int)(velocity.X * 10f);
            int usvel = Math.Abs(vel); 

            if (vel == 0)
                return;

            // Note: mov == 1 -> when moving right, mov == -1 when moving left
            int mov = vel / usvel; 

            for (int i = 0; i < usvel; i++)
            {
                StepHorizontal(mov);
            }
        }

        private bool MoveVertical()
        {
            int vel = (int)(velocity.Y * 10f);
            int usvel = Math.Abs(vel);

            if (vel == 0)
                return false;

            bool onground = false;

            // Note: mov == 1 -> when moving right, mov == -1 when moving left
            int mov = vel / usvel;

            for (int i = 0; i < usvel; i++)
            {
                onground = StepVertical(mov, vel);
            }

            return onground;
        }

        protected virtual void StepHorizontal(int mov)
        {
            //Note: move the sprite 1/10th of a x axis velocity
            Move(mov * 0.1f, 0);

            int highY, lowY;

            level.CollisionManager.CheckHorizontalMove(this, out highY, out lowY);

            // Note: slope movement detection
            
            //if (highY < 3 && highY > 0)
            if(lowY < 3 && lowY > 0)
            {
                //Note: moving up sprite the slope
                //Move(0, -highY);
                Move(0, -lowY);

                int vhY, vlY;

                //Note: check if sprite isn't in another tile after moving up
                level.CollisionManager.CheckVerticalMove(this, out vhY, out vlY);

                if (vhY != 0)
                    bufferedPosition.Y = position.Y;
            }
            // else if(highY >= 3)
            if (lowY >= 3 || highY != 0)
            {
                //Note: sprite encoutered obstacle -> restore last position, stop sprite form moving in x axis
                bufferedPosition.Y = position.Y;
                bufferedPosition.X = position.X;
                velocity.X = 0;
            }

            if(highY != 0)
                System.Diagnostics.Debug.Print("HighY: " + highY); 

            CheckEvents();
        }

        protected virtual bool StepVertical(int mov, int vel)
        {
            //Note: move the sprite 1/10th of a y axis velocity
            Move(0, mov * 0.1f);

            int highY, lowY;

            level.CollisionManager.CheckVerticalMove(this, out highY, out lowY);

            //Note: if there is ground under or above sprite stop it from moving
            if (lowY != 0 || highY != 0)    
            {
                bufferedPosition.Y = position.Y;
                velocity.Y = 0f;
            }

            if (lowY != 0 && vel < 0)
                System.Diagnostics.Debug.Print("lowY: " + lowY);

            CheckEvents();

            // if the ground is under you then the sprite is staying on ground
            
            //if (highY < 7 && highY > 0) 
            if(lowY < 7 && lowY > 0)
                return true;

            return false;
        }

        protected virtual bool CheckIfOnGround(int down)
        {
            bool ground = false;
            Vector2 last = bufferedPosition;

            for (int i = 0; i < down; i++)
            {
                Move(0, down / Math.Abs(down));

                int highY, lowY;

                level.CollisionManager.CheckVerticalMove(this, out highY, out lowY);

                if (lowY < 7 && lowY > 0) // if the ground is under you then it is ground
                {
                    ground = true;
                    break;
                }
            }

            bufferedPosition = last;

            return ground;
        }

        private void CheckEvents()
        {
            if(lastTile != level.GetTile(Center))
            {
                lastTile = level.GetTile(Center);

                if(lastTile.IsExtended)
                    for(int i = 0; i < lastTile.Extended.Events.Count; i++)
                        if(!lastTile.Extended.Events[i].Info.IsGlobal)
                            this.OnEventEffect(lastTile.Extended.Events[i]);
            }
        }

        public override Vector2 Position { get { return bufferedPosition; } set { bufferedPosition = value; } }
        public Vector2 Velocity { get { return velocity; } }
    }
}
