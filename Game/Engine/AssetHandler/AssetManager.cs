using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace GMTK2025.Engine;

public class AssetManager
{
	protected ContentManager contentManager;
	protected AudioManager audioManager;

	public AudioManager AudioManager => audioManager;

	public AssetManager(ContentManager content)
	{
		contentManager = content;
		audioManager = new AudioManager();
		audioManager.LoadAllAudio(contentManager);
	}

	public Texture2D GetTexture(string assetName)
	{
		if (assetName == "")
		{
			return null;
		}
		return contentManager.Load<Texture2D>(assetName);
	}

	public ContentManager Content
	{
		get { return contentManager; }
	}
}
