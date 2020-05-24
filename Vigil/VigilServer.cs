using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace Vigil
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    public class VigilServer : Game
    {
        public class ShipMovements
        {
            public Vector2 VelocityChange { get; set; } = new Vector2( 0, 0 );
            public float ThrustChange { get; set; } = 0f;
            public float SpinChange { get; set; } = 0f;
        }

        public VigilServer() {}

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            var playerShip = ShipManager.Instance.SpawnShip(ShipType.Corvette);
            ShipManager.Instance.SetPlayerShip(playerShip);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent() {}

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

            ShipMovements shipMove = new ShipMovements();
            bool exit = false;

            // TODO MIKAEL: This needs to be handled
            // ShipMovements shipMove = _InputHandler.Parse(out bool exit);
            if (exit)
                Exit();

            ShipManager.Instance.GetPlayerShip().UpdateMoves(shipMove);
            ShipManager.Instance.UpdatePositions();

            base.Update(gameTime);
        }
    }
}
