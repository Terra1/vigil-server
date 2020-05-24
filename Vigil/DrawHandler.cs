using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Vigil
{
    class DrawHandler
    {
        SpriteBatch _Sprites;
        Vector2 _CameraPosition;
        GraphicsDeviceManager _GDM;
        Texture2D _Background;
        public DrawHandler(GraphicsDeviceManager GDM)
        {
            _GDM = GDM;
            _Sprites = new SpriteBatch(_GDM.GraphicsDevice);
        }
        public void Load(Vigil game)
        {
            _Background = game.Content.Load<Texture2D>("background");
        }
        public void Update(Vigil game)
        {
            _GDM.GraphicsDevice.Clear(Color.TransparentBlack);

            var ShipMgrInstance = ShipManager.Instance;
            var PlayerShip = ShipMgrInstance.GetPlayerShip();

            //This will move our camera
            ScrollCamera(_Sprites.GraphicsDevice.Viewport, ShipMgrInstance.GetShipPosition(PlayerShip));

            //We now must get the center of the screen
            Vector2 Origin = new Vector2(_Sprites.GraphicsDevice.Viewport.Width / 2.0f, _Sprites.GraphicsDevice.Viewport.Height / 2.0f);

            //Now the matrix, It will hold the position, and Rotation/Zoom for advanced features
            Matrix cameraTransform = Matrix.CreateTranslation(new Vector3(-_CameraPosition, 0.0f)) *
               Matrix.CreateTranslation(new Vector3(-Origin, 0.0f)) *
               // Matrix.CreateRotationZ(rot) * //Add Rotation
               // Matrix.CreateScale(zoom, zoom, 1) * //Add Zoom
               Matrix.CreateTranslation(new Vector3(Origin, 0.0f)); //Add Origin

            //Now we can start to draw with our camera, using the Matrix overload
            _Sprites.Begin(SpriteSortMode.Immediate, BlendState.AlphaBlend, SamplerState.LinearClamp, DepthStencilState.Default,
                                     RasterizerState.CullCounterClockwise, null, cameraTransform);

            _Sprites.Draw(_Background, new Vector2(0, 0), new Rectangle(0, 0, _Background.Width, _Background.Height), Color.White, 0.0f, new Vector2(0, 0), 1.0f, SpriteEffects.None, 1.0f);

            foreach (var shipPos in ShipMgrInstance.GetShipPositions())
            {
                Texture2D texture = ShipTextureManager.Instance.GetTexture(shipPos.Key.GetShipType());
                Vector2 location = shipPos.Value;
                Rectangle sourceRectangle = new Rectangle(0, 0, texture.Width, texture.Height);
                float angle = shipPos.Key.GetAngle();
                Vector2 origin = new Vector2(texture.Width / 2, texture.Height / 2);

                _Sprites.Draw(texture, location, sourceRectangle, Color.White, angle, origin, 1.0f, SpriteEffects.None, 0.0f);
            }

            _Sprites.End(); //End the camera spritebatch
                            // After this you can make another spritebatch without a camera to draw UI and things that will not move

            _Sprites.Begin();
            _Sprites.DrawString(game.DebugFont, "Velocity: " + PlayerShip.GetVelocity().ToString(), new Vector2(20, 20), Color.White);
            _Sprites.DrawString(game.DebugFont, "Thrust: " + PlayerShip.GetThrust().ToString(), new Vector2(20, 40), Color.White);
            _Sprites.DrawString(game.DebugFont, "Spin: " + PlayerShip.GetSpin().ToString(), new Vector2(20, 60), Color.White);
            _Sprites.DrawString(game.DebugFont, "Angle: " + PlayerShip.GetAngle().ToString(), new Vector2(20, 80), Color.White);
            _Sprites.DrawString(game.DebugFont, "Coordinates: " + ShipMgrInstance.GetShipPosition(PlayerShip), new Vector2(20, 100), Color.White);
            _Sprites.End();
        }

        private void ScrollCamera(Viewport viewport, Vector2 PlayerPosition )
        {
            //Add to the camera positon, So we can see the origin
            _CameraPosition.X = _CameraPosition.X + (viewport.Width / 2);
            _CameraPosition.Y = _CameraPosition.Y + (viewport.Height / 2);

            //Smoothly move the camera towards the player
            _CameraPosition.X = MathHelper.Lerp(_CameraPosition.X, PlayerPosition.X, 0.1f);
            _CameraPosition.Y = MathHelper.Lerp(_CameraPosition.Y, PlayerPosition.Y, 0.1f);
            //Undo the origin because it will be calculated with the Matrix (I know this isnt the best way but its what I had real quick)
            _CameraPosition.X = _CameraPosition.X - (viewport.Width / 2);
            _CameraPosition.Y = _CameraPosition.Y - (viewport.Height / 2);

            //Shake the camera, Use the mouse to scroll or anything like that, add it here (Ex, Earthquakes)

            //Round it, So it dosent try to draw in between 2 pixels
            _CameraPosition.Y = (float)Math.Round(_CameraPosition.Y);
            _CameraPosition.X = (float)Math.Round(_CameraPosition.X);

            //Clamp it off, So it stops scrolling near the edges
            _CameraPosition.X = MathHelper.Clamp(_CameraPosition.X, 1f, viewport.Width * 2048);
            _CameraPosition.Y = MathHelper.Clamp(_CameraPosition.Y, 1f, viewport.Height * 2048);
        }
    }
}
