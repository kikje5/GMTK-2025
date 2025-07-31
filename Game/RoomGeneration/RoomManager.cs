using System.Collections.Generic;

namespace GMTK2025.RoomGeneration;

public class RoomManager
{
	private static RoomManager instance = null;

	private Room[] _rooms;

	/// <summary>
	/// Wow now THIS is a singleton :)
	/// <para>See: <see cref="https://refactoring.guru/design-patterns/singleton"/></para>
	/// </summary>
	/// <returns>the instance of the singleton</returns>
	public static RoomManager GetInstance()
	{
		if (instance == null)
		{
			instance = new RoomManager();
			instance.LoadRooms();
		}
		return instance;
	}

	private void LoadRooms()
	{
		List<Room> _rooms = new List<Room>();
		string[] RoomNames = RoomSaver.GetRoomNames();
		foreach (string RoomName in RoomNames)
		{
			_rooms.Add(RoomSaver.LoadRoom(RoomName));
		}
		this._rooms = _rooms.ToArray();
	}

	static public Room[] GetRooms()
	{
		return instance._rooms;
	}
}
