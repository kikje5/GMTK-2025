using System;
using GMTK2025.Engine;
using GMTK2025.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
namespace GMTK2025.Entities;

public class Player : Entity
{
	public Texture2D SmallLassoTexture { get; set; } = App.AssetManager.GetTexture("Player/SmallLasso");
	public Texture2D LargeLassoTexture { get; set; } = App.AssetManager.GetTexture("Player/LargeLasso");
	public Texture2D SmallLassoHologramTexture { get; set; } = App.AssetManager.GetTexture("Player/SmallLassoHologram");
	public Texture2D LargeLassoHologramTexture { get; set; } = App.AssetManager.GetTexture("Player/LargeLassoHologram");
	public Texture2D DamageCircleTexture { get; set; } = App.AssetManager.GetTexture("Points/DamageCircle");
	public Vector2 DamageCirclePosition { get; set; } = Vector2.Zero;
	public int DamageCircleRadius
	{
		get => damageCircleRadius;
		set
		{
			damageCircleRadius = value;
			DamageCircleCountdown = 30;
		}
	}
	private int damageCircleRadius;
	public int DamageCircleCountdown { get; set; } = 0;
	public Texture2D LassoTexture { get; set; }
	public bool IsThrowing { get; set; } = false;
	public bool CanThrow { get; set; } = true;
	private bool throwHasReachedMax = false;
	public int ThrowCharge { get; set; } = 0;
	public bool HasThrownLasso
	{
		get => hasThrownLasso;
		set
		{
			hasThrownLasso = value;
			if (value == true)
			{
				LassoCountDown = 30;
			}
		}
	}
	private bool hasThrownLasso = false;
	private int LassoCountDown { get; set; } = 0;
	public Rope Rope { get; set; }
	public int RopeCircleCountdown = 0;
	public bool LassoIsSmall
	{
		get => lassoIsSmall;
		set
		{
			lassoIsSmall = value;
			LassoTexture = value ? SmallLassoTexture : LargeLassoTexture;
		}
	}
	private bool lassoIsSmall;
	public Vector2 LassoPosition { get; set; }
	public int CurrentExperience
	{
		get => _currentExperience;
		set
		{
			_currentExperience = value;
			if (_currentExperience >= ExperienceNeededForNextLevel)
			{
				Level++;
				_currentExperience = 0;
				ExperienceNeededForNextLevel += 50;
				HasJustLeveledUp = true;
			}
		}
	}
	private int _currentExperience = 0;
	public int ExperienceNeededForNextLevel { get; set; } = 100;
	public int Level { get; set; } = 1;
	public bool HasJustLeveledUp { get; set; } = false;
	const float sqrt2 = 0.707106781185f;
	public Player(Vector2 position) : base(position, App.AssetManager.GetTexture("Player/Cowboy_hat"))
	{
		Rope = new Rope(60, position);
		LassoIsSmall = false;
	}

	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);

		Rope.Update(Position);

		CheckRopeLoop();

		if (DamageCircleCountdown <= 0)
		{
			DamageCircleRadius = 0;
		}
		else
		{
			DamageCircleCountdown--;
		}

		if (LassoCountDown <= 0)
		{
			HasThrownLasso = false;
		}
		else
		{
			LassoCountDown--;
		}
	}

	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
	{
		Rope.Draw(spriteBatch);
		base.Draw(gameTime, spriteBatch);
		if (HasThrownLasso)
		{
			Vector2 directionToLasso = LassoPosition - Position;
			float lassoRotation = (float)(Math.Atan2(directionToLasso.Y, directionToLasso.X) + Math.PI / 2);
			spriteBatch.Draw(LassoTexture, LassoPosition, null, Color.White, lassoRotation, new Vector2(LassoTexture.Width / 2, LassoTexture.Height / 2), 1f, SpriteEffects.None, 0f);
		}
		if (DamageCircleRadius > 0)
		{
			spriteBatch.Draw(DamageCircleTexture, DamageCirclePosition, null, Color.White, 0, new Vector2(DamageCircleTexture.Width / 2, DamageCircleTexture.Height / 2), DamageCircleRadius / (float)DamageCircleTexture.Width, SpriteEffects.None, 0f);
		}
		if (IsThrowing)
		{
			DrawLassoHologram(spriteBatch);
		}
	}

	public override void HandleInput(InputHelper inputHelper)
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

	public void TakeDamage(int damage)
	{
		Health -= damage;
		if (Health <= 0)
		{
			// Handle player death
			Console.WriteLine("Player has died.");
			App.ScreenManager.SwitchTo(ScreenManager.TITLE_SCREEN);
		}
	}

	private void ThrowLasso(float charge, InputHelper inputHelper)
	{
		// Implement lasso throwing logic here
		LassoIsSmall = charge > 65;
		Vector2 directionToMouse = inputHelper.MousePosition - Position;
		directionToMouse.Normalize();
		Vector2 lassoOffset = directionToMouse * charge * (1 + (charge / 25));

		LassoPosition = new Vector2(Position.X + lassoOffset.X, Position.Y + lassoOffset.Y);
		DealDamage(LassoPosition, LassoIsSmall ? 32 : 64, 50);
		HasThrownLasso = true;
	}

	public void DealDamage(Vector2 position, int Range, int damage)
	{
		DamageCirclePosition = position;
		DamageCircleRadius = Range;
		EnemyManager.Instance.DamageEnemiesInRadius(position, Range, damage);
	}

	private void CheckRopeLoop()
	{
		if (RopeCircleCountdown > 0)
		{
			RopeCircleCountdown--;
			return;
		}
		int intersectionIndex = GetPlayerRopeIntersection();
		if (intersectionIndex == -1)
		{
			return;
		}
		RopeCircleCountdown = 30;

		int halfwayIndex = intersectionIndex / 2;
		int quarterIndex = intersectionIndex / 4;
		int threeQuarterIndex = intersectionIndex - quarterIndex;
		float halfwayDistance = Vector2.Distance(Rope.Positions[halfwayIndex], Rope.Positions[intersectionIndex]);
		float quarterDistance = Vector2.Distance(Rope.Positions[quarterIndex], Rope.Positions[threeQuarterIndex]);
		bool SmallestIsQuarter = quarterDistance < halfwayDistance;
		int SmallestRange = (int)Math.Min(halfwayDistance, quarterDistance);

		if (SmallestRange < 24)
		{
			return;
		}

		Vector2 midPoint;
		if (SmallestIsQuarter)
		{
			midPoint = (Rope.Positions[quarterIndex] + Rope.Positions[threeQuarterIndex]) / 2;
		}
		else
		{
			midPoint = (Rope.Positions[halfwayIndex] + Rope.Positions[intersectionIndex]) / 2;
		}

		DealDamage(midPoint, SmallestRange, 100);




	}

	private int GetPlayerRopeIntersection()
	{
		for (int i = 8; i < Rope.Length; i++)
		{
			Vector2 segmentPosition = Rope.Positions[i];
			if (Vector2.Distance(segmentPosition, Position) < 12)
			{
				return i;
			}
		}
		return -1;
	}

	private void DrawLassoHologram(SpriteBatch spriteBatch)
	{
		float charge = ThrowCharge;
		bool smallHologram = charge > 65;
		Vector2 directionToMouse = App.InputHelper.MousePosition - Position;
		directionToMouse.Normalize();
		Vector2 lassoOffset = directionToMouse * charge * (1 + (charge / 25));

		Vector2 hologramPosition = new Vector2(Position.X + lassoOffset.X, Position.Y + lassoOffset.Y);
		Texture2D hologramTexture = smallHologram ? SmallLassoHologramTexture : LargeLassoHologramTexture;

		spriteBatch.Draw(hologramTexture, hologramPosition, null, Color.White, 0, new Vector2(LassoTexture.Width / 2, LassoTexture.Height / 2), 1f, SpriteEffects.None, 0f);
	}

}
