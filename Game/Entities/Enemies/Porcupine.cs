using System;
using System.Collections.Generic;
using GMTK2025.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GMTK2025.Entities;

public class Porcupine : Enemy
{
	public int FiringRange = 400;
	public bool IsAttacking = false;

	public List<Projectile> Needles = new List<Projectile>();
	public int NeedleSpeed = 5;
	private readonly Texture2D needleTexture = App.AssetManager.GetTexture("Enemies/PorcupineNeedle");

	public Porcupine(Vector2 position, Player player) : base(position, player, App.AssetManager.GetTexture("Enemies/Porcupine"))
	{
		Drag = 0.25f;
		AttackFrames = 60;
		Damage = 10;
	}

	public override void Update(GameTime gameTime, Enemy[] enemies)
	{
		float DistanceToPlayer = Vector2.Distance(Position, Player.Position);
		if (DistanceToPlayer < FiringRange / 2)
		{
			IsAttacking = true;
		}
		else if (DistanceToPlayer >= FiringRange)
		{
			IsAttacking = false;
		}

		if (IsAttacking)
		{
			Velocity = Vector2.Zero;
			if (CanAttack)
			{
				if (DistanceToPlayer < FiringRange)
				{
					ShootNeedle();
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
		}
		else
		{
			base.Update(gameTime, enemies);
		}

		// Update needles
		for (int i = Needles.Count - 1; i >= 0; i--)
		{
			Needles[i].Update(gameTime);
			if (Vector2.Distance(Needles[i].Position, Position) > FiringRange)
			{
				Needles.RemoveAt(i);
			}
		}

		//Check if needles hit the player
		for (int i = Needles.Count - 1; i >= 0; i--)
		{
			if (Vector2.Distance(Needles[i].Position, Player.Position) < 16)
			{
				Player.TakeDamage(Needles[i].Damage);
				Needles.RemoveAt(i); // Remove needle after hitting the player
			}
		}
	}

	private void ShootNeedle()
	{
		Vector2 direction = Player.Position - Position;
		direction.Normalize();
		Projectile needle = new Projectile(Position, direction, NeedleSpeed, Damage, needleTexture);
		Needles.Add(needle);
	}

	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
	{
		base.Draw(gameTime, spriteBatch);
		foreach (var needle in Needles)
		{
			needle.Draw(gameTime, spriteBatch);
		}
	}
}