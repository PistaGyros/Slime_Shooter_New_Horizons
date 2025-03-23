using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class Plort : Sprite
{
    public int plortID;
    
    public Plort(int plortID, Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle,
        float scaleMultiplier) : base(texture, destinationRectangle, sourceRectangle, scaleMultiplier)
    {
        this.plortID = plortID;
    }
    
    public void ThrowPlort(int quadrantSpawned)
    {
        Throw(destinationRectangle);
        isThrowed = true;
        initQuadrant = quadrantSpawned;
        velocity *= DecideWhatinitQuadrant(quadrantSpawned);
    }

    public new void Update(GameTime gameTime, Rectangle playerRec)
    {
        UpdateSprite(gameTime, playerRec);
    }
    
    public new Rectangle GetCollisionRectangle()
    {
        return new Rectangle(destinationRectangle.X, destinationRectangle.Y, 
            destinationRectangle.Width * (int)scaleMultiplier, destinationRectangle.Height * (int)scaleMultiplier);
    }
}