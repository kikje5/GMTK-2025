using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GMTK2025.Engine.UI;

public class IntegerInput : UIElement
{
	private TextElement _text;
	private int _integer;

	public Action OnIntegerChanged;

	public int Integer
	{
		get { return _integer; }
		set
		{
			_text.Text = value.ToString();
			_integer = value;
			_text.Position = Position + Size / 2;
			OnIntegerChanged?.Invoke();
		}
	}

	public IntegerInput(Vector2 position, Vector2 size) : base(
		App.AssetManager.GetTexture("UI/Buttons/SimpleButtonNormal"),
		App.AssetManager.GetTexture("UI/Buttons/SimpleButtonHover"),
		App.AssetManager.GetTexture("UI/Buttons/SimpleButtonPressed"),
		App.AssetManager.GetTexture("UI/Buttons/SimpleButtonDisabled"),
		position - size / 2,
		size
		)
	{ _text = new TextElement("Fonts/SimpleButtonFont"); }

	public override void HandleInput(InputHelper inputHelper)
	{
		if (UIElementState == UIElementMouseState.Disabled) return;
		if (inputHelper.MouseLeftButtonPressed)
		{
			Vector2 mousePosition = inputHelper.MousePosition;
			int mouseX = (int)mousePosition.X;
			int mouseY = (int)mousePosition.Y;
			if (mouseX >= CollisionRectangle.X && mouseX <= CollisionRectangle.X + CollisionRectangle.Width &&
				mouseY >= CollisionRectangle.Y && mouseY <= CollisionRectangle.Y + CollisionRectangle.Height)
			{
				isClicked = true;
				UIElementState = UIElementMouseState.Pressed;
			}
			else
			{
				isClicked = false;
				UIElementState = UIElementMouseState.Normal;
			}
		}

		if (!isClicked) return;

		//if it is clicked, do text input logic

		inputHelper.GetKeyPressed(out Keys key);

		if (key == Keys.None) return;
		int keyValue = (int)key;

		Console.WriteLine($"Key pressed: {key}");

		if (key == Keys.Back)
		{
			if (_text.Text.Length > 0)
			{
				Integer /= 10;
			}
			return;
		}
		if (_text.Text.Length >= 8) return;

		int digit = keyValue - 48;
		if (digit < 0 || digit > 9) return;
		Integer = Integer * 10 + digit;
	}
	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
	{
		base.Draw(gameTime, spriteBatch);
		_text.Draw(gameTime, spriteBatch);
	}
}