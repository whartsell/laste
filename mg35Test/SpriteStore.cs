using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MoonSharp.Interpreter.Interop;
using System;
using System.Collections.Generic;

namespace mg35Test
{
    class SpriteStore
    {
        ContentManager Content;
        Dictionary<Guid,Sprite> Sprites;
        public SpriteStore(ContentManager content)
        {
            Sprites = new Dictionary<Guid,Sprite>();
            Content = content;

        }
        [MoonSharpVisible(true)]
        public Guid addSprite(string assetName,float x,float y, int? width, int? height)
        {
            Guid guid;
            Sprite sprite = new Sprite(Content, assetName, x, y, width, height);
            guid = Guid.NewGuid();
            Sprites.Add(guid, sprite);
            return guid;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            foreach (var pair in Sprites)
            {
                pair.Value.draw(spriteBatch);
            }
        }
        
        public void test()
        {
            Console.WriteLine("test");
        }
    }
}
