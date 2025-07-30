using System;
using Microsoft.Xna.Framework;

namespace GMTK2025.Engine;

public class Vector2Int
{
	public int X { get; set; }
	public int Y { get; set; }

	public Vector2Int(int x, int y)
	{
		X = x;
		Y = y;
	}

	public Vector2Int(Point point)
	{
		X = point.X;
		Y = point.Y;
	}

	public Vector2Int()
	{
		// Default constructor required for serialization
	}

	public Vector2 ToVector2() => new Vector2(X, Y);

	public int Distance(Vector2Int other) => Math.Abs(X - other.X + Y - other.Y);

	public static int Distance(Vector2Int a, Vector2Int b) => Math.Abs(a.X - b.X) + Math.Abs(a.Y - b.Y);

	public override string ToString()
	{
		return "Vector2Int: (" + X + ", " + Y + ")";
	}

	public override bool Equals(object obj)
	{
		return obj is Vector2Int other && X == other.X && Y == other.Y;
	}

	public override int GetHashCode()
	{
		return X.GetHashCode() ^ Y.GetHashCode();
	}

	public static Vector2Int operator +(Vector2Int a, Vector2Int b) => new Vector2Int(a.X + b.X, a.Y + b.Y);
	public static Vector2Int operator -(Vector2Int a, Vector2Int b) => new Vector2Int(a.X - b.X, a.Y - b.Y);
	public static Vector2Int operator *(Vector2Int a, int b) => new Vector2Int(a.X * b, a.Y * b);
	public static Vector2Int operator /(Vector2Int a, int b) => new Vector2Int(a.X / b, a.Y / b);
}
