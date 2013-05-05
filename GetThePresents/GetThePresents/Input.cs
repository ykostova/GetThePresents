using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;

namespace GetThePresents
{
    public class Input
    {
        private KeyboardState keyboardState;
        private KeyboardState lastState;
        private MouseState mouseState;
        private MouseState lastMState;

        /**********************************************************************
         * 
         * Constructor method to set the initial values of the keyboard and mouse input
         * 
         **********************************************************************/
        public Input()
        {
            keyboardState = Keyboard.GetState();
            lastState = keyboardState;
            mouseState = Mouse.GetState();
            lastMState = mouseState;
        }

        /**********************************************************************
         * 
         * Method to update the values of the keyboard and the mouse input 
         * 
         **********************************************************************/
        public void Update()
        {
            lastState = keyboardState;
            keyboardState = Keyboard.GetState();
            lastMState = mouseState;
            mouseState = Mouse.GetState();
        }

        /**********************************************************************
         * 
         * Mehod to determine whether there is a right movement based on key boolean flag
         * 
         **********************************************************************/
        public bool keyRight
        {
            get
            {
                return keyboardState.IsKeyDown(Keys.Right);
            }
        }

        /**********************************************************************
         * 
         * Mehod to determine the mouse is moving left or right
         * 
         **********************************************************************/
        public int mouseHorizontal()
        {
             return lastMState.X - mouseState.X;
        }

        /**********************************************************************
         * 
         * Mehod to determine the mouse is moving up or down
         * 
         **********************************************************************/
        public int mouseVertical()
        {
            return lastMState.Y - mouseState.Y;
        }

        /**********************************************************************
         * 
         * Mehod to determine whether there is a left movement based on key boolean flag
         * 
         **********************************************************************/
        public bool keyLeft
        {
            get
            {
                return keyboardState.IsKeyDown(Keys.Left);
            }
        }

        /**********************************************************************
         * 
         * Mehod to determine whether there is an upward movement based on key boolean flag
         * 
         **********************************************************************/
        public bool keyUp
        {
            get
            {
                if (Game1.gamestate == Game1.GameStates.Menu)
                {
                    return keyboardState.IsKeyDown(Keys.Up) && lastState.IsKeyUp(Keys.Up);
                }
                else
                {
                    return keyboardState.IsKeyDown(Keys.Up);
                }
            }
        }

        /**********************************************************************
         * 
         * Mehod to determine whether there is a down movement based on key boolean flag
         * 
         **********************************************************************/
        public bool keyDown
        {
            get
            {
                if (Game1.gamestate == Game1.GameStates.Menu)
                {
                    return keyboardState.IsKeyDown(Keys.Down) && lastState.IsKeyUp(Keys.Down);
                }
                else
                {
                    return keyboardState.IsKeyDown(Keys.Down);
                }
            }
        }

        /**********************************************************************
         * 
         * Mehod to determine whether ENTER has been hit thus a menu selection or enter to continue 
         * 
         **********************************************************************/
        public bool MenuSelect
        {
            get
            {
                return keyboardState.IsKeyDown(Keys.Enter) && lastState.IsKeyUp(Keys.Enter);
            }
        }

    }
}
