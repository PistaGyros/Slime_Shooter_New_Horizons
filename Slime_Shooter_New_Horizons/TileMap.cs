using System.Collections.Generic;
using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slime_Shooter_New_Horizons;

public class TileMap
{
    private Dictionary<Vector2, int> tileMap;
    private List<Rectangle> textureStore;
    public List<Rectangle> collisionRecs;
    private Texture2D tilemapAtlas;
    private int tileSize;
    private float scaleMultiplier;
    
    public TileMap(string mapFileLocation, List<Rectangle> textureStore, Texture2D tilemapAtlas, int tileSize,
        float scaleMultiplier)
    {
        tileMap = LoadMap(mapFileLocation);
        this.textureStore = textureStore;
        this.tilemapAtlas = tilemapAtlas;
        this.tileSize = tileSize;
        this.scaleMultiplier = scaleMultiplier;
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

    private List<Rectangle> LoadCollisionRecs(string filepath)
    {
        List<Rectangle> result = new();
        return result;
    }
    
    public virtual void Update(GameTime gameTime){}

    public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset)
    {
        foreach (var item in tileMap)
        {
            Rectangle dest = new Rectangle(
                (int) (item.Key.X * tileSize * scaleMultiplier) + (int)offset.X,
                (int) (item.Key.Y * tileSize * scaleMultiplier) + (int)offset.Y,
                (int)(tileSize * scaleMultiplier), (int)(tileSize * scaleMultiplier));
            Rectangle src = textureStore[item.Value - 1];
            spriteBatch.Draw(tilemapAtlas, dest, src, Color.White);
        }
    }
}