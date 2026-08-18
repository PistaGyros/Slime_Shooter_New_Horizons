using System;
using Microsoft.Xna.Framework;


namespace Slime_Shooter_New_Horizons;

public class Physics
{
    private float time;
    public double vacuumTime;
    public Vector2 velocity;
    public Vector2 initPos;
    public int initQuadrant;
    public float power = 100;
    public float gravityAcceleration;

    public void Throw(Rectangle destinationRectangle)
    {
        velocity = new Vector2(MathF.Cos(40 * MathF.PI / 180), MathF.Sin(215 * MathF.PI / 180)) * power;
        initPos = new Vector2(destinationRectangle.X, destinationRectangle.Y);
    }

    public Rectangle Vacuum( GameTime gameTime, Rectangle vacuumerRec, Rectangle vacuumedRec, Rectangle vacuumedColRec)
    {
        Vector2 pointVec = new Vector2(
            (vacuumerRec.X + ((float)vacuumerRec.Width / 2)) - (vacuumedColRec.X + (float)vacuumedColRec.Width / 2), 
            (vacuumerRec.Y + ((float)vacuumerRec.Height / 2)) - (vacuumedColRec.Y + (float)vacuumedColRec.Height / 2)
            );
        var x = pointVec.X * vacuumTime * gameTime.ElapsedGameTime.TotalSeconds;
        var y = pointVec.Y * vacuumTime * gameTime.ElapsedGameTime.TotalSeconds;
        vacuumedRec.X += (int)x;
        vacuumedRec.Y += (int)y;
        return vacuumedRec;
    }
    
    
    public Rectangle Fly(GameTime gameTime, Rectangle destinationRectangle)
    {
        time += (float)gameTime.ElapsedGameTime.TotalSeconds * 300;

        float newX = KinematicEquation(5f, velocity.X, initPos.X, time * (float)gameTime.ElapsedGameTime.TotalSeconds);
        float newY = KinematicEquation(gravityAcceleration, velocity.Y, initPos.Y, time * (float)gameTime.ElapsedGameTime.TotalSeconds);
        
        destinationRectangle.X = (int)newX;
        destinationRectangle.Y = (int)newY;
        return new Rectangle(destinationRectangle.X, destinationRectangle.Y, destinationRectangle.Width, destinationRectangle.Height);
    }
    
    public Vector2 DecideWhatinitQuadrant(int quadrantSpawned)
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
    
    private float KinematicEquation(float acceleration, float velocity, float position, float time)
    {
        return 0.5f * acceleration * time * time + velocity * time + position;
    }
}