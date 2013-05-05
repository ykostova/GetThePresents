using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace GetThePresents
{
    public class Bar
    {
        private Vector2 resetPos;
        private Vector2 position;
        private int speed;
        private Rectangle size;
        private int screenWidth;
        private Texture2D texture;

        /**********************************************************************
         * 
         * Constructor method to set up the initial properties of a Bar object
         * 
         **********************************************************************/
        public Bar(ContentManager content, Vector2 screenSize)
        {
	        speed = 6;
	        texture = content.Load<Texture2D>("BarNormal");
	        size = new Rectangle(0, 0, texture.Width, texture.Height);
	        resetPos = new Vector2( screenSize.X / 2 - size.Width / 2, screenSize.Y - 25);
            position = resetPos;
	        screenWidth = (int)screenSize.X;
        }


        /**********************************************************************
        * 
        * Method to draw the bar using the provided texture
        * 
        **********************************************************************/
        public void Draw(SpriteBatch batch, float scale)
        {
            batch.Draw(texture, position, Color.White);
        }


        /**********************************************************************
        * 
        * Method to set the position of the bar so that it does not go off the screen when moved
        * 
        **********************************************************************/
        private void SetPosition(Vector2 position)
        {
            if (position.X < 0)
            {
                position.X = 0;
            }

            if (position.X > screenWidth - size.Width)
            {
                position.X = screenWidth - size.Width;
            }

            this.position = position;
        }


        /**********************************************************************
        * 
        * Method to define the left movement of the bar based on current speed
        * 
        **********************************************************************/
        public void MoveLeft()
        {
            SetPosition(position + new Vector2(-speed, 0));
        }


        /**********************************************************************
        * 
        * Method to define the right movement of the bar based on current speed
        * 
        **********************************************************************/
        public void MoveRight()
        {
            SetPosition(position + new Vector2(speed, 0));
        }


        /**********************************************************************
        * 
        * Method to define the update of position and speed of the bar
        * 
        **********************************************************************/
        public virtual void UpdatePosition()
        {
            size.X = (int)position.X;
            size.Y = (int)position.Y;
            speed = 8;
        }


        /**********************************************************************
        * 
        * Method to get the rectangle underlying the bar object
        * 
        **********************************************************************/
        public Rectangle GetSize()
        {
            return size;
        }


        /**********************************************************************
        * 
        * Method to get the position of the bar object as a vector
        * 
        **********************************************************************/
        public Vector2 GetPosition()
        {
            return position;
        }

        /**********************************************************************
        * 
        * Method to set the bar to the reset position
        * 
        **********************************************************************/
        public void Reset()
        {
            position = resetPos;
        }

        /**********************************************************************
        * 
        * Method to stop the bar from movement
        * 
        **********************************************************************/
        public void Stop()
        {
            position = resetPos;
            speed = 0;
        }

        /**********************************************************************
        * 
        * Method to set a new texture to the bar
        * 
        **********************************************************************/
        public virtual void SetTexture(String textureID, ContentManager content)
        {
            texture = content.Load<Texture2D>(textureID);
            size = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }
    }
}
