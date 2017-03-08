using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MoonSharp.Interpreter;
using System;
using System.IO;

namespace laste
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Game1 : Game
    {
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        Script gauge;
        SpriteStore store;
        KeyValueStore kvStore;
        Network network;
        Protocol protocol;
        int rotation;
        Texture2D test;
        Rectangle srcRect;
        Rectangle dstRect;
        string aFile;
        //ASI asi;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            
            Content.RootDirectory = "Content";
            
           

        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            UserData.RegisterType<Guid>();
            gauge = new Script();
            store = new SpriteStore(graphics.GraphicsDevice, gauge);
            kvStore = new KeyValueStore();
            network = new Network(12800);
            protocol = new Protocol(kvStore, store, network);


            gauge.Globals["Test"] = (System.Action)store.test;
            gauge.Globals["addSprite"] = (Func<string, int, int, int?, int?, Guid>)store.addSprite;
            gauge.Globals["rotateSprite"] = (System.Action<Guid, float>)store.rotateSprite;
            gauge.Globals["setSpriteOrigin"] = (System.Action<Guid, float, float>)store.setSpriteOrigin;
            gauge.Globals["subscribeSprite"] = (System.Action<object[]>)store.subscribeSprite;
            gauge.Globals["setAircraftType"] = (System.Action<string>)kvStore.setAircraftType;
            gauge.Globals["setSpriteViewPort"] = (System.Action<Guid, int, int, int, int>)store.spriteViewPort;
            rotation = 0;
            network.MessageReceived += protocol.MessageReceivedHandler;
            network.Start();
            aFile = "C:/Users/whartsell/Documents/ARU-2/resources/Horizon.png";
            using (FileStream fs = File.OpenRead(aFile))
            {
                test = Texture2D.FromStream(graphics.GraphicsDevice, fs);
            }
            base.Initialize();
            
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            // asi = new mg35Test.ASI(Content);
            gauge.DoFile("scripts/gauge.lua");
            // TODO: use this.Content to load your game content here
            //store.addSprite("resources/faceplate", 0, 0, null, null);
            //store.addSprite("resources/needle", 150, 150, null, null);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
            network.Stop();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // TODO: Add your update logic here
            //asi.update();
            kvStore.set("Test", rotation++);
            kvStore.update(store);
            srcRect = new Rectangle(0, 0, 338, 338);
            srcRect.Y = 520;
            dstRect = new Rectangle(0,0, 338, 338);

            base.Update(gameTime);

        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            //asi.Draw(spriteBatch);
            store.draw(spriteBatch);
            //spriteBatch.Draw(test,dstRect,srcRect,Color.White);
            spriteBatch.End();
            // TODO: Add your drawing code here

            base.Draw(gameTime);
        }
    }
}
