using Microsoft.Xna.Framework.Graphics;

namespace GMTK2025.RoomGeneration
{
    public abstract class Tile
    {
        public string Name { get; }
        public Texture2D Texture { get; }
        public Tile(string name, Texture2D texture)
        {
            Name = name;
            Texture = texture;
        }
    }
}