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

    private Texture2D slimeTexture;
    private float slimeShootTimer;
    
    public PlayerOrientation playerOrientation;
    
    public Player(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, Vector2 colliderSize,
        float scaleMultiplier, Texture2D slimeTexture) : 
        base(texture, destinationRectangle, sourceRectangle, colliderSize, scaleMultiplier)
    {
        this.slimeTexture = slimeTexture;
    }
    
    
    
    public virtual void Update(GameTime gameTime, List<Slime> slimeList, Vector2 offset, Vector2 screenRes)
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

        if (slimeShootTimer <= 0 && Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            slimeShootTimer = 0.25f;
            Vector2 mousePos = Mouse.GetState().Position.ToVector2();
            SpawnSlime(slimeList, mousePos, offset, screenRes);
        }
        slimeShootTimer -= gameTime.ElapsedGameTime.Milliseconds * 0.001f;
    }
    

    private void SpawnSlime(List<Slime> slimeList, Vector2 spawnPos, Vector2 offset, Vector2 screenRes)
    {
        Slime slime = new Slime(slimeTexture,
            new Rectangle(destinationRectangle.X, destinationRectangle.Y, 20, 20),
            new Rectangle(0, 0, 20, 20), 
            new Vector2(20, 20), 
            2, 6, 6, new Vector2(20, 20), QuadrantClicked(spawnPos, screenRes));
        slimeList.Add(slime);
    }
    
    private int QuadrantClicked(Vector2 clickPos, Vector2 screenRes)
    {
        int quadrant = 0;
        float angle = MathF.Atan(-(clickPos.Y - screenRes.Y / 2) / (clickPos.X - screenRes.X / 2)) / MathF.PI * 180;
        float relativeX = clickPos.X - screenRes.X / 2;
        if (relativeX > 0)
        {
            if (angle is >= -90 and <= -45)
                quadrant = 4;
            else if (angle is >= -44 and <= 45)
                quadrant = 1;
            else if (angle is >= 46 and <= 90)
                quadrant = 2;
        }
        else if (relativeX < 0)
        {
            if (angle is >= -90 and <= -45)
                quadrant = 2;
            else if (angle is >= -44 and <= 45)
                quadrant = 3;
            else if (angle is >= 46 and <= 90)
                quadrant = 4;
        }
        return quadrant;
    }
}