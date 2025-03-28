using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class StatusBars : UI
{
    
    
    public StatusBars(Rectangle UIRec, Texture2D backGroundTexture, Texture2D forGroundTexture, SpriteFont font) :
        base(UIRec, backGroundTexture, forGroundTexture, font)
    {
        
    }
}