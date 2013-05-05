using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GetThePresents
{
    public abstract class Block
    {
        public Vector2 position;
        public Rectangle size;
        public Texture2D texture;
        public int textureID;
        private int points;
        public bool isVisible;
        public bool isHit;
        public bool canCollide;
        public Vector2[] piecesPosition;
        public Texture2D pieceTexture;

        /***********************************************************************************
        * 
        * A Constructor method to set all the properties of the Block object- to be used within 
        * any of the inherited objects
        * 
        **********************************************************************************/
        public Block(ContentManager content, Vector2 pos, int textureUR, int thePoints)
        {
            textureID = textureUR;
            texture = content.Load<Texture2D>(textureUR.ToString());
            size = new Rectangle((int)pos.X, (int)pos.Y, texture.Width, texture.Height);

            piecesPosition = new Vector2[4];
            pieceTexture = content.Load<Texture2D>("piece");

            position = pos;

            for (int i = 0; i < piecesPosition.Length; i++)
            {
                piecesPosition[i] = new Vector2((int)position.X, (int)position.Y);
            }

            points = thePoints;
            isVisible = true;
            isHit = false;
            canCollide = true;
        }

        public abstract void Draw(SpriteBatch batch);

        public abstract void UpdatePosition();

        public abstract void DestroyBlock(ContentManager content);

        public virtual Rectangle GetSize()
        {
            return size;
        }

        public virtual Vector2 GetPosition()
        {
            return position;
        }

        public virtual int GetPoints()
        {
            return points;
        }

        public virtual void SetTexture(int number, ContentManager content)
        {
            textureID = number;
            texture = content.Load<Texture2D>(number.ToString());
            size = new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
        }
    }
}
