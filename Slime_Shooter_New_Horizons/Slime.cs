﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slime_Shooter_New_Horizons;

public class Slime : Animator
{
    public Slime(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Vector2 colliderSize,
        float scaleMultiplier, int numFrames, int numCollums, Vector2 size, int quadrantSpawned) :
        base(texture, destinationRectangle, sourceRectangle, colliderSize, scaleMultiplier, numFrames, numCollums, size)
    {
        Throw(destinationRectangle);
        initQuadrant = quadrantSpawned;
        initVelocity *= DecideWhatinitQuadrant(quadrantSpawned);
    }

    

    public override void Update(GameTime gameTime)
    {
        UpdateAnimator(gameTime);
        if (fly)
        {
            destinationRectangle = Fly(gameTime, destinationRectangle);
        }

        if (initQuadrant == 1 | initQuadrant == 3 && destinationRectangle.Y >= initPos.Y + 46)
        {
            fly = false;
        }
        else if (initQuadrant == 4 && destinationRectangle.Y >= initPos.Y + 146)
            fly = false;
        else if (initQuadrant == 2 && destinationRectangle.Y <= initPos.Y - 146)
            fly = false;
    }
    
    
    private Vector2 DecideWhatinitQuadrant(int quadrantSpawned)
    {
        Vector2 quadrantVec = Vector2.Zero;
        Console.WriteLine(quadrantSpawned);
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