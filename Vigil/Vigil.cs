using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Vigil
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class Vigil : Game
    {
        static public GraphicsDeviceManager graphics;
        InputHandler _InputHandler = new InputHandler();
        DrawHandler _DrawHandler;

        public SpriteFont DebugFont;

        public struct ShipMovements
        {
            public Vector2 VelocityChange;
            public float ThrustChange;
            public float SpinChange;
        }

        public Vigil()
        {
            // Init graphics
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
            _DrawHandler = new DrawHandler(graphics);

            var playerShip = ShipManager.Instance.SpawnShip(ShipType.Corvette);
            ShipManager.Instance.SetPlayerShip(playerShip);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Init the Draw Handler
            _DrawHandler.Load(this);

            // Load all shiptype textures
            ShipTextureManager.Instance.Load(this);
            DebugFont = Content.Load<SpriteFont>("debug");
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
            Content.Unload();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            // Parse current keyboard state and update player speed
            ShipMovements shipMove = _InputHandler.Parse(out bool exit);
            if (exit)
                Exit();

            ShipManager.Instance.GetPlayerShip().UpdateMoves(shipMove);
            ShipManager.Instance.UpdatePositions();

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            _DrawHandler.Update(this);
            base.Draw(gameTime);
        }
    }
}
