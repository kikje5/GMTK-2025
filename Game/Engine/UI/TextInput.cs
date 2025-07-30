using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace GMTK2025.Engine.UI;

public class TextInput : UIElement
{
	private TextElement _text;

	public string Text
	{
		get { return _text.Text; }
		set
		{
			_text.Text = value;
			_text.Position = Position + Size / 2;
		}
	}

	public TextInput(Vector2 position, Vector2 size) : base(
		App.AssetManager.GetTexture("UI/TextInput/SimpleTextInputNormal"),
		App.AssetManager.GetTexture("UI/TextInput/SimpleTextInputHover"),
		App.AssetManager.GetTexture("UI/TextInput/SimpleTextInputPressed"),
		App.AssetManager.GetTexture("UI/TextInput/SimpleTextInputDisabled"),
		position - size / 2,
		size
		)
	{ _text = new TextElement("Fonts/TextInputFont"); }

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

		if (key == Keys.Back)
		{
			if (_text.Text.Length > 0)
			{
				_text.Text = _text.Text.Substring(0, _text.Text.Length - 1);
			}
			return;
		}
		if (_text.Text.Length >= 40) return;

		if (key == Keys.Space)
		{
			_text.Text += " ";
			return;
		}

		//letters
		if ((int)key < 65 || (int)key > 90) return;

		if (inputHelper.ShiftKeyDown)
		{
			_text.Text += key.ToString();
		}
		else
		{
			_text.Text += key.ToString().ToLower();
		}
	}
	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
	{
		base.Draw(gameTime, spriteBatch);
		_text.Draw(gameTime, spriteBatch);
	}
}