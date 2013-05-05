using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GetThePresents
{
    public class Ball
    {
        private Vector2 position;
        private double direction;
        private Texture2D texture;
        private Texture2D fireTexture;
        private Rectangle size;
        private float speed;
        private float moveSpeed;
        private Vector2 resetPos;
        public bool isVisible;
        Random rand;
        public bool extraFire;

        /**********************************************************************
         * 
         * Constructor method to set up the initial properties of a Ball object
         * 
         **********************************************************************/
        public Ball(ContentManager content, Vector2 screenSize)
        {
            extraFire = false;
            moveSpeed = 6f;
            speed = 0;
            texture = content.Load<Texture2D>("OrdinaryBall");
            fireTexture = content.Load<Texture2D>("FireBall");
            direction = Math.PI;
            size = new Rectangle(0, 0, texture.Width, texture.Height);
            resetPos = new Vector2(screenSize.X / 2 - texture.Width / 2, screenSize.Y - 41);
            position = resetPos;
            rand = new Random();
            isVisible = true;
        }


        /**********************************************************************
         * 
         * Method to draw the ball if it is visible
         * 
         **********************************************************************/
        public void Draw(SpriteBatch batch)
        {
            if (isVisible)
            {
                if(!extraFire)batch.Draw(texture, position, Color.White);
                else batch.Draw(fireTexture, position, Color.White);
            }
        }

        /**********************************************************************
         * 
         * Method to update the position vector of the ball based on its current speed, position and direction 
         * 
         **********************************************************************/
        public void UpdatePosition()
        {
            size.X = (int)position.X;
            size.Y = (int)position.Y;
            position.X += speed * (float)Math.Sin(direction);
            position.Y += speed * (float)Math.Cos(direction);

            CheckWallHit();
        }

        /**********************************************************************
        * 
        * Method to stop the ball from movement - thus make it visible, give it speed of 0 and reset its position
        * 
        **********************************************************************/
        public void Stop()
        {
            isVisible = true;
            speed = 0;
            position = resetPos;
        }

        /**********************************************************************
        * 
        * Method to reset the position of the ball, give it direction straight up, initial speed and make it visible
        * 
        **********************************************************************/
        public void Reset()
        {
            direction = Math.PI;
            position = resetPos;
            isVisible = true;
            speed = moveSpeed;
        }

        /**********************************************************************
        * 
        * Method to check when a wall has been hit and correct ball's direction to bounce of the wall
        * 
        **********************************************************************/
        private void CheckWallHit()
        {
            while (direction > 2 * Math.PI) direction -= 2 * Math.PI;
            while (direction < 0) direction += 2 * Math.PI;
            if (position.Y <= 0)
            {
                position.Y += 2;
                direction = Math.PI - direction;
            }
            else if (position.X >= 800)
            {
                position.X -= 2;
                direction = 2 * Math.PI - direction;
            }
            else if( position.X <= 0)
            {
                position.X += 2;
                direction = 2 * Math.PI - direction;
            }
        }

        /**********************************************************************
        * 
        * Method to get the underlying rectangle of the ball object
        * 
        **********************************************************************/
        public Rectangle GetSize()
        {
            return size;
        }

        /**********************************************************************
        * 
        * Method to get the current vector position of the ball
        * 
        **********************************************************************/
        public Vector2 GetPosition()
        {
            return position;
        }


        /**********************************************************************
        * 
        * Method to get the current direction of the ball object
        * 
        **********************************************************************/
        public double GetDirection()
        {
            return direction;
        }

        /**********************************************************************
        * 
        * Method to get the speed of the ball object
        * 
        **********************************************************************/
        public float GetSpeed()
        {
            return speed;
        }

        /**********************************************************************
        * 
        * Method to set the speed of the ball object
        * 
        **********************************************************************/
        public void SetSpeed(int change)
        {
            speed += change;
        }

        /**********************************************************************
        * 
        * Method to set the direction of the ball object
        * 
        **********************************************************************/
        public void SetDirection(double newDirection)
        {
            direction = newDirection;
        }

        /**********************************************************************
        * 
        * Method to determine the behaviour of the ball thus the direction of the bounce on a collision with the bar 
        * dependent on the part of the bar that was hit. 
        * 
        **********************************************************************/
        public void CheckBarHit(int block)
        {
            switch (block)
            {
                case 1:
                    direction = MathHelper.ToRadians(240);
                    break;
                case 2:
                    direction = MathHelper.ToRadians(225);
                    break;
                case 3:
                    direction = MathHelper.ToRadians(210);
                    break;
                case 4:
                    direction = MathHelper.ToRadians(195);
                    break;
                case 5:
                    direction = MathHelper.ToRadians(180);
                    break;
                case 6:
                    direction = MathHelper.ToRadians(180);
                    break;
                case 7:
                    direction = MathHelper.ToRadians(165);
                    break;
                case 8:
                    direction = MathHelper.ToRadians(140);
                    break;
                case 9:
                    direction = MathHelper.ToRadians(125);
                    break;
                case 10:
                    direction = MathHelper.ToRadians(110);
                    break;
            }
        }

        /**********************************************************************
        * 
        * Method to determine the behaviour of the ball thus the direction of the bounce on a collision with a block 
        * dependent on the part of the bar that was hit. 
        * 
        **********************************************************************/
        public void CheckBlockHit(int block)
        {
            switch (block)
            {
                //Bottom
                case 1:
                    direction = Math.PI - direction;
                    break;
                //Top
                case 2: 
                    direction = Math.PI - direction;
                    break;
                //Left
                case 3:
                    direction = 2 * Math.PI - direction; //Math.PI - direction;
                    break;
                //Right
                case 4:
                    direction = 2 * Math.PI - direction;
                    break;
                //Corners
                default: direction = Math.PI - direction;
                    break;
            }
        }

        /**********************************************************************
        * 
        * Method to correct the position of the ball on an impact with another object so that 
        * it could prevent a further collision due to big overlap of the objects
        * 
        **********************************************************************/
        public void CorrectImpact(int block, int impact)
        {
            switch (block)
            {
                //Bottom
                case 1:
                    position.Y += (impact + 2);
                    break;
                //Top
                case 2:
                    position.Y -= (impact + 2);
                    break;
                //Left
                case 3:
                    position.X -= (impact + 2); 
                    break;
                //Right
                case 4:
                    position.X += (impact + 2);
                    break;
                /*
                //Left Top
                case 5:
                    position.X -= (impact + 2);
                    position.Y -= (impact + 2);
                    break;
                //Left Bottom
                case 6:
                    position.X -= (impact + 2);
                    position.Y += (impact + 2);
                    break;
                //Right Top
                case 7:
                    position.X += (impact + 2);
                    position.Y -= (impact + 2);
                    break;
                //Right Bottom
                case 8:
                    position.X += (impact + 2);
                    position.Y += (impact + 2);
                    break;
                */
                default: break;
            }
        }
    }
}
