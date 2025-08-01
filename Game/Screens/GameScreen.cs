using GMTK2025.Engine;
using GMTK2025.Engine.UI;
using Microsoft.Xna.Framework;
using GMTK2025.Entities;
using Microsoft.Xna.Framework.Graphics;

namespace GMTK2025.Screens;

public class GameScreen : Screen
{
	Player player;
	Bar ThrowBar;
	Bar healthBar;

	Bar ExperienceBar;

	Vector2 BoardPosition = new Vector2(420, 0);
	Texture2D BoardTexture = App.AssetManager.GetTexture("Decoration/SandBackground");

	public GameScreen()
	{
		BackgroundSong = "GameMusic";
		int ScreenWidth = 1920;
		int ScreenHeight = 1080;
		player = new Player(new Vector2(ScreenWidth / 2, ScreenHeight / 2));
		EnemyManager.Instance.Player = player;
		Add(player);
		healthBar = new Bar(new Vector2(ScreenWidth - 288, 0), new Vector2(288, 72), player.MaxHealth, Color.Red);
		ThrowBar = new Bar(new Vector2(ScreenWidth - 288, ScreenHeight - 72), new Vector2(288, 72), 100, Color.Green);
		ExperienceBar = new Bar(new Vector2(0, 0), new Vector2(288, 72), player.ExperienceNeededForNextLevel, Color.Blue);
		Add(healthBar);
		Add(ThrowBar);
		Add(ExperienceBar);
		Add(EnemyManager.Instance);


	}
	public override void Update(GameTime gameTime)
	{
		if (player.HasJustLeveledUp)
		{
			// Handle leveling up logic here
			player.HasJustLeveledUp = false;
			return;
		}
		base.Update(gameTime);
		healthBar.Value = player.Health;
		ThrowBar.Value = player.ThrowCharge;
		ExperienceBar.Value = player.CurrentExperience;
	}

	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
	{
		spriteBatch.Draw(BoardTexture, BoardPosition, Color.White);
		base.Draw(gameTime, spriteBatch);
		if (player.HasJustLeveledUp)
		{
			//draw all things for leveling up
		}
	}

	public override void Reset()
	{
		base.Reset();
	}
}