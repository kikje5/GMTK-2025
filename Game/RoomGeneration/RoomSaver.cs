using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace GMTK2025.LevelGeneration
{
	/// <summary>
	/// Class for saving and loading rooms
	/// </summary>
	public static class RoomSaver
	{
		private const string RoomPath = "../../../LevelGeneration/SavedRooms/objects/";
		public const string NameListPath = "../../../LevelGeneration/SavedRooms/NameList.txt";
		private const string Extension = ".room";

		/// <summary>
		/// Loads a room
		/// </summary>
		/// <param name="name">the name of the saved room</param>
		/// <returns>the room object</returns>
		public static Room LoadRoom(string name)
		{
			Room room = LoadBinary<Room>(RoomPath + name + Extension);
			return room;
		}

		public static bool SaveRoom(Room room)
		{
			// Save Room Data
			bool successData = SaveBinary(RoomPath + room.Name + Extension, room);
			//save name in the name list
			bool successName = AddNameToNameList(room.Name);

			return successData && successName;
		}

		private static bool AddNameToNameList(string name)
		{
			string[] NameList = LoadBinary<string[]>(NameListPath);
			HashSet<string> NewNameList = new HashSet<string>(NameList);
			NewNameList.Add(name);
			return SaveBinary(NameListPath, NewNameList.ToArray());
		}

		/// <summary>
		/// Saves a room to a binary file
		/// </summary>
		/// <typeparam name="T">the type of the object to save</typeparam>
		/// <param name="filePath">the filepath where the object will be saved</param>
		/// <param name="objectToWrite">the object to save</param>
		/// <returns></returns>
		public static bool SaveBinary<T>(string filePath, T objectToWrite)
		{
			try
			{
				using FileStream stream = new FileStream(filePath, FileMode.Create);
				DataContractSerializer serializer = new DataContractSerializer(typeof(T));
				serializer.WriteObject(stream, objectToWrite);
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error saving binary file: {ex.Message}");
				return false;
			}
		}

		/// <summary>
		/// Loads a room from a binary file
		/// </summary>
		/// <typeparam name="T">the type of the object to load into</typeparam>
		/// <param name="filePath">the filepath where the object is saved</param>
		/// <returns>the object saved at the filepath</returns>
		public static T LoadBinary<T>(string filePath)
		{
			try
			{
				using FileStream stream = new FileStream(filePath, FileMode.Open);
				DataContractSerializer serializer = new DataContractSerializer(typeof(T));
				return (T)serializer.ReadObject(stream);
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error loading binary file: {ex.Message}");
				return default;
			}
		}

		public static string[] GetRoomNames()
		{
			return LoadBinary<string[]>(NameListPath);
		}
	}
}