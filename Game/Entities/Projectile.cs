using System;
using GMTK2025.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GMTK2025.Entities;

public class Projectile : Entity
{
	public Vector2 Direction;
	public float Speed;
	public int Damage;

	public Projectile(Vector2 position, Vector2 direction, float speed, int damage, Texture2D texture)
		: base(position, texture)
	{
		Direction = direction;
		Speed = speed;
		Damage = damage;
		Rotation = (float)Math.Atan2(direction.Y, direction.X);
	}

	public override void Update(GameTime gameTime)
	{
		Position += Direction * Speed;
	}
}