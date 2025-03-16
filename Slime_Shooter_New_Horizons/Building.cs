using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slime_Shooter_New_Horizons;

public class Building : Sprite
{
    public Building(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, float scaleMultiplier, 
        Vector2 colliderSize) : base(texture, destinationRectangle, sourceRectangle, scaleMultiplier, colliderSize)
    {
        
    }

    public Building(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle,
        float scaleMultiplier) :
        base(texture, destinationRectangle, sourceRectangle, scaleMultiplier)
    {
        
    }
}