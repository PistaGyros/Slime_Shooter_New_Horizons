using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class Inventory : UI
{
    private Texture2D atlasItemsTexture;
    private int itemTexSize = 16;
    private Dictionary<string, int> itemsID;
    private Dictionary<int, Texture2D> itemsIDTextures;
    
    private Dictionary<int, int> inventoryItems;
    
    
    public Inventory(Vector2 UIPosition, Texture2D backgroundTexture, Texture2D forGroundTexture,
        Dictionary<string, int> itemsID) : base(UIPosition, backgroundTexture, forGroundTexture)
    {
        this.atlasItemsTexture = forGroundTexture;
        this.itemsID = itemsID;
        AssignTexturesToIDs(atlasItemsTexture);
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        DrawBack(spriteBatch);
        DrawItems(spriteBatch);
    }
    
    private void AssignTexturesToIDs(Texture2D backgroundTexture)
    {
        
    }

    private void DrawItems(SpriteBatch spriteBatch)
    {
        
    }
}