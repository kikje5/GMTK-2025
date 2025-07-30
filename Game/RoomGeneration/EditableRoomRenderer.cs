using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GMTK2025.Engine;

namespace GMTK2025.LevelGeneration
{
	public class EditableRoomRenderer : ILoopObject
	{
		private readonly EditableRoom _room;
		public Vector2 Position { get; set; }

		private readonly Texture2D EnemyPoint = App.AssetManager.GetTexture("Points/EnemyPoint");
		private readonly Texture2D GridTile = App.AssetManager.GetTexture("Tiles/GridTile");
		public EditableRoomRenderer(Vector2 position, EditableRoom room)
		{
			Position = position;
			_room = room;
		}

		public void Draw(GameTime gameTime, SpriteBatch spriteBatch)
		{
			DrawGrid(spriteBatch);
			DrawTiles(spriteBatch);
			DrawDecoration(spriteBatch);
			DrawEnemyPoints(spriteBatch);
		}

		public void Update(GameTime gameTime) { }
		public void HandleInput(InputHelper inputHelper) { }
		public void Reset() { }

		private void DrawGrid(SpriteBatch spriteBatch)
		{
			int tileSize = _room.TileSize;
			for (int y = 0; y < _room.GetSize().Y; y++)
			{
				for (int x = 0; x < _room.GetSize().X; x++)
				{
					spriteBatch.Draw(GridTile, new Rectangle((int)(x * tileSize + Position.X),
					(int)(y * tileSize + Position.Y), tileSize, tileSize), Color.White);
				}
			}
		}

		private void DrawTiles(SpriteBatch spriteBatch)
		{
			Tile[,] tiles = _room.GetTiles();
			int tileSize = _room.TileSize;
			for (int y = 0; y < _room.GetSize().Y; y++)
			{
				for (int x = 0; x < _room.GetSize().X; x++)
				{
					if (tiles[x, y] == null) continue;
					spriteBatch.Draw(tiles[x, y].Texture, new Rectangle((int)(x * tileSize + Position.X),
					(int)(y * tileSize + Position.Y), tileSize, tileSize), Color.White);
				}
			}
		}

		private void DrawDecoration(SpriteBatch spriteBatch)
		{
			Decoration[] decorations = _room.GetDecorations();
			for (int i = 0; i < decorations.Length; i++)
			{
				spriteBatch.Draw(decorations[i].Type.Texture, new Rectangle((int)(decorations[i].Position.X + Position.X - decorations[i].Size / 2),
				(int)(decorations[i].Position.Y + Position.Y - decorations[i].Size / 2), decorations[i].Size, decorations[i].Size), Color.White);
			}
		}

		private void DrawEnemyPoints(SpriteBatch spriteBatch)
		{
			Vector2Int[] enemyPoints = _room.GetEnemySpawnPoints();
			for (int i = 0; i < enemyPoints.Length; i++)
			{
				spriteBatch.Draw(EnemyPoint, new Rectangle((int)(enemyPoints[i].X + Position.X - EnemyPoint.Width / 2),
				(int)(enemyPoints[i].Y + Position.Y - EnemyPoint.Height / 2), 16, 16), Color.White);
			}
		}
	}
}