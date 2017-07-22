using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using OldStylePlatformingGame.Engine;

namespace OldStylePlatformingGame.GFX
{
    public class TextureManager
    {
        private Game1 game;

        public Texture2D tileset, tilesetmask;
        public TextureSource rabbit;
        public Texture2D rabbitmask;
        private Texture2D spaz;
        private TextureAnimation spazStand, spazWalk, spazSkid, spazSkidTrans,
                                 spazJumpUpTrans, spazJumpUp, spazFallDownTrans, spazFallDown,
                                 spazJumpSideTrans, spazJumpSide, spazFallSideTrans, spazFallSide,
                                 spazSpringJump, spazRunTrans, spazRun;

        public TextureManager(Game1 game)
        {
            this.game = game;
        }

        public void LoadContent()
        {
            tileset = game.Content.Load<Texture2D>("TS");
            tilesetmask = game.Content.Load<Texture2D>("TS_mask");
            rabbit = new TextureSource(game.Content.Load<Texture2D>("rabbitanim"), new Rectangle(0, 0, 32, 32));
            rabbitmask = game.Content.Load<Texture2D>("RabbitMask");

            spaz = game.Content.Load<Texture2D>("spaz");
            spazWalk = new TextureAnimation(spaz, 0, 0, 8, 50, 50);
            spazStand = new TextureAnimation(spaz, 5, 51, 5, 44, 50);
            
            spazSkidTrans = new TextureAnimation(spaz, 0, 101, 3, 57, 50);
            spazSkid = new TextureAnimation(spaz, 57 * 3, 101, 3, 57, 50);

            spazJumpUpTrans = new TextureAnimation(spaz, 0, 151, 3, 50, 52);
            spazJumpUp = new TextureAnimation(spaz, 50 * 3, 151, 3, 50, 52);
            spazFallDownTrans = new TextureAnimation(spaz, 50 * 6, 151, 3, 50, 52);
            spazFallDown = new TextureAnimation(spaz, 50 * 9, 151, 3, 50, 52);

            spazJumpSideTrans = new TextureAnimation(spaz, 0, 207, 3, 40, 43);
            spazJumpSide = new TextureAnimation(spaz, 40 * 3 , 207, 3, 40, 43);
            spazFallSideTrans = new TextureAnimation(spaz, 40 * 6, 207, 3, 40, 43);
            spazFallSide = new TextureAnimation(spaz, 40 * 9, 207, 3, 40, 43);

            spazSpringJump = new TextureAnimation(spaz, 0, 258, 8, 41, 50);

            spazRunTrans = new TextureAnimation(spaz, 0, 308, 8, 53, 40);
            spazRun = new TextureAnimation(spaz, 53 * 8, 308, 4, 53, 40);
        }

        public TextureAnimation SpazStanding { get { return spazStand; } }
        public TextureAnimation SpazWalking { get { return spazWalk; } }
        public TextureAnimation SpazSkiddingTransition { get { return spazSkidTrans; } }
        public TextureAnimation SpazSkidding { get { return spazSkid; } }
        public TextureAnimation SpazJumpingUpTransition { get { return spazJumpUpTrans; } }
        public TextureAnimation SpazJumpingUp { get { return spazJumpUp; } }
        public TextureAnimation SpazFallingDownTransition { get { return spazFallDownTrans; } }
        public TextureAnimation SpazFallingDown { get { return spazFallDown; } }
        public TextureAnimation SpazJumpingSidwaysTransition { get { return spazJumpSideTrans; } }
        public TextureAnimation SpazJumpingSideways { get { return spazJumpSide; } }
        public TextureAnimation SpazFallingSidewaysTransition { get { return spazFallSideTrans; } }
        public TextureAnimation SpazFallingSideways { get { return spazFallSide; } }
        public TextureAnimation SpazSpringJumping { get { return spazSpringJump; } }
        public TextureAnimation SpazRunningTransition { get { return spazRunTrans; } }
        public TextureAnimation SpazRunning { get { return spazRun; } }
    }
}
