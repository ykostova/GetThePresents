using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Threading;
using GetThePresents.Blocks;

namespace GetThePresents
{
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private static Game1 instance = null;

        public const int BLOCK_WIDTH = 50;
        public const int BLOCK_HEIGHT = 27;
        public const int SCREEN_WIDTH = 800;
        public const int SCREEN_HEIGHT = 600;
        public const int BLOCK_COL = 7;
        public const int BLOCK_ROW = 5;

        private List<int[]> levelPattern;
        private List<Extra> extras;

        Color backgroundColor;
        Color textColor;

        int i;
        int j;

        SpriteFont cambria;

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        private Bar bar;

        private Input input;

        private Ball ball;

        Menu menu;

        private Block[,] blockWall;

        private Block icon;

        private int progress;

        private int liveBlocks;
        private int points;
        private int level;
        private int lives;
        private int presents;
        private int resetTimer;
        private int delayTimer;
        private bool resetTimerInUse;
        private bool delayTimerInUse;
        private int extraPeriod;

        Random random;

        public static GameStates gamestate;

        /***********************************************************************************
         * 
         * Enumerator for the game states
         * 
         **********************************************************************************/
        public enum GameStates
        {
            Menu,
            Instructions,
            Running,
            WonEnd,
            LostEnd
        }

        /***********************************************************************************
         * 
         * A Constructor for the Game class
         * 
         **********************************************************************************/
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";

            backgroundColor = new Color(12, 25, 78);
            textColor = new Color(252, 225, 41);

            menu = new Menu();

            blockWall = new Block[BLOCK_ROW, BLOCK_COL];

            input = new Input();

            lives = 3;
            level = 0;
            points = 0;
            presents = 0;
            progress = 0;
            resetTimer = 0;
            resetTimerInUse = true;
            delayTimer = 0;
            delayTimerInUse = false;

            levelPattern = new List<int[]>();

            levelPattern.Add(new int[] {4,3,2,1,2,3,4,6,5,7,2,7,5,6,4,3,9,1,9,3,4,6,5,7,2,7,5,6,4,3,2,1,2,3,4});

            levelPattern.Add(new int[] {5,4,3,1,3,4,5,6,7,2,7,2,7,6,7,9,7,9,7,9,7,6,7,2,7,2,7,6,5,4,3,1,3,4,5});

            random = new Random();
        }

        /***********************************************************************************
         * 
         * A Method to implement Singleton Design Pattern
         * 
         **********************************************************************************/
        public static Game1 GetInstance()
        {
                if (instance == null)
                {
                    instance = new Game1();
                }
                return instance;
        }

        /***********************************************************************************
         * 
         * A Method to initialise the game and its graphics
         * 
         **********************************************************************************/
        protected override void Initialize()
        {
            cambria = Content.Load<SpriteFont>("Cambria"); 
            
            gamestate = GameStates.Menu;

            graphics.PreferredBackBufferWidth = SCREEN_WIDTH;
            graphics.PreferredBackBufferHeight = SCREEN_HEIGHT;
            graphics.ApplyChanges();

            base.Initialize();
        }

        /***********************************************************************************
         * 
         * A Method to load the game content
         * 
         **********************************************************************************/
        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        /***********************************************************************************
         * 
         * A Method to coordinate the update within the game
         * 
         **********************************************************************************/
        protected override void Update(GameTime gameTime)
        {
            input.Update();

            if (gamestate == GameStates.Running)
            {
                if (lives == 0)
                {
                    menu.InfoText = "You LOST the Game";
                    gamestate = GameStates.LostEnd;
                }
                else if (liveBlocks == 0)
                {
                    menu.InfoText = "You WON Level " + level;
                    progress = 0;
                    delayTimerInUse = true;
                    if (delayTimerInUse)
                    {
                        delayTimer++;
                        ball.Stop();
                        ball.isVisible = false;
                    }
                    if (delayTimer == 180)
                    {
                        if ((double)presents >= (double)(level + 1)/2 ) lives++;
                        else lives--;
                        gamestate = GameStates.WonEnd;
                    }
                }
                if (resetTimerInUse)
                {
                    resetTimer++;
                    ball.Stop();
                    bar.Stop();
                }
                if (resetTimer == 120)
                {
                    resetTimerInUse = false;
                    ball.Reset();
                    bar.Reset();
                    resetTimer = 0;
                }

                    if (input.keyLeft || input.mouseHorizontal() > 0) bar.MoveLeft();
                    if (input.keyRight || input.mouseHorizontal() < 0) bar.MoveRight();

                bar.UpdatePosition();
                ball.UpdatePosition();

                if (bar.GetSize().Intersects(ball.GetSize()))
                {
                    ball.CheckBarHit(CheckBarHitLocation(bar));
                }

                UpdateBlocks();
                if (extras.Count!=0) UpdateExtras();

                if (!resetTimerInUse)
                {
                    if (ball.GetPosition().Y > 775)
                    {
                        resetTimerInUse = true;
                        lives--;
                    }
                }
            }
            else if (gamestate == GameStates.Menu)
            {
                if (input.keyDown)
                {
                    menu.Iterator++;
                }
                else if (input.keyUp)
                {
                    menu.Iterator--;
                }
                if (input.MenuSelect)
                {
                    if (menu.Iterator == 0)
                    {
                        gamestate = GameStates.Running;
                        level++;
                        PlayGame();
                    }
                    else if (menu.Iterator == 1)
                    {
                        gamestate = GameStates.Instructions;
                    }
                    else if (menu.Iterator == 2)
                    {
                        this.Exit();
                    }
                    menu.Iterator = 0;
                }
            }
            else if (gamestate == GameStates.Instructions)
            {
                if (input.MenuSelect)
                {
                    gamestate = GameStates.Menu;
                }
            }
            else if (gamestate == GameStates.LostEnd)
            {
                if (input.MenuSelect)
                {
                    gamestate = GameStates.Menu;
                }
            }
            else if (gamestate == GameStates.WonEnd)
            {
                if (menu.done)
                {
                    if (level == 1)
                    {
                        if (input.MenuSelect)
                        {
                            gamestate = GameStates.Running;
                            level++;
                            menu.done = false;
                            PlayGame();
                        }
                    }
                    else
                    {
                        if (input.MenuSelect)
                        {
                            gamestate = GameStates.Menu;
                        }
                    }
                }
                else
                {
                    double result = ((double)presents / (level + 1)) * 100;
                    if (progress < (int)Math.Round(result))
                    {
                        progress++;
                    }
                    else
                    {
                        Thread.Sleep(3000);
                        menu.done = true;
                    }
                }
            }
            
            base.Update(gameTime);
        }

        /***********************************************************************************
         * 
         * A Method to Update the Blocks
         * 
         **********************************************************************************/
        private void UpdateBlocks()
        {
            for (i = 0; i < BLOCK_ROW; i++)
            {
                for (j = 0; j < BLOCK_COL; j++)
                {
                    if (blockWall[i, j].GetSize().Intersects(ball.GetSize()) && blockWall[i, j].canCollide)
                    {
                        points += blockWall[i, j].GetPoints();
                        blockWall[i, j].DestroyBlock(Content);
                        if (!ball.extraFire)
                        {
                            int hitPlace = CheckBlockHitLocation((Block)blockWall[i, j]);
                            int impactCorrection = FindImpact(hitPlace, (Block)blockWall[i, j]);
                            ball.CheckBlockHit(hitPlace);
                            ball.CorrectImpact(hitPlace, impactCorrection);
                            ball.UpdatePosition();
                        }
                        ball.UpdatePosition();
                        if (blockWall[i, j].isHit) liveBlocks--;
                        for (int k = 1; k <= (level+1); k++)
                        {
                            if (liveBlocks == (k * extraPeriod)) 
                                 extras.Add(new Extra(Content, blockWall[i,j].position, random.Next(1,6)));
                        }
                    }

                    blockWall[i, j].UpdatePosition();

                    if (blockWall[i, j].textureID == 10 && blockWall[i, j].isVisible)
                    {
                        blockWall[i, j].UpdatePosition();
                        if (blockWall[i, j].GetSize().Intersects(bar.GetSize()))
                        {
                            blockWall[i, j].isVisible = false;
                            presents++;
                        }
                    }
                }
            }
        }

        /***********************************************************************************
         * 
         * A Method to Update the Extras Falling
         * 
         **********************************************************************************/
        private void UpdateExtras()
        {
            for (int k = 0; k < extras.Count; k++)
            {
                //Update the position of the extra
                extras[k].UpdatePosition();
                //Behaviour of the extra If collected with the bar
                if (extras[k].GetSize().Intersects(bar.GetSize()) && extras[k].isVisible) 
                {
                    extras[k].DestroyBlock(Content);
                    //If the previous collected extra is the same as the current 
                    //add on the extra influence time
                    if (k != 0 && extras[k - 1].GetName().Equals(extras[k].GetName()))
                    {
                        extras[k - 1].timer += 500;
                        extras.Remove(extras[k]);
                        return;
                    }
                }
                //If the extra is collected with the bar => start its influence
                if (k < extras.Count && extras[k].start)
                {
                    ExtraInfluence(Content, extras[k].GetName());
                    extras[k].start = false;
                }
                //If the extra is influencing the objects => end its influence
                if (k < extras.Count && extras[k].end)
                {
                    StopInfluence(Content, extras[k].GetName());
                    extras[k].end = false;
                    extras.Remove(extras[k]);
                }
            }
        }

        /***********************************************************************************
         * 
         * A Method to Draw all the game components
         * 
         **********************************************************************************/
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(backgroundColor);

            spriteBatch.Begin();
            if (gamestate == GameStates.Running)
            {
                DrawRunning(spriteBatch);
            }
            else if (gamestate == GameStates.Menu)
            {
                menu.DrawMenu(Content, spriteBatch, SCREEN_WIDTH, cambria);
            }
            else if (gamestate == GameStates.Instructions)
            {
                menu.DrawInstructions(spriteBatch, SCREEN_WIDTH, cambria);
            }
            else if (gamestate == GameStates.LostEnd)
            {
                menu.DrawLostEndScreen(spriteBatch, SCREEN_WIDTH, cambria);
            }
            else if (gamestate == GameStates.WonEnd)
            {
                menu.DrawWonEndScreen(this.GraphicsDevice, Content, spriteBatch, SCREEN_WIDTH, cambria, progress);
            }
            spriteBatch.End();

            base.Draw(gameTime);
        }

        /***********************************************************************************
         * 
         * A Method to Draw the running game components
         * 
         **********************************************************************************/
        private void DrawRunning(SpriteBatch spriteBatch)
        {
            Texture2D texture = Content.Load<Texture2D>("BackgroundStatePlay");
            Rectangle size = new Rectangle(0, 0, texture.Width, texture.Height);
            spriteBatch.Draw(texture, new Vector2(0, 0), Color.White);
            bar.Draw(spriteBatch, 0f);
            ball.Draw(spriteBatch);

            if (extras.Count != 0)
                foreach (Extra entry in extras) entry.Draw(spriteBatch);

            foreach (Block entry in blockWall) entry.Draw(spriteBatch);

            spriteBatch.DrawString(cambria, ("LEVEL: "+ level +"       Lives: " + lives + "      Points: " + points), new Vector2(80, 20), textColor);
            spriteBatch.DrawString(cambria, (presents.ToString()), new Vector2(SCREEN_WIDTH - 110, 20), textColor);
            icon.Draw(spriteBatch);
        }

        /***********************************************************************************
         * 
         * A Method to check the location of the bar - ball collision
         * 
         **********************************************************************************/
        private int CheckBarHitLocation(Bar bar)
        {
            int block = 0;
            if (ball.GetPosition().X < bar.GetPosition().X + bar.GetSize().Width / 20) block = 1;
            else if (ball.GetPosition().X < bar.GetPosition().X + bar.GetSize().Width / 10 * 2) block = 2;
            else if (ball.GetPosition().X < bar.GetPosition().X + bar.GetSize().Width / 10 * 3) block = 3;
            else if (ball.GetPosition().X < bar.GetPosition().X + bar.GetSize().Width / 10 * 4) block = 4;
            else if (ball.GetPosition().X < bar.GetPosition().X + bar.GetSize().Width / 10 * 5) block = 5;
            else if (ball.GetPosition().X < bar.GetPosition().X + bar.GetSize().Width / 10 * 6) block = 6;
            else if (ball.GetPosition().X < bar.GetPosition().X + bar.GetSize().Width / 10 * 7) block = 7;
            else if (ball.GetPosition().X < bar.GetPosition().X + bar.GetSize().Width / 10 * 8) block = 8;
            else if (ball.GetPosition().X < bar.GetPosition().X + bar.GetSize().Width / 20 * 19) block = 9;
            else block = 10;
            return block;
        }

        /***********************************************************************************
         * 
         * A Method to check the location of the block - ball collision
         * 
         **********************************************************************************/
        private int CheckBlockHitLocation(Block theBlock)
        {
            int block = 0;
            //Bottom
            if (ball.GetSize().Top <= theBlock.GetSize().Bottom) block = 1;
            //Top
            else if (ball.GetSize().Bottom >= theBlock.GetSize().Top) block = 2;
            //Left
            else if (ball.GetSize().Right >= theBlock.GetSize().Left) block = 3;
            //Right
            else if (ball.GetSize().Left <= theBlock.GetSize().Right) block = 4;
            //Left Top
            else if (ball.GetSize().Right >= theBlock.GetSize().Left && ball.GetSize().Bottom >= theBlock.GetSize().Top) block = 5;
            //Left Bottom
            else if (ball.GetSize().Right >= theBlock.GetSize().Left && ball.GetSize().Top <= theBlock.GetSize().Bottom) block = 6;
            //Right Top
            else if (ball.GetSize().Left <= theBlock.GetSize().Right && ball.GetSize().Bottom >= theBlock.GetSize().Top) block = 7;
            //Right Bottom
            else block = 8;
            return block;
        }

        /***********************************************************************************
         * 
         * A Method to find the overlap in the collision
         * 
         **********************************************************************************/
        private int FindImpact(int hitPlace,Block theBlock)
        { 
            int impact = 0;
            switch (hitPlace)
            {
                //Bottom
                case 1: impact = ball.GetSize().Top - theBlock.GetSize().Bottom;
                    break;
                //Top
                case 2: impact = ball.GetSize().Bottom - theBlock.GetSize().Top;
                    break;
                //Left
                case 3: impact = ball.GetSize().Right - theBlock.GetSize().Left;
                    break;
                //Right
                case 4: impact = ball.GetSize().Left - theBlock.GetSize().Right;
                    break;
                default: impact = 3;
                    break;
            }
            return impact;
        }

        /***********************************************************************************
         * 
         * A Method to begin a new game via the menu options
         * 
         **********************************************************************************/
        private void PlayGame()
        {
            /*
             * Calculate the times at which to drop extra within the game, based on the overall number of blocks
             * and the number of presents + 1 to be collected within the level.
             */
            extraPeriod = (int)Math.Round((double)(BLOCK_COL * BLOCK_ROW) / (level + 2));
            extras = new List<Extra>();
            
            icon = new PresentBlock(Content, new Vector2(SCREEN_WIDTH - 80, 25), 9, 100);
            icon.DestroyBlock(Content);
            icon.canCollide = false;

            blockWall = new Block[BLOCK_ROW, BLOCK_COL];

            liveBlocks = BLOCK_COL * BLOCK_ROW;

            int x0 = (SCREEN_WIDTH - (BLOCK_COL * BLOCK_WIDTH) - ((BLOCK_COL - 1) * 5)) / 2;
            int y = 100;
            int x = x0;

            //Creating all the blocks for the level
            for (i = 0; i < BLOCK_ROW; i++)
            {
                for (j = 0; j < BLOCK_COL; j++)
                {
                    //Create the blocks of a certain type dependent on the pattern number
                    blockWall[i, j] = BlockFactory.CreateBlock(levelPattern[level - 1][i * BLOCK_COL + j], Content, new Vector2(x, y));
                    blockWall[i, j].isVisible = true;
                    x += (BLOCK_WIDTH + 5);
                }
                x = x0;
                y += (BLOCK_HEIGHT + 5);
            }

            bar = new Bar(Content, new Vector2(SCREEN_WIDTH, SCREEN_HEIGHT));

            input = new Input();

            ball = new Ball(Content, new Vector2(SCREEN_WIDTH, SCREEN_HEIGHT));
            ball.isVisible = true;

            presents = 0;

            resetTimer = 0;
            resetTimerInUse = true;

            delayTimer = 0;
            delayTimerInUse = false;

            base.Initialize();
        }

        /***********************************************************************************
         * 
         * A Method to create the influence of extras, thus change the objects affected
         * to the new state required.
         * 
         **********************************************************************************/
        private void ExtraInfluence(ContentManager content, string textureName)
        {
            switch (textureName)
            {
                case "FireBall": ball.extraFire = true;  break;

                case "Life": lives++;  break;

                case "Shorten": bar.SetTexture("BarSmall", Content);  break;

                case "Widen": bar.SetTexture("BarLarge", Content); break;

                case "SpeedUp": ball.SetSpeed(2); break;

                case "SlowDown": ball.SetSpeed(-2); break;

                default: break;
            }
        }

        /***********************************************************************************
         * 
         * A Method to stop the influence of extras, thus return the objects affected to 
         * the state they started with.
         * 
         **********************************************************************************/
        private void StopInfluence(ContentManager content, string textureName)
        {
            switch (textureName)
            {
                case "FireBall": ball.extraFire = false; break;

                case "Shorten": bar.SetTexture("BarNormal", Content); break;

                case "Widen": bar.SetTexture("BarNormal", Content); break;

                case "SpeedUp": ball.SetSpeed(-2); break;

                case "SlowDown": ball.SetSpeed(+2); break;

                default: break;
            }
        }

    }
}
