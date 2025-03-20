using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class Player : Sprite
{
    public bool IsRightButtonPressed;
        
    private float defaultSpeed = 0.3f;

    private Texture2D colliderTexture;

    public Texture2D playerTexture;
    public List<Texture2D> slimeTextures;
    
    private float slimeShootTimer;
    public Inventory inventory;
    
    public PlayerOrientation playerOrientation;
    
    public Player(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle, float scaleMultiplier, 
        Vector2 colliderSize, Texture2D colliderTexture) : 
        base(texture, destinationRectangle, sourceRectangle, scaleMultiplier, colliderSize, colliderTexture)
    {
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
            changeX += (int)(defaultSpeed * gameTime.ElapsedGameTime.Milliseconds);
        }
        destinationRectangle.X += changeX;

        if (Mouse.GetState().RightButton == ButtonState.Pressed)
        {
            List<Slime> vacuumedSlimeList = new List<Slime>();
            Vector2 mousePos = Mouse.GetState().Position.ToVector2();
            List<Rectangle> vacuumConeRecs = CreateVacuumConeRecs(mousePos, screenRes);
            // Check for vacuum collisions with slimes
            foreach (var vacuumCone in vacuumConeRecs)
            {
                foreach (var slime in slimeList)
                {
                    if (slime.destinationRectangle.Intersects(vacuumCone))
                    {
                        slime.vacuumTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                        slime.IsVacuumed = true;
                        int availableSlot = inventory.WhichSlotIsAvailable(slime.slimeID);
                        if (destinationRectangle.Intersects(slime.destinationRectangle) && availableSlot != 69)
                        {
                            Console.WriteLine("Slime is vacuumed");
                            inventory.UpdateInventory(availableSlot, slime.slimeID, 1);
                            vacuumedSlimeList.Add(slime);
                        }
                    }
                }
                if (vacuumedSlimeList != null)
                    foreach (var vacuumedSlime in vacuumedSlimeList)
                    {
                        slimeList.Remove(vacuumedSlime);
                    }
            }
        }
        else if (Mouse.GetState().RightButton == ButtonState.Released)
        {
            Vector2 mousePos = Mouse.GetState().Position.ToVector2();
            List<Rectangle> vacuumConeRecs = CreateVacuumConeRecs(mousePos, screenRes);
            // Check for vacuum collisions with slimes
            foreach (var vacuumCone in vacuumConeRecs)
            {
                foreach (var slime in slimeList)
                {
                    if (slime.destinationRectangle.Intersects(vacuumCone))
                    {
                        slime.vacuumTime = 0;
                        slime.IsVacuumed = false;
                    }
                }
            }
        }

        if (Mouse.GetState().LeftButton == ButtonState.Pressed)
        {
            Vector2 mousePos = Mouse.GetState().Position.ToVector2();
            Rectangle clickRec = new Rectangle((int)mousePos.X, (int)mousePos.Y, 5, 5);
            bool clickedOnSlot = false;
            int clickedSlot = 0;
            for (int i = 0; i < inventory.inventorySlotsSize; i++)
            {
                Console.WriteLine(inventory.slotsRectangles[i]);
                if (inventory.slotsRectangles[i].Contains(clickRec))
                {
                    clickedOnSlot = true;
                    clickedSlot = i;
                    break;
                }
            }
            if (clickedOnSlot)
                inventory.ChangeActiveSlot(clickedSlot + 1);
            else if (slimeShootTimer <= 0 && Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                slimeShootTimer = 0.25f;
                Shoot(slimeList, mousePos, screenRes);
            }
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

    private List<Rectangle> CreateVacuumConeRecs(Vector2 mousePos, Vector2 screenRes)
    {
        List<Rectangle> vacuumConeRecs = new List<Rectangle>();
        int quadrantClicked = QuadrantClicked(mousePos, screenRes);
        switch (quadrantClicked)
        {
            case 1:
                for(int i = 0; i < 6; i++)
                {
                    vacuumConeRecs.Add(new Rectangle(
                        (int)(destinationRectangle.X + destinationRectangle.Width + 25 * i), 
                        (int)(destinationRectangle.Y - 10 * i),
                        25, 
                        destinationRectangle.Height + 10 * i * 2));
                }
                break;
            case 3:
                for(int i = 0; i < 6; i++)
                {
                    vacuumConeRecs.Add(new Rectangle(
                        (int)destinationRectangle.X - 25 * i, (int)destinationRectangle.Y - 10 * i,
                        25, destinationRectangle.Height + 10 * i * 2));
                }
                break;
            case 2:
                for(int i = 0; i < 6; i++)
                {
                    vacuumConeRecs.Add(new Rectangle(
                        (int)(destinationRectangle.X - 10 * i), (int)(destinationRectangle.Y - 25 * i),
                        destinationRectangle.Width + 10 * i * 2, 25));
                }
                break;
            case 4:
                for(int i = 0; i < 6; i++)
                {
                    vacuumConeRecs.Add(new Rectangle(
                        (int)(destinationRectangle.X - 10 * i), (int)destinationRectangle.Y + destinationRectangle.Height + 25 * i,
                        destinationRectangle.Width + 10 * i * 2, 25));
                }
                break;
        }
        return vacuumConeRecs;
    }

    private void Shoot(List<Slime> slimes, Vector2 mousePos, Vector2 screenRes)
    {
        int activeSlot = inventory.activeSlot;
        if (inventory.inventorySlots[activeSlot][1] >= 1)
        {
            // Shoot an item from active slot
            int slimeID = inventory.inventorySlots[activeSlot][0];
            SpawnSlime(slimeTextures[slimeID], slimes, mousePos, screenRes, slimeID);
            inventory.UpdateInventory(activeSlot, inventory.inventorySlots[activeSlot][0], -1);
            
            // Check if the slot is not empty now
            if (inventory.inventorySlots[activeSlot][1] <= 0)
            {
                inventory.UpdateInventory(activeSlot, inventory.inventorySlots[activeSlot][0] = 0, 
                    inventory.inventorySlots[activeSlot][1] = 0);
            }
        }
    }
    

    private void SpawnSlime(Texture2D slimeTex, List<Slime> slimeList, Vector2 spawnPos, Vector2 screenRes, int slimeID)
    {
        Slime slime = new Slime(slimeTex,
            new Rectangle(destinationRectangle.X, destinationRectangle.Y, 22, 22),
            new Rectangle(0, 0, 22, 22), 2, new Vector2(22, 22), colliderTexture);
        slime.slimeID = slimeID;
        slime.SetupAnimator(6, 6, 1, new Vector2(22, 22));
        slime.ThrowSlime(QuadrantClicked(spawnPos, screenRes));
        slimeList.Add(slime);
    }
    

    public void CreateInventory(Vector2 screenRes, Texture2D itemsAtlas, Texture2D inventoryTex, SpriteFont font,
        Dictionary<string, int> itemsID)
    {
        inventory = new(new Rectangle((int)(screenRes.X / 2 - 150f), (int)(screenRes.Y - 100f), 
                itemsAtlas.Width, itemsAtlas.Height),
            new Rectangle((int)(screenRes.X / 2 - 150f), (int)(screenRes.Y - 100f), itemsAtlas.Width, itemsAtlas.Height),
            inventoryTex, itemsAtlas, font, itemsID);
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