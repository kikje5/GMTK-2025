using GMTK2025.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GMTK2025.Entities;

public class PrairieDog : Enemy
{
	public PrairieDog(Vector2 position, Player player) : base(position, player, App.AssetManager.GetTexture("Enemies/PrairieDog"))
	{
		Drag = 0.25f;
		AttackFrames = 10;
		Damage = 2;
	}
}