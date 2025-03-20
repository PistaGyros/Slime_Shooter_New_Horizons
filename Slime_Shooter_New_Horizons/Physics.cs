using System;
using Microsoft.Xna.Framework;


namespace Slime_Shooter_New_Horizons;

public class Physics
{
    private float time;
    public float vacuumTime;
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

    public Rectangle Vacuum(Rectangle vacuumerRec, Rectangle vacuumedRec, GameTime gameTime)
    {
        Vector2 pointVec = new Vector2(vacuumerRec.X - vacuumedRec.X, vacuumerRec.Y - vacuumedRec.Y);
        float x = vacuumedRec.X + pointVec.X * vacuumTime * (float)gameTime.ElapsedGameTime.TotalSeconds;
        float y = vacuumedRec.Y + pointVec.Y * vacuumTime * (float)gameTime.ElapsedGameTime.TotalSeconds;
        vacuumedRec.X = (int)x;
        vacuumedRec.Y = (int)y;
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
    
    private float KinematicEquation(float acceleration, float velocity, float position, float time)
    {
        return 0.5f * acceleration * time * time + velocity * time + position;
    }
}