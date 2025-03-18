﻿using System;
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
    private Texture2D colliderTexture;
    private float slimeShootTimer;
    public Inventory inventory;
    
    public PlayerOrientation playerOrientation;
    
    public Player(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle,
        float scaleMultiplier, Vector2 colliderSize, Texture2D colliderTexture, Texture2D slimeTexture) : 
        base(texture, destinationRectangle, sourceRectangle, scaleMultiplier, colliderSize, colliderTexture)
    {
        this.slimeTexture = slimeTexture;
        this.colliderTexture = colliderTexture;
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
        
        if (keyboardState.IsKeyDown(Keys.C))
            ShowCollider();
        if (keyboardState.IsKeyDown(Keys.D1))
            inventory.ChangeActiveSlot(1);
        else if (keyboardState.IsKeyDown(Keys.D2))
            inventory.ChangeActiveSlot(2);
        else if (keyboardState.IsKeyDown(Keys.D3))
            inventory.ChangeActiveSlot(3);
        else if (keyboardState.IsKeyDown(Keys.D4))
            inventory.ChangeActiveSlot(4);
            
    }
    

    private void SpawnSlime(List<Slime> slimeList, Vector2 spawnPos, Vector2 offset, Vector2 screenRes)
    {
        Slime slime = new Slime(slimeTexture,
            new Rectangle(destinationRectangle.X, destinationRectangle.Y, 20, 20),
            new Rectangle(0, 0, 20, 20), 2, new Vector2(20, 20), colliderTexture, 
            QuadrantClicked(spawnPos, screenRes));
        slime.SetupAnimator(6, 6, 1, new Vector2(20, 20), 1);
        slimeList.Add(slime);
    }

    public void CreateInventory(Vector2 screenRes, Texture2D itemsAtlas, Texture2D inventoryTex, 
        Dictionary<string, int> itemsID)
    {
        inventory = new(new Rectangle((int)(screenRes.X / 2 - 150f), (int)(screenRes.Y - 100f), 
                itemsAtlas.Width, itemsAtlas.Height),
            new Rectangle((int)(screenRes.X / 2 - 150f), (int)(screenRes.Y - 100f), itemsAtlas.Width, itemsAtlas.Height),
            inventoryTex, itemsAtlas, itemsID);
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