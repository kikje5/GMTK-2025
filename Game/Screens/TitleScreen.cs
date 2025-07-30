using GMTK2025.Engine;
using GMTK2025.Engine.UI;
using Microsoft.Xna.Framework;

namespace GMTK2025.Screens;

public class TitleScreen : Screen
{
	private Button SimSelectionButton;
	private Button GlobalSettingsButton;
	private Button exitButton;
	private TextElement titleText;

	public TitleScreen()
	{
		BackgroundSong = "main_menu";

		int ButtonWidth = 512;
		int ButtonHeight = ButtonWidth / 4;

		int ButtonSpacing = ButtonHeight + 64;
		int ButtonYStart = 512;
		int ButtonX = 960;

		Vector2 buttonSize = new Vector2(ButtonWidth, ButtonHeight);

		titleText = new TextElement("Fonts/TitleFont");
		titleText.Text = "GMTK 2025";
		titleText.Position = new Vector2(ButtonX, 128);
		Add(titleText);

		SimSelectionButton = new Button(new Vector2(ButtonX, ButtonYStart), buttonSize);
		SimSelectionButton.Text = "Start Game";
		SimSelectionButton.Clicked += () => App.ScreenManager.SwitchTo(ScreenManager.TITLE_SCREEN);
		Add(SimSelectionButton);

		GlobalSettingsButton = new Button(new Vector2(ButtonX, ButtonYStart + ButtonSpacing), buttonSize);
		GlobalSettingsButton.Text = "Global Settings";
		GlobalSettingsButton.Clicked += () => App.ScreenManager.SwitchTo(ScreenManager.SETTINGS_SCREEN);
		Add(GlobalSettingsButton);

		exitButton = new Button(new Vector2(ButtonX, ButtonYStart + ButtonSpacing * 2), buttonSize);
		exitButton.Clicked += App.Instance.Exit;
		exitButton.Text = "Exit";
		Add(exitButton);
	}
}