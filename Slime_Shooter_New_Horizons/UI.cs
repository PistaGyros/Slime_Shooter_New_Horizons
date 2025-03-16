using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class UI
{
    private Vector2 position;
    private Texture2D bgTexture;
    private Texture2D fgTexture;
    
    
    public UI(Vector2 UIPosition, Texture2D backgroundTexture, Texture2D forGroundTexture)
    {
        this.position = UIPosition;
        this.bgTexture = backgroundTexture;
        this.fgTexture = forGroundTexture;
        
    }

    public UI(Vector2 UIPosition, Texture2D backgroundTexture)
    {
        this.position = UIPosition;
        this.bgTexture = backgroundTexture;
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        DrawBack(spriteBatch);
        DrawForeground(spriteBatch);
    }

    public void DrawBack(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(bgTexture, position, Color.White);
    }

    private void DrawForeground(SpriteBatch spriteBatch)
    {
        
    }
}