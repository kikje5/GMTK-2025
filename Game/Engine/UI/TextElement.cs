using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GMTK2025.Engine.UI;

public class TextElement : ILoopObject
{
	private SpriteFont _spriteFont;
	private Color _color;
	private string _text;
	private Vector2 _position;
	private Vector2 _TruePosition;
	public bool Visible = true;

	public TextElement(string spriteFontName)
	{
		_spriteFont = App.AssetManager.Content.Load<SpriteFont>(spriteFontName);
		_color = Color.White;
		_text = string.Empty;
		_position = Vector2.Zero;
	}

	public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
	{
		if (Visible)
		{
			spriteBatch.DrawString(_spriteFont, _text, Position, _color);
		}
	}

	public void Update(GameTime gameTime) { }

	public void HandleInput(InputHelper inputHelper) { }

	public void Reset() { }

	public Color Color
	{
		get { return _color; }
		set { _color = value; }
	}

	public string Text
	{
		get { return _text; }
		set
		{
			_text = value;
			_position = _TruePosition - Size / 2;
		}
	}

	public Vector2 Position
	{
		get { return _position; }
		set
		{
			_TruePosition = value;
			_position = _TruePosition - Size / 2;
		}
	}

	public Vector2 Size
	{
		get
		{ return _spriteFont.MeasureString(_text); }
	}
}
