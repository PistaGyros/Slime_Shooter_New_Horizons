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
    
    
    public UI(Rectangle UIRec, Texture2D backGroundTexture, Texture2D forGroundTexture)
    {
        this.UIRec = UIRec;
        this.bgTexture = backGroundTexture;
        this.fgTexture = forGroundTexture;
        
    }

    public UI(Rectangle UIRec, Texture2D backGroundTexture)
    {
        this.UIRec = UIRec;
        this.bgTexture = backGroundTexture;
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