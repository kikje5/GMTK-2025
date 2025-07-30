using GMTK2025.Engine;
using GMTK2025.Engine.UI;
using Microsoft.Xna.Framework;

namespace GMTK2025.Screens;

public class GameScreen : Screen
{
	public GameScreen()
	{
		int ButtonWidth = 512;
		int ButtonHeight = ButtonWidth / 4;

		int ButtonSpacing = ButtonHeight + 64;
		int ButtonYStart = 512;
		int ButtonX = 960;

		Vector2 buttonSize = new Vector2(ButtonWidth, ButtonHeight);

		Button backButton = new Button(new Vector2(ButtonX, ButtonYStart + ButtonSpacing * 2), buttonSize);
		backButton.Text = "Back to Title";
		backButton.Clicked += () => App.ScreenManager.SwitchTo(ScreenManager.TITLE_SCREEN);
		Add(backButton);
	}
}