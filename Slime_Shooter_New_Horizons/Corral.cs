using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slime_Shooter_New_Horizons;

public class Corral : Building
{
    public Rectangle destinationRectangle;
    public Rectangle sourceRectangle;

    private Texture2D collarTex;
    private Texture2D forceFieldHorizontalTex;
    private Texture2D forceFieldVerticalTex;
    private Texture2D colliderTexture;
    private CorralForceField horizontalForceFieldUp;
    private CorralForceField horizontalForceFieldDown;
    private CorralForceField verticalForceFieldLeft;
    private CorralForceField verticalForceFieldRight;
    public List<CorralForceField> forceFields = new();
    public List<Rectangle> ForceFieldRectangles = new();
    
    public Corral(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle,
        float scaleMultiplier,Texture2D colliderTexture, Texture2D forceFieldHorizontalTex, Texture2D forceFieldVerticalTex) : 
        base (texture, destinationRectangle, sourceRectangle, scaleMultiplier)
    {
        this.forceFieldHorizontalTex = forceFieldHorizontalTex;
        this.forceFieldVerticalTex = forceFieldVerticalTex;
        this.colliderTexture = colliderTexture;
        CreateForceFields();
    }

    private void CreateForceFields()
    {
        horizontalForceFieldUp = new CorralForceField(forceFieldHorizontalTex,
            new Rectangle((int)(destinationRectangle.X + 17 * scaleMultiplier),
                (int)(destinationRectangle.Y + 8 * scaleMultiplier), forceFieldHorizontalTex.Width / 4,
                forceFieldHorizontalTex.Height),
            new Rectangle(0, 0, forceFieldHorizontalTex.Width, forceFieldHorizontalTex.Height), 3,
            4, 4, new Vector2((float)forceFieldHorizontalTex.Width / 4, forceFieldHorizontalTex.Height),
            0.5f);
        forceFields.Add(horizontalForceFieldUp);
        horizontalForceFieldDown = new CorralForceField(forceFieldHorizontalTex,
            new Rectangle((int)(destinationRectangle.X + 17 * scaleMultiplier),
                (int)(destinationRectangle.Y + 103 * scaleMultiplier), forceFieldHorizontalTex.Width / 4, 
                forceFieldHorizontalTex.Height),
            new Rectangle(0, 0, forceFieldHorizontalTex.Width, forceFieldHorizontalTex.Height), 3,
            4, 4, new Vector2((float)forceFieldHorizontalTex.Width / 4, forceFieldHorizontalTex.Height), 
            0.5f);
        forceFields.Add(horizontalForceFieldDown);
        
        
        ForceFieldRectangles.Add(horizontalForceFieldUp.destinationRectangle);
        ForceFieldRectangles.Add(horizontalForceFieldDown.destinationRectangle);
        

        verticalForceFieldLeft = new CorralForceField(forceFieldVerticalTex,
            new Rectangle((int)(destinationRectangle.X + 17 * scaleMultiplier),
                (int)(destinationRectangle.Y + 8 * scaleMultiplier),
                forceFieldVerticalTex.Width / 4,
                forceFieldVerticalTex.Height), 
            new Rectangle(0, 0, forceFieldVerticalTex.Width, forceFieldVerticalTex.Height), 
            3, 4, 4, 
            new Vector2((float)forceFieldVerticalTex.Width / 4, forceFieldVerticalTex.Height), 0.5f);
        forceFields.Add(verticalForceFieldLeft);
        verticalForceFieldRight = new CorralForceField(forceFieldVerticalTex,
            new Rectangle((int)(destinationRectangle.X + 134 * scaleMultiplier),
                (int)(destinationRectangle.Y + 8 * scaleMultiplier), forceFieldVerticalTex.Width / 4,
                forceFieldVerticalTex.Height),
            new Rectangle(0, 0, forceFieldVerticalTex.Width, forceFieldVerticalTex.Height), 
            3, 4, 4, 
            new Vector2((float)forceFieldVerticalTex.Width / 4, forceFieldVerticalTex.Height), 0.5f);
        forceFields.Add(verticalForceFieldRight);
        
        ForceFieldRectangles.Add(verticalForceFieldLeft.destinationRectangle);
        ForceFieldRectangles.Add(verticalForceFieldRight.destinationRectangle);
    }

    public new void Update(GameTime gameTime)
    {
        horizontalForceFieldUp.Update(gameTime);
        horizontalForceFieldDown.Update(gameTime);
        verticalForceFieldLeft.Update(gameTime);
        verticalForceFieldRight.Update(gameTime);
    }
}

public class CorralForceField : Animator
{
    public CorralForceField(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle,
        float scaleMultiplier, int numFrames, int numCollums, Vector2 size, float animSpeedMultiplier) : 
        base(texture, destinationRectangle, sourceRectangle, scaleMultiplier, numFrames, 
            numCollums, size, animSpeedMultiplier)
    {
        
    }

    public CorralForceField(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle,
        float scaleMultiplier, int numFrames, int numCollums, Vector2 size) :
        base(texture, destinationRectangle, sourceRectangle, scaleMultiplier, numFrames, numCollums, size)
    {
        
    }

    public void CreateCollisionRecs()
    {
        
    }
}