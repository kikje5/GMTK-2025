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
	private readonly Texture2D HealthBarBackgroundTexture = App.AssetManager.GetTexture("Enemies/HealthBarBackground");
	private readonly Texture2D HealthBarOverlayTexture = App.AssetManager.GetTexture("Enemies/HealthBarOverlay");
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
				Player.TakeDamage(Damage);
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

	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
	{
		base.Draw(gameTime, spriteBatch);
		DrawHealthBar(spriteBatch);
	}

	public void TakeDamage(int damage)
	{
		Health -= damage;
	}

	private void DrawHealthBar(SpriteBatch spriteBatch)
	{
		Vector2 healthBarPosition = Position + new Vector2(-Texture.Width / 2, -Texture.Height / 2 - 4);
		//draw Background
		spriteBatch.Draw(HealthBarBackgroundTexture, healthBarPosition, Color.White);
		//draw Overlay
		spriteBatch.Draw(HealthBarOverlayTexture, healthBarPosition, new Rectangle(0, 0, (int)(HealthBarOverlayTexture.Width * (Health / 100f)), HealthBarOverlayTexture.Height), Color.Red);


	}
}