using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GMTK2025.Engine;

namespace GMTK2025.Engine.UI;

public class Bar : ILoopObject
{
	public Vector2 Position { get; set; }
	public Vector2 Size { get; set; }
	public float Value { get; set; }
	public float MaxValue { get; set; }
	public Color Color { get; set; }
	private readonly Texture2D outsideTexture;
	private readonly Texture2D insideTexture;

	public Bar(Vector2 position, Vector2 size, float maxValue, Color color)
	{
		Position = position;
		Size = size;
		MaxValue = maxValue;
		Value = maxValue;
		Color = color;
		outsideTexture = App.AssetManager.GetTexture("UI/BarOutside");
		insideTexture = App.AssetManager.GetTexture("UI/BarInside");
	}

	public void Update(GameTime gameTime)
	{
		// Update logic for the bar can be added here if needed
	}

	public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
	{
		// Draw the outside of the bar
		spriteBatch.Draw(outsideTexture, new Rectangle((int)Position.X, (int)Position.Y, (int)Size.X, (int)Size.Y), Color.White);

		// Calculate the width of the inside bar based on the current value
		float insideWidth = (Value / MaxValue) * Size.X / 1.125f;

		// Draw the inside of the bar
		spriteBatch.Draw(insideTexture, new Rectangle((int)(Position.X + (Size.X * 0.0625f)), (int)(Position.Y + (Size.Y * 0.0625f)), (int)insideWidth, (int)(Size.Y / 1.125f)), Color);
	}

	public void HandleInput(InputHelper inputHelper) { }

	public void Reset() { }
}