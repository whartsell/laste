using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.IO;

namespace laste
{
    public class Sprite
    {
        protected Texture2D texture;   //image to load as texture.  Currently limited to whats in the content store
        private GraphicsDevice graphicsDevice;
        private float radians = 0.0174533f;
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

        //public Vector2 Position { get; set; }
        public Rectangle Destination { get; set; } // this is where on the screen it will be drawn.
        public Rectangle? ViewPort { get; set; }   // this is what part of the texture that will actually be seen at the destination above
        public Vector2? Origin { get; set; }
        public float Rotation { get; set; }
        public bool Visible { get; set; }
        public Color? Mask { get; set; }
        




        public Sprite(GraphicsDevice graphicsDevice,string assetName,int x, int y, int? width, int? height)
        {
            this.graphicsDevice = graphicsDevice;
            int _width, _height;
            //texture = this.graphicsDevice.Load<Texture2D>(assetName);
            using (FileStream fs = File.OpenRead(assetName))
            {
                texture = Texture2D.FromStream(graphicsDevice, fs);
            }
            //if (x == 0 & y == 0)
            //{
            //    this.Position = Vector2.Zero;
            //}
            //else
            //{
            //    this.Position = new Vector2(x, y);
            //}

            _width = (width == null) ? texture.Width : (int)width;
            _height = (height == null) ? texture.Height : (int)height;
            this.Destination = new Rectangle(x, y, _width, _height);

            //scale = new Vector2(_width / texture.Width, _height / texture.Height);
            scale = new Vector2(0.25f, 0.25f);
            Mask = Color.Green;
            
            
        }

       
        public void draw(SpriteBatch spriteBatch)
        {
           
           
            spriteBatch.Draw(texture, null, Destination, ViewPort, Origin, Rotation * radians,scale, Mask);

        }

        
        
    }
}