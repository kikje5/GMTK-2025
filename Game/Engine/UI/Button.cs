using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GMTK2025.Engine.UI;

public class Button : UIElement
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
	public Button(Vector2 position, Vector2 size) : base(
		App.AssetManager.GetTexture("UI/Buttons/SimpleButtonNormal"),
		App.AssetManager.GetTexture("UI/Buttons/SimpleButtonHover"),
		App.AssetManager.GetTexture("UI/Buttons/SimpleButtonPressed"),
		App.AssetManager.GetTexture("UI/Buttons/SimpleButtonDisabled"),
		position - size / 2,
		size
		)
	{
		_text = new TextElement("Fonts/SimpleButtonFont");

	}

	public override void HandleInput(InputHelper inputHelper)
	{
		if (UIElementState == UIElementMouseState.Disabled) return;
		Vector2 mousePosition = inputHelper.MousePosition;
		int mouseX = (int)mousePosition.X;
		int mouseY = (int)mousePosition.Y;
		if (mouseX >= CollisionRectangle.X && mouseX <= CollisionRectangle.X + CollisionRectangle.Width &&
			mouseY >= CollisionRectangle.Y && mouseY <= CollisionRectangle.Y + CollisionRectangle.Height)
		{
			if (inputHelper.MouseLeftButtonPressed)
			{
				isClicked = true;
				UIElementState = UIElementMouseState.Pressed;
			}
			else if (inputHelper.MouseLeftButtonReleased && isClicked)
			{
				isClicked = false;
				OnClicked();
			}
			else if (inputHelper.MouseLeftButtonDown)
			{
				UIElementState = UIElementMouseState.Pressed;
			}
			else
			{
				isClicked = false;
				UIElementState = UIElementMouseState.Hover;
			}
		}
		else
		{
			isClicked = false;
			UIElementState = UIElementMouseState.Normal;
		}
	}

	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
	{
		base.Draw(gameTime, spriteBatch);
		_text.Draw(gameTime, spriteBatch);
	}

	protected override void OnClicked()
	{
		base.OnClicked();
		App.AudioManager.PlaySoundEffect("Click");
		//App.AudioManager.PlaySoundEffect(AudioManager.ButtonClickSFX);
	}
}