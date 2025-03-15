using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slime_Shooter_New_Horizons;

public class Slime : Sprite
{
    private bool fly;
    private float time;
    private Vector2 initVelocity;
    private Vector2 initPos;
    private float power = 200;
    
    public Slime(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Vector2 colliderSize,
        float scaleMultiplier, int quadrantSpawned) :
        base(texture, destinationRectangle, sourceRectangle, colliderSize, scaleMultiplier)
    {
        initVelocity = new Vector2(MathF.Cos(40 * MathF.PI / 180), MathF.Sin(215 * MathF.PI / 180)) * power;
        initPos = new Vector2(destinationRectangle.X, destinationRectangle.Y);
        initVelocity *= DecideWhatinitVelocity(quadrantSpawned);
        fly = true;
    }

    private Vector2 DecideWhatinitVelocity(int quadrantSpawned)
    {
        Vector2 quadrantVec = Vector2.Zero;
        Console.WriteLine(quadrantSpawned);
        switch (quadrantSpawned)
        {
            case 1:
                quadrantVec = new Vector2(1, 1);
                break;
            case 2:
                quadrantVec = new Vector2(0, 1);
                break;
            case 3:
                quadrantVec = new Vector2(-1, 1);
                break;
            case 4:
                quadrantVec = new Vector2(0, -1);
                break;
        }

        return quadrantVec;
    }

    public new virtual void Update(GameTime gameTime)
    {
        if (fly)
        {
            time += (float)gameTime.ElapsedGameTime.TotalSeconds * 3;

            float newX = KinematicEquation(5f, initVelocity.X, initPos.X, time);
            float newY = KinematicEquation(100f, initVelocity.Y, initPos.Y, time);
        
            destinationRectangle.X = (int)newX;
            destinationRectangle.Y = (int)newY;   
        }

        if (destinationRectangle.Y >= initPos.Y + 46)
        {
            fly = false;
        }
    }
    
    private float KinematicEquation(float acceleration, float velocity, float position, float time)
    {
        return 0.5f * acceleration * time * time + velocity * time + position;
    }
}