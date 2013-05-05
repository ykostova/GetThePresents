using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GetThePresents
{
    class RockBlock: Block
    {
        private int hitTimes = 0;

        /***********************************************************************************
         * 
         * A Constructor for the RockBlock class inheriting the base constructor (Block)
         * 
         **********************************************************************************/
        public RockBlock(ContentManager content, Vector2 pos, int textureNumber, int benefit) : base(content, pos, textureNumber, benefit)
        {
        }

        /***********************************************************************************
        * 
        * A Method to override the parent's UpdatePosition - dealing with updating the position of the object 
        * thus the logic of its behaviour. 
        * 
        **********************************************************************************/
        public override void UpdatePosition()
        {
            if (isVisible)
            {
                if (!isHit)
                {
                    size.X = (int)position.X;
                    size.Y = (int)position.Y;
                }
                else
                {
                    if (piecesPosition[3].X > 0 && piecesPosition[3].X < 800 && piecesPosition[3].Y > 0 && piecesPosition[3].Y < 600)
                    {
                        piecesPosition[0].X -= 1;
                        piecesPosition[0].Y += 3;
                        piecesPosition[1].X += 1;
                        piecesPosition[1].Y += 3;
                        piecesPosition[2].X -= 3;
                        piecesPosition[2].Y += 5;
                        piecesPosition[3].X += 3;
                        piecesPosition[3].Y += 5;
                    }
                    else  isVisible = false;
                 }
            }
        }

        /***********************************************************************************
         * 
         * A Method to override the parent's DestroyBlock - dealing with the behaviour of 
         * the block object when it is to be destroyed.
         * 
         **********************************************************************************/
        public override void DestroyBlock(ContentManager content)
        {
            if (hitTimes == 0)
            {
                isHit = false;
                canCollide = true;
                isVisible = true;
                SetTexture(8, content);
                hitTimes++;
            }
            else
            {
                isHit = true;
                isVisible = true;
                canCollide = false;
            }
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
                if (!isHit)
                {
                    batch.Draw(texture, position, Color.White);
                }
                else
                {
                    foreach (Vector2 entry in piecesPosition)
                    {
                        batch.Draw(pieceTexture, entry, Color.White);
                    }
                }
            }
        }

        /***********************************************************************************
         * 
         * A Method to override the parent's GetPoints method - dealing with the return of the points number.
         * Due to this block being a rock on hit returns part of its full points and when fully destroyed -
         * the other part.
         * 
         **********************************************************************************/
        public override int GetPoints()
        {
            if (isHit)
            {
                return 25;
            }
            else 
            {
                return 75;
            }
        }
    }
}
