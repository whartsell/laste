using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System;

namespace mg35Test
{
    public class Sprite
    {
        protected Texture2D texture;   //image to load as texture.  Currently limited to whats in the content store
        private ContentManager Content;
        private float radians = 0.0174533f;
        //int x, y, width, height;            //raw x,y,width and height.  May not be needed.
        Vector2 position;                   //upper left corner of texture
        Rectangle destination;              //rectangle where image will be shown.
        Vector2 origin;                     // center of rotation
        float rotation;                     // how far to rotate image in radians
        bool visible;                       // is the image visible;
        Color mask;
        private Vector2 scale;
        // TODO need to use width,height to create scale so people can set size

        public int Width
        {
            get
            {
                return texture.Width;
            }
        }


        public int Height
        {
            get
            {
                return texture.Height;
            }
        }

        public Vector2 Position { get; set; }
        public Rectangle Destination { get; set; }
        public Vector2 Origin { get; set; }
        public float Rotation { get; set; }
        public bool Visible { get; set; }
        public Color Mask { get; set; }
        




        public Sprite(ContentManager content,string assetName,float x, float y, int? width, int? height)
        {
            Content = content;
            int _width, _height;
            texture = this.Content.Load<Texture2D>(assetName);
            if (x == 0 & y == 0)
            {
                this.Position = Vector2.Zero;
            }
            else
            {
                this.Position = new Vector2(x, y);
            }

            _width = (width == null) ? texture.Width : (int)width;
            _height = (height == null) ? texture.Height : (int)height;
            scale = new Vector2(_width / texture.Width, _height / texture.Height);
            Mask = Color.White;
        }

       
        public void draw(SpriteBatch spriteBatch)
        {
            if (Destination == Rectangle.Empty)
            {
                spriteBatch.Draw(texture, Position, null, null, Origin, Rotation * radians, scale, Mask);
            }
            else
                spriteBatch.Draw(texture, Position, Destination, null, Origin, Rotation * radians, scale, Mask);

        }

        
        
    }
}