using Microsoft.Xna.Framework.Graphics;

namespace GMTK2025.LevelGeneration
{
    public class DecorationType
    {
        public string Name { get; }
        public Texture2D Texture { get; }

        public DecorationType(string name, Texture2D texture, bool hasCollision)
        {
            Name = name;
            Texture = texture;
        }
    }
}