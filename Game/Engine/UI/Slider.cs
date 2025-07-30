// using Microsoft.Xna.Framework;
// using Blok3Game.Engine.GameObjects;
// using Microsoft.Xna.Framework.Graphics;
// using Blok3Game.Engine.Helpers;
// using System;

// namespace Blok3Game.Engine.UI;

// public class Slider : GameObject
// {
// 	private Texture2D _sliderBody;
// 	private Texture2D _sliderBall;
// 	private Vector2 _size;

// 	private int _minValue;
// 	private int _maxValue;
// 	public int _value { get; private set; }
// 	private int _range;

// 	public Action<int> OnValueChanged;

// 	private Rectangle _sliderBodyRect;
// 	private Rectangle _sliderBallRect;

// 	private TextGameObject _minText;
// 	private TextGameObject _maxText;
// 	private TextGameObject _valueText;
// 	private TextGameObject _nameText;

// 	public Slider(string name, Vector2 position, Vector2 size, int minValue, int maxValue, int startValue) : base()
// 	{
// 		Position = position - (size / 2);
// 		_size = size;
// 		_minValue = minValue;
// 		_maxValue = maxValue;
// 		_range = _maxValue - _minValue;

// 		_minText = new TextGameObject("Fonts/SpriteFont", 0, "text");
// 		_minText.Text = _minValue.ToString();
// 		_minText.Position = new Vector2(Position.X - _minText.Size.X, Position.Y + _size.Y / 2 - _minText.Size.Y / 2);

// 		_maxText = new TextGameObject("Fonts/SpriteFont", 0, "text");
// 		_maxText.Text = _maxValue.ToString();
// 		_maxText.Position = new Vector2(Position.X + _size.X + _minText.Size.X, Position.Y + _size.Y / 2 - _minText.Size.Y / 2);

// 		_valueText = new TextGameObject("Fonts/SpriteFont", 0, "text");
// 		_valueText.Text = _value.ToString();
// 		_valueText.Position = new Vector2(0 + (_size.Y / 2) - _valueText.Size.X / 2, Position.Y - _minText.Size.Y);

// 		_nameText = new TextGameObject("Fonts/SpriteFont", 0, "text");
// 		_nameText.Text = name;
// 		_nameText.Position = new Vector2(Position.X + _size.X / 2 - _nameText.Size.X / 2, Position.Y - _nameText.Size.Y - _valueText.Size.Y);

// 		_sliderBody = GameEnvironment.AssetManager.Content.Load<Texture2D>("UI/SliderBody");
// 		_sliderBall = GameEnvironment.AssetManager.Content.Load<Texture2D>("UI/SliderBall");

// 		_sliderBodyRect = new Rectangle((int)Position.X, (int)(Position.Y + (size.Y / 4)), (int)size.X, (int)size.Y / 2);

// 		SetValue(startValue);
// 	}

// 	private void SetValue(int value)
// 	{
// 		if (value < _minValue || value > _maxValue)
// 		{
// 			return;
// 		}
// 		_value = value;
// 		float valueRatio = (float)(value - _minValue) / _range;
// 		int BallXPosition = (int)(Position.X + (_size.X * valueRatio) - (_size.Y / 2));
// 		_sliderBallRect = new Rectangle(BallXPosition, (int)Position.Y, (int)_size.Y, (int)_size.Y);
// 		_valueText.Text = _value.ToString();
// 		_valueText.Position = new Vector2(BallXPosition + (_size.Y / 2) - _valueText.Size.X / 2, Position.Y - _minText.Size.Y);

// 		OnValueChanged?.Invoke(_value);
// 	}

// 	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
// 	{
// 		base.Draw(gameTime, spriteBatch);
// 		spriteBatch.Draw(_sliderBody, _sliderBodyRect, Color.White);
// 		spriteBatch.Draw(_sliderBall, _sliderBallRect, Color.White);

// 		_minText.Draw(gameTime, spriteBatch);
// 		_maxText.Draw(gameTime, spriteBatch);
// 		_valueText.Draw(gameTime, spriteBatch);
// 		_nameText.Draw(gameTime, spriteBatch);
// 	}

// 	public override void HandleInput(InputHelper inputHelper)
// 	{
// 		base.HandleInput(inputHelper);

// 		if (!inputHelper.MouseLeftButtonDown)
// 		{
// 			return;
// 		}
// 		Vector2 MousePosition = inputHelper.MousePosition;
// 		bool isInSliderBody = MousePosition.X > _sliderBodyRect.X &&
// 			MousePosition.X < _sliderBodyRect.X + _sliderBodyRect.Width &&
// 			MousePosition.Y > _sliderBodyRect.Y &&
// 			MousePosition.Y < _sliderBodyRect.Y + _sliderBodyRect.Height;

// 		if (!isInSliderBody)
// 		{
// 			return;
// 		}

// 		int relativeMouseX = (int)(MousePosition.X - _sliderBodyRect.X);
// 		float valueRatio = (float)relativeMouseX / _sliderBodyRect.Width;
// 		int value = (int)MathF.Round(_minValue + (valueRatio * _range));
// 		SetValue(value);

// 	}
// }