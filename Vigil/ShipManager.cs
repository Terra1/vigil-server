using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vigil
{
    using ShipPosition = KeyValuePair<Ship, Vector2>;
    using ShipTexture = KeyValuePair<ShipType, Texture2D>;
    public enum ShipType { Frigate, Corvette, Battleship, Carrier, Titan }
    public sealed class ShipTextureManager
    {
        private static readonly Lazy<ShipTextureManager> lazy = new Lazy<ShipTextureManager>(() => new ShipTextureManager());
        private ShipTextureManager()
        {
        }
        public static ShipTextureManager Instance { get { return lazy.Value; } }
        private List<ShipTexture> _ShipTextures = new List<ShipTexture>();
        public void Load(Vigil game)
        {
            foreach (ShipType type in Enum.GetValues(typeof(ShipType)).Cast<ShipType>())
            {
                string textureName = type.ToString();
                Texture2D texture;
                try
                {
                    texture = game.Content.Load<Texture2D>(textureName);
                }
                catch
                {
                    continue;
                }
                _ShipTextures.Add(new ShipTexture(type, texture));
            }
        }
        public Texture2D GetTexture( ShipType type )
        {
            return _ShipTextures.Find(item => item.Key == type).Value;
        }
    }
    public sealed class ShipManager
    {
        private static readonly Lazy<ShipManager> lazy = new Lazy<ShipManager>(() => new ShipManager());
        private ShipManager()
        {
        }
        public static ShipManager Instance { get { return lazy.Value; } }
        private List<Ship> _Ships = new List<Ship>();
        private List<ShipPosition> _ShipPositions = new List<ShipPosition>();
        Ship _PlayerShip;
        public Ship SpawnShip( ShipType type )
        {
            Ship newShip = new Ship( type );
            _Ships.Add(newShip);
            ShipPosition newPos = new ShipPosition(newShip, new Vector2(0, 0));
            _ShipPositions.Add(newPos);
            return newShip;
        }
        public void SetPlayerShip( Ship ship )
        {
            _PlayerShip = ship;

            ShipPosition oldPos = _ShipPositions.Find(item => item.Key == ship);
            Vector2 position = new Vector2(1024, 1024);
                
                /* new Vector2(
                    Vigil.graphics.GraphicsDevice.Viewport.Width / 2 - 64,
                    Vigil.graphics.GraphicsDevice.Viewport.Height / 2 - 64
                ); */
            ShipPosition newPos = new ShipPosition(oldPos.Key, position);
            _ShipPositions.Remove(oldPos);
            _ShipPositions.Add(newPos);
        }
        public void UpdatePositions()
        {
            foreach(Ship ship in _Ships)
            {
                ShipPosition oldPos = _ShipPositions.Find(item => item.Key == ship);
                ShipPosition newPos = new ShipPosition(oldPos.Key, oldPos.Value + oldPos.Key.GetVelocity());
                _ShipPositions.Remove(oldPos);
                _ShipPositions.Add(newPos);
            }
        }
        public Ship GetPlayerShip()
        {
            return _PlayerShip;
        }
        public List<ShipPosition> GetShipPositions()
        {
            return _ShipPositions;
        }
        public Vector2 GetShipPosition( Ship ship )
        {
            return _ShipPositions.Where( x => x.Key == ship ).ToList().FirstOrDefault().Value;
        }
        public bool DestroyShip( Ship ship )
        {
            _ShipPositions.RemoveAll(item => item.Key == ship);
            return _Ships.Remove(ship);
        }
    }
}
