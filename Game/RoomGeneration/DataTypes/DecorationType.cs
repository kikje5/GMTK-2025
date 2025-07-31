using Microsoft.Xna.Framework.Graphics;

namespace GMTK2025.RoomGeneration
{
    public class DecorationType
    {
        public string Name { get; }
        public Texture2D Texture { get; }

        public DecorationType(string name, Texture2D texture)
        {
            Name = name;
            Texture = texture;
        }
    }
}