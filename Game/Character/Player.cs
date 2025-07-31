using System;
using GMTK2025.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace GMTK2025.Character;

public class Player : ILoopObject
{
	public Vector2 Position { get; set; }
	public Vector2 Size { get; set; }
	public Texture2D Texture { get; set; }
	public Texture2D SmallLassoTexture { get; set; } = App.AssetManager.GetTexture("Player/SmallLasso");
	public Texture2D LargeLassoTexture { get; set; } = App.AssetManager.GetTexture("Player/LargeLasso");
	public Texture2D LassoTexture { get; set; }
	public float Rotation { get; set; } = 0;
	public float MaxSpeed { get; set; }
	public Vector2 Velocity { get; set; }
	public float Acceleration { get; set; }
	public float Drag { get; set; }
	public int MaxHealth { get; set; }
	public int Health { get; set; }
	public bool IsThrowing { get; set; } = false;
	public bool CanThrow { get; set; } = true;
	private bool throwHasReachedMax = false;
	public int ThrowCharge { get; set; } = 0;
	public bool HasThrownLasso { get; set; } = false;
	public Rope Rope { get; set; }
	public bool LassoIsSmall
	{
		get => lassoIsSmall;
		set
		{
			lassoIsSmall = value;
			LassoTexture = value ? SmallLassoTexture : LargeLassoTexture;
		}
	}
	private bool lassoIsSmall = false;
	public Vector2 LassoPosition { get; set; }
	const float sqrt2 = 0.707106781185f;
	public Player(Vector2 position, Vector2 size, float maxSpeed, float acceleration, float drag, int maxHealth)
	{
		Position = position;
		Size = size;
		Texture = App.AssetManager.GetTexture("Player/Cowboy_hat");
		MaxSpeed = maxSpeed;
		Velocity = new Vector2(0, 0);
		Acceleration = acceleration;
		Drag = drag;
		MaxHealth = maxHealth;
		Health = maxHealth;
		Rope = new Rope(50, position);
	}

	public void Update(GameTime gameTime)
	{
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

		Rope.Update(Position);
	}

	public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
	{
		Rope.Draw(spriteBatch);
		spriteBatch.Draw(Texture, Position, null, Color.White, Rotation, new Vector2(Texture.Width / 2, Texture.Height / 2), 1f, SpriteEffects.None, 0f);
		if (HasThrownLasso)
		{
			Vector2 directionToLasso = LassoPosition - Position;
			float lassoRotation = (float)(Math.Atan2(directionToLasso.Y, directionToLasso.X) + Math.PI / 2);
			spriteBatch.Draw(LassoTexture, LassoPosition, null, Color.White, lassoRotation, new Vector2(LassoTexture.Width / 2, LassoTexture.Height / 2), 1f, SpriteEffects.None, 0f);
		}
	}

	public void HandleInput(InputHelper inputHelper)
	{
		bool Up = inputHelper.IsKeyDown(Keys.W) || inputHelper.IsKeyDown(Keys.Up);
		bool Down = inputHelper.IsKeyDown(Keys.S) || inputHelper.IsKeyDown(Keys.Down);
		bool Left = inputHelper.IsKeyDown(Keys.A) || inputHelper.IsKeyDown(Keys.Left);
		bool Right = inputHelper.IsKeyDown(Keys.D) || inputHelper.IsKeyDown(Keys.Right);
		int Y = 0;
		int X = 0;
		if (Up) Y -= 1;
		if (Down) Y += 1;
		if (Left) X -= 1;
		if (Right) X += 1;
		Vector2 direction = new Vector2(X, Y);

		if (X != 0 && Y != 0)
		{
			direction = new Vector2(sqrt2 * X, sqrt2 * Y);
		}

		Velocity += direction * Acceleration;

		//Look at mouse
		Vector2 mousePosition = inputHelper.MousePosition;
		Vector2 directionToMouse = mousePosition - Position;
		directionToMouse.Normalize();
		Rotation = (float)Math.Atan2(directionToMouse.Y, directionToMouse.X);


		if (inputHelper.MouseLeftButtonReleased && IsThrowing)
		{
			ThrowLasso(ThrowCharge, inputHelper);
			IsThrowing = false;
			ThrowCharge = 0;
		}
		HandleThrowCharge();

		if (inputHelper.MouseLeftButtonPressed && CanThrow)
		{
			IsThrowing = true;
			throwHasReachedMax = false;
			HasThrownLasso = false;
		}
	}

	private void HandleThrowCharge()
	{
		if (IsThrowing)
		{
			if (!throwHasReachedMax)
			{
				ThrowCharge += 2;
				if (ThrowCharge >= 100)
				{
					throwHasReachedMax = true;
				}
			}
			else
			{
				ThrowCharge -= 1;
				if (ThrowCharge <= 0)
				{
					IsThrowing = false;
					ThrowCharge = 0;
				}
			}
		}
	}

	public void Reset()
	{
	}

	private void ThrowLasso(int charge, InputHelper inputHelper)
	{
		// Implement lasso throwing logic here
		Console.WriteLine("Throwing lasso with charge: " + charge);
		LassoIsSmall = charge > 65;
		Vector2 directionToMouse = inputHelper.MousePosition - Position;
		directionToMouse.Normalize();
		Vector2 lassoOffset = directionToMouse * charge * (1 + (charge / 25));

		LassoPosition = new Vector2(Position.X + lassoOffset.X, Position.Y + lassoOffset.Y);
		HasThrownLasso = true;
	}

}
