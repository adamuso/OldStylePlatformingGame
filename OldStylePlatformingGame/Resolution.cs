using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace OldStylePlatformingGame
{
    public class Resolution
    {
        private int sceneWidth, sceneHeight;
        private int resWidth, resHeight;
        private GraphicsDeviceManager graphics;
        private Matrix projection;

        private Resolution(GraphicsDeviceManager graphics)
        {
            sceneWidth = 1280;
            sceneHeight = 720;
            resWidth = 1024;
            resHeight = 768;
            this.graphics = graphics;
        }

        private Resolution(GraphicsDeviceManager graphics, int resWidth, int resHeight)
            : this(graphics)
        {
            this.resWidth = resWidth;
            this.resHeight = resHeight;
            this.sceneWidth = resWidth > maxSceneWidth ? maxSceneWidth : resWidth;
            this.sceneHeight = resHeight > maxSceneHeight ? maxSceneHeight : resHeight;
            this.graphics = graphics;
        }

        public void Apply()
        {
            graphics.PreferredBackBufferWidth = resWidth;
            graphics.PreferredBackBufferHeight = resHeight;
            graphics.IsFullScreen = fullscreen;
            graphics.ApplyChanges();

            int rw = resWidth, rh = resHeight;

            if (Fullscreen)
            {
                rw = graphics.GraphicsDevice.DisplayMode.Width;
                rh = graphics.GraphicsDevice.DisplayMode.Height;
            }

            float scalex = rw / (float)sceneWidth,
                  scaley = rh / (float)sceneHeight,
                  aspect = resWidth / (float)resHeight;

            if (keepAspect)
            {
                float scale = scalex;

                if (scalex > scaley)
                    scale = scaley;
                else
                    scale = scalex;

                projection = Matrix.CreateScale(scale, scale, 1f);
            }
            else
            {
                projection = Matrix.CreateScale(scalex, scaley, 1f);
            }
        }

        public void UpdateViewport()
        {
            if (keepAspect && !fullscreen)
            {
                float scalex = resWidth / (float)sceneWidth,
                      scaley = resHeight / (float)sceneHeight;

                float scale = scalex;

                if (scalex > scaley)
                    scale = scaley;
                else
                    scale = scalex;

                float spacex = resWidth - scale * sceneWidth,
                      spacey = resHeight - scale * sceneHeight;

                graphics.GraphicsDevice.Viewport = new Viewport((int)(spacex / 2f), (int)(spacey / 2f), (int)(sceneWidth * scale), (int)(sceneHeight * scale));
            }
        }

        public int SceneWidth { get { return sceneWidth; } }
        public int SceneHeight { get { return sceneHeight; } }
        public Matrix Projection { get { return projection; } }

        // static

        public static void Initialize(GraphicsDeviceManager g, int maxSceneWidth, int maxSceneHeight)
        {
            Resolution.maxSceneWidth = maxSceneWidth;
            Resolution.maxSceneHeight = maxSceneHeight;

            Resolution.keepAspect = false;
            Resolution.fullscreen = false;

            resolutions = new List<Resolution>();

            foreach (DisplayMode d in g.GraphicsDevice.Adapter.SupportedDisplayModes)
            {
                resolutions.Add(new Resolution(g, d.Width, d.Height));
            }
        }

        public static Resolution GetResolution(int width, int height)
        {
            foreach(Resolution r in resolutions)
            {
                if(r.resWidth == width && r.resHeight == height)
                    return r;
            }

            throw new NotSupportedException("Resolution is not supported!");
        }

        private static List<Resolution> resolutions;
        private static int maxSceneWidth, maxSceneHeight;
        private static bool keepAspect, fullscreen;

        public static bool Fullscreen { get { return fullscreen; } set { fullscreen = value; } }
        public static bool KeepAspect { get { return keepAspect; } set { keepAspect = value; } }
        public static List<Resolution> Resolutions { get { return resolutions; } }
    }
}
