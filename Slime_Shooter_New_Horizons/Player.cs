using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public enum PlayerOrientation
{
    Down,
    Right,
    Left,
    Up
}

public class Player : Sprite
{
    private float defaultSpeed = 0.3f;
    
    public PlayerOrientation playerOrientation;
    
    public Player(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Vector2 colliderSize,
        float scaleMultiplier) : 
        base(texture, destinationRectangle, sourceRectangle, colliderSize, scaleMultiplier)
    {
    }
    
    
    
    public virtual void Update(GameTime gameTime)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        int changeY = 0;
        if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
        {
            playerOrientation = PlayerOrientation.Up;
            changeY -= (int)(defaultSpeed * gameTime.ElapsedGameTime.Milliseconds);
        }
        if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
        {
            playerOrientation = PlayerOrientation.Down;
            changeY += (int)(defaultSpeed * gameTime.ElapsedGameTime.Milliseconds);
        }
        destinationRectangle.Y += changeY;
        

        int changeX = 0;
        if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
        {
            playerOrientation = PlayerOrientation.Left;
            changeX -= (int)(defaultSpeed * gameTime.ElapsedGameTime.Milliseconds);
        }
        if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
        {
            playerOrientation = PlayerOrientation.Right;
            changeX += (int) (defaultSpeed * gameTime.ElapsedGameTime.Milliseconds);
        }
        destinationRectangle.X += changeX;
    }

    /**public new virtual void Draw(SpriteBatch spriteBatch, Vector2 offset)
    {
        Rectangle dest = new Rectangle(
            (int)offset.X + destinationRectangle.X,
            (int)offset.Y + destinationRectangle.Y,
            destinationRectangle.Width,
            destinationRectangle.Height);
        
        spriteBatch.Draw(texture, dest, sourceRectangle, Color.White);
    }**/
}