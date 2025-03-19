using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slime_Shooter_New_Horizons;

public class Slime : Animator
{
    private bool isThrowed = false;
    public bool IsVacuumed = false;
    
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
    

    public new void Update(GameTime gameTime, Rectangle playerRec)
    {
        UpdateAnimator(gameTime);
        if (IsVacuumed)
        {
            destinationRectangle = Vacuum(playerRec, destinationRectangle, gameTime);
        }
        else if (isThrowed)
            destinationRectangle = Fly(gameTime, destinationRectangle);


        if (initQuadrant == 1 | initQuadrant == 3 && destinationRectangle.Y >= initPos.Y + 46)
        {
            isThrowed = false;
        }
        else if (initQuadrant == 4 && destinationRectangle.Y >= initPos.Y + 146)
            isThrowed = false;
        else if (initQuadrant == 2 && destinationRectangle.Y <= initPos.Y - 146)
            isThrowed = false;
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
                gravityAcceleration = 0f;
                break;
            case 3:
                quadrantVec = new Vector2(-1, 1);
                gravityAcceleration = 100f;
                break;
            case 4:
                quadrantVec = new Vector2(0, -1);
                gravityAcceleration = 0f;
                break;
        }

        return quadrantVec;
    }
}