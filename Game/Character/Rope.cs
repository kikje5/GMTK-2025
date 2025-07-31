using System;
using GMTK2025.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GMTK2025.Character;

public class Rope
{
	public Vector2[] Positions; // Positions[0] is the player's position, Positions[1] is the first segment, etc.

	public int Length => Positions.Length;

	private readonly Texture2D texture = App.AssetManager.GetTexture("Player/Rope");

	public Rope(int length, Vector2 playerPosition)
	{
		Positions = new Vector2[length];
		for (int i = 0; i < length; i++)
		{
			Positions[i] = new Vector2(playerPosition.X, playerPosition.Y + i * 8);
		}
	}

	public void Update(Vector2 playerPosition)
	{
		Positions[0] = playerPosition;

		for (int i = 1; i < Length; i++)
		{
			Vector2 previousSegment = Positions[i - 1];
			Vector2 currentSegment = Positions[i];

			Vector2 difference = currentSegment - previousSegment;
			difference.Normalize();
			difference *= 8; // Distance between segments

			Positions[i] = previousSegment + difference;
		}
	}

	public void Draw(SpriteBatch spriteBatch)
	{
		for (int i = 1; i < Length; i++)
		{
			Vector2 previousSegment = Positions[i - 1];
			Vector2 currentSegment = Positions[i];
			Vector2 nextSegment;
			if (i == Length - 1)
			{
				nextSegment = currentSegment;
			}
			else
			{
				nextSegment = Positions[i + 1];
			}
			Vector2 direction = nextSegment - previousSegment;
			float rotation = (float)Math.Atan2(direction.Y, direction.X);

			spriteBatch.Draw(texture, currentSegment, null, Color.White, rotation, new Vector2(texture.Width / 2, texture.Height / 2), 1f, SpriteEffects.None, 0f);
		}
	}
}
