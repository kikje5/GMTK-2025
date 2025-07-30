using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GMTK2025.LevelGeneration;

public class NamedTexture
{
	public string Name { get; }
	public Texture2D Texture { get; }

	public NamedTexture(string name, Texture2D texture)
	{
		Name = name;
		Texture = texture;
	}
}