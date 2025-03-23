using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slime_Shooter_New_Horizons;

public class Slime : Animator
{
    public int slimeID;
    private bool isCollidingWithSlime = false;


    public Slime(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle,
        List<List<Rectangle>> objectsColRecs, float scaleMultiplier) :
        base(texture, destinationRectangle, sourceRectangle, objectsColRecs, scaleMultiplier)
    {
    }

    public void ThrowSlime(int quadrantSpawned)
    {
        Throw(destinationRectangle);
        isThrowed = true;
        initQuadrant = quadrantSpawned;
        velocity *= DecideWhatinitQuadrant(quadrantSpawned);
    }
    

    public new void Update(GameTime gameTime, Rectangle playerRec, List<Slime> slimeList)
    {
        Rectangle collidedSlimeRec = new();
        UpdateAnimator(gameTime);
        var outputOfChecking = CheckForCollisionsWithSlimes(slimeList);
        
        if (outputOfChecking.Item1)
        {
            IsVacuumed = false;
            isThrowed = false;
            isCollidingWithSlime = true;
            collidedSlimeRec = outputOfChecking.Item2;
        }
        else
        {
            isCollidingWithSlime = false;
        }

        if (isCollidingWithSlime)
        {
            BounceAwayFromSlime(collidedSlimeRec);
        }

        else if (!isCollidingWithSlime)
        {
            UpdateSprite(gameTime, playerRec);
        }
    }

    private void BounceAwayFromSlime(Rectangle badSlime)
    {
        Vector2 centerDestRec = new Vector2(GetCollisionRectangle().X + GetCollisionRectangle().Width / 2,
            GetCollisionRectangle().Y + GetCollisionRectangle().Height / 2);
        Vector2 centerBadSlimeRec = new Vector2(badSlime.X + badSlime.Width / 2, badSlime.Y + badSlime.Height / 2);
        Vector2 pointVec = new Vector2(centerDestRec.X - centerBadSlimeRec.X, centerDestRec.Y - centerBadSlimeRec.Y);
        destinationRectangle.X += (int)pointVec.X;
        destinationRectangle.Y += (int)pointVec.Y;
    }
    
    public new (bool, Rectangle) CheckForCollisionsWithSlimes(List<Slime> slimeList)
    {
        bool collision = false;
        Rectangle collidedRectangle = new Rectangle();
        foreach (var slime in slimeList)
        {
            if (this != slime)
                if(GetCollisionRectangle().Intersects(slime.GetCollisionRectangle()))
                {
                    collision = true;
                    collidedRectangle = slime.GetCollisionRectangle();
                    slime.isThrowed = false;
                }
        }

        return (collision, collidedRectangle);
    }
}