using GMTK2025.Engine;
using GMTK2025.Engine.UI;
using Microsoft.Xna.Framework;
using GMTK2025.Entities;

namespace GMTK2025.Screens;

public class GameScreen : Screen
{
	Player player;
	Bar ThrowBar;
	Bar healthBar;
	public GameScreen()
	{
		BackgroundSong = "GameMusic";

		int ScreenWidth = 1920;
		int ScreenHeight = 1080;
		player = new Player(new Vector2(ScreenWidth / 2, ScreenHeight / 2));
		Add(player);
		healthBar = new Bar(new Vector2(ScreenWidth - 288, 0), new Vector2(288, 72), player.MaxHealth, Color.Red);
		ThrowBar = new Bar(new Vector2(ScreenWidth - 288, ScreenHeight - 72), new Vector2(288, 72), 100, Color.Green);
		Add(healthBar);
		Add(ThrowBar);
		PrairieDog prairieDog = new PrairieDog(new Vector2(ScreenWidth / 2 + 100, ScreenHeight / 2), player);
		//Add(prairieDog);
		Wolf wolf = new Wolf(new Vector2(ScreenWidth / 2 + 300, ScreenHeight / 2), player);
		//Add(wolf);
		Porcupine porcupine = new Porcupine(new Vector2(ScreenWidth / 2 + 500, ScreenHeight / 2), player);
		Add(porcupine);
	}
	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		healthBar.Value = player.Health;
		ThrowBar.Value = player.ThrowCharge;
	}

	public override void Reset()
	{
		base.Reset();
	}
}