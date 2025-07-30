using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using GMTK2025.Engine;

namespace GMTK2025.LevelGeneration
{
	public static class RoomTextureGenerator
	{
		/// <summary>
		/// Generates the room texture
		/// </summary>
		/// <param name="size">the size of the room in tiles by tiles.</param>
		/// <param name="tiles">all the tiles of the room.</param>
		/// <param name="decorations">all the decorations in the room.</param>
		/// <param name="tileSize">the size of tiles.</param>
		/// <returns><c>Texture2D</c> the room texture</returns>
		public static Texture2D GenerateRoomTexture(Room room, int tileSize)
		{
			Vector2Int size = room.Size;
			Tile[,] tiles = room.Tiles;
			Decoration[] decorations = room.Decorations;
			// generating the room works by letting the graphics device render to a render target which can be cast to a texture2D
			RenderTarget2D texture = new RenderTarget2D(App.Instance.GraphicsDevice, size.X * tileSize, size.Y * tileSize);
			App.Instance.GraphicsDevice.SetRenderTarget(texture);
			App.Instance.GraphicsDevice.Clear(Color.Transparent);
			//now we have a canvas to draw upon with sprite batches

			//fist we draw the tiles in a batch because these should be behind the decorations.
			App.SpriteBatch.Begin();

			for (int x = 0; x < size.X; x++)
			{
				for (int y = 0; y < size.Y; y++)
				{
					if (tiles[x, y] != null && tiles[x, y].Texture != null)
					{
						App.SpriteBatch.Draw(tiles[x, y].Texture,
						new Rectangle(x * tileSize, y * tileSize, tileSize, tileSize), Color.White);
					}
				}
			}
			App.SpriteBatch.End();

			//then we draw the decorations in a batch because these should be on top of the tiles.
			App.SpriteBatch.Begin();
			for (int i = 0; i < decorations.Length; i++)
			{
				App.SpriteBatch.Draw(decorations[i].Type.Texture,
				new Rectangle((int)(decorations[i].Position.X - decorations[i].Size / 2),
				(int)(decorations[i].Position.Y - decorations[i].Size / 2), decorations[i].Size, decorations[i].Size), Color.White);
			}
			App.SpriteBatch.End();

			//reset the graphics device to draw on screen instead of the render target
			App.Instance.GraphicsDevice.SetRenderTarget(null);

			//return the render target as a texture
			return texture;
		}
	}
}