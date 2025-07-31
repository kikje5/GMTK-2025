using System;
using System.Collections.Generic;
using GMTK2025.RoomGeneration;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GMTK2025.Engine.UI;

public class Selector : UIElement
{
	private readonly SpriteFont font = App.AssetManager.Content.Load<SpriteFont>("Fonts/SimpleButtonFont");
	public List<NamedTexture> Icons { get; set; }
	public int SelectedIndex { get; set; }
	public Action OnChange { get; set; }
	public Selector(Vector2 position, List<NamedTexture> icons) : base(
			App.AssetManager.GetTexture("UI/Selector"), // normalTexture
			App.AssetManager.GetTexture("UI/Selector"), // hoverTexture
			App.AssetManager.GetTexture("UI/Selector"), // pressedTexture
			App.AssetManager.GetTexture("UI/Selector"), // disabledTexture
			position,
			new Vector2(128, 48))
	{
		Icons = icons;
	}

	public override void HandleInput(InputHelper inputHelper)
	{
		if (!inputHelper.MouseLeftButtonPressed)
		{
			return; //mouse button is not pressed
		}
		Vector2 mousePosition = inputHelper.MousePosition;
		if (mousePosition.X < Position.X - 64 || mousePosition.X > Position.X + 64 ||
			mousePosition.Y < Position.Y - 24 || mousePosition.Y > Position.Y + 24)
		{
			return; //mouse position is not in the selector
		}

		if (mousePosition.X < Position.X - 16)
		{
			SelectedIndex--;
		}
		else if (mousePosition.X > Position.X + 16)
		{
			SelectedIndex++;
		}
		if (SelectedIndex >= Icons.Count)
		{
			SelectedIndex = 0;
		}
		if (SelectedIndex < 0)
		{
			SelectedIndex = Icons.Count - 1;
		}
		OnChange?.Invoke();


	}

	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
	{
		//base.Draw(gameTime, spriteBatch);
		Texture2D texture = Icons[SelectedIndex].Texture;
		string name = Icons[SelectedIndex].Name;

		spriteBatch.Draw(currentTexture, new Rectangle((int)Position.X - 64, (int)Position.Y - 24, 128, 48), Color.White);

		spriteBatch.Draw(texture, new Rectangle((int)Position.X - 16, (int)Position.Y - 16, 32, 32), Color.White);
		spriteBatch.DrawString(font, name, Position + new Vector2(font.MeasureString(name).X / -2, 32), Color.White);

	}

}