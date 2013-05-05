using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GetThePresents
{
    public static class ExtraFactory
    {
        public static string CreateName(ContentManager content, int type)
        {
            switch (type)
            {
                case 1: return "FireBall";

                case 2: return "Life";

                case 3: return "Shorten";

                case 4: return "Widen";

                case 5: return "SpeedUp";

                case 6: return "SlowDown";

                default: return null;
            }
        }

        public static Texture2D CreateExtra(ContentManager content, String name)
        {
            switch (name)
            {
                case "FireBall": return content.Load<Texture2D>("FireBall");

                case "Life": return content.Load<Texture2D>("Life");

                case "Shorten": return content.Load<Texture2D>("Shorten");

                case "Widen": return content.Load<Texture2D>("Widen");

                case "SpeedUp": return content.Load<Texture2D>("SpeedUp");

                case "SlowDown": return content.Load<Texture2D>("SlowDown");

                default: return null;
            }
        }
    }
}
