﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slime_Shooter_New_Horizons;

public class Animator : Sprite
{
    public int numFrames;
    public int numCollums;
    public Vector2 size;
    public int counter;
    
    public int currentFrame;
    public int colPos;
    
    public Animator(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Vector2 colliderSize,
        float scaleMultiplier, int numFrames, int numCollums, Vector2 size) : 
        base(texture, destinationRectangle, sourceRectangle, colliderSize, scaleMultiplier)
    {
        this.numFrames = numFrames;
        this.numCollums = numCollums;
        this.size = size;
    }

    public new virtual void Update(GameTime gameTime)
    {
        UpdateAnimator(gameTime);
    }

    public void UpdateAnimator(GameTime gameTime)
    {
        counter++;
        if (counter >= 8)
        {
            counter = 0;
            NextFrame();
        }
    }

    public new virtual void Draw(SpriteBatch spriteBatch, Vector2 offset)
    {
        Rectangle dest = new Rectangle(
            (int)offset.X + destinationRectangle.X,
            (int)offset.Y + destinationRectangle.Y,
            (int)(destinationRectangle.Width * scaleMultiplier),
            (int)(destinationRectangle.Height * scaleMultiplier));
        
        spriteBatch.Draw(texture, dest, GetFrame(0), Color.White);
    }

    public void NextFrame()
    {
        currentFrame++;
        colPos++;
        
        if (currentFrame >= numFrames)
        {
            ResetAnim();
        }

        if (colPos >= numCollums)
        {
            colPos = 0;
        }
    }

    public void ResetAnim()
    {
        currentFrame = 0;
        colPos = 0;
    }

    public Rectangle GetFrame(int actualRow)
    {
        return new Rectangle(colPos * (int)size.X, actualRow * (int)size.Y, (int)size.X, (int)size.Y);
    }
}