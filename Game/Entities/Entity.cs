using System;
using GMTK2025.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace GMTK2025.Entities;

public class Entity : ILoopObject
{
	public Vector2 Position { get; set; }
	public Vector2 Size { get; set; } = new Vector2(32, 32);
	public Texture2D Texture { get; set; }
	public float Rotation { get; set; } = 0;
	public float MaxSpeed { get; set; } = 10;
	public Vector2 Velocity { get; set; } = new Vector2(0, 0);
	public float Acceleration { get; set; } = 1;
	public float Drag { get; set; } = 0.15f;
	public int MaxHealth { get; set; } = 100;
	public int Health { get; set; }
	public bool IsAlive => Health > 0;

	public Entity(Vector2 position, Texture2D texture)
	{
		Position = position;
		Texture = texture;
		Health = MaxHealth;
	}

	public virtual void Update(GameTime gameTime)
	{
		if (!IsAlive)
		{
			return; // Do not update if the entity is not alive
		}

		// Clamp velocity to max speed
		if (Velocity.X * Velocity.X + Velocity.Y * Velocity.Y > MaxSpeed * MaxSpeed)
		{
			Velocity.Normalize();
			Velocity *= MaxSpeed;
		}
		Velocity *= 1 - Drag; // Apply drag

		if (Math.Abs(Velocity.X) < 0.01f)
		{
			Velocity = new Vector2(0, Velocity.Y);
		}
		if (Math.Abs(Velocity.Y) < 0.01f)
		{
			Velocity = new Vector2(Velocity.X, 0);
		}

		// Update position
		Position += Velocity;

		//keep inside screen bounds
		int Width = 1920;
		int Height = 1080;
		if (Position.X - Size.X < 0)
		{
			Position = new Vector2(Size.X, Position.Y);
		}
		if (Position.X + Size.X > Width)
		{
			Position = new Vector2(Width - Size.X, Position.Y);
		}
		if (Position.Y - Size.Y < 0)
		{
			Position = new Vector2(Position.X, Size.Y);
		}
		if (Position.Y + Size.Y > Height)
		{
			Position = new Vector2(Position.X, Height - Size.Y);
		}
	}

	public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, new Vector2(Texture.Width / 2, Texture.Height / 2), 1f, SpriteEffects.None, 0f);
	}

	public virtual void HandleInput(InputHelper inputHelper) { }
	public virtual void Reset()
	{
		Health = MaxHealth;
	}

}