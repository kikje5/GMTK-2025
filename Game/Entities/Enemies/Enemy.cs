using GMTK2025.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GMTK2025.Entities;

public class Enemy : Entity
{
	public Player Player;
	public int AttackFrames = 60;
	public int Damage = 1;
	public int attackRange = 10;
	protected bool CanAttack = true;
	protected int AttackCooldown = 0;
	public Enemy(Vector2 position, Player player, Texture2D texture) : base(position, texture)
	{
		Player = player;
	}

	public override void Update(GameTime gameTime)
	{
		if (CanAttack)
		{
			float DistanceToPlayer = Vector2.Distance(Position, Player.Position);
			if (DistanceToPlayer < attackRange) // Example attack range
			{
				Player.DoDamage(Damage);
				CanAttack = false;
			}
		}
		else
		{
			AttackCooldown++;
			if (AttackCooldown >= AttackFrames)
			{
				CanAttack = true;
				AttackCooldown = 0;
			}
		}

		// Example AI logic: Move towards the player
		Vector2 direction = Player.Position - Position;
		direction.Normalize();
		Velocity += direction * Acceleration;

		base.Update(gameTime);
	}
}