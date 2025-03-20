using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class Sprite : Physics
{
    public Texture2D texture;
    public Rectangle destinationRectangle, sourceRectangle;
    public float scaleMultiplier;

    private Vector2 colliderSize;
    private Texture2D colliderTexture;
    private bool colliderVisible = false;

    public Sprite(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, float scaleMultiplier, 
        Vector2 colliderSize, Texture2D colliderTexture)
    {
        this.texture = texture;
        this.destinationRectangle = destinationRectangle;
        this.sourceRectangle = sourceRectangle;
        this.scaleMultiplier = scaleMultiplier;
        this.colliderSize = colliderSize;
        this.colliderTexture = colliderTexture;
    }

    public Sprite(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, float scaleMultiplier)
    {
        this.texture = texture;
        this.destinationRectangle = destinationRectangle;
        this.sourceRectangle = sourceRectangle;
        this.scaleMultiplier = scaleMultiplier;
    }
    
    public void ShowCollider()
    {
        colliderVisible = true;
    }

    public new void Update(GameTime gameTime)
    {
    }

    public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset)
    {
        Rectangle dest = new Rectangle(
            (int)offset.X + destinationRectangle.X,
            (int)offset.Y + destinationRectangle.Y,
            destinationRectangle.Width * (int)scaleMultiplier,
            destinationRectangle.Height * (int)scaleMultiplier);
        
        spriteBatch.Draw(texture, dest, sourceRectangle, Color.White);

        /**if (colliderVisible)
        {
            spriteBatch.Draw(colliderTexture, destinationRectangle, 
                new Rectangle(0, 0, colliderTexture.Width, colliderTexture.Height), Color.White);
        }**/
    }
    
    public Rectangle GetCollisionRectangle
    {
        get
        {
            return new Rectangle(destinationRectangle.X + destinationRectangle.Width / 2, 
                destinationRectangle.Y + destinationRectangle.Height / 2, (int)colliderSize.X, (int)colliderSize.Y);
        }
    }
}