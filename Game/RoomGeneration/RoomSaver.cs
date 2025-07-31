using System.IO;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Runtime.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace GMTK2025.RoomGeneration;

/// <summary>
/// Class for saving and loading rooms
/// </summary>
public static class RoomSaver
{
	private const string RoomPath = "../../../RoomGeneration/SavedRooms/objects/";
	public const string NameListPath = "../../../RoomGeneration/SavedRooms/NameList.txt";

	private const string Extension = ".room";

	/// <summary>
	/// Loads a room
	/// </summary>
	/// <param name="name">the name of the saved room</param>
	/// <returns>the room object</returns>
	public static Room LoadRoom(string name)
	{
		Room room = LoadBinary<Room>(RoomPath + name + Extension);
		room.Texture = LoadTextureFromPng(room.TexturePath);
		return room;
	}

	public static bool SaveRoom(Room room)
	{
		// Save Room Data
		bool successData = SaveBinary(RoomPath + room.Name + Extension, room);
		// Save Texture Separately
		bool successTexture = SaveTextureToPng(room.Texture, room.TexturePath);
		//save name in the name list
		bool successName = AddNameToNameList(room.Name);

		return successData && successTexture && successName;
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

	/// <summary>
	/// Loads a texture from a png file
	/// </summary>
	/// <param name="filePath">the filepath where the texture is saved</param>
	/// <returns>the loaded texture</returns>
	public static Texture2D LoadTextureFromPng(string filePath)
	{
		using FileStream stream = new FileStream(filePath, FileMode.Open);
		return Texture2D.FromStream(App.Instance.GraphicsDevice, stream);
	}

	/// <summary>
	/// Saves a texture to a png
	/// </summary>
	/// <param name="texture">the texture to save</param>
	/// <param name="filePath">the filepath where the texture will be saved</param>
	public static bool SaveTextureToPng(Texture2D texture, string filePath)
	{
		if (texture == null) return false;

		using FileStream stream = new FileStream(filePath, FileMode.Create);
		texture.SaveAsPng(stream, texture.Width, texture.Height);
		return true;
	}

	public static string[] GetRoomNames()
	{
		return LoadBinary<string[]>(NameListPath);
	}

}