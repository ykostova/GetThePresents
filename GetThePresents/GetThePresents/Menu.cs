using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;

namespace GetThePresents
{
    public class Menu
    {
        private List<string> MenuItems;
        private int iterator;
        public string InfoText { get; set; }
        public string Title { get; set; }
        public List<string> Instructions;
        private Color colour;
        public bool done;
        public int result;
        public Rectangle scoreBar;

        /**********************************************************************
         * 
         * Constructor to set all proeprties of the Menu Object
         * 
         **********************************************************************/
        public Menu()
        {
            result = 0;
            done = false;
            colour = new Color(252, 225, 41);
            Title = "GET THE PRESENTS";
            MenuItems = new List<string>();
            MenuItems.Add("Play Game");
            MenuItems.Add("Instructions");
            MenuItems.Add("Exit Game");
            Iterator = 0;
            Instructions = new List<string>();
            Instructions.Add("Help Santa deliver the presents to all homes on Christmas Eve.");
            Instructions.Add("Destroy all the blocks & collect the presents trapped within.");
            Instructions.Add("To navigate your bar use LEFT & RIGHT arrow keys");
            Instructions.Add("or LEFT & RIGHT mouse movements.");
            Instructions.Add("If the ball falls off the screen, you lose a life.");
            Instructions.Add("At the end of the level, Santa could be happy or sad.");
            Instructions.Add("If he is happy, you gain a life, else you lose one.");
            Instructions.Add("Throughout the game you would be rewarded extras to collect:");
            Instructions.Add("FIRE BALL - destroys everything ||| LIFE - gives a new life");
            Instructions.Add("SHORT BAR - shortens your bar ||| WIDEN - widens the bar");
            Instructions.Add("SPEED UP - speeds the ball ||| SLOW DOWN - slows the ball");
            InfoText = string.Empty;
        }

        /**********************************************************************
         * 
         * Iterator method to iterate through the menu items 
         * 
         **********************************************************************/
        public int Iterator
        {
            get
            {
                return iterator;
            }
            set
            {
                iterator = value;
                if (iterator > MenuItems.Count - 1) iterator = MenuItems.Count - 1;
                if (iterator < 0) iterator = 0;
            }
        }

        /**********************************************************************
         * 
         * Mehod to get the count of the Menu items
         * 
         **********************************************************************/
        public int GetNumberOfOptions()
        {
            return MenuItems.Count;
        }

        /**********************************************************************
         * 
         * Mehod to get a particular menu item
         * 
         **********************************************************************/
        public string GetItem(int index)
        {
            return MenuItems[index];
        }

        /**********************************************************************
         * 
         * Mehod to Draw the menu in runtime
         * 
         **********************************************************************/
        public void DrawMenu(ContentManager content, SpriteBatch batch, int screenWidth, SpriteFont cambria)
        {
            Texture2D texture = content.Load<Texture2D>("BackgroundStateLit");
            Rectangle size = new Rectangle(0, 0, texture.Width, texture.Height);
            batch.Draw(texture, new Vector2(0, 0), Color.White);
            batch.DrawString(cambria, Title, new Vector2(screenWidth / 2 - cambria.MeasureString(Title).X / 2, 370), colour);
            int yPos = 420;
            for (int i = 0; i < GetNumberOfOptions(); i++)
            {
                Color colourSelected = colour;
                if (i == Iterator)
                {
                    colourSelected = new Color(200, 75, 51);
                }
                batch.DrawString(cambria, GetItem(i), new Vector2(screenWidth / 2 - cambria.MeasureString(GetItem(i)).X / 2, yPos), colourSelected);
                yPos += 50;
            }
        }

        /**********************************************************************
         * 
         * Mehod to draw the instructions in runtime
         * 
         **********************************************************************/
        public void DrawInstructions(SpriteBatch batch, int screenWidth, SpriteFont cambria)
        {
            batch.DrawString(cambria, Title, new Vector2(screenWidth / 2 - cambria.MeasureString(Title).X / 2, 30), colour);
            int yPos = 80;
            cambria.Spacing = 0f;
            Color colourSelected = new Color(200, 75, 51);
            for (int i = 0; i < Instructions.Count; i++)
            {
                if(i >= Instructions.Count - 3 || i==2 || i==3)
                    batch.DrawString(cambria, Instructions[i], new Vector2(screenWidth / 2 - cambria.MeasureString(Instructions[i]).X / 2, yPos), colour);
                else
                    batch.DrawString(cambria, Instructions[i], new Vector2(screenWidth / 2 - cambria.MeasureString(Instructions[i]).X / 2, yPos), colourSelected);
                yPos += 40;
            }
            string prompt = "Press Enter to Return to MENU";
            batch.DrawString(cambria, prompt, new Vector2(screenWidth / 2 - cambria.MeasureString(prompt).X / 2, yPos+20), colourSelected);
        }

        /**********************************************************************
         * 
         * Mehod to draw the lost end game screen in runtime
         * 
         **********************************************************************/
        public void DrawLostEndScreen(SpriteBatch batch, int screenWidth, SpriteFont cambria)
        {
            batch.DrawString(cambria, InfoText, new Vector2(screenWidth / 2 - cambria.MeasureString(InfoText).X / 2, 300), colour);
            string prompt = "Press Enter to Continue";
            batch.DrawString(cambria, prompt, new Vector2(screenWidth / 2 - cambria.MeasureString(prompt).X / 2, 400), colour);
        }

        /**********************************************************************
         * 
         * Mehod to draw the won end game screen in runtime
         * 
         **********************************************************************/
        public void DrawWonEndScreen(GraphicsDevice theGD, ContentManager content, SpriteBatch batch, int screenWidth, SpriteFont cambria, int endResult)
        {
                String text = (endResult).ToString() + "%";
                Texture2D texture = content.Load<Texture2D>("BackgroundStateLit");
                batch.Draw(texture, new Vector2(0, 0), Color.White);
                batch.DrawString(cambria, text, new Vector2(screenWidth / 2 - cambria.MeasureString(text).X / 2, 390), colour);
                texture = content.Load<Texture2D>("bar");
                int barStart = screenWidth / 2 - texture.Width / 2;
                batch.Draw(texture, new Vector2(barStart, 430), Color.White);
                texture = CreateRectangle(theGD, colour);
                double growingWidth = ((double)endResult / 100) * 519;
                int thisWidth = (int)Math.Round(growingWidth);
                batch.Draw(texture, new Rectangle(barStart + 3, 433, thisWidth, 31), colour);
                texture = content.Load<Texture2D>("SadSanta");
                batch.Draw(texture, new Vector2(screenWidth / 6 - texture.Width / 2, 220), Color.White);
                texture = content.Load<Texture2D>("HappySanta");
                batch.Draw(texture, new Vector2(screenWidth * 5 / 6 - texture.Width / 2, 220), Color.White);
                if (done)
                {
                    batch.DrawString(cambria, InfoText, new Vector2(screenWidth / 2 - cambria.MeasureString(InfoText).X / 2, 500), colour);
                    string prompt = "Press Enter to Continue";
                    batch.DrawString(cambria, prompt, new Vector2(screenWidth / 2 - cambria.MeasureString(prompt).X / 2, 550), colour);
                }
        }

        /**********************************************************************
         * 
         * Mehod to dynamically create a texture to fill in a rectangle
         * 
         **********************************************************************/
        static private Texture2D CreateRectangle(GraphicsDevice theGD, Color colori)
        {
            Texture2D rectangleTexture = new Texture2D(theGD, 1, 1);
            rectangleTexture.SetData(new Color[] { colori });
            return rectangleTexture;
        }
    }
}
