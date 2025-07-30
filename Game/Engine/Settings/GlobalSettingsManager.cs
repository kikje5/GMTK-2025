
using System;
using System.IO;
using System.Text.Json;


namespace GMTK2025.Engine.Settings;

public enum Language
{
	English
}
public static class GlobalSettingsManager
{
	private static SettingsObject _settings;

	public static SettingsObject Settings
	{
		get
		{
			if (_settings == null)
			{
				_settings = LoadSettings();
			}
			return _settings;
		}
		private set
		{
			_settings = value;
			SaveSettings();
		}
	}

	public static Language Language
	{
		get => (Language)Settings.Language;
		set
		{
			Settings.Language = (int)value;
			SaveSettings();
		}
	}

	public static int MasterVolume
	{
		get => Settings.MasterVolume;
		set
		{
			if (value < 0)
			{
				Settings.MasterVolume = 100;
			}
			else if (value > 100)
			{
				Settings.MasterVolume = 0;
			}
			else
			{
				Settings.MasterVolume = value;
			}
			App.AudioManager.ChangeMusicVolume();
			SaveSettings();
		}
	}

	public static int MusicVolume
	{
		get => Settings.MusicVolume;
		set
		{
			if (value < 0)
			{
				Settings.MusicVolume = 100;
			}
			else if (value > 100)
			{
				Settings.MusicVolume = 0;
			}
			else
			{
				Settings.MusicVolume = value;
			}
			App.AudioManager.ChangeMusicVolume();
			SaveSettings();
		}
	}

	public static int SFXVolume
	{
		get => Settings.SfxVolume;
		set
		{
			if (value < 0)
			{
				Settings.SfxVolume = 100;
			}
			else if (value > 100)
			{
				Settings.SfxVolume = 0;
			}
			else
			{
				Settings.SfxVolume = value;
			}
			SaveSettings();
		}
	}

	private static string _saveFilePath = "./Settings.json";
	public static void SaveSettings()
	{
		Console.WriteLine("Saving settings...");
		try
		{
			string json = JsonSerializer.Serialize(Settings);
			Console.WriteLine($"Settings JSON: {json}");
			File.WriteAllText(_saveFilePath, json);
			Console.WriteLine("Settings saved successfully.");
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error saving settings: {ex.Message}");
		}
	}

	public static SettingsObject LoadSettings()
	{
		try
		{
			string json = File.ReadAllText(_saveFilePath);
			Settings = JsonSerializer.Deserialize<SettingsObject>(json);
			return Settings ?? new SettingsObject();
		}
		catch (Exception ex)
		{
			Console.WriteLine($"Error loading settings: {ex.Message}");
			return new SettingsObject();
		}
	}
}