using GMTK2025.Engine;
using GMTK2025.Engine.UI;
using Microsoft.Xna.Framework;
using GMTK2025.Engine.Settings;

namespace GMTK2025.Screens;

public class SettingsScreen : Screen
{
	Button languageButton;
	Button masterVolumeButton;
	Button musicVolumeButton;
	Button sfxVolumeButton;
	Button backButton;
	private TextElement settingsText;
	public SettingsScreen()
	{
		int ButtonWidth = 512;
		int ButtonHeight = ButtonWidth / 4;

		int ButtonSpacing = ButtonHeight + 64;
		int ButtonYStart = 256 + 128;
		int centerX = 960;
		int ButtonX1 = 960 - ButtonWidth / 2 - 64;
		int ButtonX2 = 960 + ButtonWidth / 2 + 64;

		Vector2 buttonSize = new Vector2(ButtonWidth, ButtonHeight);

		settingsText = new TextElement("Fonts/TitleFont");
		settingsText.Text = "Settings";
		settingsText.Position = new Vector2(centerX, 128);
		Add(settingsText);

		languageButton = new Button(new Vector2(ButtonX1, ButtonYStart), buttonSize);
		languageButton.Text = "Language: " + GlobalSettingsManager.Language.ToString();
		languageButton.Clicked += () =>
		{
			GlobalSettingsManager.Language = GlobalSettingsManager.Language == Language.English ? Language.English : Language.English;
			languageButton.Text = "Language: " + GlobalSettingsManager.Language.ToString();
		};
		Add(languageButton);

		masterVolumeButton = new Button(new Vector2(ButtonX2, ButtonYStart), buttonSize);
		masterVolumeButton.Text = "Master Volume: " + GlobalSettingsManager.MasterVolume;
		masterVolumeButton.Clicked += () =>
		{
			GlobalSettingsManager.MasterVolume += 10;
			masterVolumeButton.Text = "Master Volume: " + GlobalSettingsManager.MasterVolume;
		};
		Add(masterVolumeButton);

		musicVolumeButton = new Button(new Vector2(ButtonX2, ButtonYStart + ButtonSpacing * 1), buttonSize);
		musicVolumeButton.Text = "Music Volume: " + GlobalSettingsManager.MusicVolume;
		musicVolumeButton.Clicked += () =>
		{
			GlobalSettingsManager.MusicVolume += 10;
			musicVolumeButton.Text = "Music Volume: " + GlobalSettingsManager.MusicVolume;
		};
		Add(musicVolumeButton);

		sfxVolumeButton = new Button(new Vector2(ButtonX2, ButtonYStart + ButtonSpacing * 2), buttonSize);
		sfxVolumeButton.Text = "SFX Volume: " + GlobalSettingsManager.SFXVolume;
		sfxVolumeButton.Clicked += () =>
		{
			GlobalSettingsManager.SFXVolume = GlobalSettingsManager.SFXVolume + 10;
			sfxVolumeButton.Text = "SFX Volume: " + GlobalSettingsManager.SFXVolume;
		};
		Add(sfxVolumeButton);

		backButton = new Button(new Vector2(centerX, ButtonYStart + ButtonSpacing * 3), buttonSize);
		backButton.Text = "Go Back";
		backButton.Clicked += () => App.ScreenManager.SwitchTo(ScreenManager.TITLE_SCREEN);
		Add(backButton);
	}
}