using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slime_Shooter_New_Horizons;

public class TileMap
{
    private Dictionary<Vector2, int> tileMap;
    private List<Rectangle> textureStore;
    private Texture2D tilemapAtlas;
    
    public TileMap(string mapFileLocation, List<Rectangle> textureStore, Texture2D tilemapAtlas)
    {
        tileMap = LoadMap(mapFileLocation);
        this.textureStore = textureStore;
        this.tilemapAtlas = tilemapAtlas;
    }
    
    private Dictionary<Vector2, int> LoadMap(string filepath)
    {
        Dictionary<Vector2, int> result = new ();
        
        StreamReader reader = new (filepath);
        int y = 0;
        string line;
        while ((line = reader.ReadLine()) != null)
        {
            string[] items = line.Split(',');

            for (int x = 0; x < items.Length; x++)
            {
                if (int.TryParse(items[x], out int value))
                {
                    if (value > 0)
                    {
                        result[new Vector2(x, y)] = value;
                    }
                }
            }
            y++;
        }
        
        return result;
    }
    
    public virtual void Update(GameTime gameTime){}

    public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset, int tileSize)
    {
        foreach (var item in tileMap)
        {
            Rectangle dest = new Rectangle(
                (int) (item.Key.X * tileSize) + (int)offset.X,
                (int) (item.Key.Y * tileSize) + (int)offset.Y,
                tileSize, tileSize);
            Rectangle src = textureStore[item.Value - 1];
            spriteBatch.Draw(tilemapAtlas, dest, src, Color.White);
        }
    }
}