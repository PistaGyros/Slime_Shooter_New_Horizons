using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class FruitVeggie : Sprite
{
    public int fruitVeggieID;
    public bool isCollidingWithObject;
    
    
    public FruitVeggie(int fruitVeggieId,
        Texture2D texture,
        Rectangle destinationRectangle,
        Rectangle sourceRectangle,
        float scaleMultiplier)
        : base(texture, destinationRectangle, sourceRectangle, scaleMultiplier)
    {
        this.fruitVeggieID = fruitVeggieId;
    }

    public void ThrowFruitVeggie(int quadrantSpawned)
    {
        Throw(destinationRectangle);
        isThrowed = true;
        initQuadrant = quadrantSpawned;
        velocity *= DecideWhatinitQuadrant(quadrantSpawned);
    }

    public new void Update(GameTime gameTime, Rectangle playerRec)
    {
        Rectangle collidedObjectRec = new();
        var outputOfChecking = CheckForCollisionsWithFruitsVeggies();
        
        if (outputOfChecking.Item1)
        {
            IsVacuumed = false;
            isThrowed = false;
            isCollidingWithObject = true;
            collidedObjectRec = outputOfChecking.Item2;
        }
        else
        {
            isCollidingWithObject = false;
        }

        if (isCollidingWithObject)
        {
            BounceAwayFromFruitVeggie(collidedObjectRec);
        }

        else if (!isCollidingWithObject)
        {
            UpdateSprite(gameTime, playerRec, destinationRectangle, GetCollisionRectangle());
        }
    }
    
    public new void BounceAwayFromFruitVeggie(Rectangle badObject)
    {
        Vector2 centerDestRec = new Vector2(GetCollisionRectangle().X + GetCollisionRectangle().Width / 2,
            GetCollisionRectangle().Y + GetCollisionRectangle().Height / 2);
        Vector2 centerBadObjectRec = new Vector2(badObject.X + badObject.Width / 2, badObject.Y + badObject.Height / 2);
        Vector2 pointVec = new Vector2(centerDestRec.X - centerBadObjectRec.X, centerDestRec.Y - centerBadObjectRec.Y);
        destinationRectangle.X += (int)(pointVec.X / 2);
        destinationRectangle.Y += (int)(pointVec.Y / 2);
    }
    
    public new (bool, Rectangle) CheckForCollisionsWithFruitsVeggies()
    {
        bool collision = false;
        Rectangle collidedRectangle = new Rectangle();
        
        foreach (var fruitVeggie in fruitsVeggiesList)
        {
            if (this != fruitVeggie)
            {
                if (GetCollisionRectangle().Intersects(fruitVeggie.GetCollisionRectangle()))
                {
                    collision = true;
                    collidedRectangle = fruitVeggie.GetCollisionRectangle();
                    isThrowed = false;
                    fruitVeggie.isThrowed = false;
                }
            }
        }
        return (collision, collidedRectangle);
    }
}