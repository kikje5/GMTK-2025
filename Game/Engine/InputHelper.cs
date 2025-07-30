using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace GMTK2025.Engine;

public class InputHelper
{
	private MouseState _currentMouseState;
	private MouseState _previousMouseState;
	private KeyboardState _currentKeyboardState;
	private KeyboardState _previousKeyboardState;
	public KeyboardState CurrentKeyboardState { get => _currentKeyboardState; }
	public KeyboardState PreviousKeyboardState { get => _previousKeyboardState; }
	public MouseState CurrentMouseState { get => _currentMouseState; }
	public MouseState PreviousMouseState { get => _previousMouseState; }
	private Vector2 scale, offset;

	public InputHelper()
	{
		scale = Vector2.One;
		offset = Vector2.Zero;
	}

	public void Update()
	{
		_previousMouseState = _currentMouseState;
		_previousKeyboardState = _currentKeyboardState;
		_currentMouseState = Mouse.GetState();
		_currentKeyboardState = Keyboard.GetState();
	}

	public Vector2 Scale
	{
		get { return scale; }
		set { scale = value; }
	}

	public Vector2 Offset
	{
		get { return offset; }
		set { offset = value; }
	}

	public Vector2 MousePosition
	{
		get { return (new Vector2(_currentMouseState.X, _currentMouseState.Y) - offset) / scale; }
	}

	public bool ShiftKeyDown => _currentKeyboardState.IsKeyDown(Keys.LeftShift) || _currentKeyboardState.IsKeyDown(Keys.RightShift);
	public bool ControlKeyDown => _currentKeyboardState.IsKeyDown(Keys.LeftControl) || _currentKeyboardState.IsKeyDown(Keys.RightControl);
	public bool AltKeyDown => _currentKeyboardState.IsKeyDown(Keys.LeftAlt) || _currentKeyboardState.IsKeyDown(Keys.RightAlt);

	public bool MouseLeftButtonPressed =>
		_currentMouseState.LeftButton == ButtonState.Pressed &&
		_previousMouseState.LeftButton == ButtonState.Released;

	public bool MouseLeftButtonReleased =>
		_currentMouseState.LeftButton == ButtonState.Released &&
		_previousMouseState.LeftButton == ButtonState.Pressed;

	public bool MouseLeftButtonDown =>
		_currentMouseState.LeftButton == ButtonState.Pressed;

	public bool MouseRightButtonPressed =>
		_currentMouseState.RightButton == ButtonState.Pressed &&
		_previousMouseState.RightButton == ButtonState.Released;

	public bool MouseRightButtonReleased =>
		_currentMouseState.RightButton == ButtonState.Released &&
		_previousMouseState.RightButton == ButtonState.Pressed;

	public bool MouseRightButtonDown =>
		_currentMouseState.RightButton == ButtonState.Pressed;

	public bool KeyPressed(Keys k)
	{
		return _currentKeyboardState.IsKeyDown(k) && _previousKeyboardState.IsKeyUp(k);
	}

	public bool IsKeyDown(Keys k)
	{
		return _currentKeyboardState.IsKeyDown(k);
	}

	public bool AnyKeyPressed =>
		_currentKeyboardState.GetPressedKeys().Length > 0 &&
		_previousKeyboardState.GetPressedKeys().Length == 0;

	public bool GeDownKey(out Keys key)
	{
		if (_currentKeyboardState.GetPressedKeys().Length > 0)
		{
			key = _currentKeyboardState.GetPressedKeys()[0];
			return true;
		}
		else
		{
			key = Keys.None;
			return false;
		}
	}

	public bool GetKeyPressed(out Keys key)
	{
		if (_currentKeyboardState.GetPressedKeys().Length > 0)
		{
			foreach (var k in _currentKeyboardState.GetPressedKeys())
			{
				if (_previousKeyboardState.IsKeyUp(k))
				{
					key = k;
					return true;
				}
			}
		}
		key = Keys.None;
		return false;
	}
}
