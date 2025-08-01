using System;
using System.Collections.Generic;
using GMTK2025.Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace GMTK2025.Entities;

public class EnemyManager : ILoopObject
{
	List<Enemy> enemies;
	public Player Player;

	private int summonFrames = 120;
	public int SummonCooldown = 0;
	private Random random = new Random();

	static public EnemyManager Instance
	{
		get
		{
			if (_instance == null)
			{
				_instance = new EnemyManager();
			}
			return _instance;
		}
	}
	static private EnemyManager _instance;
	EnemyManager()
	{
		enemies = new List<Enemy>();
	}

	private void AddEnemy(Enemy enemy)
	{
		enemies.Add(enemy);
	}

	private void RemoveEnemy(Enemy enemy)
	{
		enemies.Remove(enemy);
	}

	public void SummonPrairieDog(Vector2 position)
	{
		var prairieDog = new PrairieDog(position, Player);
		AddEnemy(prairieDog);
	}
	public void SummonWolf(Vector2 position)
	{
		var wolf = new Wolf(position, Player);
		AddEnemy(wolf);
	}
	public void SummonPorcupine(Vector2 position)
	{
		var porcupine = new Porcupine(position, Player);
		AddEnemy(porcupine);
	}

	public void Update(GameTime gameTime)
	{
		Enemy[] enemiesArray = enemies.ToArray();
		foreach (var enemy in enemies)
		{
			enemy.Update(gameTime, enemiesArray);
		}

		SummonCooldown++;
		if (SummonCooldown >= summonFrames)
		{
			SummonCooldown = random.Next(0, summonFrames / 2);
			SummonRandomEnemy();
		}
	}

	public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
	{
		foreach (var enemy in enemies)
		{
			enemy.Draw(gameTime, spriteBatch);
		}
	}

	public void HandleInput(InputHelper inputHelper) { }

	public void Reset()
	{
		enemies.Clear();
	}

	public void DamageEnemiesInRadius(Vector2 position, float radius, int damage)
	{
		for (int i = enemies.Count - 1; i >= 0; i--)
		{
			var enemy = enemies[i];
			if (!enemy.IsAlive) continue; // Skip dead enemies1
			{
				if (Vector2.Distance(enemy.Position, position) <= radius)
				{
					enemy.TakeDamage(damage);
					if (!enemy.IsAlive)
					{
						RemoveEnemy(enemy); // Remove enemy if dead
					}
				}
			}
		}
	}

	private void SummonRandomEnemy()
	{
		int quarterWidth = 1920 / 4;
		int quarterHeight = 1080 / 4;
		int enemyType = random.Next(0, 3); // 0: PrairieDog, 1: Wolf, 2: Porcupine
		Vector2 position = new Vector2(random.Next(quarterWidth, 3 * quarterWidth), random.Next(quarterHeight, 3 * quarterHeight));

		switch (enemyType)
		{
			case 0:
				SummonPrairieDog(position);
				break;
			case 1:
				SummonWolf(position);
				break;
			case 2:
				SummonPorcupine(position);
				break;
		}
	}
}