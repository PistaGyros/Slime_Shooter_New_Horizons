using System;
using Microsoft.Xna.Framework;


namespace Slime_Shooter_New_Horizons;

public class Force
{
    public bool fly;
    public float time;
    public Vector2 initVelocity;
    public Vector2 initPos;
    public int initQuadrant;
    public float power = 200;
    public float gravityAcceleration;
    
    public Force()
    {
        
    }

    public void Throw(Rectangle destinationRectangle)
    {
        initVelocity = new Vector2(MathF.Cos(40 * MathF.PI / 180), MathF.Sin(215 * MathF.PI / 180)) * power;
        initPos = new Vector2(destinationRectangle.X, destinationRectangle.Y);
        fly = true;
    }
    
    public Rectangle Fly(GameTime gameTime, Rectangle destinationRectangle)
    {
        time += (float)gameTime.ElapsedGameTime.TotalSeconds * 3;

        float newX = KinematicEquation(5f, initVelocity.X, initPos.X, time);
        float newY = KinematicEquation(gravityAcceleration, initVelocity.Y, initPos.Y, time);
        
        destinationRectangle.X = (int)newX;
        destinationRectangle.Y = (int)newY;
        return new Rectangle(destinationRectangle.X, destinationRectangle.Y, destinationRectangle.Width, destinationRectangle.Height);
    }
    
    private float KinematicEquation(float acceleration, float velocity, float position, float time)
    {
        return 0.5f * acceleration * time * time + velocity * time + position;
    }
}