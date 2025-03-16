using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slime_Shooter_New_Horizons;

public class Animator : Sprite
{
    public int numFrames = 1;
    public int numCollums = 1;
    public Vector2 size;
    public float counter;
    private float animSpeedMultiplier = 1;
    
    public int currentFrame;
    public int colPos;
    
    
    public Animator(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, float scaleMultiplier,
        Vector2 colliderSize, int numFrames, int numCollums, Vector2 size, float animSpeedMultiplier) :
        base(texture, destinationRectangle, sourceRectangle, scaleMultiplier, colliderSize)
    {
        this.numFrames = numFrames;
        this.numCollums = numCollums;
        this.size = size;
        this.animSpeedMultiplier = animSpeedMultiplier;
    }
    
    public Animator(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, float scaleMultiplier,
        Vector2 colliderSize) : 
        base(texture, destinationRectangle, sourceRectangle, scaleMultiplier, colliderSize)
    {
        size = new Vector2(sourceRectangle.Width, sourceRectangle.Height);
    }

    public void SetupAnimator(int numFrames, int numCollums, Vector2 size, float animSpeedMultiplier)
    {
        this.numFrames = numFrames;
        this.numCollums = numCollums;
        this.size = size;
        this.animSpeedMultiplier = animSpeedMultiplier;
    }

    public new void Update(GameTime gameTime)
    {
        UpdateAnimator(gameTime);
    }

    public void UpdateAnimator(GameTime gameTime)
    {
        counter += animSpeedMultiplier;
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