using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using GMTK2025.Engine;
using System;
using Microsoft.Xna.Framework.Input;

namespace GMTK2025;

public class App : Game
{
    public static GraphicsDeviceManager Graphics;
    public static SpriteBatch SpriteBatch;
    public static InputHelper InputHelper;
    public static ScreenManager ScreenManager;
    public static App Instance;
    public static Random Random;
    public static Matrix SpriteScale;
    public static AssetManager AssetManager;
    public static AudioManager AudioManager => AssetManager.AudioManager;
    public static Point windowSize;

    public static Point screen;

    public static bool DoClear = true;


    public static Color BackgroundColor = new Color(32, 32, 32, 255);

    public App()
    {
        IsMouseVisible = true;
        Graphics = new GraphicsDeviceManager(this);

        Content.RootDirectory = "Content";
        AssetManager = new AssetManager(Content);
        IsMouseVisible = true;
        Instance = this;

        InputHelper = new InputHelper();

        ScreenManager = new ScreenManager();

        SpriteScale = Matrix.CreateScale(1, 1, 1);
        Random = new Random();

        screen = new Point(1920, 1080);

        ApplyResolutionSettings(true, true);
    }

    protected override void Initialize()
    {
        IsMouseVisible = true;
        ScreenManager.Initialize();
        ScreenManager.SwitchTo(ScreenManager.TITLE_SCREEN);
        base.Initialize();
    }

    public bool FullScreen
    {
        get { return Graphics.IsFullScreen; }
        set
        {
            ApplyResolutionSettings(value);
        }
    }

    public void ApplyResolutionSettings(bool fullScreen = false, bool borderLess = false)
    {
        Window.ClientSizeChanged -= (sender, args) => ApplyResolutionSettings(false);

        if (!fullScreen)
        {
            Graphics.PreferredBackBufferWidth = Window.ClientBounds.Width;
            Graphics.PreferredBackBufferHeight = Window.ClientBounds.Height;
            Graphics.IsFullScreen = false;
            Window.AllowUserResizing = true;
            Graphics.HardwareModeSwitch = false;

            Window.ClientSizeChanged += (sender, args) => ApplyResolutionSettings(false);
        }
        else
        {
            Graphics.PreferredBackBufferWidth = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Width;
            Graphics.PreferredBackBufferHeight = GraphicsAdapter.DefaultAdapter.CurrentDisplayMode.Height;
            Graphics.IsFullScreen = true;
        }

        Graphics.HardwareModeSwitch = !borderLess;

        Graphics.ApplyChanges();

        float targetAspectRatio = (float)screen.X / (float)screen.Y;
        int width = Graphics.PreferredBackBufferWidth;
        int height = (int)(width / targetAspectRatio);
        if (height > Graphics.PreferredBackBufferHeight)
        {
            height = Graphics.PreferredBackBufferHeight;
            width = (int)(height * targetAspectRatio);
        }

        Viewport viewport = new Viewport();
        viewport.X = (Graphics.PreferredBackBufferWidth / 2) - (width / 2);
        viewport.Y = (Graphics.PreferredBackBufferHeight / 2) - (height / 2);
        viewport.Width = width;
        viewport.Height = height;
        GraphicsDevice.Viewport = viewport;

        InputHelper.Scale = new Vector2((float)GraphicsDevice.Viewport.Width / screen.X,
                                        (float)GraphicsDevice.Viewport.Height / screen.Y);
        InputHelper.Offset = new Vector2(viewport.X, viewport.Y);
        SpriteScale = Matrix.CreateScale(InputHelper.Scale.X, InputHelper.Scale.Y, 1);
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);
        base.LoadContent();
    }

    protected void HandleInput()
    {
        InputHelper.Update();
        if (InputHelper.KeyPressed(Keys.Escape))
        {
            ScreenManager.SwitchTo(ScreenManager.TITLE_SCREEN);
        }
        if (InputHelper.KeyPressed(Keys.F5))
        {
            FullScreen = !FullScreen;
        }
        ScreenManager.HandleInput(InputHelper);
    }

    protected override void Update(GameTime gameTime)
    {
        if (!IsActive) return;
        HandleInput();
        ScreenManager.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        if (!IsActive) return;
        if (DoClear)
        {
            GraphicsDevice.Clear(BackgroundColor);
        }
        SpriteBatch.Begin(SpriteSortMode.Deferred, null, null, null, null, null, SpriteScale);
        ScreenManager.Draw(gameTime, SpriteBatch);
        SpriteBatch.End();
    }
}
