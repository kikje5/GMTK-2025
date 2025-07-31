using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.Serialization;
using GMTK2025.Engine;

namespace GMTK2025.RoomGeneration
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
        [IgnoreDataMember] public Texture2D Texture { get; set; }
        [IgnoreDataMember] public bool[,] Walls { get; set; }
        [DataMember]
        public bool[] WallsArray
        {
            get => Array2DToArray1D(Walls);
            set => Walls = Array1DToArray2D(value, Size.X, Size.Y);
        }
        [DataMember] public Vector2Int[] EnemySpawnPoints { get; set; }
        [DataMember] public string TexturePath { get; set; } // Store texture filename

        public Room(string name, RoomType type, Vector2Int size, bool[,] walls, Vector2Int[] enemySpawnPoints)
        {
            Name = name;
            TexturePath = $"../../../RoomGeneration/SavedRooms/textures/{name}.png"; // Save texture path
            Type = type;
            Size = size;
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
            TexturePath = $"../../../RoomGeneration/SavedRooms/textures/{name}.png"; // Save texture path
            Texture = RoomTextureGenerator.GenerateRoomTexture(editableRoom, editableRoom.TileSize);
            Type = editableRoom.GetRoomType();
            Size = editableRoom.GetSize();
            Walls = editableRoom.GetWalls();
            EnemySpawnPoints = editableRoom.GetEnemySpawnPoints();
        }

        static private T[,] Array1DToArray2D<T>(T[] array, int width, int height)
        {
            T[,] result = new T[width, height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    result[j, i] = array[i * width + j];
                }
            }
            return result;
        }

        static private T[] Array2DToArray1D<T>(T[,] array)
        {
            int width = array.GetLength(0);
            int height = array.GetLength(1);
            T[] result = new T[width * height];
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    result[i * width + j] = array[j, i];
                }
            }
            return result;
        }
    }
}
