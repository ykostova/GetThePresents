using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GetThePresents
{
    public static class BlockFactory
    {
        /***********************************************************************************
        * 
        * A Method to create a block type based on the texture number within the level pattern
        * fed into the game.
        * 
        **********************************************************************************/
        public static Block CreateBlock(int texture, ContentManager content, Vector2 pos)
        {
            switch (texture)
            {
                case 1: return new OrdinaryBlock(content, pos, texture , 80);

                case 2: return new OrdinaryBlock(content, pos, texture , 70);

                case 3: return new OrdinaryBlock(content, pos, texture, 60);

                case 4: return new OrdinaryBlock(content, pos, texture, 50);

                case 5: return new OrdinaryBlock(content, pos, texture, 40);

                case 6: return new OrdinaryBlock(content, pos, texture, 30);

                case 7: return new RockBlock(content, pos, texture, 90);

                case 9: return new PresentBlock(content, pos, texture, 100);

                default: return null;
             }
         }
     }
}
