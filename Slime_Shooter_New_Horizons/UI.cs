using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class UI
{
    private Rectangle UIRec;
    private Texture2D bgTexture;
    private Texture2D fgTexture;
    private SpriteFont font;
    
    
    public UI(Rectangle UIRec, Texture2D backGroundTexture, Texture2D forGroundTexture, SpriteFont font)
    {
        this.UIRec = UIRec;
        this.bgTexture = backGroundTexture;
        this.fgTexture = forGroundTexture;
        this.font = font;
    }

    public UI(Rectangle UIRec, Texture2D backGroundTexture, SpriteFont font)
    {
        this.UIRec = UIRec;
        this.bgTexture = backGroundTexture;
        this.font = font;
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        DrawBack(spriteBatch);
        DrawForeground(spriteBatch);
    }

    public virtual void DrawBack(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(bgTexture, UIRec, Color.White);
    }

    private void DrawForeground(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(fgTexture, UIRec, Color.White);
    }
}