using GMTK2025.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GMTK2025.Entities;

public class Wolf : Enemy
{
	public Wolf(Vector2 position, Player player) : base(position, player, App.AssetManager.GetTexture("Enemies/Wolf"))
	{
		Drag = 0.2f;
		Damage = 5;
		AttackFrames = 30;
		MaxHealth = 150;
		Health = MaxHealth;
	}
}