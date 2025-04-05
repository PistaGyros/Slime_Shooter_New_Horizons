using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Slime_Shooter_New_Horizons;

public class Corral : Building
{
    //public Rectangle destinationRectangle;
    //public Rectangle sourceRectangle;

    private Texture2D collarTex;
    private Texture2D forceFieldHorizontalTex;
    private Texture2D forceFieldVerticalTex;
    private CorralForceField horizontalForceFieldUp;
    private CorralForceField horizontalForceFieldDown;
    private CorralForceField verticalForceFieldLeft;
    private CorralForceField verticalForceFieldRight;
    public List<CorralForceField> forceFields = new();
    
    public Corral(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle,
        float scaleMultiplier,Texture2D colliderTexture, Texture2D forceFieldHorizontalTex, Texture2D forceFieldVerticalTex) : 
        base (texture, destinationRectangle, sourceRectangle, scaleMultiplier)
    {
        this.forceFieldHorizontalTex = forceFieldHorizontalTex;
        this.forceFieldVerticalTex = forceFieldVerticalTex;
        CreateForceFields();
    }
    
    public Corral(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle,
        float scaleMultiplier, Texture2D forceFieldHorizontalTex, Texture2D forceFieldVerticalTex) : 
        base (texture, destinationRectangle, sourceRectangle, scaleMultiplier)
    {
        this.forceFieldHorizontalTex = forceFieldHorizontalTex;
        this.forceFieldVerticalTex = forceFieldVerticalTex;
        CreateForceFields();
    }

    private void CreateForceFields()
    {
        // HORIZONTAL FORCE FIELDS (OR FENCE)
        horizontalForceFieldUp = new CorralForceField(ForceFieldTypes.Horizontal, forceFieldHorizontalTex,
            new Rectangle((int)(destinationRectangle.X + 17 * scaleMultiplier),
                (int)(destinationRectangle.Y + 8 * scaleMultiplier), forceFieldHorizontalTex.Width / 4,
                forceFieldHorizontalTex.Height),
            new Rectangle(0, 0, forceFieldHorizontalTex.Width, forceFieldHorizontalTex.Height), 3,
            4, 4, new Vector2((float)forceFieldHorizontalTex.Width / 4, forceFieldHorizontalTex.Height),
            0.5f);
        forceFields.Add(horizontalForceFieldUp);
        horizontalForceFieldDown = new CorralForceField(ForceFieldTypes.Horizontal, forceFieldHorizontalTex,
            new Rectangle((int)(destinationRectangle.X + 17 * scaleMultiplier),
                (int)(destinationRectangle.Y + 103 * scaleMultiplier), forceFieldHorizontalTex.Width / 4, 
                forceFieldHorizontalTex.Height),
            new Rectangle(0, 0, forceFieldHorizontalTex.Width, forceFieldHorizontalTex.Height), 3,
            4, 4, new Vector2((float)forceFieldHorizontalTex.Width / 4, forceFieldHorizontalTex.Height), 
            0.5f);
        forceFields.Add(horizontalForceFieldDown);
        
        // VERTICAL FORCE FIELDS (OR FENCE)
        verticalForceFieldLeft = new CorralForceField(ForceFieldTypes.Vertical, forceFieldVerticalTex,
            new Rectangle((int)(destinationRectangle.X + 17 * scaleMultiplier),
                (int)(destinationRectangle.Y + 8 * scaleMultiplier),
                forceFieldVerticalTex.Width / 4,
                forceFieldVerticalTex.Height), 
            new Rectangle(0, 0, forceFieldVerticalTex.Width, forceFieldVerticalTex.Height), 
            3, 4, 4, 
            new Vector2((float)forceFieldVerticalTex.Width / 4, forceFieldVerticalTex.Height), 0.5f);
        forceFields.Add(verticalForceFieldLeft);
        verticalForceFieldRight = new CorralForceField(ForceFieldTypes.Vertical, forceFieldVerticalTex,
            new Rectangle((int)(destinationRectangle.X + 134 * scaleMultiplier),
                (int)(destinationRectangle.Y + 8 * scaleMultiplier), forceFieldVerticalTex.Width / 4,
                forceFieldVerticalTex.Height),
            new Rectangle(0, 0, forceFieldVerticalTex.Width, forceFieldVerticalTex.Height), 
            3, 4, 4, 
            new Vector2((float)forceFieldVerticalTex.Width / 4, forceFieldVerticalTex.Height), 0.5f);
        forceFields.Add(verticalForceFieldRight);
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
    private Rectangle destRec;
    private ForceFieldTypes fenceType;
    
    public CorralForceField(ForceFieldTypes fenceType, Texture2D texture, Rectangle destinationRectangle, 
        Rectangle sourceRectangle, float scaleMultiplier, int numFrames, int numCollums, Vector2 size, 
        float animSpeedMultiplier) : 
        base(texture, destinationRectangle, sourceRectangle, scaleMultiplier, numFrames, 
            numCollums, size, animSpeedMultiplier)
    {
        this.fenceType = fenceType;
    }

    public CorralForceField(ForceFieldTypes fenceType, Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle,
        float scaleMultiplier, int numFrames, int numCollums, Vector2 size) :
        base(texture, destinationRectangle, sourceRectangle, scaleMultiplier, numFrames, numCollums, size)
    {
        this.fenceType = fenceType;
    }

    public new Rectangle GetCollisionRectangle()
    {
        Rectangle collisionRec = new();
        switch (fenceType)
        {
            case ForceFieldTypes.Vertical:
                collisionRec = new Rectangle(
                    destinationRectangle.X, 
                    destinationRectangle.Y, 
                    (int)(destinationRectangle.Width * scaleMultiplier), 
                    (int)(destinationRectangle.Height * scaleMultiplier));
                break;
            case ForceFieldTypes.Horizontal:
                collisionRec = new Rectangle(
                    destinationRectangle.X, 
                    destinationRectangle.Y + (int)(destinationRectangle.Height * scaleMultiplier - 3 * scaleMultiplier), 
                    (int)(destinationRectangle.Width * scaleMultiplier), 
                    (int)(3 * scaleMultiplier));
                break;
        }
        
        return collisionRec;
    }
}