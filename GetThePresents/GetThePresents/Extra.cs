using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

namespace GetThePresents.Blocks
{
    public class Extra
    {
        public string name;
        public Vector2 position;
        public Rectangle size;
        public Texture2D texture;
        public bool isVisible;
        public int timer;
        public bool timerInUse;
        public bool start;
        public bool end;

            public Extra(ContentManager content, Vector2 pos, int textureNumber)
            {
                name = ExtraFactory.CreateName(content, textureNumber);
                texture = ExtraFactory.CreateExtra(content, name);
                size = new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height);
                position = pos;
                isVisible = true;
                timer = 500;
                start = false;
                end = false;
                timerInUse = false;
            }

            public void DestroyBlock(ContentManager content)
            {
                isVisible = false;
                start = true;
                timerInUse = true;
            }

            public void Draw(SpriteBatch batch)
            {
                if (isVisible)
                {
                    batch.Draw(texture, position, Color.White);
                }
            }

            public void UpdatePosition()
            {
                if (timerInUse) timer--;
                if (timer == 0)
                {
                    timerInUse = false;
                    end = true;
                }
                if (position.Y < 600 && isVisible)
                {
                    position.Y += 1;
                }
                size.X = (int)position.X;
                size.Y = (int)position.Y;
            }

            public virtual string GetName()
            {
                return name;
            }    

            public virtual Rectangle GetSize()
            {
                return size;
            }

            public virtual Vector2 GetPosition()
            {
                return position;
            }
      }
}
