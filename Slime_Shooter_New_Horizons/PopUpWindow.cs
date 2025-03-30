using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class PopUpWindow : UI
{
    private Rectangle bgRec;
    private Texture2D bgTex;
    private SpriteFont font;
    
    public PopUpWindow(Rectangle bgRectangle, Texture2D backGroundTexture, SpriteFont font)
    : base(bgRectangle, backGroundTexture, font)
    {
        bgRec = bgRectangle;
        bgTex = backGroundTexture;
        this.font = font;
    }

    public new void Update(GameTime gameTime)
    {
        
    }

    public new void Draw(SpriteBatch spriteBatch)
    {
        // Draw bg
        spriteBatch.Draw(bgTex, bgRec, new Rectangle(2, 0, 1, 1), Color.White);
    }
}