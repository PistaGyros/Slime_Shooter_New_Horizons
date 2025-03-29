using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class UI
{
    private Vector2 position;
    private Rectangle bgRectangle;
    private Texture2D bgTexture;
    private Texture2D fgTexture;
    private SpriteFont font;

    public int SizeScaler = 3;
    
    
    public UI(Rectangle bgRectangle, Texture2D backGroundTexture, Texture2D forGroundTexture, SpriteFont font)
    {
        this.bgRectangle = bgRectangle;
        this.bgTexture = backGroundTexture;
        this.fgTexture = forGroundTexture;
        this.font = font;
    }

    public UI(Rectangle bgRectangle, Texture2D backGroundTexture, SpriteFont font)
    {
        this.bgRectangle = bgRectangle;
        this.bgTexture = backGroundTexture;
        this.font = font;
    }

    public UI(Vector2 position, Texture2D backGroundTexture, Texture2D forGroundTexture, SpriteFont font)
    {
        this.position = position;
        this.bgTexture = backGroundTexture;
        this.fgTexture = forGroundTexture;
        this.font = font;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        DrawBack(spriteBatch);
        DrawForeground(spriteBatch);
    }

    public virtual void DrawBack(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(bgTexture, bgRectangle, Color.White);
    }

    private void DrawForeground(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(fgTexture, bgRectangle, Color.White);
    }
    
    public void ChangeSizeScale(int sizeScaler)
    {
        this.SizeScaler = sizeScaler;
    }
}