using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class Sprite : Physics
{
    public Texture2D texture;
    public Rectangle destinationRectangle, sourceRectangle, colliderRectangle;
    public float scaleMultiplier;

    public List<Slime> slimesList;
    public List<Plort> plortsList;
    public List<FruitVeggie> fruitsVeggiesList;

    private Vector2 colliderSize;
    private Texture2D colliderTexture;
    private bool colliderVisible = false;
    
    public bool isThrowed = false;
    public bool IsVacuumed = false;

    public Sprite(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Rectangle colliderRectangle, float scaleMultiplier)
    {
        this.texture = texture;
        this.destinationRectangle = destinationRectangle;
        this.sourceRectangle = sourceRectangle;
        this.colliderRectangle = colliderRectangle;
        this.scaleMultiplier = scaleMultiplier;
    }

    public Sprite(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, float scaleMultiplier)
    {
        this.texture = texture;
        this.destinationRectangle = destinationRectangle;
        this.sourceRectangle = sourceRectangle;
        this.scaleMultiplier = scaleMultiplier;
    }
    
    public void SetLists(List<Slime> slimesList, List<Plort> plortsList, List<FruitVeggie> fruitVeggiesList)
    {
        this.slimesList = slimesList;
        this.plortsList = plortsList;
        this.fruitsVeggiesList = fruitVeggiesList;
    }
    
    public void ShowCollider()
    {
        colliderVisible = true;
    }

    public new void Update(GameTime gameTime, Rectangle playerRec)
    {
        UpdateSprite(gameTime, playerRec);
    }

    public void UpdateSprite(GameTime gameTime, Rectangle playerRec)
    {
        if (IsVacuumed)
        {
            destinationRectangle = Vacuum(playerRec, destinationRectangle, gameTime);
        }
        else if (isThrowed)
        {
            destinationRectangle = Fly(gameTime, destinationRectangle);
            
            if (initQuadrant == 1 | initQuadrant == 3 && destinationRectangle.Y >= initPos.Y + 46)
            {
                isThrowed = false;
            }
            else if ((initQuadrant == 4 || initQuadrant == 2) &&
                     (destinationRectangle.Y >= initPos.Y + 146 || destinationRectangle.Y <= initPos.Y - 146))
            {
                isThrowed = false;
            }
        }
    }

    public virtual void Draw(SpriteBatch spriteBatch, Vector2 offset)
    {
        Rectangle dest = new Rectangle(
            (int)offset.X + destinationRectangle.X,
            (int)offset.Y + destinationRectangle.Y,
            destinationRectangle.Width * (int)scaleMultiplier,
            destinationRectangle.Height * (int)scaleMultiplier);
        
        spriteBatch.Draw(texture, dest, sourceRectangle, Color.White);
    }

    public void DrawCollisionRec(SpriteBatch spriteBatch, Texture2D greenDot, Vector2 offset, Rectangle collisionRec)
    {
        int lineThicknes = 2;
        Rectangle upperLine = new Rectangle(
            collisionRec.X + (int)offset.X, 
            collisionRec.Y + (int)offset.Y, 
            collisionRec.Width, 
            lineThicknes);
        Rectangle bottomLine = new Rectangle(
            (int)(collisionRec.X + offset.X), 
            (int)(collisionRec.Y + collisionRec.Height - lineThicknes + offset.Y), 
            collisionRec.Width, lineThicknes);
        Rectangle leftLine = new(
            collisionRec.X + (int)offset.X, 
            collisionRec.Y + (int)offset.Y,
            lineThicknes, 
            collisionRec.Height);
        Rectangle rightLine = new(
            (int)(collisionRec.X + collisionRec.Width - lineThicknes + offset.X), 
            collisionRec.Y + (int)offset.Y,
            lineThicknes, 
            collisionRec.Height);
        spriteBatch.Draw(greenDot, upperLine, Color.Green);
        spriteBatch.Draw(greenDot, bottomLine, Color.Green);
        spriteBatch.Draw(greenDot, leftLine, Color.Green);
        spriteBatch.Draw(greenDot, rightLine, Color.Green);
    }
    
    public new Rectangle GetCollisionRectangle()
    {
        return new Rectangle(destinationRectangle.X, destinationRectangle.Y, 
            destinationRectangle.Width * (int)scaleMultiplier, destinationRectangle.Height * (int)scaleMultiplier);
    }
}