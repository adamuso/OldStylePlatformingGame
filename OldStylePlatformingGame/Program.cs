#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
#endregion

namespace OldStylePlatformingGame
{
#if WINDOWS || LINUX
    /// <summary>
    /// The main class.
    /// </summary>
    public static class Program
    {
        private static Game1 game;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            game = new Game1();
            game.Run();
        }

        public static Game1 Instance { get { return game; } }
    }
#endif
}
