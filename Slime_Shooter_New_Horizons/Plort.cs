using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class Plort(
    int plortId,
    Texture2D texture,
    Rectangle destinationRectangle,
    Rectangle sourceRectangle,
    float scaleMultiplier)
    : Sprite(texture, destinationRectangle, sourceRectangle, scaleMultiplier)
{
    public int plortID = plortId;
    public bool isCollidingWithObject;

    public void ThrowPlort(int quadrantSpawned)
    {
        Throw(destinationRectangle);
        isThrowed = true;
        initQuadrant = quadrantSpawned;
        velocity *= DecideWhatinitQuadrant(quadrantSpawned);
    }

    public new void Update(GameTime gameTime, Rectangle playerRec, List<Slime> slimeList, List<Plort> plortsList)
    {
        Console.WriteLine(isThrowed);
        Rectangle collidedObjectRec = new();
        var outputOfChecking = CheckForCollisionsWithSlimesPlorts(slimeList, plortsList);
        
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
            BounceAwayFromSlimePlortFruitVeggie(collidedObjectRec);
        }

        else if (!isCollidingWithObject)
        {
            UpdateSprite(gameTime, playerRec);
        }
    }
    
    public new void BounceAwayFromSlimePlortFruitVeggie(Rectangle badObject)
    {
        Vector2 centerDestRec = new Vector2(GetCollisionRectangle().X + GetCollisionRectangle().Width / 2,
            GetCollisionRectangle().Y + GetCollisionRectangle().Height / 2);
        Vector2 centerBadObjectRec = new Vector2(badObject.X + badObject.Width / 2, badObject.Y + badObject.Height / 2);
        Vector2 pointVec = new Vector2(centerDestRec.X - centerBadObjectRec.X, centerDestRec.Y - centerBadObjectRec.Y);
        destinationRectangle.X += (int)pointVec.X;
        destinationRectangle.Y += (int)pointVec.Y;
    }
    
    public new (bool, Rectangle) CheckForCollisionsWithSlimesPlorts(
        List<Slime> slimeList, List<Plort> plortsList)
    {
        bool collision = false;
        Rectangle collidedRectangle = new Rectangle();
        foreach (var plort in plortsList)
        {
            if (this != plort)
                if(GetCollisionRectangle().Intersects(plort.GetCollisionRectangle()))
                {
                    collision = true;
                    collidedRectangle = plort.GetCollisionRectangle();
                    isThrowed = false;
                    plort.isThrowed = false;
                    Console.WriteLine("Plort has collided with plort");
                }
        }
        foreach (var slime in slimeList)
        {
            if(GetCollisionRectangle().Intersects(slime.GetCollisionRectangle()))
            {
                collision = true;
                collidedRectangle = slime.GetCollisionRectangle();
                isThrowed = false;
                Console.WriteLine("Plort has collided with slime");
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