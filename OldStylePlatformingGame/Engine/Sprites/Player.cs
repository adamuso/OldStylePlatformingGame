using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OldStylePlatformingGame.Engine.Sprites
{
    public class Player : MovingSprite
    {
        private GFX.AnimationController animController;

        public Player(Level level)
            : base(level)
        {
            animController = new GFX.AnimationController(Program.Instance.TextureManager.SpazStanding);
            animController.Start(100, GFX.AnimationLoop.Loop);

            source = animController;
            drawingArea = new Rectangle(0, 0, 40, 50);
            position = new Vector2(0, 428);
            collisionArea = new Rectangle(0, 0, 40, 50);
            mask = SpriteMask.FromTexture(this, Program.Instance.TextureManager.rabbitmask);
        }

        bool d = false;
        bool onground = false;
        bool wj = false;
        bool slide = false;
        byte dbljump = 0;
        float dbljumpvel = 0f;

        public override void Update(GameTime gt)
        {
            base.Update(gt);

            animController.Update(gt);

            if (Keyboard.GetState().IsKeyDown(Keys.Right))
            {
                if (onground)
                    if (Math.Abs(velocity.X) < 5f)
                        animController.ChangeAnimationAndStart(Program.Instance.TextureManager.SpazWalking, 100, GFX.AnimationLoop.Loop);
                    else if (Math.Abs(velocity.X) < 6f)
                        animController.ChangeAnimationAndStart(Program.Instance.TextureManager.SpazRunningTransition, 50, GFX.AnimationLoop.Loop);
                    else
                        animController.ChangeAnimationAndStart(Program.Instance.TextureManager.SpazRunning, 50, GFX.AnimationLoop.Loop);

                spriteEffect = SpriteEffects.None;
                velocity += new Vector2(0.5f * (Keyboard.GetState().IsKeyDown(Keys.LeftShift) ? 2.5f : 1f), 0);
                slide = true;
            }

            if (Keyboard.GetState().IsKeyDown(Keys.Left))
            {
                if (onground)
                    if (Math.Abs(velocity.X) < 5f)
                        animController.ChangeAnimationAndStart(Program.Instance.TextureManager.SpazWalking, 100, GFX.AnimationLoop.Loop);
                    else if (Math.Abs(velocity.X) < 6f)
                        animController.ChangeAnimationAndStart(Program.Instance.TextureManager.SpazRunningTransition, 50, GFX.AnimationLoop.Loop);
                    else
                        animController.ChangeAnimationAndStart(Program.Instance.TextureManager.SpazRunning, 25, GFX.AnimationLoop.Loop);

                spriteEffect = SpriteEffects.FlipHorizontally;
                velocity += new Vector2(-0.5f * (Keyboard.GetState().IsKeyDown(Keys.LeftShift) ? 2.5f : 1f), 0);
                slide = true;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.Right) && Keyboard.GetState().IsKeyUp(Keys.Left) && Math.Abs(velocity.X) > 0.1f && slide && onground)
            {
                slide = false;
                animController.ChangeAnimationAndStart(Program.Instance.TextureManager.SpazSkidding, 80, GFX.AnimationLoop.Loop, Program.Instance.TextureManager.SpazSkiddingTransition);
            }

            if (Math.Abs(velocity.X) < 0.1f && onground)
                animController.ChangeAnimationAndStart(Program.Instance.TextureManager.SpazStanding, 100, GFX.AnimationLoop.Loop);

            if (velocity.Y < 0 && Math.Abs(velocity.X) < 3f && !onground)
                if (animController.Animation != Program.Instance.TextureManager.SpazSpringJumping || animController.HasEnded)
                    animController.ChangeAnimationAndStart(Program.Instance.TextureManager.SpazJumpingUp, 80, GFX.AnimationLoop.Loop, Program.Instance.TextureManager.SpazJumpingUpTransition);

            if (velocity.Y > -0.5f && Math.Abs(velocity.X) < 3f && !onground)
                if (animController.Animation != Program.Instance.TextureManager.SpazSpringJumping || animController.HasEnded)
                    animController.ChangeAnimationAndStart(Program.Instance.TextureManager.SpazFallingDown, 80, GFX.AnimationLoop.Loop, Program.Instance.TextureManager.SpazFallingDownTransition);

            if (velocity.Y <= -2f && Math.Abs(velocity.X) > 3f && !onground)
                animController.ChangeAnimationAndStart(Program.Instance.TextureManager.SpazJumpingSideways, 100, GFX.AnimationLoop.None, Program.Instance.TextureManager.SpazJumpingSidwaysTransition);

            if (velocity.Y > -2f && Math.Abs(velocity.X) > 3f && !onground)
                animController.ChangeAnimationAndStart(Program.Instance.TextureManager.SpazFallingSideways, 100, GFX.AnimationLoop.Loop, Program.Instance.TextureManager.SpazFallingSidewaysTransition);

            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && !d)
            {
                if (dbljump == 2 || dbljump == 5)
                    dbljump = 0;

                if (velocity.Y > 0)
                    velocity.Y = 0f;

                if (!wj)
                { 
                    velocity.Y -= 10f;

                    //if (velocity.Y <= -11.5f - Math.Abs(velocity.X) / 10f)
                    //{
                    //    velocity.Y = -11.5f - Math.Abs(velocity.X) / 10f;
  
                        d = true;

                        if (dbljump == 0)
                            dbljump = 1;
                    //}
                }
            }
            else if (onground || wj)
            {
                d = false;
                wj = false;

                dbljump = 0;
            }
            else if (!onground)
            {
                d = true;

                if (dbljump == 0)
                    dbljump = 1;
            }

            if (Keyboard.GetState().IsKeyUp(Keys.LeftControl))
                if (dbljump == 1)
                    dbljump = 2;

            if (Keyboard.GetState().IsKeyDown(Keys.LeftControl) && (dbljump == 2 || dbljump == 3 || dbljump == 4))
            {
                if (dbljump == 2 && Velocity.Y > -4f)
                    dbljump = 3;
                else if(dbljump != 3 && dbljump != 4 && Velocity.Y <= -4f)
                    dbljump = 1;

                if (dbljump == 3 || dbljump == 4)
                {
                    animController.ChangeAnimationAndStart(Program.Instance.TextureManager.SpazSpringJumping, 40, GFX.AnimationLoop.None, 200);

                    //TODO: DblJump bug when jumping up into tiles and the double jump, stick to the tile

                    if(velocity.Y == 0 && dbljump == 4)
                    {
                        dbljump = 5;
                        dbljumpvel = 0f;
                    }

                    dbljump = 4;

                    if (velocity.Y > 0)
                        velocity.Y = 0f;

                     velocity.Y -= 4f;

                    if ((velocity.Y < -7f - Math.Abs(velocity.X) / 10f))
                    {
                        dbljump = 5;
                        dbljumpvel = 0f;
                    }
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            base.Draw(sb);

            sb.DrawString(Program.Instance.DefaultFont, "vX: " + velocity.X.ToString("0.00") + ", vY: " + velocity.Y.ToString("0.00") + ", dY: " + dbljumpvel.ToString("0.00"), Vector2.Zero, Color.White);
            sb.DrawString(Program.Instance.DefaultFont, "G: " + onground + ", D: " + dbljump, position - new Vector2(0, 10), Color.White, 0f, Vector2.Zero, 0.4f, SpriteEffects.None, 0f);
            sb.DrawString(Program.Instance.DefaultFont, "" + Program.Instance.fr.AverageFramesPerSecond, position - new Vector2(0, 20), Color.White);
        }

        protected override void StepHorizontal(int mov)
        {
            //wj = false;

            //if (Keyboard.GetState().IsKeyDown(Keys.LeftShift))
            //{
            //    int highY, lowY;

            //    Move(mov * 0.1f, 0);

            //    level.CollisionManager.CheckHorizontalMove(this, out highY, out lowY);

            //    if (highY < 3 && highY > 0) // collision with low ground, you can walk up
            //    {
            //        Move(0, -1);

            //        int vhY, vlY;

            //        level.CollisionManager.CheckVerticalMove(this, out vhY, out vlY);

            //        if (vhY != 0)
            //            bufferedPosition.Y = position.Y;
            //    }

            //    if (highY >= 3) // collision with high ground, your character need to stop
            //    {
            //        bufferedPosition.X = position.X;
            //        if(!onground)
            //        velocity.Y = 0.2f;
            //        wj = true;
            //    }
            //}
            //else
                
            base.StepHorizontal(mov);
        }

        protected override bool StepVertical(int mov, int vel)
        {
            onground = base.StepVertical(mov, vel);

            //onground = Velocity.Y >= -0.1f;

            //if (onground)
                onground = base.CheckIfOnGround(3) || onground;

            return onground;
        }
    }

    public class FrameCounter
    {
        public FrameCounter()
        {
        }

        public long TotalFrames { get; private set; }
        public float TotalSeconds { get; private set; }
        public float AverageFramesPerSecond { get; private set; }
        public float CurrentFramesPerSecond { get; private set; }

        public const int MAXIMUM_SAMPLES = 100;

        private Queue<float> _sampleBuffer = new Queue<float>();

        public bool Update(float deltaTime)
        {
            CurrentFramesPerSecond = 1.0f / deltaTime;

            _sampleBuffer.Enqueue(CurrentFramesPerSecond);

            if (_sampleBuffer.Count > MAXIMUM_SAMPLES)
            {
                _sampleBuffer.Dequeue();
                AverageFramesPerSecond = _sampleBuffer.Average(i => i);
            }
            else
            {
                AverageFramesPerSecond = CurrentFramesPerSecond;
            }

            TotalFrames++;
            TotalSeconds += deltaTime;
            return true;
        }
    }
}
