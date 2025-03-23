using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class FruitVeggie : Sprite
{
    public int fruitVeggieID;
    
    public FruitVeggie(int fruitVeggieID,Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle,
        float scaleMultiplier) : base(texture, destinationRectangle, sourceRectangle, scaleMultiplier)
    {
        this.fruitVeggieID = fruitVeggieID;
    }
}