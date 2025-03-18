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
            new Rectangle(destinationRectangle.X + 51, destinationRectangle.Y + 327, forceFieldHorizontalTex.Width, 
                forceFieldHorizontalTex.Height / 6),
            new Rectangle(0, 0, forceFieldHorizontalTex.Width, forceFieldHorizontalTex.Height), 3, Vector2.Zero,
            colliderTexture, 6, 1, 6, new Vector2(forceFieldHorizontalTex.Width, 
                forceFieldHorizontalTex.Height / 6), 1f);
        forceFields.Add(horizontalForceFieldUp);
        horizontalForceFieldDown = new CorralForceField(forceFieldHorizontalTex,
            new Rectangle(destinationRectangle.X + 51, destinationRectangle.Y + 24, forceFieldHorizontalTex.Width,
                forceFieldHorizontalTex.Height / 6),
            new Rectangle(0, 0, forceFieldHorizontalTex.Width, forceFieldHorizontalTex.Height), 3, Vector2.Zero,
            colliderTexture, 6, 1, 6, new Vector2(forceFieldHorizontalTex.Width, forceFieldHorizontalTex.Height / 6), 1f);
        forceFields.Add(horizontalForceFieldDown);

        verticalForceFieldLeft = new CorralForceField(forceFieldVerticalTex,
            new Rectangle(destinationRectangle.X + 51, destinationRectangle.Y + 24, forceFieldVerticalTex.Width / 6,
                forceFieldVerticalTex.Height), new Rectangle(0, 0, forceFieldVerticalTex.Width, forceFieldVerticalTex.Height), 
            3, Vector2.Zero, colliderTexture, 6, 6, 1, 
            new Vector2(forceFieldVerticalTex.Width / 6, forceFieldVerticalTex.Height), 1f);
        forceFields.Add(verticalForceFieldLeft);
        verticalForceFieldRight = new CorralForceField(forceFieldVerticalTex,
            new Rectangle(destinationRectangle.X + 390, destinationRectangle.Y + 24, forceFieldVerticalTex.Width / 6,
                forceFieldVerticalTex.Height),
            new Rectangle(0, 0, forceFieldVerticalTex.Width, forceFieldVerticalTex.Height), 3, Vector2.Zero,
            colliderTexture, 6, 6, 1, new Vector2(forceFieldVerticalTex.Width / 6, forceFieldVerticalTex.Height), 1f);
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
    public CorralForceField(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle,
        float scaleMultiplier, Vector2 colliderSize, Texture2D colliderTexture, int numFrames, int numCollums, int numRows, Vector2 size, float animSpeedMultiplier) : 
        base(texture, destinationRectangle, sourceRectangle, scaleMultiplier, colliderSize, colliderTexture, numFrames, 
            numCollums, numRows, size, animSpeedMultiplier)
    {
        
    }
}