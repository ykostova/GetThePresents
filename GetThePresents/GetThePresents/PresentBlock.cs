using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GetThePresents
{
    class PresentBlock: Block
    {
        /***********************************************************************************
        * 
        * A Constructor for the PresentBlock class inheriting the base constructor (Block)
        * 
        **********************************************************************************/
        public PresentBlock(ContentManager content, Vector2 pos, int textureNumber, int benefit) : base(content, pos, textureNumber, benefit)
        {
        }

        /***********************************************************************************
         * 
         * A Method to override the parent's DestroyBlock - dealing with the behaviour of 
         * the block object when it is to be destroyed.
         * 
         **********************************************************************************/
        public override void DestroyBlock(ContentManager content)
        {
            this.SetTexture(10, content);
            isHit = true;
            isVisible = true;
            canCollide = false;
        }

        /***********************************************************************************
         * 
         * A Method to override the parent's Draw method - dealing with drawing of the object 
         * in respect to its position and texture.
         * 
         **********************************************************************************/
        public override void Draw(SpriteBatch batch)
        {
            if (isVisible)
            {
                batch.Draw(texture, position, Color.White);
            }
        }

        /***********************************************************************************
        * 
        * A Method to override the parent's UpdatePosition - dealing with updating the position of the object 
        * thus the logic of its behaviour. 
        * 
        **********************************************************************************/
        public override void UpdatePosition()
        {
            if (isHit)
            {
                if (position.X < 800 && position.X > 0 && position.Y > 0 && position.Y < 600 && isVisible)
                {
                    if (position.X <= 400)
                    {
                        position.X -= 1;
                        position.Y += 2;
                    }
                    else
                    {
                        position.X += 1;
                        position.Y += 2;
                    }
                    size.X = (int)position.X;
                    size.Y = (int)position.Y;
                }
                else
                {
                    isVisible = false;
                    canCollide = false;
                }

            } 
        }
    }
}
