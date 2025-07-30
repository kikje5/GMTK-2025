using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.Serialization;
using GMTK2025.Engine;

namespace GMTK2025.LevelGeneration
{
    /// <summary>
    /// The room class which is saved and loaded in for use in levels.
    /// </summary>
    [DataContract]
    public class Room
    {
        [DataMember] public string Name { get; set; }
        [DataMember] public RoomType Type { get; set; }
        [DataMember] public Vector2Int Size { get; set; }
        [DataMember] public Tile[,] Tiles { get; set; }
        [DataMember] public Decoration[] Decorations { get; set; }
        [DataMember] public bool[,] Walls { get; set; }
        [DataMember] public Vector2Int[] EnemySpawnPoints { get; set; }

        public Room(string name, RoomType type, Vector2Int size, Tile[,] tiles, Decoration[] decorations, bool[,] walls, Vector2Int[] enemySpawnPoints)
        {
            Name = name;
            Type = type;
            Size = size;
            Tiles = tiles;
            Decorations = decorations;
            Walls = walls;
            EnemySpawnPoints = enemySpawnPoints;
        }

        public Room()
        {
            // Default constructor required for serialization
        }

        public Room(string name, EditableRoom editableRoom)
        {
            Name = name;
            Type = editableRoom.GetRoomType();
            Size = editableRoom.GetSize();
            Tiles = editableRoom.GetTiles();
            Decorations = editableRoom.GetDecorations();
            Walls = editableRoom.GetWalls();
            EnemySpawnPoints = editableRoom.GetEnemySpawnPoints();
        }
    }
}
