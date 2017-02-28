using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace mg35Test
{
    public class Sprite
    {
        protected Texture2D texture;   //image to load as texture.  Currently limited to whats in the content store
        ContentManager Content;
        int x, y, width, height;            //raw x,y,width and height.  May not be needed.
        Vector2 position;                   //upper left corner of texture
        Rectangle destination;              //rectangle where image will be shown.
        Vector2 origin;                     // center of rotation
        float rotation;                     // how far to rotate image in radians
        bool visible;                       // is the image visible;
        Color mask;


        public Sprite(ContentManager content,string assetName,float x, float y, int? width, int? height)
        {
            Content = content;
            texture = this.Content.Load<Texture2D>(assetName);
            position = new Vector2(x, y);
            if (width==null)
            {
                this.width = texture.Width;
            }
            if (height==null)
            {
                this.height = texture.Height;
            }
        }


        public void draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(texture, position, Color.White);
        }

        
        
    }
}