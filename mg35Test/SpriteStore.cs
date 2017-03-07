using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using MoonSharp.Interpreter;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace mg35Test
{
    class SpriteStore
    {
        GraphicsDevice graphicsDevice;
        Script Script;
        Dictionary<Guid,Sprite> Sprites; //TODO: need to create a type safe ordered dictionary class
        
        //Dictionary<Closure, List<string>> RegisteredCallbacks;

        public Dictionary<Closure,List<string>> RegisteredCallbacks { get;}

        public SpriteStore(GraphicsDevice gDevice,Script script)
        {
            Sprites = new Dictionary<Guid,Sprite>();
            graphicsDevice = gDevice;
            Script = script;
            RegisteredCallbacks = new Dictionary<Closure, List<string>>();


        }

        public Guid addSprite(string assetName,int x,int y, int? width, int? height)
        {
            Guid guid;
            Sprite sprite = new Sprite(graphicsDevice, assetName, x, y, width, height);
            guid = Guid.NewGuid();
            Sprites.Add(guid, sprite);
            return guid;
        }

        public void rotateSprite(Guid id,float angle)
        {
            Sprites[id].Rotation = angle;
        }

        public void spriteViewPort(Guid id, int x, int y, int width, int height)
        {
            Sprites[id].ViewPort = new Rectangle(x, y, width, height);
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
            List<string> keys = new List<string>() ; 
            for (int i = 0; i <list.Length-1; i++)
            {
                DynValue value = DynValue.FromObject(Script, list[i]);
                Console.WriteLine("Param {0}: {1}", i, value.Type);
                if (value.Type == DataType.String)
                {
                    Console.WriteLine("adding {0} to keys",value.String);
                    keys.Add(value.String);
                }
                    
            }
            RegisteredCallbacks.Add(callback, keys);
        }

        public HashSet<string> getAllSubscribedKeys()
        {
            HashSet<string> keys = new HashSet<string>();
            foreach (var pair in RegisteredCallbacks)
            {
                foreach (string key in pair.Value)
                {
                    keys.Add(key);
                }
            }
            return keys;
        }


    }
}
