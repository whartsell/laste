using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace mg35Test
{
    class ASI
    {
        Texture2D faceplate, needle;
        Vector2 needlePos, needleOrg;
        Rectangle needleRect;
        ContentManager Content;
        float rads = 0;

        public ASI(ContentManager content)
        {
            Content = content;
            needlePos = new Vector2(150, 150);
            faceplate = this.Content.Load<Texture2D>("resources/faceplate");
            needle = this.Content.Load<Texture2D>("resources/needle");
            needleRect = new Rectangle(0, 0, needle.Width, needle.Height);
            needleOrg = new Vector2(needle.Width / 2, needle.Height - 35);
        }
         ~ASI()
        {
            Content.Unload();
        }

        public void update()
        {
            rads++;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
        
            spriteBatch.Draw(faceplate, Vector2.Zero);
            spriteBatch.Draw(needle, needlePos, needleRect, Color.White, rads * 0.0174f
                , needleOrg, 0.9f, SpriteEffects.None, 1);

        }
    }
}
