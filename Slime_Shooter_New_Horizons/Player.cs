using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Mime;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Slime_Shooter_New_Horizons;

public class Player : Animator
{
    public bool IsRightButtonPressed;
    public bool canMove = true;
    public int coins = 500;
    private double staminaMax = 100;
    public double Stamina = 100;
    private double healthMax = 100;
    public double Health = 100;

    private bool isWalking;
    private float defaultSpeed = 0.3f;
    private float sprintSpeed;
    private double sprintDelay;
    private int lastScrollWheel;
    public float slimeShootTimer;
    
    private List<List<List<Rectangle>>> objectsColRecs;

    public Texture2D playerTexture;
    public List<Texture2D> objectsTextures;
    public List<PlotBuilding> plots;
    
    public Inventory inventory;
    public StatusBars StatusBarsUi;
    
    public PlayerOrientation playerOrientation;
    public Rectangle interactRec;
    
    public Player(Texture2D texture, Rectangle destinationRectangle, Rectangle sourceRectangle,
        float scaleMultiplier, List<List<List<Rectangle>>> objectsColRecs) : 
        base(texture, destinationRectangle, sourceRectangle, scaleMultiplier)
    {
        this.objectsColRecs = objectsColRecs;
    }
    
    public new virtual void Update(GameTime gameTime, Vector2 screenRes)
    {
        KeyboardState keyboardState = Keyboard.GetState();
        if (canMove)
        {
            int changeY = 0;
            if (keyboardState.IsKeyDown(Keys.W) || keyboardState.IsKeyDown(Keys.Up))
            {
                playerOrientation = PlayerOrientation.Up;
                changeY -= (int)(defaultSpeed * sprintSpeed * gameTime.ElapsedGameTime.Milliseconds);
                isWalking = true;
            }
            else if (keyboardState.IsKeyDown(Keys.S) || keyboardState.IsKeyDown(Keys.Down))
            {
                playerOrientation = PlayerOrientation.Down;
                changeY += (int)(defaultSpeed * sprintSpeed * gameTime.ElapsedGameTime.Milliseconds);
                isWalking = true;
            }
            else
                isWalking = false;
            destinationRectangle.Y += changeY;


            int changeX = 0;
            if (keyboardState.IsKeyDown(Keys.A) || keyboardState.IsKeyDown(Keys.Left))
            {
                playerOrientation = PlayerOrientation.Left;
                changeX -= (int)(defaultSpeed * sprintSpeed * gameTime.ElapsedGameTime.Milliseconds);
                isWalking = true;
            }
            else if (keyboardState.IsKeyDown(Keys.D) || keyboardState.IsKeyDown(Keys.Right))
            {
                playerOrientation = PlayerOrientation.Right;
                changeX += (int)(defaultSpeed * sprintSpeed * gameTime.ElapsedGameTime.Milliseconds);
                isWalking = true;
            }
            destinationRectangle.X += changeX;
            Sprint(gameTime, keyboardState);
            
            // VACUUM
            if (Mouse.GetState().RightButton == ButtonState.Pressed)
            {
                Vacuum(gameTime, screenRes);
            }
            else if (Mouse.GetState().RightButton == ButtonState.Released)
            {
                StopVacuum(screenRes);
            }

            if (Mouse.GetState().LeftButton == ButtonState.Pressed)
            {
                Vector2 mousePos = Mouse.GetState().Position.ToVector2();
                Rectangle clickRec = new Rectangle((int)mousePos.X, (int)mousePos.Y, 5, 5);
                bool clickedOnSlot = false;
                int clickedSlot = 0;
                for (int i = 0; i < inventory.InventorySlotsSize; i++)
                {
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
                    slimeShootTimer = 0.5f;
                    Shoot(mousePos, screenRes);
                }
            }
            
            if (keyboardState.IsKeyDown(Keys.C))
                ShowCollider();
        
            if (Mouse.GetState().ScrollWheelValue > lastScrollWheel)
            {
                if (inventory.activeSlot != 0)
                {
                    inventory.ChangeActiveSlot(inventory.activeSlot + 1 - 1);
                }
                
            }
            else if (Mouse.GetState().ScrollWheelValue < lastScrollWheel)
                if (inventory.activeSlot != 3)
                {
                    inventory.ChangeActiveSlot(inventory.activeSlot + 1 + 1);
                }
                
            
            if (keyboardState.IsKeyDown(Keys.D1))
                inventory.ChangeActiveSlot(1);
            else if (keyboardState.IsKeyDown(Keys.D2))
                inventory.ChangeActiveSlot(2);
            else if (keyboardState.IsKeyDown(Keys.D3))
                inventory.ChangeActiveSlot(3);
            else if (keyboardState.IsKeyDown(Keys.D4))
                inventory.ChangeActiveSlot(4);

            lastScrollWheel = Mouse.GetState().ScrollWheelValue;
        }
        
        slimeShootTimer -= (float)gameTime.ElapsedGameTime.TotalSeconds;

        interactRec = CreateInteractiveRectangle();
        
        StatusBarsUi.Update(gameTime, coins, (int)Stamina, (int)Health);
    }

    private void Sprint(GameTime gameTime, KeyboardState keyboardState)
    {
        if (isWalking && Stamina > 0 && keyboardState.IsKeyDown(Keys.LeftShift))
        {
            sprintSpeed = 1.5f;
            Stamina -= (double)gameTime.ElapsedGameTime.Milliseconds / 50;
        }
        else
        {
            sprintDelay += (double)gameTime.ElapsedGameTime.Milliseconds / 1000;
            if (sprintDelay > 3 && Stamina < 100)
                Stamina += (double)gameTime.ElapsedGameTime.Milliseconds / 25;
            else if (Stamina >= staminaMax)
            {
                sprintDelay = 0;
                Stamina = staminaMax;
            }
            sprintSpeed = 1;
        }
    }

    private Rectangle CreateInteractiveRectangle()
    {
        Rectangle rect = new Rectangle(0, 0, 
            (int)((float)GetCollisionRectangle().Width * 2 * scaleMultiplier),
            (int)((float)GetCollisionRectangle().Height / 2 * scaleMultiplier));
        switch (playerOrientation)
        {
            case PlayerOrientation.Right:
                rect.X = GetCollisionRectangle().X + GetCollisionRectangle().Width;
                rect.Y = (int)(GetCollisionRectangle().Y + (float)GetCollisionRectangle().Height / 3);
                break;
            case PlayerOrientation.Up:
                rect.X = (int)(GetCollisionRectangle().X + (float)GetCollisionRectangle().X / 3);
                rect.Y = GetCollisionRectangle().Y + rect.Height;
                break;
            case PlayerOrientation.Left:
                rect.X = GetCollisionRectangle().X - rect.Width;
                rect.Y = (int)(GetCollisionRectangle().Y + (float)GetCollisionRectangle().Height / 3);
                break;
            case PlayerOrientation.Down:
                rect.X = (int)(GetCollisionRectangle().X + (float)GetCollisionRectangle().X / 3);
                rect.Y = GetCollisionRectangle().Y + GetCollisionRectangle().Height;
                break;
        }
        return rect;
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

    private void Vacuum(GameTime gameTime, Vector2 screenRes)
    {
        List<Slime> vacuumedSlimeList = new List<Slime>();
        List<Plort> vacuumedPlortList = new List<Plort>();
        List<FruitVeggie> vacuumedFruitVeggieList = new List<FruitVeggie>();
        Vector2 mousePos = Mouse.GetState().Position.ToVector2();
        List<Rectangle> vacuumConeRecs = CreateVacuumConeRecs(mousePos, screenRes);
        
        // Check for vacuum collisions with slimes and/or plorts, fruits and veggies
        foreach (var vacuumCone in vacuumConeRecs)
        {
            foreach (var slime in slimesList)
            {
                if (slime.GetCollisionRectangle().Intersects(vacuumCone))
                {
                    slime.Vacuum(gameTime);
                    int availableSlot = inventory.WhichSlotIsAvailable(slime.slimeID);
                    if (GetCollisionRectangle().Intersects(slime.GetCollisionRectangle()) && availableSlot != 69)
                    {
                        Console.WriteLine("Slime was vacuumed");
                        inventory.UpdateInventory(availableSlot, slime.slimeID, 1);
                        vacuumedSlimeList.Add(slime);
                    }
                }
            }

            foreach (var plort in plortsList)
            {
                if (plort.GetCollisionRectangle().Intersects(vacuumCone))
                {
                    plort.vacuumTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    plort.IsVacuumed = true;
                    int availableSlot = inventory.WhichSlotIsAvailable(plort.plortID);
                    if (GetCollisionRectangle().Intersects(plort.GetCollisionRectangle()) && availableSlot != 69)
                    {
                        Console.WriteLine("Plort is vacuumed");
                        inventory.UpdateInventory(availableSlot, plort.plortID, 1);
                        vacuumedPlortList.Add(plort);
                    }
                }
            }

            foreach (var fruitVeggie in fruitsVeggiesList)
            {
                if (fruitVeggie.GetCollisionRectangle().Intersects(vacuumCone))
                {
                    fruitVeggie.vacuumTime += (float)gameTime.ElapsedGameTime.TotalSeconds;
                    fruitVeggie.IsVacuumed = true;
                    int availableSlot = inventory.WhichSlotIsAvailable(fruitVeggie.fruitVeggieID);
                    if (GetCollisionRectangle().Intersects(fruitVeggie.GetCollisionRectangle()) && availableSlot != 69)
                    {
                        Console.WriteLine("Fruit or veggie is vacuumed");
                        inventory.UpdateInventory(availableSlot, fruitVeggie.fruitVeggieID, 1);
                        vacuumedFruitVeggieList.Add(fruitVeggie);
                    }
                }
            }
            
            if (vacuumedSlimeList != null)
                foreach (var vacuumedSlime in vacuumedSlimeList)
                {
                    slimesList.Remove(vacuumedSlime);
                }
            if (vacuumedPlortList != null)
                foreach (var vacuumedPlort in vacuumedPlortList)
                {
                    plortsList.Remove(vacuumedPlort);
                }

            if (vacuumedFruitVeggieList != null)
            {
                foreach (var fruitVeggie in vacuumedFruitVeggieList)
                {
                    fruitsVeggiesList.Remove(fruitVeggie);
                }
            }
        }
    }

    private void StopVacuum(Vector2 screenRes)
    {
        if (slimesList != null)
        {
            Vector2 mousePos = Mouse.GetState().Position.ToVector2();
            List<Rectangle> vacuumConeRecs = CreateVacuumConeRecs(mousePos, screenRes);
            // Check for vacuum collisions with slimes and/or plorts, fruits and veggies
            foreach (var vacuumCone in vacuumConeRecs)
            {
                foreach (var slime in slimesList)
                {
                    if (!slime.GetCollisionRectangle().Intersects(vacuumCone) || slime.isCollidingWithFence)
                    {
                        slime.vacuumTime = 0;
                        slime.IsVacuumed = false;
                    }
                }

                foreach (var plort in plortsList)
                {
                    if (!plort.GetCollisionRectangle().Intersects(vacuumCone))
                    {
                        plort.vacuumTime = 0;
                        plort.IsVacuumed = false;
                    }
                }

                foreach (var fruitVeggie in fruitsVeggiesList)
                {
                    if (!fruitVeggie.GetCollisionRectangle().Intersects(vacuumCone))
                    {
                        fruitVeggie.vacuumTime = 0;
                        fruitVeggie.IsVacuumed = false;
                    }
                }
            }
        }
    }

    private void Shoot(Vector2 mousePos, 
        Vector2 screenRes)
    {
        int activeSlot = inventory.activeSlot;
        if (inventory.inventorySlots[activeSlot][1] >= 1)
        {
            // Shoot an item from active slot
            int objectID = inventory.inventorySlots[activeSlot][0];
            DecideWhatTypeOfObjectToShoot(objectID, mousePos, screenRes);
            inventory.UpdateInventory(activeSlot, inventory.inventorySlots[activeSlot][0], -1);
            
            // Check if the slot is not empty now
            if (inventory.inventorySlots[activeSlot][1] <= 0)
            {
                inventory.UpdateInventory(activeSlot, inventory.inventorySlots[activeSlot][0] = 0, 
                    inventory.inventorySlots[activeSlot][1] = 0);
            }
        }
    }

    private void DecideWhatTypeOfObjectToShoot(int objectID, Vector2 mousePos, Vector2 screenRes)
    {
        if (objectID > 0 && objectID < 10)
            SpawnSlime(objectsTextures[objectID], mousePos, screenRes, objectID);
        else if (objectID > 9 && objectID < 21)
            SpawnPlort(objectsTextures[objectID], mousePos, screenRes, objectID);
        else if (objectID > 20 && objectID < 31)
            SpawnFruitOrVeggie(objectsTextures[objectID], mousePos, screenRes, objectID);
    }
    

    private void SpawnSlime(Texture2D slimeTex, Vector2 spawnPos, Vector2 screenRes, int slimeID)
    {
        Slime slime = new Slime(slimeID, slimeTex,
            new Rectangle(destinationRectangle.X, destinationRectangle.Y, 22, 22),
            new Rectangle(0, 0, 22, 22), objectsColRecs[slimeID], 3);
        slime.plortsTexs = objectsTextures;
        slime.scaleMultiplier = 3;
        slime.SetLists(slimesList, plortsList, fruitsVeggiesList);
        slime.SetupAnimator(6, 6, new Vector2(22, 22), 0.9f);
        slime.ThrowSlime(QuadrantClicked(spawnPos, screenRes));
        slime.plotsList = plots;
        slimesList.Add(slime);
    }

    private void SpawnPlort(Texture2D plortTex, Vector2 spawnPos, Vector2 screenRes, int plortID)
    {
        Plort plort = new Plort(plortID, plortTex, 
            new Rectangle(destinationRectangle.X, destinationRectangle.Y, plortTex.Width, plortTex.Height),
            new Rectangle(0, 0, plortTex.Width, plortTex.Height), 2);
        plort.ThrowPlort(QuadrantClicked(spawnPos, screenRes));
        plort.SetLists(slimesList, plortsList, fruitsVeggiesList);
        plortsList.Add(plort);
    }

    private void SpawnFruitOrVeggie(Texture2D fruitVeggieTex, Vector2 spawnPos, 
        Vector2 screenRes, int objectID)
    {
        FruitVeggie fruitVeggie = new FruitVeggie(objectID, fruitVeggieTex, 
            new Rectangle(destinationRectangle.X, destinationRectangle.Y, fruitVeggieTex.Width, fruitVeggieTex.Height),
            new Rectangle(0, 0, fruitVeggieTex.Width, fruitVeggieTex.Height), 2);
        fruitVeggie.ThrowFruitVeggie(QuadrantClicked(spawnPos, screenRes));
        fruitVeggie.SetLists(slimesList, plortsList, fruitsVeggiesList);
        fruitsVeggiesList.Add(fruitVeggie);
    }
    

    public void CreateInventory(Vector2 screenRes, Texture2D itemsAtlas, Texture2D inventoryTex, SpriteFont font,
        Dictionary<int, string> itemsNames, List<Rectangle> itemsIDTexturesRec)
    {
        inventory = new(new Rectangle((int)(screenRes.X / 2 - 150f), (int)(screenRes.Y - 100f), 
                itemsAtlas.Width, itemsAtlas.Height),
            new Rectangle((int)(screenRes.X / 2 - 150f), (int)(screenRes.Y - 100f), itemsAtlas.Width, itemsAtlas.Height),
            inventoryTex, itemsAtlas, font, itemsNames, itemsIDTexturesRec);
    }

    public void CreateStatusBar(Vector2 screenRes, Texture2D bgTexture, Texture2D fgTexture, Texture2D coinTex,
        SpriteFont font)
    {
        Vector2 statusBarPos = new Vector2(screenRes.X - 300, screenRes.Y - 100f);
        StatusBarsUi = new StatusBars(statusBarPos, bgTexture, fgTexture, font, coinTex);
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

    private new bool CheckForCollisionsWithSlimes()
    {
        bool collision = false;
        foreach (var slime in slimesList)
        {
            if(destinationRectangle.Intersects(slime.GetCollisionRectangle()))
            {
                collision = true;
            }
        }

        return collision;
    }
    
    // TODO: In future remove this, when added animation spritesheet for player character
    public new Rectangle GetCollisionRectangle()
    {
        return new Rectangle(destinationRectangle.X * (int)scaleMultiplier, destinationRectangle.Y * (int)scaleMultiplier, 
            destinationRectangle.Width * (int)scaleMultiplier, destinationRectangle.Height * (int)scaleMultiplier);
    }
}