using System;
using System.Collections.Generic;
using System.Linq;
using GMTK2025.Engine;
using Microsoft.Xna.Framework.Graphics;

namespace GMTK2025.LevelGeneration
{
    /// <summary>
    /// A BuilderType for room that can be used in the room editor
    /// </summary>
    public class EditableRoom
    {
        private RoomType _type;
        private Vector2Int _size;
        private Tile[,] _tiles;
        private readonly List<Decoration> _decorations;
        private bool[,] _walls;
        private readonly List<Vector2Int> _enemySpawnPoints;
        public int TileSize;

        public EditableRoom(Vector2Int size, int tileSize)
        {
            _size = size;
            _tiles = new Tile[size.X, size.Y];
            _walls = new bool[size.X, size.Y];
            _type = RoomType.NormalRoom;
            _decorations = new List<Decoration>();
            _enemySpawnPoints = new List<Vector2Int>();
            TileSize = tileSize;
        }

        //* getters
        public RoomType GetRoomType()
        {
            return _type;
        }

        public Vector2Int GetSize()
        {
            return _size;
        }

        public Tile[,] GetTiles()
        {
            return _tiles;
        }

        public Decoration[] GetDecorations()
        {
            return _decorations.ToArray();
        }

        public bool[,] GetWalls()
        {
            return _walls;
        }

        public Vector2Int[] GetEnemySpawnPoints()
        {
            return _enemySpawnPoints.ToArray();
        }

        //* setters

        /// <summary>
        /// Resets the room
        /// </summary>
        /// <returns></returns>
        public bool ResetRoom()
        {
            _tiles = new Tile[_size.X, _size.Y];
            _walls = new bool[_size.X, _size.Y];
            _decorations.Clear();
            _enemySpawnPoints.Clear();
            _type = RoomType.NormalRoom;
            return true;
        }

        /// <summary>
        /// Sets a tile in the room
        /// </summary>
        /// <param name="x">x position of the tile</param>
        /// <param name="y">y position of the tile</param>
        /// <param name="tile">the tile</param>
        /// <returns><c>bool</c> if the tile was set</returns>
        public bool SetTile(int x, int y, GroundTile tile)
        {
            if (TileIsOutOfBounds(x, y))
            {
                Console.WriteLine("Tile at: " + x + " " + y + " is out of bounds");
                return false;
            }
            _tiles[x, y] = tile;
            _walls[x, y] = false;
            return true;
        }

        /// <summary>
        /// Sets a tile in the room
        /// </summary>
        /// <param name="x">x position of the tile</param>
        /// <param name="y">y position of the tile</param>
        /// <param name="tile">the tile</param>
        /// <returns><c>bool</c> if the tile was set</returns>
        public bool SetTile(int x, int y, WallTile tile)
        {
            if (TileIsOutOfBounds(x, y))
            {
                Console.WriteLine("Tile at: " + x + " " + y + " is out of bounds");
                return false;
            }
            _tiles[x, y] = tile;
            _walls[x, y] = true;
            return true;
        }

        /// <summary>
        /// Deletes a tile in the room
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool DeleteTile(int x, int y)
        {
            if (TileIsOutOfBounds(x, y))
            {
                Console.WriteLine("Tile at: " + x + " " + y + " is out of bounds");
                return false;
            }
            _tiles[x, y] = null;
            _walls[x, y] = false;
            return true;
        }

        public bool RemoveDecoration(int x, int y, int eraserSize)
        {
            for (int i = 0; i < _decorations.Count; i++)
            {
                if (_decorations[i].Position.X > x - eraserSize && _decorations[i].Position.X < x + eraserSize &&
                    _decorations[i].Position.Y > y - eraserSize && _decorations[i].Position.Y < y + eraserSize)
                {
                    _decorations.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }
        public bool RemoveSpawnPoint(int x, int y, int eraserSize)
        {
            for (int i = 0; i < _enemySpawnPoints.Count; i++)
            {
                if (_enemySpawnPoints[i].X > x - eraserSize && _enemySpawnPoints[i].X < x + eraserSize &&
                    _enemySpawnPoints[i].Y > y - eraserSize && _enemySpawnPoints[i].Y < y + eraserSize)
                {
                    _enemySpawnPoints.RemoveAt(i);
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Sets a decoration in the room
        /// </summary>
        /// <param name="x">x position of the decoration</param>
        /// <param name="y">y position of the decoration</param>
        /// <param name="decoration">the decoration</param>
        /// <returns><c>bool</c> if the decoration was added</returns>
        public bool AddDecoration(Decoration decoration)
        {
            int x = (int)decoration.Position.X;
            int y = (int)decoration.Position.Y;
            int size = decoration.Size;
            if (ObjectIsOutOfBounds(x, y, size))
            {
                Console.WriteLine("Decoration at: " + x + " " + y + " is out of bounds");
                return false;
            }
            _decorations.Add(decoration);
            return true;
        }

        /// <summary>
        /// Sets an enemy spawn point in the room
        /// </summary>
        /// <param name="x">x position of the enemy spawn point</param>
        /// <param name="y">y position of the enemy spawn point</param>
        /// <returns><c>bool</c> if the enemy spawn point was added</returns>
        public bool AddEnemySpawnPoint(int x, int y)
        {
            if (ObjectIsOutOfBounds(x, y, TileSize / 2))
            {
                Console.WriteLine("Enemy spawn point at: " + x + " " + y + " is out of bounds");
                return false;
            }
            _enemySpawnPoints.Add(new Vector2Int(x, y));
            return true;
        }


        public void SetType(RoomType type)
        {
            _type = type;
        }

        //* validators

        /// <summary>
        /// Checks if a tile position is out of bounds
        /// </summary>
        /// <param name="x">x position to check</param>
        /// <param name="y">y position to check</param>
        /// <returns><c>bool</c> if the position is out of bounds</returns>
        private bool TileIsOutOfBounds(int x, int y)
        {
            return x < 0 || y < 0 || x >= _size.X || y >= _size.Y;
        }

        /// <summary>
        /// Checks if an object position is out of bounds
        /// </summary>
        /// <param name="x">x position to check</param>
        /// <param name="y">y position to check</param>
        /// <returns><c>bool</c> if the position is out of bounds</returns>
        private bool ObjectIsOutOfBounds(int x, int y, int size)
        {
            return x - size < 0 || y - size < 0 || x + size > _size.X * TileSize || y + size > _size.Y * TileSize;
        }
    }
}