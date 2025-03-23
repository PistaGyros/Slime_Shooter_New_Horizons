using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class FruitVeggie(
    int fruitVeggieId,
    Texture2D texture,
    Rectangle destinationRectangle,
    Rectangle sourceRectangle,
    float scaleMultiplier)
    : Sprite(texture, destinationRectangle, sourceRectangle, scaleMultiplier)
{
    public int fruitVeggieID = fruitVeggieId;
    public bool isCollidingWithObject;

    public void ThrowFruitVeggie(int quadrantSpawned)
    {
        Throw(destinationRectangle);
        isThrowed = true;
        initQuadrant = quadrantSpawned;
        velocity *= DecideWhatinitQuadrant(quadrantSpawned);
    }

    public new void Update(GameTime gameTime, Rectangle playerRec, List<FruitVeggie> fruitVeggiesList)
    {
        Rectangle collidedObjectRec = new();
        var outputOfChecking = CheckForCollisionsWithFruitsVeggies(fruitVeggiesList);
        
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
            UpdateSprite(gameTime, playerRec);
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
    
    public new (bool, Rectangle) CheckForCollisionsWithFruitsVeggies(List<FruitVeggie> fruitVeggiesList)
    {
        bool collision = false;
        Rectangle collidedRectangle = new Rectangle();
        foreach (var fruitVeggie in fruitVeggiesList)
        {
            if (this != fruitVeggie)
            {
                if (GetCollisionRectangle().Intersects(fruitVeggie.GetCollisionRectangle()))
                {
                    collision = true;
                    collidedRectangle = fruitVeggie.GetCollisionRectangle();
                    isThrowed = false;
                    fruitVeggie.isThrowed = false;
                    Console.WriteLine("FruitVeggie has collided with fruitVeggie");
                }
            }
        }
        return (collision, collidedRectangle);
    }
    
    public new Rectangle GetCollisionRectangle()
    {
        return new Rectangle(destinationRectangle.X, destinationRectangle.Y, 
            destinationRectangle.Width * (int)scaleMultiplier, destinationRectangle.Height * (int)scaleMultiplier);
    }
}