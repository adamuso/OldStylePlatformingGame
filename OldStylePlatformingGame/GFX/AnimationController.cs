using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace OldStylePlatformingGame.GFX
{
    public class AnimationController : TextureSource
    {
        private TextureAnimation animation, transition;
        private TextureAnimation current;
        private int cFrame;
        private int msSpeed, msTime, minTime;
        private AnimationLoop loop;
        private bool end, ping;

        public AnimationController(TextureAnimation animation)
            : base(animation.Parent, Rectangle.Empty)
        {
            this.current = animation;
            this.cFrame = 0;
            this.end = true;
        }

        public void Start(int msSpeed, AnimationLoop loop)
        {
            this.msSpeed = msSpeed;
            this.loop = loop;
            this.cFrame = 0;
            this.end = false;
        }

        public void ChangeAnimationAndStart(TextureAnimation animation, int msSpeed, AnimationLoop loop)
        {
            if (this.current != animation && this.animation != animation && minTime <= 0)
            {
                this.transition = null;
                this.animation = null;
                this.current = animation;
                this.minTime = 0;
                Start(msSpeed, loop);

                Console.WriteLine("Anim: " + animation.GetHashCode());
            }
        }

        public void ChangeAnimationAndStart(TextureAnimation animation, int msSpeed, AnimationLoop loop, int minTime)
        {
            if (this.current != animation && this.animation != animation)
            {
                this.transition = null;
                this.animation = null;
                this.current = animation;
                this.minTime = minTime;
                Start(msSpeed, loop);

                Console.WriteLine("Anim: " + animation.GetHashCode());
            }
        }

        public void ChangeAnimationAndStart(TextureAnimation animation, int msSpeed, AnimationLoop loop, TextureAnimation transition)
        {
            if (this.animation != animation && minTime <= 0)
            {
                this.transition = transition;
                this.animation = animation;
                this.current = transition;
                this.minTime = 0;
                Start(msSpeed, loop);

                Console.WriteLine("Anim: " + animation.GetHashCode());
            }
        }

        public void Update(GameTime gt)
        {
            if (!end)
            {
                if (minTime > 0)
                    minTime -= gt.ElapsedGameTime.Milliseconds;

                msTime += gt.ElapsedGameTime.Milliseconds;

                if (msTime >= msSpeed)
                {
                    cFrame += ping ? -1 : 1;

                    if (cFrame < 0)
                    {
                        ping = !ping;
                        cFrame = 0;
                    }

                    if (cFrame >= current.FrameCount)
                    {
                        if (loop == AnimationLoop.None || current == transition)
                        {
                            end = true;
                            cFrame = current.FrameCount - 1;
                        }
                        else if (loop == AnimationLoop.Loop)
                            cFrame = 0;
                        else if (loop == AnimationLoop.Pingpong)
                        {
                            cFrame += ping ? 1 : -1;
                            ping = !ping;
                        }
                    }

                    msTime -= msSpeed;
                }
            }
            
            if(end)
            {
                if (transition != null)
                {
                    this.current = animation;
                    Start(msSpeed, loop);
                    transition = null;
                }
            }
        }

        public void Stop()
        {
            end = true;
        }

        public TextureAnimation Animation { get { return current; } set { current = value; } }
        public bool HasEnded { get { return end; } }

        public override Rectangle Source
        {
            get
            {
                return current[cFrame];
            }
        }
    }
}
