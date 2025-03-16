using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slime_Shooter_New_Horizons;

public class Slime : Animator
{
    private bool fly;
    private float time;
    private Vector2 initVelocity;
    private Vector2 initPos;
    private int initQuadrant;
    private float power = 200;
    private float gravityAcceleration;
    
    public Slime(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Vector2 colliderSize,
        float scaleMultiplier, int numFrames, int numCollums, Vector2 size, int quadrantSpawned) :
        base(texture, destinationRectangle, sourceRectangle, colliderSize, scaleMultiplier, numFrames, numCollums, size)
    {
        initVelocity = new Vector2(MathF.Cos(40 * MathF.PI / 180), MathF.Sin(215 * MathF.PI / 180)) * power;
        initPos = new Vector2(destinationRectangle.X, destinationRectangle.Y);
        initQuadrant = quadrantSpawned;
        initVelocity *= DecideWhatinitQuadrant(quadrantSpawned);
        fly = true;
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

    public override void Update(GameTime gameTime)
    {
        UpdateAnimator(gameTime);
        if (fly)
        {
            Fly(gameTime);
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
    
    private float KinematicEquation(float acceleration, float velocity, float position, float time)
    {
        return 0.5f * acceleration * time * time + velocity * time + position;
    }

    private void Fly(GameTime gameTime)
    {
        time += (float)gameTime.ElapsedGameTime.TotalSeconds * 3;

        float newX = KinematicEquation(5f, initVelocity.X, initPos.X, time);
        float newY = KinematicEquation(gravityAcceleration, initVelocity.Y, initPos.Y, time);
        
        destinationRectangle.X = (int)newX;
        destinationRectangle.Y = (int)newY;
    }
}