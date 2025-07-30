using GMTK2025.Engine;
using GMTK2025.Engine.UI;
using Microsoft.Xna.Framework;
using GMTK2025.Character;

namespace GMTK2025.Screens;

public class GameScreen : Screen
{
	Player player;
	Bar ThrowBar;
	Bar healthBar;
	public GameScreen()
	{
		int ScreenWidth = 1920;
		int ScreenHeight = 1080;
		player = new Player(new Vector2(100, 100), new Vector2(32, 32), 20, 1, 0.15f, 100);
		Add(player);
		healthBar = new Bar(new Vector2(ScreenWidth - 288, 0), new Vector2(288, 72), player.MaxHealth, Color.Red);
		ThrowBar = new Bar(new Vector2(ScreenWidth - 288, ScreenHeight - 72), new Vector2(288, 72), 100, Color.Green);
		Add(healthBar);
		Add(ThrowBar);
	}
	public override void Update(GameTime gameTime)
	{
		base.Update(gameTime);
		healthBar.Value = player.Health;
		ThrowBar.Value = player.ThrowCharge;
	}
}