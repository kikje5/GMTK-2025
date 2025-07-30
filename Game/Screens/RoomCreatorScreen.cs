using System;
using GMTK2025.LevelGeneration;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using GMTK2025.Engine;
using GMTK2025.Engine.UI;
using System.Linq;

namespace GMTK2025.Screens;

public class RoomCreatorScreen : Screen
{
	//room
	private readonly EditableRoom _room;
	private readonly EditableRoomRenderer _roomRenderer;
	private readonly int _tileSize;

	//grid
	private readonly Texture2D _gridTile = LoadTexture("tiles/GridTile");
	private readonly Texture2D _selectionTile = LoadTexture("tiles/SelectionTile");
	private Vector2Int _highlightTile;
	private Vector2 _selectionTilePosition;
	private Vector2 _gridPosition = new Vector2(0, 0);
	private bool _mouseIsInGrid = false;

	//dragging variables
	private bool _isDragging = false;
	private Decoration _currentDraggingDecoration = null;
	private Vector2Int _currentDraggingSpawnPoint = new Vector2Int(0, 0);

	//Assets
	private readonly List<WallTile> _wallTiles = new List<WallTile>();
	private readonly List<GroundTile> _groundTiles = new List<GroundTile>();
	private readonly List<DecorationType> _decorationTypes = new List<DecorationType>();

	/// <summary>
	/// 0 = GroundTile, 1 = WallTile, 2 = Decoration 3 = spawnPoints
	/// </summary>
	private int _currentEditType = 0;
	private Selector _editModeSelector;
	private Selector _assetSelector;
	List<NamedTexture> _editModesNamedTextures;
	private List<NamedTexture>[] EditModes;

	//named textures for in the asset selector
	private List<NamedTexture> _wallTileNamedTextures;
	private List<NamedTexture> _groundTileNamedTextures;
	private List<NamedTexture> _decorationNamedTextures;
	private List<NamedTexture> _spawnPointNamedTextures;

	public RoomCreatorScreen() : base()
	{
		LoadAssets();

		_tileSize = 32;

		_room = new EditableRoom(new Vector2Int(32, 32), _tileSize);
		_roomRenderer = new EditableRoomRenderer(_gridPosition, _room);
		Add(_roomRenderer);

		AddUIElements();

		//reset room to generate Grid
		ResetRoom();

		//generate test room
		//GenerateTestRoom();

	}

	private static Texture2D LoadTexture(string path)
	{
		return App.AssetManager.Content.Load<Texture2D>(path);
	}

	private void LoadAssets()
	{
		LoadTiles();
		LoadDecorationTypes();
	}

	private void LoadTiles()
	{
		_wallTiles.Add(new WallTile("StandardWall", LoadTexture("tiles/WallTile")));
		_groundTiles.Add(new GroundTile("StandardGround", LoadTexture("tiles/GroundTile")));

		_groundTiles.Add(new GroundTile("TexturedGround1", LoadTexture("tiles/TexturedGround1")));
		_groundTiles.Add(new GroundTile("TexturedGround2", LoadTexture("tiles/TexturedGround2")));
		_groundTiles.Add(new GroundTile("TexturedGround3", LoadTexture("tiles/TexturedGround3")));
		_groundTiles.Add(new GroundTile("TexturedGround4", LoadTexture("tiles/TexturedGround4")));
		_groundTiles.Add(new GroundTile("TexturedGround5", LoadTexture("tiles/TexturedGround5")));
	}

	private void LoadDecorationTypes()
	{
		_decorationTypes.Add(new DecorationType("chest", LoadTexture("Decoration/chest"), true));
		_decorationTypes.Add(new DecorationType("smudge", LoadTexture("Decoration/smudge"), false));
	}

	private void GenerateNamedTextures()
	{
		_groundTileNamedTextures = _groundTiles.ConvertAll(tile => new NamedTexture(tile.Name, tile.Texture));
		_wallTileNamedTextures = _wallTiles.ConvertAll(tile => new NamedTexture(tile.Name, tile.Texture));
		_decorationNamedTextures = _decorationTypes.ConvertAll(decoration => new NamedTexture(decoration.Name, decoration.Texture));
		_spawnPointNamedTextures = new List<NamedTexture>
		{
			new NamedTexture("Enemy",LoadTexture("Points/EnemyPoint")),
		};

		EditModes =
		[
			_groundTileNamedTextures,
			_wallTileNamedTextures,
			_decorationNamedTextures,
			_spawnPointNamedTextures
		];

		_editModesNamedTextures = new List<NamedTexture>
		{
			new NamedTexture("EditMode: Ground", LoadTexture("tiles/GroundTile")),
			new NamedTexture("EditMode: Wall", LoadTexture("tiles/WallTile")),
			new NamedTexture("EditMode: Decoration", LoadTexture("Decoration/chest")),
			new NamedTexture("EditMode: Spawn Points", LoadTexture("Points/EnemyPoint"))
		};

	}

	private void AddUIElements()
	{
		AddSelectors();
		AddSaveElements();
		AddTypeButton();
	}

	private void AddSelectors()
	{
		GenerateNamedTextures();

		int screenWidth = 1920;

		_assetSelector = new Selector(new Vector2(screenWidth - 80, 32), _groundTileNamedTextures);
		Add(_assetSelector);

		_editModeSelector = new Selector(new Vector2(80, 32), _editModesNamedTextures);
		_editModeSelector.OnChange += () =>
		{
			_currentEditType = _editModeSelector.SelectedIndex;
			_assetSelector.Icons = EditModes[_currentEditType];
			_assetSelector.SelectedIndex = 0;
		};
		Add(_editModeSelector);
	}

	private void AddTypeButton()
	{

	}

	private void AddSaveElements()
	{
		int screenWidth = 1920;
		int screenHeight = 1080;

		TextElement nameText = new TextElement("Fonts/SimpleButtonFont");
		nameText.Text = "Room Name:";
		nameText.Position = new Vector2(screenWidth - 128, screenHeight - 80);
		Add(nameText);

		TextInput nameInput = new TextInput(new Vector2(screenWidth - 128, screenHeight - 64), new Vector2(256, 32));
		Add(nameInput);

		Button saveButton = new Button(new Vector2(screenWidth - 128, screenHeight - 32), new Vector2(128, 32));
		saveButton.Text = "Save";
		saveButton.Clicked += () => { SaveRoom(nameInput.Text); };
		Add(saveButton);
	}

	public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
	{
		base.Draw(gameTime, spriteBatch);
		if (_mouseIsInGrid)
		{
			Rectangle rect = new Rectangle((int)_selectionTilePosition.X, (int)_selectionTilePosition.Y, _tileSize, _tileSize);
			spriteBatch.Draw(_selectionTile, rect, Color.White);
		}
	}

	public override void HandleInput(InputHelper inputHelper)
	{
		base.HandleInput(inputHelper);

		Vector2 mousePosition = inputHelper.MousePosition;

		_mouseIsInGrid =
		mousePosition.X > _gridPosition.X &&
		mousePosition.X < _gridPosition.X + _room.GetSize().X * _tileSize &&
		mousePosition.Y > _gridPosition.Y &&
		mousePosition.Y < _gridPosition.Y + _room.GetSize().Y * _tileSize;

		if (!_mouseIsInGrid)
		{
			return;
		}

		_highlightTile = GlobalPositionToGridPosition(mousePosition);
		_selectionTilePosition = GridPositionToGlobalPosition(_highlightTile);

		if (inputHelper.MouseLeftButtonDown)
		{
			HandleMouseLeftDown(mousePosition);
		}

		if (inputHelper.MouseLeftButtonPressed)
		{
			HandleLeftPress(mousePosition);
		}

		if (inputHelper.MouseRightButtonDown)
		{
			HandleMouseRightPress(mousePosition);
		}

		if (inputHelper.MouseLeftButtonReleased)
		{
			_isDragging = false;
		}
	}

	private void HandleMouseLeftDown(Vector2 mousePosition)
	{
		if (_isDragging && _currentEditType > 2)
		{
			HandleDragging(mousePosition);
			return;
		}

		int selectedTile = _assetSelector.SelectedIndex;
		switch (_currentEditType)
		{
			case 0: //EditMode: Ground
				_room.SetTile(_highlightTile.X, _highlightTile.Y, _groundTiles[selectedTile]);
				break;
			case 1: // EditMode: Wall
				_room.SetTile(_highlightTile.X, _highlightTile.Y, _wallTiles[selectedTile]);
				break;
		}
	}

	private void HandleLeftPress(Vector2 mousePosition)
	{
		Vector2 mouseGridPosition = GlobalPositionToPositionInGrid(mousePosition);

		if (_currentEditType == 3)
		{
			if (!GrabDecoration(mouseGridPosition))
			{
				AddDecoration((int)mouseGridPosition.X, (int)mouseGridPosition.Y);
			}
		}
		else if (_currentEditType == 4)
		{
			if (!GrabSpawnPoint(mouseGridPosition))
			{
				AddSpawnPoint((int)mouseGridPosition.X, (int)mouseGridPosition.Y);
			}
		}
	}

	private void HandleMouseRightPress(Vector2 mousePosition)
	{
		Vector2 mouseGridPosition = GlobalPositionToPositionInGrid(mousePosition);
		if (_currentEditType <= 2)
		{
			_room.DeleteTile(_highlightTile.X, _highlightTile.Y);
		}
		if (_currentEditType == 3)
		{
			RemoveDecoration(mouseGridPosition);
		}
		if (_currentEditType == 4)
		{
			RemoveSpawnPoint(mouseGridPosition);
		}
	}

	private void HandleDragging(Vector2 mousePosition)
	{
		if (!_isDragging)
		{
			return;
		}
		if (_currentEditType == 3)
		{
			_currentDraggingDecoration.Position = GlobalPositionToPositionInGrid(mousePosition);
		}
		else if (_currentEditType == 4)
		{
			Vector2 mouseGridPosition = GlobalPositionToPositionInGrid(mousePosition);

			_currentDraggingSpawnPoint.X = (int)mouseGridPosition.X;
			_currentDraggingSpawnPoint.Y = (int)mouseGridPosition.Y;
		}
	}

	private void AddDecoration(int x, int y)
	{
		_room.AddDecoration(new Decoration(_decorationTypes[_assetSelector.SelectedIndex], new Vector2(x, y), _tileSize * 3 / 4));
	}

	private void RemoveDecoration(Vector2 position)
	{
		_room.RemoveDecoration((int)position.X, (int)position.Y, _tileSize / 2);
	}

	private bool GrabDecoration(Vector2 position)
	{
		foreach (Decoration decoration in _room.GetDecorations())
		{
			if (Vector2.Distance(decoration.Position, position) < _tileSize / 2)
			{
				_currentDraggingDecoration = decoration;
				_isDragging = true;
				return true;
			}
		}
		return false;
	}

	private void AddSpawnPoint(int x, int y)
	{
		switch (_assetSelector.SelectedIndex)
		{
			case 0:
				_room.AddEnemySpawnPoint(x, y);
				break;
		}
	}

	private void RemoveSpawnPoint(Vector2 position)
	{
		_room.RemoveSpawnPoint((int)position.X, (int)position.Y, _tileSize / 2);
	}

	private bool GrabSpawnPoint(Vector2 mousePosition)
	{
		Vector2Int[] spawnPointsToCheck;
		switch (_assetSelector.SelectedIndex)
		{
			case 0:
				spawnPointsToCheck = _room.GetEnemySpawnPoints();
				break;
			default:
				return false;
		}
		foreach (Vector2Int SpawnPoint in spawnPointsToCheck)
		{
			if (Vector2.Distance(new Vector2(SpawnPoint.X, SpawnPoint.Y), mousePosition) < _tileSize / 2)
			{
				_currentDraggingSpawnPoint = SpawnPoint;
				_isDragging = true;
				return true;
			}
		}
		return false;
	}

	private Vector2Int GlobalPositionToGridPosition(Vector2 globalPosition)
	{
		Vector2 gridPosition = globalPosition - _gridPosition;
		return new Vector2Int((int)(gridPosition.X / _tileSize), (int)(gridPosition.Y / _tileSize));
	}

	private Vector2 GlobalPositionToPositionInGrid(Vector2 globalPosition)
	{
		return globalPosition - _gridPosition;
	}

	private Vector2 GridPositionToGlobalPosition(Vector2Int gridPosition)
	{
		return _gridPosition + new Vector2(gridPosition.X * _tileSize, gridPosition.Y * _tileSize);
	}

	public void ResetRoom()
	{
		_room.ResetRoom();
		GenerateStandardRoom();
	}


	private void GenerateStandardRoom()
	{
		for (int x = 0; x < _room.GetSize().X; x++)
		{
			for (int y = 0; y < _room.GetSize().Y; y++)
			{
				SetTileInStandardRoom(x, y);
			}
		}
	}

	private void SetTileInStandardRoom(int x, int y)
	{
		int _roomWidth = _room.GetSize().X;
		int _roomHeight = _room.GetSize().Y;
		if (x == 0 || y == 0 || x == _roomWidth - 1 || y == _roomHeight - 1)
		{
			if (-1 <= (int)(_roomWidth / 2f) - x && (int)(_roomWidth / 2f) - x <= 1)
			{
				_room.SetTile(x, y, _groundTiles[0]);
			}
			else
			{
				_room.SetTile(x, y, _wallTiles[0]);
			}
		}
		else
		{
			_room.SetTile(x, y, _groundTiles[0]);
		}
	}

	private bool SaveRoom(string name)
	{
		if (!IsValidName(name))
		{
			return false;
		}
		name = _room.GetSize().X + "x" + _room.GetSize().Y + "_" + name;

		RoomSaver.SaveRoom(new Room(name, _room));
		App.ScreenManager.SwitchTo(ScreenManager.TITLE_SCREEN);
		ResetRoom();
		return true;
	}

	private bool IsValidName(string name)
	{
		if (name.Length < 4) return false;
		if (name.Length > 100) return false;
		return true;
	}

	//* debug function
	private static void PrintRoom(string name)
	{
		Room _room = RoomSaver.LoadRoom(name);
		Console.WriteLine(_room);
	}


	public override void Reset()
	{
		base.Reset();
		ResetRoom();
		//App.AssetManager.AudioManager.PlaySong("RoomEditorMusic");

	}
}
