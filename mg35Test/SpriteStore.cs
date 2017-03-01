using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using System;
using System.Collections.Generic;

namespace mg35Test
{
    class SpriteStore
    {
        ContentManager Content;
        Script Script;
        Dictionary<Guid,Sprite> Sprites;

        public SpriteStore(ContentManager content,Script script)
        {
            Sprites = new Dictionary<Guid,Sprite>();
            Content = content;
            Script = script;


        }

        public Guid addSprite(string assetName,float x,float y, int? width, int? height)
        {
            Guid guid;
            Sprite sprite = new Sprite(Content, assetName, x, y, width, height);
            guid = Guid.NewGuid();
            Sprites.Add(guid, sprite);
            return guid;
        }

        public void rotateSprite(Guid id,float angle)
        {
            Sprites[id].Rotation = angle;
        }

        public void draw(SpriteBatch spriteBatch)
        {
            foreach (var pair in Sprites)
            {
                pair.Value.draw(spriteBatch);
            }
        }
        
        public void setSpriteOrigin(Guid id,float x,float y)
        {
            Sprites[id].Origin = new Vector2(x, y);
        }
        public void test()
        {
            Console.WriteLine("test");
        }

        public void subscribeSprite(params object[] list)
        {
            // the last param is always the callback
            Closure callback = (Closure)list[list.Length - 1]; // last element is always the name of the callback
            Console.WriteLine("callback is {0} - {1}", callback.GetType().ToString(),callback.ToString());
            List<object> parameters = new List<object>() ; 
            for (int i = 0; i <list.Length-1; i++)
            {
                Console.WriteLine("Param {0}: {1}", i, DynValue.FromObject(Script,list[i]).Type);
                parameters.Add(list[i]);
            }
            callback.Call(parameters.ToArray());

        }
    }
}
