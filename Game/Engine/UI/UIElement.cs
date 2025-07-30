using GMTK2025.Engine;
using Microsoft.Xna.Framework.Graphics;
using System;
using Microsoft.Xna.Framework;

namespace GMTK2025.Engine.UI;

public enum UIElementMouseState
{
	Disabled = 0,
	Normal = 1,
	Hover = 2,
	Pressed = 3
}

public abstract class UIElement : ILoopObject
{
	protected Texture2D disabledTexture;
	protected Texture2D normalTexture;
	protected Texture2D hoverTexture;
	protected Texture2D pressedTexture;
	protected Texture2D currentTexture;
	public UIElementMouseState UIElementState
	{
		get
		{
			return uiElementMouseState;
		}
		set
		{
			uiElementMouseState = value;
			switch (uiElementMouseState)
			{
				case UIElementMouseState.Disabled:
					currentTexture = disabledTexture;
					break;
				case UIElementMouseState.Normal:
					currentTexture = normalTexture;
					break;
				case UIElementMouseState.Hover:
					currentTexture = hoverTexture;
					break;
				case UIElementMouseState.Pressed:
					currentTexture = pressedTexture;
					break;
			}
		}
	}
	protected UIElementMouseState uiElementMouseState;

	public event Action Clicked;

	protected bool isClicked = false;

	protected Rectangle CollisionRectangle;

	public Vector2 Position
	{
		get
		{
			return new Vector2(CollisionRectangle.X, CollisionRectangle.Y);
		}
		set
		{
			CollisionRectangle.X = (int)value.X;
			CollisionRectangle.Y = (int)value.Y;
		}
	}

	public Vector2 Size
	{
		get
		{
			return new Vector2(CollisionRectangle.Width, CollisionRectangle.Height);
		}
		set
		{
			CollisionRectangle.Width = (int)value.X;
			CollisionRectangle.Height = (int)value.Y;
		}
	}

	protected UIElement(Texture2D normalTexture, Texture2D hoverTexture, Texture2D pressedTexture, Texture2D disabledTexture, Vector2 position, Vector2 size)
	{
		CollisionRectangle = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
		this.normalTexture = normalTexture;
		this.hoverTexture = hoverTexture;
		this.pressedTexture = pressedTexture;
		this.disabledTexture = disabledTexture;
		currentTexture = normalTexture;
		UIElementState = UIElementMouseState.Normal;
	}

	protected virtual void OnClicked()
	{
		Clicked?.Invoke();
	}

	public virtual void Update(GameTime gameTime) { }

	public virtual void Draw(GameTime gameTime, SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(currentTexture, CollisionRectangle, Color.White);
	}

	public virtual void HandleInput(InputHelper inputHelper) { }

	public virtual void Reset()
	{
		isClicked = false;
		UIElementState = UIElementMouseState.Normal;
	}
}
