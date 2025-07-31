using Microsoft.Xna.Framework;
using GMTK2025.Engine;
namespace GMTK2025.RoomGeneration;

public class Decoration
{
    public DecorationType Type { get; }
    public Vector2 Position { get; set; }
    public int Size { get; set; }

    public Decoration(DecorationType type, Vector2Int position, int size)
    {
        Type = type;
        Position = new Vector2(position.X, position.Y);
        Size = size;
    }
    public Decoration(DecorationType type, Vector2 position, int size)
    {
        Type = type;
        Position = position;
        Size = size;
    }
}
