using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slime_Shooter_New_Horizons;

public class Slime : Animator
{
    public bool isThrowed = false;
    public bool IsVacuumed = false;
    public int slimeID;
    private bool isCollidingWithSlime = false;
    
    
    public Slime(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, float scaleMultiplier, 
        Vector2 colliderSize, Texture2D colliderTexture) :
        base(texture, destinationRectangle, sourceRectangle, scaleMultiplier, colliderSize, colliderTexture)
    {
    }

    public Slime(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, float scaleMultiplier, 
        Vector2 colliderSize, Texture2D colliderTexture, int numFrames, int numCollums, int numRows, Vector2 size, 
        int animSpeedMultiplier) : base(texture, destinationRectangle, sourceRectangle, 
        scaleMultiplier, colliderSize, colliderTexture, numFrames, numCollums, numRows, size, animSpeedMultiplier)
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
            if (IsVacuumed)
            {
                destinationRectangle = Vacuum(playerRec, destinationRectangle, gameTime);
            }
            else if (isThrowed)
            {
                destinationRectangle = Fly(gameTime, destinationRectangle);
            
                if (initQuadrant == 1 | initQuadrant == 3 && destinationRectangle.Y >= initPos.Y + 46)
                {
                    isThrowed = false;
                }
                else if ((initQuadrant == 4 || initQuadrant == 2) &&
                         (destinationRectangle.Y >= initPos.Y + 146 || destinationRectangle.Y <= initPos.Y - 146))
                {
                    isThrowed = false;
                }
            }
        }
    }

    private void BounceAwayFromSlime(Rectangle badSlime)
    {
        Vector2 centerDestRec = new Vector2(destinationRectangle.X + destinationRectangle.Width / 2,
            destinationRectangle.Y + destinationRectangle.Height / 2);
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
                if(destinationRectangle.Intersects(slime.destinationRectangle))
                {
                    collision = true;
                    collidedRectangle = slime.destinationRectangle;
                    slime.isThrowed = false;
                }
        }

        return (collision, collidedRectangle);
    }
    
    
    private Vector2 DecideWhatinitQuadrant(int quadrantSpawned)
    {
        Vector2 quadrantVec = Vector2.Zero;
        switch (quadrantSpawned)
        {
            case 1:
                quadrantVec = new Vector2(1, 1);
                gravityAcceleration = 100f;
                break;
            case 2:
                quadrantVec = new Vector2(0, 1);
                gravityAcceleration = 1f;
                break;
            case 3:
                quadrantVec = new Vector2(-1, 1);
                gravityAcceleration = 100f;
                break;
            case 4:
                quadrantVec = new Vector2(0, -1);
                gravityAcceleration = 1f;
                break;
        }

        return quadrantVec;
    }
}