using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slime_Shooter_New_Horizons;

public class Animator : Sprite
{
    public int numFrames;
    public int numCollums;
    public int numRows;
    public Vector2 size;
    public float counter;
    private float animSpeedMultiplier = 1;
    
    public int currentFrame;
    public int colPos;
    public int currentRow;
    
    
    public Animator(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, float scaleMultiplier,
        Vector2 colliderSize, Texture2D colliderTexture, int numFrames, int numCollums, int numRows, Vector2 size, float animSpeedMultiplier) :
        base(texture, destinationRectangle, sourceRectangle, scaleMultiplier, colliderSize, colliderTexture)
    {
        this.numFrames = numFrames;
        this.numCollums = numCollums;
        this.numRows = numRows;
        this.size = size;
        this.animSpeedMultiplier = animSpeedMultiplier;
    }
    
    public Animator(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, float scaleMultiplier,
        Vector2 colliderSize, Texture2D colliderTexture, int numFrames, int numCollums, int numRows, Vector2 size) :
        base(texture, destinationRectangle, sourceRectangle, scaleMultiplier, colliderSize, colliderTexture)
    {
        this.numFrames = numFrames;
        this.numCollums = numCollums;
        this.numRows = numRows;
        this.size = size;
    }
    
    public Animator(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, float scaleMultiplier,
        Vector2 colliderSize, Texture2D colliderTexture) : 
        base(texture, destinationRectangle, sourceRectangle, scaleMultiplier, colliderSize, colliderTexture)
    {
        size = new Vector2(sourceRectangle.Width, sourceRectangle.Height);
    }
    
    public Animator(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, float scaleMultiplier) : 
        base(texture, destinationRectangle, sourceRectangle, scaleMultiplier)
    {
        size = new Vector2(sourceRectangle.Width, sourceRectangle.Height);
    }

    public void SetupAnimator(int numFrames, int numCollums, int numRows, Vector2 size, float animSpeedMultiplier = 1)
    {
        this.numFrames = numFrames;
        this.numCollums = numCollums;
        this.numRows = numRows;
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
        
        spriteBatch.Draw(texture, dest, GetFrame(currentRow), Color.White);
    }

    public void NextFrame()
    {
        currentFrame++;
        colPos++;
        
        if (colPos >= numCollums)
        {
            ResetAnim();
        }
    }

    public void ResetAnim()
    {
        currentFrame = 0;
        colPos = 0;
        currentRow = 0;
    }

    public Rectangle GetFrame(int actualRow)
    {
        return new Rectangle(colPos * (int)size.X, actualRow * (int)size.Y, (int)size.X, (int)size.Y);
    }

    public bool CheckForCollisionsWithSlimes(List<Slime> slimeList)
    {
        foreach (var slime in slimeList)
        {
            if (this != slime) continue;
            if(destinationRectangle.Intersects(slime.destinationRectangle))
            {
                return true;
            }
        }

        return false;
    }
    
    public bool CheckForCollisionsWithPlayer(Rectangle playerRec)
    {
        if(destinationRectangle.Intersects(playerRec))
        {
            return true;
        }
        
        return false;    
    }
}