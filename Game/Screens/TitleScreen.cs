using GMTK2025.Engine;
using GMTK2025.Engine.UI;
using Microsoft.Xna.Framework;

namespace GMTK2025.Screens;

public class TitleScreen : Screen
{
	private Button GameButton;
	private Button GlobalSettingsButton;
	private Button exitButton;
	private Button RoomCreatorButton;
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

		GameButton = new Button(new Vector2(ButtonX, ButtonYStart), buttonSize);
		GameButton.Text = "Start Game";
		GameButton.Clicked += () => App.ScreenManager.SwitchTo(ScreenManager.GAME_SCREEN);
		Add(GameButton);

		RoomCreatorButton = new Button(new Vector2(ButtonWidth / 4 + 8, 1080 - ButtonHeight / 4 - 8), buttonSize / 2);
		RoomCreatorButton.Text = "Room Creator";
		RoomCreatorButton.Clicked += () => App.ScreenManager.SwitchTo(ScreenManager.ROOM_CREATOR_SCREEN);
		Add(RoomCreatorButton);

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