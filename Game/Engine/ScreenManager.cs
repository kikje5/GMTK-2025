using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GMTK2025.Screens;

namespace GMTK2025.Engine;

public class ScreenManager : ILoopObject
{
	public static string GO_TO_PREVIOUS_SCREEN = "GO TO PREVIOUS SCREEN";
	public static string TITLE_SCREEN = "Title Screen";
	public static string SETTINGS_SCREEN = "Settings Screen";


	private readonly Stack<string> previousGameScreens = new Stack<string>();
	private string currentScreenName = string.Empty;
	Dictionary<string, Screen> gameScreens;
	Screen currentGameScreen;

	public ScreenManager()
	{
		gameScreens = new Dictionary<string, Screen>();
		currentGameScreen = null;
	}

	public void Initialize()
	{
		AddScreen(TITLE_SCREEN, new TitleScreen());
		AddScreen(SETTINGS_SCREEN, new SettingsScreen());
	}

	public void AddScreen(string name, Screen state)
	{
		gameScreens[name] = state;
	}

	public ILoopObject GetScreen(string name)
	{
		return gameScreens[name];
	}

	public void SwitchTo(string name)
	{
		if (name == GO_TO_PREVIOUS_SCREEN)
		{
			GoToPreviousScreen();
		}
		else
		{
			SwitchToScreen(name);
		}
	}

	private void SwitchToScreen(string name, bool addScreenToStack = true)
	{
		if (gameScreens.ContainsKey(name))
		{
			if (addScreenToStack && currentScreenName != string.Empty)
			{
				previousGameScreens.Push(currentScreenName);
			}
			currentGameScreen = gameScreens[name];
			currentScreenName = name;
			currentGameScreen.Reset();
		}
		else
		{
			throw new KeyNotFoundException("Could not find game state: " + name);
		}
	}

	private void GoToPreviousScreen()
	{
		if (previousGameScreens.Count > 0)
		{
			string previousScreen = previousGameScreens.Pop();

			//Do not add the current state to the stack because we are going back to the previous state.
			SwitchToScreen(previousScreen, false);
		}
	}

	public ILoopObject CurrentGameScreen
	{
		get
		{
			return currentGameScreen;
		}
	}

	public void HandleInput(InputHelper inputHelper)
	{
		currentGameScreen?.HandleInput(inputHelper);
	}

	public void Update(GameTime gameTime)
	{
		currentGameScreen?.Update(gameTime);
	}

	public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
	{
		currentGameScreen?.Draw(gameTime, spriteBatch);
	}

	public void Reset()
	{
		currentGameScreen?.Reset();
	}
}
